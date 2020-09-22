namespace Vurdalakov
{
    using System;
    using System.Security.Policy;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            if (this._commandLineParser.FileNames.Length != 1)
            {
                this.Help();
            }

            var fileName = this._commandLineParser.FileNames[0];

            if (this._commandLineParser.IsOptionSet("url2host"))
            {
                var host = new Uri(fileName).Host;

                if (this._commandLineParser.TryGetOptionString("url2host", out var optionValue) && optionValue.Equals("nowww", StringComparison.OrdinalIgnoreCase) && host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                {
                    host = host.Substring(4);
                }

                Console.WriteLine(host);
            }
            //else if (_commandLineParser.IsOptionSet(""))
            //{
            //}
            else
            {
                Help();
            }

            return 0;
        }

        protected override void Help()
        {
            Console.WriteLine("DosTool {0} | https://github.com/vurdalakov/dostools\n", ApplicationVersion);
            Console.WriteLine("A collections of BAT/CMD helpers.\n");
            Console.WriteLine("Usage:\n\tdostool <-command[:parameter]> [-silent]\n");
            Console.WriteLine("Commands:");
            Console.WriteLine("\tGet h  ost/domain name from URL:  -url2host <url>");
            Console.WriteLine("\tGet h  ost/domain name from URL:  -url2host:nowww <url> ; removes 'www.' prefix");
            Console.WriteLine("Options:\n\t-silent - no error messages are shown; check exit code\n");
            Console.WriteLine("Exit codes:\n\t0 - command succeeded\n\t1 - command failed\n\t-1 - invalid command line syntax\n");

            base.Help();
        }
    }
}
