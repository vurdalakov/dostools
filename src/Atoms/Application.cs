namespace Vurdalakov
{
    using System;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            if (_commandLineParser.IsOptionSet("ga"))
            {
                var name = GetName("ga");
                var atom = GlobalAtomTable.GlobalAdd(name);
                Console.WriteLine("{0}={1}", name, atom);
            }
            else if (_commandLineParser.IsOptionSet("gd"))
            {
                var atom = GetAtom("gd");
                GlobalAtomTable.GlobalDelete(atom);
            }
            else if (_commandLineParser.IsOptionSet("gf"))
            {
                var name = GetName("gf");
                var atom = GlobalAtomTable.GlobalFind(name);
                Console.WriteLine("{0}={1}", name, 0 == atom ? "<Not found>" : atom.ToString());
            }
            else if (_commandLineParser.IsOptionSet("gq"))
            {
                var atom = GetAtom("gq");
                var name = GlobalAtomTable.GlobalGetName(atom);
                Console.WriteLine("{0}={1}", atom, null == name ? "<Not found>" : name);
            }
            else if (_commandLineParser.IsOptionSet("ge"))
            {
                for (var atom = 0xC000; atom <= 0xFFFF; atom++)
                {
                    var name = GlobalAtomTable.GlobalGetName((UInt16)atom);
                    if (name != null)
                    {
                        Console.WriteLine("{0}\t0x{0:X4}\t{1}", atom, name);
                    }
                }
            }
            else if (_commandLineParser.IsOptionSet("na"))
            {
                var name = GetName("na");
                var atom = GlobalAtomTable.NtAdd(name);
                Console.WriteLine("{0}={1}", name, atom);
            }
            else if (_commandLineParser.IsOptionSet("nd"))
            {
                var atom = GetAtom("nd");
                GlobalAtomTable.NtDelete(atom);
            }
            else if (_commandLineParser.IsOptionSet("nf"))
            {
                var name = GetName("nf");
                var atom = GlobalAtomTable.NtFind(name);
                Console.WriteLine("{0}={1}", name, atom);
            }
            else if (_commandLineParser.IsOptionSet("nq"))
            {
                var atom = GetAtom("nq");
                var atomBasicInformation = GlobalAtomTable.NtQueryBasicInformation(atom);
                if (null == atomBasicInformation)
                {
                    Console.WriteLine("<Not found>");
                }
                else
                {
                    Console.WriteLine("Atom={0}", atomBasicInformation.Atom);
                    Console.WriteLine("ReferenceCount={0}", atomBasicInformation.ReferenceCount);
                    Console.WriteLine("Pinned={0}", atomBasicInformation.Pinned);
                    Console.WriteLine("Name={0}", atomBasicInformation.Name);
                }
            }
            else if (_commandLineParser.IsOptionSet("ne"))
            {
                for (var atom = 0xC000; atom <= 0xFFFF; atom++)
                {
                    var info = GlobalAtomTable.NtQueryBasicInformation((UInt16)atom);
                    if (info != null)
                    {
                        Console.WriteLine("{0}\t0x{0:X4}\t{1}", atom, info.Name);
                    }
                }
            }
            else if (_commandLineParser.IsOptionSet("ua"))
            {
                var name = GetName("ua");
                var atom = GlobalAtomTable.UserAdd(name);
                Console.WriteLine("{0}={1}", name, atom);
            }
            else if (_commandLineParser.IsOptionSet("ue"))
            {
                for (var atom = 0xC000; atom <= 0xFFFF; atom++)
                {
                    var name = GlobalAtomTable.UserGetName((UInt16)atom);
                    if (name != null)
                    {
                        Console.WriteLine("{0}\t0x{0:X4}\t{1}", atom, name);
                    }
                }
            }
            else
            {
                Help();
            }

            return 0;
        }

        private String GetName(String optionName)
        {
            var name = _commandLineParser.GetOptionString(optionName, null);
            if (null == name)
            {
                Help();
            }
            return name;
        }

        private UInt16 GetAtom(String optionName)
        {
            var atom = _commandLineParser.GetOptionInt(optionName, -1);
            if ((atom < 0) || (atom > UInt16.MaxValue))
            {
                Help();
            }
            return (UInt16)atom;
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
