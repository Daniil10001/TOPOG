using Android.Widget;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOPOG.Views
{
    public partial class Abris : ContentPage
    {
        public Abris()
        {
            InitializeComponent();
        }
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 1,
            StrokeCap = SKStrokeCap.Square,
            StrokeJoin = SKStrokeJoin.Round
        };
        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();
            for (int i = 0; i < completedPaths.Count(); i++)
            {
                canvas.DrawPath(completedPaths[i], paint);
            }
            foreach (SKPath path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);
            }
        }

        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> completedPaths = new List<SKPath>();
        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath() { };
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                        Toast.MakeText(Android.App.Application.Context, "Pressed", ToastLength.Long).Show();
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        //kol++;
                        //sch.Text = kol.ToString();
                        //Отсылание сообщения с сдвигом

                        //Toast.MakeText(Android.App.Application.Context, "Moved", ToastLength.Short).Show();
                        SKPath path = inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        Toast.MakeText(Android.App.Application.Context, "Released", ToastLength.Long).Show();
                        completedPaths.Add(inProgressPaths[args.Id]);
                        //completedPathsI.Add(swt.IsToggled);
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        //Toast.MakeText(Android.App.Application.Context, "Clanceled", ToastLength.Long).Show();
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }
    }
}