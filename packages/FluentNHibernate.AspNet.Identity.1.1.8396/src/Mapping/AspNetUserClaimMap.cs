using FluentNHibernate.AspNet.Identity.Entities;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AspNet.Identity.Mapping
{
    internal class AspNetUserClaimMap : ClassMap<AspNetUserClaim>
    {
        public AspNetUserClaimMap()
        {
            Table("`AspNetUserClaim`");
            LazyLoad();
            Id(i => i.Id).GeneratedBy.Identity().Column("`Id`");
            References(i => i.User).Column("`UserId`").Not.Nullable();
            Map(i => i.Type).Column("`Type`").Length(4001);
            Map(i => i.Value).Column("`Value`").Length(4001);
        }
    }
}