using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TOPOG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Paint : ContentPage
    {
        //TableView table = new TableView();
        public Paint()
        { 
            InitializeComponent();
            table.Root = new TableRoot()
            {
            new TableSection("Линии"){},
            new TableSection("Точки"){},
            new TableSection("Области"){},
            };
            List<CPaintL> l = new List<CPaintL>();
            CPaintL cpnt = new CPaintL();
            cpnt.A = 255;
            cpnt.R = 255;
            cpnt.name = "red";
            l.Add(new CPaintL(cpnt));
            cpnt.R = 0;
            cpnt.A = 255;
            cpnt.B = 255;
            cpnt.name = "blue";
            l.Add(cpnt);
            foreach (CPaintL cp in l)
            {
                Grid g = new Grid() { RowDefinitions = { new RowDefinition { }, },
                    ColumnDefinitions = { new ColumnDefinition{ }, new ColumnDefinition { }, new ColumnDefinition { } } };
                SKCanvasView c = new SKCanvasView();
                c.PaintSurface+=async(sender,args)=> {
                    SKCanvas cn = args.Surface.Canvas;
                    cn.DrawLine(0, c.CanvasSize.Height / 2, c.CanvasSize.Width, c.CanvasSize.Height / 2, cp.getc());
                };
                c.InvalidateSurface();
                g.Children.Add(c, 0, 0);
                g.Children.Add(new ScrollView { Orientation=ScrollOrientation.Horizontal, Content=new Label { Text=cp.name, VerticalOptions=LayoutOptions.Center, HorizontalOptions=LayoutOptions.Center } },1,0);
                Button bt = new Button { Text="Выбрать" };
                bt.Clicked += async(sender,args) =>
                { App.Current.Properties["Paint"] = new Tuple<int, CPaintL>(1,new CPaintL(cp)); }; //((Tuple<int, CPaintL>)App.Current.Properties["Paint"]).Item2
                g.Children.Add(bt, 2, 0);
                table.Root[0].Add(new ViewCell { View=g });
            }
        }
    }
} 