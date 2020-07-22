using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Trash_Collector.Models
{
    public class Customer
    {
        private double balance = 0;

        [Key]
        public int Id { get; set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Required, Phone]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required, MaxLength(2)]
        public string State { get; set; }

        [Required, MaxLength(5)]
        public string ZipCode { get; set; }

        public double Longtitude { get; set; }

        public double Latitude { get; set; }

        [Display(Name ="Credit Card")]
        public int CreditCard { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Custom Pickup Date")]
        public DateTime CustomPickup { get; set; }
        public bool BeenPicked { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Suspend Start")]
        public DateTime SuspendStart { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Suspend End")]
        public DateTime SuspendEnd { get; set; }

        [Display(Name ="Account Balance")]
        public double CustomerBalance
        {
            get { return balance; }
            set { balance = value; }
        }
       
        [Display(Name ="Pickup Day")]
        public string DayWeek { get; set; }

    }

    /*public enum DayWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }*/
}
