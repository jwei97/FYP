
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
    public class Document : _CommonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Doc Name")]
        public string DocName { get; set; }
        
        [StringLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(200)]
        [Display(Name = "FileURL")]
        public string FileURL { get; set; }

        [Display(Name = "Module ID")]
        public int ModuleId { get; set; }

        [Display(Name = "Upload By Id")]
        public int UploadById { get; set; }

        [StringLength(50)]
        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }

        [ForeignKey("UploadById")]
        public virtual Account AccountUpload { get; set; }

        public virtual List<TracingHistory> ListTracingHistory { get; set; }
    }
}
