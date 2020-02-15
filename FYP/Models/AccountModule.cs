
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
    public class AccountModule : _CommonModel
    {

        [Key, Column(Order = 1)]
        [Display(Name = "Account Id")]
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        [Key, Column(Order = 2)]
        [Display(Name = "Module Id")]
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }




    }
}
