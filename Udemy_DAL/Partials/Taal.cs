using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Udemy_DAL
{
    public partial class Taal
    {
        public override bool Equals(object obj)
        {
            return obj is Taal taal &&
                   Id == taal.Id && Naam == taal.Naam;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}

