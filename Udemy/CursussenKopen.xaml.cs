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
    /// Interaction logic for CursussenKopen.xaml
    /// </summary>
    public partial class CursussenKopen : Window
    {
        public CursussenKopen()
        {
            InitializeComponent();
        }
        Student student = new Student(); 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblNaamStudent.Content = $"{Inloggegevens.Voornaam} {Inloggegevens.Naam}"; //Naam van student inladen in label links bovenaan
            List<Cursus> cursussen = DatabaseOperations.OphalenCursussen(); //Hier gaat hij alle cursussen die in de database staan opvragen.
            foreach (var item in cursussen) //Alle cursussen opvragen en hier de hoofdcategorie van weergeven in het veld categorie
            {
                if (item.Categorie.Cat_Id != null)
                {   
                    Categorie categorie = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                    item.Categorie.Naam = categorie.Naam;
                }
            }
            datagridAlleCursussen.ItemsSource = cursussen;
            cmbCategorie.ItemsSource = DatabaseOperations.OphalenHoofdcategorieen(); //alle hoofdcategorien opvragen en invullen in de combobox
        }
        private void btnTerugNaarOverzicht_Click(object sender, RoutedEventArgs e) //Scherm OverzichtStudent 
        {
            OverzichtStudent overzichtStudent = new OverzichtStudent();
            overzichtStudent.Show();
            this.Close();
        }
        private void btnZoeken_Click(object sender, RoutedEventArgs e) //Hij gaat hier alle cursussen zoeken die dezelfde naam of deel van een naam hebben als de ingevoerde tekst
        {
            List<Cursus> cursussenViaZoekfunctie = DatabaseOperations.OphalenCursussenViaCursusnaam(txtCursus.Text);
            foreach (var item in cursussenViaZoekfunctie)
            {
                if (item.Categorie.Cat_Id != null)
                {
                    Categorie categorie = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                    item.Categorie.Naam = categorie.Naam;
                }
            }
            datagridAlleCursussen.ItemsSource = cursussenViaZoekfunctie;
        }
        private void btnZoekCategorie_Click(object sender, RoutedEventArgs e) //Hier gaat hij de cursussen zoeken die overeenkomen met de ingevoerde categorie
        {
            if (cmbCategorie.SelectedItem is Categorie categorie)
            {
                if (cmbOnderwerp.SelectedItem is Categorie onderwerp)
                {
                    List<Cursus> cursussenOnderwerpen = DatabaseOperations.OphalenCursussenViaCategorieId(onderwerp.Id);
                    foreach (var item in cursussenOnderwerpen)
                    {
                        if (item.Categorie.Cat_Id != null)
                        {
                            Categorie categorieZoekenViaOnderwerp = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                            item.Categorie.Naam = categorieZoekenViaOnderwerp.Naam;
                        }
                    }
                    datagridAlleCursussen.ItemsSource = cursussenOnderwerpen;
                }
                else
                {
                    List<Cursus> cursussenCategorie = DatabaseOperations.OphalenCursussenViaCatID(categorie.Id);
                    foreach (var item in cursussenCategorie)
                    {
                        if (item.Categorie.Cat_Id != null)
                        {
                            Categorie categorieZoekenViaCategorieID = DatabaseOperations.OphalenCategorieViaId(item.Categorie.Cat_Id.Value);
                            item.Categorie.Naam = categorieZoekenViaCategorieID.Naam;
                        }
                    }
                    datagridAlleCursussen.ItemsSource = cursussenCategorie;
                }
            }
            else
            {
                MessageBox.Show("Gelieve eerst een Categorie te selecteren");
            }
        }
        private void cmbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e) //Hier gaat hij de 2de combobox invullen aan de wat je in de 1ste hebt ingevuld
        {
            if (cmbCategorie.SelectedItem is Categorie categorie)
            {
                cmbOnderwerp.ItemsSource = DatabaseOperations.OphalenOnderwerpenViaCategorieId(categorie.Id);
            }
        }

        private void btnCursusKopen_Click(object sender, RoutedEventArgs e) //Hier gaat je effectief de cursus kunnen toevoegen een aan student.
        {
            if (datagridAlleCursussen.SelectedItem == null)
            {
                MessageBox.Show("Gelieve eerst een cursus te selecteren.");
            }
            else
            {
                Cursus_Student cursusKopen = new Cursus_Student();
                cursusKopen.Student_Id = Inloggegevens.Id;
                cursusKopen.Aankoopdatum = DateTime.Now;
                if (datagridAlleCursussen.SelectedItem is Cursus cursus)
                {
                    cursusKopen.Cursus_Id = cursus.Id;
                }
                if (DatabaseOperations.CursusKopen(cursusKopen) > 0)
                {
                    MessageBox.Show("Uw Cursus is succesvol aangekocht.");
                }
                else
                {
                    MessageBox.Show("Uw cursus is NIET aangekocht.");
                }
            }

        }       
    }
}
