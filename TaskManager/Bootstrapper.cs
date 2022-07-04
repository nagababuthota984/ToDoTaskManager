using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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
            _container = new();
            Initialize();
        }

        //called by the Initialize() method. (order of exec-2)
        protected override void Configure()
        {
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.PerRequest<HomeViewModel>();
            _container.PerRequest<MainViewModel>();
        }
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        //not called by initialize(). Gets control when the app starts up.
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
