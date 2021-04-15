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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Udemy_DAL;

namespace Udemy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnInloggen_Click(object sender, RoutedEventArgs e)
        {

            string foutmeldingen = Valideer("txtEmail");
            foutmeldingen += Valideer("paswoordbox");
            foutmeldingen += Valideer("cmbRol");

            if (string.IsNullOrWhiteSpace(foutmeldingen))
            {
                //als in de combox "student" wordt geselecteerd
                if (cmbRol.SelectedIndex == 0)
                {
                    //ophalen studenten via opgegeven e-mail
                    Student student = DatabaseOperations.OphalenStudentViaEmail(txtEmail.Text);
                    if (student != null)//als de student reeds aanwezig is in de database
                    {
                        if (student.Paswoord == paswoordbox.Password)
                        {
                            Inloggegevens.Id = student.Id;
                            Inloggegevens.Naam = student.Naam;
                            Inloggegevens.Voornaam = student.Voornaam;

                            OverzichtStudent overzichtStudent = new OverzichtStudent();
                            overzichtStudent.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ik kan uw logingegevens niet vinden!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ik kan uw logingegevens niet vinden!");
                    }
                }
                else if (cmbRol.SelectedIndex == 1)//als in de combox "Lesgever" wordt geselecteerd
                {
                    Lesgever lesgever = DatabaseOperations.OphalenLesgeverViaEmail(txtEmail.Text);//ophalen Lesgevers via opgegeven e-mail
                    if (lesgever != null)//als de Lesgever reeds aanwezig is in de database
                    {
                        if (lesgever.Paswoord == paswoordbox.Password)
                        {
                            Inloggegevens.Id = lesgever.Id;
                            Inloggegevens.Naam = lesgever.Naam;
                            Inloggegevens.Voornaam = lesgever.Voornaam;

                            OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
                            overzichtLesgever.Show();
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("Ik kan uw logingegevens niet vinden !");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ik kan uw logingegevens niet vinden");
                    }
                }
                else
                {
                    MessageBox.Show("Gelieve een functie/rol te selecteren !");
                }
            }
 
        }

        private string Valideer(string veldnaam)
        {
            if (veldnaam == "txtEmail" && string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                return "Geef uw E-mailadres in!" + Environment.NewLine;
            }
            if (veldnaam == "paswoordbox" && string.IsNullOrWhiteSpace(paswoordbox.Password))
            {
                return "Geef een paswoord in!" + Environment.NewLine;
            }
            if (veldnaam == "cmbRol" && cmbRol.SelectedItem == null)
            {
                return "Selecteer een functie/rol!" + Environment.NewLine;
            }
            return "";

         }

        private void BtnNieuweAanmelding_Click(object sender, RoutedEventArgs e)
        {
            // een nieuwe gebruiker kan via deze knop naar het scherm "Aanmelding Gebruiker"
            AanmeldingGebruiker aanmeldingGebruiker = new AanmeldingGebruiker();
            aanmeldingGebruiker.Show();
            this.Close();

        }
    }
}
