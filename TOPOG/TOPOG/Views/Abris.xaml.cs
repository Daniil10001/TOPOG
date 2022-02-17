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
        float dx = 0, dy = 0, scl = 1;
        SKPath nitka = new SKPath();
        List<Tuple<SKPoint, string>> picts = new List<Tuple<SKPoint, string>>();
        public Abris()
        {
            InitializeComponent();
            typel.SelectedIndex = 0;
        }
        SKPaint paintS = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Square,
            StrokeJoin = SKStrokeJoin.Round
        };
        SKPaint paintT = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            Color = SKColors.Pink,
            StrokeWidth = 1
        };
        /*SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Square,
            StrokeJoin = SKStrokeJoin.Round
        }*/
        SKPaint paint2 = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Blue

        };
        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            
            try
            {
                SKCanvas canvas = args.Surface.Canvas;
                canvas.Clear();
                //SKCanvas c;
                //SKPaint paintC = new SKPaint() { Color = SKColors.White, StrokeWidth=1 };
                //SKPath rc = new SKPath() { }
                //rc.AddRect(SKRect.Create(50,50));
                float sh = 50*Math.Max(1,2*(int)(1/scl/2));
                nm.Text = "Название: " + Name + " Приближение:" + (Math.Round(scl, 2)).ToString() + " Клетка: "+((int)(sh/50)).ToString()+"м";
                SKPathEffect ef = 
                   SKPathEffect.CreateSum(SKPathEffect.Create2DLine(1, SKMatrix.CreateScaleTranslation(1,sh*scl,0,(dy+sh/2)*scl)),
                   SKPathEffect.Create2DLine(1, SKMatrix.Concat(SKMatrix.CreateScaleTranslation(sh*scl, 1,(dx+sh/2)*scl,0), SKMatrix.CreateRotationDegrees(90))));//SKPathEffect.Create2DPath(SKMatrix.CreateScaleTranslation(50,50,50,50),paintC.GetFillPath(rc));
                canvas.DrawRect(0,0,canvasView.CanvasSize.Width,canvasView.CanvasSize.Height,
                    new SKPaint() {Style=SKPaintStyle.Fill, Color = SKColors.White, StrokeWidth = 1, PathEffect = ef });
                canvas.Scale(scl);
                if (count <= 1 && type != -1)
                {
                    foreach (SKPath path in inProgressPaths.Values)
                    {
                        
                        SKPath p = new SKPath(path);
                        p.Transform(SKMatrix.CreateTranslation(dx,dy));
                        canvas.DrawPath(p, (((Tuple<int, CPaintL>)App.Current.Properties["Paint"]).Item2).getc());
                        
                    }
                }
                foreach (CPaintL pnf in completedPaths.Keys)
                {
                    SKPaint pn = pnf.getc();
                    foreach (SKPath pth in completedPaths[pnf])
                    {
                        pn.StrokeWidth = Math.Max(1, pn.StrokeWidth / scl);
                        SKPath p = new SKPath(pth);
                        p.Transform(SKMatrix.CreateTranslation(dx, dy));
                        canvas.DrawPath(p, pn);
                        pn.StrokeWidth = 2;
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
                SKPath pth2 = new SKPath(nitka);
                paintS.StrokeWidth = Math.Max(1, paintS.StrokeWidth / scl);
                pth2.Transform(SKMatrix.CreateTranslation(dx, dy));
                canvas.DrawPath(pth2, paintS);
                paintT.TextSize = 50 / scl;
                foreach (Tuple<SKPoint, string> pic in picts)
                {
                    paintT.MeasureText(pic.Item2);
                    canvas.DrawText(pic.Item2, new SKPoint(dx + pic.Item1.X+10/scl, dy + pic.Item1.Y-10/scl), paintT);
                }
                paintS.StrokeWidth = 2;
            }
            catch
            {

            }
        }
        void upPth(int p = -1)
        {
            CPaintL pnt = ((Tuple<int, CPaintL>)App.Current.Properties["Paint"]).Item2;
            List<SKPath> pth = completedPaths[pnt];
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
                        (pt.LastPoint.Y - pt1.GetPoint(0).Y) * (pt.LastPoint.Y - pt1.GetPoint(0).Y) <= k 
                        && (pt.LastPoint.X - pt1.GetPoint(0).X) * (pt.LastPoint.X - pt1.GetPoint(0).X) +
                        (pt.LastPoint.Y - pt1.GetPoint(0).Y) * (pt.LastPoint.Y - pt1.GetPoint(0).Y)>=0)
                    {
                        pth[p].LineTo(pt1.GetPoint(0));
                        completedPaths[pnt] = pth;
                    }

                    if ((pt.GetPoint(0).X - pt1.LastPoint.X) * (pt.GetPoint(0).X - pt1.LastPoint.X) +
                        (pt.GetPoint(0).Y - pt1.LastPoint.Y) * (pt.GetPoint(0).Y - pt1.LastPoint.Y) <= k
                        && (pt.GetPoint(0).X - pt1.LastPoint.X) * (pt.GetPoint(0).X - pt1.LastPoint.X) +
                        (pt.GetPoint(0).Y - pt1.LastPoint.Y) * (pt.GetPoint(0).Y - pt1.LastPoint.Y) >=0)
                    {
                        //Toast.MakeText(Android.App.Application.Context, j.ToString(), ToastLength.Short).Show();
                        pth[i].LineTo(pt.GetPoint(0));
                        completedPaths[pnt] = pth;
                    }

                    if ((pt.LastPoint.X - pt1.LastPoint.X) * (pt.LastPoint.X - pt1.LastPoint.X) +
                        (pt.LastPoint.Y - pt1.LastPoint.Y) * (pt.LastPoint.Y - pt1.LastPoint.Y) <= k
                        && (pt.LastPoint.X - pt1.LastPoint.X) * (pt.LastPoint.X - pt1.LastPoint.X) +
                        (pt.LastPoint.Y - pt1.LastPoint.Y) * (pt.LastPoint.Y - pt1.LastPoint.Y) >= 0)
                    {
                        pth[p].LineTo(pt1.LastPoint);
                        completedPaths[pnt] = pth;
                    }

                    if ((pt.GetPoint(0).X - pt1.GetPoint(0).X) * (pt.GetPoint(0).X - pt1.GetPoint(0).X) +
                        (pt.GetPoint(0).Y - pt1.GetPoint(0).Y) * (pt.GetPoint(0).Y - pt1.GetPoint(0).Y) <= k
                        && (pt.GetPoint(0).X - pt1.GetPoint(0).X) * (pt.GetPoint(0).X - pt1.GetPoint(0).X) +
                        (pt.GetPoint(0).Y - pt1.GetPoint(0).Y) * (pt.GetPoint(0).Y - pt1.GetPoint(0).Y) >=0)
                    {
                        SKPath pth2 = new SKPath() { };
                        pth2.MoveTo(pt1.GetPoint(0));
                        pth2.AddPath(pt);
                        pth[p] = pth2;
                        completedPaths[pnt] = pth;
                    }
                }

            }
        }
        
        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width /scl - dx),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height /scl - dy));
        }
        SKPoint ConvertToPixel2(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        Dictionary<long, SKPath> inProgressPathScl = new Dictionary<long, SKPath>();

        Dictionary<CPaintL, List<SKPath>> completedPaths = new Dictionary<CPaintL, List<SKPath>>();

        Dictionary<SKPaint, List<SKPoint>> completedPoint = new Dictionary<SKPaint, List<SKPoint>>();

        Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>> completedShape = new Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>>(); //Tuple<SKPaint, SKPaint>

        List<Tuple<int, object>> pred = new List<Tuple<int, object>>(), sled = new List<Tuple<int, object>>();
        string Name = "";
        int type;
        int count = 0;
        private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
        {

            CPaintL pnt = ((Tuple<int, CPaintL>)App.Current.Properties["Paint"]).Item2;
            if (Name == "")
                return;
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path2 = new SKPath() { };
                        path2.MoveTo(ConvertToPixel2(args.Location));
                        inProgressPathScl.Add(args.Id, path2);
                        SKPath path = new SKPath() { };
                        path.MoveTo(ConvertToPixel(args.Location));
                        if (!inProgressPaths.ContainsKey(args.Id))
                        {
                            inProgressPaths.Add(args.Id, path);
                            
                            count += 1;
                        }
                        if (count > 1)
                        {
                            type = -1;
                        }
                        else
                        {
                            type = typel.SelectedIndex-1;
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
                        SKPath path2 = inProgressPathScl[args.Id];
                        path2.LineTo(ConvertToPixel2(args.Location));
                        canvasView.InvalidateSurface();
                        try
                        {
                            if (type ==-1)
                            {
                                int dp = 2;
                                float sx = 0, sy = 0, z1x = 0, z1y = 0, z2x = 0, z2y = 0, r1 = 0, r2 = 0, kl = 0;
                                foreach (long pn in inProgressPaths.Keys)
                                {
                                    z1y += inProgressPathScl[pn].GetPoint(inProgressPathScl[pn].PointCount - dp).Y;
                                    z1x += inProgressPathScl[pn].GetPoint(inProgressPathScl[pn].PointCount - dp).X;
                                    z2y += inProgressPathScl[pn].LastPoint.Y;
                                    z2x += inProgressPathScl[pn].LastPoint.X;
                                    sx += inProgressPathScl[pn].LastPoint.X - inProgressPathScl[pn].GetPoint(inProgressPathScl[pn].PointCount - dp).X;
                                    sy += inProgressPathScl[pn].LastPoint.Y - inProgressPathScl[pn].GetPoint(inProgressPathScl[pn].PointCount - dp).Y;
                                    kl += 1;
                                }
                                float ds = 1;
                                if (count > 1) 
                                {
                                    z1y /= kl; z1x /= kl; z2x /= kl; z2y /= kl;
                                    foreach (long pn in inProgressPaths.Keys)
                                    {
                                        r2 += (float)Math.Sqrt(Math.Pow((double)(inProgressPathScl[pn].LastPoint.Y - z2y), 2d) + Math.Pow((double)(inProgressPathScl[pn].LastPoint.X - z2x), 2));
                                        r1 += (float)Math.Sqrt(Math.Pow((double)(inProgressPathScl[pn].GetPoint(inProgressPathScl[pn].PointCount - dp).Y - z1y), 2d)
                                            + Math.Pow((double)(inProgressPathScl[pn].GetPoint(inProgressPathScl[pn].PointCount - dp).X - z1x), 2));
                                    }
                                    ds = (float)Math.Round(Math.Max(0.99f, Math.Min(1.01, r2 / r1)), 2);
                                }
                                dx += sx/kl/scl;
                                dy += sy/kl/scl;
                                
                                if (scl < 15 || ds<1)
                                {
                                    scl *= ds;
                                    dx = dx / ds;// + (canvasView.CanvasSize.Width / scl - dx)/ds/2;
                                    //dx -=(canvasView.CanvasSize.Width / scl - dx);
                                    dy = dy / ds;// + (canvasView.CanvasSize.Height / scl - dx)/ds/2;
                                    //dy -= (canvasView.CanvasSize.Height / scl - dy);
                                }
                               // foreach (long pn in inProgressPaths.Keys)
                                //{
                               //     inProgressPaths[pn].Transform(SKMatrix.CreateScale());
                               // }
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
                                if (!completedPaths.ContainsKey(pnt)) completedPaths[pnt] = new List<SKPath>();
                                { completedPaths[pnt].Add(inProgressPaths[args.Id]);
                                    pred.Add(new Tuple<int, object>(1,pnt));
                                }
                                try
                                {
                                    upPth();
                                }
                                finally
                                {

                                }
                            }
                            /*if (type == 1)
                            {
                                if (!completedShape.ContainsKey(new Tuple<SKPaint, SKPaint>(pnt, paint2))) completedShape[new Tuple<SKPaint, SKPaint>(pnt, paint2)] = new List<SKPath>();
                                { completedShape[new Tuple<SKPaint, SKPaint>(pnt, paint2)].Add(inProgressPaths[args.Id]); pred.Add(new Tuple<int, object>(2,paintS)); }
                            }
                            if (type == 2)
                                if (inProgressPaths[args.Id].PointCount <= 5)
                                {
                                    if (!completedPoint.ContainsKey(pnt)) completedPoint[pnt] = new List<SKPoint>();
                                    { completedPoint[pnt].Add(inProgressPaths[args.Id].LastPoint); pred.Add(new Tuple<int, object>(3, paint2)); }
                                    //Toast.MakeText(Android.App.Application.Context, inProgressPaths[args.Id].PointCount.ToString(), ToastLength.Long).Show();
                                }*/
                        }
                        //completedPathsI.Add(swt.IsToggled);
                        inProgressPaths.Remove(args.Id);
                        inProgressPathScl.Remove(args.Id);
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
                        inProgressPathScl.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
            nsvd = true;
        }

        private void Nazad(object sender, EventArgs e)
        {
            CPaintL pnt = ((Tuple<int, CPaintL>)App.Current.Properties["Paint"]).Item2;
            if (pred.Count>0)
            {
                Tuple<int, object> ob = pred[pred.Count - 1];
                if (ob.Item1 == 1)
                    if (completedPaths[(CPaintL)ob.Item2].Count > 0)
                    {
                        sled.Add(new Tuple<int, object>(ob.Item1, new Tuple<CPaintL, SKPath>(pnt, completedPaths[(CPaintL)ob.Item2][completedPaths[(CPaintL)ob.Item2].Count - 1])));
                        completedPaths[(CPaintL)ob.Item2].RemoveAt(completedPaths[(CPaintL)ob.Item2].Count - 1);
                    }
                pred.Remove(ob);
            }
            canvasView.InvalidateSurface();
        }

        private void Vpered(object sender, EventArgs e)
        {
            if (sled.Count>0)
            {
                Tuple<int, object> ob = sled[sled.Count - 1];
                if (ob.Item1==1)
                {
                    pred.Add(new Tuple<int, object>(ob.Item1, ((Tuple<CPaintL, SKPath>)ob.Item2).Item1));
                    completedPaths[((Tuple<CPaintL, SKPath>)ob.Item2).Item1].Add(((Tuple<CPaintL, SKPath>)ob.Item2).Item2);
                }
                sled.Remove(ob);
            }
            canvasView.InvalidateSurface();
        }

        bool nsvd = false;
        private async void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Toast.MakeText(Android.App.Application.Context, Name, ToastLength.Long).Show();
            if (Name != "")
            {
                /*string createText = JsonConvert.SerializeObject(a, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string path = Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + ((Semka)App.Current.Properties["Semka"]).Name + @"\\" + Name + ".abr";
                File.WriteAllText(path, createText);*/
                //Toast.MakeText(Android.App.Application.Context, "I", ToastLength.Long).Show();
                if (nsvd)
                {
                    nsvd = false;
                    abri a = new abri(Name, completedPaths, completedPoint, completedShape);
                    //Toast.MakeText(Android.App.Application.Context, "Clanceled2", ToastLength.Long).Show();
                    Serial.Save(abris.crt(a), typeof(abris), Serial.ptha(
                        ((Semka)App.Current.Properties["Semka"]).Name,Name));
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
                    if (File.Exists(Serial.ptha(((Semka)App.Current.Properties["Semka"]).Name,Name)))
                        a = abri.getabr(Name);
                    completedPaths = a.Paths;
                    completedPoint = a.Point;
                    completedShape = a.Shape;
                    dx = canvasView.CanvasSize.Width / 2;
                    dy = canvasView.CanvasSize.Height / 2;
                    
                }
                object b = App.Current.Properties["Poloj"];
                if (b != null)
                {
                    picts = new List<Tuple<SKPoint, string>>();
                    nitka = new SKPath();
                    bool b1 = (bool)b;
                    Builder.BuildPathH(new Builder(((Semka)App.Current.Properties["Semka"])).dfs("1"), ref nitka, ref picts, ref b1);
                }
                pred.Clear();
                sled.Clear();
                canvasView.InvalidateSurface();
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
            scl = 1;
            dx = canvasView.CanvasSize.Width / 2;
            dy = canvasView.CanvasSize.Height / 2;
            App.Current.Properties["IC"] = false;
            await Navigation.PushPopupAsync(new Vibor());
            while (!(bool)App.Current.Properties["IC"])
                await Task.Delay(100);
            Name = (string)App.Current.Properties["Nm"];
            abri a = new abri(Name);
            if (File.Exists(Serial.ptha(((Semka)App.Current.Properties["Semka"]).Name,Name)))
            { a = abri.getabr(Name);}
           
            completedPaths = a.Paths;
            //Toast.MakeText(Android.App.Application.Context, completedPaths.Count.ToString(), ToastLength.Long).Show();
            //Toast.MakeText(Android.App.Application.Context, a.Paths.Count.ToString(), ToastLength.Long).Show();
            completedPoint =a.Point;
            completedShape=a.Shape;
            object b = App.Current.Properties["Poloj"];
            if (b != null)
            {
                picts = new List<Tuple<SKPoint, string>>();
                nitka = new SKPath();
                bool b1 = (bool)b;
                Builder.BuildPathH(new Builder(((Semka)App.Current.Properties["Semka"])).dfs("1"), ref nitka, ref picts, ref b1);
            }
            pred.Clear();
            sled.Clear();
            //Toast.MakeText(Android.App.Application.Context, "Clanceled", ToastLength.Long).Show();
            canvasView.InvalidateSurface();
        }
    }
}