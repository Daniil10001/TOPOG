using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Android.Widget;

namespace TOPOG.Views
{ 

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mn : ContentPage
    {  
        public List<Cave> CavesA { get; set; }
        public string path = Serial.path;
        public bool b = false; 
        public Mn()
        { 
            InitializeComponent(); 
            if (!File.Exists(path)) 
            {  
                //Create a file to write  
                /*string createText = JsonConvert.SerializeObject(new List<Cave>() { });
                
                File.WriteAllText(path, createText);*/
                //Toast.MakeText(Android.App.Application.Context, "1", ToastLength.Short).Show();
                Serial.Save(new List<Cave>() { }, typeof(List<Cave>), path);
                //Toast.MakeText(Android.App.Application.Context, "2", ToastLength.Short).Show();
            } 
            CavesA = (List<Cave>)Serial.Open(typeof(List<Cave>), path);//JsonConvert.DeserializeObject<List<Cave>>(File.ReadAllText(path));
            //Environment.SpecialFolder.LocalApplicationData
            this.BindingContext = this;
            b = true;
        }
        public void update()
        {
            Caves.IsRefreshing = true;
            Caves.ItemsSource = null;
            CavesA = (List<Cave>)Serial.Open(typeof(List<Cave>), path);//JsonConvert.DeserializeObject<List<Cave>>(File.ReadAllText(path));
            //Environment.SpecialFolder.LocalApplicationData
            Caves.ItemsSource = CavesA;
            this.BindingContext = this;
            Caves.IsRefreshing = false;
        }
        public async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Toast.MakeText(Android.App.Application.Context, "1", ToastLength.Long).Show();
            ((Semka)App.Current.Properties["Semka"]).save();
            //Toast.MakeText(Android.App.Application.Context, "12", ToastLength.Long).Show();
            Cave c = CavesA[e.SelectedItemIndex];
           
            App.Current.Properties["Cave"]=c;
            Pt.Text = "Статус:Обработка";
            Semka s = (Semka)Serial.Open(typeof(Semka), c.PathA);//JsonConvert.DeserializeObject<Semka>(File.ReadAllText());
            App.Current.Properties["Semka"] = s;
            App.Current.Properties["Nm"] = "";
            Pt.Text = "Статус:ОК";
            
            await Shell.Current.GoToAsync("//Map");
        }
        public void Del(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync("//CaveSettings");
            SwipeItem sw = ((SwipeItem)sender);
            Cave cv = (Cave)sw.BindingContext;
            List<Cave> ls = (List<Cave>)Serial.Open(typeof(List<Cave>), path);// JsonConvert.DeserializeObject<List<Cave>>(File.ReadAllText(path));
            int i = 0,ans=0;
            foreach (Cave ca in ls)
            {
                if (ca.Name == cv.Name)
                {
                    ans=i;
                }
                i++;
            }
            Semka sm = ((Semka)Serial.Open(typeof(Semka), cv.PathA));
            foreach (Tuple<string,string> st in sm.abrisy)
            {
                if (File.Exists(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + sm.Name + @"\\" + st.Item1 + ".abr"))
                    File.Delete(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + sm.Name + @"\\" + st.Item1 + ".abr");
                if (File.Exists(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + sm.Name + @"\\" + st.Item2 + ".abr"))
                    File.Delete(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + sm.Name + @"\\" + st.Item2 + ".abr");
            }
            ls.RemoveAt(ans);
            File.Delete(cv.PathA);
            /*string createText = JsonConvert.SerializeObject(ls);
            File.WriteAllText(path, createText); */
            Serial.Save(ls, typeof(List<Cave>), path);
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
