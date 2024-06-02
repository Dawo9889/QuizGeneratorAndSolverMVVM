﻿using PierwszePodejscieDoQuizu.Database;
using PierwszePodejscieDoQuizu.Database.Entities;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PierwszePodejscieDoQuizu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var database = new QuizDbContext();
            database.Database.EnsureCreated();

        }
    }
    
}
