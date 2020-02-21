using System;

namespace designer_patterns_csharper.Util
{
    public class LogConsole : ILog
    {
        public LogConsole()
        {
        }

        public void Register(string message)
        {
            Console.WriteLine("/***********************************************************/");
            Console.WriteLine("");
            Console.WriteLine($"Data: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            Console.WriteLine($"Message Info: {message}");
            Console.WriteLine("");
            Console.WriteLine("/***********************************************************/");
        }
    }
}