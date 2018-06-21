using System;
using TPI_Simulacion.Models;

namespace TPI_Simulacion
{
    class Program
    {
        static void Main(string[] args)
        {
            RandomVariableGenerator Generator = new RandomVariableGenerator
            {
                DecimalAmount = 10000,
                fdp = 0.002083
            };

            Console.WriteLine("Hello World!");
        }
    }
}
