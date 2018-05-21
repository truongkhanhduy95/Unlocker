﻿using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Java.Util;

namespace Unlock
{
    [Service(Exported = true, Enabled = true)]
    public class LockScreenBackgroundService : Service
    {
        private LockScreenReciever mReciever = null;
        private Timer timer;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            //return base.OnStartCommand(intent, flags, startId);
            return StartCommandResult.Sticky;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            IntentFilter intentFilter = new IntentFilter();
            intentFilter.AddAction(Android.Content.Intent.ActionScreenOff);
            intentFilter.AddAction(ActionUnlock.Recall);
            intentFilter.Priority = 100;

            mReciever = new LockScreenReciever();

            RegisterReceiver(mReciever, intentFilter);
        }

        public override void OnDestroy()
        {

            Intent broadcastIntent = new Intent(ActionUnlock.Recall);
            SendBroadcast(broadcastIntent);

            base.OnDestroy();
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            //Intent restartServiceTask = new Intent(ApplicationContext, this.GetType());
            //restartServiceTask.SetPackage(PackageName);
            //Intent broadcastIntent = new Intent("Android.Content.Intent.ActionScreenOff.Recall");
            //PendingIntent restartPendingIntent = PendingIntent.GetService(ApplicationContext, 1, broadcastIntent, PendingIntentFlags.OneShot);
            //AlarmManager myAlarmService = (AlarmManager)ApplicationContext.GetSystemService(Context.AlarmService);
            //myAlarmService.Set(
                //AlarmType.ElapsedRealtime,
                //SystemClock.ElapsedRealtime() + 1000,
                //restartPendingIntent);

            base.OnTaskRemoved(rootIntent);
        }
    }
	
}
