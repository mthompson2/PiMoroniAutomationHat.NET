using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using static Pimoroni.AutomationHat.SN3218LEDDriver;

namespace Pimoroni.AutomationHat
{
    public class Relay
    {
        private GpioPin _pin;
        private Led _ncLed;
        private Led _noLed;

        internal Relay(int gpioPin, int ledNumberNO, int ledNumberNC)
        {

            _pin = Controllers.GPIOController.OpenPin(gpioPin);
            _pin.SetDriveMode(GpioPinDriveMode.Output);

            _ncLed = new Led(ledNumberNC);
            _noLed = new Led(ledNumberNO);
            State = false;
        }

        private bool _state = false;
        public bool State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
                _pin.Write(value ? GpioPinValue.High : GpioPinValue.Low);
                _noLed.State = value;
                _ncLed.State = !value;
            }
        }

    }
}
