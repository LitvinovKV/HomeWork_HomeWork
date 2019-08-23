using FluentNHibernate.Mapping;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeWorkApp.Models
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }
        public virtual SubCatalog SubCatalog { get; set; }
    }

    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Text).Not.Nullable().Length(1000);
            References(x => x.SubCatalog);
            Cache.ReadWrite();
        }
    }
}