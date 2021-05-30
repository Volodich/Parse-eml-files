using System;

namespace ParseEmlFiles
{
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException(string message) : base(message) { }
    }
}
