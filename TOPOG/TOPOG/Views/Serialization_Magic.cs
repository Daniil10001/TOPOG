using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TOPOG.Views
{
    class Serialization_Magic
    {
        public class abris
        {
            public abris(string name)
            {
                name = "";
            }
            public string name { get; set; }

            public List<SKPaint> PathsK = new List<SKPaint>();
            public List<List<String>> PathsI = new List<List<string>>();

            public List<SKPaint> PointK = new List<SKPaint>();
            public List<List<SKPoint>> PointI = new List<List<SKPoint>>();

            public List<Tuple<SKPaint, SKPaint>> ShapeK = new List<Tuple<SKPaint, SKPaint>>();
            public List<List<string>> ShapeI = new List<List<string>>();
            public static abri get(abris a1)
            {
                abri a = new abri(a1.name);
                for (int i = 0; i < a1.PathsK.Count; i++)
                {
                    a.Paths[a1.PathsK[i]] = new List<SKPath>();
                    foreach (string p in a1.PathsI[i])
                    {
                        a.Paths[a1.PathsK[i]].Add(SKPath.ParseSvgPathData(p));
                    }
                }
                for (int i = 0; i < a1.PointK.Count; i++)
                {
                    a.Point[a1.PointK[i]] = a1.PointI[i];
                }
                for (int i = 0; i < a1.ShapeK.Count; i++)
                {
                    a.Shape[a1.ShapeK[i]] = new List<SKPath>();
                    foreach (string p in a1.ShapeI[i])
                    {
                        a.Shape[a1.ShapeK[i]].Add(SKPath.ParseSvgPathData(p));
                    }
                }
                return a;
            }
            public static abris crt(abri a1)
            {
                abris a = new abris(a1.name);
                foreach (SKPaint ob in a1.Paths.Keys)
                {
                    a.PathsK.Add(ob);
                    a.PathsI.Add(new List<string>());
                    foreach (SKPath p in a1.Paths[ob])
                    {
                        a.PathsI[a.PathsI.Count-1].Add(p.ToSvgPathData());
                    }
                }
                foreach (SKPaint ob in a1.Point.Keys)
                {
                    a.PointK.Add(ob);
                    a.PointI.Add(a1.Point[ob]);
                }
                foreach (Tuple<SKPaint,SKPaint> ob in a1.Shape.Keys)
                {
                    a.ShapeK.Add(ob); 
                    a.ShapeI.Add(new List<string>());
                    foreach (SKPath p in a1.Shape[ob])
                    {
                        a.ShapeI[a.ShapeI.Count - 1].Add(p.ToSvgPathData());
                    }
                }
                return a;
            }
        }
    }
}
