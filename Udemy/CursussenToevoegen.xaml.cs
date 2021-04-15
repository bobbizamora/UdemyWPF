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
using System.Globalization;

namespace Udemy
{
    /// <summary>
    /// Interaction logic for CursussenToevoegen.xaml
    /// </summary>
    public partial class CursussenToevoegen : Window
    {
        public CursussenToevoegen()
        {
            InitializeComponent();
        }

        Lesgever lesgever = new Lesgever();
        Cursus cursus = new Cursus();
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblGebruiker.Content = $"{Inloggegevens.Voornaam} {Inloggegevens.Naam}"; //ophalen gegevens van de gebruiker die ingelogd is
            cmbTaal.ItemsSource = DatabaseOperations.OphalenTalen(); // inladen van alle comboboxen met gegevens uit de databank
            cmbNiveau.ItemsSource = DatabaseOperations.OphalenNiveaus();
            cmbCategorie.ItemsSource = DatabaseOperations.OphalenHoofdcategorieen();
        }
        private void cmbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Categorie categorie = cmbCategorie.SelectedItem as Categorie; // pas wanneer een categorie is ingevuld kan men een onderwerp selecteren omdat dit een recrusieve relatie is in de databank en deze van elkaar afhangen
            if (categorie == null)
            {
                MessageBox.Show("Gelieve eerst een categorie te kiezen");
            }
            else
            {
                cmbOnderwerpen.ItemsSource = DatabaseOperations.OphalenOnderwerpenViaCatId(categorie.Id);
            }
        }

        private void btnTerugNaarOverzicht_Click(object sender, RoutedEventArgs e)
        {
            OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
            overzichtLesgever.Show();
            this.Close();
        }


        private void btnMaakCursusAan_Click(object sender, RoutedEventArgs e)
        {
            lesgever = DatabaseOperations.OphalenLesgeverViaId(Inloggegevens.Id); //gegevens ophalen van de lesgever via de inloggegevens
            List<Cursus> aantalCursussenLesgever = DatabaseOperations.OphalenCursussenViaLesgeverId(lesgever.Id); // voor een unieke code te genereren voor de cursus worden alle cursussen opgehaald om hiervan het aantal te weten zodat deze op volgorde kan aangemaakt worden
            string code = lesgever.Id + lesgever.Naam.Substring(0, 1).ToUpper() + lesgever.Voornaam.Substring(0, 1).ToUpper() + (aantalCursussenLesgever.Count + 1);//creëren unieke code
            string foutmeldingen = Valideer("cmbTaal");//validatie
            foutmeldingen += Valideer("cmbNiveau");            
            foutmeldingen += Valideer("cmbCategorie");
            foutmeldingen += Valideer("cmbOnderwerpen");
            foutmeldingen += Valideer("txtPrijs");//moet gevalideert worden hier en niet in de methode is geldig omdat deze geconverteerd moet worden naar een decimal
            if (string.IsNullOrWhiteSpace(foutmeldingen))//Indien er geen foutmeldingen kan een cursus aangemaakt worden
            {
                //opvullen gegevens
                Taal taal = cmbTaal.SelectedItem as Taal;
                Niveau niveau = cmbNiveau.SelectedItem as Niveau;
                Categorie onderwerpen = cmbOnderwerpen.SelectedItem as Categorie;
                cursus.Naam = txtNaamCursus.Text;
                cursus.Datum = DateTime.Now;
                cursus.Categorie_Id = onderwerpen.Id;
                cursus.Niveau_Id = niveau.Id;
                string seporator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                decimal prijs = System.Convert.ToDecimal(txtPrijs.Text + seporator + txtPrijsDecimaal.Text);
                //prijs is in 2 velden opgemaakt omdat ik dit niet in orde kreeg om een Amerikaanse notatie . of Belgische notatie , in een getal eruit te halen
                cursus.Prijs = prijs;
                cursus.Taal_Id = taal.Id;
                cursus.Code = code;
                cursus.Beschrijving = txtbBeschrijving.Text;
                cursus.Lesgever_Id = lesgever.Id;
                if (cursus.IsGeldig())//Via de partial class wordt gekeken of deze cursus geldig is
                {
                    if (DatabaseOperations.ToevoegenCursus(cursus) > 0 )
                    {
                        //Wanneer de cursus is aangemaakt worden zijn bijhorende bijzonderheden aangemaakt
                        string msg = "Uw cursus is aangemaakt" + Environment.NewLine;                        
                        if (cbOndertitels.IsChecked ?? false) 
                        {
                          CursusBijzonderheidToevoegen("Ondertitels", ref msg);
                        }
                        if (cbExamens.IsChecked ?? false)
                        {
                          CursusBijzonderheidToevoegen("Examens", ref msg);
                        }
                        if (cbCodeerOefeningen.IsChecked ?? false)
                        {
                          CursusBijzonderheidToevoegen("Codeeroefeningen", ref msg);
                        }
                        if (cbOefenExamens.IsChecked ?? false)
                        {
                           CursusBijzonderheidToevoegen("Oefenexamens", ref msg);
                        }
                        if (msg.Contains("niet"))
                        {
                            msg = "Er is een fout in de cursus bijzonderheden en deze worden terug verwijdert:";
                            //Men probeert de cursusbijzonderheden te verwijderen omdat er een fout in de databank zit
                            List<Cursus_Bijzonderheid> cursusBijzonderheden = DatabaseOperations.OphalenCursusBijzonderhedenViaCursusId(cursus.Id);
                            bool verwijderenGelukt = true;
                            foreach (var item in cursusBijzonderheden)
                            {
                                if(!CursusBijzonderheidVerwijderen(item.Bijzonderheid.Naam, ref msg))
                                {
                                    verwijderenGelukt = false;
                                }
                            }
                            if (verwijderenGelukt == false)
                            {
                                MessageBox.Show($"Uw cursus kon niet verwijdert worden. {msg}");
                            }
                            else
                            {
                                Cursus verwijderenCursus = DatabaseOperations.OphalenCursusViaId(cursus.Id);
                                if (DatabaseOperations.VerwijderenCursus(verwijderenCursus) > 0)
                                {
                                    MessageBox.Show("Uw cursus en zijn bijzonderheden zijn terug verwijdert wegens een fout bij het toevoegen van de cursus");
                                }
                                else
                                {                                   
                                    MessageBox.Show("De cursus is aangemaakt zonder zijn bijzonderheden door fouten in de bijzonderheden");
                                }
                            }
                        }
                        else
                        {
                            //Indien alles gelukt is wordt de gebruiker terug geleid naar zijn overzicht
                            MessageBox.Show(msg);
                            OverzichtLesgever overzichtLesgever = new OverzichtLesgever();
                            overzichtLesgever.Show();
                            this.Close();
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("Uw cursus is niet toegevoegd. Als het probleem zich blijft voordoen gelieve Udemy te verwittigen");
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
            if (kolomnaam == "cmbOnderwerpen" && cmbOnderwerpen.SelectedItem == null )
            {
                return "Gelieve een onderwerp te selecteren" + Environment.NewLine;
            }
            if (kolomnaam == "txtPrijs")
            {
                string msg = "";
                if (string.IsNullOrWhiteSpace(txtPrijs.Text) || !(int.TryParse(txtPrijs.Text, out int controlePrijs) && controlePrijs >= 0))//controle of het veld txtPrijs ingevuld is, kijken of dit een getal is en of dit getal groter of gelijk is aan nul
                {
                    msg += "Gelieve een juiste positieve prijs in de geven in het volgende format: 00000.00" + Environment.NewLine;
                }
                if (!(string.IsNullOrWhiteSpace(txtPrijsDecimaal.Text)) && !(int.TryParse(txtPrijsDecimaal.Text, out int controlePrijsDecimaal) && controlePrijsDecimaal >= 0))//controle of het veld txtPrijsDecimaal ingevuld is, kijken of dit een getal is en of dit getal groter of gelijk is aan nul
                {
                    msg += "Gelieve een juiste positief getal na de komma te geven in prijs." + Environment.NewLine;
                }                
                return msg;
            }
            return "";
        }
        private void CursusBijzonderheidToevoegen(string checkboxNaam, ref string msg)
        {            
            Cursus_Bijzonderheid cursusBijzonderheid = new Cursus_Bijzonderheid(); // aanmaken instantie van cursus bijzonderheid
            Cursus controleCursus = DatabaseOperations.OphalenCursusViaId(cursus.Id);// De juist aangemaakte cursus terug oproepen
            cursusBijzonderheid.Cursus_Id = controleCursus.Id; // invullen gegevens
            Bijzonderheid bijzonderheid = DatabaseOperations.OphalenBijzonderheidViaNaam(checkboxNaam);
            cursusBijzonderheid.Bijzonderheid_Id = bijzonderheid.Id;
            if (DatabaseOperations.ToevoegenCursusBijzonderheid(cursusBijzonderheid) <= 0)//Toevoegen van de cursusbijzonderheden en indien dit niet mogelijk is een fout werpen zodat hier later in het programma rekening mee kan worden gehouden
            {
                msg += $"Uw bijzonderheid {checkboxNaam} is niet meegenomen in de aanmaak van de cursus" + Environment.NewLine;               
            }            
        }

        private bool CursusBijzonderheidVerwijderen(string checkboxNaam, ref string msg)
        {
            bool gelukt = true;
            Bijzonderheid bijzonderheid = DatabaseOperations.OphalenBijzonderheidViaNaam(checkboxNaam);
            Cursus_Bijzonderheid cursusBijzonderheid = DatabaseOperations.OphalenCursusBijzonderheidViaCursusIdEnBijzonderheidId(cursus.Id, bijzonderheid.Id);
            if (DatabaseOperations.VerwijderenCursusBijzonderheid(cursusBijzonderheid) <= 0)//verwijderen van de cursus bijzonderheden en indien dit niet mogelijk dat er een fout wordt gegooit
            {
                msg += $"Uw bijzonderheid {checkboxNaam} is niet verwijderd." + Environment.NewLine;
                gelukt = false;
            }
            return gelukt;
        }
    }
}
