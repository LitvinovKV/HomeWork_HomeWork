using HomeWorkApp.Util.IoC_Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HomeWorkApp.Util
{
    // Вспомогательный класс в котором прописывается логика работы Ninject
    public class NinjectResolver : IDependencyResolver
    {

        private IKernel kernel;

        public NinjectResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings(kernel);
        }

        // Связи интерфейсов и их реализаций
        private void AddBindings(IKernel karnel)
        {
            kernel.Bind<IDataBaseConnection>().To<DataBaseConnection>();
            kernel.Bind<ICatalogOfSubcatalogs>().To<CatalogOfSubcatalog>();
            kernel.Bind<ICatalogOfArticles>().To<CatalogOfArticles>();
        }

        public NinjectResolver() : this(new StandardKernel()) { }


        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}