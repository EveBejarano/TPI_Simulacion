using System;
using TPI_Simulacion.Models;

namespace TPI_Simulacion
{
    class Program
    {
        static void Main(string[] args)
        {
            int randomVariableAmount;
            var path = "Lote1.txt";
            const string whiteSpace = "   ";

            var generator = new RandomVariablegenerator
            {
                DecimalAmount = 10000,
                Fdp = 0.002083
            };
            Console.WriteLine("         Modulo del programa que se encargara de generar los valores de las variables del LOTE 1");
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            Console.WriteLine("         Funcion inversa para el LOTE 1");
            Console.WriteLine("         X1_i = 0.002083 * Ri");
            Console.WriteLine("");
            Console.WriteLine("====================================================================================================");
            
            // Obtiene la cantidad de valores de la variable aleatoria que se desean obtener.
            while (true)
            {
                Console.WriteLine(whiteSpace + "Cuantos valores aleatorios de la variable desea generar?");
                Int16.TryParse(Console.ReadLine(), out var a);
                randomVariableAmount = a;

                if (randomVariableAmount <0 )
                {
                    Console.WriteLine("El número ingresado no es válido. Ingrese un valor entero positivo.");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine(whiteSpace + "A continuación,Se mostraran los R_i y los X_1 generados:");
            Console.WriteLine();

            for (var j = 1; j <= randomVariableAmount; j++)
            {
                var r = generator.GenerateRandomNumber();
                var x1 = generator.GenerateRandomVariableValue(r);
                Console.WriteLine("{0} R1_{1} : {2}", whiteSpace, j , r);   //esta es para control nomas
                Console.WriteLine("{0} X1_{1} : {2}", whiteSpace, j, x1);   //esto es para control nomas
                Console.WriteLine();

            }
            Console.WriteLine(whiteSpace, "PROCESO DE GENERACION DE VALORES DE LA VARIABLE ALEATORIA FINALIZADO - SE GENERO EL DOCUMENTO LOTE 1");
            Console.ReadLine();
        }
    }
}
