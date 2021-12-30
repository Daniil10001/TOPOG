using Android.Widget;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;
using TOPOG.Views;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOPOG.ToastPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Vibor : PopupPage 
    {
        public List<Tuple<string,string>> lst { get; set; }
        public Vibor()
        {
            InitializeComponent();
            lst=((Semka)App.Current.Properties["Semka"]).abrisy;
            ls.ItemsSource = lst;
            this.BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        // Method for animation child in PopupPage
        // Invoced after custom animation end
        protected virtual Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected virtual Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);
        }
        
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Button sw = ((Xamarin.Forms.Button)sender);
            string pr = (string)sw.Text;
            //Toast.MakeText(Android.App.Application.Context, pr, ToastLength.Short).Show();
            App.Current.Properties["Nm"] = pr;
            App.Current.Properties["Poloj"] = true;
            App.Current.Properties["IC"] = true;
            await Navigation.PopAllPopupAsync();
        }
        private async void Button_Clicked1(object sender, EventArgs e)
        {
            Xamarin.Forms.Button sw = ((Xamarin.Forms.Button)sender);
            string pr = (string)sw.Text;
            //Toast.MakeText(Android.App.Application.Context, pr, ToastLength.Short).Show();
            App.Current.Properties["Nm"] = pr;
            App.Current.Properties["Poloj"] = false;
            App.Current.Properties["IC"] = true;
            await Navigation.PopAllPopupAsync();
        }
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (fst.Text!="" && scd.Text!="" && scd.Text!=fst.Text)
            {
                ((Semka)App.Current.Properties["Semka"]).abrisy.Add(new Tuple<string, string>(fst.Text, scd.Text));
                ls.ItemsSource = null;
                lst = ((Semka)App.Current.Properties["Semka"]).abrisy;
                ls.ItemsSource = lst;
                this.BindingContext = this; 
            }
        }
    }
}