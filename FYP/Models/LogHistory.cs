
using FYP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class LogHistory : _CommonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Display(Name = "User id")]
        public int Userid { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(45)]
        [Display(Name = "Type")]
        public string Type { get; set; }
        
        [Required]
        [Display(Name = "Access Time")]
        public DateTime AccessTime { get; set; }
        
    }
}
