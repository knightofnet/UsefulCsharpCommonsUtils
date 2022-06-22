using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsefulCsharpCommonsUtils.cli.argsparser
{
    public static class CliParserLangRef
    {

        public static String AddOption_MustHaveNameAndShOpt = "Option must have a name and a shortOpt.";

        public static String AddOption_SameNameOptExist = "Error when registering option ({0}, shortOpt: {1}): An option with the same name has already been defined.";

        public static String AddOption_SameShortOptExist =
            "Error when registering option ({0}, shortOpt: {1}): An option with the same short-option has already been defined.";

        public static String AddOption_SameLongOptExist =
            "Error when registering option ({0}, shortOpt: {1}): An option with the same long-option has already been defined.";

        public static String CheckOption_OptionNotPresent = "Option -{0} is not present when it is mandatory.";

        public static String CheckOption_OptionMustHaveArg = "Option -{0} must have argument.";

        public static String ShowSyntax_OptionsLbl = "  OPTIONS :";

        public static String ShowSyntax_SyntaxTpl = "  Syntax: {0} OPTIONS";

        public static String CwWriteLines_Required = "Required. ";

        public static String CwWriteLines_Also = "also";
    }
}
