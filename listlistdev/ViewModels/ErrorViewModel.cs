using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListListDev.ViewModels
{
    public class ErrorViewModel
    {
        public string errorMsg { get; set; }

        public ErrorViewModel()
        { }

        public ErrorViewModel(string message)
        {
            this.errorMsg = message;
        }
    }
}