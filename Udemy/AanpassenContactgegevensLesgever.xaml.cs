using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Udemy_DAL;



namespace Udemy
{
    /// <summary>
    /// Interaction logic for aanpassenDocent.xaml
    /// </summary>
    public partial class AanpassenContactgegevensLesgever : Window
    {
        public AanpassenContactgegevensLesgever()
        {
            InitializeComponent();
        }

        Lesgever lesgever = new Lesgever();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblGebruiker.Content = $"{Inloggegevens.Voornaam} {Inloggegevens.Naam}";
            // Ophalen lesgever die ingelogd is en inladen in formulier
            lesgever = DatabaseOperations.OphalenLesgeverViaId(Inloggegevens.Id);
            if (lesgever == null)
            {
                MessageBox.Show("Kan uw gegevens niet vinden.");
            }
            else
            {
                //opvullen formulier met gegevens van opgehaalde lesgever
                txtVoornaam.Text = lesgever.Voornaam;
                txtNaam.Text = lesgever.Naam;
                txtEmailAdres.Text = lesgever.Email;
                pwbPaswoord.Password = lesgever.Paswoord;
                pwbControlePaswoord.Password = lesgever.Paswoord;
                txtStraat.Text = lesgever.Straat;
                txtHuisnummer.Text = lesgever.Huisnummer;
                txtPostcode.Text = lesgever.Postcode;
                txtGemeente.Text = lesgever.Stad;
                txtLand.Text = lesgever.Land;
                txtBoxBeschrijving.Text = lesgever.Beschrijving;
            }
           
        }        
        private void btnTerugNaarOverzicht_Click(object sender, RoutedEventArgs e)
        {
           
            OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
            overzichtLesgever.Show();
            this.Close();
        }
        private void btnAanpassenGegevens_Click(object sender, RoutedEventArgs e)
        {
            //opvullen gegevens lesgever door middel van de formuliervelden
            lesgever.Voornaam = txtVoornaam.Text;
            lesgever.Naam = txtNaam.Text;
            lesgever.Email = txtEmailAdres.Text.ToLower();
            lesgever.Paswoord = pwbPaswoord.Password;
            lesgever.Straat = txtStraat.Text;
            lesgever.Huisnummer = txtHuisnummer.Text;
            lesgever.Postcode = txtPostcode.Text;
            lesgever.Stad = txtGemeente.Text;
            lesgever.Land = txtLand.Text;
            lesgever.Beschrijving = txtBoxBeschrijving.Text;
            //validatie uitvoeren
            string foutmelding = Valideer("pwbControlePaswoord");
            foutmelding += Valideer("Email");

            if (lesgever.IsGeldig() && string.IsNullOrWhiteSpace(foutmelding))//valideren lesgever via partial klasse methode is geldig + kijken of de foutmeldingen leeg zijn
            {
                int ok = DatabaseOperations.AanpassenGegevensLesgever(lesgever);
                if (ok > 0)// Als de gegevens van de lesgever gewijzigt zijn wordt de gebruiker terug geleid naar hun overzicht
                {
                    MessageBox.Show("Uw gegevens zijn aangepast.");
                    Wissen();
                    OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
                    overzichtLesgever.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Uw gegevens zijn niet veranderd!");
                }
            }
            else
            {
                MessageBox.Show(lesgever.Error + Environment.NewLine + foutmelding);//tonen foutmeldingen
                if (foutmelding.Contains("Uw controle paswoord komt niet overeen met uw paswoord") || lesgever.Error.Contains("Gelieve een paswoord in te vullen met volgende eigenschappen"))
                    //Indien er een fout staat in foutmeldingen en lesgever.Error omwille van het paswoord worden de velden die met het paswoord te maken hebben terug leeg gehaald.
                {
                    pwbControlePaswoord.Password = "";
                    pwbPaswoord.Password = "";
                }
            }            
        }

        private string Valideer(string veldnaam)//validatie
        {
            
            if (veldnaam == "pwbControlePaswoord" && pwbControlePaswoord.Password != pwbPaswoord.Password )
            {
                return "Uw controle paswoord komt niet overeen met uw paswoord." + Environment.NewLine;
            }
            if (veldnaam == "Email" && !(NakijkenEmail(lesgever.Email, lesgever)))
            {
                return "Het emailadres dat u heeft gekozen is al toegewezen aan iemand anders of is niet ingevuld. Gelieve een nieuw e-mail adres te kiezen" + Environment.NewLine;
            }
            return "";
        }
        private void Wissen()//gegevens wissen
        {
            lesgever.Voornaam = "";
            lesgever.Naam = "";
            lesgever.Email = "";
            lesgever.Paswoord = "";
            lesgever.Straat = "";
            lesgever.Huisnummer = "";
            lesgever.Postcode = "";
            lesgever.Stad = "";
            lesgever.Land = "";
            lesgever.Beschrijving = "";
        }

        private bool NakijkenEmail(string mail, Lesgever lesgever)
            //nakijken of het e-mailadres hetzelfde is met de huidige lesgever die wordt aangepast,
            //anders kan dit e-mailadres al in gebruik zijn door iemand anders en mag deze instantie niet worden aangepast om e-mailadres unique is.
        {
            if (!string.IsNullOrEmpty(txtEmailAdres.Text))
            {
                Lesgever vergelijkLesgever = DatabaseOperations.OphalenLesgeverViaEmail(mail);
                if (vergelijkLesgever.Id == lesgever.Id)
                {
                    return true;
                }
            }            
            return false;
        }
    }
}
