namespace FluentNHibernate.AspNet.Identity.Entities
{
    internal class AspNetUserLogin
    {
        public virtual int Id { get; set; }
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}