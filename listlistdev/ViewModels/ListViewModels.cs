using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ListListDev.Models;
using ListListDev.ViewModels;

namespace ListListDev.ViewModels
{
    public class DashboardViewModel
    {
        public List<ListHeader> Lists { get; set; }

        public DashboardViewModel()
        {
            this.Lists = new List<ListHeader>();
        }

        public DashboardViewModel(List<ListHeader> lists)
        {
            this.Lists = lists;
        }
    }

    public class CreateListHeaderViewModel
    {
        [Required(ErrorMessage = "You must enter a list title")]
        [StringLength(25)]
        [DisplayName("List Title")]
        public string Title { get; set; }
    }

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

    public class EditListItemViewModel
    {
        [Required(ErrorMessage = "You must enter a name for the item")]
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