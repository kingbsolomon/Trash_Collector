using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Trash_Collector.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, MaxLength(5)]
        [Display(Name="Zip Code Managed")]
        public string ZipCode { get; set; }

        [NotMapped]
        public string SearchBy { get; set; }

        public enum DayWeek
         {
           Sunday,
           Monday,
           Tuesday,
           Wednesday,
           Thursday,
           Friday,
           Saturday
         }
        

    }
}
