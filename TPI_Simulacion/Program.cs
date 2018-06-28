using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using TPI_Simulacion.Models;

namespace TPI_Simulacion
{
    class Program
    {
        public GenerarVariableAleatoria GeneradorIA { get; set; }
        public GenerarVariableAleatoria GeneradorTA { get; set; }

        #region Variables Endogenas y Exogenas
        //Cantidad de cadetes
        public int n { get; set; }

        //Tiempo comprometido de cada cadete
        public List<float> TC { get; set; }

        //Porcentaje de tiempo ocioso de cada cadete
        public List<float> PTO { get; set; }

        //Porcentaje de mandados que no se pudieron concretar debido al tiempo de demora por cada cadete
        public List<float> PMNC { get; set; }

        //Promedio de permanencia del cliente en el sistema por cada cadete
        public List<float> PPS { get; set; }

        //Promedio de tiempo en cola del cliente por cada cadete
        public List<float> PTE { get; set; }

        //Porcentaje de clientes que se arrepintieron porque el tiempo de espera era mayor a 18 minutos respecto del total de clientes que llamaron, por cada cadete
        public List<float> PA18 { get; set; }

        //Porcentaje de clientes que tuvieron el máximo tiempo de atención sobre todos los clientes atendidos: PCMax
        public List<float> PCMax { get; set; }

        //Porcentaje de clientes que tuvieron el mínimo tiempo de atención sobre todos los clientes atendidos: PCMin
        public List<float> PCMin { get; set; }

        //Máximo tiempo de espera de un cliente: MaxTA
        public float MaxTA { get; set; }

        #endregion

        #region Variables de Uso Interno

        public int T { get; set; }
        public int TPLL { get; set; }
        public int NL { get; set; }

        public float IA { get; set; }
        public float TA { get; set; }
        public int TF { get; set; }
        public List<float> N { get; set; }
        public List<float> STP { get; set; }
        public List<float> STA { get; set; }
        public List<float> STE { get; set; }
        public List<float> STO { get; set; }
        public List<float> NA { get; set; }
        public List<float> NE { get; set; }
        public List<float> NA18 { get; set; }
        public int ContMax { get; set; }
        public int MinTA { get; set; }
        public int MaxTE { get; set; }
        public int ContMin { get; set; }


        #endregion

        void Main(string[] args)
        {
            n = 0;
            while (n==0)
            {
                Console.WriteLine("Ingrese la cantidad de Cadetes.");
                var Ncadetes = Console.ReadLine();
                int.TryParse(Ncadetes, out var a);
                n = a;
            }

            TF = 0;
            while (n == 0)
            {
                Console.WriteLine("Ingrese el tiempo de duracion de la simulacion en minutos.");
                var Ncadetes = Console.ReadLine();
                int.TryParse(Ncadetes, out var a);
                TF = a;
            }

            GeneradorIA = new GenerarVariableAleatoria("IA");
            GeneradorTA = new GenerarVariableAleatoria("TA");

            CondicionesIniciales();

            while(T <= TF)
            {
                T = TPLL;
                IA = GeneradorIA.GenerarR();
                var TCi = BuscarMenorTC(TC);

                var NOarrepentido = true;

                if (T <= TC[TCi])
                {
                    NOarrepentido = TratarArrepentimiento();
                    if (NOarrepentido)
                    {
                        STE[TCi] += (TC[TCi] - T);
                        TratarEspera();
                        TC[TCi] += TA;
                        STA[TCi] += TA;
                        NE[TCi]++;
                    }
                    
                }
                else
                {
                    STO[TCi] += (T - TC[TCi]);
                    TC[TCi] += TA;
                    STA[TCi] += TA;
                }

                if (NOarrepentido)
                {
                    STP[TCi] += (TC[TCi] - T);
                    N[TCi]++;
                    TratarMaximo();
                    TratarMinimo();
                }

                NL++;
            }
            CalculoResultados();
            ImprimirResultados();
        }

        private void TratarEspera()
        {
            throw new NotImplementedException();
        }

        private void ImprimirResultados()
        {
            throw new NotImplementedException();
        }

        private void CalculoResultados()
        {
            throw new NotImplementedException();
        }

        private void TratarMinimo()
        {
            throw new NotImplementedException();
        }

        private void TratarMaximo()
        {
            throw new NotImplementedException();
        }

        private bool TratarArrepentimiento()
        {
            throw new NotImplementedException();
        }

        private int BuscarMenorTC(List<float> tc)
        {
            var min = tc.Min();
            return tc.FindIndex(p => p == min);
        }


        private void CondicionesIniciales()
        {
            T = 0;
            TPLL = 0;
            NL = 0;

            MaxTA = int.MinValue;
            ContMax = 0;
            MinTA = int.MaxValue ;
            ContMin = 0;
            MaxTE = int.MinValue;
            float defaultValue = 0;
            
            // cargar arreglos
            TC = Enumerable.Repeat(defaultValue, n).ToList();
            N = Enumerable.Repeat(defaultValue, n).ToList(); ;
            STP = Enumerable.Repeat(defaultValue, n).ToList(); ;
            STA = Enumerable.Repeat(defaultValue, n).ToList();
            STE = Enumerable.Repeat(defaultValue, n).ToList();
            STO = Enumerable.Repeat(defaultValue, n).ToList();
            NA = Enumerable.Repeat(defaultValue, n).ToList();
            NE = Enumerable.Repeat(defaultValue, n).ToList();
            NA18 = Enumerable.Repeat(defaultValue, n).ToList();
            
        }
    }
}
