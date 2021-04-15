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
using System.Globalization;
using Udemy_DAL;


namespace Udemy
{
    /// <summary>
    /// Interaction logic for CursusAanpassen.xaml
    /// </summary>
    public partial class CursusAanpassen : Window
    {
        public CursusAanpassen()
        {
            InitializeComponent();
        }

        Cursus cursus = new Cursus();
        List<Cursus_Bijzonderheid> cursusBijzonderheden;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] result;
            cursus = DatabaseOperations.OphalenCursusViaId(Cursusgegevens.Id);
            cmbTaal.ItemsSource = DatabaseOperations.OphalenTalen();
            cmbNiveau.ItemsSource = DatabaseOperations.OphalenNiveaus();
            cmbCategorie.ItemsSource = DatabaseOperations.OphalenHoofdcategorieen();
            cmbOnderwerpen.ItemsSource = DatabaseOperations.OphalenOnderwerpenViaCatId(cursus.Categorie.Cat_Id.Value);
            cursusBijzonderheden = DatabaseOperations.OphalenCursusBijzonderhedenViaCursusId(cursus.Id);//
            foreach (var item in cursusBijzonderheden)
            {
                if (item.Bijzonderheid.Naam.Equals("Examens"))
                {
                    cbExamens.IsChecked = true;
                }
                if (item.Bijzonderheid.Naam.Equals("Ondertitels"))
                {
                    cbOndertitels.IsChecked = true;
                }
                if (item.Bijzonderheid.Naam.Equals("Codeeroefeningen"))
                {
                    cbCodeerOefeningen.IsChecked = true;
                }
                if (item.Bijzonderheid.Naam.Equals("Oefenexamens"))
                {
                    cbOefenExamens.IsChecked = true;
                }
            }
            string prijs = cursus.Prijs.ToString("00.00",CultureInfo.CreateSpecificCulture("fr-BE"));// verplichten om de Belgische notatie te gebruiken
            result = prijs.Split(new char[] {','});//de opgeven prijs wordt opgesplitst in een deel voor de komma en een deel na de komma.
            txtPrijs.Text = result[0];// het deel voor de komma wordt hier opgevuld
            txtPrijsDecimaal.Text = result[1];// het deel na de komma wordt hier opgevuld
            txtNaamCursus.Text = cursus.Naam;
            cmbTaal.SelectedItem = cursus.Taal;
            cmbNiveau.SelectedItem = cursus.Niveau;
            cmbCategorie.SelectedItem = DatabaseOperations.OphalenCategorieViaId(cursus.Categorie.Cat_Id.Value);
            cmbOnderwerpen.SelectedItem = cursus.Categorie;
            txtbBeschrijving.Text = cursus.Beschrijving;
        }

        private void BtnTerugnaarOverzicht_Click(object sender, RoutedEventArgs e)
        {
            OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
            overzichtLesgever.Show();
            this.Close();
        }

        private void BtnWijzigingenDoorvoeren_Click(object sender, RoutedEventArgs e)
        {
            string foutmeldingen = Valideer("cmbTaal");
            foutmeldingen += Valideer("cmbNiveau");
            foutmeldingen += Valideer("cmbCategorie");
            foutmeldingen += Valideer("cmbOnderwerpen");
            foutmeldingen += Valideer("txtPrijs");
            if (string.IsNullOrWhiteSpace(foutmeldingen))
            {
                Taal taal = cmbTaal.SelectedItem as Taal;
                Niveau niveau = cmbNiveau.SelectedItem as Niveau;
                Categorie onderwerpen = cmbOnderwerpen.SelectedItem as Categorie;
                cursus.Naam = txtNaamCursus.Text;
                cursus.Categorie = onderwerpen;
                cursus.Categorie_Id = onderwerpen.Id;
                cursus.Niveau = niveau;
                cursus.Niveau_Id = niveau.Id;
                string seporator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                decimal prijs = System.Convert.ToDecimal(txtPrijs.Text + seporator + txtPrijsDecimaal.Text);
                cursus.Prijs = prijs;
                cursus.Taal = taal;
                cursus.Taal_Id = taal.Id;
                cursus.Beschrijving = txtbBeschrijving.Text;
                if (cursus.IsGeldig())
                {
                    cursusBijzonderheden = DatabaseOperations.OphalenCursusBijzonderhedenViaCursusId(cursus.Id);
                    if (DatabaseOperations.UpdateCursus(cursus)>0)
                    {
                        string msg = "Uw cursus is succesvol aangepast" + Environment.NewLine;
                        if ((cbOndertitels.IsChecked ?? false) && (!cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Ondertitels"))))
                        {
                            CursusBijzonderheidAanpassen("Ondertitels", ref msg);
                        }
                        if ((cbCodeerOefeningen.IsChecked ?? false) && (!cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Codeeroefeningen"))))
                        {
                            CursusBijzonderheidAanpassen("Codeeroefeningen", ref msg);
                        }
                        if ((cbExamens.IsChecked ?? false) && (!cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Examens")) ))
                        {
                            CursusBijzonderheidAanpassen("Examens", ref msg);
                        }
                        if ((cbOefenExamens.IsChecked ?? false) && (!cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Oefenexamens"))))
                        {
                            CursusBijzonderheidAanpassen("Oefenexamens", ref msg);
                        }

                        if (!(cbOndertitels.IsChecked ?? false) && (cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Ondertitels"))))
                        {
                            CursusBijzonderheidVerwijderen("Ondertitels", ref msg);
                        }
                        if (!(cbCodeerOefeningen.IsChecked ?? false) && (cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Codeeroefeningen")) ))
                        {
                            CursusBijzonderheidVerwijderen("Codeeroefeningen", ref msg);
                        }
                        if (!(cbExamens.IsChecked ?? false) && (cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Examens")) ))
                        {
                            CursusBijzonderheidVerwijderen("Examens", ref msg);
                        }
                        if (!(cbOefenExamens.IsChecked ?? false) && (cursusBijzonderheden.Any(x => x.Bijzonderheid.Naam.Equals("Oefenexamens")) ))
                        {
                            CursusBijzonderheidVerwijderen("Oefenexamens", ref msg);
                        }
                        if (msg.Contains("niet"))
                        {
                            MessageBox.Show(msg);
                        }
                        else
                        {
                            MessageBox.Show(msg);
                            OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
                            overzichtLesgever.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Uw cursus is niet aangepast. Als het probleem zich blijft voordoen gelieve Udemy te verwittigen");
                    }
                }
                else
                {
                    MessageBox.Show(cursus.Error);
                }
            }
            else
            {
                MessageBox.Show(foutmeldingen);
            }
        }

        private string Valideer(string kolomnaam)
        {
            if (kolomnaam == "cmbTaal" && cmbTaal.SelectedItem == null)
            {
                return "Gelieve een taal te selecteren" + Environment.NewLine;
            }
            if (kolomnaam == "cmbNiveau" && cmbNiveau.SelectedItem == null)
            {
                return "Gelieve een niveau te selecteren" + Environment.NewLine;
            }
            if (kolomnaam == "cmbCategorie" && cmbCategorie.SelectedItem == null)
            {
                return "Gelieve een categorie te selecteren" + Environment.NewLine;
            }
            if (kolomnaam == "cmbOnderwerpen" && cmbOnderwerpen.SelectedItem == null)
            {
                return "Gelieve een onderwerp te selecteren" + Environment.NewLine;
            }
            if (kolomnaam == "txtPrijs")
            {
                string msg = "";
                if (string.IsNullOrWhiteSpace(txtPrijs.Text) || !(int.TryParse(txtPrijs.Text, out int controlePrijs) && controlePrijs >= 0))
                {
                    msg += "Gelieve een juiste positieve prijs in de geven in het volgende format: 00000.00" + Environment.NewLine;
                }
                if (!(string.IsNullOrWhiteSpace(txtPrijsDecimaal.Text)) && !(int.TryParse(txtPrijsDecimaal.Text, out int controlePrijsDecimaal) && controlePrijsDecimaal >= 0))
                {
                    msg += "Gelieve een juiste positief getal na de komma te geven in prijs." + Environment.NewLine;
                }
                return msg;
            }
            return "";
        }

        private void CursusBijzonderheidAanpassen(string checkboxNaam, ref string msg)
        {   //Cursus bijzonderheden  worden opgehaald en een lokale variable gestoken
            Cursus_Bijzonderheid cursusBijzonderheid = new Cursus_Bijzonderheid();
            cursusBijzonderheid.Cursus_Id = cursus.Id;
            Bijzonderheid bijzonderheid = DatabaseOperations.OphalenBijzonderheidViaNaam(checkboxNaam);
            cursusBijzonderheid.Bijzonderheid_Id = bijzonderheid.Id;
           if (DatabaseOperations.ToevoegenCursusBijzonderheid(cursusBijzonderheid) <= 0)//het aantal toe te voegen cursussen moet groter zijn dan 0
            {
                msg += $"Uw bijzonderheid {checkboxNaam} is niet aangepast in de cursus." + Environment.NewLine;
            }            
        }


        private void CursusBijzonderheidVerwijderen(string checkboxNaam, ref string msg)
        {//Bijzonderheden van een cursus en de bijzonderheden per cursus worden opgehaald
            Bijzonderheid bijzonderheid = DatabaseOperations.OphalenBijzonderheidViaNaam(checkboxNaam);
            Cursus_Bijzonderheid cursusBijzonderheid = DatabaseOperations.OphalenCursusBijzonderheidViaCursusIdEnBijzonderheidId(cursus.Id, bijzonderheid.Id);          
            if (DatabaseOperations.VerwijderenCursusBijzonderheid(cursusBijzonderheid) <= 0)//het aantal te verwijderen cursussen moet groter zijn dan 0 
            {
                msg += $"Uw bijzonderheid {checkboxNaam} is niet verwijderd." + Environment.NewLine;
            }            
        }
    }
}
