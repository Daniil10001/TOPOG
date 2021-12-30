using Android.Bluetooth;
using Android.Widget;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Util;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Telephony;
using Android.Content;
using Android.OS;

namespace TOPOG.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public class Devi 
    { 
        public string Addres { get; set; }
        public string Name { get; set; }
        public BluetoothDevice bt { get; set; }
        public UUID ind { get; set; }
        //public BluetoothSocket bsck { get; set; }
    }
    public partial class Conect : ContentPage
    {
        public List<Devi> ConnectL { get; set; }
        public Conect()
        {
            InitializeComponent();
            ConnectL = new List<Devi>();
            this.BindingContext = this;
        }

        public void Reset(object sender, System.EventArgs e)
        {
            ConnectLs.BeginRefresh();
            ConnectLs.ItemsSource = null;
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (!mBluetoothAdapter.IsEnabled)
                mBluetoothAdapter.Enable();
            ConnectL.Clear();
            foreach (BluetoothDevice bt in mBluetoothAdapter.BondedDevices)
            {
                ConnectL.Add(new Devi { Addres = bt.Address, Name = bt.Name, bt=bt});
            }
            ConnectLs.EndRefresh();
            ConnectLs.ItemsSource = ConnectL;
            this.BindingContext = this;
        }

        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
        //[Obsolete]
        public void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ConnectLs.BeginRefresh();
            Devi d = (Devi)e.SelectedItem;
            //Context cnt = Android.App.Application.Context;
            //TelephonyManager tManager = (TelephonyManager)cnt.GetSystemService(Context.TelephonyService);
            lb.Text = "";
            //Toast.MakeText(Android.App.Application.Context, d.bt.GetType().ToString(), ToastLength.Long).Show();
            BluetoothSocket sock=null;
            bool Completed = ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(10000), () =>
            {
                try
                {
                    foreach (ParcelUuid ind in d.bt.GetUuids())
                    {
                        try
                        {
                            //tManager.GetDeviceId(0)
                            sock = d.bt.CreateRfcommSocketToServiceRecord(ind.Uuid);
                            sock.Connect();
                            sock.Close();
                            //Toast.MakeText(Android.App.Application.Context, sock.IsConnected.ToString(), ToastLength.Short).Show();
                            lb.Text = lb.Text + "\nyes " + ind.Uuid.ToString();
                        }
                        catch (Exception ex) { lb.Text = lb.Text + "\nno " + ind.Uuid.ToString(); }
                    }
                    lb.Text = lb.Text + "\nall";
                }
                catch
                {
                    Toast.MakeText(Android.App.Application.Context, "no availible connection", ToastLength.Short).Show();
                }
            });
            try { sock.Close(); }
            catch { }
            if (!Completed) Toast.MakeText(Android.App.Application.Context, "no availible connection", ToastLength.Short).Show();
            ConnectLs.EndRefresh();
            //ConnectLs.SelectedItem = null;
        }
    }
}