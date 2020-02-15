
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
    public class TracingHistory : _CommonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Access Time")]
        public DateTime AccessTime { get; set; }

        [Display(Name = "Document ID")]
        public int DocId { get; set; }
        
        [Display(Name = "Doc Name")]
        public string DocName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(200)]
        [Display(Name = "FileURL")]
        public string FileURL { get; set; }
        
        [Display(Name = "UserId")]
        public int UserId { get; set; }
        
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(45)]
        [Display(Name = "Type")]
        public string Type { get; set; }
        
    }
}
