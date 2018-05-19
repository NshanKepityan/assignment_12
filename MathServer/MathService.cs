using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MathServer
{
    public class MathService : IMathService
    {
        public double Add(double firstValue, double secondValue)
        {
            return firstValue + secondValue;
        }

        public double Sub(double firstValue, double secondValue)
        {
            return firstValue - secondValue;
        }

        public double Div(double firstValue, double secondValue)
        {
            return (secondValue != 0) ? firstValue / secondValue : throw new ArgumentOutOfRangeException();
        }

        public double Mult(double firstValue, double secondValue)
        {
            return firstValue * secondValue;
        }
    }

}