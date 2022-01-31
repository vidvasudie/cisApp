using System;
using System.Collections.Generic;
using System.Text;

namespace cisApp.library
{
    public class Utility
    {
        public static bool IsValidIdentityFormat(string identity)
        {
            char[] numberChars = identity.ToCharArray();

            int total = 0;
            int mul = 13;
            int mod = 0, mod2 = 0;
            int nsub = 0;
            int numberChars12 = 0;

            for (int i = 0; i < numberChars.Length - 1; i++)
            {
                int num = 0;
                int.TryParse(numberChars[i].ToString(), out num);

                total = total + num * mul;
                mul = mul - 1;

                //Debug.Log(total + " - " + num + " - "+mul);
            }

            mod = total % 11;
            nsub = 11 - mod;
            mod2 = nsub % 10;

            int.TryParse(numberChars[12].ToString(), out numberChars12);

            if (mod2 != numberChars12)
                return false;
            else
                return true;
        }

        public static string GenerateRequestCode(string format, int number, bool reRun = false)
        {
            //request pattern UDSyyMMxxx1 
            string year = DateTime.Now.Year < 2500 ? String.Format("{0}", DateTime.Now.Year + 543).Substring(2, 2) : String.Format("{0}", DateTime.Now.Year).Substring(2, 2);
            string month = DateTime.Now.Month.ToString("00");
            if (reRun)
            {
                number = 1;
            }
            return String.Format(format, year, month, number.ToString("00000"));
        }


        public static string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(2, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public static string RandomOtpNumber(int size = 6)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                builder.Append(RandomNumber(0, 9));
            }
            return builder.ToString();
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


    }
}
