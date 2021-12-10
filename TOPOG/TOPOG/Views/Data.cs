using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SkiaSharp;
using Android.Widget;
using System.Xml.Serialization;
using Newtonsoft.Json;
using static TOPOG.Views.Serialization_Magic;

namespace TOPOG.Views
{

    public class Serial
    {
        public static void Save(object obj,Type T,string path)
        {
            try
            {
                /*FileStream fs = new FileStream(path, FileMode.OpenOrCreate,
                                       FileAccess.ReadWrite,
                                       FileShare.None);
                XmlSerializer serializer = new XmlSerializer(T);
                //Invisionware.Serialization.IXmlSerializer;
                TextWriter writer = new StreamWriter(fs, Encoding.UTF8);
                serializer.Serialize(writer, obj);
                writer.Close();
                fs.Close();*/
                File.WriteAllText(path, JsonConvert.SerializeObject(obj));
            }
            catch(Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.ToString(), ToastLength.Long).Show();
                File.WriteAllText(path+".txt", ex.ToString());
            }
        }
        public static object Open(Type T,string path)
        {
            try
            {
                /*XmlSerializer serializer = new XmlSerializer(T);
                FileStream fs = new FileStream(path, FileMode.Open);
                object o = serializer.Deserialize(fs);
                fs.Close();
                return o;*/
                return JsonConvert.DeserializeObject(File.ReadAllText(path), T);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.ToString(), ToastLength.Long).Show();
                File.WriteAllText(path + ".txt", ex.ToString());
                return null;
            }
        }
    }
    public class Cave
    {


        public Cave()
        {
            Name = "";
            Locat = "";
            Description = "";
        }
        public Cave(string a,string b,string c)
        {
            Name = a;
            Locat = b;
            Description = c;
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
    public class Ez
    {
        public Ez(double xn, double yn, double zn, bool b)
        {
            x = xn;
            y = yn;
            x = xn;
            orient = b;
        }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public bool orient { get; set; }

    }
    public class Spley
    {
        public Spley()
        {
            spl = new HashSet<Ez>();
        }
        public Spley(HashSet<Ez> a)
        {
            spl = a;
        }
        public HashSet<Ez> spl { get; set; }
    }
    public class abri
    {
        public abri(string name)
        {
            name = "";
            Paths = new Dictionary<SKPaint, List<SKPath>>();
            Point = new Dictionary<SKPaint, List<SKPoint>>();
            Shape = new Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>>();
        }
        public abri(string name, Dictionary<SKPaint, List<SKPath>> a,Dictionary<SKPaint, List<SKPoint>> b,Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>>c) 
        {
            name = "";
            Paths = a;
            Point = b;
            Shape = c;
        }
        public string name { get; set; }

        public Dictionary<SKPaint, List<SKPath>> Paths = new Dictionary<SKPaint, List<SKPath>>();

        public Dictionary<SKPaint, List<SKPoint>> Point = new Dictionary<SKPaint, List<SKPoint>>();

        public Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>> Shape = new Dictionary<Tuple<SKPaint, SKPaint>, List<SKPath>>();
        public static abri getabr(string pth)
        {
            return abris.get((abris)Serial.Open(typeof(abris),pth));
            //return JsonConvert.DeserializeObject<abri>(File.ReadAllText(pth));
        }
    }
    public class Semka
    {
        public Semka()
        {
            Name = "";
            Description = "";
            Mest = "";
            nach = "";
            splei = new Dictionary<string, Spley>();
            pereh = new Dictionary<string, HashSet<string>>();
            sdvig = new Dictionary<string, Izm>();
            abrisy = new List<Tuple<string, string>>();
        }
        public Semka(string a, string b, string c, string d, Dictionary<string, Spley> e, Dictionary<string, HashSet<string>> f, Dictionary<string, Izm> g, List<Tuple<string, string>> h)
        {
            Name = a;
            Description = b;
            Mest = c;
            nach = d;
            splei = e;
            pereh = f;
            sdvig = g;
            abrisy = h;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Mest { get; set; }
        public string nach { get; set; }
        public Dictionary<string, Spley> splei { get; set; }
        public Dictionary<string, HashSet<string>> pereh { get; set; }
        public Dictionary<string, Izm> sdvig { get; set; }
        public List<Tuple<string, string>> abrisy { get; set; }
        /*public string PathA
        {
            get
            {
                if (Name != "")
                {
                    return Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + Name + ".abr";
                }
                else
                {
                    return null;
                }
            }
        }*/
        public void save()
        {
            if (Name != "")
            {
                string pth = Android.App.Application.Context.GetExternalFilesDir("").ToString() + "/" + Name + ".cv";
                /*string createText = JsonConvert.SerializeObject(this);
                File.WriteAllText(pth, createText);*/
                Serial.Save(this, this.GetType(), pth);
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
                var vr = smk.sdvig[nach+ "!@TOPOG@!" + sl];
                Izm izm = new Izm(vr.x, vr.y, vr.z);
                if (isp.ContainsKey(sl) || !smk.pereh.ContainsKey(sl)) ps.berh[sl] = new Tuple<Izm,Postr>(izm,null);
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
                pr.Add(new Predst(it.Item1,ps.nm,p,i));
                if (it.Item2!=null)
                    pr.AddRange(obratn(it.Item2,i+1));
            }
            return pr;
        }

        static public List<Predst> obratn1(Postr ps, int i = 0)
        {
            List<Predst> pr = new List<Predst>();
            foreach (string p in ps.berh.Keys)
            {
                Tuple<Izm, Postr> it = ps.berh[p];
                pr.Add(new Predst(it.Item1, ps.nm, p, i));
            }
            foreach (string p in ps.berh.Keys)
            {
                Tuple<Izm, Postr> it = ps.berh[p];
                if (it.Item2 != null)
                    pr.AddRange(obratn(it.Item2));
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
            otstup = new String('.', otst);
        }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public string ot { get; set; }
        public string to { get; set; }
        public string otstup { get; set; }

    }
}
