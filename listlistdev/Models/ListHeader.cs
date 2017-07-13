using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ListListDev.Models;
using ListListDev.ViewModels;

namespace ListListDev.Models
{
    public class ListHeader
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string UserID { get; set; }

        public List<ListItem> ListItems { get; set; }

        public ListHeader()
        { }

        public ListHeader(CreateListHeaderViewModel model, string userID)
        {
            this.Title = model.Title;
            this.UserID = userID;
        }

        public ListHeader(EditListHeaderViewModel model, string userID)
        {
            this.ID = model.ID;
            this.Title = model.Title;
            this.UserID = userID;
        }
    }
}