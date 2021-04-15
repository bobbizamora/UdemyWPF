using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.NetworkInformation;

namespace Udemy_DAL
{
    public static class DatabaseOperations
    {
        public static List<Cursus> OphalenCursussen()
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                var query = udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal);
                return query.ToList();
            }
        }
        public static List<Cursus> OphalenCursussenViaStudentId(int studentId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                var query = udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal)
                    .Include(x => x.Cursussen_Student.Select(y => y.Student))
                    .Where(x => x.Cursussen_Student.Any(y => y.Student_Id == studentId));
                return query.ToList();
            }
        }
        public static List<Cursus> OphalenCursussenViaLesgeverId(int lesgeverId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                var query = udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal)
                    .Where(x => x.Lesgever_Id == lesgeverId);
                return query.ToList();
            }
        }
        public static List<Cursus> OphalenCursussenViaCursusnaam(string CursusNaam)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal)
                    .Where(x => x.Naam.Contains(CursusNaam))
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }
        public static List<Cursus> OphalenCursussenViaCursusnaamEnStudentId(string cursusNaam, int studentId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal)
                    .Include(x => x.Cursussen_Student.Select(y => y.Student))
                    .Where(x => x.Cursussen_Student.Any(y => y.Student_Id == studentId))
                    .Where(x => x.Naam.Contains(cursusNaam))
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }
        public static List<Cursus> OphalenCursussenViaCursusnaamEnLesgeverId(string cursusNaam, int lesgeverId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal)
                    .Where(x => x.Naam.Contains(cursusNaam))
                    .Where(x => x.Lesgever_Id == lesgeverId)
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }
        public static List<Categorie> OphalenHoofdcategorieen()

        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Categorie
                    .Where(x => x.Cat_Id == null)
                    .OrderBy(x => x.Naam)
                    .ToList();
            }

        }
        public static List<Cursus> OphalenCursussenViaCategorieId(int categorieId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal)
                    .Where(x => x.Categorie_Id == categorieId)
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }
        public static List<Cursus> OphalenCursussenViaCatID(int catID)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus
                    .Include(x => x.Categorie)
                    .Include(x => x.Lesgever)
                    .Include(x => x.Niveau)
                    .Include(x => x.Taal)
                    .Where(x => x.Categorie.Cat_Id == catID)
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }
        public static List<Categorie> OphalenOnderwerpenViaCategorieId(int id)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Categorie
                    .Where(x => x.Cat_Id == id)
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }
        public static int AanpassenGegevensLesgever(Lesgever lesgever)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {
                    udemyEntities.Entry(lesgever).State = EntityState.Modified;
                    return udemyEntities.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                FileOperations.Foutloggen(ex);
                return 0;
                throw;
            }
        }
        public static int AanpassenGegevensStudent(Student student)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {
                    udemyEntities.Entry(student).State = EntityState.Modified;
                    return udemyEntities.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                FileOperations.Foutloggen(ex);
                return 0;
                throw;
            }
        }

        public static Lesgever OphalenLesgeverViaId(int lesgeverId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Lesgever
                    .Where(x => x.Id == lesgeverId)
                    .SingleOrDefault();
            }
        }

        public static Student OphalenStudentViaId(int studentId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Student
                    .Where(x => x.Id == studentId)
                    .SingleOrDefault();
            }
        }

        public static Student OphalenStudentViaEmail(string email)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Student
                    .Where(x => x.Email.ToLower() == email.ToLower())
                    .SingleOrDefault();
            }
        }

        public static Lesgever OphalenLesgeverViaEmail(string email)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Lesgever
                    .Where(x => x.Email.ToLower() == email.ToLower())
                    .SingleOrDefault();
            }
        }

        public static Categorie OphalenCategorieViaId(int categorieId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Categorie
                .Where(x => x.Id == categorieId)
                .SingleOrDefault();
            }
        }
        public static int ToevoegenCursus(Cursus cursus)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {
                    udemyEntities.Cursus.Add(cursus);
                    return udemyEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.Foutloggen(ex);
                return 0;
            }
        }
        public static int CursusKopen(Cursus_Student cursusKopen)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {
                    udemyEntities.Cursus_Student.Add(cursusKopen);
                    return udemyEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.Foutloggen(ex);
                return 0;
            }
        }

        public static List<Taal> OphalenTalen()
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Taal
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }

        public static List<Niveau> OphalenNiveaus()
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Niveau
                    .OrderBy(x => x.Naam)
                    .ToList();
            }
        }

        public static List<Categorie> OphalenOnderwerpenViaCatId(int catId) 
        //We halen de subcategorieën(onderwerpen) op via het catId. Deze klasse heeft een relatie met zichself en hebben wij een andere benaming voor gekozen
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Categorie
                    .Where(x => x.Cat_Id == catId)
                    .OrderBy(x => x.Naam)
                    .ToList();
            }

        }    
        
        public static int ToevoegenLesgever(Lesgever lesgever)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                try
                {
                    udemyEntities.Lesgever.Add(lesgever);
                    return udemyEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    FileOperations.Foutloggen(ex);
                    return 0;
                }
            }
        }

        public static int ToevoegenStudent(Student student)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                try
                {
                    udemyEntities.Student.Add(student);
                    return udemyEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    FileOperations.Foutloggen(ex);
                    return 0;
                }
            }

        }

        public static Cursus OphalenCursusViaCursusnaam(string cursusNaam)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus
                    .Where(x => x.Naam == cursusNaam)
                    .SingleOrDefault();
            }
        }

        public static Bijzonderheid OphalenBijzonderheidViaNaam(string naam)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Bijzonderheid
                    .Where(x => x.Naam == naam)
                    .SingleOrDefault();
            }
        }

        public static int ToevoegenCursusBijzonderheid(Cursus_Bijzonderheid cursusBijzonderheid)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {
                    udemyEntities.Cursus_Bijzonderheid.Add(cursusBijzonderheid);
                    return udemyEntities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                FileOperations.Foutloggen(ex);
                return 0;
            }
        }

        public static int VerwijderenCursus(Cursus cursus)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {

                    udemyEntities.Entry(cursus).State = EntityState.Deleted;
                    return udemyEntities.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                FileOperations.Foutloggen(ex);
                return 0;
            }

        }

        public static Cursus OphalenCursusViaId(int cursusId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus
                    .Where(x => x.Id == cursusId)
                    .Include(x => x.Taal )
                    .Include(x => x.Niveau)
                    .Include(x => x.Categorie)
                    .SingleOrDefault();
            }
        }

        public static List<Cursus_Bijzonderheid> OphalenCursusBijzonderhedenViaCursusId(int cursusId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus_Bijzonderheid
                    .Where(x => x.Cursus_Id == cursusId)
                    .Include(x => x.Bijzonderheid)
                    .ToList();
            }
        }

        public static Cursus_Bijzonderheid OphalenCursusBijzonderheidViaCursusIdEnBijzonderheidId(int cursusId, int bijzonderheidId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Cursus_Bijzonderheid
                    .Where(x => x.Cursus_Id == cursusId && x.Bijzonderheid_Id == bijzonderheidId)
                    .Include(x => x.Bijzonderheid)
                    .SingleOrDefault();
            }
        }

        public static int UpdateCursus(Cursus cursus)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {
                    udemyEntities.Entry(cursus).State = EntityState.Modified;
                    return udemyEntities.SaveChanges();
                }
            }
            
            catch (Exception ex )
            {
                FileOperations.Foutloggen(ex);
                return 0;
            }
           
        }

        public static int VerwijderenCursusBijzonderheid(Cursus_Bijzonderheid cursus_Bijzonderheid)
        {
            try
            {
                using (UdemyEntities udemyEntities = new UdemyEntities())
                {
                    udemyEntities.Entry(cursus_Bijzonderheid).State = EntityState.Deleted;
                    return udemyEntities.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                FileOperations.Foutloggen(ex);
                return 0;
            }
        }

        public static Taal OphalenTalenViaId(int taalId)
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Taal
                    .Where(x => x.Id == taalId)
                    .SingleOrDefault();
            }
        }

        public static List<Lesgever> OphalenLesgevers()
        {
            using (UdemyEntities udemyEntities = new UdemyEntities())
            {
                return udemyEntities.Lesgever
                    .ToList();
            }
        }

    }
}
