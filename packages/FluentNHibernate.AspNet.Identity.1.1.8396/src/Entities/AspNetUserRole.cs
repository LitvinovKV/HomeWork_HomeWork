namespace FluentNHibernate.AspNet.Identity.Entities
{
    public class AspNetUserRole
    {
        internal AspNetUserRole()
        {
        }

        public virtual int Id { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual AspNetRole Role { get; set; }
    }
}