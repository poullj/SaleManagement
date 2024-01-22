using System;

namespace Shared.Types
{
    public class ExpectedException : Exception
    {
        public string Headline { get; set; }
        public string Detail { get; set; }

        public ExpectedException(string message) : base(message)
        {
            
        }

        public ExpectedException(string message, string headline, string detail = "") : this(message)
        {
            Headline = headline;
            Detail = detail;
        }
    }
}
