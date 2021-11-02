using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TOPOG.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOPOG
{
    public partial class App : Application
    {
        public App()
        {
            Current.Properties.Add("Cave",new Cave());
            Current.Properties.Add("IC", new bool());
            Current.Properties.Add("Rv", new Predst(new Izm(0,0,0),"","",0));
            Current.Properties["Rv"] = null;
            Current.Properties.Add("Semka",new Semka());
            InitializeComponent();
            
            MainPage = new Sh();
        }
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Saved Caves.cv";
        protected override void OnStart()
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"Saved Caves";
        }

        protected override void OnSleep()
        {
            ((Semka)App.Current.Properties["Semka"]).save();
            
        }

        protected override void OnResume()
        {
        }
    }
}
