using AutoMapper;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Windows;
using TaskManager.Data;
using TaskManager.Data.SQLite;
using TaskManager.Data.SqlServer;
using TaskManager.Models;
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
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.SqlServer.Task, TaskDisplayModel>();
                cfg.CreateMap<TaskDisplayModel, Data.SqlServer.Task>();

            });
            var mapper = mapperConfig.CreateMapper();
            _container.Instance(mapper);

            _container.Instance(_container);
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .PerRequest<SqliteRepository>()
                .PerRequest<SqlServerRepository>()
                .PerRequest<HomeViewModel>()
                .PerRequest<MainViewModel>();

            
            
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
