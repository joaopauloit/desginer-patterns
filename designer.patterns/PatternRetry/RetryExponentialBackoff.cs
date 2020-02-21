using System;
using System.Collections.Generic;
using System.Threading;
using designer_patterns_csharper.designer.patterns.PatternRetry.Exceptions;
using designer_patterns_csharper.Util;

namespace designer_patterns_csharper.designer.patterns.PatternRetry
{
    public class RetryExponentialBackoff<T> : IBusinessOperation<T>
    {
        private static Random RANDOM = new Random();
        private ILog _log;
        private IBusinessOperation<T> _operation;
        private int _maxAttempts;
        private int _maxDelay;
        private int _attempts;
        private List<Type> _test;
        private List<Exception> _errors;
        public RetryExponentialBackoff(
            ILog log,
            IBusinessOperation<T> op,
            int maxAttempts,
            int maxDelay,
            Type ignoreTest
        )
        {
            this._test = new List<Type>();
            this._errors = new List<Exception>();
            this._attempts = 0;
            this._log = log;
            this._operation = op;
            this._maxAttempts = maxAttempts;
            this._maxDelay = maxDelay;
            this._test.Add(ignoreTest);
        }
        public T Execute()
        {
            do
            {
                try
                {
                    _log.Register($"Tentativa Nro. {this._attempts + 1}");
                    return this._operation.Execute();
                }
                catch (Exception e)
                {
                    _log.Register($"Tentativa Nro. {this._attempts + 1} com erro  \"{e.Message}\" ");
                    this._errors.Add(e);

                    this._attempts++;
                    var exists = this._test.Contains(e.GetType());

                    if (this._attempts >= this._maxAttempts || !exists)
                    {
                        _log.Register($"Excedeu {this._maxAttempts} tentativas e não será reenviado.");
                        throw e;
                    }

                    try
                    {
                        int testDelay = RANDOM.Next(50000);
                        int delay = testDelay < this._maxDelay ? testDelay : _maxDelay;

                        _log.Register($"Aguardando {(delay / 1000)}s para a proxima tentativa");
                        Thread.Sleep(delay);
                    }
                    catch (Exception f)
                    {
                        //ignore
                    }
                }
            } while (true);
        }
    }

}