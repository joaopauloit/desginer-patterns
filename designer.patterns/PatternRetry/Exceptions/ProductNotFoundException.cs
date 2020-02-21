using designer_patterns_csharper.designer.patterns.PatternRetry;

namespace designer_patterns_csharper.designer.patterns.PatternRetry.Exceptions
{
    public class ProductNotFoundException : BusinessException
    {
        public ProductNotFoundException(string message) : base(message)
        {

        }
    }
}
