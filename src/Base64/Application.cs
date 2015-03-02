﻿namespace Vurdalakov
{
    using System;
    using System.IO;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            var input = "";

            switch (_commandLineParser.FileNames.Length)
            {
                case 0:
                    if (_commandLineParser.IsOptionSet("e", "encode", "d", "decode", "ef", "encodefile", "df", "decodefile"))
                    {
                        input += Console.In.ReadToEnd().TrimEnd(Environment.NewLine.ToCharArray());
                    }
                    else
                    {
                        Help();
                    }
                    break;
                case 1:
                    input = _commandLineParser.FileNames[0];
                    break;
                default:
                    Help();
                    break;
            }

            var maxLineLength = _commandLineParser.GetOptionInt("l", "length", 76);

            var output = "";

            if (_commandLineParser.IsOptionSet("e", "encode"))
            {
                output = Base64Converter.Encode(input, maxLineLength);
            }
            else if (_commandLineParser.IsOptionSet("d", "decode"))
            {
                output = Base64Converter.Decode(input);
            }
            else if (_commandLineParser.IsOptionSet("ef", "encodefile"))
            {
                input = File.ReadAllText(input);
                output = Base64Converter.Encode(input, maxLineLength);
            }
            else if (_commandLineParser.IsOptionSet("df", "decodefile"))
            {
                input = File.ReadAllText(input);
                output = Base64Converter.Decode(input);
            }
            else
            {
                Help();
            }

            if (_commandLineParser.IsOptionSet("o", "output"))
            {
                var outputFile = _commandLineParser.GetOptionString("o", "output", null);
                if (String.IsNullOrEmpty(outputFile))
                {
                    Help();
                }

                File.WriteAllText(outputFile, output);
            }
            else
            {
                Console.WriteLine(output);
            }

            return 0;
        }

        protected override void Help()
        {
            Console.WriteLine("Base64 converter 1.0 | (c) Vurdalakov | https://github.com/vurdalakov/dostools\n");
            Console.WriteLine("Encodes and decodes base64 strings\n");
            Console.WriteLine("Usage:\n\tbase64 <filename | text> <-e | -d | -ef | -df> [-output:filename] [-length:0] [-silent]\n");
            Console.WriteLine("Exit codes:\n\t0 - conversion succeeded\n\t1 - conversion failed\n\t-1 - invalid command line syntax\n");
            
            base.Help();
        }
    }
}
