using FluentNHibernate.AspNet.Identity.Entities;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AspNet.Identity.Mapping
{
    internal class AspNetRoleMap : ClassMap<AspNetRole>
    {
        public AspNetRoleMap()
        {
            Table("AspNetRole");
            LazyLoad();
            Id(i => i.Id).GeneratedBy.Assigned().Length(128).Not.Nullable();
            Map(i => i.Name).Column("`Name`").Not.Nullable().Length(256).UniqueKey("a");
            HasMany(i => i.AspNetUserRoles).KeyColumn("RoleId").Cascade.All();
        }
    }
}