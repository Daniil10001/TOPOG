using Android.Widget;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TOPOG.ToastPage;
using TouchTracking;
using Xamarin.Forms;
using static TOPOG.Views.Serialization_Magic;

namespace TOPOG.Views
{
    public partial class Abris : ContentPage
    {
        public Abris()
        {
            InitializeComponent();
        }
        SKPaint paintS = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 1,
            StrokeCap = SKStrokeCap.Square,
            StrokeJoin = SKStrokeJoin.Round
        };
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 1,
            StrokeCap = SKStrokeCap.Square,
            StrokeJoin = SKStrokeJoin.Round
        };
        SKPaint paint2 = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Blue

        };
        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            nm.Text = Name;
            try
            {
                SKCanvas canvas = args.Surface.Canvas;
                canvas.Clear();
                SKCanvas c;
                canvas.Scale(1f);

                if (count <= 1 && type != -1)
                {
                    foreach (SKPath path in inProgressPaths.Values)
                    {
                        SKPath p = new SKPath(path);
                        p.Transform(SKMatrix.CreateTranslation(-dx, -dy));
                        canvas.DrawPath(p, paint);
                    }
                }
                else
                {
                    List<int> a = new List<int>();

                }
                foreach (SKPaint pn in completedPaths.Keys)
                {
                    foreach (SKPath pth in completedPaths[pn])
                    {
                        SKPath p = new SKPath(pth);
                        p.Transform(SKMatrix.CreateTranslation(-dx, -dy));
                        canvas.DrawPath(p, paint);
                    }
                }
                foreach (Tuple<SKPaint, SKPaint> pn in completedShape.Keys)
                {
                    foreach (SKPath pth in completedShape[pn])
                    {
                        canvas.DrawPath(pth, pn.Item1);
                        canvas.DrawPath(pth, pn.Item2);
                    }
                }
                foreach (SKPaint pn in completedPoint.Keys)
                {
                    foreach (SKPoint pth in completedPoint[pn])
                    {
                        canvas.DrawPoint(pth, pn);
                    }
                }

            }
            catch
            {

            }
        }
        void upPth(int p = -1)
        {
            List<SKPath> pth = completedPaths[paint];
            SKPath pt = pth[pth.Count() - 1];
            int k = 1000;
            if (p != -1)
                pt = pth[p];
            else
                p = pth.Count() - 1;
            for (int i = 0; i < pth.Count(); i++)
            {
                if (i != p)
                {
                    SKPath pt1 = pth[i];
                    pt = pth[p];
                    if ((pt.LastPoint.X - pt1.GetPoint(0).X) * (pt.LastPoint.X - pt1.GetPoint(0).X) +
                        (pt.LastPoint.Y - pt1.GetPoint(0).Y) * (pt.LastPoint.Y - pt1.GetPoint(0).Y) <= k)
                    {
                        pth[p].LineTo(pt1.GetPoint(0));
                        completedPaths[paint] = pth;
                    }

                    if ((pt.GetPoint(0).X - pt1.LastPoint.X) * (pt.GetPoint(0).X - pt1.LastPoint.X) +
                        (pt.GetPoint(0).Y - pt1.LastPoint.Y) * (pt.GetPoint(0).Y - pt1.LastPoint.Y) <= k) //pt.GetPoint(0).X
                    {
                        //Toast.MakeText(Android.App.Application.Context, j.ToString(), ToastLength.Short).Show();
                        pth[i].LineTo(pt.GetPoint(0));
                        completedPaths[paint] = pth;
                    }

                    if ((pt.LastPoint.X - pt1.LastPoint.X) * (pt.LastPoint.X - pt1.LastPoint.X) +
                        (pt.LastPoint.Y - pt1.LastPoint.Y) * (pt.LastPoint.Y - pt1.LastPoint.Y) <= k)
                    {
                        pth[p].LineTo(pt1.LastPoint);
                        completedPaths[paint] = pth;
                    }

                    if ((pt.GetPoint(0).X - pt1.GetPoint(0).X) * (pt.GetPoint(0).X - pt1.GetPoint(0).X) +
                        (pt.GetPoint(0).Y - pt1.GetPoint(0).Y) * (pt.GetPoint(0).Y - pt1.GetPoint(0).Y) <= k)
                    {
                        SKPath pth2 = new SKPath() { };
                        pth2.MoveTo(pt1.GetPoint(0));
                        pth2.AddPath(pt);
                        pth[p] = pth2;
                        completedPaths[paint] = pth;
                    }
                }

            }
        }
        float dx = 0, dy = 0, scl = 1;
        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width + dx),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height + dy));
        }
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();

        Dictionary<SKPaint, List<SKPath>> completedPaths = new Dictionary<SKPaint, List<SKPath>>();

        Dictionary<SKPaint, List<SKPoint>> completedPoint = new Dictionary<SKPaint, List<SKPoint>>();

        Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>> completedShape = new Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>>(); //Tuple<SKPaint, SKPaint>

        string Name = "";
        int type;
        int count = 0;
        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            if (Name == "")
                return;
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath() { };
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                        count += 1;
                        if (count > 1)
                        {
                            type = -1;
                        }
                        else
                        {
                            type = 0;
                        }
                        //Toast.MakeText(Android.App.Application.Context, JsonConvert.SerializeObject(paint), ToastLength.Long).Show();
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
                        try
                        {
                            if (type == -1)
                            {
                                float sx = 0, sy = 0,rx=0,ry=0, kl = 0;
                                foreach (long pn in inProgressPaths.Keys)
                                {
                                    sx -= inProgressPaths[pn].LastPoint.X - inProgressPaths[pn].GetPoint(inProgressPaths[pn].PointCount - 10).X;
                                    sy -= inProgressPaths[pn].LastPoint.Y - inProgressPaths[pn].GetPoint(inProgressPaths[pn].PointCount - 10).Y;
                                    kl += 1;
                                }
                                dx += sx / kl / 6;
                                dy += sy / kl / 6;
                            }
                        }
                        catch { }
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        //Toast.MakeText(Android.App.Application.Context, "Released", ToastLength.Long).Show();
                        if (count == 1)
                        {
                            if (type == 0)
                            {
                                if (!completedPaths.ContainsKey(paint)) completedPaths[paint] = new List<SKPath>();
                                completedPaths[paint].Add(inProgressPaths[args.Id]);
                                try
                                {
                                    upPth();
                                }
                                finally
                                {

                                }
                            }
                            if (type == 1)
                            {
                                if (!completedShape.ContainsKey(new Tuple<SKPaint, SKPaint>(paint, paint2))) completedShape[new Tuple<SKPaint, SKPaint>(paint, paint2)] = new List<SKPath>();
                                completedShape[new Tuple<SKPaint, SKPaint>(paint, paint2)].Add(inProgressPaths[args.Id]);
                            }
                            if (type == 2)
                                if (inProgressPaths[args.Id].PointCount <= 5)
                                {
                                    if (!completedPoint.ContainsKey(paint)) completedPoint[paint] = new List<SKPoint>();
                                    completedPoint[paint].Add(inProgressPaths[args.Id].LastPoint);
                                    //Toast.MakeText(Android.App.Application.Context, inProgressPaths[args.Id].PointCount.ToString(), ToastLength.Long).Show();
                                }
                        }
                        //completedPathsI.Add(swt.IsToggled);
                        inProgressPaths.Remove(args.Id);
                        count -= 1;
                        canvasView.InvalidateSurface();

                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        count -= 1;

                        //Toast.MakeText(Android.App.Application.Context, "Clanceled", ToastLength.Long).Show();
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
            nsvd = true;
        }
        bool nsvd = false;
        private async void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Toast.MakeText(Android.App.Application.Context, Name, ToastLength.Long).Show();
            if (Name != "")
            {
                /*string createText = JsonConvert.SerializeObject(a, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string path = Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr";
                File.WriteAllText(path, createText);*/
                Toast.MakeText(Android.App.Application.Context, "I", ToastLength.Long).Show();
                if (nsvd)
                {

                    nsvd = false;
                    abri a = new abri(Name, completedPaths, completedPoint, completedShape);
                    Toast.MakeText(Android.App.Application.Context, "Clanceled2", ToastLength.Long).Show();
                    string path = Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr";
                    Serial.Save(abris.crt(a), typeof(abris), path);
                }
            }
            if (((Semka)App.Current.Properties["Semka"]).Name == "")
                return;
            else
            {
                if (Name == "" && (bool)App.Current.Properties["IC"])
                {
                    App.Current.Properties["IC"] =false;
                    //await Navigation.PopAllPopupAsync();
                    await Navigation.PushPopupAsync(new Vibor());
                    while (!(bool)App.Current.Properties["IC"])
                        await Task.Delay(100);
                    Name = (string)App.Current.Properties["Nm"];
                    abri a = new abri(Name);
                    if (File.Exists(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr"))
                        a = abri.getabr(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr");
                    completedPaths = a.Paths;
                    completedPoint = a.Point;
                    completedShape = a.Shape;
                }
            }
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (((Semka)App.Current.Properties["Semka"]).Name=="")
                return;
            //Toast.MakeText(Android.App.Application.Context, Name, ToastLength.Long).Show();
            if (Name != "")
            {
                nsvd = false;
                abri ab = new abri(Name,completedPaths,completedPoint,completedShape);
                string path = Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr";
                Serial.Save(abris.crt(ab), typeof(abris), path);
            }
            dx = 0;dy = 0;scl = 1;
            App.Current.Properties["IC"] = false;
            await Navigation.PushPopupAsync(new Vibor());
            while (!(bool)App.Current.Properties["IC"])
                await Task.Delay(100);
            Name = (string)App.Current.Properties["Nm"];
            abri a = new abri(Name);
            if (File.Exists(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr"))
            { a = abri.getabr(Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr");}
           
            completedPaths = a.Paths;
            //Toast.MakeText(Android.App.Application.Context, completedPaths.Count.ToString(), ToastLength.Long).Show();
            //Toast.MakeText(Android.App.Application.Context, a.Paths.Count.ToString(), ToastLength.Long).Show();
            completedPoint =a.Point;
            completedShape=a.Shape;
            
            Toast.MakeText(Android.App.Application.Context, "Clanceled", ToastLength.Long).Show();
            canvasView.InvalidateSurface();
        }
    }
}