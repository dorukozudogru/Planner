using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Helpers
{
    public class CitizenshipNoHelper
    {
        public static bool ControlCitizenshipNo(string citizenshipNo)
        {
            bool returnvalue;
            try
            {
                if (citizenshipNo.Length == 11)
                {
                    Int64 atcno, btcno, tcno;
                    long c1, c2, c3, c4, c5, c6, c7, c8, c9, q1, q2;
                    tcno = Int64.Parse(citizenshipNo);
                    atcno = tcno / 100;
                    btcno = tcno / 100;
                    c1 = atcno % 10;
                    atcno = atcno / 10;
                    c2 = atcno % 10;
                    atcno = atcno / 10;
                    c3 = atcno % 10;
                    atcno = atcno / 10;
                    c4 = atcno % 10;
                    atcno = atcno / 10;
                    c5 = atcno % 10;
                    atcno = atcno / 10;
                    c6 = atcno % 10;
                    atcno = atcno / 10;
                    c7 = atcno % 10;
                    atcno = atcno / 10;
                    c8 = atcno % 10;
                    atcno = atcno / 10;
                    c9 = atcno % 10;
                    atcno = atcno / 10;
                    q1 = ((10 - ((((c1 + c3 + c5 + c7 + c9) * 3) + (c2 + c4 + c6 + c8)) % 10)) % 10);
                    q2 = ((10 - (((((c2 + c4 + c6 + c8) + q1) * 3) + (c1 + c3 + c5 + c7 + c9)) % 10)) % 10);
                    return returnvalue = ((btcno * 100) + (q1 * 10) + q2 == tcno);
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}