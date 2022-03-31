pragma solidity 0.8.1;

interface IERC20 {
    /**
     * @dev Returns the amount of tokens in existence.
     */
    function totalSupply() external view returns (uint256);

    /**
     * @dev Returns the amount of tokens owned by `account`.
     */
    function balanceOf(address account) external view returns (uint256);

    /**
     * @dev Moves `amount` tokens from the caller's account to `recipient`.
     *
     * Returns a boolean value indicating whether the operation succeeded.
     *
     * Emits a {Transfer} event.
     */
    function transfer(address recipient, uint256 amount) external returns (bool);

    /**
     * @dev Returns the remaining number of tokens that `spender` will be
     * allowed to spend on behalf of `owner` through {transferFrom}. This is
     * zero by default.
     *
     * This value changes when {approve} or {transferFrom} are called.
     */
    function allowance(address owner, address spender) external view returns (uint256);

    /**
     * @dev Sets `amount` as the allowance of `spender` over the caller's tokens.
     *
     * Returns a boolean value indicating whether the operation succeeded.
     *
     * IMPORTANT: Beware that changing an allowance with this method brings the risk
     * that someone may use both the old and the new allowance by unfortunate
     * transaction ordering. One possible solution to mitigate this race
     * condition is to first reduce the spender's allowance to 0 and set the
     * desired value afterwards:
     * https://github.com/ethereum/EIPs/issues/20#issuecomment-263524729
     *
     * Emits an {Approval} event.
     */
    function approve(address spender, uint256 amount) external returns (bool);

    /**
     * @dev Moves `amount` tokens from `sender` to `recipient` using the
     * allowance mechanism. `amount` is then deducted from the caller's
     * allowance.
     *
     * Returns a boolean value indicating whether the operation succeeded.
     *
     * Emits a {Transfer} event.
     */
    function transferFrom(
        address sender,
        address recipient,
        uint256 amount
    ) external returns (bool);

    /**
     * @dev Emitted when `value` tokens are moved from one account (`from`) to
     * another (`to`).
     *
     * Note that `value` may be zero.
     */
    event Transfer(address indexed from, address indexed to, uint256 value);

    /**
     * @dev Emitted when the allowance of a `spender` for an `owner` is set by
     * a call to {approve}. `value` is the new allowance.
     */
    event Approval(address indexed owner, address indexed spender, uint256 value);
}

interface IWEAPONStakable is IERC20 {
    function stakedBalanceOf(address account) external view returns (uint256);
    function getStake(address account) external view returns (uint256, uint256, uint256);

    function stake(address account, uint256 amount, uint256 unstakeTime, bool isPlayer, uint256 adjustedStake) external;
    function unstake(address account, uint256 unstakeAmount, bool isPlayer, uint256 adjustedStake) external;
    function sync(address account, uint256 adjustedStake) external;
    function toggleStaking() external;
}

/**
 * @dev Provides information about the current execution context, including the
 * sender of the transaction and its data. While these are generally available
 * via msg.sender and msg.data, they should not be accessed in such a direct
 * manner, since when dealing with meta-transactions the account sending and
 * paying for execution may not be the actual sender (as far as an application
 * is concerned).
 *
 * This contract is only required for intermediate, library-like contracts.
 */
abstract contract Context {
    function _msgSender() internal view virtual returns (address) {
        return msg.sender;
    }

    function _msgData() internal view virtual returns (bytes calldata) {
        return msg.data;
    }
}

