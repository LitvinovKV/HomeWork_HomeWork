using System;
using System.Collections.Generic;
using FluentNHibernate.AspNet.Identity.Entities;
using Microsoft.AspNet.Identity;

namespace FluentNHibernate.AspNet.Identity
{
    public class IdentityUser : AspNetUser, IUser
    {
        public IdentityUser()
        {
            Claims = new List<IdentityUserClaim>();
            Roles = new List<string>();
            Logins = new List<UserLoginInfo>();
            Id = Guid.NewGuid().ToString();
            LockoutEnabled = true;
        }

        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        public virtual DateTime? LockoutEndDate { get; set; }
        public virtual bool TwoFactorAuthEnabled { get; set; }
        public virtual List<string> Roles { get; set; }
        public virtual List<IdentityUserClaim> Claims { get; set; }
        public virtual List<UserLoginInfo> Logins { get; set; }
    }
}