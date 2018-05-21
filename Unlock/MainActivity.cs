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
            if (!IsServiceRunning())
            {
                StartService(backgroundService);
                Toast.MakeText(
                    this,
                    "Service registered successfully!!",
                    ToastLength.Short
                );
                Finish();
            }
                
        }

        private bool IsServiceRunning()
        {
            //ActivityManager manager = (ActivityManager)GetSystemService(typeof(LockScreenBackgroundService));
            //for (ActivityManager.RunningServiceInfo service : manager.GetRunningServices(30))
            //{
            //    if (serviceClass.getName().equals(service.service.getClassName()))
            //    {
            //        return true;
            //    }
            //}
            return false;

        }

		protected override void OnDestroy()
		{
          
			base.OnDestroy();
            StopService(backgroundService);
		}

	}
}

