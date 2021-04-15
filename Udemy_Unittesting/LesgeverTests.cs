using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Udemy_DAL;
using Udemy_Models;
using System.Windows;
using System.Collections.Generic;

namespace Udemy_Unittesting
{
    [TestClass]
    public class LesgeverTests
    {
        [TestMethod]
        public void Naam_ValueIsNietOpgevuld_LesgeverIsNietGeldigGooitFoutmelding()
        {
            // Arrange          
            Lesgever lesgever = new Lesgever();         
            // Act          
            lesgever.Naam = "";
            lesgever.Voornaam = "Lieven";
            lesgever.Email = "lieven.Janssens@gmail.com";
            lesgever.Paswoord = "Ikbendebeste007";
            lesgever.Straat = "lesgeverstraat";
            lesgever.Huisnummer = "13A";
            lesgever.Postcode = "recidentof";
            lesgever.Stad = "Awesomeville";
            lesgever.Land = "MargarittaVille";
            lesgever.Beschrijving = "Insert great things here";
            lesgever.Begindatum = DateTime.Now;          
            //Assert          
            Assert.IsFalse(lesgever.IsGeldig());
        }
        [TestMethod]
        public void Naam_ValueIsNietOpgevuld_LesgeverFoutmeldingNaamIsNietIngevuldGooien()
        {
            // Arrange
            Lesgever lesgever = new Lesgever();
            string foutmelding = "Gelieve een familienaam in te vullen." + Environment.NewLine;
            // Act
            lesgever.Naam = "";
            lesgever.Voornaam = "Jarno";
            lesgever.Email = "jarno.Peeters@gmail.com";
            lesgever.Paswoord = "TestTest69";
            lesgever.Straat = "Codeerstraat";
            lesgever.Huisnummer = "75";
            lesgever.Postcode = "2440";
            lesgever.Stad = "Yeloow";
            lesgever.Land = "Bluestone";
            lesgever.Beschrijving = "Insert great things here";
            lesgever.Begindatum = DateTime.Now;
            lesgever.IsGeldig();
            //Assert
            Assert.AreEqual(foutmelding, lesgever.Error);
        }

        [TestMethod]
        public void Email_ValueIsUniek_LesgeverEmailIsUniek ()
        {

            // Arrange
            List<Lesgever> lesgevers;
            List<String> emailLijst = new List<String>();            

            // Act
            lesgevers = DatabaseOperations.OphalenLesgevers();
            foreach (var item in lesgevers)
            {
                emailLijst.Add(item.Email);
            }

            // Assert
            CollectionAssert.AllItemsAreUnique(emailLijst);

        }
    }
}
