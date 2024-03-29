﻿
using System.Runtime.Versioning;

#if WINDOWS
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
#endif


namespace Shell.Notification.CoreClr
{
    public class Notification
    {
        public void SendNotification(string title, string message, string sound, string icon)
        {
            var (isSend, error_message) = IsCanNotification();

            if (! isSend)
            {
                throw new SystemException(error_message);
	        }
#if WINDOWS
            WindowsSendNotification();
#else
            if (OperatingSystem.IsLinux())
            {
                LinuxSendNotification();   
            } else if (OperatingSystem.IsMacOS())
            {
                MacOSSendNotification();
            }
#endif
        }

        private void _SendNotification()
        { 
	    }

        public (bool, string) IsCanNotification()
        {
            var judge = true;
            var message = "";
#if WINDOWS
            // 自 分でterminal-notify notify-send

            WindowsSendNotification();
#else
            if (OperatingSystem.IsLinux())
            {
                judge = System.IO.File.Exists("/usr/bin/notify-send");
                message = "Linux OS need notify-send.";
            } else if (OperatingSystem.IsMacOS())
            {
                judge = System.IO.File.Exists("/usr/local/bin/terminal-notifier");
                message = "macos need terminal-notifier.";
                // いない芋も持っていない
            }

#endif
            return (judge, message);
	    }

#if WINDOWS
        public void WindowsSendNotification()
        {
            var notification = new AppNotificationBuilder()
                .AddText("Notification text.")
                .SetScenario(AppNotificationScenario.Reminder)
                .BuildNotification();

            AppNotificationManager.Default.Show(notification);
        }
#else
        [SupportedOSPlatform("linux")]
        public void LinuxSendNotification()
        {

            // /usr/share/icons/image.png
            // ~/.local/share/icons/image.png
            // --icon image　で実行できる。
            // title, messageの順に処理される。
            // どちらかに置かないと処理できない。
            // judge = System.IO.File.Exists("/usr/bin/notify-send");
        }

        [SupportedOSPlatform("MacCatalyst")]
        public void MacOSSendNotification()
        {
            // macはappiconハウゴッはは
            // terminal-notifier
            // judge = System.IO.File.Exists("/usr/local/bin/terminal-notifier");
            // いない芋も持っていない

        }

#endif

    }
}