using Caliburn.Micro;
using System.Windows;
using TaskManager.ViewModels;

namespace TaskManager
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;
        public Bootstrapper()
        {
            Initialize();
        }
        protected override void Configure()
        {
            _container = new SimpleContainer();
            _container.Instance(_container);
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.PerRequest<MainViewModel>();
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