contract MWStaking is Context {

    string private constant _name = "MEGAWEAPON Staking";
    string private constant _version = "1";

    mapping (uint => uint) private _epochStart;
    mapping (uint => uint) private _epochPoolSize;
    mapping (uint => uint) private _epochRewards;
    uint public currentEpoch;

    mapping (address => bool) private _isPlayer;
    bool public playersAllowed;

    mapping (address => mapping(uint => bool)) private _hasClaimedWeek;

    address payable immutable private _multiSig;
    address payable immutable private _weaponAddress;
    address payable private _playerContract;
    address private immutable _epochRobot;

    IWEAPONStakable private _weapon;

    constructor(address weaponAddress, address multiSig, address epochRobot) {
        require (weaponAddress != address(0) && multiSig != address(0) && epochRobot != address(0), "cannot be zero address");
        _multiSig = payable(multiSig);
        _weaponAddress = payable(weaponAddress);
        _epochRobot = epochRobot;
        _weapon = IWEAPONStakable(weaponAddress);

        _epochStart[0] = 1647216001;
    }

    modifier onlyMultiSig {
        require (_msgSender() == _multiSig, "unauthorized");
        _;
    }

    function name() external pure returns (string memory) {
        return _name;
    }

    function version() external pure returns (string memory) {
        return _version;
    }

    function stake(address account, uint256 amount, uint256 unstakeTime, uint256 adjustedStake) external {
        _checkEpoch();
        if (_isPlayer[account]) {
            //require players to use player contract
            require (_msgSender() == _playerContract, "must stake through player contract");
            //handle off-chain delegation from player contract
            _weapon.stake(account, amount, unstakeTime, true, adjustedStake);
        }
        else {
            require(account == _msgSender(), "must be account owner");
            _weapon.stake(account, amount, unstakeTime, false, 0);
        }
    }

    function unstake(address account, uint256 unstakeAmount, uint256 adjustedStake) external {
        _checkEpoch();
        if (_isPlayer[account]) {
            //require players to use player contract
            require (_msgSender() == _playerContract, "must unstake through player contract");
            //handle off-chain delegation from player contract
            _weapon.unstake(account, unstakeAmount, true, adjustedStake);
        }
        else {
            require (account == _msgSender(), "must be account owner");
            _weapon.unstake(account, unstakeAmount, false, 0);
        }
    }

    function sync(address account, uint256 adjustedStake) external {
        _checkEpoch();
        require (_isPlayer[account], "not a player");
        //require players to use player contract
        require (_msgSender() == _playerContract, "must sync through player contract");
        _weapon.sync(account, adjustedStake);
    }

    function claim(uint256[] calldata epochs) external {
        require (!_isPlayer[_msgSender()], "cannot be player");
        require (epochs.length <= 20, "max 20 epochs");
        _checkEpoch();
        uint8 i = 0;
        uint256 max = epochs.length;
        uint256 weiToPay = 0;
        (uint256 _stakedBalance, uint256 _stakeBeginTime, uint256 _stakeEndTime) = _weapon.getStake(_msgSender());
        require (_stakedBalance > 0, "cannot claim without staked balance");
        while (i < max) {
            require (currentEpoch > epochs[i], "epoch not closed");
            require (_stakeBeginTime <= _epochStart[epochs[i]] + 86400  && _stakeEndTime >= _epochStart[epochs[i] +1], "not eligible for this epoch");
            require (!_hasClaimedWeek[_msgSender()][epochs[i]], "already claimed");

            uint256 share = (_stakedBalance * 1000000) / _epochPoolSize[epochs[i]];
            weiToPay += (share * _epochRewards[epochs[i]]) / 1000000;
            _hasClaimedWeek[_msgSender()][epochs[i]] = true;
            i++;
        }

        //watch for reentrancy here since we're using call
        _transferETH(weiToPay, payable(_msgSender()));
    }

    function claimPlayer(address account, uint256[] calldata epochs, uint256[] calldata amounts) external {
        require (_isPlayer[account], "not a player");
        //require players to use player contract
        require (_msgSender() == _playerContract, "must unstake through player contract");
        require (epochs.length <= 20 && amounts.length == epochs.length, "epoch mismatch");
        _checkEpoch();
        uint8 i = 0;
        uint256 max = epochs.length;
        uint256 weiToPay = 0;
        while (i < max)
        {
            require (currentEpoch > epochs[i], "epoch not closed");
            require (!_hasClaimedWeek[account][epochs[i]], "already claimed");
            weiToPay += amounts[i];
            _hasClaimedWeek[account][epochs[i]] = true;
            i++;
        }

        //watch for reentrancy here since we're using call
        _transferETH(weiToPay, payable(account));
    }

    function hasClaimed(address account, uint256 epoch) external view returns (bool) {
        return _hasClaimedWeek[account][epoch];
    }

    function becomePlayer() external {
        require(playersAllowed, "gameplay not enabled");
        _isPlayer[_msgSender()] = true;
    }

    function isPlayer(address addr) external view returns (bool) {
        return _isPlayer[addr];
    }

    function togglePlayersAllowed() external onlyMultiSig {
        if (playersAllowed) playersAllowed = false;
        else playersAllowed = true;
    }

    function setPlayerContract(address pContract) external onlyMultiSig {
        _playerContract = payable(pContract);
    }

    function getPlayerContract() external view returns (address) {
        return _playerContract;
    }

    function addEpochs(uint256 firstAdd, uint256 numToAdd) external onlyMultiSig{
        require (numToAdd <= 20, "limit 20 epochs");
        require (_epochStart[firstAdd] == 0, "epoch already set");
        
        uint8 i = 0;
        while (i < numToAdd)
        {
            require (_epochStart[firstAdd + i - 1] != 0, "previous epoch not set");
            _epochStart[firstAdd + i] = _epochStart[firstAdd + i - 1] + 604800;
            i++;
        }
    }

    function setPoolSize(uint256 poolSize) external {
        require (_msgSender() == _epochRobot, "unauthorized");
        _epochPoolSize[currentEpoch] = poolSize;
    }

    function _checkEpoch() internal {
        if(block.timestamp >= _epochStart[currentEpoch + 1] && _epochStart[currentEpoch + 1] != 0) currentEpoch += 1;
    }

    function getEpoch(uint256 epoch) external view returns(uint256 epochStartDate, uint256 epochPool, uint256 epochEth) {
        return (_epochStart[epoch], _epochPoolSize[epoch], _epochRewards[epoch]);
    }

    function _transferETH(uint256 amount, address payable _to) private {
        (bool sent, /*bytes memory data*/) = _to.call{value: amount}("");
        require(sent, "Failed to send Ether");
    }
    
    function failsafeETHtransfer() external onlyMultiSig {
        (bool sent, /*bytes memory data*/) = _msgSender().call{value: address(this).balance}("");
        require(sent, "Failed to send Ether");
    }

    receive() external payable {
        _checkEpoch();
        _epochRewards[currentEpoch] += msg.value;
    }

    fallback() external payable {}
}
