using System;
using Android.App;
using Android.App.Job;
using Android.Content;

namespace Unlock
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [Android.App.IntentFilter(new[] { 
        Android.Content.Intent.ActionScreenOff,
        ActionUnlock.Recall,
        Android.Content.Intent.ActionBootCompleted,
        Android.Content.Intent.ActionUserPresent
    }, Priority = 100)]
    public class LockScreenReciever : BroadcastReceiver
    {
        const int notificationId = 777;
        const string CHANNEL_ID = "my_channel_01";

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                switch (intent.Action)
                {
                    case Android.Content.Intent.ActionBootCompleted: //Device rebooted
                    case ActionUnlock.Recall:                       //Application killed
                        StartLockScreenService(context);
                        break;
                    case Android.Content.Intent.ActionUserPresent:   //Screen unlocked
                        KillLocalNotification(context);
                        break;
                    case Android.Content.Intent.ActionScreenOff: //Screen locked
                        ShowLocalNotification(context);
                        break;
                }
            } catch(Exception ex)
            {
                Android.Util.Log.Error("LockScreenReciever", ex.Message);
            }
        }

        private void ShowLocalNotification(Context context)
        {

            Intent intent = new Intent(context, typeof(MainActivity));;

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            //Implement notification
            // Instantiate the builder and set notification elements:
            var importance = NotificationImportance.High;
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle(string.Empty)
                .SetContentText(string.Empty)
                .SetSmallIcon(Resource.Drawable.@lock)
                .SetChannelId(CHANNEL_ID)
                .AddAction(new Notification.Action(Resource.Drawable.@lock,"action",pendingIntent));
            
            // Build the notification:
            Notification notification = builder.Build();
            notification.SetLatestEventInfo(context, string.Empty, string.Empty, pendingIntent);

            notification.Flags |= NotificationFlags.AutoCancel;
            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            var notiChannel = new NotificationChannel(CHANNEL_ID, "Unlocker", importance);
            notificationManager.CreateNotificationChannel(notiChannel);

            // Publish the notification:
            notificationManager.Notify(notificationId, notification);
        }

        private void KillLocalNotification(Context context)
        {
            NotificationManager notificationManager =
                    context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Cancel(notificationId);
        }

        public void StartLockScreenService(Context context)
        {
            var javaClass = Java.Lang.Class.FromType(typeof(LockScreenBackgroundService));
            ComponentName component = new ComponentName(context, javaClass);
            JobInfo.Builder builder = new JobInfo.Builder(777, component)
                                     .SetMinimumLatency(1000)   // Wait at least 1 second
                                     //.SetOverrideDeadline(5000) // But no longer than 5 seconds
                                     .SetRequiredNetworkType(NetworkType.Unmetered);
            JobInfo jobInfo = builder.Build();

            JobScheduler jobScheduler = (JobScheduler)context.GetSystemService(MainActivity.JobSchedulerService);
            int result = jobScheduler.Schedule(jobInfo);
            if (result == JobScheduler.ResultSuccess)
            {
                // The job was scheduled.
            }
            else
            {
                // Couldn't schedule the job.
            }
        }

    }

}
