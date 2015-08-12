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
                    //var name = GlobalAtomTable.GetRegisteredFormatName((UInt16)i);
                    //if (name != null)
                    //{
                    //    Console.WriteLine("{0}\t0x{0:X4}\t{1}", i, name);
                    //}
                    try
                    {
                        var info = GlobalAtomTable.QueryBasicInformation((UInt16)i);
                        Console.WriteLine("{0}\t0x{0:X4}\t{1}", i, info.Name);
                    }
                    catch { }
                }
            }
            else if (_commandLineParser.IsOptionSet("a"))
            {
                var name = _commandLineParser.GetOptionString("a", null);
                if (null == name)
                {
                    Help();
                }

                var atom = GlobalAtomTable.Add(name);
                Console.WriteLine("{0}={1}", name, atom);
            }
            else if (_commandLineParser.IsOptionSet("f"))
            {
                var name = _commandLineParser.GetOptionString("f", null);
                if (null == name)
                {
                    Help();
                }

                var atom = GlobalAtomTable.Add(name);
                Console.WriteLine("{0}={1}", name, atom);
            }
            else if (_commandLineParser.IsOptionSet("d"))
            {
                var atom = _commandLineParser.GetOptionInt("d", -1);
                if ((atom < 0) || (atom > UInt16.MaxValue))
                {
                    Help();
                }

                GlobalAtomTable.Delete((UInt16)atom);
            }
            else if (_commandLineParser.IsOptionSet("q"))
            {
                var atom = _commandLineParser.GetOptionInt("q", -1);
                if ((atom < 0) || (atom > UInt16.MaxValue))
                {
                    Help();
                }

                Console.WriteLine("Atom={0}", atom);
                var atomBasicInformation = GlobalAtomTable.QueryBasicInformation((UInt16)atom);
                Console.WriteLine("ReferenceCount={0}", atomBasicInformation.ReferenceCount);
                Console.WriteLine("Pinned={0}", atomBasicInformation.Pinned);
                Console.WriteLine("NameLength={0}", atomBasicInformation.NameLength);
                Console.WriteLine("Name={0}", atomBasicInformation.Name);
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
            Console.WriteLine("Commands:\n\t-e - enumerates all strings\n\t-a:string - adds string\n\t-f:string - finds string\n\t-d:atom - deletes atom\n\t-q:atom - queries basic information about atom\n");
            Console.WriteLine("Options:\n\t-silent - no error messsages are shown; check exit code\n");
            Console.WriteLine("Exit codes:\n\t0 - command succeeded\n\t1 - command failed\n\t-1 - invalid command line syntax\n");

            base.Help();
        }
    }
}
