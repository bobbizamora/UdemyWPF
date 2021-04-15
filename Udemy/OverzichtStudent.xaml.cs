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
    /// Interaction logic for OverzichtStudent.xaml
    /// </summary>
    public partial class OverzichtStudent : Window
    {
        public OverzichtStudent()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblNaamStudent.Content = $"{Inloggegevens.Voornaam} {Inloggegevens.Naam}";//Naam van student inladen in label links bovenaan
            List<Cursus> cursussen = DatabaseOperations.OphalenCursussenViaStudentId(Inloggegevens.Id); //Alle cursussen van een bepaalde student opvragen
            foreach (var item in cursussen)//Hier gaan we de naam van de cursussen opvragen waar alle Cat_Id gelijk zijn aan NULL 
                                           //als dit niet het geval is gaat hij de hoofdcategorie zoeken
            {
                if (item.Categorie.Cat_Id != null)
                {
                    Categorie categorie = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                    item.Categorie.Naam = categorie.Naam;
                }
            }
            datagridAangekochteCursussen.ItemsSource = cursussen;
        }
        private void btnContactgegevensAanpassen_Click(object sender, RoutedEventArgs e) //Scherm AanpassenContactgegevensStudent openen
        {
            AanpassenContactgegevensStudent aanpassenContactgegevensStudent = new AanpassenContactgegevensStudent();
            aanpassenContactgegevensStudent.Show();
            this.Close();
        }
        private void btnNieuweCursusKopen_Click(object sender, RoutedEventArgs e) //Scherm CursussenKopen openen
        {
            CursussenKopen cursussenKopen = new CursussenKopen();
            cursussenKopen.Show();
            this.Close();
        }
        private void btnZoekCursus_Click(object sender, RoutedEventArgs e) //Hier kun je zoeken door een naam van een cursus in te geven 
        //Als deze overeenkomt met een cursus in Student dan gaat hij deze weergeven
        {
            List<Cursus> cursussenViaZoekfunctie = DatabaseOperations.OphalenCursussenViaCursusnaamEnStudentId(txtCursus.Text, Inloggegevens.Id);
            foreach (var item in cursussenViaZoekfunctie)
            {
                if (item.Categorie.Cat_Id != null)
                {
                    Categorie categorie = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                    item.Categorie.Naam = categorie.Naam;
                }
            }
            datagridAangekochteCursussen.ItemsSource = cursussenViaZoekfunctie;
        }
    }
}
