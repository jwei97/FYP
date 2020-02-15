
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class _CommonModel
    {
        [DefaultValue(false)]
        [Display(Name = "Deleted")]
        public bool Deleted { get; set; }
    }
}
