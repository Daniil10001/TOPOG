using Android.Widget;
using Rg.Plugins.Popup.Extensions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
            else
            {
                Picets.ItemsSource = null;
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
            if (sm != null)
            {
                sm.save();
                sm = (Semka)App.Current.Properties["Semka"];
                nach = sm.nach;
                NachP.Text = sm.nach;
                //Toast.MakeText(Android.App.Application.Context, "123", ToastLength.Long).Show();
                up();
            }
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
            await Navigation.PushPopupAsync(new SplView(pr.ot));
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
        private void comp(Dictionary<SKPaint, List<SKPath>> Paths, string type)
        {
            var metadata = new SKDocumentPdfMetadata
            {
                Author = "Cool Person",
                Creation = DateTime.Now,
                Creator = "YOU",
                Keywords = "PDF,map,TOPOG",
                Modified = DateTime.Now,
                Producer = "TOPOG",
                Subject = "PDF",
                Title = "Toposemka",
            };
            string pathd = Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + sm.Name + type + ".pdf";
            if (File.Exists(pathd))
                File.Delete(pathd);
            using (SKDocument document = SKDocument.CreatePdf(pathd, metadata))
            { 
                float pW = 840;
                float pH = 1188;
                SKPaint paint = new SKPaint() { Color = SKColors.Black, StrokeWidth = 1, Style = SKPaintStyle.Stroke };
                SKPaint paintT = new SKPaint() { Color = SKColors.Black, StrokeWidth = 1, Style = SKPaintStyle.Stroke };
                int k = (Paths.Keys.Count + 2) / 3;
                float x = 45, y = pH - 10, w = 40, h = 20, dy = 30;
                float mny = float.MaxValue, mxy = -float.MaxValue, mnx = float.MaxValue, mxx = -float.MaxValue;
                foreach (SKPaint pt in Paths.Keys)
                    foreach (SKPath pth in Paths[pt])
                        //Toast.MakeText(Android.App.Application.Context, Math.Max(Math.Abs(mxx), Math.Abs(mnx)).ToString(), ToastLength.Short).Show();
                        foreach (SKPoint point in pth.Points)
                        {
                            if (point.X > mxx) mxx = point.X;
                            if (point.X < mnx) mnx = point.X;
                            if (point.Y > mxy) mxy = point.Y;
                            if (point.Y < mny) mny = point.Y;
                        } 
                float mx=2*Math.Max(Math.Max(Math.Abs(mxx), Math.Abs(mnx)), Math.Max(Math.Abs(mxx), Math.Abs(mnx)));
                Toast.MakeText(Android.App.Application.Context, Math.Max(Math.Abs(mxx), Math.Abs(mnx)).ToString(), ToastLength.Short).Show();
                float scl = Math.Max(Math.Min(15, Math.Min(pW / (60+mx), 
                    (pH - 10 - dy * (k + 4)) / (60+mx))), 0)/1.5f;// - 20 - dy * (k + 4)
                //scl = 840 / (896.69f * 2);
                Toast.MakeText(Android.App.Application.Context, scl.ToString(), ToastLength.Long).Show();
                SKPaint PTxt = new SKPaint() { Color = SKColors.Black, TextSize = h };
                using (var pdfCanvas = document.BeginPage(pW, pH))
                {
                    pdfCanvas.DrawRect(x, y - h, w, h, paint);
                    //pdfCanvas.DrawRect(x, y - h, 250, h, paint2);
                    pdfCanvas.DrawLine(x, y - h / 2, x + w, y - h / 2, paint);
                    string str = "стена";
                    PTxt.MeasureText(str);
                    pdfCanvas.DrawPath(PTxt.GetTextPath(str, x + w + 10, y - 5), PTxt);
                    //
                    y -= dy;
                    string str1 = "Дегенда карты:";//75
                    PTxt.MeasureText(str1);
                    pdfCanvas.DrawPath(PTxt.GetTextPath(str1, x, y - 5), PTxt);
                    //
                    y -= dy;
                    string Autors = "Авторы: ";//75
                    PTxt.MeasureText(Autors);
                    pdfCanvas.DrawPath(PTxt.GetTextPath(Autors, x, y - 5), PTxt);
                    //
                    y -= dy;
                    pdfCanvas.DrawLine(x, y, x, y - 5, paint);
                    pdfCanvas.DrawLine(x + 100, y, x + 100, y - 5, paint);
                    string str2 = Math.Round(10 * scl, 3).ToString() + " м";//75
                    PTxt.MeasureText(str2);
                    paint.StrokeWidth = 2;
                    pdfCanvas.DrawPath(PTxt.GetTextPath(str2, x + 5, y - 5), PTxt);
                    pdfCanvas.DrawLine(x, y, x + 100, y, paint);
                    string str3 = "Север - верх карты";//75
                    PTxt.MeasureText(str3);
                    pdfCanvas.DrawPath(PTxt.GetTextPath(str3, x + 105, y - 5), PTxt);
                    //
                    y -= dy;
                    string Name = "Название: ";
                    PTxt.MeasureText(Name);
                    pdfCanvas.DrawPath(PTxt.GetTextPath(Name, x, y - 5), PTxt);
                    //]
                    y -= dy;
                    pdfCanvas.DrawLine(0, y, 1000, y, paint);
                    paint.StrokeWidth = 1;
                    //pdfCanvas.DrawRect(0, y, 840, 150, pain
                    pdfCanvas.Scale(scl);
                    foreach (SKPaint pt in Paths.Keys)
                    {
                        for (int i = 0; i < Paths[pt].Count; i++)
                        {
                            SKPath pth = new SKPath(Paths[pt][i]);
                            pth.Transform(SKMatrix.CreateTranslation(pW/2/scl,(pH - 10 - dy * (k + 4)) /2/scl));//-10-dy*(k+4)
                            //pth.Transform(SKMatrix.CreateScale(scl, scl));
                            pdfCanvas.DrawPath(pth,paintT);
                        }
                    }
                }
                //document.Close();
            }
        }
        private void ToolbarItem(object sender, EventArgs e)
        {
            if (sm.Name!="")
            {
               

                Dictionary<SKPaint, List<SKPath>> PathsH = new Dictionary<SKPaint, List<SKPath>>();
                Dictionary<SKPaint, List<SKPath>> PathsV = new Dictionary<SKPaint, List<SKPath>>();
                foreach (Tuple<string,string> abrises in sm.abrisy)
                {
                    abri aH = new abri(abrises.Item1);
                    if (File.Exists(Serial.ptha(abrises.Item1)))
                        aH = abri.getabr(abrises.Item1);
                    abri aV = new abri(abrises.Item2);
                    if (File.Exists(Serial.ptha(abrises.Item2)))
                        aV = abri.getabr(abrises.Item2);
                    foreach (SKPaint pt in aH.Paths.Keys)
                    {
                        if (!PathsH.ContainsKey(pt)) PathsH.Add(pt, new List<SKPath>());
                        PathsH[pt].AddRange(aH.Paths[pt]);
                    }
                    foreach (SKPaint pt in aV.Paths.Keys)
                    {
                        if (!PathsV.ContainsKey(pt)) PathsV.Add(pt, new List<SKPath>());
                        PathsV[pt].AddRange(aV.Paths[pt]);
                    }
                }
                comp(PathsH,"H");
                comp(PathsV, "V");
                //Console.WriteLine(pH - y - 45);
                //Console.ReadKey();
            }
        }
    }
}