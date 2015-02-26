namespace Vurdalakov
{
    using System;

    class Program
    {
        static void Main(String[] args)
        {
            try
            {
                CommandLineParser commandLineParser = new CommandLineParser();

                if (commandLineParser.IsOptionSet("?", "h", "help"))
                {
                    Console.WriteLine("Prints date and/or time in specified format | https://github.com/vurdalakov/dostools");
                    Console.WriteLine("Examples:");
                    Console.WriteLine("\tdatetime /format:\"dd.MM.yyyy HH.mm.ss.ffff\"");
                    Console.WriteLine("\tdatetime /adddays:-1 /format:yyyy-MM-dd");
                    Console.WriteLine("\tdatetime /newline");
                    Environment.Exit(1);
                }

                DateTime dateTime = DateTime.Now;

                Int32 addDays = commandLineParser.GetOptionInt("adddays", 0);
                if (addDays != 0)
                {
                    dateTime = dateTime.AddDays(addDays);
                }

                String format = commandLineParser.GetOptionString("format", "yyyy-MM-dd HH-mm-ss");

                String text = String.Format("{0:" + format + "}", dateTime);

                Console.Write(text);

                if (commandLineParser.IsOptionSet("newline"))
                {
                    Console.WriteLine();
                }

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);

                Environment.Exit(2);
            }
        }
    }
}
