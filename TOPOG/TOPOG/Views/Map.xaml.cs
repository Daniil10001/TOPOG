using Android.Widget;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
                prd = Builder.obratn((new Builder(sm)).dfs(nach));
                Picets.ItemsSource = prd;
                this.BindingContext = this;
            }

        }

        private async void Picets_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushPopupAsync(new ToastPicet((Predst)e.Item));
        }

        private async void Create(object sender, EventArgs e)
        {
            App.Current.Properties["IC"] = false;
            await Navigation.PushPopupAsync(new ToastPicet(new Predst(new Izm(0, 0, 0), "", "", 0)));
            while (!(bool)App.Current.Properties["IC"]) await Task.Delay(100);
            Predst prd = (Predst)App.Current.Properties["Rv"];
            if (prd == null) return;
            if (!sm.pereh.ContainsKey(prd.ot)) sm.pereh[prd.ot] = new HashSet<string>();
            //if (!sm.pereh.ContainsKey(prd.to)) sm.pereh[prd.to] = new HashSet<string>();
            sm.pereh[prd.ot].Add(prd.to);
            //sm.pereh[prd.to].Add(prd.ot);
            sm.sdvig[prd.ot+"!@TOPOG@!"+prd.to] = new Izm(prd.x, prd.y, prd.z);
            sm.sdvig[prd.to+ "!@TOPOG@!" + prd.ot] = new Izm(prd.x, prd.y, prd.z);
            App.Current.Properties["Semka"] = sm;
            up();
        }

        private void TextChanged(object sender, EventArgs e)
        {
            
            sm.nach = NachP.Text;
            App.Current.Properties["Semka"] = sm;
            nach = NachP.Text;
            up();
        }

        private async void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            sm = (Semka)App.Current.Properties["Semka"];
            nach = sm.nach;
            NachP.Text = sm.nach;
            sm.save();
            up();
        }
    }
}