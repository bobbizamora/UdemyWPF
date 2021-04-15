using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy_DAL
{
    public static class Inloggegevens //gebruiker gegevens in een static class zetten zodat dit in de andere formulieren kan gebruikt worden
    {
        public static int Id { get; set; }
        public static string Naam { get; set; }
        public static string Voornaam { get; set; }
    }
}
