using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImageDiffFinder.Models;
using ImageDiffFinder.Models.Other;

namespace ImageDiffFinder.Data
{
    public class ImageDiffContext : DbContext
    {
        public ImageDiffContext(DbContextOptions<ImageDiffContext> options)
            : base(options)
        {
        }


        // This is all that the Entity Framework needs in order to configure table-per-hierarchy inheritance.
        // As you'll see, when the database is updated, it will have
        // a Person table in place of the Student and Instructor tables.
        // https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/inheritance?view=aspnetcore-6.0
        public DbSet<Person> People { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Credential> Credentials { get; set; }



        // The recommended practice for using fluent API or attributes:
        // * Choose one of these two approaches.
        // * Use the chosen approach consistently as much as possible.

        // The OnModelCreating method in the preceding code uses the fluent API to configure EF Core behavior.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Commented by MF: Because table names are the same as class names (or
            // i think instead of below lines, we can specify related Sql table in entity class by annotations)
            // If you uncomment it, you must apply a migration to everything be matched
            //modelBuilder.Entity<Course>().ToTable("Course");
            //modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            //modelBuilder.Entity<Student>().ToTable("Student");
            //modelBuilder.Entity<Person>().ToTable("Person");

            // Is it really needed to specify many-to-many relationship between Course and Instructor entities here in order for a Pure Join Table(PJT) be created? 
            // Is this a neccessary step??
            //
            // Answer: 
            //  No, If you include a collection navigation property at both ends,
            //  EF will create a many-to-many relationship automatically
            //  between them without any configuration.
            //  In EF Core 5.0, There is nothing to add to code for many-to-many relations
            //  and all the things are hanlded by EF automatically.
            //
            //  For each many-to-many connection there should be a Join Table,
            //  Which can be Pure Join Table(PJT),Which Contains only foreign keys,
            //  or A Join Table which contain extra properties(which we should
            //  create a class to handle that) like Enrollment class in this project)
            //  But with below code a PJT will be created in database by EF automatically and 
            //  internally, EF creates an entity type to represent the join table that will
            //  be referred to as the join entity type. 
            //modelBuilder.Entity<Course>().ToTable(nameof(Course))
            //    .HasMany(c => c.Instructors)
            //    .WithMany(i => i.Courses);

            //modelBuilder.Entity<Student>().ToTable(nameof(Student));
            //modelBuilder.Entity<Instructor>().ToTable(nameof(Instructor));

            // ************************** Custom Discriminator in Inheritance **************************
            // Below lines are opional and can be removed and after that
            // if there is nothing in OnModelCreating() it can be removed too.
            // https://docs.microsoft.com/en-us/ef/core/modeling/inheritance
            // EF added the discriminator implicitly as a shadow property on the base entity of the hierarchy.
            // This property can be configured like any other.
            // You can configure the name and type of the discriminator
            // column and the values that are used to identify each type in the hierarchy:
            modelBuilder.Entity<Person>()
                        .HasDiscriminator<string>("person_type")
                        .HasValue<Person>("person")
                        .HasValue<Manager>("manager")
                        .HasValue<User>("user");

            modelBuilder.Entity<Person>()
                        .Property("person_type")
                        .HasMaxLength(200);
            // ************************** Custom Discriminator in Inheritance **************************


        }

    }
}
