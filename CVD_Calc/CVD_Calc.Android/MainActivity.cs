using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Content;

namespace CVD_Calc.Droid
{
    [Activity(Label = "CVD_Calc", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            string persondb = Path.Combine(
                System.Environment.GetFolderPath(System.
                Environment.SpecialFolder.Personal),  "personaldata.sqlite");
            string everydaydb = Path.Combine(
                System.Environment.GetFolderPath(System.
                Environment.SpecialFolder.Personal),  "everydayinfo.sqlite");
            LoadApplication(new App(persondb, everydaydb));
            //LoadApplication(new App(persondb,everydaydb));
            startrunservice();
        }
        public void startrunservice()
        {
            Intent i = new Intent(this, typeof(Autostart));
            PendingIntent pi = PendingIntent.GetBroadcast(
                Application.Context, 0, i, 0);
            AlarmManager am = (AlarmManager)GetSystemService(AlarmService);
            am.Set(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis(), pi);
        }
    }
}