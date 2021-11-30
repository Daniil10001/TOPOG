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
    public partial class Priem : PopupPage
    {
        public Priem()
        {
            InitializeComponent();
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
        List<long> posl = new List<long>(); 
        Dictionary<long, Tuple<Izm,Izm>> inProgress = new Dictionary<long, Tuple<Izm, Izm>>();
        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgress.ContainsKey(args.Id))
                    {
                        TouchTrackingPoint pt = args.Location;
                        inProgress.Add(args.Id, new Tuple<Izm,Izm>(new Izm(pt.X,pt.Y,0), new Izm(0,0,0)));
                        posl.Add(args.Id);
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgress.ContainsKey(args.Id)) 
                    {
                        TouchTrackingPoint pt = args.Location;
                        Tuple<Izm, Izm> pereh = inProgress[args.Id];
                        inProgress[args.Id]=new Tuple<Izm,Izm>(pereh.Item1,new Izm(pt.X,pt.Y,0));
                        // Отсылание сообщения с сдвигом
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgress.ContainsKey(args.Id))
                    {
                        inProgress.Remove(args.Id);
                        posl.Remove(args.Id);
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgress.ContainsKey(args.Id))
                    {
                        inProgress.Remove(args.Id);
                        posl.Remove(args.Id);
                    }
                    break;
            }
        }

    }
}