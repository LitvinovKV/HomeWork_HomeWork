using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWorkApp.Models
{
    public class SubCatalogModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id {get; set;}

        // HiddenInput не подходит т.к. при DropDownListFor Затирается значение
        [ScaffoldColumn(false)]
        public int? ParentId { get; set; }

        [Required]
        [DisplayName("Название подкаталога")]
        public string Name { get; set; }
    }
}