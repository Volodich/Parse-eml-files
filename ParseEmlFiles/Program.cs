using System;

namespace ParseEmlFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input path to directory: ");
            string dir = Console.ReadLine();

            ParseEmlService parse = new ParseEmlService(dir);
            parse.StartParseAsync().Wait();

            Console.WriteLine("Program end work");
            Console.ReadKey();
        }

    }
}
