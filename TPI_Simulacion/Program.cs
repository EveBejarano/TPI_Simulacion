using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using ClosedXML.Excel;
using TPI_Simulacion.Models;

namespace TPI_Simulacion
{
    class Program
    {
        public static GenerarVariableAleatoria GeneradorIA { get; set; }
        public static GenerarVariableAleatoria GeneradorTA { get; set; }

        #region Variables Endogenas y Exogenas
        //Cantidad de cadetes
        public static int n { get; set; }

        //Tiempo comprometido de cada cadete
        public static List<float> TC { get; set; }

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
        public static float PCMax { get; set; }

        //Porcentaje de clientes que tuvieron el mínimo tiempo de atención sobre todos los clientes atendidos: PCMin
        public static float PCMin { get; set; }

        //Máximo tiempo de espera de un cliente: MaxTA
        public static float MaxTA { get; set; }

        #endregion

        #region Variables de Uso Interno

        public static float T { get; set; }
        public static float TPLL { get; set; }
        public static int NL { get; set; }

        public static float IA { get; set; }
        public static float TA { get; set; }
        public static int TF { get; set; }
        public static List<float> N { get; set; }
        public static List<float> STP { get; set; }
        public static List<float> STA { get; set; }
        public static List<float> STE { get; set; }
        public static List<float> STO { get; set; }
        public static List<float> NA { get; set; }
        public static List<float> NE { get; set; }
        public static List<float> NA18 { get; set; }
        public static int ContMax { get; set; }
        public static float MinTA { get; set; }
        public static float MaxTE { get; set; }
        public static int ContMin { get; set; }


        #endregion

        static void Main(string[] args)
        {
            n = 0;
            while (n==0)
            {
                Console.WriteLine("Ingrese la cantidad de cadetes.");
                var Ncadetes = Console.ReadLine();
                int.TryParse(Ncadetes, out var a);
                n = a;
            }



            GeneradorIA = new GenerarVariableAleatoria("IA");
            GeneradorTA = new GenerarVariableAleatoria("TA");

            Console.WriteLine();
            Console.WriteLine("____________________________________________________________________");
            // Condiciones Iniciales
            CondicionesIniciales();
            while (T <= TF)
            {

                
                // T= TPLL
                T = TPLL;
                
                // Gen IA
                IA = GeneradorIA.GenerarR();

                // TPLL = T + IA
                TPLL += IA;
                
                // Gen TA
                TA = GeneradorTA.GenerarR();
                
                //Buscar Menor TCi
                var TCi = BuscarMenorTC(TC);
                
                var NOarrepentido = true;

                // T < TCi
                if (T < TC[TCi])
                {
                    // Tratar Arrepentimiento
                    NOarrepentido = TratarArrepentimiento(TC[TCi], TCi);
                    
                    // No arrepentido
                    if (NOarrepentido)
                    {
                        STE[TCi] += (TC[TCi] - T);
                        TratarEspera(TCi);

                        
                        TC[TCi] += TA;
                        STA[TCi] += TA;
                        NE[TCi]++;
                    }
                    
                }
                // T> TCi
                else
                {
                    STO[TCi] += (T - TC[TCi]);
                    TC[TCi] = T + TA;
                    STA[TCi] += TA;
                }
                // Fin condicional T<= TCi

                if (NOarrepentido)
                {
                    STP[TCi] += (TC[TCi] - T);
                    N[TCi]++;
                    TratarMaximo();
                    TratarMinimo();
                }
                // Si se arrepintio el cliente cuando T<= TCi, entonces "retorna aca"

                NL++;
            }

            CalculoResultados();
            ImprimirResultados();
        }


