using System;
using System.Collections.Generic;
using System.Text;

namespace TPI_Simulacion.Models
{
    class GenerarVariableAleatoria
    {
        public RandomVariablegenerator RandomVariablegenerator { get; set; }

        public GenerarVariableAleatoria(string typevariable)
        {
            if (typevariable =="IA")
            {
                RandomVariablegenerator = new RandomVariablegenerator
                {
                    DecimalAmount = 10000,
                    A = 0,
                    B = 3,
                    RandomVariableDecimalAmount = 0,
                    RandomNumberDecimalAmount = 4,

                };
            }
            else
            {
                if (typevariable == "TA")
                {
                    RandomVariablegenerator = new RandomVariablegenerator
                    {
                        DecimalAmount = 10000,
                        A = 5,
                        B = 12,
                        RandomVariableDecimalAmount = 0,
                        RandomNumberDecimalAmount = 4,

                    };
                }
            }

        }

        public float GenerarR()
        {
            var r = RandomVariablegenerator.GenerateRandomNumber();   
            var x1 = RandomVariablegenerator.GenerateRandomVariableValue(r);
            return (float)x1;
        }
    }
}
