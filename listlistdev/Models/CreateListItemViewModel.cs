using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ListListDev.Models
{
    public class CreateListItemViewModel
    {
        [Required(ErrorMessage = "You must enter a name for the item")]
        [StringLength(150)]
        [DisplayName("List Item Text")]
        public string Text { get; set; }
        public int ListHeaderID { get; set; }
    
        public CreateListItemViewModel()
        { }

        public CreateListItemViewModel(int id)
        {
            this.ListHeaderID = id;
        }
    }
}