        private static  void CondicionesIniciales()
        {
            T = 0;
            TF = 377;
            TPLL = 0;
            NL = 0;

            MaxTA = int.MinValue;
            ContMax = 0;
            MinTA = int.MaxValue;
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

        private static  int BuscarMenorTC(List<float> tc)
        {
            var min = tc.Min();
            return tc.FindIndex(p => p == min);
        }

        private static  bool TratarArrepentimiento(float tci, int indexTCi)
        {
            // TE = TCi - T
            var TE = tci - T;

            // Generar r
            var randomNumbergenerator = new Random();
            float r = (float)(randomNumbergenerator.NextDouble());
            
            if (TE <= 15)
            {
                if (r <= 0.62)
                {
                    return true;
                }
            }
            else
            {
                if (TE <= 20)
                {
                    if (r <= 0.15)
                    {
                        return true;
                    }

                }
                if (TE >= 18)
                {
                    //NA18i = NA18i + 1
                    NA18[indexTCi]++;
                    
                }
            }

            //NAi = NAi + 1
            NA[indexTCi]++;
            
            return false;
        }

        private static  void TratarEspera(int indexTCi)
        {
            if (TC[indexTCi] - T > MaxTE)
            {
                MaxTE = TC[indexTCi] - T;
            }
        }

        private static  void CalculoResultados()
        {
            PTO = Enumerable.Repeat((float)0, n).ToList();
            PPS = Enumerable.Repeat((float)0, n).ToList();
            PTE = Enumerable.Repeat((float)0, n).ToList();
            PMNC = Enumerable.Repeat((float)0, n).ToList();
            PA18 = Enumerable.Repeat((float)0, n).ToList();

            for (int i = 0; i < n; i++)
            {
                PTO[i] = (STO[i] * 100) / T;
                PPS[i] = STP[i] / N[i];
                PTE[i] = STE[i] / N[i];
                PMNC[i] = (NA[i] * 100) / NL;
                PA18[i] = (NA18[i] * 100) / NL;
                
            }
            PCMax = (ContMax * 100) / N.Sum();
            PCMin = (ContMin * 100) / N.Sum();

            if (MaxTE <0)
            {
                MaxTE = 0;
            }

        }

        private static  void ImprimirResultados()
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Simulacion");
            var index = 1;


            index++;
            
            ws.Cell("A" + index).Value = "Terminos Utilizados:";


            var rango = "A"+ index;
            var rngnegrita = ws.Range(rango);
            rngnegrita.Style.Font.Bold = true;

            index++;
            var star = index;
            ws.Cell("A" + index).Value = "PTO: ";
            ws.Cell("B" + index).Value = "Porcentaje de tiempo ocioso.";
            index++;
            ws.Cell("A" + index).Value = "PMNC:";
            ws.Cell("B" + index).Value = "Porcentaje de mandados que no se pudieron concretar debido al tiempo de demora.";
            index++;
            ws.Cell("A" + index).Value = "PPS:";
            ws.Cell("B" + index).Value = "Promedio de permanencia del cliente en el sistema.";
            index++;
            ws.Cell("A" + index).Value = "PTE:";
            ws.Cell("B" + index).Value = "Promedio de tiempo en cola del cliente.";
            index++;
            ws.Cell("A" + index).Value = "PA18:";
            ws.Cell("B" + index).Value = "Porcentaje de clientes que se arrepintieron porque el tiempo de espera era mayor a 18 minutos respecto del total de clientes que llamaron.";
            index++;
            ws.Cell("A" + index).Value = "PCMax:";
            ws.Cell("B" + index).Value = "Porcentaje de clientes que tuvieron el máximo tiempo de atención sobre todos los clientes atendidos.";
            index++;
            ws.Cell("A" + index).Value = "PCMin:";
            ws.Cell("B" + index).Value = "Porcentaje de clientes que tuvieron el mínimo tiempo de atención sobre todos los clientes atendidos.";
            index++;
            ws.Cell("A" + index).Value = "MaxTE:";
            ws.Cell("B" + index).Value = "Máximo tiempo de espera de un cliente en minutos.";
            
            index++;
            var fin = index;
            index++;
            index++;

            rango = "A" + star + ":A" + fin;
            rngnegrita = ws.Range(rango);
            rngnegrita.Style.Font.Bold = true;

            rango = "A" + index + ":F" + index;
            var rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Style.Font.Bold = true;
            rngTable.Style.Font.FontColor = XLColor.Black;
            rngTable.Style.Fill.BackgroundColor = XLColor.Cyan;
            rngTable.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            ws.Cell("A" + index).Value = "Tabla De Resultados";

            index++;
            ws.Cell("A" + index).Value = "Nro Cadetes";
            ws.Cell("D" + index).Value = n;

            rango = "A" + index + ":C" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.Cyan;
            rngTable.Style.Font.Bold = true;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            rango = "D" + index + ":F" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.LightCyan;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;



            index++;
            ws.Cell("A" + index).Value = "PCMax";
            ws.Cell("D" + index).Value = PCMax;

            rango = "A" + index + ":C" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.Cyan;
            rngTable.Style.Font.Bold = true;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            rango = "D" + index + ":F" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.LightCyan;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            index++;
            ws.Cell("A" + index).Value = "PCMin";
            ws.Cell("D" + index).Value = PCMin;


            rango = "A" + index + ":C" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.Cyan;
            rngTable.Style.Font.Bold = true;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            rango = "D" + index + ":F" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.LightCyan;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            index++;
            ws.Cell("A" + index).Value = "MaxTE";
            ws.Cell("D" + index).Value = MaxTE;


            rango = "A" + index + ":C" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.Cyan;
            rngTable.Style.Font.Bold = true;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            rango = "D" + index + ":F" + index;
            rngTable = ws.Range(rango);
            rngTable.FirstRow().Merge();
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTable.Style.Fill.BackgroundColor = XLColor.LightCyan;

            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            index++;
            ws.Cell("A" + index).Value = "N° Cadete";
            ws.Cell("B" + index).Value = "PTO";
            ws.Cell("C" + index).Value = "PMNC";
            ws.Cell("D" + index).Value = "PPS";
            ws.Cell("E" + index).Value = "PTE";
            ws.Cell("F" + index).Value = "PA18";

            rango = "A" + index + ":F" + index;
            var rngHeaders = ws.Range(rango); // The address is relative to rngTable (NOT the worksheet)
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Font.FontColor = XLColor.Black;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Cyan;
            rngHeaders.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rngHeaders.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngHeaders.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngHeaders.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            


            Console.WriteLine();
            Console.WriteLine("PTO: el Porcentaje de tiempo ocioso.");
            Console.WriteLine("PMNC: el Porcentaje de mandados que no se pudieron concretar debido al tiempo de demora.");
            Console.WriteLine("PPS: el Promedio de permanencia del cliente en el sistema.");
            Console.WriteLine("PTE: el Promedio de tiempo en cola del cliente.");
            Console.WriteLine("PA18: el Porcentaje de clientes que se arrepintieron porque el tiempo de espera era mayor a 18 minutos respecto del total de clientes que llamaron.");
            Console.WriteLine();
            Console.WriteLine("| Nº Cadetes | PTO     | PMNC     | PPS      | PTE      |  PA18     |");


            rango = "A" + index + ":F" + index;
            rngTable = ws.Range(rango);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var indexend = index + n;
            rango = "A" + indexend + ":F" + indexend;
            rngTable = ws.Range(rango);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;


            rango = "A" + index + ":A" + indexend;
            rngTable = ws.Range(rango);
            rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;


            rango = "F" + index + ":F" + indexend;
            rngTable = ws.Range(rango);
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            index++;
            for (int i = 0; i < n; i++)
            {
                
                ws.Cell("A"+index).Value = i;
                ws.Cell("B" + index).Value = PTO[i];
                ws.Cell("C" + index).Value = PMNC[i];
                ws.Cell("D" + index).Value = PPS[i];
                ws.Cell("E" + index).Value = PTE[i];
                ws.Cell("F" + index).Value = PA18[i];

                rango = "A"+index+":F" + index;
                var Table = ws.Range(rango); // The address is relative to rngTable (NOT the worksheet)
                Table.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                Table.Style.Fill.BackgroundColor = XLColor.LightCyan;

                index++;
                Console.WriteLine("|    {0}      | {1}   | {2}   | {3}   | {4}   | {5}   |", i, PTO[i], PMNC[i], PPS[i], PTE[i], PA18[i]);
            }


            Console.WriteLine();
            Console.WriteLine("El Porcentaje de clientes que tuvieron el máximo tiempo de atención sobre todos los clientes atendidos es {0}.", PCMax);
            Console.WriteLine();
            Console.WriteLine("El Porcentaje de clientes que tuvieron el mínimo tiempo de atención sobre todos los clientes atendidos es {0}.", PCMin);
            Console.WriteLine();
            Console.WriteLine("El Máximo tiempo de espera de un cliente es {0} minutos.", (short)MaxTE);
            
            string path = "TPiSimulacion-" + System.DateTime.Now + ".xlsx";
            path = path.Replace(":", ".");
            wb.SaveAs(path);
            Console.ReadLine();

            ;
        }



        private static  void TratarMinimo()
        {
            if (TA < MinTA)
            {
                MinTA = TA;
                ContMin = 1;
            }
            else
            {
                if (TA == MinTA)
                {
                    ContMin++;
                }
            };
        }

        private static  void TratarMaximo()
        {
            if (TA > MaxTA)
            {
                MaxTA = TA;
                ContMax = 1;
            }
            else
            {
                if (TA == MaxTA)
                {
                    ContMax ++;
                }
            }
        }




    }
}


