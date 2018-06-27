using System;

namespace TPI_Simulacion.Models
{
    class RandomVariablegenerator
    {

        internal int DecimalAmount { get; set; }

        public float A { get; set; }
        public float B { get; set; }
        public int RandomVariableDecimalAmount { get; set; }
        public int RandomNumberDecimalAmount { get; set; }

        public float GenerateRandomNumber()
        {
            var randomNumbergenerator = new Random();
            DecimalAmount = (int)Math.Pow(10, RandomNumberDecimalAmount);
            float r = (randomNumbergenerator.Next(0, DecimalAmount));
            r = r / DecimalAmount;
            return r;
        }

        public float GenerateRandomVariableValue(float r)
        {
            r =(A + (B - A) * r);
            float roundedR = (float)Math.Round(r, RandomVariableDecimalAmount);
            return roundedR; 
        }

        // decimal d = Decimal.Parse("1.2345E-02", System.Globalization.NumberStyles.Float);
    }
}
