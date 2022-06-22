using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UsefulCsharpCommonsUtils.cli.argsparser.exceptions;
using UsefulCsharpCommonsUtils.lang;

namespace UsefulCsharpCommonsUtils.cli.argsparser
{
    public abstract class CliParser<T>
    {
        readonly Dictionary<string, Option> _options = new Dictionary<string, Option>();

        public CliParser()
        {


        }

        public void ClearOptions()
        {
            _options.Clear();
        }

        public void AddOption(Option option)
        {
            if (string.IsNullOrWhiteSpace(option.Name) || string.IsNullOrWhiteSpace(option.ShortOpt))
            {
                throw new CliParserInitException(CliParserLangRef.AddOption_MustHaveNameAndShOpt);
            }

            if (_options.Any(r => r.Value.Name.Equals(option.Name)))
            {
                throw new CliParserInitException(string.Format(CliParserLangRef.AddOption_SameNameOptExist, option.Name, option.ShortOpt));
            }
            if (_options.Any(r => r.Value.ShortOpt.Equals(option.ShortOpt)))
            {
                throw new CliParserInitException(string.Format(CliParserLangRef.AddOption_SameShortOptExist, option.Name, option.ShortOpt));
            }
            if (_options.Any(r => r.Value.LongOpt.Equals(option.LongOpt)))
            {
                throw new CliParserInitException(string.Format(CliParserLangRef.AddOption_SameLongOptExist, option.Name, option.ShortOpt));
            }
            _options.Add(option.Name, option);
        }


        public abstract T ParseDirect(string[] args);


        public T Parse(string[] args, Func<Dictionary<string, Option>, T> parseTrt)
        {

            List<Option> optionSeen = GetOptAndArgs(args);

            CheckOptions(optionSeen);

            Dictionary<string, Option> optDictionary = optionSeen.ToDictionary(opt => opt.Name);



            return parseTrt.Invoke(optDictionary);
        }

        private void CheckOptions(List<Option> optionSeen)
        {
            foreach (KeyValuePair<string, Option> valuePair in _options)
            {
                Option valueOption = valuePair.Value;
                if (valueOption.IsMandatory)
                {
                    if (!optionSeen.Any(r => r.Name.Equals(valueOption.Name)))
                    {
                        throw new CliParsingException(string.Format(CliParserLangRef.CheckOption_OptionNotPresent, valueOption.ShortOpt));
                    }
                }

                if (valueOption.HasArgs)
                {
                    Option option = optionSeen.FirstOrDefault(r => r.Name.Equals(valueOption.Name));
                    if (option == null) continue;

                    if (!option.Value.Any())
                    {
                        throw new CliParsingException(string.Format(CliParserLangRef.CheckOption_OptionMustHaveArg, valueOption.ShortOpt));
                    }
                }
            }
        }

        private List<Option> GetOptAndArgs(string[] args)
        {
            Stack<Option> optionPresent = new Stack<Option>();
            Option aloneOption = new Option()
            {
                Name = "DEFAULT"
            };

            bool nextIsArg = false;
            foreach (string arg in args)
            {
                if (arg.StartsWith("-"))
                {

                    Func<KeyValuePair<string, Option>, bool> predicate;

                    if (arg.StartsWith("--"))
                    {
                        var arg1 = arg;
                        predicate = r => r.Value.LongOpt == arg1.TrimStart('-');
                    }
                    else
                    {
                        var arg1 = arg;
                        predicate = r => r.Value.ShortOpt == arg1.TrimStart('-');
                    }

                    Option o = _options.FirstOrDefault(predicate).Value;
                    if (o == null)
                    {
                        if (nextIsArg)
                        {
                            optionPresent.Peek().Value.Add(arg);

                        }

                        continue;

                    };

                    nextIsArg = o.HasArgs;

                    optionPresent.Push(o);
                }
                else
                {
                    if (nextIsArg)
                    {
                        optionPresent.Peek().Value.Add(arg);
                    }
                    else
                    {
                        aloneOption.Value.Add(arg);
                    }
                }
            }
            List<Option> optRet = optionPresent.ToList();
            optRet.Add(aloneOption);
            return optRet;
        }

        /// <summary>
        /// Show the parser syntax.
        /// </summary>
        public void ShowSyntax()
        {
            string codeBase = Assembly.GetEntryAssembly().CodeBase;
            string name = Path.GetFileName(codeBase);

            Console.WriteLine(CliParserLangRef.ShowSyntax_SyntaxTpl, name);
            Console.WriteLine("");
            Console.WriteLine(CliParserLangRef.ShowSyntax_OptionsLbl);
            foreach (KeyValuePair<string, Option> valuePair in _options)
            {
                Option opt = valuePair.Value;
                if (opt.Description == null || opt.IsHiddenInHelp)
                {
                    continue;
                }
                CwWriteLines(opt);


            }
        }

