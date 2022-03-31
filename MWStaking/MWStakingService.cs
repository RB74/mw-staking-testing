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
using MwStakingSol.Contracts.MWStaking.ContractDefinition;

namespace MwStakingSol.Contracts.MWStaking
{
    public partial class MWStakingService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, MWStakingDeployment mWStakingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MWStakingDeployment>().SendRequestAndWaitForReceiptAsync(mWStakingDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, MWStakingDeployment mWStakingDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MWStakingDeployment>().SendRequestAsync(mWStakingDeployment);
        }

        public static async Task<MWStakingService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, MWStakingDeployment mWStakingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, mWStakingDeployment, cancellationTokenSource);
            return new MWStakingService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public MWStakingService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddEpochsRequestAsync(AddEpochsFunction addEpochsFunction)
        {
             return ContractHandler.SendRequestAsync(addEpochsFunction);
        }

        public Task<TransactionReceipt> AddEpochsRequestAndWaitForReceiptAsync(AddEpochsFunction addEpochsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addEpochsFunction, cancellationToken);
        }

        public Task<string> AddEpochsRequestAsync(BigInteger firstAdd, BigInteger numToAdd)
        {
            var addEpochsFunction = new AddEpochsFunction();
                addEpochsFunction.FirstAdd = firstAdd;
                addEpochsFunction.NumToAdd = numToAdd;
            
             return ContractHandler.SendRequestAsync(addEpochsFunction);
        }

        public Task<TransactionReceipt> AddEpochsRequestAndWaitForReceiptAsync(BigInteger firstAdd, BigInteger numToAdd, CancellationTokenSource cancellationToken = null)
        {
            var addEpochsFunction = new AddEpochsFunction();
                addEpochsFunction.FirstAdd = firstAdd;
                addEpochsFunction.NumToAdd = numToAdd;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addEpochsFunction, cancellationToken);
        }

        public Task<string> BecomePlayerRequestAsync(BecomePlayerFunction becomePlayerFunction)
        {
             return ContractHandler.SendRequestAsync(becomePlayerFunction);
        }

        public Task<string> BecomePlayerRequestAsync()
        {
             return ContractHandler.SendRequestAsync<BecomePlayerFunction>();
        }

        public Task<TransactionReceipt> BecomePlayerRequestAndWaitForReceiptAsync(BecomePlayerFunction becomePlayerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(becomePlayerFunction, cancellationToken);
        }

        public Task<TransactionReceipt> BecomePlayerRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<BecomePlayerFunction>(null, cancellationToken);
        }

        public Task<string> ClaimRequestAsync(ClaimFunction claimFunction)
        {
             return ContractHandler.SendRequestAsync(claimFunction);
        }

        public Task<TransactionReceipt> ClaimRequestAndWaitForReceiptAsync(ClaimFunction claimFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimFunction, cancellationToken);
        }

        public Task<string> ClaimRequestAsync(List<BigInteger> epochs)
        {
            var claimFunction = new ClaimFunction();
                claimFunction.Epochs = epochs;
            
             return ContractHandler.SendRequestAsync(claimFunction);
        }

        public Task<TransactionReceipt> ClaimRequestAndWaitForReceiptAsync(List<BigInteger> epochs, CancellationTokenSource cancellationToken = null)
        {
            var claimFunction = new ClaimFunction();
                claimFunction.Epochs = epochs;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimFunction, cancellationToken);
        }

        public Task<string> ClaimPlayerRequestAsync(ClaimPlayerFunction claimPlayerFunction)
        {
             return ContractHandler.SendRequestAsync(claimPlayerFunction);
        }

        public Task<TransactionReceipt> ClaimPlayerRequestAndWaitForReceiptAsync(ClaimPlayerFunction claimPlayerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimPlayerFunction, cancellationToken);
        }

        public Task<string> ClaimPlayerRequestAsync(string account, List<BigInteger> epochs, List<BigInteger> amounts)
        {
            var claimPlayerFunction = new ClaimPlayerFunction();
                claimPlayerFunction.Account = account;
                claimPlayerFunction.Epochs = epochs;
                claimPlayerFunction.Amounts = amounts;
            
             return ContractHandler.SendRequestAsync(claimPlayerFunction);
        }

        public Task<TransactionReceipt> ClaimPlayerRequestAndWaitForReceiptAsync(string account, List<BigInteger> epochs, List<BigInteger> amounts, CancellationTokenSource cancellationToken = null)
        {
            var claimPlayerFunction = new ClaimPlayerFunction();
                claimPlayerFunction.Account = account;
                claimPlayerFunction.Epochs = epochs;
                claimPlayerFunction.Amounts = amounts;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimPlayerFunction, cancellationToken);
        }

        public Task<BigInteger> CurrentEpochQueryAsync(CurrentEpochFunction currentEpochFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CurrentEpochFunction, BigInteger>(currentEpochFunction, blockParameter);
        }

        
        public Task<BigInteger> CurrentEpochQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CurrentEpochFunction, BigInteger>(null, blockParameter);
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

        public Task<GetEpochOutputDTO> GetEpochQueryAsync(GetEpochFunction getEpochFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetEpochFunction, GetEpochOutputDTO>(getEpochFunction, blockParameter);
        }

        public Task<GetEpochOutputDTO> GetEpochQueryAsync(BigInteger epoch, BlockParameter blockParameter = null)
        {
            var getEpochFunction = new GetEpochFunction();
                getEpochFunction.Epoch = epoch;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetEpochFunction, GetEpochOutputDTO>(getEpochFunction, blockParameter);
        }

        public Task<string> GetPlayerContractQueryAsync(GetPlayerContractFunction getPlayerContractFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPlayerContractFunction, string>(getPlayerContractFunction, blockParameter);
        }

        
        public Task<string> GetPlayerContractQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPlayerContractFunction, string>(null, blockParameter);
        }

        public Task<bool> HasClaimedQueryAsync(HasClaimedFunction hasClaimedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HasClaimedFunction, bool>(hasClaimedFunction, blockParameter);
        }

        
        public Task<bool> HasClaimedQueryAsync(string account, BigInteger epoch, BlockParameter blockParameter = null)
        {
            var hasClaimedFunction = new HasClaimedFunction();
                hasClaimedFunction.Account = account;
                hasClaimedFunction.Epoch = epoch;
            
            return ContractHandler.QueryAsync<HasClaimedFunction, bool>(hasClaimedFunction, blockParameter);
        }

        public Task<bool> IsPlayerQueryAsync(IsPlayerFunction isPlayerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsPlayerFunction, bool>(isPlayerFunction, blockParameter);
        }

        
        public Task<bool> IsPlayerQueryAsync(string addr, BlockParameter blockParameter = null)
        {
            var isPlayerFunction = new IsPlayerFunction();
                isPlayerFunction.Addr = addr;
            
            return ContractHandler.QueryAsync<IsPlayerFunction, bool>(isPlayerFunction, blockParameter);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<bool> PlayersAllowedQueryAsync(PlayersAllowedFunction playersAllowedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PlayersAllowedFunction, bool>(playersAllowedFunction, blockParameter);
        }

        
        public Task<bool> PlayersAllowedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PlayersAllowedFunction, bool>(null, blockParameter);
        }

        public Task<string> SetPlayerContractRequestAsync(SetPlayerContractFunction setPlayerContractFunction)
        {
             return ContractHandler.SendRequestAsync(setPlayerContractFunction);
        }

        public Task<TransactionReceipt> SetPlayerContractRequestAndWaitForReceiptAsync(SetPlayerContractFunction setPlayerContractFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPlayerContractFunction, cancellationToken);
        }

        public Task<string> SetPlayerContractRequestAsync(string pContract)
        {
            var setPlayerContractFunction = new SetPlayerContractFunction();
                setPlayerContractFunction.PContract = pContract;
            
             return ContractHandler.SendRequestAsync(setPlayerContractFunction);
        }

        public Task<TransactionReceipt> SetPlayerContractRequestAndWaitForReceiptAsync(string pContract, CancellationTokenSource cancellationToken = null)
        {
            var setPlayerContractFunction = new SetPlayerContractFunction();
                setPlayerContractFunction.PContract = pContract;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPlayerContractFunction, cancellationToken);
        }

        public Task<string> SetPoolSizeRequestAsync(SetPoolSizeFunction setPoolSizeFunction)
        {
             return ContractHandler.SendRequestAsync(setPoolSizeFunction);
        }

        public Task<TransactionReceipt> SetPoolSizeRequestAndWaitForReceiptAsync(SetPoolSizeFunction setPoolSizeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPoolSizeFunction, cancellationToken);
        }

        public Task<string> SetPoolSizeRequestAsync(BigInteger poolSize)
        {
            var setPoolSizeFunction = new SetPoolSizeFunction();
                setPoolSizeFunction.PoolSize = poolSize;
            
             return ContractHandler.SendRequestAsync(setPoolSizeFunction);
        }

        public Task<TransactionReceipt> SetPoolSizeRequestAndWaitForReceiptAsync(BigInteger poolSize, CancellationTokenSource cancellationToken = null)
        {
            var setPoolSizeFunction = new SetPoolSizeFunction();
                setPoolSizeFunction.PoolSize = poolSize;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPoolSizeFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(StakeFunction stakeFunction)
        {
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(StakeFunction stakeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(string account, BigInteger amount, BigInteger unstakeTime, BigInteger adjustedStake)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Account = account;
                stakeFunction.Amount = amount;
                stakeFunction.UnstakeTime = unstakeTime;
                stakeFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(string account, BigInteger amount, BigInteger unstakeTime, BigInteger adjustedStake, CancellationTokenSource cancellationToken = null)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Account = account;
                stakeFunction.Amount = amount;
                stakeFunction.UnstakeTime = unstakeTime;
                stakeFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
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

        public Task<string> TogglePlayersAllowedRequestAsync(TogglePlayersAllowedFunction togglePlayersAllowedFunction)
        {
             return ContractHandler.SendRequestAsync(togglePlayersAllowedFunction);
        }

        public Task<string> TogglePlayersAllowedRequestAsync()
        {
             return ContractHandler.SendRequestAsync<TogglePlayersAllowedFunction>();
        }

        public Task<TransactionReceipt> TogglePlayersAllowedRequestAndWaitForReceiptAsync(TogglePlayersAllowedFunction togglePlayersAllowedFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(togglePlayersAllowedFunction, cancellationToken);
        }

        public Task<TransactionReceipt> TogglePlayersAllowedRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<TogglePlayersAllowedFunction>(null, cancellationToken);
        }

        public Task<string> UnstakeRequestAsync(UnstakeFunction unstakeFunction)
        {
             return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(UnstakeFunction unstakeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unstakeFunction, cancellationToken);
        }

        public Task<string> UnstakeRequestAsync(string account, BigInteger unstakeAmount, BigInteger adjustedStake)
        {
            var unstakeFunction = new UnstakeFunction();
                unstakeFunction.Account = account;
                unstakeFunction.UnstakeAmount = unstakeAmount;
                unstakeFunction.AdjustedStake = adjustedStake;
            
             return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(string account, BigInteger unstakeAmount, BigInteger adjustedStake, CancellationTokenSource cancellationToken = null)
        {
            var unstakeFunction = new UnstakeFunction();
                unstakeFunction.Account = account;
                unstakeFunction.UnstakeAmount = unstakeAmount;
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
