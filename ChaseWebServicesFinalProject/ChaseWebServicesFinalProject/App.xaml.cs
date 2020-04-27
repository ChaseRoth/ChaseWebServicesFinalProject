using System;
using Xamarin.Forms;
using ChaseWebServicesFinalProject.Pages;

namespace ChaseWebServicesFinalProject
{
    public partial class App : Application
    {
        // https://newsapi.org/ Token: ab377f107083401c9a5085038a62f6c0
        public const string Token = "ab377f107083401c9a5085038a62f6c0";

        public App()
        {
            InitializeComponent();

            MainPage = new MainMasterPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
