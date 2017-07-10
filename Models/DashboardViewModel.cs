using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListListDev.Models
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
}