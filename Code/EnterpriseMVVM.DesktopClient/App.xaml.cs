﻿using EnterpriseMVVM.DesktopClient.ViewModels;
using EnterpriseMVVM.DesktopClient.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EnterpriseMVVM.Data;
using Unity;

namespace EnterpriseMVVM.DesktopClient
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);

            //MainWindow window = new MainWindow
            //{
            //    DataContext = new MainViewModel()
            //};

            //window.ShowDialog();

            base.OnStartup(e);

            var container = new UnityContainer();

            container.RegisterType<IBusinessContext, BusinessContext>();
            container.RegisterType<MainViewModel>();

            var window = new MainWindow
            {
                DataContext = container.Resolve<MainViewModel>()
            };

            window.ShowDialog();
        }
    }
}
