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
using MwStakingSol.Contracts.Disperse.ContractDefinition;

namespace MwStakingSol.Contracts.Disperse
{
    public partial class DisperseService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, DisperseDeployment disperseDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<DisperseDeployment>().SendRequestAndWaitForReceiptAsync(disperseDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, DisperseDeployment disperseDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<DisperseDeployment>().SendRequestAsync(disperseDeployment);
        }

        public static async Task<DisperseService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, DisperseDeployment disperseDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, disperseDeployment, cancellationTokenSource);
            return new DisperseService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public DisperseService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> DisperseTokenSimpleRequestAsync(DisperseTokenSimpleFunction disperseTokenSimpleFunction)
        {
             return ContractHandler.SendRequestAsync(disperseTokenSimpleFunction);
        }

        public Task<TransactionReceipt> DisperseTokenSimpleRequestAndWaitForReceiptAsync(DisperseTokenSimpleFunction disperseTokenSimpleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disperseTokenSimpleFunction, cancellationToken);
        }

        public Task<string> DisperseTokenSimpleRequestAsync(string token, List<string> recipients, List<BigInteger> values)
        {
            var disperseTokenSimpleFunction = new DisperseTokenSimpleFunction();
                disperseTokenSimpleFunction.Token = token;
                disperseTokenSimpleFunction.Recipients = recipients;
                disperseTokenSimpleFunction.Values = values;
            
             return ContractHandler.SendRequestAsync(disperseTokenSimpleFunction);
        }

        public Task<TransactionReceipt> DisperseTokenSimpleRequestAndWaitForReceiptAsync(string token, List<string> recipients, List<BigInteger> values, CancellationTokenSource cancellationToken = null)
        {
            var disperseTokenSimpleFunction = new DisperseTokenSimpleFunction();
                disperseTokenSimpleFunction.Token = token;
                disperseTokenSimpleFunction.Recipients = recipients;
                disperseTokenSimpleFunction.Values = values;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disperseTokenSimpleFunction, cancellationToken);
        }

        public Task<string> DisperseTokenRequestAsync(DisperseTokenFunction disperseTokenFunction)
        {
             return ContractHandler.SendRequestAsync(disperseTokenFunction);
        }

        public Task<TransactionReceipt> DisperseTokenRequestAndWaitForReceiptAsync(DisperseTokenFunction disperseTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disperseTokenFunction, cancellationToken);
        }

        public Task<string> DisperseTokenRequestAsync(string token, List<string> recipients, List<BigInteger> values)
        {
            var disperseTokenFunction = new DisperseTokenFunction();
                disperseTokenFunction.Token = token;
                disperseTokenFunction.Recipients = recipients;
                disperseTokenFunction.Values = values;
            
             return ContractHandler.SendRequestAsync(disperseTokenFunction);
        }

        public Task<TransactionReceipt> DisperseTokenRequestAndWaitForReceiptAsync(string token, List<string> recipients, List<BigInteger> values, CancellationTokenSource cancellationToken = null)
        {
            var disperseTokenFunction = new DisperseTokenFunction();
                disperseTokenFunction.Token = token;
                disperseTokenFunction.Recipients = recipients;
                disperseTokenFunction.Values = values;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disperseTokenFunction, cancellationToken);
        }

        public Task<string> DisperseEtherRequestAsync(DisperseEtherFunction disperseEtherFunction)
        {
             return ContractHandler.SendRequestAsync(disperseEtherFunction);
        }

        public Task<TransactionReceipt> DisperseEtherRequestAndWaitForReceiptAsync(DisperseEtherFunction disperseEtherFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disperseEtherFunction, cancellationToken);
        }

        public Task<string> DisperseEtherRequestAsync(List<string> recipients, List<BigInteger> values)
        {
            var disperseEtherFunction = new DisperseEtherFunction();
                disperseEtherFunction.Recipients = recipients;
                disperseEtherFunction.Values = values;
            
             return ContractHandler.SendRequestAsync(disperseEtherFunction);
        }

        public Task<TransactionReceipt> DisperseEtherRequestAndWaitForReceiptAsync(List<string> recipients, List<BigInteger> values, CancellationTokenSource cancellationToken = null)
        {
            var disperseEtherFunction = new DisperseEtherFunction();
                disperseEtherFunction.Recipients = recipients;
                disperseEtherFunction.Values = values;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disperseEtherFunction, cancellationToken);
        }
    }
}
