using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Udemy_Models;

namespace Udemy_DAL
{
    public partial class Lesgever: BasisKlasse
    {
        public override string this[string veldnaam]
        {
            get
            {
                if (veldnaam == "Voornaam" && string.IsNullOrWhiteSpace(Voornaam))
                {
                    return "Gelieve een voornaam in te vullen.";
                }
                if (veldnaam == "Naam" && string.IsNullOrWhiteSpace(Naam))
                {
                    return "Gelieve een familienaam in te vullen.";
                }              
                if (veldnaam == "Email" && !(IsEenValideEmailAdres(Email)))
                {
                     return "Gelieve een valide e-mail adres in te geven.";
                }
                if (veldnaam == "Paswoord" && !(IsEenValidePaswoord(Paswoord)))
                {
                    return "Gelieve een paswoord in te vullen met volgende eigenschappen:." + Environment.NewLine
                        + "\t Groter dan 8 letters" + Environment.NewLine
                        + "\t Minstens 1 hoofdletter" + Environment.NewLine
                        + "\t Minstens 1 kleine letter" + Environment.NewLine
                        + "\t Minstens 1 cijfer";
                }
                if (veldnaam == "Straat" && string.IsNullOrWhiteSpace(Straat))
                {
                    return "Gelieve uw straat in te voeren." + Environment.NewLine;
                }
                if (veldnaam == "Huisnummer" && string.IsNullOrWhiteSpace(Huisnummer))
                {
                    return "Gelieve een huisnummer in te voeren." + Environment.NewLine;
                }
                if (veldnaam == "Postcode" && string.IsNullOrWhiteSpace(Postcode))
                {
                    return "Gelieve een postcode in te voeren" + Environment.NewLine;
                }
                if (veldnaam == "Stad" && string.IsNullOrWhiteSpace(Stad))
                {
                    return "Gelieve een gemeente in te voeren" + Environment.NewLine;
                }
                if (veldnaam == "Land" && string.IsNullOrWhiteSpace(Land))
                {
                    return "Gelieve een land in te voeren" + Environment.NewLine;
                }
                if (veldnaam == "Beschrijving" && string.IsNullOrWhiteSpace(Beschrijving))
                {
                    return "Gelieve een beschrijving bij te voegen" + Environment.NewLine;
                }
                return "";
            }
        }
        static bool IsEenValideEmailAdres(string emailadres)
        {
            return Regex.IsMatch(emailadres, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }

        static bool IsEenValidePaswoord(string paswoord)
        {
            Regex heeftNummer = new Regex(@"[0-9]+");
            Regex heeftHoofdletter = new Regex(@"[A-Z]+");
            Regex minLengteIs8 = new Regex(@".{8,}");
            var isValidated = heeftNummer.IsMatch(paswoord) && heeftHoofdletter.IsMatch(paswoord) && minLengteIs8.IsMatch(paswoord);
            return isValidated;
        }

        public override bool Equals(object obj)
        {
            return obj is Lesgever lesgever &&
                   Id == lesgever.Id &&
                   Email == lesgever.Email;
        }

        public override int GetHashCode()
        {
            int hashCode = -1058553241;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            return hashCode;
        }
    }
}
