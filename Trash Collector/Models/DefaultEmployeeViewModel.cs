using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash_Collector.Models
{
    public class DefaultEmployeeViewModel
    {
        public List<Customer> Customers { get; set; }
        public SelectList DaysOfWeekList { get; set; }

        [Display(Name = "Selected Day")]
        public string SelectedDay { get; set; }
    }
}
