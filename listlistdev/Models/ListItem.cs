using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ListListDev.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ListListDev.Models
{
    public class ListItem
    {
        public int ID { get; set; }
        public int ListHeaderID { get; set; }
        public string Text { get; set; }

        public ListHeader ListHeader { get; set; }

        public ListItem()
        { }

        public ListItem(CreateListItemViewModel model)
        {
            this.ListHeaderID = model.ListHeaderID;
            this.Text = model.Text;
        }
    }
}