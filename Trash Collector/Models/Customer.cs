using Microsoft.AspNetCore.Identity;
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

        [Display(Name="MI")]
        public char MiddleInit { get; set; }

        [Required, Phone]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        [Display(Name="Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required, MaxLength(2)]
        public string State { get; set; }

        [Required, MaxLength(5)]
        public int ZipCode { get; set; }

        [NotMapped]
        public float Longtitude { get; set; }

        [NotMapped]
        public float Latitude { get; set; }

        [CreditCard]
        public int CreditCard { get; set; }

        [NotMapped]
        public DateTime CustomPickup { get; set; }

        [NotMapped]
        public DateTime SuspendStart { get; set; }

        [NotMapped]
        public DateTime SuspendEnd { get; set; }

        [NotMapped]
        public double CustomerBalance
        {
            get { return balance; }
            set { balance = value; }
        }

        [Required]
        [Range(1,7)]
        public byte PickUpDay { get; set; }
    }

    public enum DaysWeek
    {
        Sunday = 1,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }
}
