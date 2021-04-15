using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Udemy_DAL;

namespace Udemy_Unittesting
{
    [TestClass]
    public class DataOperationsTests
    {
        [TestMethod]
        public void IdEnEmail_ValuesZijnUniek_IdEnEmailIsGelijkInBeideObjecten()
        {
            //Arrange
            Lesgever lesgever = new Lesgever();

            //Act
            lesgever.Id = 1;
            lesgever.Email = "ChristineMaisel@gmail.com";
            Lesgever ophalenLesgever = DatabaseOperations.OphalenLesgeverViaId(lesgever.Id);

            //Assert
            Assert.AreEqual(lesgever, ophalenLesgever);
        }

        [TestMethod]
        public void ToevoegenEnVerwijderen_CursusBijzonderheden_CursusBijzonderhedenToevoegenEnVerwijderenIsTrue()
        {
            //Arrange
            Cursus_Bijzonderheid cursusBijzonderheid = new Cursus_Bijzonderheid();
            int toevoegenGelukt = -1;
            int verwijderenGelukt = -1;
            int cursusId = 1;
            int bijzonderheidId = 1;
            cursusBijzonderheid.Bijzonderheid_Id = bijzonderheidId;
            cursusBijzonderheid.Cursus_Id = cursusId;
            //Act
            toevoegenGelukt = DatabaseOperations.ToevoegenCursusBijzonderheid(cursusBijzonderheid);

            if (toevoegenGelukt > 0)
            {
                verwijderenGelukt = DatabaseOperations.VerwijderenCursusBijzonderheid(cursusBijzonderheid);
            }
            //Assert
            Assert.IsTrue(toevoegenGelukt == 1);
            Assert.IsTrue(verwijderenGelukt == 1);
        }
      
        [TestMethod]
        public void OpvragenEnBijwerken_Cursus_OpvragenEnBijwerkenCursusnaam()
        {
            //Arrange
            int cursusId = 1;
            bool opvragenGelukt = false;
            int bijwerkenGelukt = -1;

            //Act
            Cursus cursus = DatabaseOperations.OphalenCursusViaId(cursusId);

            if (cursus != null)
            {
                opvragenGelukt = true;
                cursus.Naam = "Start and Run a Successful Web Design Business from Home";
                bijwerkenGelukt = DatabaseOperations.UpdateCursus(cursus);
            }


            //Assert
            Assert.IsTrue(opvragenGelukt);
            Assert.IsTrue(bijwerkenGelukt == 1);
        }
    }
}
