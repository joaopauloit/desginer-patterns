namespace designer_patterns_csharper.designer.patterns.PatternRetry.Exceptions
{
    public class DatabaseNotAvailableException : BusinessException
    {        
        public DatabaseNotAvailableException(string message) : base(message)
        {
        }
    }
}