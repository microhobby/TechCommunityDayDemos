using System;
using System.Runtime.InteropServices;

namespace hello
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Arch {RuntimeInformation.OSArchitecture}");
            Console.WriteLine($"OS {RuntimeInformation.OSDescription}");
            Console.WriteLine("Hello World!");
        }
    }
}
