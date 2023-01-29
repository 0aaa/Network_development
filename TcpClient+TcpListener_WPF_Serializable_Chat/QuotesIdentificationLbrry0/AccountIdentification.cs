using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesIdentificationLbrry0
{
    public class AccountIdentification
    {
        public PseudoPasswordStrctre[] PseudoPasswordArr { get; set; }
        public AccountIdentification()
        {
            PseudoPasswordArr = new PseudoPasswordStrctre[3];
            for (short i = 0; i < PseudoPasswordArr.Length; i++)
            {
                PseudoPasswordArr[i] = new PseudoPasswordStrctre()
                {
                    Pseudo = "user" + i,
                    Password = "user" + i
                };
            }
        }
    }
}
