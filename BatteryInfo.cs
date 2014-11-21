using System;

namespace SysInfo
{
    [Serializable]
    public class BatteryInfo
    {
        public DateTime DateTime { get; set; }
        public int BatteryChargeStatus { get; set; }
        public int BatteryFullLifetime { get; set; }
        public float BatteryLifePercent { get; set; }
        public int BatteryLifeRemaining { get; set; }
        public int PowerLineStatus { get; set; }
    }
}
