namespace Vurdalakov
{
    using System;
    using System.IO;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            var fileName = _commandLineParser.FileNames.Length > 0 ? _commandLineParser.FileNames[0] : null;
            var text1 = _commandLineParser.FileNames.Length > 1 ? _commandLineParser.FileNames[1] : null;
            var text2 = _commandLineParser.FileNames.Length > 2 ? _commandLineParser.FileNames[2] : null;
            
            if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(text1) || String.IsNullOrEmpty(text2))
            {
                Help();
            }

            try
            {
                var text = File.ReadAllText(fileName);
                text = text.Replace(text1, text2);
                File.WriteAllText(fileName, text);
            }
            catch (FileNotFoundException)
            {
                Error(2, "File not found.");
            }
            catch (DirectoryNotFoundException)
            {
                Error(3, "Directory not found.");
            }
            catch (UnauthorizedAccessException)
            {
                Error(5, "Access denied.");
            }

            return 0;
        }

        protected override void Help()
        {
            Console.WriteLine("FReplace {0} | https://github.com/vurdalakov/dostools\n", ApplicationVersion);
            Console.WriteLine("Replaces text in text file.\n");
            Console.WriteLine("Usage:\n\tfreplace <file name> <text to replace> <text to replace with> [-silent]\n");
            Console.WriteLine("Options:\n\t-silent - no error messsages are shown; check exit code\n");
            Console.WriteLine("Exit codes:\n\t0 - replace succeeded\n\t2 - file not found\n\t3 - directory not found\n\t5 - access denied\n\t-1 - invalid command line syntax\n");

            base.Help();
        }
    }
}
