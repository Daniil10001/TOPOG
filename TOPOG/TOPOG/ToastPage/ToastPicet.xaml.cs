using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Threading.Tasks;
using TOPOG.Views;
using Xamarin.Forms;

namespace TOPOG.ToastPage
{
    public partial class ToastPicet : PopupPage
    {
        private int k { get; set; }
        public ToastPicet(Predst pr)
        {
            InitializeComponent();
            xo.Text = pr.x.ToString();
            yo.Text = pr.y.ToString();
            zo.Text = pr.z.ToString();
            Ot.Text = pr.ot;
            To.Text = pr.to;
            k = pr.otstup.Length;
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

        private async void Close(object sender, EventArgs e)
        {
            App.Current.Properties["Rv"] = new Predst(new Izm(Convert.ToDouble(xo), Convert.ToDouble(yo), Convert.ToDouble(zo)),Ot.Text,To.Text,k);
            App.Current.Properties["IC"] = true;
            await Navigation.PopPopupAsync();
        }

        private async void Save(object sender, EventArgs e)
        {
            App.Current.Properties["Rv"] = null;
            App.Current.Properties["IC"] = true;
            await Navigation.PopPopupAsync();
        }
    }
}