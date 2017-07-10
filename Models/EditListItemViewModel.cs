using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ListListDev.Models
{
    public class EditListItemViewModel
    {
        [Required(ErrorMessage = "You must enter a name for the list item")]
        [StringLength(150)]
        [DisplayName("List Item Text")]
        public string Text { get; set; }
        public int ListHeaderID { get; set; }
        public int ID { get; set; }

        public EditListItemViewModel()
        { }

        public EditListItemViewModel(ListItem model)
        {
            this.Text = model.Text;
            this.ListHeaderID = model.ListHeaderID;
            this.ID = model.ID;
        }
    }
}