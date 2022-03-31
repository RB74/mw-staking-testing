using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using MwStakingSol.Contracts.WEAPON.ContractDefinition;

namespace MwStakingSol.Contracts.WEAPON
{
    public partial class WEAPONService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, WEAPONDeployment wEAPONDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<WEAPONDeployment>().SendRequestAndWaitForReceiptAsync(wEAPONDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, WEAPONDeployment wEAPONDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<WEAPONDeployment>().SendRequestAsync(wEAPONDeployment);
        }

        public static async Task<WEAPONService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, WEAPONDeployment wEAPONDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, wEAPONDeployment, cancellationTokenSource);
            return new WEAPONService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public WEAPONService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        
        public Task<BigInteger> AllowanceQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowanceFunction = new AllowanceFunction();
                allowanceFunction.Owner = owner;
                allowanceFunction.Spender = spender;
            
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public Task<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(string spender, BigInteger amount)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        
        public Task<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public Task<string> FailsafeETHtransferRequestAsync(FailsafeETHtransferFunction failsafeETHtransferFunction)
        {
             return ContractHandler.SendRequestAsync(failsafeETHtransferFunction);
        }

        public Task<string> FailsafeETHtransferRequestAsync()
        {
             return ContractHandler.SendRequestAsync<FailsafeETHtransferFunction>();
        }

        public Task<TransactionReceipt> FailsafeETHtransferRequestAndWaitForReceiptAsync(FailsafeETHtransferFunction failsafeETHtransferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(failsafeETHtransferFunction, cancellationToken);
        }

        public Task<TransactionReceipt> FailsafeETHtransferRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<FailsafeETHtransferFunction>(null, cancellationToken);
        }

        public Task<string> FailsafeTokenSwapRequestAsync(FailsafeTokenSwapFunction failsafeTokenSwapFunction)
        {
             return ContractHandler.SendRequestAsync(failsafeTokenSwapFunction);
        }

        public Task<TransactionReceipt> FailsafeTokenSwapRequestAndWaitForReceiptAsync(FailsafeTokenSwapFunction failsafeTokenSwapFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(failsafeTokenSwapFunction, cancellationToken);
        }

        public Task<string> FailsafeTokenSwapRequestAsync(BigInteger amount)
        {
            var failsafeTokenSwapFunction = new FailsafeTokenSwapFunction();
                failsafeTokenSwapFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(failsafeTokenSwapFunction);
        }

        public Task<TransactionReceipt> FailsafeTokenSwapRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var failsafeTokenSwapFunction = new FailsafeTokenSwapFunction();
                failsafeTokenSwapFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(failsafeTokenSwapFunction, cancellationToken);
        }

        public Task<string> FreezeMintRequestAsync(FreezeMintFunction freezeMintFunction)
        {
             return ContractHandler.SendRequestAsync(freezeMintFunction);
        }

        public Task<TransactionReceipt> FreezeMintRequestAndWaitForReceiptAsync(FreezeMintFunction freezeMintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(freezeMintFunction, cancellationToken);
        }

        public Task<string> FreezeMintRequestAsync(BigInteger timestamp)
        {
            var freezeMintFunction = new FreezeMintFunction();
                freezeMintFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAsync(freezeMintFunction);
        }

        public Task<TransactionReceipt> FreezeMintRequestAndWaitForReceiptAsync(BigInteger timestamp, CancellationTokenSource cancellationToken = null)
        {
            var freezeMintFunction = new FreezeMintFunction();
                freezeMintFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(freezeMintFunction, cancellationToken);
        }

        public Task<GetStakeOutputDTO> GetStakeQueryAsync(GetStakeFunction getStakeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeFunction, GetStakeOutputDTO>(getStakeFunction, blockParameter);
        }

        public Task<GetStakeOutputDTO> GetStakeQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var getStakeFunction = new GetStakeFunction();
                getStakeFunction.Account = account;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetStakeFunction, GetStakeOutputDTO>(getStakeFunction, blockParameter);
        }

        public Task<string> GetStakingContractQueryAsync(GetStakingContractFunction getStakingContractFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetStakingContractFunction, string>(getStakingContractFunction, blockParameter);
        }

        
        public Task<string> GetStakingContractQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetStakingContractFunction, string>(null, blockParameter);
        }

        public Task<bool> IsPoolQueryAsync(IsPoolFunction isPoolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsPoolFunction, bool>(isPoolFunction, blockParameter);
        }

        
        public Task<bool> IsPoolQueryAsync(string addr, BlockParameter blockParameter = null)
        {
            var isPoolFunction = new IsPoolFunction();
                isPoolFunction.Addr = addr;
            
            return ContractHandler.QueryAsync<IsPoolFunction, bool>(isPoolFunction, blockParameter);
        }

        public Task<string> MintRequestAsync(MintFunction mintFunction)
        {
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> MintRequestAsync(BigInteger amount, string recipient)
        {
            var mintFunction = new MintFunction();
                mintFunction.Amount = amount;
                mintFunction.Recipient = recipient;
            
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(BigInteger amount, string recipient, CancellationTokenSource cancellationToken = null)
        {
            var mintFunction = new MintFunction();
                mintFunction.Amount = amount;
                mintFunction.Recipient = recipient;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<BigInteger> MintLockTimeQueryAsync(MintLockTimeFunction mintLockTimeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MintLockTimeFunction, BigInteger>(mintLockTimeFunction, blockParameter);
        }

        
        public Task<BigInteger> MintLockTimeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MintLockTimeFunction, BigInteger>(null, blockParameter);
        }

        public Task<bool> MintLockedQueryAsync(MintLockedFunction mintLockedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MintLockedFunction, bool>(mintLockedFunction, blockParameter);
        }

        
        public Task<bool> MintLockedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MintLockedFunction, bool>(null, blockParameter);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<string> SetBuyTaxRequestAsync(SetBuyTaxFunction setBuyTaxFunction)
        {
             return ContractHandler.SendRequestAsync(setBuyTaxFunction);
        }

        public Task<TransactionReceipt> SetBuyTaxRequestAndWaitForReceiptAsync(SetBuyTaxFunction setBuyTaxFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setBuyTaxFunction, cancellationToken);
        }

        public Task<string> SetBuyTaxRequestAsync(byte newTax)
        {
            var setBuyTaxFunction = new SetBuyTaxFunction();
                setBuyTaxFunction.NewTax = newTax;
            
             return ContractHandler.SendRequestAsync(setBuyTaxFunction);
        }

        public Task<TransactionReceipt> SetBuyTaxRequestAndWaitForReceiptAsync(byte newTax, CancellationTokenSource cancellationToken = null)
        {
            var setBuyTaxFunction = new SetBuyTaxFunction();
                setBuyTaxFunction.NewTax = newTax;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setBuyTaxFunction, cancellationToken);
        }

        public Task<string> SetPoolRequestAsync(SetPoolFunction setPoolFunction)
        {
             return ContractHandler.SendRequestAsync(setPoolFunction);
        }

        public Task<TransactionReceipt> SetPoolRequestAndWaitForReceiptAsync(SetPoolFunction setPoolFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPoolFunction, cancellationToken);
        }

        public Task<string> SetPoolRequestAsync(string addr)
        {
            var setPoolFunction = new SetPoolFunction();
                setPoolFunction.Addr = addr;
            
             return ContractHandler.SendRequestAsync(setPoolFunction);
        }

        public Task<TransactionReceipt> SetPoolRequestAndWaitForReceiptAsync(string addr, CancellationTokenSource cancellationToken = null)
        {
            var setPoolFunction = new SetPoolFunction();
                setPoolFunction.Addr = addr;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPoolFunction, cancellationToken);
        }

        public Task<string> SetRewardsRequestAsync(SetRewardsFunction setRewardsFunction)
        {
             return ContractHandler.SendRequestAsync(setRewardsFunction);
        }

        public Task<TransactionReceipt> SetRewardsRequestAndWaitForReceiptAsync(SetRewardsFunction setRewardsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRewardsFunction, cancellationToken);
        }

        public Task<string> SetRewardsRequestAsync(byte newRewards)
        {
            var setRewardsFunction = new SetRewardsFunction();
                setRewardsFunction.NewRewards = newRewards;
            
             return ContractHandler.SendRequestAsync(setRewardsFunction);
        }

        public Task<TransactionReceipt> SetRewardsRequestAndWaitForReceiptAsync(byte newRewards, CancellationTokenSource cancellationToken = null)
        {
            var setRewardsFunction = new SetRewardsFunction();
                setRewardsFunction.NewRewards = newRewards;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRewardsFunction, cancellationToken);
        }

        public Task<string> SetSellTaxRequestAsync(SetSellTaxFunction setSellTaxFunction)
        {
             return ContractHandler.SendRequestAsync(setSellTaxFunction);
        }

        public Task<TransactionReceipt> SetSellTaxRequestAndWaitForReceiptAsync(SetSellTaxFunction setSellTaxFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setSellTaxFunction, cancellationToken);
        }

        public Task<string> SetSellTaxRequestAsync(byte newTax)
        {
            var setSellTaxFunction = new SetSellTaxFunction();
                setSellTaxFunction.NewTax = newTax;
            
             return ContractHandler.SendRequestAsync(setSellTaxFunction);
        }

        public Task<TransactionReceipt> SetSellTaxRequestAndWaitForReceiptAsync(byte newTax, CancellationTokenSource cancellationToken = null)
        {
            var setSellTaxFunction = new SetSellTaxFunction();
                setSellTaxFunction.NewTax = newTax;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setSellTaxFunction, cancellationToken);
        }

        public Task<string> SetStakingContractRequestAsync(SetStakingContractFunction setStakingContractFunction)
        {
             return ContractHandler.SendRequestAsync(setStakingContractFunction);
        }

        public Task<TransactionReceipt> SetStakingContractRequestAndWaitForReceiptAsync(SetStakingContractFunction setStakingContractFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setStakingContractFunction, cancellationToken);
        }

        public Task<string> SetStakingContractRequestAsync(string addr)
        {
            var setStakingContractFunction = new SetStakingContractFunction();
                setStakingContractFunction.Addr = addr;
            
             return ContractHandler.SendRequestAsync(setStakingContractFunction);
        }

        public Task<TransactionReceipt> SetStakingContractRequestAndWaitForReceiptAsync(string addr, CancellationTokenSource cancellationToken = null)
        {
            var setStakingContractFunction = new SetStakingContractFunction();
                setStakingContractFunction.Addr = addr;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setStakingContractFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(StakeFunction stakeFunction)
        {
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(StakeFunction stakeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(string account, BigInteger amount, BigInteger unstakeTime, bool isPlayer, BigInteger adjustedStake)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Account = account;
                stakeFunction.Amount = amount;
                stakeFunction.UnstakeTime = unstakeTime;
                stakeFunction.IsPlayer = isPlayer;
                stakeFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(string account, BigInteger amount, BigInteger unstakeTime, bool isPlayer, BigInteger adjustedStake, CancellationTokenSource cancellationToken = null)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Account = account;
                stakeFunction.Amount = amount;
                stakeFunction.UnstakeTime = unstakeTime;
                stakeFunction.IsPlayer = isPlayer;
                stakeFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<BigInteger> StakedBalanceOfQueryAsync(StakedBalanceOfFunction stakedBalanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StakedBalanceOfFunction, BigInteger>(stakedBalanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> StakedBalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var stakedBalanceOfFunction = new StakedBalanceOfFunction();
                stakedBalanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<StakedBalanceOfFunction, BigInteger>(stakedBalanceOfFunction, blockParameter);
        }

        public Task<bool> StakingEnabledQueryAsync(StakingEnabledFunction stakingEnabledFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StakingEnabledFunction, bool>(stakingEnabledFunction, blockParameter);
        }

        
        public Task<bool> StakingEnabledQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StakingEnabledFunction, bool>(null, blockParameter);
        }

        public Task<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        
        public Task<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
        }

        public Task<string> SyncRequestAsync(SyncFunction syncFunction)
        {
             return ContractHandler.SendRequestAsync(syncFunction);
        }

        public Task<TransactionReceipt> SyncRequestAndWaitForReceiptAsync(SyncFunction syncFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(syncFunction, cancellationToken);
        }

        public Task<string> SyncRequestAsync(string account, BigInteger adjustedStake)
        {
            var syncFunction = new SyncFunction();
                syncFunction.Account = account;
                syncFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAsync(syncFunction);
        }

        public Task<TransactionReceipt> SyncRequestAndWaitForReceiptAsync(string account, BigInteger adjustedStake, CancellationTokenSource cancellationToken = null)
        {
            var syncFunction = new SyncFunction();
                syncFunction.Account = account;
                syncFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(syncFunction, cancellationToken);
        }

        public Task<string> ThawMintRequestAsync(ThawMintFunction thawMintFunction)
        {
             return ContractHandler.SendRequestAsync(thawMintFunction);
        }

        public Task<string> ThawMintRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ThawMintFunction>();
        }

        public Task<TransactionReceipt> ThawMintRequestAndWaitForReceiptAsync(ThawMintFunction thawMintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(thawMintFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ThawMintRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ThawMintFunction>(null, cancellationToken);
        }

        public Task<string> ToggleStakingRequestAsync(ToggleStakingFunction toggleStakingFunction)
        {
             return ContractHandler.SendRequestAsync(toggleStakingFunction);
        }

        public Task<string> ToggleStakingRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ToggleStakingFunction>();
        }

        public Task<TransactionReceipt> ToggleStakingRequestAndWaitForReceiptAsync(ToggleStakingFunction toggleStakingFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(toggleStakingFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ToggleStakingRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ToggleStakingFunction>(null, cancellationToken);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(string recipient, BigInteger amount)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(string sender, string recipient, BigInteger amount)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string sender, string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> UnstakeRequestAsync(UnstakeFunction unstakeFunction)
        {
             return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(UnstakeFunction unstakeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unstakeFunction, cancellationToken);
        }

        public Task<string> UnstakeRequestAsync(string account, BigInteger unstakeAmount, bool isPlayer, BigInteger adjustedStake)
        {
            var unstakeFunction = new UnstakeFunction();
                unstakeFunction.Account = account;
                unstakeFunction.UnstakeAmount = unstakeAmount;
                unstakeFunction.IsPlayer = isPlayer;
                unstakeFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(string account, BigInteger unstakeAmount, bool isPlayer, BigInteger adjustedStake, CancellationTokenSource cancellationToken = null)
        {
            var unstakeFunction = new UnstakeFunction();
                unstakeFunction.Account = account;
                unstakeFunction.UnstakeAmount = unstakeAmount;
                unstakeFunction.IsPlayer = isPlayer;
                unstakeFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unstakeFunction, cancellationToken);
        }

        public Task<string> VersionQueryAsync(VersionFunction versionFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VersionFunction, string>(versionFunction, blockParameter);
        }

        
        public Task<string> VersionQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VersionFunction, string>(null, blockParameter);
        }
    }
}
