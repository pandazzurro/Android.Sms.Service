using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;

namespace SmsService
{
    // https://forums.xamarin.com/discussion/981/unable-to-start-a-background-service-on-device-bootup

    [Activity(Label = "SmsService", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Intent startServiceIntent;
        Intent stopServiceIntent;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            //SetContentView (Resource.Layout.Main);

            startServiceIntent = new Intent(this, typeof(SmsService));
            startServiceIntent.SetAction(Constants.ACTION_START_SERVICE);

            stopServiceIntent = new Intent(this, typeof(SmsService));
            stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);

            StartService(startServiceIntent);
        }

        protected override void OnDestroy()
        {
            StopService(startServiceIntent);
            base.OnDestroy();
        }
    }
}

