using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using designer_patterns_csharper.designer.patterns.PatternRetry.Exceptions;
using designer_patterns_csharper.Util;

namespace designer_patterns_csharper.designer.patterns.PatternRetry
{
    public class Retry<T> : IBusinessOperation<T>
    {
        private ILog _log;
        private IBusinessOperation<T> _operation;
        private int _maxAttempts;
        private int _delay;
        private int _attempts;
        private List<Type> _test;
        private List<Exception> _errors;

        public Retry(
            ILog log,
            IBusinessOperation<T> op,
            int maxAttempts,
            int delay,
            Type ignoreTest
        )
        {
            this._attempts = 0;
            this._test = new List<Type>();
            this._errors = new List<Exception>();
            this._log = log;
            this._operation = op;
            this._maxAttempts = maxAttempts;
            this._delay = delay;
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
                        _log.Register($"Aguardando {(this._delay / 1000)}s para a proxima tentativa");
                        Thread.Sleep(this._delay);
                    }
                    catch (ThreadInterruptedException f)
                    {
                        //ignore
                    }
                }
            } while (true);
        }
    }
}