using FluentNHibernate.AspNet.Identity.Entities;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AspNet.Identity.Mapping
{
    internal class AspNetUserLoginMap : ClassMap<AspNetUserLogin>
    {
        public AspNetUserLoginMap()
        {
            Table("`AspNetUserLogin`");
            Id(i => i.Id).Column("`Id`").GeneratedBy.Identity();
            LazyLoad();
            References(i => i.User).Column("`UserId`").Not.Nullable().UniqueKey("uq3");
            Map(i => i.LoginProvider).Column("`LoginProvider`").Length(128).Not.Nullable().UniqueKey("uq3");
            Map(i => i.ProviderKey).Column("`ProviderKey`").Not.Nullable().Length(128);
        }
    }
}