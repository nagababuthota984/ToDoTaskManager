﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;
using TaskManager.Data;
using TaskManager.ViewModels;

namespace TaskManager
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public Bootstrapper()
        {
            _container = new();
            Initialize();
        }

        protected override void Configure()
        {
            _container.Instance(_container);
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<SqliteRepository>()
                .Singleton<SqlServerRepository>()
                .PerRequest<MainViewModel>()
                .PerRequest<CreateTaskViewModel>()
                .PerRequest<ListViewModel>()
                .PerRequest<HomeViewModel>();
            MapperHelper.CreateMapper();
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

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
