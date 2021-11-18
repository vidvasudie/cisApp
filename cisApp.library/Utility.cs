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
    }
}
