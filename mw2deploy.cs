using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;


namespace MwStakingSol.Contracts
{
    public class RandomTrader
    {
        public Account account;
        public Web3 web3;
        public string name;
        public BigInteger tokens;
    }

    public class Signature
    {
        public string R { get; set; }
        public string S { get; set; }
        public byte V { get; set; }
    }

    class Program
    {
        static int debugLevel = 0;

        static Web3 deployerWeb3;
        static Web3 wallet1Web3;
        static Web3 wallet2Web3;
        static Web3 wallet3Web3;
        static Web3 wallet4Web3;
        static Web3 wallet5Web3;
        static Web3 wallet6Web3;
        static Web3 wallet7Web3;
        static Web3 vaultWeb3;
        static Web3 obcWeb3;

        static List<Web3> allWeb3;

        static Account deployerAccount;
        static Account wallet1Account;
        static Account wallet2Account;
        static Account wallet3Account;
        static Account wallet4Account;
        static Account wallet5Account;
        static Account wallet6Account;
        static Account wallet7Account;
        static Account vaultAccount;

        static List<Account> allAccounts;

        static string httpsEndpoint;

        static string deployerKey;
        static string wallet1Key;
        static string wallet2Key;
        static string wallet3Key;
        static string wallet4Key;
        static string wallet5Key;
        static string wallet6Key;
        static string wallet7Key;
        static string vaultKey;

        static BigInteger deployerETH;
        static BigInteger wallet1ETH;
        static BigInteger wallet2ETH;
        static BigInteger wallet3ETH;
        static BigInteger wallet4ETH;
        static BigInteger wallet5ETH;
        static BigInteger wallet6ETH;
        static BigInteger wallet7ETH;
        static BigInteger vaultETH;
        static BigInteger stakingETH;

        static BigInteger pairWETH;
        static BigInteger contractETH;

        static BigInteger deployerTOKEN;
        static BigInteger wallet1TOKEN;
        static BigInteger wallet2TOKEN;
        static BigInteger wallet3TOKEN;
        static BigInteger wallet4TOKEN;
        static BigInteger wallet5TOKEN;
        static BigInteger wallet6TOKEN;
        static BigInteger wallet7TOKEN;
        static BigInteger pairTOKEN;
        static BigInteger contractTOKEN;
        static BigInteger stakingTOKEN;

        static BigInteger wallet1Staked;
        static BigInteger wallet2Staked;
        static BigInteger wallet3Staked;
        static BigInteger wallet4Staked;
        static BigInteger wallet5Staked;
        static BigInteger wallet6Staked;
        static BigInteger wallet7Staked;

        static BigInteger wallet1StakeTime;
        static BigInteger wallet2StakeTime;
        static BigInteger wallet3StakeTime;
        static BigInteger wallet4StakeTime;
        static BigInteger wallet5StakeTime;
        static BigInteger wallet6StakeTime;
        static BigInteger wallet7StakeTime;

        static BigInteger walllet1UnstakeTime;
        static BigInteger walllet2UnstakeTime;
        static BigInteger walllet3UnstakeTime;
        static BigInteger walllet4UnstakeTime;
        static BigInteger walllet5UnstakeTime;
        static BigInteger walllet6UnstakeTime;
        static BigInteger walllet7UnstakeTime;

        static Random random = new Random();
        static List<string> log;

        static List<RandomTrader> randos;
        static BigInteger randoBuyVolume;
        static BigInteger randoSellVolume;
        static BigInteger totalETHvolume;
        static int txCount;
        static BigInteger currentBlockTimestamp;

        static string uniRouter = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";
        static string uniFactory = "0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f";
        static string weth = "0xc02aaa39b223fe8d0a0e5c4f27ead9083c756cc2";
        static string burn = "0x0000000000000000000000000000000000000000";

        //============SET DECIMALS OR IT ALL BREAKS
        static int decimals = 9;

