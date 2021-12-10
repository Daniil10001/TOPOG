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
            public List<List<SKPath>> PathsI = new List<List<SKPath>>();

            public List<SKPaint> PointK = new List<SKPaint>();
            public List<List<SKPoint>> PointI = new List<List<SKPoint>>();

            public List<Tuple<SKPaint, SKPaint>> ShapeK = new List<Tuple<SKPaint, SKPaint>>();
            public List<List<SKPath>> ShapeI = new List<List<SKPath>>();
            public static abri get(abris a1)
            {
                abri a = new abri(a1.name);
                for (int i = 0; i < a1.PathsK.Count; i++)
                {
                    a.Paths[a1.PathsK[i]] = a1.PathsI[i];
                }
                for (int i = 0; i < a1.PointK.Count; i++)
                {
                    a.Point[a1.PointK[i]] = a1.PointI[i];
                }
                for (int i = 0; i < a1.ShapeK.Count; i++)
                {
                    a.Shape[a1.ShapeK[i]] = a1.ShapeI[i];
                }
                return a;
            }
            public static abris crt(abri a1)
            {
                abris a = new abris(a1.name);
                foreach (SKPaint ob in a1.Paths.Keys)
                {
                    a.PathsK.Add(ob);
                    a.PathsI.Add(a1.Paths[ob]);
                }
                foreach (SKPaint ob in a1.Point.Keys)
                {
                    a.PointK.Add(ob);
                    a.PointI.Add(a1.Point[ob]);
                }
                foreach (Tuple<SKPaint,SKPaint> ob in a1.Shape.Keys)
                {
                    a.ShapeK.Add(ob);
                    a.ShapeI.Add(a1.Shape[ob]);
                }
                return a;
            }
        }
    }
}
