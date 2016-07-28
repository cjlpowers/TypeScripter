using System;

namespace TypeScripter.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NUnit.Runner.Program.Main(new[] { "TypeScripter.Tests.dll" });
        }
    }
}