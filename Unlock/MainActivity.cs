using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

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

            backgroundService = new Intent(ApplicationContext, typeof(LockScreenBackgroundService));
            if (!IsServiceRunning(Java.Lang.Class.FromType(typeof(LockScreenBackgroundService))))
            {
                StartForegroundService(backgroundService);
                Toast.MakeText(
                    this,
                    "Service registered successfully!!",
                    ToastLength.Short
                );
              
            }
            Finish();    
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
            StopService(backgroundService);
		}

	}
}

