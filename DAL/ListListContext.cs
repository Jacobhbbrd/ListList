using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ListListDev.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ListListDev.DAL
{
    public class ListListContext : DbContext
    {
        public ListListContext() : base("ListListContext")
        { }

        public DbSet<ListHeader> ListHeaders { get; set; }
        public DbSet<ListItem> ListItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Makes the table names not plural
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}