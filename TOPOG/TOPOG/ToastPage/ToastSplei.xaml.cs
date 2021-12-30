using Android.Widget;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;  
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPOG.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOPOG.ToastPage
{ 
    public class BoolToObjectConverter<T> : IValueConverter
    { 
        public T TrueObject { set; get; }

        public T FalseObject { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            //Toast.MakeText(Android.App.Application.Context, value.ToString(), ToastLength.Long).Show();
            //Toast.MakeText(Android.App.Application.Context, value.GetType().Name, ToastLength.Long).Show();
            return (bool)value ? TrueObject : FalseObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((T)value).Equals(TrueObject);
        }
    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplView : PopupPage
    {
        public string nm { get; set; } 
        public SplView(string str)
        {
            InitializeComponent();
            nm = str;
            izm = new Izm(0, 0, 0);
            Semka sm = (Semka)App.Current.Properties["Semka"];
            pic.Text = str;
            if (sm!=null)
            {
                if (sm.splei.ContainsKey(str))
                {
                    spl = sm.splei[str];
                    
                }
                else
                {
                    spl = new Spley();
                }
            }
            up();
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

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            //Toast.MakeText(Android.App.Application.Context, sender.GetType().Name, ToastLength.Long).Show();
        }
        public Izm izm { get; set; }
        public Spley spl { get; set; }
        public List<Ez> splei { get; set; }
        private void Del(object sender, EventArgs e)
        {
            SwipeItem sw = ((SwipeItem)sender);
            Ez pr = (Ez)sw.BindingContext;
            spl.spl.Remove(pr);
            up();
            //Toast.MakeText(Android.App.Application.Context, sw.BindingContext.GetType().Name, ToastLength.Long).Show();
        }

        private void pic_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = pic.Text;
            if (nm != "")
            {
                pic.Text = nm;
                return;
            }
            str = str.Replace(" ","");
            //Toast.MakeText(Android.App.Application.Context, "dlsl;sff", ToastLength.Long).Show();
            Semka sm = (Semka)App.Current.Properties["Semka"];
            if (sm != null)
            {
                if (sm.splei.ContainsKey(str))
                {
                    spl = sm.splei[str];
                }
                else
                {
                    spl = new Spley();
                }
            }
            up();
        }
        private async void Save(object sender, EventArgs e)
        {
            Semka sm = ((Semka)App.Current.Properties["Semka"]);
            sm.splei[pic.Text] = spl;
            App.Current.Properties["Semka"]=sm;
            App.Current.Properties["IC"] = true;
            await Navigation.PopPopupAsync();
        }
        private async void Close(object sender, EventArgs e)
        {
            App.Current.Properties["IC"] = true;
            await Navigation.PopPopupAsync();
        }
        private void Sdvig(object sender, EventArgs e)
        {
            //....
            Izm iz = new Izm(0, 0, 0);//Заменить
            xo.Text = iz.x.ToString();
            yo.Text = iz.z.ToString();
            zo.Text = iz.y.ToString();
            izm = iz;
        }

        private void New_Spl(object sender, EventArgs e)
        {
            //....
            Ez ez = new Ez(0+izm.x, 0+izm.y, 0+izm.z, false);
            spl.spl.Add(ez);
            up();
        }
        private void up()
        {
            splei = new List<Ez>(spl.spl);
            otspl.ItemsSource = null;
            otspl.ItemsSource = splei;
            this.BindingContext = this;
        }
    }
}