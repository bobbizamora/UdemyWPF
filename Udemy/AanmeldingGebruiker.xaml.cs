using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for AanmeldingGebruiker.xaml
    /// </summary>
    public partial class AanmeldingGebruiker : Window
    {
        public AanmeldingGebruiker()
        {
            InitializeComponent();
        }

        Lesgever lesgever = new Lesgever();
        Student student = new Student();

        private void RbStudent_Checked(object sender, RoutedEventArgs e)
        {// Wanneer "student" geselecteerd is, is "textbox beschrijving" niet zichtbaar
            lblBeschrijving.Visibility = Visibility.Collapsed;
            txtBeschrijving.Visibility = Visibility.Collapsed;
        }

        private void RbLesgever_Checked(object sender, RoutedEventArgs e)
        {// Wanneer "Lesgever" geselecteerd is, is "textbox beschrijving" WEL zichtbaar
            lblBeschrijving.Visibility = Visibility.Visible;
            txtBeschrijving.Visibility = Visibility.Visible;
        }


        private void BtnInloggen_Click(object sender, RoutedEventArgs e)
        {
            string foutmeldingen = Valideer("txtEmailadres");
            foutmeldingen += Valideer("paswoordbox2"); 
         
            if (string.IsNullOrWhiteSpace(foutmeldingen))
            {
                if (rbLesgever.IsChecked == true)// wanneer lesgever geselecteerd is
                {

                    lesgever.Naam = txtAchternaam.Text;
                    lesgever.Voornaam = txtVoornaam.Text;
                    lesgever.Email = txtEmailadres.Text;          
                    lesgever.Paswoord = paswoordbox1.Password;
                    lesgever.Straat = txtStraat.Text;
                    lesgever.Huisnummer = txtHuisnummer.Text;
                    lesgever.Stad = txtGemeente.Text;
                    lesgever.Postcode = txtPostcode.Text;
                    lesgever.Land = txtLand.Text;
                    lesgever.Beschrijving = txtBeschrijving.Text;
                    lesgever.Begindatum = DateTime.Now;

                    if (lesgever.IsGeldig())// als er geen foutmeldingen zijn, wordt er een lesgever toegevoegd
                    {
                        int ok = DatabaseOperations.ToevoegenLesgever(lesgever);
                        if (ok <= 0)
                        {
                            MessageBox.Show("Toevoegen van lesgever is niet gelukt!");
                        }
                        else
                        {//de gegevens id, naam en voornaam worden meegenomen naar het aangemaakte overzichtscherm van deze Lesgever
                            Inloggegevens.Id = lesgever.Id;
                            Inloggegevens.Naam = lesgever.Naam;
                            Inloggegevens.Voornaam = lesgever.Voornaam;
                            OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
                            overzichtLesgever.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show(lesgever.Error);
                    }

                }
                else if (rbStudent.IsChecked == true) //wanneer student geselecteerd is
                {
                    student.Voornaam = txtVoornaam.Text;
                    student.Naam = txtAchternaam.Text;
                    student.Email = txtEmailadres.Text;
                    student.Paswoord = paswoordbox1.Password;
                    student.Straat = txtStraat.Text;
                    student.Huisnummer = txtHuisnummer.Text;
                    student.Stad = txtGemeente.Text;
                    student.Postcode = txtPostcode.Text;
                    student.Land = txtLand.Text;
                    student.BeginDatum = DateTime.Now;

                    if (student.IsGeldig())
                    {
                        int ok = DatabaseOperations.ToevoegenStudent(student);
                        if (ok <= 0)
                        {
                            MessageBox.Show("Toevoegen van lesgever is niet gelukt!");
                        }
                        else
                        {//de gegevens id, naam en voornaam worden meegenomen naar het aangemaakte overzichtscherm van deze student
                            Inloggegevens.Id = student.Id;
                            Inloggegevens.Naam = student.Naam;
                            Inloggegevens.Voornaam = student.Voornaam;
                            OverzichtStudent overzichtStudent = new OverzichtStudent();
                            overzichtStudent.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show(student.Error);
                    }

                }
                else
                {
                    MessageBox.Show("De functie/rol is niet geselecteerd!");
                }


            }
            else
            {
                MessageBox.Show(foutmeldingen);
            }


        }

        private string Valideer (string kolomnaam)
        {
            
            if (kolomnaam == "paswoordbox2" && paswoordbox2.Password != paswoordbox1.Password )
            {
                return "Het paswoord en het controle paswoord zijn niet identiek" + Environment.NewLine;
            }
            if (kolomnaam == "txtEmailadres")
            {
                string msg = "Het emailadres dat u heeft gekozen is al toegewezen aan iemand anders. Gelieve een nieuw e-mail adres te kiezen" + Environment.NewLine;
                if (rbLesgever.IsChecked == true && NakijkenEmailLesgever(txtEmailadres.Text))
                {
                    return msg;
                }
                if (rbStudent.IsChecked == true && NakijkenEmailStudent(txtEmailadres.Text))
                {
                    return msg;
                }
                return "";                
            }
            return "";
        }

        private bool NakijkenEmailLesgever(string mail)
        {//checken of Lesgver reeds bestaat. via opgegeven mail wordt gegevens uit db opgehaald
            Lesgever controleLesgever = DatabaseOperations.OphalenLesgeverViaEmail(mail);
            if (controleLesgever == null)
            {
                return false;
            }
            return true;
        }

        private bool NakijkenEmailStudent(string mail)
        {// checken of student reeds bestaat. via opgegeven mail wordt gegevens uit db opgehaald
            Student controleStudent = DatabaseOperations.OphalenStudentViaEmail(mail);
            if (controleStudent == null)
            {
                return false;
            }
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {// bij het laden van de pagina staat "Student" geselecteerd.
            rbStudent.IsChecked = true;         
        }
    }
}
