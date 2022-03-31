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
using MwStakingSol.Contracts.IWEAPONStakable.ContractDefinition;

namespace MwStakingSol.Contracts.IWEAPONStakable
{
    public partial class IWEAPONStakableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IWEAPONStakableDeployment iWEAPONStakableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IWEAPONStakableDeployment>().SendRequestAndWaitForReceiptAsync(iWEAPONStakableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IWEAPONStakableDeployment iWEAPONStakableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IWEAPONStakableDeployment>().SendRequestAsync(iWEAPONStakableDeployment);
        }

        public static async Task<IWEAPONStakableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IWEAPONStakableDeployment iWEAPONStakableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iWEAPONStakableDeployment, cancellationTokenSource);
            return new IWEAPONStakableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IWEAPONStakableService(Nethereum.Web3.Web3 web3, string contractAddress)
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
    }
}
