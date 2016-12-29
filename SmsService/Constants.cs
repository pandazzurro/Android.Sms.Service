using System;
namespace SmsService
{
	public static class Constants
	{
		public const int DELAY_BETWEEN_LOG_MESSAGES = 50; // seconds
		public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
		public const string SERVICE_STARTED_KEY = "has_service_been_started";
		public const string BROADCAST_MESSAGE_KEY = "broadcast_message";
		public const string NOTIFICATION_BROADCAST_ACTION = "SmsService.Notification.Action";

		public const string ACTION_START_SERVICE = "SmsService.action.START_SERVICE";
		public const string ACTION_STOP_SERVICE = "SmsService.action.STOP_SERVICE";
		public const string ACTION_RESTART_TIMER = "SmsService.action.RESTART_TIMER";
		public const string ACTION_MAIN_ACTIVITY = "SmsService.action.MAIN_ACTIVITY";
	}
}
