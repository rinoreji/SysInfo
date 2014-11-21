using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;
using SystemInfo = System.Windows.Forms.SystemInformation;

namespace SysInfo
{
    /// <summary>
    /// Interaction logic for PowerInfo.xaml
    /// </summary>
    public partial class PowerInfo : UserControl
    {
        //Timer timer = new Timer();
        DispatcherTimer timer = new DispatcherTimer();
        List<BatteryInfo> batteryInfos = new List<BatteryInfo>();

        public PowerInfo()
        {
            InitializeComponent();
            //SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            try
            {
                batteryInfos = ExtentionMethods.DeserializeBatteryInfo();
            }
            catch { }
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += timer_Tick;
            RefreshStatus();
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void RefreshStatus()
        {
            txtChargeStatus.Text = "Battery Charge Status : {0}".UseFormat(SystemInfo.PowerStatus.BatteryChargeStatus);
            txtFullLifetime.Text = "Battery Full lifetime : {0}".UseFormat(SecToTime(SystemInfo.PowerStatus.BatteryFullLifetime));
            txtLifePercent.Text = "Battery Life percent : {0}%".UseFormat(SystemInfo.PowerStatus.BatteryLifePercent * 100);
            txtLifeRemaning.Text = "Battery Life Remaining : {0}".UseFormat(SecToTime(SystemInfo.PowerStatus.BatteryLifeRemaining));
            txtLineStatus.Text = "Power Line Status : {0}".UseFormat(SystemInfo.PowerStatus.PowerLineStatus);

            batteryInfos.Add(new BatteryInfo
            {
                DateTime = DateTime.Now,
                BatteryChargeStatus = (int)SystemInfo.PowerStatus.BatteryChargeStatus,
                BatteryFullLifetime = SystemInfo.PowerStatus.BatteryFullLifetime,
                BatteryLifePercent = SystemInfo.PowerStatus.BatteryLifePercent,
                BatteryLifeRemaining = SystemInfo.PowerStatus.BatteryLifeRemaining,
                PowerLineStatus = (int)SystemInfo.PowerStatus.PowerLineStatus
            });

            batteryInfos.Serialize();

            PlayAlarmIfNeeded();
        }

        string SecToTime(int seconds)
        {
            if (seconds <= 0)
                return "<No data>";
            var timespan = new TimeSpan(0, 0, seconds);
            return timespan.ToString();
        }

        void PlayAlarmIfNeeded()
        {
            if ((SystemInfo.PowerStatus.BatteryChargeStatus == System.Windows.Forms.BatteryChargeStatus.High) &&
                (SystemInfo.PowerStatus.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Online))
                Console.Beep(1000, 600);

            if ((SystemInfo.PowerStatus.BatteryChargeStatus == System.Windows.Forms.BatteryChargeStatus.Critical) &&
                (SystemInfo.PowerStatus.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Offline))
                Console.Beep(1000, 600);
        }
    }
}
