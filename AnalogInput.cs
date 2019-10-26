using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pimoroni.AutomationHat.SN3218LEDDriver;

namespace Pimoroni.AutomationHat
{
    public class AnalogInput
    {

        public delegate void OnVoltageChanged(int channel, double voltage);
        public event OnVoltageChanged VoltageChanged;

        public double MaxVoltage { get; set; }

        private double _voltage = 0;        
        public double Voltage
        {
            get
            {
                return _voltage;
            }


            internal set
            {
                double dNewVoltage = value * MaxVoltage;

                bool bFire = HasVoltageChanged(_voltage, dNewVoltage);
                _voltage = dNewVoltage;
                if ((bFire) && (VoltageChanged!=null))
                {
                    Task.Run(() =>
                    {
                        VoltageChanged(Channel, _voltage);
                    });
                }
                
            }
        }

        internal Led Light { get; private set; }

      
        internal AnalogInput(int channel, double maxVoltage, Led light, double changeThreshold=0.5)
        {
            Channel = channel;
            Light = light;
            MaxVoltage = maxVoltage;
            ChangeThreshold = changeThreshold;
        }


        public double ChangeThreshold { get; set; }

        /// <summary>
        /// Which ADC channel (0-4) is the input connected to 
        /// </summary>
        public int Channel { get; set; }

        private bool HasVoltageChanged(double oldv, double newv)
        {
                    double diff = newv - oldv;
                    if (diff < 0) { diff *= -1; }

                    return (diff > ChangeThreshold);
        }



    }
}