        private void CwWriteLines(Option opt)
        {
            List<string> lines = new List<string>(1);

            int colAlen = 8;
            int colBlen = 4;
            int colClen = Console.BufferWidth - colAlen - colBlen - 1;
            string strTpl = "{0," + colAlen + "}{1," + colBlen + "}{2,-" + colClen + "}";

            lines.Add(opt.Description);


            foreach (string line in lines)
            {
                string enhancedLine = opt.IsMandatory ? CliParserLangRef.CwWriteLines_Required : "";
                enhancedLine += line;
                enhancedLine += " (" + CliParserLangRef.CwWriteLines_Also + " : --" + opt.LongOpt + ")";

                string fsLine = string.Format(strTpl, "-" + opt.ShortOpt, "", enhancedLine);
                if (fsLine.Length > Console.BufferWidth)
                {

                    string strT = fsLine;
                    bool isFirstLine = true;
                    int mLen = Console.BufferWidth - 1;
                    while (strT.Length >= mLen)
                    {
                        string s = strT.Substring(0, mLen);
                        strT = strT.Substring(mLen);

                        if (isFirstLine)
                        {
                            Console.WriteLine(s);
                            isFirstLine = false;
                            mLen = colClen;
                        }
                        else
                        {
                            Console.WriteLine(strTpl, "", "", s);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(strT))
                        Console.WriteLine(strTpl, "", "", strT);




                }
                else
                {
                    Console.WriteLine(fsLine);
                }


            }
        }


     

      

        public int? GetSingleOptionValueInt(Option option, Dictionary<string, Option> dictionary,
            int? returnDefault = null)
        {
            if (!HasOption(option, dictionary))
            {
                return returnDefault;
            }

            string retValueStr = GetSingleOptionValue(option.Name, dictionary, returnDefault.HasValue ? returnDefault.ToString() : null);
            if (int.TryParse(retValueStr, out var retInt))
            {
                return retInt;
            }
            return returnDefault;
        }

        public int GetSingleOptionValueInt(Option option, Dictionary<string, Option> dictionary,
            int returnDefault = 0)
        {
            if (!HasOption(option, dictionary))
            {
                return returnDefault;
            }

            string retValueStr = GetSingleOptionValue(option.Name, dictionary, returnDefault.ToString() );
            if (int.TryParse(retValueStr, out var retInt))
            {
                return retInt;
            }
            return returnDefault;
        }

        public bool? GetSingleOptionValueBoolean(Option option, Dictionary<string, Option> dictionary,
            bool? returnDefault = null)
        {
            if (!HasOption(option, dictionary))
            {
                return returnDefault;
            }

            string retValueStr = GetSingleOptionValue(option.Name, dictionary, returnDefault.HasValue ? returnDefault.ToString() : null);
            if (bool.TryParse(retValueStr, out var retBool))
            {
                return retBool;
            }

            if (retValueStr.Equals("1") || retValueStr.ToLower().Equals("true"))
            {
                return true;
            }

            return returnDefault;
        }

        public bool GetSingleOptionValueBoolean(Option option, Dictionary<string, Option> dictionary,
            bool returnDefault = false)
        {

            string retValueStr = GetSingleOptionValue(option.Name, dictionary,  returnDefault.ToString());
            if (bool.TryParse(retValueStr, out var retBool))
            {
                return retBool;
            }

            if (retValueStr.Equals("1") || retValueStr.ToLower().Equals("true"))
            {
                return true;
            }

            return returnDefault;
        }

        public string GetSingleOptionValue(Option option, Dictionary<string, Option> dictionary,
            string returnDefault = null)
        {
            return GetSingleOptionValue(option.Name, dictionary, returnDefault);
        }

        public string GetSingleOptionValue(string optName, Dictionary<string, Option> dictionary, string returnDefault = null)
        {
            if (dictionary.ContainsKey(optName))
            {
                return dictionary[optName].Value.FirstOrDefault();
            }

            if (returnDefault == null && _options.ContainsKey(optName))
            {
                return _options[optName].DefaultValue;
            }

            return returnDefault;
        }



        public bool HasOption(Option option, Dictionary<string, Option> dictionary)
        {
            return HasOption(option.Name, dictionary);
        }

        public bool HasOption(string optName, Dictionary<string, Option> dictionary)
        {
            return dictionary.ContainsKey(optName);
        }


    }

    
    public class Option
    {
        private List<string> _value = new List<string>();

        /// <summary>
        /// Nom de l'option
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ecriture courte pour utiliser l'option
        /// </summary>
        public string ShortOpt { get; set; }

        /// <summary>
        /// Ecriture longue pour utiliser l'option
        /// </summary>
        public string LongOpt { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Définit si l'option a des arguments
        /// </summary>
        public bool HasArgs { get; set; }

        /// <summary>
        /// Indique si l'option est requise obligatoirement
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Indique si l'option est masquée lors de l'affichage de la syntaxe
        /// </summary>
        public bool IsHiddenInHelp { get; set; }

        public List<String> Value
        {
            get => _value;
            set => _value = value;
        }

        public string DefaultValue { get; set; }



        public override string ToString()
        {
            return string.Format("Option:[ShortOpt: {0}, LongOpt: {1}, Description: {2}, HasArgs: {3}, IsMandatory: {4}, Value: {5}, IsHiddenInHelp: {6}]", ShortOpt, LongOpt, Description, HasArgs, IsMandatory, Value, IsHiddenInHelp);
        }

        public static Option CreateNew(string name, string shortOp, string longOp, bool hasArg = false, bool isMandatory = false, string description = "")
        {
            Option opt = new Option()
            {
                ShortOpt = shortOp,
                LongOpt = longOp,
                Description = description,
                HasArgs = hasArg,
                IsMandatory = isMandatory,
                Name = name

            };

            return opt;
        }
    }
}
