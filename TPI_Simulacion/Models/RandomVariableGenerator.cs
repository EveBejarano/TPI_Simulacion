using System;
using System.Collections.Generic;
using System.Text;

namespace TPI_Simulacion.Models
{
    class RandomVariableGenerator
    {

        public int DecimalAmount { get; set; }

        public double fdp { get; set; }

        public float GenerateRandomVariable()
        {
            Random RandomNumberGenerator = new Random();
            var r = RandomNumberGenerator.Next(0, DecimalAmount);
            r = r / DecimalAmount;
            return r;
        }

        public double GenerateRandomNumber(float r)
        {
            return (fdp * r);
        }

    }
}
