using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Plugin.LocalNotifications;

namespace CVD_Calc.Droid
{
    [Service]
    class StarterService : Service
    {
        Timer timer = new Timer(800);
        public override IBinder OnBind(Intent intent) => null;
        public override void OnCreate()
        {
            base.OnCreate();
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                startMyOwnForeground();
            else
                StartForeground(1, new Notification());
            timer.Elapsed += (sender, e) =>
            {
                if ((DateTime.Now.Hour +1) % 6== 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                    //f(DateTime.Now.Hour==9&&DateTime.Now.Minute==16&&DateTime.Now.Second==0)
                   //CrossLocalNotifications.Current.Show("A reminder from cvd", "the time now is" + DateTime.Now.ToShortTimeString());
                    App.calcvdAsync(true);
            };
            timer.Start();
        }
        private void startMyOwnForeground()
        {
            string channelid = "com.yw.CVD_Calc";
            string channelname = "Calculate CVD";
            NotificationChannel channel = new NotificationChannel(channelid,
                channelname, NotificationImportance.None);
            channel.LightColor = Color.LightBlue;
            channel.LockscreenVisibility = NotificationVisibility.Private;
            NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
            if (manager == null) return;
            manager.CreateNotificationChannel(channel);
            StartForeground(2, new NotificationCompat.Builder(this, channelid)
                .SetOngoing(true).SetContentTitle("App is running")
                .SetPriority((int)NotificationImportance.Max)
                .SetCategory(Notification.CategoryService).Build());
        }
    }
}