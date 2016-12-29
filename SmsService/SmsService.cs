using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Telephony;

namespace SmsService
{
    [Service]
    public class SmsService : Service
    {
        static readonly string TAG = typeof(SmsService).FullName;

        UtcTimestamper timestamper;
        bool isStarted;
        Handler handler;
        Action runnable;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "OnCreate: the service is initializing.");

            timestamper = new UtcTimestamper();
            handler = new Handler();

            // This Action is only for demonstration purposes.
            runnable = new Action(() =>
            {
                if (timestamper == null)
                {
                    Log.Wtf(TAG, "Why isn't there a Timestamper initialized?");
                }
                else
                {
                    string msg = timestamper.GetFormattedTimestamp();
                    Log.Debug(TAG, msg);
                    Intent i = new Intent(Constants.NOTIFICATION_BROADCAST_ACTION);
                    string smsText = "Messaggio di prova";
                    string mezzaNumber = "+393492980069";
                    string mioNumber = "+393408911824";
                    //SmsManager.Default.SendTextMessage(mezzaNumber, null, smsText, null, null);
                    SmsManager.Default.SendTextMessage(mioNumber, null, smsText, null, null);
                    Log.Info(TAG, "sent sms");
                    handler.PostDelayed(runnable, Constants.DELAY_BETWEEN_LOG_MESSAGES * (long)1000);
                }
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals(Constants.ACTION_START_SERVICE))
            {
                if (isStarted)
                {
                    Log.Info(TAG, "OnStartCommand: The service is already running.");
                }
                else
                {
                    Log.Info(TAG, "OnStartCommand: The service is starting.");
                    RegisterForegroundService();
                    handler.PostDelayed(runnable, Constants.DELAY_BETWEEN_LOG_MESSAGES * (long)1000); // lancio per la prima volta il servizio
                    isStarted = true;
                }
            }
            else if (intent.Action.Equals(Constants.ACTION_STOP_SERVICE))
            {
                Log.Info(TAG, "OnStartCommand: The service is stopping.");
                timestamper = null;
                StopForeground(true);
                StopSelf();
                isStarted = false;

            }
            else if (intent.Action.Equals(Constants.ACTION_RESTART_TIMER))
            {
                Log.Info(TAG, "OnStartCommand: Restarting the timer.");
                timestamper.Restart();
            }

            // This tells Android not to restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
        }


        public override IBinder OnBind(Intent intent)
        {
            // Return null because this is a pure started service. A hybrid service would return a binder that would
            // allow access to the GetFormattedStamp() method.
            return null;
        }


        public override void OnDestroy()
        {
            // We need to shut things down.
            Log.Info(TAG, "OnDestroy: The started service is shutting down.");

            // Stop the handler.
            handler.RemoveCallbacks(runnable);

            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(Constants.SERVICE_RUNNING_NOTIFICATION_ID);

            timestamper = null;
            isStarted = false;
            base.OnDestroy();
        }
        

        void RegisterForegroundService()
        {
            var notification = new Notification.Builder(this)
                .SetContentTitle(Resources.GetString(Resource.String.app_name))
                .SetContentText(Resources.GetString(Resource.String.notification_text))                               
                //.SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                //.AddAction(BuildRestartTimerAction())
                //.AddAction(BuildStopServiceAction())
                .Build();


            // Enlist this instance of the service as a foreground service
            StartForeground(Constants.SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }

        /// <summary>
        /// Builds a PendingIntent that will display the main activity of the app. This is used when the 
        /// user taps on the notification; it will take them to the main activity of the app.
        /// </summary>
        /// <returns>The content intent.</returns>
        //PendingIntent BuildIntentToShowMainActivity()
        //{
        //    var notificationIntent = new Intent(this, typeof(MainActivity));
        //    notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
        //    notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
        //    notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

        //    var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
        //    return pendingIntent;
        //}

        /// <summary>
        /// Builds a Notification.Action that will instruct the service to restart the timer.
        /// </summary>
        /// <returns>The restart timer action.</returns>
        //Notification.Action BuildRestartTimerAction()
        //{
        //    var restartTimerIntent = new Intent(this, GetType());
        //    restartTimerIntent.SetAction(Constants.ACTION_RESTART_TIMER);
        //    var restartTimerPendingIntent = PendingIntent.GetService(this, 0, restartTimerIntent, 0);

        //    var builder = new Notification.Action.Builder(Resource.Drawable.Icon,
        //                                      GetText(Resource.String.restart_timer),
        //                                      restartTimerPendingIntent);

        //    return builder.Build();
        //}

        /// <summary>
        /// Builds the Notification.Action that will allow the user to stop the service via the
        /// notification in the status bar
        /// </summary>
        /// <returns>The stop service action.</returns>
        //Notification.Action BuildStopServiceAction()
        //{
        //    var stopServiceIntent = new Intent(this, GetType());
        //    stopServiceIntent.SetAction(Constants.ACTION_STOP_SERVICE);
        //    var stopServicePendingIntent = PendingIntent.GetService(this, 0, stopServiceIntent, 0);

        //    var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
        //                                                  GetText(Resource.String.stop_service),
        //                                                  stopServicePendingIntent);
        //    return builder.Build();

        //}
    }

}