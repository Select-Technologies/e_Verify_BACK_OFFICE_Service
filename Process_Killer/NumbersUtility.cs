using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
    public static class NumbersUtility
    {
    
        public static int FindNumberLength(int number)
        {
            return Convert.ToInt32( Math.Floor(Math.Log(number,10))+1);
        }

        public static int FindNumberDivisor(int number)
        {
            return Convert.ToInt32(Math.Pow(10, FindNumberLength(number)-1));
        }

        public static int[] FindNumberElements(int number)
        {
            int[] elements = new int[FindNumberLength(number)];
            int divisor = FindNumberDivisor(number);
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = number/divisor;
                number %= divisor;
                divisor /= 10;
            }
            return elements;
        }
    }
}
