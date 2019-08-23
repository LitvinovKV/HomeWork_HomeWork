using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HomeWorkApp.Models
{
    public class ArticleModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        
        [ScaffoldColumn(false)]
        public int SubCatalogId { get; set; }

        [Required]
        [DisplayName("Название статьи")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Текст статьи")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}