using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;

namespace HomeWorkApp.Models
{
    public class SubCatalog 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ISet<Article> Articles { get; set; }
        public virtual SubCatalog ParentCatalog { get; set; }
        public virtual ISet<SubCatalog> SubCatalogs { get; set; }

        public SubCatalog()
        {
            Articles = new HashSet<Article>();
            SubCatalogs = new HashSet<SubCatalog>();
        }
    }

    public class SubCatalogMap : ClassMap<SubCatalog>
    {
        public SubCatalogMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable();
            HasMany(x => x.Articles).Cascade.All().AsSet().Inverse();
            HasMany(x => x.SubCatalogs).KeyColumn("ParentCatalog_Id").Cascade.All().AsSet().Inverse();
            References(x => x.ParentCatalog).Column("ParentCatalog_Id");
            Cache.ReadWrite();
        }
    }
}