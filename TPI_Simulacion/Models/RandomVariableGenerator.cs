using System;

namespace TPI_Simulacion.Models
{
    class RandomVariablegenerator
    {

        public float DecimalAmount { get; set; }

        public double A { get; set; }
        public double B { get; set; }
        public int RandomVariableDecimalAmount { get; set; }
        public int RandomNumberDecimalAmount { get; set; }

        public double GenerateRandomNumber()
        {
            var randomNumbergenerator = new Random();
            float r = (randomNumbergenerator.Next(0, (int)DecimalAmount));
            r = r / DecimalAmount;
            var roundedR = (float)Math.Round(r, RandomNumberDecimalAmount);
            return roundedR;
        }

        public double GenerateRandomVariableValue(double r)
        {
            r =(A + (B - A) * r);
            var roundedR = (float)Math.Round(r, RandomVariableDecimalAmount);
            return roundedR; 
        }

        // decimal d = Decimal.Parse("1.2345E-02", System.Globalization.NumberStyles.Float);
    }
}
