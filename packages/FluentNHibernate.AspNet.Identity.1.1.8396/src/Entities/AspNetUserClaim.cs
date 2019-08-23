namespace FluentNHibernate.AspNet.Identity.Entities
{
    internal class AspNetUserClaim
    {
        public virtual int Id { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
    }
}