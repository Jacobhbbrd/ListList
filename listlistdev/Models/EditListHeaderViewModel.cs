using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ListListDev.Models
{
    public class EditListHeaderViewModel
    {
        [Required(ErrorMessage = "You must enter a list title")]
        [StringLength(25)]
        [DisplayName("List Title")]
        public string Title { get; set; }
        public int ID { get; set; }

        public EditListHeaderViewModel()
        { }

        public EditListHeaderViewModel(ListHeader list)
        {
            this.Title = list.Title;
            this.ID = list.ID;
        }
    }
}