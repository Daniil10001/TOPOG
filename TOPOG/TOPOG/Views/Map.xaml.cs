using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TOPOG.ToastPage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOPOG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        public List<Predst> prd { get; set; }
        public string nach { get; set; } 
        public Semka sm { get; set; }
        public Map()
        {
            InitializeComponent();
            this.BindingContext = this;
            nach = ((Semka)App.Current.Properties["Semka"]).nach;
            sm = (Semka)App.Current.Properties["Semka"];            
            //Builder.obratn((new Builder(sm)).dfs(" "));
        }
        
        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {

        }

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {

        }

        //private async void Button_Clicked(object sender, EventArgs e)
        //{
            //await PopupNavigation.PushAsync(new ToastPicet());
        //} 
        public void up()
        {
            //Prn.Text = JsonConvert.SerializeObject(sm);
            if (sm.pereh.ContainsKey(nach))
            {
                //Toast.MakeText(Android.App.Application.Context, "ch", ToastLength.Long).Show();
                Picets.ItemsSource = null;
                
                if (Srt.IsToggled) prd = Builder.obratn((new Builder(sm)).dfs(nach));
                else prd = Builder.obratn1((new Builder(sm)).dfs(nach));
                Picets.ItemsSource = prd;
                this.BindingContext = this;
            }
             
        }

        private async void Picets_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            App.Current.Properties["IC"] = false;
            await Navigation.PushPopupAsync(new ToastPicet((Predst)e.Item));
            while (!(bool)App.Current.Properties["IC"]) await Task.Delay(100);
            Predst prd = (Predst)App.Current.Properties["Rv"], pr = (Predst)e.Item;
            if (prd == null) return;
            sm.sdvig.Remove(pr.ot + "!@TOPOG@!" + pr.to);
            sm.pereh[pr.ot].Remove(pr.to);
            if (!sm.pereh.ContainsKey(prd.ot)) sm.pereh[prd.ot] = new HashSet<string>();
            sm.pereh[prd.ot].Add(prd.to);
            sm.sdvig[prd.ot + "!@TOPOG@!" + prd.to] = new Izm(prd.x, prd.y, prd.z);
            App.Current.Properties["Semka"] = sm;
            up();

        }

        private async void Create(object sender, EventArgs e)
        {
            App.Current.Properties["IC"] = false;
            await Navigation.PushPopupAsync(new ToastPicet(new Predst(new Izm(0, 0, 0), "", "", 0)));
            while (!(bool)App.Current.Properties["IC"]) await Task.Delay(100);
            Predst prd = (Predst)App.Current.Properties["Rv"];
            if (prd == null) return;
            if (!sm.pereh.ContainsKey(prd.ot)) sm.pereh[prd.ot] = new HashSet<string>();
            sm.pereh[prd.ot].Add(prd.to);
            sm.sdvig[prd.ot+"!@TOPOG@!"+prd.to] = new Izm(prd.x, prd.y, prd.z);
            App.Current.Properties["Semka"] = sm;
            up();
        }

        private void TextChanged(object sender, EventArgs e)
        {
            
            sm.nach = NachP.Text.Replace(" ","");
            App.Current.Properties["Semka"] = sm;
            nach = NachP.Text.Replace(" ","");
            up();
        }

        private async void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            sm = (Semka)App.Current.Properties["Semka"];
            nach = sm.nach;
            NachP.Text = sm.nach;
            //Toast.MakeText(Android.App.Application.Context, "123", ToastLength.Long).Show();
            sm.save();
            up();
        }

        private void Srt_Toggled(object sender, ToggledEventArgs e)
        {
            if (Srt.IsToggled) SrtT.Text = "По нитке";
            else SrtT.Text = "По пикетам";
            if (sm!=null)
            up();
        }

        private async void Splei(object sender, EventArgs e)
        {

            SwipeItem sw = ((SwipeItem)sender);
            Predst pr = (Predst)sw.BindingContext;
            App.Current.Properties["IC"] = false;
            await Navigation.PushPopupAsync(new SplView(pr.to));
            while (!(bool)App.Current.Properties["IC"]) await Task.Delay(100);
            sm = (Semka)App.Current.Properties["Semka"];

        }
        private void Del(object sender, EventArgs e)
        {
            SwipeItem sw = ((SwipeItem)sender);
            Predst pr = (Predst)sw.BindingContext;
            sm.sdvig.Remove(pr.ot+ "!@TOPOG@!" + pr.to);
            sm.pereh[pr.ot].Remove(pr.to);
            App.Current.Properties["Semka"] = sm;
            up();
            //Toast.MakeText(Android.App.Application.Context, sw.BindingContext.GetType().Name, ToastLength.Long).Show();
        }

        private async void Splplus(object sender, EventArgs e)
        {
            App.Current.Properties["IC"] = false;
            await Navigation.PushPopupAsync(new SplView(""));
            while (!(bool)App.Current.Properties["IC"]) await Task.Delay(100);
            sm = (Semka)App.Current.Properties["Semka"];
        }

        private void Pereh(object sender, EventArgs e)
        {
            App.Current.Properties["IC"] = false;
        }
    }
}