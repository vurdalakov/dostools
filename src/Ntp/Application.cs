namespace Vurdalakov
{
    using System;
    using System.Globalization;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            var ntpServer = _commandLineParser.GetOptionString("s", "server", "pool.ntp.org");
            var showUtc = _commandLineParser.IsOptionSet("u", "utc");
            var timeFormat = _commandLineParser.GetOptionString("f", "format", "yyyy MM dd HH mm ss fff");

            var ntpClient = new NtpClient(ntpServer);
            var time = ntpClient.GetUtcTime();

            if (!showUtc)
            {
                time = time.ToLocalTime();
            }

            Console.Write(time.ToString(timeFormat, CultureInfo.InvariantCulture));

            return 0;
        }

        protected override void Help()
        {
            Console.WriteLine("NTP client {0} | https://github.com/vurdalakov/dostools\n", ApplicationVersion);
            Console.WriteLine("Gets network time from NTP server.\n");
            Console.WriteLine("Usage:\n\tntp [-utc] [-server:ntp.something.com] [-format:yyyy.MM.dd HH:mm:ss.fff]\n");
            Console.WriteLine("Commands:\n\t-u - show UTC time instead of local\n\t-s - use server other than pool.ntp.org\n\t-f - use custom date format\n");
            Console.WriteLine("Exit codes:\n\t0 - getting time succeeded\n\t1 - getting time failed\n\t-1 - invalid command line syntax\n");
            
            base.Help();
        }
    }
}
