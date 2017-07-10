using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ListListDev.Models;

namespace ListListDev.DAL
{
    // Deletes all database data and adds test records when database is changed
    public class ListListInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ListListContext>
    {
        protected override void Seed(ListListContext context)
        {
            // Creates lists for the user jacob@gmail.com and some items for those lists
            var lists = new List<ListHeader>
            {
                new ListHeader{Title="1st List", UserID="2e9cfce9-8cfd-4221-8ecc-3dd30f560d79"},
                new ListHeader{Title="2nd List", UserID="2e9cfce9-8cfd-4221-8ecc-3dd30f560d79"},
                new ListHeader{Title="3rd List", UserID="2e9cfce9-8cfd-4221-8ecc-3dd30f560d79"}
            };

            lists.ForEach(l => context.ListHeaders.Add(l));
            context.SaveChanges();

            var listItems = new List<ListItem>
            {
                new ListItem{ListHeaderID = 1, Text="List Item 1"},
                new ListItem{ListHeaderID = 1, Text="List Item 2"},
                new ListItem{ListHeaderID = 2, Text="List Item 1"},
                new ListItem{ListHeaderID = 2, Text="List Item 2"},
                new ListItem{ListHeaderID = 3, Text="List Item 1"},
                new ListItem{ListHeaderID = 3, Text="List Item 2"}
            };

            listItems.ForEach(li => context.ListItems.Add(li));
            context.SaveChanges();
        }
    }
}