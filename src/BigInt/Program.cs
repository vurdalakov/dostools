namespace Vurdalakov
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Help();
            }

            switch (args[0])
            {
                case "fromhex":
                    Console.WriteLine("{0}", BigInt.FromHex(args[1]));
                    break;
                case "tohex":
                    Console.WriteLine("{0}", BigInt.ToHex(args[1]));
                    break;
                default:
                    Help();
                    break;
            }
        }

        static void Help()
        {
            Console.WriteLine("bigint fromhex <hex string>");
            Console.WriteLine("bigint tohex <int string>");
            Environment.Exit(-1);
        }
    }
}
