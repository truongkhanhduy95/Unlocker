using System;
using Android.App;
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

        public override void OnReceive(Context context, Intent intent)
        {
            switch(intent.Action)
            {
                case Android.Content.Intent.ActionBootCompleted: //Device rebooted
                case ActionUnlock.Recall:                       //Application killed
                    context.StartForegroundService(new Intent(context, typeof(LockScreenBackgroundService))); //restart service
                    break;
                case Android.Content.Intent.ActionUserPresent:   //Screen unlocked
                    KillLocalNotification(context);
                    break;
                case Android.Content.Intent.ActionScreenOff: //Screen locked
                    ShowLocalNotification(context);
                    break;
            }
        }

        private void ShowLocalNotification(Context context)
        {

            Intent intent = new Intent(context, typeof(MainActivity));;

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            //Implement notification
            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle(string.Empty)
                .SetContentText(string.Empty)
                .SetSmallIcon(Resource.Drawable.@lock)
                .AddAction(new Notification.Action(Resource.Drawable.@lock,"action",pendingIntent));
            
            // Build the notification:
            Notification notification = builder.Build();
            notification.SetLatestEventInfo(context, string.Empty, string.Empty, pendingIntent);

            notification.Flags |= NotificationFlags.AutoCancel;
            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            notificationManager.Notify(notificationId, notification);
        }

        private void KillLocalNotification(Context context)
        {
            NotificationManager notificationManager =
                    context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Cancel(notificationId);
        }
    }

}
