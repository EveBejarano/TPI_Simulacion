using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using TPI_Simulacion.Models;

namespace TPI_Simulacion
{
    class Program
    {
        #region Variables Endogenas y Exogenas
        //Cantidad de cadetes
        public static int N { get; set; }

        //Tiempo comprometido de cada cadete
        public static List<int> TC { get; set; }

        //Porcentaje de tiempo ocioso de cada cadete
        public static List<float> PTO { get; set; }

        //Porcentaje de mandados que no se pudieron concretar debido al tiempo de demora por cada cadete
        public static List<float> PMNC { get; set; }

        //Promedio de permanencia del cliente en el sistema por cada cadete
        public static List<float> PPS { get; set; }

        //Promedio de tiempo en cola del cliente por cada cadete
        public static List<float> PTE { get; set; }

        //Porcentaje de clientes que se arrepintieron porque el tiempo de espera era mayor a 18 minutos respecto del total de clientes que llamaron, por cada cadete
        public static List<float> PA18 { get; set; }

        //Porcentaje de clientes que tuvieron el máximo tiempo de atención sobre todos los clientes atendidos: PCMax
        public static List<float> PCMax { get; set; }

        //Porcentaje de clientes que tuvieron el mínimo tiempo de atención sobre todos los clientes atendidos: PCMin
        public static List<float> PCMin { get; set; }

        //Máximo tiempo de espera de un cliente: MaxTA
        public static int MaxTA { get; set; }

        #endregion

        #region Variables de Uso Interno

        public static int T { get; set; }
        public static int TPLL { get; set; }
        public static int NL { get; set; }
        public static int TCi { get; set; }
        public static int Ni { get; set; }
        public static int STPi { get; set; }
        public static int STAi { get; set; }
        public static int STEi { get; set; }
        public static int STOi { get; set; }
        public static int NAi { get; set; }
        public static int NEi { get; set; }
        public static int NA18i { get; set; }
        public static int ContMax { get; set; }
        public static int MinTA { get; set; }
        public static int ContMin { get; set; }


        #endregion

        static void Main(string[] args)
        {
            N = 0;
            while (N==0)
            {
                Console.WriteLine("Ingrese la cantidad de Cadetes");
                var Ncadetes = Console.ReadLine();
                int.TryParse(Ncadetes, out var a);
                N = a;
            }

            CondicionesIniciales();
        }

        private static void CondicionesIniciales()
        {
            T = 0;
            TPLL = 0;
            NL = 0;
            TCi = 0;
            Ni = 0;
            STPi = 0;
            STAi = 0;
            STEi = 0;
            STOi = 0;
            NAi = 0;
            NEi = 0;
            NA18i = 0;
            MaxTA = int.MaxValue;
            ContMax = 0;
            MinTA = int.MaxValue ;
            ContMin = 0;
        }
    }
}
