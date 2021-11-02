using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Newtonsoft.Json;
using System.IO;
using Android.Widget;
using Android.Content;

namespace TOPOG.Views
{

    
    public partial class Mn : ContentPage
    {
        public List<Cave> CavesA { get; set; }
        public string path = Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/Saved Caves.cv";
        public bool b = false;
        public Mn()
        {
            InitializeComponent();
            if (!File.Exists(path))
            {
                //Create a file to write 
                string createText = JsonConvert.SerializeObject(new List<Cave>() { });
                //Toast.MakeText(Android.App.Application.Context, createText, ToastLength.Short).Show();
                File.WriteAllText(path, createText);
            }
            CavesA = JsonConvert.DeserializeObject<List<Cave>>(File.ReadAllText(path));
            //Environment.SpecialFolder.LocalApplicationData
            this.BindingContext = this;
            b = true;
        }
        public void update()
        {
            Caves.IsRefreshing = true;
            Caves.ItemsSource = null;
            CavesA = JsonConvert.DeserializeObject<List<Cave>>(File.ReadAllText(path));
            //Environment.SpecialFolder.LocalApplicationData
            Caves.ItemsSource = CavesA;
            this.BindingContext = this;
            Caves.IsRefreshing = false;
        }
        public async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((Semka)App.Current.Properties["Semka"]).save();
            Cave c = CavesA[e.SelectedItemIndex];
            App.Current.Properties["Cave"]=c;
            Pt.Text = "Статус:Обработка";
            Semka s = JsonConvert.DeserializeObject<Semka>(File.ReadAllText(c.PathA));
            App.Current.Properties["Semka"] = s;
            Pt.Text = "Статус:ОК";
            await Shell.Current.GoToAsync("//Map");
        }
        public void Del(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync("//CaveSettings");
            SwipeItem sw = ((SwipeItem)sender);
            Cave cv = (Cave)sw.BindingContext;
            List<Cave> ls = JsonConvert.DeserializeObject<List<Cave>>(File.ReadAllText(path));
            int i = 0,ans=0;
            foreach (Cave ca in ls)
            {
                if (ca.Name == cv.Name)
                {
                    ans=i;
                }
                i++;
            }
            ls.RemoveAt(ans);
            File.Delete(cv.PathA);
            string createText = JsonConvert.SerializeObject(ls);
            File.WriteAllText(path, createText);
            update();
            //Toast.MakeText(Android.App.Application.Context, ls.Contains(cv).ToString(), ToastLength.Short).Show();
        }
        public async void Nw(object sender, EventArgs e)
        {
            Cave c = new Cave();
            App.Current.Properties["Cave"]=c;
            Semka s = new Semka();
            App.Current.Properties["Semka"] = s;
            await Shell.Current.GoToAsync("//CaveSettings");
        }

        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (b) { update(); }
        }
    }
}
