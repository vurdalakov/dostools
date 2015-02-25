namespace Vurdalakov
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CommandLineParser commandLineParser = new CommandLineParser();

                if ((commandLineParser.FileNames.Length != 1) || commandLineParser.IsSwitchSet("?", "h", "help"))
                {
                    Console.WriteLine("Converts text file to base64 format | https://github.com/vurdalakov/dostools");
                    Console.WriteLine("Usage:\n    base64 <file name>");
                    Console.WriteLine("Example:\n    base64 readme.txt");
                    Environment.Exit(1);
                }

                var buffer = File.ReadAllBytes(commandLineParser.FileNames[0]);

                String base64 = Convert.ToBase64String(buffer);

                Console.WriteLine(base64);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }
    }
}
