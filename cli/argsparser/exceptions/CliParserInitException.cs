using System;

namespace UsefulCsharpCommonsUtils.cli.argsparser.exceptions
{
    class CliParserInitException : Exception
    {
        public CliParserInitException()
        {
        }

        public CliParserInitException(string message)
            : base(message)
        {
        }

        public CliParserInitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
