using HomeWorkApp.Models;
using Microsoft.AspNet.Identity;
using NHibernate;

namespace HomeWorkApp.Util.IoC_Interfaces
{
    // Интерфейс для создания сессии с СУБД
    public interface IDataBaseConnection
    {
        ISession CreateSession();
        IUserStore<User, int> CreateIdentityStoreUsers { get; }
    }
}