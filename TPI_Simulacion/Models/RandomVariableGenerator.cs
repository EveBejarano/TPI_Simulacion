using System;

namespace TPI_Simulacion.Models
{
    class RandomVariablegenerator
    {

        public float DecimalAmount { get; set; }

        public double Fdp { get; set; }

        public double GenerateRandomNumber()
        {
            var randomNumbergenerator = new Random();
            float r = (randomNumbergenerator.Next(0, (int)DecimalAmount));
            r = r / DecimalAmount;
            return r;
        }

        public double GenerateRandomVariableValue(double r)
        {
            return (Fdp * r);
        }

    }
}
