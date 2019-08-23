using System.Collections.Generic;

namespace FluentNHibernate.AspNet.Identity.Entities
{
    public class AspNetRole
    {
        internal AspNetRole()
        {
            AspNetUserRoles = new List<AspNetUserRole>();
        }

        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}