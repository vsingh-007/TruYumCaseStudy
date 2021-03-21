using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace truYum.Models
{
    public class MenuItem
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public bool Active { get; set; }    

        [Display(Name = "Date Of Launch")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfLaunch { get; set; }

        [Display(Name = "Free Delivery")]
        public bool FreeDelivery { get; set; }

        //public int CategoryID { get; set; }
        public Category Categories { get; set; }

    }
}