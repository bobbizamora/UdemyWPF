using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AanpassenContactgegevensStudent.xaml
    /// </summary>
    public partial class AanpassenContactgegevensStudent : Window
    {
        public AanpassenContactgegevensStudent()
        {
            InitializeComponent();
        }

        Student student = new Student();
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblGebruiker.Content = $"{Inloggegevens.Voornaam} {Inloggegevens.Naam}";// Ophalen lesgever en inladen in formulier
            student = DatabaseOperations.OphalenStudentViaId(Inloggegevens.Id);
            if (student == null)
            {
                MessageBox.Show("Kan uw gegevens niet vinden.");
            }
            else
            {
                //opvullen formulier met gegevens van opgehaalde student
                txtVoornaam.Text = student.Voornaam;
                txtNaam.Text = student.Naam;
                txtEmailAdres.Text = student.Email;
                pwbPaswoord.Password = student.Paswoord;
                pwbControlePaswoord.Password = student.Paswoord;
                txtStraat.Text = student.Straat;
                txtHuisnummer.Text = student.Huisnummer;
                txtPostcode.Text = student.Postcode;
                txtGemeente.Text = student.Stad;
                txtLand.Text = student.Land;                
            }

        }
        private void btnTerugNaarOverzicht_Click(object sender, RoutedEventArgs e)
        {
            OverzichtStudent overzichtStudent = new OverzichtStudent();
            overzichtStudent.Show();
            this.Close();
        }
        private void btnAanpassenGegevens_Click(object sender, RoutedEventArgs e)
        {
            //opvullen gegevens student door middel van de formuliervelden
            student.Voornaam = txtVoornaam.Text;
            student.Naam = txtNaam.Text;
            student.Email = txtEmailAdres.Text.ToLower();
            student.Paswoord = pwbPaswoord.Password;
            student.Straat = txtStraat.Text;
            student.Huisnummer = txtHuisnummer.Text;
            student.Postcode = txtPostcode.Text;
            student.Stad = txtGemeente.Text;
            student.Land = txtLand.Text;
            //validatie uitvoeren
            string foutmelding = Valideer("pwbControlePaswoord");
            foutmelding += Valideer("Email");

            if (student.IsGeldig() && string.IsNullOrWhiteSpace(foutmelding))//valideren student via partial klasse methode is geldig + kijken of de foutmeldingen leeg zijn
            {
                int ok = DatabaseOperations.AanpassenGegevensStudent(student);
                if (ok > 0)// Als de gegevens van de student gewijzigt zijn wordt de gebruiker terug geleid naar hun overzicht
                {
                    MessageBox.Show("Uw gegevens zijn aangepast.");
                    Wissen();
                    OverzichtStudent overzichtStudent = new OverzichtStudent();
                    overzichtStudent.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Uw gegevens zijn niet veranderd!");
                }
            }
            else
            {               
                MessageBox.Show(student.Error + Environment.NewLine + foutmelding);
                if (foutmelding.Contains("Uw controle paswoord komt niet overeen met uw paswoord") || student.Error.Contains("Gelieve een paswoord in te vullen met volgende eigenschappen"))
                //Indien er een fout staat in foutmeldingen en student.Error omwille van het paswoord worden de velden die met het paswoord te maken hebben terug leeg gehaald.               
                {
                    pwbControlePaswoord.Password = "";
                    pwbPaswoord.Password = "";
                }
            }
        }

        private string Valideer(string veldnaam)//validatie
        {

            if (veldnaam == "pwbControlePaswoord" && pwbControlePaswoord.Password != pwbPaswoord.Password)
            {
                return "Uw controle paswoord komt niet overeen met uw paswoord." + Environment.NewLine;
            }
            if(veldnaam == "Email" && !(NakijkenEmail(student.Email, student)))
            {
                return "Het emailadres dat u heeft gekozen is al toegewezen aan iemand anders. Gelieve een nieuw e-mail adres te kiezen" + Environment.NewLine;
            }
            return "";
        }
        private void Wissen()//gegevens wissen
        {
            student.Voornaam = "";
            student.Naam = "";
            student.Email = "";
            student.Paswoord = "";
            student.Straat = "";
            student.Huisnummer = "";
            student.Postcode = "";
            student.Stad = "";
            student.Land = "";
        }

        private bool NakijkenEmail(string mail, Student student)
        //nakijken of het e-mailadres hetzelfde is met de huidige student die wordt aangepast,
        //anders kan dit e-mailadres al in gebruik zijn door iemand anders en mag deze instantie niet worden aangepast om e-mailadres unique is.
        {
            if (!string.IsNullOrEmpty(txtEmailAdres.Text))
            {
                Student vergelijkStudent = DatabaseOperations.OphalenStudentViaEmail(mail);
                if (vergelijkStudent.Id == student.Id)
                {
                    return true;
                }
            }           
            return false;
        }
    }
}
