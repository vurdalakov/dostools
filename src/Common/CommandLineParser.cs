namespace Vurdalakov
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    // Command format: [/switch] [/option=value] [filename1 [filename2 ...]]
    public class CommandLineParser
    {
        private readonly Dictionary<String, String> options = new Dictionary<String, String>();

        public CommandLineParser(Char[] prefixes = null, Char[] valueSeparators = null)
        {
            if (null == prefixes)
            {
                prefixes = new[] { '/', '-' };
            }

            if (null == valueSeparators)
            {
                valueSeparators = new[] { '=', ':' };
            }

            this.ParseArguments(prefixes, valueSeparators);
        }

        public String ExecutableFileName { get; private set; }

        public String ExecutablePath { get; private set; }

        public String[] FileNames { get; private set; }

        public Boolean IsSwitchSet(String switchName)
        {
            return this.options.ContainsKey(switchName.ToLower());
        }

        public Boolean IsSwitchSet(params String[] switchNames)
        {
            foreach (String switchName in switchNames)
            {
                if (this.IsSwitchSet(switchName))
                {
                    return true;
                }
            }

            return false;
        }

        public String GetOptionString(String optionName, String defaultValue = null)
        {
            return this.IsSwitchSet(optionName) ? this.options[optionName.ToLower()] : defaultValue;
        }

        public Int32 GetOptionInt32(String optionName, Int32 defaultValue = 0)
        {
            try
            {
                return this.IsSwitchSet(optionName) ? Convert.ToInt32(this.GetOptionString(optionName)) : defaultValue;
            }
            catch
            {
                throw new ArgumentException(String.Format("'/{0}' option does not contain a valid 32-bit integer value", optionName));
            }
        }

        private void ParseArguments(Char[] prefixes, Char[] valueSeparators)
        {
            String[] args = Environment.GetCommandLineArgs();

            this.ExecutableFileName = args[0];
            this.ExecutablePath = Path.GetDirectoryName(this.ExecutableFileName);

            List<String> fileNames = new List<String>();

            for (int i = 1; i < args.Length; i++)
            {
                String s = args[i];

                if (String.IsNullOrEmpty(s))
                {
                    // just skip
                }
                else if (s.IndexOfAny(prefixes) < 0) // file names
                {
                    fileNames.Add(s);
                }
                else
                {
                    if (s.IndexOfAny(valueSeparators) > 0) // option
                    {
                        String[] optionParts = s.Split(valueSeparators);
                        this.options.Add(optionParts[0].TrimStart(prefixes).ToLower(), optionParts[1]);
                    }
                    else // switch
                    {
                        this.options.Add(s.TrimStart(prefixes).ToLower(), null);
                    }
                }
            }

            this.FileNames = fileNames.ToArray();
        }
    }
}
