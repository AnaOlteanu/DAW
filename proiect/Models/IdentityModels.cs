using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace proiect.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("proiectBD1", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieType> MovieTypes { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        public static void addIntoDb(ApplicationDbContext ctx)
        {
            MovieType type1 = new MovieType { MovieTypeId = 10, Name = "Scurtmetraj" };
            MovieType type2 = new MovieType { MovieTypeId = 11, Name = "Lungmetraj" };

            ctx.MovieTypes.Add(type1);
            ctx.MovieTypes.Add(type2);

            Genre genre1 = new Genre { Name = "Comedie" };
            Genre genre2 = new Genre { Name = "Actiune" };
            Genre genre3 = new Genre { Name = "Drama" };
            Genre genre4 = new Genre { Name = "Romantic" };
            Genre genre5 = new Genre { Name = "Horror" };

            ctx.Genres.Add(genre1);
            ctx.Genres.Add(genre2);
            ctx.Genres.Add(genre3);
            ctx.Genres.Add(genre4);
            ctx.Genres.Add(genre5);

            ctx.Actors.Add(new Actor
            {
                Name = "Daniel Radcliffe",
                ContactInfo = new ContactInfo
                {
                    PhoneNumber = "0722985515",
                    BirthDay = 14,
                    BirthMonth = 12,
                    BirthYear = 1988,
                    GenderType = Gender.Male
                }
            });


            ctx.Movies.Add(new Movie
            {
                Title = "Titanic",
                ReleaseDate = "12.12.2020",
                Genres = new List<Genre> {
                    genre2,
                    genre4
                },
                Description = "Un film senzational ce merita vazut de intreg mapamondul.",
                Duration = 60,
                MovieTypeId = type2.MovieTypeId,
                Actors = new List<Actor> {
                    new Actor { Name = "Leonardo Dicaprio",
                                 ContactInfo = new ContactInfo
                                 {
                                 PhoneNumber = "0722985515",
                                 BirthDay = 14,
                                 BirthMonth = 12,
                                 BirthYear = 1988, 
                                 GenderType = Gender.Male
                                 }
                    },
                    new Actor { Name = "Kate Winslet",
                                 ContactInfo = new ContactInfo
                                 {
                                 PhoneNumber = "0723737665",
                                 GenderType = Gender.Female
                                 
                                 }
                    }
                }
            });
            ctx.SaveChanges();
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public class Initp : DropCreateDatabaseAlways<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext ctx)
            {
                addIntoDb(ctx);
                base.Seed(ctx);
            }
        }
    }
}