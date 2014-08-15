namespace Vurdalakov
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DateTime startTime = DateTime.Now;

                CommandLineParser commandLineParser = new CommandLineParser();

                if (commandLineParser.IsSwitchSet("?", "h", "help"))
                {
                    Help();
                }

                if (commandLineParser.FileNames.Length != 1)
                {
                    Help();
                }

                String delay = commandLineParser.FileNames[0].ToLower();

                Char suffix = delay[delay.Length - 1];
                if (Char.IsDigit(suffix))
                {
                    suffix = 's';
                }
                else
                {
                    delay = delay.Substring(0, delay.Length - 1);
                }

                Double number;
                if (!Double.TryParse(delay, out number))
                {
                    Help();
                }

                switch (suffix)
                {
                    case 's':
                        break;
                    case 'm':
                        number *= 60;
                        break;
                    case 'h':
                        number *= 3660;
                        break;
                    case 'd':
                        number *= 86400;
                        break;
                    default:
                        Help();
                        break;
                }

                number *= 1000;

                TimeSpan timeout = new TimeSpan();
                timeout = new TimeSpan(0, 0, 0, 0, (int)Math.Round(number));

                Thread.Sleep(timeout);

                if (commandLineParser.IsSwitchSet("showdelay"))
                {
                    TimeSpan realTimeout = DateTime.Now - startTime;
                    Console.WriteLine("{0:N0} {1:00}:{2:00}:{3:00}.{4:000}", Math.Floor(realTimeout.TotalDays), realTimeout.Hours, realTimeout.Minutes, realTimeout.Seconds, realTimeout.Milliseconds);
                }

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);

                Environment.Exit(2);
            }
        }

        static void Help()
        {
            Console.WriteLine("Delays for a specified amount of time | https://github.com/vurdalakov/dostools");
            Console.WriteLine("Examples:");
            Console.WriteLine("\tsleep 10");
            Console.WriteLine("\tsleep 10s");
            Console.WriteLine("\tsleep 10s -showdelay");
            Console.WriteLine("\tsleep 60m");
            Console.WriteLine("\tsleep 24h");
            Console.WriteLine("\tsleep  7d");
            Environment.Exit(1);
        }
    }
}
