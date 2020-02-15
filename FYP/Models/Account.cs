
using FYP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static FYP.Helper.ConstantManager.SecurityConstants;

namespace FYP.Models
{
    public class Account : _CommonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [StringLength(45)]
        [Display(Name = "Type")]
        public string Type { get; set; }
        
    }
}
