using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.App.Job;
using Java.Lang;

namespace Unlock
{
    [Activity(Label = "Unlocker", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        Intent backgroundService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //backgroundService = new Intent(ApplicationContext, typeof(LockScreenBackgroundService));
            //if (!IsServiceRunning(Java.Lang.Class.FromType(typeof(LockScreenBackgroundService))))
            //{
            //    StartService(backgroundService);
            //    Toast.MakeText(
            //        this,
            //        "Service registered successfully!!",
            //        ToastLength.Short
            //    );
            //}

            StartLockScreenService();
            Finish();    
        }

        public void StartLockScreenService()
        {
            var javaClass = Java.Lang.Class.FromType(typeof(LockScreenBackgroundService));
            ComponentName component = new ComponentName(ApplicationContext, javaClass);
            JobInfo.Builder builder = new JobInfo.Builder(777, component)
                                     .SetMinimumLatency(1000)   // Wait at least 1 second
                                     //.SetOverrideDeadline(5000) // But no longer than 5 seconds
                                     .SetRequiredNetworkType(NetworkType.Unmetered);
            JobInfo jobInfo = builder.Build();

            JobScheduler jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
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

        private bool IsServiceRunning(Java.Lang.Class serviceClass)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            // foreach (RunningServiceInfo service in manager.GetRunningServices(int.MaxValue))
            foreach (var item in manager.GetRunningServices(int.MaxValue))
            {
                if (serviceClass.Name.Equals(item.Service.ClassName))
                {
                    return true;
                }
            }
            return false;

        }

		protected override void OnDestroy()
		{
			base.OnDestroy();
            //StopService(backgroundService);
		}

	}
}

