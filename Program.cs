using System;
using System.Collections.Generic;
using System.Net;
using designer_patterns_csharper.designer.patterns.PatternRetry;
using designer_patterns_csharper.designer.patterns.PatternRetry.Exceptions;
using designer_patterns_csharper.Util;

namespace designer_patterns_csharper
{
    public class Program
    {
        private static IBusinessOperation<PurchaseOrder> operation;
        private static ILog _log;

        public static void Main(String[] args)
        {
            _log = new LogConsole();
            NoErrors();
            ErrorNoRetry();
            ErrorWithRetry();
            ErrorWithRetryNetworking();
            ErrorWithRetryExceeded();
        }

        private static void NoErrors()
        {
            var purchaseOrder = new PurchaseOrder()
            {
                CodeProduct = "COD10",
                Quantity = 500
            };
            operation = new GenerateOrder(purchaseOrder);
            operation.Execute();
            _log.Register("Executado sem erros.");
        }

        private static void ErrorNoRetry()
        {
            var purchaseOrder = new PurchaseOrder()
            {
                CodeProduct = "COD01",
                Quantity = 1000
            };
            operation = new GenerateOrder(purchaseOrder,
                new List<Exception> {
                    new WebException("503 Service Unavailable - HTTP")
                }
            );

            try
            {
                operation.Execute();
            }
            catch (WebException e)
            {
                _log.Register("Executado com erro e não tentou novamente por que não utiliza \"o padrão Retry\".");
            }
        }

        private static void ErrorWithRetry()
        {
            var purchaseOrder = new PurchaseOrder()
            {
                CodeProduct = "COD02",
                Quantity = 1000
            };
            Retry<PurchaseOrder> retry = new Retry<PurchaseOrder>(
                _log,
                new GenerateOrder(purchaseOrder, new List<Exception> {
                    new WebException("503 Service Unavailable - HTTP") }),
                3,  //3 tentativas
                5000, //2 s de espera entre as tentativas
                typeof(WebException) //tipo de erro que deve passar pelas 3 tentativas
            );

            operation = retry;

            var purchaseOrderReturn = operation.Execute();
            _log.Register($"Reference Number \"{purchaseOrderReturn.ReferenceNumber}\" da Ordem de Compra, gerado com sucesso");
        }


        private static void ErrorWithRetryExceeded()
        {
            int tentativas = 3;
            try
            {
                var purchaseOrder = new PurchaseOrder()
                {
                    CodeProduct = "COD03",
                    Quantity = 1000
                };

                Retry<PurchaseOrder> retry = new Retry<PurchaseOrder>(
                    _log,
                    new GenerateOrder(purchaseOrder,
                    new List<Exception> {
                    new WebException("503 Service Unavailable - HTTP"),
                    new WebException("503 Service Unavailable - HTTP"),
                    new WebException("503 Service Unavailable - HTTP"),
                    new WebException("503 Service Unavailable - HTTP"),
                    }),
                    tentativas,  //3 tentativas
                    30000, //30 s de espera entre as tentativas
                    typeof(WebException) //tipo de erro que deve passar pelas 3 tentativas
                );

                operation = retry;
                var purchaseOrderReturn = operation.Execute();
                _log.Register($"Reference Number \"{purchaseOrderReturn.ReferenceNumber}\" da Ordem de Compra, gerado com sucesso");
            }
            catch (Exception ex)
            {
                _log.Register($"Erro \"{ex.Message}\" disparado após {tentativas}.");
            }
        }

        private static void ErrorWithRetryNetworking()
        {
            var purchaseOrder = new PurchaseOrder()
            {
                CodeProduct = "COD04",
                Quantity = 1000
            };
            RetryExponentialBackoff<PurchaseOrder> retry = new RetryExponentialBackoff<PurchaseOrder>(
                _log,
                new GenerateOrder(purchaseOrder,
                new List<Exception> {
                    new WebException("503 Service Unavailable - HTTP")
                    }),
                6,  //6 tentativas
                5000, //5 s de espera entre tentativas
                typeof(WebException) //tipo de erro que deve passar pelas 6 tentativas
            );
            operation = retry;

            var purchaseOrderReturn = operation.Execute();
            _log.Register($"Reference Number \"{purchaseOrderReturn.ReferenceNumber}\" da Ordem de Compra, gerado com sucesso");
        }
    }
}
