
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
    public class Module : _CommonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        
        [StringLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Display(Name = "Course ID")]
        public int CourseID { get; set; }
        

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }
        
    }
}
