using FluentNHibernate.AspNet.Identity.Entities;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AspNet.Identity.Mapping
{
    internal class AspNetUserRoleMap : ClassMap<AspNetUserRole>
    {
        public AspNetUserRoleMap()
        {
            Table("AspNetUserRole");
            LazyLoad();
            Id(i => i.Id).GeneratedBy.Identity().Not.Nullable();
            References(i => i.User).Column("UserId").Not.Nullable().UniqueKey("uq1");
            References(i => i.Role).Column("RoleId").Not.Nullable().UniqueKey("uq1");
        }
    }
}