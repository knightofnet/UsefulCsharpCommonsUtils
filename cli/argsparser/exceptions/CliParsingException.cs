using System;

namespace UsefulCsharpCommonsUtils.cli.argsparser.exceptions
{
    public class CliParsingException : Exception
    {
        public CliParsingException()
        {
        }

        public CliParsingException(string message)
            : base(message)
        {
        }

        public CliParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
