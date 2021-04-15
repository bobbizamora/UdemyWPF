using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy_Models;

namespace Udemy_DAL
{
    public partial class Cursus: BasisKlasse
    {
        public override string this[string veldnaam]
        {
            get
            {
                if (veldnaam == "Naam" && string.IsNullOrWhiteSpace(Naam))
                {
                    return "Gelieve de cursus een naam te geven";
                }                
                if (veldnaam == "Beschrijving" && string.IsNullOrWhiteSpace(Beschrijving))
                {
                    return "Gelieve een beschrijving bij de cursus te formuleren";
                }

                return "";
            }
        }

    }
}
