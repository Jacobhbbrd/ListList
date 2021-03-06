﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ListListDev.Models;
using ListListDev.ViewModels;

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

        public ListItem(EditListItemViewModel model)
        {
            this.ID = model.ID;
            this.ListHeaderID = model.ListHeaderID;
            this.Text = model.Text;
        }
    }
}