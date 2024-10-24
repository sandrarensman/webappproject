using System.Globalization;
using SchoolApp.Models;

namespace SchoolApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var jan = new Student
            {
                FirstName = "Jan",
                LastName = "Steen",
                PhoneNumber = "071 1234567",
                EmailAddress = "jan@steen.nl",
                Motivation = "Ik heb jarenlang Tai Chi gedaan. Ik wil het graag weer oppakken."
            };
            var rembrandt = new Student
            {
                FirstName = "Rembrandt",
                Prefix = "van",
                LastName = "Rijn",
                PhoneNumber = "071 2345678",
                EmailAddress = "rembrandt@vanrijn.nl"
            };
            var vincent = new Student
            {
                FirstName = "Vincent",
                Prefix = "van",
                LastName = "Gogh",
                PhoneNumber = "076 1234567",
                EmailAddress = "vincent@vangogh.nl",
                Motivation = "Geen ervaring, wel interesse"
            };
            var karel = new Student
            {
                FirstName = "Karel",
                LastName = "Appel",
                PhoneNumber = "020 1234567",
                EmailAddress = "karel@appel.nl",
                Motivation = "Ik werd nieuwsgierig toen ik mensen in het park Tai Chi zag doen."
            };
            var hendrick = new Student
            {
                FirstName = "Hendrick",
                LastName = "Avercamp",
                PhoneNumber = "020 2345678",
                EmailAddress = "hendrick@avercamp.nl",
                Motivation = "Ik heb altijd veel gesport en wil graag in beweging blijven."
            };
            var jhieronimus = new Student
            {
                FirstName = "Jheronimus",
                LastName = "Bosch",
                PhoneNumber = "073 1234567",
                EmailAddress = "jheronimus@bosch.nl"
            };
            var piet = new Student
            {
                FirstName = "Piet",
                LastName = "Mondriaan",
                PhoneNumber = "033 1234567",
                EmailAddress = "piet@mondriaan.nl"
            };
            var pieter = new Student
            {
                FirstName = "Pieter",
                LastName = "Breughel",
                PhoneNumber = "0499 123456",
                EmailAddress = "pieter@breughel.nl"
            };
            var johannes = new Student
            {
                FirstName = "Johannes",
                LastName = "Vermeer",
                PhoneNumber = "015 1234567",
                EmailAddress = "johannes@vermeer.nl",
                Motivation = "Ik ben op zoek naar meer ontspanning."
            };
            var aelbert = new Student
            {
                FirstName = "Aelbert",
                LastName = "Cuyp",
                PhoneNumber = "078 1234567",
                EmailAddress = "aelbert@cuyp.nl"
            };
            var frans = new Student
            {
                FirstName = "Frans",
                LastName = "Hals",
                PhoneNumber = "023 1234567",
                EmailAddress = "frans@hals.nl"
            };
            var jozef = new Student
            {
                FirstName = "Jozef",
                LastName = "Israëls",
                PhoneNumber = "070 1234567",
                EmailAddress = "jozef@israels.nl"
            };
            var isaac = new Student
            {
                FirstName = "Isaac",
                LastName = "Israëls",
                PhoneNumber = "020 1234567",
                EmailAddress = "isaac@israels.nl",
                Motivation = "Ik hoorde van mijn vader hoe leuk Tai Chi is. Ik wil het ook graag eens proberen."
            };
            var ferdinand = new Student
            {
                FirstName = "Ferdinand",
                LastName = "Bol",
                PhoneNumber = "078 2345678",
                EmailAddress = "ferdinand@bol.nl"
            };
            var charley = new Student
            {
                FirstName = "Charley",
                LastName = "Toorop",
                PhoneNumber = "071 3456789",
                EmailAddress = "charley@toorop.nl"
            };
            var willem = new Student
            {
                FirstName = "Hendrik Willem",
                LastName = "Mesdag",
                PhoneNumber = "070 2345678",
                EmailAddress = "hendrikwillem@mesdag.nl",
                Motivation = "Een collega vertelde mij over Tai Chi."
            };
            var maurits = new Student
            {
                FirstName = "Maurits Cornelis",
                LastName = "Escher",
                PhoneNumber = "058 1234567",
                EmailAddress = "mc@escher.nl",
                Motivation = "Ik heb pijn in mijn rug van al die trappen."
            };

            var students = new Student[]
            {
                jan,
                rembrandt,
                vincent,
                karel,
                hendrick,
                jhieronimus,
                piet,
                pieter,
                johannes,
                aelbert,
                frans,
                jozef,
                isaac,
                ferdinand,
                charley,
                willem,
                maurits
            };

            context.Students.AddRange(students);

            var groups = new Group[]
            {
                new()
                {
                    Day = "Maandag",
                    StartTime = DateTime.Parse("18:00"),
                    EndTime = DateTime.Parse("19:15"),
                    Level = Level.BC,
                    Students = [pieter, ferdinand, charley, willem]
                },
                new()
                {
                    Day = "Maandag",
                    StartTime = DateTime.Parse("19:20"),
                    EndTime = DateTime.Parse("20:45"),
                    Level = Level.YY,
                    Students = [rembrandt, hendrick, jozef]
                },
                new()
                {
                    Day = "Maandag",
                    StartTime = DateTime.Parse("20:45"),
                    EndTime = DateTime.Parse("22:00"),
                    Level = Level.VL,
                    Students = [jhieronimus, aelbert, maurits]
                },
                new()
                {
                    Day = "Woensdag",
                    StartTime = DateTime.Parse("18:00"),
                    EndTime = DateTime.Parse("19:15"),
                    Level = Level.BC,
                    Students = [johannes, frans]
                },
                new()
                {
                    Day = "Woensdag",
                    StartTime = DateTime.Parse("19:30"),
                    EndTime = DateTime.Parse("20:45"),
                    Level = Level.YY,
                    Students = [karel, hendrick, piet]
                },
                new() {
                    Day = "Woensdag",
                    StartTime = DateTime.Parse("20:45"),
                    EndTime = DateTime.Parse("22:00"),
                    Level = Level.VL,
                    Students = [jan, vincent, isaac]
                }
            };

            context.Groups.AddRange(groups);


            var enrollments = new Enrollment[]
            {
                new()
                {
                    StartDate = DateTime.ParseExact("07-09-2022", "dd-MM-yyyy",null),
                    Student = jan,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("14-09-2022", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("30-11-2022", "dd-MM-yyyy",null),
                    Student = jan,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-12-2022", "dd-MM-yyyy",null),
                    Student = jan,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("16-09-2019", "dd-MM-yyyy",null),
                    Student = rembrandt,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("20-06-2022", "dd-MM-yyyy",null),
                    Student = vincent,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("07-09-2022", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("14-11-2022", "dd-MM-yyyy",null),
                    Student = vincent,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("15-11-2022", "dd-MM-yyyy",null),
                    Student = vincent,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("05-01-2022", "dd-MM-yyyy",null),
                    Student = karel,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("12-01-2022", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("31-03-2022", "dd-MM-yyyy",null),
                    Student = karel,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-04-2022", "dd-MM-yyyy",null),
                    Student = karel,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("05-01-2022", "dd-MM-yyyy",null),
                    Student = hendrick,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("12-01-2022", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("31-03-2022", "dd-MM-yyyy",null),
                    Student = hendrick,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-04-2022", "dd-MM-yyyy",null),
                    Student = hendrick,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("15-03-2023", "dd-MM-yyyy",null),
                    Student = jhieronimus,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("22-03-2023", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("31-05-2023", "dd-MM-yyyy",null),
                    Student = jhieronimus,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-06-2023", "dd-MM-yyyy",null),
                    Student = jhieronimus,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("05-01-2022", "dd-MM-yyyy",null),
                    Student = piet,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("12-01-2022", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("31-03-2022", "dd-MM-yyyy",null),
                    Student = piet,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-04-2022", "dd-MM-yyyy",null),
                    Student = piet,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("05-09-2022", "dd-MM-yyyy",null),
                    Student = pieter,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("07-09-2022", "dd-MM-yyyy",null),
                    Student = johannes,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("14-09-2022", "dd-MM-yyyy",null),
                    Student = johannes,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("15-03-2023", "dd-MM-yyyy",null),
                    Student = aelbert,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("22-03-2023", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("31-05-2023", "dd-MM-yyyy",null),
                    Student = aelbert,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-06-2023", "dd-MM-yyyy",null),
                    Student = aelbert,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("07-09-2022", "dd-MM-yyyy",null),
                    Student = frans,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("14-09-2022", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("30-11-2022", "dd-MM-yyyy",null),
                    Student = frans,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-12-2022", "dd-MM-yyyy",null),
                    Student = frans,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("14-01-2019", "dd-MM-yyyy",null),
                    Student = jozef,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("21-09-2019", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("31-03-2019", "dd-MM-yyyy",null),
                    Student = jozef,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-04-2019", "dd-MM-yyyy",null),
                    Student = jozef,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("07-09-2022", "dd-MM-yyyy",null),
                    Student = isaac,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("14-09-2022", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("30-11-2022", "dd-MM-yyyy",null),
                    Student = isaac,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-12-2022", "dd-MM-yyyy",null),
                    Student = isaac,
                    EnrollmentType = EnrollmentType.advanced
                },
                new()
                {
                    StartDate = DateTime.ParseExact("28-08-2023", "dd-MM-yyyy",null),
                    Student = ferdinand,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("02-09-2023", "dd-MM-yyyy",null),
                    Student = ferdinand,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("28-08-2023", "dd-MM-yyyy",null),
                    Student = charley,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("02-09-2023", "dd-MM-yyyy",null),
                    Student = charley,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("28-08-2023", "dd-MM-yyyy",null),
                    Student = willem,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("02-09-2023", "dd-MM-yyyy",null),
                    Student = willem,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("15-03-2023", "dd-MM-yyyy",null),
                    Student = maurits,
                    EnrollmentType = EnrollmentType.trial
                },
                new()
                {
                    StartDate = DateTime.ParseExact("22-03-2023", "dd-MM-yyyy",null),
                    EndDate = DateTime.ParseExact("31-05-2023", "dd-MM-yyyy",null),
                    Student = maurits,
                    EnrollmentType = EnrollmentType.beginner
                },
                new()
                {
                    StartDate = DateTime.ParseExact("01-06-2023", "dd-MM-yyyy",null),
                    Student = maurits,
                    EnrollmentType = EnrollmentType.advanced
                },
            };

            context.Enrollments.AddRange(enrollments);

            context.SaveChanges();
        }
    }
}