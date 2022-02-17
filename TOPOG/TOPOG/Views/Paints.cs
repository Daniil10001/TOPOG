using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TOPOG.Views
{
    public static class StandPaints
    {
        private static Dictionary<int, Dictionary<string, object>> paints = new Dictionary<int, Dictionary<string, object>>(){
            {
                0, new Dictionary<string,object>(){
                {
                "стена",
                new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Red,
                    StrokeWidth = 2,
                    StrokeCap = SKStrokeCap.Square,
                    StrokeJoin = SKStrokeJoin.Round}
                },
                {
                "стена",
                new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Red,
                    StrokeWidth = 2,
                    StrokeCap = SKStrokeCap.Square,
                    StrokeJoin = SKStrokeJoin.Round}
                }
            }
            }
        };
        public static object getpts(int i, string s)
        {
            return paints[i][s];
        }
        public static object getks(int i)
        {
            return paints[i].Keys;
        }
    }
    public class CPaintL
    {
        public byte R, G, B, A;
        public string p, name;
        public float sx, sy;
        public CPaintL()
        {
            R = 0;
            G = 0;
            B = 0;
            A = 0;
            p = (new SKPath()).ToSvgPathData();
            name = "";
            sx = 1;
            sy = 1;
        }
        public CPaintL(byte Rv,byte Gv,byte Bv, byte Av,string namev)
        {
            R = Rv;
            G = Gv; 
            B = Bv;
            A = Av;
            p = (new SKPath()).ToSvgPathData();
            name = namev;
            sx = 1;
            sy = 1;
        }
        public CPaintL(CPaintL cp)
        {
            R = cp.R;
            G = cp.G;
            B = cp.B;
            A = cp.A;
            p = cp.p;
            name = cp.name;
            sx = cp.sx;
            sy = cp.sy;
        }
        public SKPaint getc()
        {
            SKColor cl = new SKColor();
            cl = cl.WithRed(R);
            cl = cl.WithGreen(G);
            cl = cl.WithBlue(B);
            cl = cl.WithAlpha(A);
            SKPaint pt = new SKPaint { Color = cl, Style=SKPaintStyle.Stroke, StrokeWidth=2, StrokeCap = SKStrokeCap.Square, StrokeJoin = SKStrokeJoin.Round };
            return pt;
        }
    }
}