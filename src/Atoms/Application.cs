namespace Vurdalakov
{
    using System;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            if (_commandLineParser.IsOptionSet("e", "enumerate"))
            {
                for (var i = 0xC000; i <= 0xFFFF; i++)
                {
                    var name = GlobalAtomTable.GetRegisteredFormatName((UInt16)i);
                    if (name != null)
                    {
                        Console.WriteLine("{0}\t0x{0:X4}\t{1}", i, name);
                    }
                }
            }
            else
            {
                Help();
            }

            return 0;
        }

        protected override void Help()
        {
            Console.WriteLine("Atoms {0} | https://github.com/vurdalakov/dostools\n", ApplicationVersion);
            Console.WriteLine("Works with Windows global atom table.\n");
            Console.WriteLine("Usage:\n\tatoms <-command> [-silent]\n");
            Console.WriteLine("Commands:\n\t-e - enumerates all strings\n");
            Console.WriteLine("Options:\n\t-silent - no error messsages are shown; check exit code\n");
            Console.WriteLine("Exit codes:\n\t0 - command succeeded\n\t1 - command failed\n\t-1 - invalid command line syntax\n");

            base.Help();
        }
    }
}
