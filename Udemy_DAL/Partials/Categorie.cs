using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy_DAL
{
    public partial class Categorie
    {
        public override bool Equals(object obj)
        {
            return obj is Categorie categorie &&
                   Id == categorie.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}

