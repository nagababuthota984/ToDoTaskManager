using Caliburn.Micro;
using System.Windows;
using TaskManager.ViewModels;

namespace TaskManager
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;
        
        //gets the control first (order of exec-1)
        public Bootstrapper()
        {
            _container = new SimpleContainer();
            Initialize();
        }

        //called by the Initialize() method. (order of exec-2)
        protected override void Configure()
        {
            _container.Instance(_container);
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.PerRequest<MainViewModel>();
        }

        //gets the control last.. not called by initialize(). Gets control when startup event is raised in app.xaml
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
