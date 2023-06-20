using Lab4.CustomIdentity;
using Lab4.Models;

namespace Lab4.Data
{
    public class Seed
    {
        public static async void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                var roleManager = serviceScope.ServiceProvider.GetService<CustomRoleManager>();

                var userManager = serviceScope.ServiceProvider.GetService<CustomUserManager>();

                context.Database.EnsureCreated();

                if (!context.Addresses.Any())
                {
                    context.Addresses.AddRange(new List<Address>()
                    {
                        new Address()
                        {
                            Street = "Stockholmgatan 20A",
                            City = "Stockholm",
                            PostalCode = "85340"
                        },

                        new Address()
                        {
                            Street = "Granloholmvägen 15B",
                            City = "Söderhamn",
                            PostalCode = "85720"
                        },

                        new Address()
                        {
                            Street = "Sundsvallsvägen 22D",
                            City = "Sundsvall",
                            PostalCode = "85350"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Roles.Any())
                {
                    var admin = new Role
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };
                    await roleManager.CreateAsync(admin);

                    var customer = new Role
                    {
                        Name = "Customer",
                        NormalizedName = "CUSTOMER"
                    };
                    await roleManager.CreateAsync(customer);
                }

                if (!context.Users.Any())
                {
                    var newAdmin = new User()
                    {
                        FirstName = "Admin",
                        LastName = "Aldor",
                        FK_AddressId = 1,
                        UserName = "admin",
                        FK_RoleId = 1,
                        Email = "admin@biblioteket.se",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newAdmin, "adminMaX33!");

                    var newCustomer = new User()
                    {
                        FirstName = "Customer",
                        LastName = "Aldor",
                        FK_AddressId = 2,
                        UserName = "user",
                        FK_RoleId = 2,
                        Email = "user@MA.se",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newCustomer, "customerMaX33!");
                }

                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(new List<Author>()
                    {
                        new Author()
                        {
                            FirstName = "Lucinda",
                            LastName = "Riley"
                        },

                        new Author()
                        {
                            FirstName = "Harry",
                            LastName = "Whittaker"
                        },

                        new Author()
                        {
                            FirstName = "Ulf",
                            LastName = "Kvensler"
                        },

                        new Author()
                        {
                            FirstName = "Emily",
                            LastName = "Henry"
                        },
                    });
                    context.SaveChanges();
                }

                //add Availability before books!!
                if (!context.Availabilities.Any())
                {
                    context.Availabilities.AddRange(new List<Availability>()
                    {
                        new Availability()
                        {
                            AvailabilityType = "Available"
                        },

                        new Availability()
                        {
                            AvailabilityType = "Unavailable"
                        }
                    });

                    context.SaveChanges();
                }

                if (!context.Books.Any())
                {
                    context.Books.AddRange(new List<Book>()
                    {
                        new Book()
                        {
                            Title = "Atlas: Historien om Pa Salt",
                            Description = "Atlas: Historien om Pa Salt är den efterlängtade, avslutande delen av Lucinda Rileys fantastiska saga om de sju systrarna. Den tar med läsaren på en resa genom historien och runt hela världen och utgör ett oförglömligt slut på en internationell världssuccé.\r\n\r\nDe sju systrarna har samlats på båten Titan för att ta ett sista avsked av den mystiska far de alla älskade så djupt. Till allas förvåning valde Pa Salt att ge nyckeln till deras förflutna till den sjunde systern som äntligen hittats. Men för varje sanning som avslöjas uppstår en ny fråga. Systrarna tvingas inte bara inse att de knappt kände sin far, de måste dessutom acceptera att hans sedan länge begravda hemligheter fortfarande kan få konsekvenser för deras liv.",
                            Quantity = 15,
                            Image = "https://bilder.akademibokhandeln.se/images_akb/9789180060516_383/atlas-historien-om-pa-salt",
                            FK_AvailabilityId = 1,
                        },
                        new Book()
                        {
                            Title = "Sarek",
                            Description = "Svenska Deckarakademins pris för bästa debutroman 2022\r\n\r\nTre vänner som brukar fjällvandra tar på årets tripp till Sarek med en fjärde part - Anna och Henrik har svårt att säga nej när Milena vill introducera sin nya kille. Jacob är trevlig, men Anna genomfars av tanken \"jag känner igen honom\". Vandringen utvecklas snart till en mardröm - att ta med Jacob visar sig vara ett ödesdigert beslut som kommer att förändra deras liv för alltid.\r\n\r\nSarek nominerades även till priset Årets Bok 2022.",
                            Quantity = 15,
                            Image = "https://bilder.akademibokhandeln.se/images_akb/9789100801717_383/sarek",
                            FK_AvailabilityId = 1,
                        },
                        new Book()
                        {
                            Title = "Bokälskare",
                            Description = "Hela Nora Stephens liv består av böcker. Som litterär agent i New York gör hon allt för sina författare, men också för sin älskade lillasyster Libby. Så när Libby ber Nora följa med till den idylliska småstaden Sunshine Falls i North Carolina ställer hon upp, om än motvilligt. Planen är att de ska ha en avkopplande systersemester tillsammans, samtidigt som Libby i hemlighet hoppas att Nora ska bli hjälte i sitt eget liv och inte bara i andras. Men i stället för picknickar i mysiga gläntor och heta möten med någon sexig skogshuggare eller snygg landsortsläkare, springer Nora ideligen på Charlie Lastra, en butter förläggare som också kommer från New York. Det skulle kanske ha kunnat bli gulligt och romantiskt, om det inte vore för det faktum att Nora och Charlie redan har träffats massor av gånger hemma i stan, och det har aldrig varit det minsta gulligt",
                            Quantity = 15,
                            Image = "https://bilder.akademibokhandeln.se/images_akb/9789189306837_383/bokalskare",
                            FK_AvailabilityId = 1,
                        },
                    });
                    context.SaveChanges();
                }

                if (!context.BookAuthorsRT.Any())
                {
                    context.BookAuthorsRT.AddRange(new List<Book_Author_RT>()
                    {
                        new Book_Author_RT()
                        {
                            FK_BookId = 1,
                            FK_AuthorId = 1
                        },
                        new Book_Author_RT()
                        {
                            FK_BookId = 1,
                            FK_AuthorId = 2
                        },
                        new Book_Author_RT()
                        {
                            FK_BookId = 2,
                            FK_AuthorId = 3
                        },
                        new Book_Author_RT()
                        {
                            FK_BookId = 3,
                            FK_AuthorId = 4
                        },
                    });

                    context.SaveChanges();
                }

                if (!context.CustomerBooksRT.Any())
                {
                    var today = DateTime.Now;

                    var borrow1 = new Customer_Book_RT()
                    {
                        FK_BookId = 1,
                        FK_CustomerId = 2,
                        BorrowDate = DateTime.Now.Date,
                        ReturnDate = today.AddDays(7),

                    };

                    context.CustomerBooksRT.Add(borrow1);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
