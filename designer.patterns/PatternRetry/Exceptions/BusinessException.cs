using System;

namespace designer_patterns_csharper.designer.patterns.PatternRetry.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
            
        }
    }
}