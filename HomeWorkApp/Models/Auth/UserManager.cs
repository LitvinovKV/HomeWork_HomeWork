﻿using Microsoft.AspNet.Identity;

namespace HomeWorkApp.Models
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store) : base(store)
        {
            UserValidator = new UserValidator<User, int>(this);
            PasswordValidator = new PasswordValidator() { RequiredLength = 6 };
        }
    }
}