using System;

namespace FinlandiaHiihtoAPI.Exceptions
{
    public class TooMuchResultsExceptions : ArgumentException
    {
        public TooMuchResultsExceptions()
        {
        }

        public TooMuchResultsExceptions(string message)
            : base(message)
        {
        }

        public TooMuchResultsExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
