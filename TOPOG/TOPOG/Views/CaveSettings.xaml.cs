using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOPOG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaveSettings : ContentPage
    {
        public CaveSettings()
        { 
            InitializeComponent(); 
        } 

        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Toast.MakeText(Android.App.Application.Context, "no availible connection", ToastLength.Short).Show();
            Cave c = (Cave)App.Current.Properties["Cave"];
            nm.Text = c.Name;
            ds.Text = c.Description;
            athours.Text = c.Athours;
        }
        public string path = Serial.path;
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (nm.Text == "") return;
            Cave c = new Cave();
            Cave c1 = (Cave)App.Current.Properties["Cave"];
            c.Name = nm.Text;
            c.Description = ds.Text;
            c.Athours = athours.Text;
            //Toast.MakeText(Android.App.Application.Context, c.Name, ToastLength.Short).Show();
            if ((c.PathA != null && !(File.Exists(c1.PathA) && File.Exists(c.PathA)))||c.Name==c1.Name)
            {
                List<Cave> CavesA = (List<Cave>)Serial.Open(typeof(List<Cave>),path);//JsonConvert.DeserializeObject<List<Cave>>(File.ReadAllText(path));
                //Toast.MakeText(Android.App.Application.Context, c.Name+" "+c1.Name, ToastLength.Short).Show();
                bool b = false; int i = 0;
                foreach (Cave ca in CavesA)
                {
                    if (ca.Name==c1.Name)
                    {
                        b = true;
                        i = CavesA.IndexOf(ca);
                    }
                }
                if (b)
                {
                    CavesA[i] = c;
                    //Toast.MakeText(Android.App.Application.Context, "no connection", ToastLength.Short).Show();
                }
                else
                {
                    CavesA.Add(c);
                }
                App.Current.Properties["Cave"] = c;
                //string createText = JsonConvert.SerializeObject(CavesA);
                //File.WriteAllText(path, createText);
                Serial.Save(CavesA, typeof(List<Cave>), path);

                if (!File.Exists(c1.PathA))
                {
                    Serial.Save(App.Current.Properties["Semka"], typeof(Semka), c.PathA);
                    Semka s = (Semka)App.Current.Properties["Semka"];
                    s.Name = c.Name;
                    s.Athours = c.Athours;
                    s.Description = c.Description;
                    App.Current.Properties["Semka"] = s;
                    /*string createText1 = JsonConvert.SerializeObject(App.Current.Properties["Semka"]);
                    File.WriteAllText(c.PathA, createText1);*/
                   
                }
                else
                {
                    //Toast.MakeText(Android.App.Application.Context, "no availible connection", ToastLength.Short).Show();
                    Semka s = (Semka)App.Current.Properties["Semka"];
                    s.Name = c.Name;
                    s.Athours = c.Athours;
                    s.Description = c.Description;
                    App.Current.Properties["Semka"] = s;
                    /*string createText1 = JsonConvert.SerializeObject(App.Current.Properties["Semka"]);
                    File.Move(c1.PathA, c.PathA);
                    File.WriteAllText(c.PathA, createText1); */
                    File.Move(c1.PathA, c.PathA);
                    Serial.Save(App.Current.Properties["Semka"], typeof(Semka), c.PathA);
                }
            }
        }
    }
}