        static string routerAbi = @"[{""inputs"":[{""internalType"":""address"",""name"":""_factory"",""type"":""address""},{""internalType"":""address"",""name"":""_WETH"",""type"":""address""}],""stateMutability"":""nonpayable"",""type"":""constructor""},{""inputs"":[],""name"":""WETH"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""},{""internalType"":""uint256"",""name"":""amountADesired"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBDesired"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountAMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""addLiquidity"",""outputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""amountTokenDesired"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""addLiquidityETH"",""outputs"":[{""internalType"":""uint256"",""name"":""amountToken"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""}],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[],""name"":""factory"",""outputs"":[{""internalType"":""address"",""name"":"""",""type"":""address""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveOut"",""type"":""uint256""}],""name"":""getAmountIn"",""outputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""}],""stateMutability"":""pure"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveOut"",""type"":""uint256""}],""name"":""getAmountOut"",""outputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""}],""stateMutability"":""pure"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""}],""name"":""getAmountsIn"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""}],""name"":""getAmountsOut"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""view"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""reserveB"",""type"":""uint256""}],""name"":""quote"",""outputs"":[{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""}],""stateMutability"":""pure"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountAMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""removeLiquidity"",""outputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""removeLiquidityETH"",""outputs"":[{""internalType"":""uint256"",""name"":""amountToken"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""removeLiquidityETHSupportingFeeOnTransferTokens"",""outputs"":[{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""},{""internalType"":""bool"",""name"":""approveMax"",""type"":""bool""},{""internalType"":""uint8"",""name"":""v"",""type"":""uint8""},{""internalType"":""bytes32"",""name"":""r"",""type"":""bytes32""},{""internalType"":""bytes32"",""name"":""s"",""type"":""bytes32""}],""name"":""removeLiquidityETHWithPermit"",""outputs"":[{""internalType"":""uint256"",""name"":""amountToken"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""token"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountTokenMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountETHMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""},{""internalType"":""bool"",""name"":""approveMax"",""type"":""bool""},{""internalType"":""uint8"",""name"":""v"",""type"":""uint8""},{""internalType"":""bytes32"",""name"":""r"",""type"":""bytes32""},{""internalType"":""bytes32"",""name"":""s"",""type"":""bytes32""}],""name"":""removeLiquidityETHWithPermitSupportingFeeOnTransferTokens"",""outputs"":[{""internalType"":""uint256"",""name"":""amountETH"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""address"",""name"":""tokenA"",""type"":""address""},{""internalType"":""address"",""name"":""tokenB"",""type"":""address""},{""internalType"":""uint256"",""name"":""liquidity"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountAMin"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountBMin"",""type"":""uint256""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""},{""internalType"":""bool"",""name"":""approveMax"",""type"":""bool""},{""internalType"":""uint8"",""name"":""v"",""type"":""uint8""},{""internalType"":""bytes32"",""name"":""r"",""type"":""bytes32""},{""internalType"":""bytes32"",""name"":""s"",""type"":""bytes32""}],""name"":""removeLiquidityWithPermit"",""outputs"":[{""internalType"":""uint256"",""name"":""amountA"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountB"",""type"":""uint256""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapETHForExactTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactETHForTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactETHForTokensSupportingFeeOnTransferTokens"",""outputs"":[],""stateMutability"":""payable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForETH"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForETHSupportingFeeOnTransferTokens"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountIn"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountOutMin"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapExactTokensForTokensSupportingFeeOnTransferTokens"",""outputs"":[],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountInMax"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapTokensForExactETH"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""internalType"":""uint256"",""name"":""amountOut"",""type"":""uint256""},{""internalType"":""uint256"",""name"":""amountInMax"",""type"":""uint256""},{""internalType"":""address[]"",""name"":""path"",""type"":""address[]""},{""internalType"":""address"",""name"":""to"",""type"":""address""},{""internalType"":""uint256"",""name"":""deadline"",""type"":""uint256""}],""name"":""swapTokensForExactTokens"",""outputs"":[{""internalType"":""uint256[]"",""name"":""amounts"",""type"":""uint256[]""}],""stateMutability"":""nonpayable"",""type"":""function""},{""stateMutability"":""payable"",""type"":""receive""}]";
        static Contract uniRouterContract;

        static MwStakingSol.Contracts.WEAPON.WEAPONService weaponservice;
        static MwStakingSol.Contracts.MWStaking.MWStakingService stakingservice;
        static string contractAddr;
        static string stakingAddr;
        static string pairAddr;
        static BigInteger deployerApproval;
        static List<BigInteger> reserves;
        static int buyGwei = 10;


        static async Task Main(string[] args)
        {
            Init();
            await HHReset();
            /*
            contractAddr = await DeployToken();
            pairAddr = await CreatePair(contractAddr);
            await Approve(deployerWeb3, contractAddr);
            await Approve(wallet4Web3, contractAddr);
            await AddLiquidity(contractAddr, 5);
            //await HHMine();
            reserves = await GetLiquidity(pairAddr);
            */
            await DeploymentMenu();
            await MainMenu();
        }

        static void Init()
        {
            httpsEndpoint = "http://127.0.0.1:8545";
            //httpsEndpoint = "http://192.168.1.22:8544";

            deployerKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
            wallet1Key = "0x59c6995e998f97a5a0044966f0945389dc9e86dae88c7a8412f4603b6b78690d";
            wallet2Key = "0x5de4111afa1a4b94908f83103eb1f1706367c2e68ca870fc3fb9a804cdab365a";
            wallet3Key = "0x7c852118294e51e653712a81e05800f419141751be58f605c371e15141b007a6";
            wallet4Key = "0x47e179ec197488593b187f80a00eb0da91f1b9d0b13f8733639f19c30a34926a";
            wallet5Key = "0x8b3a350cf5c34c9194ca85829a2df0ec3153be0318b5e2d3348e872092edffba";
            wallet6Key = "0x92db14e403b83dfe3df233f83dfa3a0d7096f21ca9b0d6d6b8d88b2b4ec1564e";
            wallet7Key = "0x4bbbf85ce3377467afe5d46f804f221813b2bb87f24d81f60f1fcdbf7cbf4356";
            vaultKey = "0xdbda1821b80551c9d65939329250298aa3472ba22feea921c0cf5d620ea67b97";

            deployerAccount = new Account(deployerKey, 31337);
            wallet1Account = new Account(wallet1Key, 31337);
            wallet2Account = new Account(wallet2Key, 31337);
            wallet3Account = new Account(wallet3Key, 31337);
            wallet4Account = new Account(wallet4Key, 31337);
            wallet5Account = new Account(wallet5Key, 31337);
            wallet6Account = new Account(wallet6Key, 31337);
            wallet7Account = new Account(wallet7Key, 31337);
            vaultAccount = new Account(vaultKey, 31337);

            deployerWeb3 = new Web3(deployerAccount, httpsEndpoint);
            wallet1Web3 = new Web3(wallet1Account, httpsEndpoint);
            wallet2Web3 = new Web3(wallet2Account, httpsEndpoint);
            wallet3Web3 = new Web3(wallet3Account, httpsEndpoint);
            wallet4Web3 = new Web3(wallet4Account, httpsEndpoint);
            wallet5Web3 = new Web3(wallet5Account, httpsEndpoint);
            wallet6Web3 = new Web3(wallet6Account, httpsEndpoint);
            wallet7Web3 = new Web3(wallet7Account, httpsEndpoint);
            vaultWeb3 = new Web3(vaultAccount, httpsEndpoint);


            allWeb3 = new List<Web3>()
            {
                wallet1Web3, wallet2Web3, wallet3Web3, wallet4Web3, wallet5Web3, wallet6Web3, wallet7Web3
            };
            allAccounts = new List<Account>()
            {
                wallet1Account, wallet2Account, wallet3Account, wallet4Account, wallet5Account, wallet6Account, wallet7Account
            };

            log = new List<string>();
        }

        static async Task DeploymentMenu()
        {
            do
            {
                Console.Clear();
                if (stakingAddr != null && stakingAddr != "")
                {
                    //await SetStakingContract(stakingAddr);
                }
                if (contractAddr != null && contractAddr != "")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if (contractAddr != null) Console.WriteLine("Deployer balance: " + await GetBalance(deployerWeb3, deployerAccount));
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Contract deployed at: " + contractAddr);
                    Console.WriteLine("Staking deployed at: " + stakingAddr);
                    if (pairAddr != null && pairAddr != "")
                        Console.WriteLine("Uniswap WETH pair created at:  " + pairAddr);
                    else Console.WriteLine("No Uniswap pair created yet.");
                    //if (pairAddr != null && pairAddr == await PairAddr()) Console.WriteLine("Contract pair address set.");
                    //else Console.WriteLine("Contract thinks pair is: " + await PairAddr());
                    if (deployerApproval == 0) Console.WriteLine("No approval for LP");
                    else Console.WriteLine("LP approved at " + deployerApproval);
                    if (weaponservice != null && pairAddr != null && await weaponservice.IsPoolQueryAsync(pairAddr) == false)
                        Console.WriteLine("Pair is pool: FALSE");
                    else Console.WriteLine("Pair is pool: True");
                    if (weaponservice != null && !String.Equals(await weaponservice.GetStakingContractQueryAsync(), stakingAddr, StringComparison.OrdinalIgnoreCase))
                        Console.WriteLine("Staking contract not set in ERC20, current staking is: " + await weaponservice.GetStakingContractQueryAsync());
                    else Console.WriteLine("Staking contract set: " + await weaponservice.GetStakingContractQueryAsync());
                }


                Console.ForegroundColor = ConsoleColor.Gray;
                if (contractAddr == null || contractAddr == "")
                    Console.WriteLine("1) Deploy ERC20 contract");
                if (stakingAddr == null || stakingAddr == "")
                    Console.WriteLine("2) Deploy staking contract");
                if (pairAddr == null || pairAddr == "")
                    Console.WriteLine("3) Create Uniswap pair");
                if (deployerApproval == 0)
                    Console.WriteLine("4) Approve for LP");
                if (reserves == null)
                    Console.WriteLine("5) Add liquidity");
                if (contractAddr != null && contractAddr != "" && await GetContractPairAddress() == false)
                    //Console.WriteLine("5) Add LBC protection");
                    Console.WriteLine("6) Set pair address");
                if (weaponservice != null && !String.Equals(await weaponservice.GetStakingContractQueryAsync(), stakingAddr, StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine("7) Set staking contract");
                if (stakingAddr != null && stakingAddr != "")
                {
                    var epoch1 = await stakingservice.GetEpochQueryAsync(1);
                    if (epoch1.EpochStartDate == 0)
                        Console.WriteLine("8) Add 20 epochs");
                }
                if (stakingAddr != null && stakingAddr != "")
                {
                    var epoch21 = await stakingservice.GetEpochQueryAsync(21);
                    if (epoch21.EpochStartDate == 0)
                        Console.WriteLine("9) Add 20 more epochs");
                }

                Console.WriteLine();
                Console.WriteLine("0) Go to interaction menu");

                ConsoleKeyInfo key = Console.ReadKey(true);
                var amount = random.NextDouble();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        if (contractAddr == null || contractAddr == "")
                            contractAddr = await DeployToken();
                        break;
                    case ConsoleKey.D2:
                        if (stakingAddr == null || stakingAddr == "")
                            stakingAddr = await DeployStaking();
                        break;
                    case ConsoleKey.D3:
                        if (pairAddr == null || pairAddr == "")
                            pairAddr = await CreatePair(contractAddr);
                        break;
                    case ConsoleKey.D4:
                        if (deployerApproval == 0)
                            deployerApproval = await Approve(deployerWeb3, contractAddr);
                        break;
                    case ConsoleKey.D5:
                        if (reserves == null)
                        {
                            await AddLiquidity(contractAddr, 10);
                            reserves = await GetLiquidity(pairAddr);
                        }
                        break;
                    case ConsoleKey.D6:
                        await SetPool(pairAddr);
                        break;
                    case ConsoleKey.D7:
                        if (weaponservice != null && await weaponservice.GetStakingContractQueryAsync() != stakingAddr)
                            await SetStakingContract(stakingAddr);
                        break;
                    case ConsoleKey.D8:
                        if (stakingAddr != null && stakingAddr != "")
                            await stakingservice.AddEpochsRequestAndWaitForReceiptAsync(1, 20);
                        break;
                    case ConsoleKey.D9:
                        if (stakingAddr != null && stakingAddr != "")
                            await stakingservice.AddEpochsRequestAndWaitForReceiptAsync(21, 20);
                        break;
                    case ConsoleKey.D0:
                        return;
                        break;
                        
                    case ConsoleKey.OemPlus:
                        await HHMine(0);
                        break;
                }


            } while (true);
        }

        static async Task MainMenu()
        {
            do
            {
                currentBlockTimestamp = deployerWeb3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(await deployerWeb3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Result.Timestamp.Value;

                deployerETH = await deployerWeb3.Eth.GetBalance.SendRequestAsync(deployerAccount.Address);
                wallet1ETH = await wallet1Web3.Eth.GetBalance.SendRequestAsync(wallet1Account.Address);
                wallet2ETH = await wallet2Web3.Eth.GetBalance.SendRequestAsync(wallet2Account.Address);
                wallet3ETH = await wallet3Web3.Eth.GetBalance.SendRequestAsync(wallet3Account.Address);
                wallet4ETH = await wallet4Web3.Eth.GetBalance.SendRequestAsync(wallet4Account.Address);
                wallet5ETH = await wallet5Web3.Eth.GetBalance.SendRequestAsync(wallet5Account.Address);
                wallet6ETH = await wallet6Web3.Eth.GetBalance.SendRequestAsync(wallet6Account.Address);
                wallet7ETH = await wallet7Web3.Eth.GetBalance.SendRequestAsync(wallet7Account.Address);
                vaultETH = await vaultWeb3.Eth.GetBalance.SendRequestAsync(vaultAccount.Address);
                stakingETH = await deployerWeb3.Eth.GetBalance.SendRequestAsync(stakingAddr);

                contractETH = await deployerWeb3.Eth.GetBalance.SendRequestAsync(contractAddr);

                pairWETH = await GetBalance(deployerWeb3, deployerAccount, pairAddr, weth);
                pairTOKEN = await GetBalance(deployerWeb3, deployerAccount, pairAddr);


                deployerTOKEN = await GetBalance(deployerWeb3, deployerAccount);
                wallet1TOKEN = await GetBalance(wallet1Web3, wallet1Account);
                wallet2TOKEN = await GetBalance(wallet2Web3, wallet2Account);
                wallet3TOKEN = await GetBalance(wallet3Web3, wallet3Account);
                wallet4TOKEN = await GetBalance(wallet4Web3, wallet4Account);
                wallet5TOKEN = await GetBalance(wallet5Web3, wallet5Account);
                wallet6TOKEN = await GetBalance(wallet6Web3, wallet6Account);
                wallet7TOKEN = await GetBalance(wallet7Web3, wallet7Account);

                contractTOKEN = await GetBalance(deployerWeb3, deployerAccount, contractAddr);

                var randoHoldings = new BigInteger();
                if (randos != null && randos.Count() > 0)
                {
                    foreach (RandomTrader trader in randos)
                    {
                        randoHoldings += await (GetBalance(trader.web3, trader.account));
                    }
                }
                Thread.Sleep(500);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Contract deployed at: " + contractAddr);
                Console.WriteLine("Uniswap WETH pair created at:  " + pairAddr);
                if (GetContractPairAddress().Result) Console.WriteLine("Contract pair address set.");
                else Console.WriteLine("Contract doesn't think the pair isPool");
                if (await weaponservice.GetStakingContractQueryAsync() != null) Console.WriteLine("Staking contract is set as: " + await weaponservice.GetStakingContractQueryAsync());
                else Console.WriteLine("Staking contract not set");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Current LP balance:");
                if (pairAddr != null && pairAddr != "") Console.WriteLine("  LP TOKEN: " + await GetBalance(deployerWeb3, deployerAccount, "", pairAddr));
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Balances:");
                Console.WriteLine("TOKEN/WETH contract:  " + Web3.Convert.FromWei(pairWETH) + "ETH / " + DropTokenDec(pairTOKEN) + "TOKEN");
                Console.WriteLine("Staking ETH: " + Web3.Convert.FromWei(stakingETH));
                Console.WriteLine("Vault ETH  : " + Web3.Convert.FromWei(vaultETH));
                Console.WriteLine("Rando trader buy volume: " + Web3.Convert.FromWei(randoBuyVolume));
                Console.WriteLine("Rando trader sell volume: " + Web3.Convert.FromWei(randoSellVolume));
                Console.WriteLine("Total ETH volume: " + Web3.Convert.FromWei(totalETHvolume));
                Console.WriteLine("Tx Count: " + txCount);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("StakingEnabled (ERC/StakingContract): " + await weaponservice.StakingEnabledQueryAsync()/* + " / " + await stakingservice.StakingEnabledQueryAsync()*/);
                var currentEpoch = await stakingservice.CurrentEpochQueryAsync();
                var currentEpochStats = await stakingservice.GetEpochQueryAsync(currentEpoch);
                Console.WriteLine("Current Epoch: " + currentEpoch);
                Console.WriteLine("Start: " + currentEpochStats.EpochStartDate + " | PoolSize: " + currentEpochStats.EpochPool + " | ETH: " + Web3.Convert.FromWei(currentEpochStats.EpochEth));
                Console.WriteLine("Current block timestamp: " + currentBlockTimestamp);
                Console.ForegroundColor = ConsoleColor.Blue;
                for (int i = 0; i < 15; i++)
                {
                    MwStakingSol.Contracts.MWStaking.ContractDefinition.GetEpochOutputDTO dto = await stakingservice.GetEpochQueryAsync(i);
                    var pool = (double)dto.EpochPool / Math.Pow(10, 9);
                    var eth = (double)dto.EpochEth / Math.Pow(10, 18);
                    Console.WriteLine("Epoch" + i + ": Pool " + pool + " // ETH: " + eth);
                }
                Console.WriteLine();
                Console.WriteLine("Contract:  " + Web3.Convert.FromWei(contractETH).ToString("F4") + "ETH / " + DropTokenDec(contractTOKEN).ToString("F2"));
                Console.WriteLine("Deployer:  " + Web3.Convert.FromWei(deployerETH).ToString("F4") + "ETH / " + DropTokenDec(deployerTOKEN).ToString("F2"));
                Console.WriteLine("Dev     :  " + Web3.Convert.FromWei(wallet1ETH).ToString("F4") + "ETH / " + DropTokenDec(wallet1TOKEN).ToString("F2") + "       Stake: " + await GetStakeString(wallet1Account.Address));
                Console.WriteLine("wallet2  :  " + Web3.Convert.FromWei(wallet2ETH).ToString("F4") + "ETH / " + DropTokenDec(wallet2TOKEN).ToString("F2") + "       Stake: " + await GetStakeString(wallet2Account.Address));
                Console.WriteLine("wallet3  :  " + Web3.Convert.FromWei(wallet3ETH).ToString("F4") + "ETH / " + DropTokenDec(wallet3TOKEN).ToString("F2") + "       Stake: " + await GetStakeString(wallet3Account.Address));
                Console.WriteLine("wallet4  :  " + Web3.Convert.FromWei(wallet4ETH).ToString("F4") + "ETH / " + DropTokenDec(wallet4TOKEN).ToString("F2") + "       Stake: " + await GetStakeString(wallet4Account.Address));
                Console.WriteLine("wallet5  :  " + Web3.Convert.FromWei(wallet5ETH).ToString("F4") + "ETH / " + DropTokenDec(wallet5TOKEN).ToString("F2") + "       Stake: " + await GetStakeString(wallet5Account.Address));
                Console.WriteLine("wallet6  :  " + Web3.Convert.FromWei(wallet6ETH).ToString("F4") + "ETH / " + DropTokenDec(wallet6TOKEN).ToString("F2") + "       Stake: " + await GetStakeString(wallet6Account.Address));
                Console.WriteLine("wallet7  :  " + Web3.Convert.FromWei(wallet7ETH).ToString("F4") + "ETH / " + DropTokenDec(wallet7TOKEN).ToString("F2") + "       Stake: " + await GetStakeString(wallet7Account.Address));
                Console.WriteLine("Rando Traders currently hold: " + randoHoldings);
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("1) Buy with wallet1");
                Console.WriteLine("2) Buy with wallet2");
                Console.WriteLine("3) Buy with wallet3");
                Console.WriteLine("4) Buy with wallet4");
                Console.WriteLine("5) Buy with wallet5");
                Console.WriteLine("6) Buy with wallet6");
                Console.WriteLine("7) Buy with wallet7");
                Console.WriteLine("Shift = stake, Alt = unstake, Ctrl = sell all");
                Console.WriteLine();
                Console.WriteLine("L) Lock and Load (lol)");
                Console.WriteLine("S) Toggle Staking");
                Console.WriteLine("R) Remove liquidity");
                Console.WriteLine("G) Generate new Rando Traders");
                Console.WriteLine("B) Execute bulk trades");
                Console.WriteLine("C) Claim staking rewards");
                Console.WriteLine("P) Lock staking pool size");
                Console.WriteLine("I) Inject .05 ETH into current pool");
                Console.WriteLine("D) Disperse test");
                //Console.WriteLine("O) Toggle LBC");


                Console.WriteLine();
                Console.WriteLine("9) Change gas for buys (currently " + buyGwei.ToString() + " gwei)");

                Console.WriteLine();
                foreach (string str in log)
                {
                    Console.WriteLine(str);
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                var amount = 0.2;

                if ((key.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            await Approve(wallet1Web3, contractAddr);
                            await Sell(wallet1Web3, wallet1Account, wallet1TOKEN, 0, "wallet1");
                            break;
                        case ConsoleKey.D2:
                            await Approve(wallet2Web3, contractAddr);
                            await Sell(wallet2Web3, wallet2Account, wallet2TOKEN, 0, "wallet2");
                            //await Sell(wallet2Web3, wallet2Account, new BigInteger(45000 * Math.Pow(10, decimals)), 0, "wallet2");
                            break;
                        case ConsoleKey.D3:
                            await Approve(wallet3Web3, contractAddr);
                            await Sell(wallet3Web3, wallet3Account, wallet3TOKEN, 0, "wallet3");
                            break;
                        case ConsoleKey.D4:
                            await Approve(wallet4Web3, contractAddr);
                            await Sell(wallet4Web3, wallet4Account, wallet4TOKEN, 0, "wallet4");
                            break;
                        case ConsoleKey.D5:
                            await Approve(wallet5Web3, contractAddr);
                            await Sell(wallet5Web3, wallet5Account, wallet5TOKEN, 0, "wallet5");
                            break;
                        case ConsoleKey.D6:
                            await Approve(wallet6Web3, contractAddr);
                            await Sell(wallet6Web3, wallet6Account, wallet6TOKEN, 0, "wallet6");
                            break;
                        case ConsoleKey.D7:
                            await Approve(wallet7Web3, contractAddr);
                            await Sell(wallet7Web3, wallet7Account, wallet7TOKEN, 0, "wallet7");
                            break;
                    }
                }
                else if ((key.Modifiers & ConsoleModifiers.Alt) != 0)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            Console.Write("Enter unstake amount:");
                            var unstake1Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            await Unstake(wallet1Web3, wallet1Account, unstake1Amount);
                            //await BuyExact(wallet1Web3, wallet1Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet1");
                            break;
                        case ConsoleKey.D2:
                            var unstake2Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            await Unstake(wallet2Web3, wallet2Account, unstake2Amount);
                            //await BuyExact(wallet2Web3, wallet2Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet2");
                            break;
                        case ConsoleKey.D3:
                            var unstake3Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            await Unstake(wallet3Web3, wallet3Account, unstake3Amount);
                            //await BuyExact(wallet3Web3, wallet3Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet3");
                            break;
                        case ConsoleKey.D4:
                            var unstake4Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            await Unstake(wallet4Web3, wallet4Account, unstake4Amount);
                            //await BuyExact(wallet4Web3, wallet4Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet4");
                            break;
                        case ConsoleKey.D5:
                            var unstake5Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            await Unstake(wallet5Web3, wallet5Account, unstake5Amount);
                            //await BuyExact(wallet5Web3, wallet5Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet5");
                            break;
                        case ConsoleKey.D6:
                            var unstake6Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            await Unstake(wallet6Web3, wallet6Account, unstake6Amount);
                            //await BuyExact(wallet6Web3, wallet6Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet6");
                            break;
                        case ConsoleKey.D7:
                            var unstake7Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            await Unstake(wallet7Web3, wallet7Account, unstake7Amount);
                            //await BuyExact(wallet7Web3, wallet7Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet7");
                            break;
                    }
                }
                else if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            Console.WriteLine("Enter stake amount: ");
                            var stake1Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            Console.WriteLine("Enter stake expire timestamp: ");
                            var stake1End = new BigInteger(int.Parse(Console.ReadLine()));
                            await Stake(wallet1Web3, wallet1Account, stake1Amount, stake1End);
                            break;
                        case ConsoleKey.D2:
                            Console.WriteLine("Enter stake amount: ");
                            var stake2Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            Console.WriteLine("Enter stake expire timestamp: ");
                            var stake2End = new BigInteger(int.Parse(Console.ReadLine()));
                            await Stake(wallet2Web3, wallet2Account, stake2Amount, stake2End);
                            break;
                        case ConsoleKey.D3:
                            Console.WriteLine("Enter stake amount: ");
                            var stake3Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            Console.WriteLine("Enter stake expire timestamp: ");
                            var stake3End = new BigInteger(int.Parse(Console.ReadLine()));
                            await Stake(wallet3Web3, wallet3Account, stake3Amount, stake3End);
                            break;
                        case ConsoleKey.D4:
                            Console.WriteLine("Enter stake amount: ");
                            var stake4Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            Console.WriteLine("Enter stake expire timestamp: ");
                            var stake4End = new BigInteger(int.Parse(Console.ReadLine()));
                            await Stake(wallet4Web3, wallet4Account, stake4Amount, stake4End);
                            break;
                        case ConsoleKey.D5:
                            Console.WriteLine("Enter stake amount: ");
                            var stake5Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            Console.WriteLine("Enter stake expire timestamp: ");
                            var stake5End = new BigInteger(int.Parse(Console.ReadLine()));
                            await Stake(wallet5Web3, wallet5Account, stake5Amount, stake5End);
                            break;
                        case ConsoleKey.D6:
                            Console.WriteLine("Enter stake amount: ");
                            var stake6Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            Console.WriteLine("Enter stake expire timestamp: ");
                            var stake6End = new BigInteger(int.Parse(Console.ReadLine()));
                            await Stake(wallet6Web3, wallet6Account, stake6Amount, stake6End);
                            break;
                        case ConsoleKey.D7:
                            Console.WriteLine("Enter stake amount: ");
                            var stake7Amount = new BigInteger(int.Parse(Console.ReadLine()) * Math.Pow(10, decimals));
                            Console.WriteLine("Enter stake expire timestamp: ");
                            var stake7End = new BigInteger(int.Parse(Console.ReadLine()));
                            await Stake(wallet7Web3, wallet7Account, stake7Amount, stake7End);
                            break;

                    }
                }
                else
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            await Buy(wallet1Web3, wallet1Account, amount, 0, "wallet1");
                            break;
                        case ConsoleKey.D2:
                            await Buy(wallet2Web3, wallet2Account, amount, 0, "wallet2");
                            break;
                        case ConsoleKey.D3:
                            //await BuyExact(wallet3Web3, wallet3Account, (BigInteger)(50000 * Math.Pow(10, decimals)), 0, "wallet3");
                            Console.Write("Enter buy amount: ");
                            var thisAmount = double.Parse(Console.ReadLine());
                            await Buy(wallet3Web3, wallet3Account, thisAmount, 1, "wallet3");
                            break;
                        case ConsoleKey.D4:
                            await Buy(wallet4Web3, wallet4Account, amount, 1, "wallet4");
                            break;
                        case ConsoleKey.D5:
                            await Buy(wallet5Web3, wallet5Account, amount, 1, "wallet5");
                            break;
                        case ConsoleKey.D6:
                            await Buy(wallet6Web3, wallet6Account, amount, 1, "wallet6");
                            break;
                        case ConsoleKey.D7:
                            await Buy(wallet7Web3, wallet7Account, amount, 1, "wallet7");
                            break;
                        case ConsoleKey.G:
                            Console.Write("Enter number of traders: ");
                            int numTraders = int.Parse(Console.ReadLine());
                            randos = await SolCreateTraders(numTraders);
                            break;
                        case ConsoleKey.B:
                            Console.Write("Enter number of trades: ");
                            int numTrades = int.Parse(Console.ReadLine());
                            //var traderList = await SolCreateTraders(numTrades / 2);
                            await ExecuteBulkTrades(numTrades, randos);
                            Console.ReadLine();
                            break;
                        case ConsoleKey.S:
                            await ToggleStaking();
                            break;
                        case ConsoleKey.C:
                            await ClaimMenu();
                            
                            Console.ReadLine();
                            break;

                        case ConsoleKey.D9:
                            Console.Write("Enter GasPrice for buys in gwei: ");
                            buyGwei = int.Parse(Console.ReadLine());
                            break;
                        case ConsoleKey.R:
                            await Approve(deployerWeb3, pairAddr);
                            await RemoveLiquidity(pairAddr);
                            break;
                        case ConsoleKey.O:
                            //await ToggleOBC();
                            break;
                        case ConsoleKey.P:
                            await SetPoolSize();
                            break;
                        case ConsoleKey.I:
                            await deployerWeb3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(stakingAddr, .05m, null, 100000);
                            break;
                        case ConsoleKey.D:
                            await Disperse();
                            break;
                    }
                }
            } while (true);
        }

        static async Task<string> DeployToken()
        {
            weaponservice = await MwStakingSol.Contracts.WEAPON.WEAPONService.DeployContractAndGetServiceAsync(deployerWeb3, new MwStakingSol.Contracts.WEAPON.ContractDefinition.WEAPONDeployment() { MultiSig = deployerAccount.Address.ToString(), Vault = vaultAccount.Address.ToString() });
            return weaponservice.ContractHandler.ContractAddress;
        }

        static async Task<string> DeployStaking()
        {
            stakingservice = await MwStakingSol.Contracts.MWStaking.MWStakingService.DeployContractAndGetServiceAsync(deployerWeb3, new MwStakingSol.Contracts.MWStaking.ContractDefinition.MWStakingDeployment() { WeaponAddress = contractAddr, MultiSig = deployerAccount.Address, EpochRobot = deployerAccount.Address });
            return stakingservice.ContractHandler.ContractAddress;
        }


        static async Task<string> CreatePair(string tokenAddr)
        {
            string pairAddr = "";

            var createPairManager = deployerWeb3.Eth.GetContractTransactionHandler<Sol.Contracts.IUniswapV2Factory.ContractDefinition.CreatePairFunction>();
            var pairCreate = new Sol.Contracts.IUniswapV2Factory.ContractDefinition.CreatePairFunction()
            {
                TokenA = weth,
                TokenB = tokenAddr
            };

            //pairCreate.GasPrice = Web3.Convert.ToWei(30, UnitConversion.EthUnit.Gwei);

            var pairReceipt = await createPairManager.SendRequestAndWaitForReceiptAsync(uniFactory, pairCreate);
            if (pairReceipt.Status.Value == 1)
            {
                var events = pairReceipt.DecodeAllEvents<Sol.Contracts.IUniswapV2Factory.ContractDefinition.PairCreatedEventDTO>();
                foreach (EventLog<Sol.Contracts.IUniswapV2Factory.ContractDefinition.PairCreatedEventDTO> e in events)
                {
                    Console.WriteLine("Pair created at: " + e.Event.Pair);
                    pairAddr = e.Event.Pair;
                    return pairAddr;
                }
                return pairAddr;
            }
            else
            {
                Console.WriteLine("Pair creation failed");
                return null;
            }

        }

        static async Task<BigInteger> Approve(Web3 web3, string tokenAddr)
        {
            var approveHandler = web3.Eth.GetContractTransactionHandler<Sol.Contracts.IERC20.ContractDefinition.ApproveFunction>();
            var approval = new Sol.Contracts.IERC20.ContractDefinition.ApproveFunction()
            {
                Spender = uniRouter,
                Amount = BigInteger.Parse("115792089237316195423570985008687907853269984665640564039457")
            };

            var approveReceipt = await approveHandler.SendRequestAndWaitForReceiptAsync(tokenAddr, approval);
            if (approveReceipt.Status.Value == 1)
            {
                Console.WriteLine("Approval to uniRouter success");
                var approvalResult = approveReceipt.DecodeAllEvents<Sol.Contracts.IERC20.ContractDefinition.ApprovalEventDTO>();
                return approvalResult[0].Event.Value;
            }
            else return 0;
        }

        static async Task<bool> AddLiquidity(string tokenAddr, double ethToAdd)
        {
            var deadlineTime = new DateTimeOffset(DateTime.Now.AddSeconds(10800));
            var deadlineStamp = deadlineTime.ToUnixTimeSeconds();

            var addLiqHandler = deployerWeb3.Eth.GetContractTransactionHandler<Sol.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityETHFunction>();
            var addLiq = new Sol.Contracts.UniswapV2Router02.ContractDefinition.AddLiquidityETHFunction()
            {
                Token = tokenAddr,
                AmountTokenDesired = BigInteger.Parse("10000000000000000"),
                AmountTokenMin = BigInteger.Parse("10000000000000000"),
                AmountETHMin = Web3.Convert.ToWei(ethToAdd),
                Deadline = deadlineStamp,
                To = deployerAccount.Address,
                AmountToSend = Web3.Convert.ToWei(ethToAdd)
            };

            var addLiqReceipt = await addLiqHandler.SendRequestAndWaitForReceiptAsync(uniRouter, addLiq);
            if (addLiqReceipt.Status.Value == 1) return true;
            else return false;
        }

        static async Task<bool> RemoveLiquidity(string pairAddr)
        {
            var liqBalance = await GetBalance(deployerWeb3, deployerAccount, "", pairAddr);
            var deadlineTime = new DateTimeOffset(DateTime.Now.AddSeconds(10800));
            var deadlineStamp = deadlineTime.ToUnixTimeSeconds();

            var removeLiqHandler = deployerWeb3.Eth.GetContractTransactionHandler<Sol.Contracts.IUniswapV2Router02.ContractDefinition.RemoveLiquidityETHSupportingFeeOnTransferTokensFunction>();
            var removeLiq = new Sol.Contracts.IUniswapV2Router02.ContractDefinition.RemoveLiquidityETHSupportingFeeOnTransferTokensFunction()
            {
                Token = contractAddr,
                Liquidity = liqBalance,
                AmountTokenMin = new BigInteger(1 * Math.Pow(10, decimals)),
                AmountETHMin = new BigInteger(1 * Math.Pow(10, 18)),
                To = deployerAccount.Address,
                Deadline = deadlineStamp
            };

            Console.WriteLine("Token: " + removeLiq.Token);
            Console.WriteLine("Liquidity: " + removeLiq.Liquidity);
            Console.WriteLine("AmountTokenMin: " + removeLiq.AmountTokenMin);
            Console.WriteLine("AmountETHMin: " + removeLiq.AmountETHMin);
            Console.WriteLine("To: " + removeLiq.To);
            Console.WriteLine("Deadline: " + removeLiq.Deadline);
            Console.ReadLine();

            var receipt = await removeLiqHandler.SendRequestAndWaitForReceiptAsync(uniRouter, removeLiq);
            if (receipt != null) return true;
            else return false;
        }


        static async Task<List<BigInteger>> GetLiquidity(string pairAddr)
        {
            var getLiqHandler = deployerWeb3.Eth.GetContractQueryHandler<Sol.Contracts.IUniswapV2Pair.ContractDefinition.GetReservesFunction>();
            var getLiq = new Sol.Contracts.IUniswapV2Pair.ContractDefinition.GetReservesFunction();

            var reserves = new Sol.Contracts.IUniswapV2Pair.ContractDefinition.GetReservesOutputDTO();
            reserves = await getLiqHandler.QueryDeserializingToObjectAsync<Sol.Contracts.IUniswapV2Pair.ContractDefinition.GetReservesOutputDTO>(getLiq, pairAddr);
            Console.WriteLine(reserves.Reserve0 + " / " + reserves.Reserve1);
            return new List<BigInteger>()
            {
                reserves.Reserve0,
                reserves.Reserve1
            };
        }

        static async Task<BigInteger> Buy(Web3 web3, Account account, double buyETH, int gasMod = 0, string name = "")
        {
            BigInteger actualBuy = 0;
            var expectedBuy = await GetTokensOut(web3, account, buyETH);

            //var deadlineTime = new DateTimeOffset(DateTime.Now.AddSeconds(10800));
            //var deadlineStamp = deadlineTime.ToUnixTimeSeconds();
            var deadlineStamp = currentBlockTimestamp + 10800;

            var buyHandler = web3.Eth.GetContractTransactionHandler<Sol.Contracts.UniswapV2Router02.ContractDefinition.SwapExactETHForTokensSupportingFeeOnTransferTokensFunction>();
            var buy = new Sol.Contracts.UniswapV2Router02.ContractDefinition.SwapExactETHForTokensSupportingFeeOnTransferTokensFunction()
            {
                AmountOutMin = 1,
                Path = new List<string> { weth, contractAddr },
                To = account.Address,
                Deadline = deadlineStamp,
                AmountToSend = Web3.Convert.ToWei(buyETH)
            };

            //buy.GasPrice = Web3.Convert.ToWei((buyGwei + gasMod), UnitConversion.EthUnit.Gwei);
            var buyReceipt = await buyHandler.SendRequestAndWaitForReceiptAsync(uniRouter, buy);
            var decoded = buyReceipt.Logs.DecodeAllEvents<Sol.Contracts.IUniswapV2Pair.ContractDefinition.SwapEventDTO>();
            Thread.Sleep(100);
            actualBuy = await GetBalance(web3, account);
            var expectedBuyReadable = ((double)expectedBuy / Math.Pow(10, decimals)).ToString("F4");
            var actualBuyReadable = ((double)actualBuy / Math.Pow(10, decimals)).ToString("F4");

            var variance = 1 - ((double)actualBuy / (double)expectedBuy);

            if (buyReceipt.Status.Value == 1)
            {
                log.Add(name + " | BUY | Expected: " + expectedBuyReadable + " | Actual swap: " + buyETH + " ETH for " + actualBuyReadable + " TOKEN | TAXED " + variance.ToString("F4"));
            }
            else log.Add(name + " | " + buyReceipt.BlockNumber + "[" + buyReceipt.TransactionIndex + "] | BUY | " + ((double)actualBuy / Math.Pow(10, decimals)).ToString("F5") + " TOKEN | | FAILURE | " + buyETH + " ETH");
            txCount += 1;
            return actualBuy;
        }

        static async Task<BigInteger> BuyExact(Web3 web3, Account account, BigInteger tokensOut, int gasMod = 0, string name = "")
        {
            BigInteger actualBuy = 0;

            var ethInHandler = web3.Eth.GetContractQueryHandler<Sol.Contracts.IUniswapV2Router02.ContractDefinition.GetAmountsInFunction>();
            var ethIn = new Sol.Contracts.IUniswapV2Router02.ContractDefinition.GetAmountsInFunction()
            {
                AmountOut = tokensOut,
                Path = new List<string>() { weth, contractAddr }
            };

            var ethInResult = await ethInHandler.QueryAsync<List<BigInteger>>(uniRouter, ethIn);


            var buyETH = (double)ethInResult[0] / Math.Pow(10, 18);
            var buyETHReadable = buyETH.ToString("F4");

            //var buyETH = 0.2;
            var expectedBuy = tokensOut;

            //var deadlineTime = new DateTimeOffset(DateTime.Now.AddSeconds(10800));
            //var deadlineStamp = deadlineTime.ToUnixTimeSeconds();
            var deadlineStamp = currentBlockTimestamp + 10800;

            var buyHandler = web3.Eth.GetContractTransactionHandler<Sol.Contracts.IUniswapV2Router02.ContractDefinition.SwapETHForExactTokensFunction>();
            var buy = new Sol.Contracts.IUniswapV2Router02.ContractDefinition.SwapETHForExactTokensFunction()
            {
                AmountOut = tokensOut,
                Path = new List<string> { weth, contractAddr },
                To = account.Address,
                Deadline = deadlineStamp,
                AmountToSend = Web3.Convert.ToWei(buyETH * 1.1)
            };

            //buy.GasPrice = Web3.Convert.ToWei((buyGwei + gasMod), UnitConversion.EthUnit.Gwei);
            var buyReceipt = await buyHandler.SendRequestAndWaitForReceiptAsync(uniRouter, buy);
            var decoded = buyReceipt.Logs.DecodeAllEvents<Sol.Contracts.IUniswapV2Pair.ContractDefinition.SwapEventDTO>();
            Thread.Sleep(100);
            actualBuy = await GetBalance(web3, account);
            var expectedBuyReadable = ((double)expectedBuy / Math.Pow(10, decimals)).ToString("F4");
            var actualBuyReadable = ((double)actualBuy / Math.Pow(10, decimals)).ToString("F4");

            var variance = 1 - ((double)actualBuy / (double)expectedBuy);

            if (buyReceipt.Status.Value == 1)
            {
                log.Add(name + " | BUY | Expected: " + expectedBuyReadable + " | Actual swap: " + buyETHReadable + " ETH for " + actualBuyReadable + " TOKEN | TAXED " + variance.ToString("F4"));
            }
            else log.Add(name + " | " + buyReceipt.BlockNumber + "[" + buyReceipt.TransactionIndex + "] | BUY | " + ((double)actualBuy / Math.Pow(10, decimals)).ToString("F5") + " TOKEN | | FAILURE | " + buyETH + " ETH");
            txCount += 1;
            return actualBuy;
        }

        static async Task<BigInteger?> Sell(Web3 web3, Account account, BigInteger tokensIn, int gasMod = -1, string name = "")
        {
            BigInteger? actualSell = 0;
            var expectedSell = await GetWethOut(web3, account, tokensIn);

            var ethBeforeSell = (BigInteger)await web3.Eth.GetBalance.SendRequestAsync(account.Address);

            //var deadlineTime = new DateTimeOffset(DateTime.Now.AddSeconds(10800));
            //var deadlineStamp = deadlineTime.ToUnixTimeSeconds();
            var deadlineStamp = currentBlockTimestamp + 10800;

            var sellHandler = web3.Eth.GetContractTransactionHandler<Sol.Contracts.UniswapV2Router02.ContractDefinition.SwapExactTokensForETHSupportingFeeOnTransferTokensFunction>();
            var sell = new Sol.Contracts.UniswapV2Router02.ContractDefinition.SwapExactTokensForETHSupportingFeeOnTransferTokensFunction()
            {
                AmountIn = tokensIn,
                AmountOutMin = 1,
                Path = new List<string> { contractAddr, weth },
                To = account.Address,
                Deadline = deadlineStamp
            };

            //sell.GasPrice = Web3.Convert.ToWei((buyGwei + gasMod), UnitConversion.EthUnit.Gwei);
            sell.Gas = 600000;

            if (debugLevel > 0)
            {
                Console.WriteLine("Outgoing sell:");
                Console.WriteLine("AmountIn: " + sell.AmountIn);
                Console.WriteLine("AmountOutMin:" + sell.AmountOutMin);
                Console.WriteLine("To: " + sell.To);
                Console.WriteLine("Deadline: " + deadlineStamp);
                Console.ReadLine();
            }

            var sellReceipt = await sellHandler.SendRequestAndWaitForReceiptAsync(uniRouter, sell);
            var decoded = sellReceipt.Logs.DecodeAllEvents<Sol.Contracts.IUniswapV2Pair.ContractDefinition.SwapEventDTO>();
            var actualTokensOut = decoded[decoded.Count() - 1].Event.Amount1In;
            if (actualTokensOut == 0) actualTokensOut = decoded[decoded.Count() - 1].Event.Amount0In;
            actualSell = decoded[decoded.Count() - 1].Event.Amount0Out;
            if (actualSell == 0) actualSell = decoded[decoded.Count() - 1].Event.Amount1Out;

            var variance = 1 - ((double)actualSell / (double)expectedSell);

            var expectedSellReadable = ((double)expectedSell / Math.Pow(10, 18)).ToString("F4");
            var actualTokensOutReadable = ((double)actualTokensOut / Math.Pow(10, decimals)).ToString("F4");
            var actualSellReadable = ((double)actualSell / Math.Pow(10, 18)).ToString("F4");

            if (sellReceipt.Status.Value == 1)
            {
                log.Add(name + " | SELL | Expected: " + expectedSellReadable + "ETH | Actual swap: " + actualTokensOutReadable + " TOKEN for " + actualSellReadable + " ETH | TAXED " + variance);

                var gasSpent = sellReceipt.GasUsed * sell.GasPrice;
                var ethAfterSell = (BigInteger)await web3.Eth.GetBalance.SendRequestAsync(account.Address);
                //actualSell = ethAfterSell - ethBeforeSell + gasSpent;
            }
            else log.Add(name + " | " + sellReceipt.BlockNumber + "[" + sellReceipt.TransactionIndex + "] | SELL | " + ((double)actualSell / Math.Pow(10, decimals)).ToString("F5") + " ETH | FAILURE | " + tokensIn);
            /*
            foreach (EventLog<Sol.Contracts.IUniswapV2Pair.ContractDefinition.SwapEventDTO> eventLog in decoded)
            {
                log.Add("ExpectedSell: " + expectedSell);
                log.Add(eventLog.Event.Amount0In.ToString());
                log.Add(eventLog.Event.Amount0Out.ToString());
                log.Add(eventLog.Event.Amount1In.ToString());
                log.Add(eventLog.Event.Amount1Out.ToString());
            }
            */
            txCount += 1;
            return actualSell;
        }

        static async Task<BigInteger> GetBalance(Web3 web3, Account account, string walletAddr = "", string tokenAddr = "")
        {
            if (walletAddr == "") walletAddr = account.Address;
            if (tokenAddr == "") tokenAddr = contractAddr;
            var balanceHandler = web3.Eth.GetContractQueryHandler<Sol.Contracts.IERC20.ContractDefinition.BalanceOfFunction>();
            var bal = new Sol.Contracts.IERC20.ContractDefinition.BalanceOfFunction()
            {
                Account = walletAddr
            };

            var balance = await balanceHandler.QueryAsync<BigInteger>(tokenAddr, bal);
            var returnBal = balance;
            return returnBal;
        }

        static async Task<BigInteger> GetTokensOut(Web3 web3, Account account, double amountIn)
        {
            var amountInBig = (BigInteger)(amountIn * Math.Pow(10, 18));

            var amountOutHandler = web3.Eth.GetContractQueryHandler<Sol.Contracts.UniswapV2Router02.ContractDefinition.GetAmountsOutFunction>();
            var amounts = new Sol.Contracts.UniswapV2Router02.ContractDefinition.GetAmountsOutFunction()
            {
                AmountIn = amountInBig,
                Path = new List<string> { weth, contractAddr }
            };

            var amountList = await amountOutHandler.QueryAsync<List<BigInteger>>(uniRouter, amounts);
            var amount = amountList[1];
            return amount;
        }

        static async Task<BigInteger> GetWethOut(Web3 web3, Account account, BigInteger tokensIn)
        {
            var amountOutHandler = web3.Eth.GetContractQueryHandler<Sol.Contracts.UniswapV2Router02.ContractDefinition.GetAmountsOutFunction>();
            var amounts = new Sol.Contracts.UniswapV2Router02.ContractDefinition.GetAmountsOutFunction()
            {
                AmountIn = tokensIn,
                Path = new List<string> { contractAddr, weth }
            };

            var amountList = await amountOutHandler.QueryAsync<List<BigInteger>>(uniRouter, amounts);
            var amount = amountList[1];
            return amount;
        }

        static BigInteger DropTokenDec(BigInteger amount)
        {
            var newAmount = (double)amount / Math.Pow(10, decimals);
            return new BigInteger(newAmount);
        }

        static async Task SetPool(string addr)
        {
            var poolAdHandler = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.WEAPON.ContractDefinition.SetPoolFunction>();
            var poolAddr = new MwStakingSol.Contracts.WEAPON.ContractDefinition.SetPoolFunction()
            {
                Addr = addr
            };
            await poolAdHandler.SendRequestAndWaitForReceiptAsync(contractAddr, poolAddr);
        }

        static async Task<bool> GetContractPairAddress()
        {
            /*
            var pairAdHandler = deployerWeb3.Eth.GetContractQueryHandler<MwStakingSol.Contracts.JOJO.ContractDefinition.PairAddressFunction>();
            var pairAd = new MwStakingSol.Contracts.JOJO.ContractDefinition.PairAddressFunction();

            var address = await pairAdHandler.QueryAsync<string>(contractAddr, pairAd);
            return address;
            */
            if (pairAddr != null)
            {
                var poolAddr = new MwStakingSol.Contracts.WEAPON.ContractDefinition.IsPoolFunction()
                {
                    Addr = pairAddr
                };
                var isPool = await weaponservice.IsPoolQueryAsync(poolAddr);
                return isPool;
            }
            else return false;
        }

        static async Task<List<RandomTrader>> SolCreateTraders(int howMany)
        {
            var traderList = new List<RandomTrader>();
            for (int i = 0; i < howMany; i++)
            {
                var trader = new RandomTrader();
                var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
                var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
                trader.account = new Account(privateKey, new BigInteger(31337));
                trader.web3 = new Web3(trader.account, httpsEndpoint);
                trader.name = "randoTrader." + i;
                trader.tokens = new BigInteger(0);
                traderList.Add(trader);
                await wallet7Web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(trader.account.Address, 2m);
            }
            return traderList;
        }

        static async Task ExecuteBulkTrades(int numberOfTrades, List<RandomTrader> traderList)
        {
            System.Random random = new System.Random();
            using (StreamWriter file = new("log.txt"))
            {
                file.AutoFlush = true;
                for (int i = 0; i < numberOfTrades; i++)
                {
                    var wallet1TOKENbefore = await GetBalance(wallet1Web3, wallet1Account);

                    int traderNum = random.Next(traderList.Count());



                    var trader = traderList[traderNum];
                    trader.tokens = await GetBalance(trader.web3, trader.account);
                    var tokensBefore = trader.tokens;
                    if (trader.tokens <= new BigInteger(100 * Math.Pow(10, decimals)))
                    {
                        var traderETH = (double)Web3.Convert.FromWei(await trader.web3.Eth.GetBalance.SendRequestAsync(trader.account.Address));
                        if (traderETH < 0.1)
                        {
                            Console.WriteLine("Skipping buying with " + trader.name + " because they're a poor");
                        }
                        else
                        {
                            double buyAmount = 0;
                            do
                            {
                                buyAmount = random.NextDouble() * traderETH;
                            } while (buyAmount >= (traderETH * 0.8) || buyAmount > 1.5);

                            var expectedBuy = await GetTokensOut(trader.web3, trader.account, buyAmount);
                            Console.WriteLine(trader.name + " / " + " buying " + expectedBuy + " for " + buyAmount + " ETH / current holding: " + trader.tokens);
                            trader.tokens = await Buy(trader.web3, trader.account, buyAmount, 0, trader.name);
                            var variance = 1 - (double)(trader.tokens - tokensBefore) / ((double)expectedBuy);

                            var wallet1TOKENafter = await GetBalance(wallet1Web3, wallet1Account);
                            var wallet1tokenDiff = wallet1TOKENafter - wallet1TOKENbefore;

                            randoBuyVolume += expectedBuy;
                            totalETHvolume += Web3.Convert.ToWei(buyAmount);

                            /*
                            var holderBals = (double)(await GetSolHolderTotal()) / Math.Pow(10, decimals);
                            var holderBalsIter = (double)(await GetSolHolderIterativeTotal()) / Math.Pow(10, decimals);
                            var holderVariance = holderBals - holderBalsIter;
                            */

                            trader.tokens = await GetBalance(trader.web3, trader.account);

                            string logEntry = i + " BUY - " + trader.name + " - " + buyAmount + " - " + trader.tokens + " (ex " + expectedBuy + ") var " + variance.ToString("F4");
                            string stateEntry = "   devWallet " + wallet1tokenDiff;
                            Console.WriteLine(logEntry);
                            Console.WriteLine(stateEntry);
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        await Approve(trader.web3, contractAddr);
                        BigInteger sellAmount = 0;
                        trader.tokens = await GetBalance(trader.web3, trader.account);
                        if (random.NextDouble() > 0.5) sellAmount = trader.tokens / 2;
                        else sellAmount = trader.tokens;
                        try
                        {
                            var expectedSell = await GetWethOut(trader.web3, trader.account, sellAmount);
                            Console.WriteLine(trader.name + " / " + "selling " + sellAmount + " of " + trader.tokens);
                            var actualSell = await Sell(trader.web3, trader.account, sellAmount, 0, trader.name);
                            var variance = 1 - ((double)actualSell / (double)expectedSell);

                            var wallet1TOKENafter = await GetBalance(wallet1Web3, wallet1Account);
                            var wallet1tokenDiff = wallet1TOKENafter - wallet1TOKENbefore;

                            /*
                            var holderBals = (double)(await GetSolHolderTotal()) / Math.Pow(10, decimals);
                            var holderBalsIter = (double)(await GetSolHolderIterativeTotal()) / Math.Pow(10, decimals);
                            var holderVariance = holderBals - holderBalsIter;
                            */

                            randoSellVolume += sellAmount;
                            totalETHvolume += (BigInteger)actualSell;

                            trader.tokens = await GetBalance(trader.web3, trader.account);

                            string logEntry = i + " SELL - " + trader.name + " - " + sellAmount.ToString("F4") + " out of " + trader.tokens.ToString("F4") + " - " + actualSell.Value.ToString("F4") + " (ex " + expectedSell.ToString("F4") + ") var " + variance.ToString("F4");
                            string stateEntry = "   devWallet " + wallet1tokenDiff;
                            Console.WriteLine(logEntry);
                            Console.WriteLine(stateEntry);
                            Console.WriteLine();
                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e);
                            Console.WriteLine("TRANSFER FAIL - CHECK HH");
                            Console.ReadLine();
                        }
                    }
                }
            }
            //Console.ReadLine();
        }

        static async Task SellOff(List<RandomTrader> traderList)
        {
            foreach (RandomTrader trader in traderList)
            {
                trader.tokens = await GetBalance(trader.web3, trader.account);
                if (trader.tokens >= new BigInteger(100 * Math.Pow(10, decimals)))
                {
                    if (random.NextDouble() <= 0.9)
                    {
                        await Approve(trader.web3, contractAddr);
                        var wallet1TOKENbefore = await GetBalance(wallet1Web3, wallet1Account);
                        var sellAmount = trader.tokens;
                        try
                        {
                            var expectedSell = await GetWethOut(trader.web3, trader.account, sellAmount);
                            Console.WriteLine(trader.name + " / " + "selling " + sellAmount + " of " + trader.tokens);
                            var actualSell = await Sell(trader.web3, trader.account, sellAmount, 0, trader.name);
                            var variance = 1 - ((double)actualSell / (double)expectedSell);

                            var wallet1TOKENafter = await GetBalance(wallet1Web3, wallet1Account);
                            var wallet1tokenDiff = wallet1TOKENafter - wallet1TOKENbefore;

                            /*
                            var holderBals = (double)(await GetSolHolderTotal()) / Math.Pow(10, decimals);
                            var holderBalsIter = (double)(await GetSolHolderIterativeTotal()) / Math.Pow(10, decimals);
                            var holderVariance = holderBals - holderBalsIter;
                            */

                            randoSellVolume += sellAmount;
                            totalETHvolume += (BigInteger)actualSell;

                            trader.tokens = await GetBalance(trader.web3, trader.account);

                            string logEntry = " SELL - " + trader.name + " - " + sellAmount.ToString("F4") + " out of " + trader.tokens.ToString("F4") + " - " + actualSell.Value.ToString("F4") + " (ex " + expectedSell.ToString("F4") + ") var " + variance.ToString("F4");
                            string stateEntry = "   devWallet " + wallet1tokenDiff;
                            Console.WriteLine(logEntry);
                            Console.WriteLine(stateEntry);
                            Console.WriteLine();

                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e);
                            Console.WriteLine("TRANSFER FAIL - CHECK HH");
                            Console.ReadLine();
                        }
                    }
                }
            }
        }

        /*
        static async Task LockedAndLoaded(BigInteger blockDelay)
        {
            var lockHandler = wallet1Web3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.WEAPON.ContractDefinition.LockedAndLoadedFunction>();
            var locked = new MwStakingSol.Contracts.WEAPON.ContractDefinition.LockedAndLoadedFunction()
            {
                TxLimit = blockDelay
            };

            await lockHandler.SendRequestAndWaitForReceiptAsync(contractAddr, locked );
        }
        */

        static async Task ToggleStaking()
        {
            var stakingH = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.WEAPON.ContractDefinition.ToggleStakingFunction>();
            await stakingH.SendRequestAndWaitForReceiptAsync(contractAddr);

            /*
            var stakingH2 = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.MWStaking.ContractDefinition.ToggleStakingFunction>();
            await stakingH2.SendRequestAndWaitForReceiptAsync(stakingAddr);
            */
        }

        static async Task SetStakingContract(string addr)
        {
            var setStakingH = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.WEAPON.ContractDefinition.SetStakingContractFunction>();
            var setSta = new MwStakingSol.Contracts.WEAPON.ContractDefinition.SetStakingContractFunction()
            {
                Addr = addr
            };

            await setStakingH.SendRequestAndWaitForReceiptAsync(contractAddr, setSta);
        }

        static async Task Stake(Web3 web3, Account account, BigInteger amount, BigInteger unstakeTime)
        {
            var stakeH = web3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.MWStaking.ContractDefinition.StakeFunction>();
            var stake = new MwStakingSol.Contracts.MWStaking.ContractDefinition.StakeFunction()
            {
                Account = account.Address,
                Amount = amount,
                UnstakeTime = unstakeTime,
                AdjustedStake = 0
            };

            await stakeH.SendRequestAndWaitForReceiptAsync(stakingAddr, stake);
        }

        static async Task Unstake(Web3 web3, Account account, BigInteger unstakeAmount)
        {
            var unstakeH = web3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.MWStaking.ContractDefinition.UnstakeFunction>();
            var unstake = new MwStakingSol.Contracts.MWStaking.ContractDefinition.UnstakeFunction()
            {
                Account = account.Address,
                UnstakeAmount = unstakeAmount
            };

            await unstakeH.SendRequestAndWaitForReceiptAsync(stakingAddr, unstake);
        }

        static async Task<string> GetStakeString(string addr)
        {
            var dto = await weaponservice.GetStakeQueryAsync(addr);
            var stakeString = DropTokenDec(dto.ReturnValue1).ToString() + ", begin: " + dto.ReturnValue2 + ", end: " + dto.ReturnValue3;
            return stakeString;
        }

        static async Task ClaimMenu()
        {
            Web3 claimWalletWeb3 = deployerWeb3;
            List<BigInteger> epochsToClaim = new List<BigInteger>();

            Console.Write("Enter wallet to claim: ");
            var walletNum = Console.ReadKey(true);
            Console.WriteLine();
            switch (walletNum.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Claiming with 1");
                    claimWalletWeb3 = wallet1Web3;
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("Claiming with 2");
                    claimWalletWeb3 = wallet2Web3;
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("Claiming with 3");
                    claimWalletWeb3 = wallet3Web3;
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("Claiming with 4");
                    claimWalletWeb3 = wallet4Web3;
                    break;
                case ConsoleKey.D5:
                    Console.WriteLine("Claiming with 5");
                    claimWalletWeb3 = wallet5Web3;
                    break;
                case ConsoleKey.D6:
                    Console.WriteLine("Claiming with 6");
                    claimWalletWeb3 = wallet6Web3;
                    break;
                case ConsoleKey.D7:
                    Console.WriteLine("Claiming with 7");
                    claimWalletWeb3 = wallet7Web3;
                    break;
                default:
                    break;
            }
            
            bool done = false;
            do
            {
                Console.Write("Claiming: ");
                foreach (BigInteger bint in epochsToClaim)
                {
                    Console.Write(bint + " ");
                }
                Console.WriteLine();
                Console.Write("Enter epoch to claim: ");
                var epKey = Console.ReadKey(true);
                //Console.WriteLine();
                switch (epKey.Key)
                {
                    case ConsoleKey.D0:
                        epochsToClaim.Add(new BigInteger(0));
                        break;
                    case ConsoleKey.D1:
                        epochsToClaim.Add(new BigInteger(1));
                        break;
                    case ConsoleKey.D2:
                        epochsToClaim.Add(new BigInteger(2));
                        break;
                    case ConsoleKey.D3:
                        epochsToClaim.Add(new BigInteger(3));
                        break;
                    case ConsoleKey.D4:
                        epochsToClaim.Add(new BigInteger(4));
                        break;
                    case ConsoleKey.D5:
                        epochsToClaim.Add(new BigInteger(5));
                        break;
                    case ConsoleKey.D6:
                        epochsToClaim.Add(new BigInteger(6));
                        break;
                    case ConsoleKey.D7:
                        epochsToClaim.Add(new BigInteger(7));
                        break;
                    case ConsoleKey.D8:
                        epochsToClaim.Add(new BigInteger(8));
                        break;
                    case ConsoleKey.D9:
                        epochsToClaim.Add(new BigInteger(9));
                        break;
                    default:
                        done = true;
                        break;
                }

            } while (!done);

            if (claimWalletWeb3 != deployerWeb3 && epochsToClaim.Count > 0)
            {
                Console.WriteLine("Claiming...");
                await Claim(claimWalletWeb3, epochsToClaim);
                Console.WriteLine("Claim attempted, press any key...");
                Console.ReadKey();
            }
        }

        static async Task Claim(Web3 web3, List<BigInteger> epochs)
        {
            var claimH = web3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.MWStaking.ContractDefinition.ClaimFunction>();
            var claim = new MwStakingSol.Contracts.MWStaking.ContractDefinition.ClaimFunction()
            {
                Epochs = epochs
            };

            await claimH.SendRequestAndWaitForReceiptAsync(stakingAddr, claim);
        }

        static async Task SetPoolSize()
        {
            var currentEpoch = await stakingservice.CurrentEpochQueryAsync();
            var currentEpochStats = await stakingservice.GetEpochQueryAsync(currentEpoch);

            BigInteger poolsize = new BigInteger(0);
            foreach (Account account in allAccounts)
            {
                var dto = await weaponservice.GetStakeQueryAsync(account.Address);
                if (dto.ReturnValue2 <= currentEpochStats.EpochStartDate + 86400 && dto.ReturnValue3 >= currentEpochStats.EpochStartDate + 604800)
                    poolsize += dto.ReturnValue1;
            }

            var setPoolSizeH = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.MWStaking.ContractDefinition.SetPoolSizeFunction>();
            var setP = new MwStakingSol.Contracts.MWStaking.ContractDefinition.SetPoolSizeFunction()
            {
                PoolSize = poolsize
            };

            await setPoolSizeH.SendRequestAndWaitForReceiptAsync(stakingAddr, setP);
        }

        static async Task Disperse()
        {
            var dispAddr = "0xD152f549545093347A162Dce210e7293f1452150";

            var holderList = new List<string>()
            {
                "0xbc2d9a1c530006c51f708478961bead35746cc0f",
                "0x8981cc81ec9d58da52ed779fb7d8181e03bc8ce3",
                "0x3d61d3da40a372ca317832717d7e6205cedb439b",
                "0x2f77daee392b7870f6087b57f2e7f1568785f168",
                "0x4ec77164b5a7306b26b085d188770a6231e4c6c8",
                "0xea747056c4a5d2a8398ec64425989ebf099733e9",
                "0xf8905baf83728e2e7a53bfc1dae345212fa315a1",
                "0xca8b73101e12c03e4e9eacf0e180fc10edf859e2",
                "0x1739ea86883295e4d49be38c26e58f2fc18edbdb",
                "0xf0843b54c5d9e78a47fafaa8f6c6a9a852f03745",
                "0x4452c23102ec7f36c9a49780ce8325775ba422f0",
                "0xefcbece0bc0b18ced69466f12d3044bd11a517cc",
                "0xe7c6e3cf3dbd6312cdd40e711edecce5090301be",
                "0x921c0b27fa2eeca7cbca305cca5cf2dac9de7e15",
                "0x782ea93d5bdab7775517f2fe448fcc8cb8680e28",
                "0x8c0c23f3309254653ddfb21c6d40785832cddc85",
                "0x8643e1db24f46c6e6a77ea7434c5efa611f48c8a",
                "0xf818acec29861228630df8361a611e0f88302958",
                "0xd7b8a034fc7990519b1198269ac98ab572b1a0dc",
                "0xd977a692e458be39d33a28f43b734ed5c167c926",
                "0xfca399b892f4e8306fc31b312a3399f422976886",
                "0x14439dbe3eacf79d66d11d866a38fff52fe67fac",
                "0x1945578febab92a095b510d1ab483e26b59c696e",
                "0xc70b35a93ed370607d0887f138ef893117fdead6",
                "0xb0e8f66e4ce09c0c108bc96fa47beb2041b7dd2b",
                "0x39d33d17c9a421ea99b1d3a97dce6fb6faa679c6",
                "0x1bf01c70f721c2bb5aee4dbabb6dea05a6f844fb",
                "0xb3e02431d6f566b927fe008877552510ef59ce47",
                "0xd26be75ef4d44155b0db1224d4b2a293f4642370",
                "0xc18c96292415514da42b6a0c40e60c5c3036aed4",
                "0x7dcb01ac133c7f8591a1f2bf5c151cbc5567b422",
                "0xa96beb4610a32e4544b59822c6a62bf070150989",
                "0xc8cc96d0d7087bf6bcb7f35d80c7634a00030799",
                "0x7768fbc67afecf2b4caee9d1841ad14637d13652",
                "0xe94ca918451df24481a9a5cdcb9534bc6da2d59f",
                "0x689ccdaa11e5ac7d3fb2a45fbb4e26bbd90fe39a",
                "0x65b1b96bd01926d3d60dd3c8bc452f22819443a9",
                "0x2b64d05172e89899edfa6b3f5efa505bb46f515d",
                "0xc01dcc6676554feab4bb355c989c01001135b986",
                "0xcfdf2aa46e48d4e661b47d4242f18555a6a77212",
                "0x74ac42da69f3d3610686cb08bc1b07c3f1b16b89",
                "0xaae654404a9370a2cce99af14e6f9e4b206e0f36",
                "0x3874b75d48cdc657daf90d4d7e837dca534dadd4",
                "0xf84d250a95faf027bea196caaf6058c43fe837e4",
                "0x2e83794ea9eabe082f15754f95f64b9025186123",
                "0x6ac792f09a58f50452f18c016556e5c03de65d90",
                "0x8f43e4a1716ed17468b3085f29e2c577b43d5291",
                "0x2dbed4ca7fd6f79e0d2ce3eac127a2e07b751cfa",
                "0x1005d33260571d7270139edb0388ab2b40975fd9",
                "0xa577f2d1e6980f61f21da0ed9b1e4e7d83b219b4",
                "0x8dee3bcc1308953588eb4b2945463506c390b208",
                "0x86c994904dd138bdc8462ac06edd919ad70fe575",
                "0xd793b8b37f1d62ba601ea6156f16489e22a24a11",
                "0xfb8578ba5ba33c0aadafe1b0454fab15417ee74a",
                "0x48f67619aef0f9117a589b9a8a0d0e6562585bf2",
                "0x0d74675b1af25a6dc9e69cb401e05ee07febe87a",
                "0xfd4564033b4c7cf43a81d20301863fcb5026a4f2",
                "0x6d2ab5d960ac0813f1530568b8244dd92779ec15",
                "0x2a720ac50346f6a6f930ddb955a17a863299b37d",
                "0x39cc23e8b2e829a5d6b340e8a738fb2965c211f2",
                "0x374e9beb7d5a15e3f4bba0408388df66ad2e6254",
                "0x8069d3705f6055bdf11024fd8bd22d98aec61e13",
                "0xeb21302586d884ac2fac1af80658dd5f30edbf27",
                "0xbac9a94c5e73574481bda81a6a7ee06cb92181ae",
                "0x708140b348c5f21bc763e98f1bd04ad80e062ffb",
                "0xb3f3658bf332ba6c9c0cc5bc1201caba7ada819b",
                "0x74d42547c0194650fdc76913614b7add7d5dca5f",
                "0xa655335b71c6db61d81991c5e6076a59d8d6660a",
                "0x8f771fcba45639a80edf8b295c7723810e5b875f",
                "0xfd205eaf047fbcbe6c930b9fa5c947ccb0166a42",
                "0x099ec0d7e419a89162780320f047c00fca867b74",
                "0x68f8a181c1158d33c2c0e65d33e3a4791002fc50",
                "0x62515a5891197931f70dbe714c7ec7bc8624f006",
                "0x18cfabf77661bd05c1c4f916b754eae4ed436a21",
                "0x7ea7508435ddcf0ff43939c1ac30ca634e8ac146",
                "0x80d88dfbdf46ab7df6d12c1c9429a960ed271c0c",
                "0x92469b32136885de92ede9013929f3a69a35735a",
                "0x559919e2285367b6f0abc2e6b97feb6c7402fa3b",
                "0x6dc9669d5dc121c582d564a30e3023ec69f570e7",
                "0xd34112c9311b8438239fa1ff1708a0b71c99f4cc",
                "0x704568756dc0b837fabe44fe56f5b62b1038ae8c",
                "0x7100f8093dac47e6b87abfb2c539caaa7cb50853",
                "0x519b324a70897c16aab474814556254fa671ec63",
                "0x24db6f5e2a6c4af50079327c5eb90773ff0a2c8d",
                "0x60087224a2514c8f3c542a75796064bc7522f432",
                "0xd5253d995ed50267500fbc9f90f3752f00aaa9bc",
                "0xd031c075864273404c31087993bce38777e9d3a2",
                "0x3ccf2830441b84194bc15ae63c851e5f0279b1e1",
                "0x28af3657536e74991d2996e6c20b9d5530b28421",
                "0x802ab638a70ccdd0cebd5dd1a6477ce859041a97",
                "0x1585a4b86b823667dc35f22fb64ec25f77b8da9a",
                "0xa19be048250c74bcbe3dc74abe4709ef5a4da722",
                "0xefbc0d80acfae9e5f90ccae9dc08ebbe68195560",
                "0x072027dc985cbd3eb50aabeab11b2a20a05d1430",
                "0xe1afeadf02b5ffd559f7914c014e94891c20f613",
                "0x2f14084c86f19051a22dd320520057d66d2884c1",
                "0x9ee61c4b678e7a39a2f6fc6161118a8eef8ecfd5",
                "0x58cbf5faf45002ea42af4597fb4fdd690d992d93",
                "0x41bc521907d7f91a9c561b603cdc2daf0ebbcfbd",
                "0xf31ccfa657abeab1af2516767891d10b54dc4ecc",
            };

            var amountList = new List<BigInteger>()
            {
                200000000000000,
                195816265021746,
                180000000000000,
                137523968422930,
                125014349036691,
                122500000000000,
                120023068000000,
                112739018048019,
                111114227126390,
                111000600000000,
                110011695770695,
                108236605440916,
                103794039528794,
                101505420325621,
                100915951387140,
                100000000000000,
                98846138583724,
                98203841471029,
                95000000000000,
                93021484917926,
                92973654330179,
                92965482566769,
                87003156171229,
                82373027784785,
                81562406784995,
                81353966543258,
                79003104874233,
                78041438890123,
                77725380000000,
                76202592133260,
                72000000000000,
                70911730735364,
                70054281540369,
                65890691310671,
                62533494740256,
                62244355822901,
                61607215751622,
                59489095480641,
                59121425285346,
                56804034853799,
                55047625349251,
                55007253315906,
                50651694117641,
                50242763883426,
                50096541635488,
                50000158674788,
                50000000000000,
                50000000000000,
                50000000000000,
                49144415585287,
                48307670974201,
                47750707337254,
                46384902547292,
                46331365388811,
                45709451543750,
                45000000000000,
                45000000000000,
                45000000000000,
                45000000000000,
                44640000000000,
                42078974676276,
                42076855994718,
                41646351536694,
                40707514921621,
                40655108036106,
                40000000000000,
                39500000000000,
                38754878152556,
                38700000000000,
                38444721847972,
                38366696517468,
                38022109896997,
                37784322609899,
                36527468746303,
                34799075664119,
                33779016822173,
                33539058861367,
                30207882218517,
                30000000000000,
                29121907407593,
                29014218351445,
                27994791901543,
                26680198579396,
                26097783794466,
                26000897581946,
                25959155533447,
                25791000000000,
                25456182697443,
                25274793792776,
                24541444634815,
                23798768033965,
                23785783069690,
                23239249166799,
                23072012236925,
                23040000000000,
                23002768555586,
                22954076917474,
                22480200000000,
                22089595929159,
                21940405336477,
            };

            var total = new BigInteger(0);
            foreach (BigInteger amount in amountList)
                total += amount;
            var approveH = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.WEAPON.ContractDefinition.ApproveFunction>();
            var approve = new MwStakingSol.Contracts.WEAPON.ContractDefinition.ApproveFunction()
            {
                Amount = total,
                Spender = dispAddr
            };

            await approveH.SendRequestAndWaitForReceiptAsync(contractAddr, approve);

            var dispH = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.Disperse.ContractDefinition.DisperseTokenSimpleFunction>();
            var disp = new MwStakingSol.Contracts.Disperse.ContractDefinition.DisperseTokenSimpleFunction()
            {
                Token = contractAddr,
                Recipients = holderList,
                Values = amountList
            };
            
            Nethereum.JsonRpc.Client.ClientBase.ConnectionTimeout = new TimeSpan(0, 0, 120);
            await dispH.SendRequestAndWaitForReceiptAsync(dispAddr, disp);

            foreach(string holder in holderList)
            {
                Console.WriteLine(await weaponservice.BalanceOfQueryAsync(holder));
            }
            Console.ReadKey();
        }

        /*
        static async Task CheckHashes()
        {
            var encode = new ABIEncode();
            var byteEncode = new Nethereum.ABI.Encoders.Bytes32TypeEncoder();
            var domainObj = new Domain()
            {
                Name = "MEGAWEAPON Staking",
                Version = "1",
                ChainId = new BigInteger(31337),
                VerifyingContract = stakingAddr
            };
            var domain = encode.GetSha3ABIEncodedPacked(
                encode.GetABIEncoded(
                    encode.GetSha3ABIEncodedPacked("EIP712Domain(string name,string version,uint256 chainId,address verifyingContract)"),
                    encode.GetSha3ABIEncodedPacked(Encoding.UTF8.GetBytes("MEGAWEAPON Staking")),
                    encode.GetSha3ABIEncodedPacked(Encoding.UTF8.GetBytes("1")),
                    new BigInteger(31337),
                    stakingAddr
                ));
            Console.WriteLine(stakingAddr);
            var stakehash = encode.GetSha3ABIEncodedPacked("stake(uint256 amount,uint256 unstakeTime,uint256 adjustedStake,uint256 nonce,uint256 expiry,bool allowed)");
            var unstakehash = encode.GetSha3ABIEncodedPacked("unstake(uint256 unstakeAmount,uint256 adjustedStake,uint256 nonce,uint256 expiry,bool allowed)");
            var synchash = encode.GetSha3ABIEncodedPacked("sync(uint256 adjustedStake,uint256 nonce,uint256 expiry,bool allowed)");
            var claimplayerhash = encode.GetSha3ABIEncodedPacked("claimPlayer(uint256[] epochs, uint256[] amounts, uint256 nonce, uint256 expiry, bool allowed)");

            var results = await stakingservice.CheckTypeHashesQueryAsync(new MwStakingSol.Contracts.MWStaking.ContractDefinition.CheckTypeHashesFunction() { Domain = domain, Stakehash = stakehash, Unstakehash = unstakehash, Claimhash = claimplayerhash, Synchash = synchash });
            Console.WriteLine(results.ReturnValue1.ToString() + results.ReturnValue2.ToString() + results.ReturnValue3.ToString() + results.ReturnValue4.ToString() + results.ReturnValue5.ToString());
            Console.WriteLine(results.ReturnValue6);
            
        }

        static async Task TestSync()
        {
            var domainObj = new Domain()
            {
                Name = "MEGAWEAPON Staking",
                Version = "1",
                ChainId = new BigInteger(31337),
                VerifyingContract = stakingAddr
            };
            var typedData = new TypedData
            {
                Domain = domainObj,
                Types = new Dictionary<string, MemberDescription[]>
                {
                    ["EIP712Domain"] = new[]
                    {
                        new MemberDescription {Name = "name", Type = "string"},
                        new MemberDescription {Name = "version", Type = "string"},
                        new MemberDescription {Name = "chainId", Type = "uint256"},
                        new MemberDescription {Name = "verifyingContract", Type = "address"},
                    },
                    ["sync"] = new[]
                    {
                        new MemberDescription() {Name = "adjustedStake", Type = "uint256" },
                        new MemberDescription() {Name = "nonce", Type = "uint256" },
                        new MemberDescription() {Name = "expiry", Type = "uint256" },
                        new MemberDescription() {Name = "allowed", Type = "bool" }
                    }
                },
                PrimaryType = "sync",
                Message = new[]
                {
                    
                    new MemberValue {TypeName = "uint256", Value = new BigInteger(1) },
                    new MemberValue {TypeName = "uint256", Value = new BigInteger(1) },
                    new MemberValue {TypeName = "uint256", Value = new BigInteger(1) },
                    new MemberValue {TypeName = "bool", Value = true }
                   
                }
                
            };
            var ethECKey = new EthECKey(deployerKey);
            var hashedData = Sha3Keccack.Current.CalculateHash(Eip712TypedDataSigner.Current.EncodeTypedData(typedData));
            var signedData = ethECKey.SignAndCalculateV(hashedData);
            //var signedData = Eip712TypedDataSigner.Current.SignTypedData(typedData, new Nethereum.Signer.EthECKey(deployerKey));
            //Console.WriteLine(signedData);
            //var parsedSig = ParseSignature(signedData.ToString());
            //Console.WriteLine("R: " + parsedSig.R);
            //Console.WriteLine("S: " + parsedSig.S);
            //Console.WriteLine("V: " + parsedSig.V);

            var encoder = new ABIEncode();
            var syncTestH = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.MWStaking.ContractDefinition.SyncFunction>();
            var syncTest = new MwStakingSol.Contracts.MWStaking.ContractDefinition.SyncFunction()
            {
                AdjustedStake = new BigInteger(1),
                Nonce = new BigInteger(1),
                Expiry = new BigInteger(1),
                Allowed = true,
                V = signedData.V[0],
                R = signedData.R,
                S = signedData.S
            };
            Console.WriteLine("R byte[] length: " + signedData.R.Length);
            Console.WriteLine("R: " + HexByteConvertorExtensions.ToHex(signedData.R));
            Console.WriteLine("S byte[] length: " + signedData.S.Length);
            Console.WriteLine("S: " + HexByteConvertorExtensions.ToHex(signedData.S));

            var result = await syncTestH.SendRequestAndWaitForReceiptAsync(stakingAddr, syncTest);
            
        }

        static async Task TestClaimPlayer()
        {
            var domainObj = new Domain()
            {
                Name = "MEGAWEAPON Staking",
                Version = "1",
                ChainId = new BigInteger(31337),
                VerifyingContract = stakingAddr
            };
            var typedData = new TypedData
            {
                Domain = domainObj,
                Types = new Dictionary<string, MemberDescription[]>
                {
                    ["EIP712Domain"] = new[]
                    {
                        new MemberDescription {Name = "name", Type = "string"},
                        new MemberDescription {Name = "version", Type = "string"},
                        new MemberDescription {Name = "chainId", Type = "uint256"},
                        new MemberDescription {Name = "verifyingContract", Type = "address"},
                    },
                    ["claimPlayer"] = new[]
                    {
                        new MemberDescription() {Name = "epochs", Type = "uint256[]" },
                        new MemberDescription() {Name = "amounts", Type = "uint256[]"},
                        new MemberDescription() {Name = "nonce", Type = "uint256" },
                        new MemberDescription() {Name = "expiry", Type = "uint256" },
                        new MemberDescription() {Name = "allowed", Type = "bool" }
                    }
                },
                PrimaryType = "claimPlayer",
                Message = new[]
                {

                    new MemberValue {TypeName = "uint256[]", Value = new List<BigInteger>()
                    {
                        new BigInteger(1),
                        new BigInteger(2)
                    }},
                    new MemberValue {TypeName = "uint256[]", Value = new List<BigInteger>()
                    {
                        new BigInteger(1),
                        new BigInteger(2)
                    }},
                    new MemberValue {TypeName = "uint256", Value = new BigInteger(1) },
                    new MemberValue {TypeName = "uint256", Value = new BigInteger(1) },
                    new MemberValue {TypeName = "bool", Value = true }

                }

            };
            var ethECKey = new EthECKey(deployerKey);
            var hashedData = Sha3Keccack.Current.CalculateHash(Eip712TypedDataSigner.Current.EncodeTypedData(typedData));
            var signedData = ethECKey.SignAndCalculateV(hashedData);
            //var signedData = Eip712TypedDataSigner.Current.SignTypedData(typedData, new Nethereum.Signer.EthECKey(deployerKey));
            //Console.WriteLine(signedData);
            //var parsedSig = ParseSignature(signedData.ToString());
            //Console.WriteLine("R: " + parsedSig.R);
            //Console.WriteLine("S: " + parsedSig.S);
            //Console.WriteLine("V: " + parsedSig.V);

            var encoder = new ABIEncode();
            var claimTestH = deployerWeb3.Eth.GetContractTransactionHandler<MwStakingSol.Contracts.MWStaking.ContractDefinition.ClaimPlayerFunction>();
            var syncTest = new MwStakingSol.Contracts.MWStaking.ContractDefinition.ClaimPlayerFunction()
            {
                Epochs = new List<BigInteger>() { new BigInteger(1), new BigInteger(2) },
                Amounts = new List<BigInteger>() { new BigInteger(1), new BigInteger(2) },
                Nonce = new BigInteger(1),
                Expiry = new BigInteger(1),
                Allowed = true,
                V = signedData.V[0],
                R = signedData.R,
                S = signedData.S
            };
            Console.WriteLine("R byte[] length: " + signedData.R.Length);
            Console.WriteLine("R: " + HexByteConvertorExtensions.ToHex(signedData.R));
            Console.WriteLine("S byte[] length: " + signedData.S.Length);
            Console.WriteLine("S: " + HexByteConvertorExtensions.ToHex(signedData.S));

            var result = await claimTestH.SendRequestAndWaitForReceiptAsync(stakingAddr, syncTest);

        }

        private static Signature ParseSignature(string fullSig)
        {
            if (fullSig.StartsWith("0x"))
                fullSig = fullSig[2..];
            return new Signature
            {
                R = "0x" + fullSig.Substring(0, 64),
                S = "0x" + fullSig.Substring(64, 64),
                V = Convert.ToByte(fullSig.Substring(128, 2), 16)
            };
        }
        */

        /*
        static async Task<Bytes32Type> GenerateStakeSig()
        {
            var domain = new Domain()
            {
                Name = "MEGAWEAPON Staking",
                Version = "1",
                ChainId = new BigInteger(31337),
                VerifyingContract = contractAddr
            };
            Eip712TypedDataSigner.Current.SignTypedData(new TypedData()
            {
                Domain = domain,
                PrimaryType = "bytes32",

            });
        }
        */

        
        static async Task HHReset()
        {
            var resetClient = new HttpClient();
            //string queryPayload = @"{""jsonrpc"": ""2.0"", ""method"":""hardhat_reset"", ""params"": [{""forking"": {""jsonRpcUrl"": ""http://192.168.1.22:8544""}}], ""id"": 1}";
            //string queryPayload = @"{""jsonrpc"": ""2.0"", ""method"":""hardhat_reset"", ""params"": [{""forking"": {""jsonRpcUrl"": ""https://mainnet.infura.io/v3/fa27fdb306874e9498bcd23a25ddc299""}}], ""id"": 1}";
            string queryPayload = @"{""jsonrpc"": ""2.0"", ""method"":""hardhat_reset"", ""params"": [{""forking"": {""jsonRpcUrl"": ""https://eth-mainnet.alchemyapi.io/v2/0yNiPKsyWCt6TW-Fl7PaYpkZDKoN2JcV""}}], ""id"": 1}";
            var content = new StringContent(queryPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await resetClient.PostAsync(new Uri(httpsEndpoint), content);
            //if (debugLevel > 0) Console.WriteLine("HH Reset response: " + response.Content.ReadAsStringAsync().Result);
            resetClient.Dispose();
            return;
        }

        static async Task HHMine(int delay)
        {
            Thread.Sleep(delay);
            var resetClient = new HttpClient();
            string queryPayload = @"{""jsonrpc"": ""2.0"", ""method"":""evm_mine"", ""params"": [], ""id"": 1}";
            var content = new StringContent(queryPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await resetClient.PostAsync(httpsEndpoint, content);
            //if (debugLevel > 0) Console.WriteLine("HH Reset response: " + response.Content.ReadAsStringAsync().Result);
            resetClient.Dispose();
            return;
        }

        static async Task HHSetTime(int time)
        {

            var resetClient = new HttpClient();
            string queryPayload = @"{""jsonrpc"": ""2.0"", ""method"":""evm_setNextBlockTimestamp"", ""params"": [" + time + @"], ""id"": 1}";
            var content = new StringContent(queryPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await resetClient.PostAsync(httpsEndpoint, content);
            //if (debugLevel > 0) Console.WriteLine("HH Reset response: " + response.Content.ReadAsStringAsync().Result);
            resetClient.Dispose();
            return;
        }
    }


}
