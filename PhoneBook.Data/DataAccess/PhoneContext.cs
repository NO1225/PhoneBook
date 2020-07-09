using Microsoft.EntityFrameworkCore;
using PhoneBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Data.DataAccess
{
    public class PhoneContext : DbContext
    {
        public PhoneContext(DbContextOptions options) : base (options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>().HasIndex(c => c.ImagePath).IsUnique();
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
