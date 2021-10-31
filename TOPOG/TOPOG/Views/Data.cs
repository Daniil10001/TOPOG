using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace TOPOG.Views
{
    public class Cave
    {
        public Cave()
        {
            Name = "";
            Locat = "";
            Description = "";
        }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Locat { get; set; }
        public string PathA
        {
            get {
                if (Name != "")
                {
                    return Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + Name + ".cv";
                }
                else
                {
                    return null;
                }
            }
        }

    }
    public class Risun
    {
        public Risun()
        {
            pts = new SKPath();
            pant = "";
            fill = "";
        }
        public SKPath pts { get; set; }
        public string pant { get; set; }
        public string fill { get; set; }
    }
    public class Izm
    {
        public Izm(double xn, double yn, double zn)
        {
            x = xn;
            y = yn;
            x = xn;

        }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }
    public class Ezi
    {
        public Ezi()
        {
            sdv = null;
            ezi = new List<Izm>();
        }
        public Izm sdv { get; set; }
        public List<Izm> ezi { get; set; }
    }
    public class abri
    {
        public abri(string name)
        {
            ris = new List<Risun>();
            name = "";
        }
        public List<Risun> ris { get; set; }
        public string name { get; set; }
    }
    public class Semka
    {
        public Semka()
        {
            Name = "";
            Description = "";
            Mest = "";
            nach = "";
            pwe = new Dictionary<string, Ezi>();
            pereh = new Dictionary<string, HashSet<string>>();
            sdvig = new Dictionary<Tuple<string, string>, Izm>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Mest { get; set; }
        public string nach { get; set; }
        public Dictionary<string, Ezi> pwe { get; set; }
        public Dictionary<string, HashSet<string>> pereh { get; set; }
        public Dictionary<Tuple<string, string>, Izm> sdvig { get; set; }
        public string PathA
        {
            get
            {
                if (Name != "")
                {
                    return Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + Name + "_abr.cv";
                }
                else
                {
                    return null;
                }
            }
        }
    }
    class Postr
    {
        public Postr()
        {
            berh = new Dictionary<string, Tuple<Izm, Postr>>();
        }
        public string nm { get; set; }
        public Dictionary<string,Tuple<Izm,Postr>> berh { get; set; }
    }
    class Builder
    {
        public Builder(Semka sm)
        {
            smk = sm;
            //dsdv = new Dictionary<string, Izm>();
            isp = new Dictionary<string, bool>();
        }
        public Semka smk { get; set; }
        //public Dictionary<string, Izm> dsdv { get; set; }
        public Dictionary<string, bool> isp { get; set; }
        public Dictionary<string, bool> nep { get; set; }
        //int otv { get; set; }
        //int posl { get; set; }
        public Postr dfs(string nach)
        {
            Postr ps = new Postr();
            ps.nm = nach;
            isp[nach] = true;
            foreach (string sl in smk.pereh[nach])
            {
                var vr = smk.sdvig[new Tuple<string, string>(nach, sl)];
                Izm izm = new Izm(vr.x, vr.y, vr.z);
                if (isp.ContainsKey(sl)) ps.berh[sl] = new Tuple<Izm,Postr>(izm,null);
                else ps.berh[sl]=new Tuple<Izm, Postr>(izm,dfs(sl));
                //if (!smk.sdvig.ContainsKey(new Tuple<string, string>(sl, nach)))
            }
            return ps;
        }
        static public List<Predst> obratn(Postr ps,int i=0)
        {
            List<Predst> pr = new List<Predst>();
            foreach (string p in ps.berh.Keys)
            {
                Tuple<Izm,Postr> it = ps.berh[p];
                pr.Add(new Predst(it.Item1,ps.nm,p,i+1));
                if (it.Item2!=null)
                    pr.AddRange(obratn(it.Item2,i+1));
            }
            return pr;
        }
    }
    public class Predst
    {
        public Predst(Izm iz, string otp, string top, int otst)
        {
            x = iz.x;
            y = iz.y;
            z = iz.z;
            ot = otp;
            to = top;
            otstup = new String(' ', otst);
        }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public string ot { get; set; }
        public string to { get; set; }
        public string otstup { get; set; }

    }
}
