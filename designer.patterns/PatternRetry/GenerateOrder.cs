using System;
using System.Collections.Generic;
using System.Linq;
using designer_patterns_csharper.designer.patterns.PatternRetry.Exceptions;

namespace designer_patterns_csharper.designer.patterns.PatternRetry
{
    public class GenerateOrder : IBusinessOperation<PurchaseOrder>
    {
        private PurchaseOrder _purchaseOrder;
        private Queue<Exception> _errors;

        public GenerateOrder()
        {
            _purchaseOrder = new PurchaseOrder();
            _errors = new Queue<Exception>();
        }

        public GenerateOrder(PurchaseOrder purchaseOrder,
            List<Exception> exception = null)
        {
            _errors = new Queue<Exception>();

            this._purchaseOrder = purchaseOrder;
            if (exception != null)
            {
                exception.ForEach(x => _errors.Enqueue(x));
            }
        }
        public PurchaseOrder Execute()
        {
            if (this._errors.Any())
            {
                var error = this._errors.Peek();
                this._errors.Dequeue();
                throw error;
            }
            
            //Ocorreu com sucesso
            this._purchaseOrder.ReferenceNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff");

            return this._purchaseOrder;
        }
    }
}