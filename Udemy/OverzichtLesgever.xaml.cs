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
    /// Interaction logic for OverzichtLesgever.xaml
    /// </summary>
    public partial class OverzichtLesgever : Window
    {
        public OverzichtLesgever()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblNaamLesgever.Content = $"{Inloggegevens.Voornaam} {Inloggegevens.Naam}"; //Naam van lesgever inladen in label links bovenaan
            List<Cursus> cursussen = DatabaseOperations.OphalenCursussenViaLesgeverId(Inloggegevens.Id);  //Alle cursussen van een bepaalde lesgever opvragen 
            foreach (var item in cursussen) //Hier gaan we de naam van de cursussen opvragen waar alle Cat_Id gelijk zijn aan NULL 
                                            //als dit niet het geval is gaat hij de hoofdcategorie zoeken
            {
                if (item.Categorie.Cat_Id != null)
                {
                    Categorie categorie = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                    item.Categorie.Naam = categorie.Naam;
                }
            }
            datagridAangemaakteCursussen.ItemsSource = cursussen;
        }
        private void btnContactgegevensAanpassen_Click(object sender, RoutedEventArgs e) //Scherm AanpassenContactgegevensLesgever openen
        {
            AanpassenContactgegevensLesgever aanpassenContactgegevensLesgever = new AanpassenContactgegevensLesgever();
            aanpassenContactgegevensLesgever.Show();
            this.Close();
        }
        private void btnCursusToevoegen_Click(object sender, RoutedEventArgs e) //Scherm CursussenToevoegen openen
        {
            CursussenToevoegen cursussenToevoegen = new CursussenToevoegen();
            cursussenToevoegen.Show();
            this.Close();
        }
        private void btnZoekCursus_Click(object sender, RoutedEventArgs e) //Hier kun je zoeken door een naam van een cursus in te geven 
        //Als deze overeenkomt met een cursus in Lesgever dan gaat hij deze weergeven
        {
            List<Cursus> cursussenViaZoekfunctie = DatabaseOperations.OphalenCursussenViaCursusnaamEnLesgeverId(txtCursus.Text, Inloggegevens.Id); 
            foreach (var item in cursussenViaZoekfunctie)
            {
                if (item.Categorie.Cat_Id != null)
                {
                    Categorie categorie = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                    item.Categorie.Naam = categorie.Naam;
                }
            }
            datagridAangemaakteCursussen.ItemsSource = cursussenViaZoekfunctie;
        }
        private void BtnCursusAanpassen_Click(object sender, RoutedEventArgs e)
        {   
                 if(!(datagridAangemaakteCursussen.SelectedItem is Cursus cursus))
                {
                    MessageBox.Show("U hebt geen cursus geselecteerd om aan te passen");
                }
                else
                {
                    Cursusgegevens.Id = cursus.Id;
                    CursusAanpassen cursusAanpassen = new CursusAanpassen();
                    cursusAanpassen.Show();
                    this.Close();
                }       
        }
    }
}
