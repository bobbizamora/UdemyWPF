using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy_DAL
{
    public partial class Niveau
    {
        public override bool Equals(object obj)
        {
            return obj is Niveau niveau &&
                   Id == niveau.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
