using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Project.Models
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<LibraryDbContext>
    {
        protected override void Seed(LibraryDbContext context)
        {
            // Create roles
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("Librarian"))
                roleManager.Create(new IdentityRole("Librarian"));

            if (!roleManager.RoleExists("Member"))
                roleManager.Create(new IdentityRole("Member"));

            // ── LIBRARIES ──────────────────────────────────────────
            var libraries = new List<Library>
            {
                new Library
                {
                    Name = "KOI Campus Library",
                    Location = "Kent Street, Sydney NSW 2000",
                    OperatingHours = "Mon-Fri 8am-10pm, Sat 9am-5pm",
                    ContactNumber = "02-9283-3583",
                    Email = "library@koi.edu.au"
                },
                new Library
                {
                    Name = "Sydney Central Library",
                    Location = "123 George Street, Sydney NSW 2000",
                    OperatingHours = "Mon-Fri 9am-8pm, Sat-Sun 10am-5pm",
                    ContactNumber = "02-9876-5432",
                    Email = "sydney@library.com"
                }
            };
            context.Libraries.AddRange(libraries);
            context.SaveChanges();

            // ── BOOKS ──────────────────────────────────────────────
            var books = new List<Book>
            {
                new Book
                {
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    Genre = "Programming",
                    ISBN = "978-0132350884",
                    IsAvailable = true,
                    Summary = "A handbook of agile software craftsmanship.",
                    PublicationYear = 2008,
                    TotalCopies = 3,
                    LibraryId = 1
                },
                new Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    Genre = "Fiction",
                    ISBN = "978-0451524935",
                    IsAvailable = true,
                    Summary = "A dystopian novel about a totalitarian society.",
                    PublicationYear = 1949,
                    TotalCopies = 5,
                    LibraryId = 1
                },
                new Book
                {
                    Title = "Sapiens",
                    Author = "Yuval Noah Harari",
                    Genre = "History",
                    ISBN = "978-0062316097",
                    IsAvailable = true,
                    Summary = "A brief history of humankind.",
                    PublicationYear = 2011,
                    TotalCopies = 4,
                    LibraryId = 1
                },
                new Book
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Genre = "Fiction",
                    ISBN = "978-0743273565",
                    IsAvailable = true,
                    Summary = "A classic story of the American Dream.",
                    PublicationYear = 1925,
                    TotalCopies = 3,
                    LibraryId = 2
                },
                new Book
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Genre = "Fiction",
                    ISBN = "978-0061120084",
                    IsAvailable = true,
                    Summary = "A Pulitzer Prize winning classic.",
                    PublicationYear = 1960,
                    TotalCopies = 3,
                    LibraryId = 2
                },
                new Book
                {
                    Title = "Introduction to Algorithms",
                    Author = "Thomas H. Cormen",
                    Genre = "Programming",
                    ISBN = "978-0262033848",
                    IsAvailable = true,
                    Summary = "The definitive algorithms textbook.",
                    PublicationYear = 2009,
                    TotalCopies = 2,
                    LibraryId = 1
                }
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // ── GROUP MEMBERS (shown on homepage) ──────────────────
            var groupMembers = new List<GroupMember>
            {
                new GroupMember
                {
                    StudentId = "YOUR_ID_1",
                    FullName  = "Your Name",
                    Role      = "Team Lead / Full-Stack Developer"
                },
                new GroupMember
                {
                    StudentId = "YOUR_ID_2",
                    FullName  = "Member 2",
                    Role      = "Backend Developer"
                },
                new GroupMember
                {
                    StudentId = "YOUR_ID_3",
                    FullName  = "Member 3",
                    Role      = "Frontend Developer"
                },
                new GroupMember
                {
                    StudentId = "YOUR_ID_4",
                    FullName  = "Member 4",
                    Role      = "Database Designer"
                }
            };
            context.GroupMembers.AddRange(groupMembers);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}