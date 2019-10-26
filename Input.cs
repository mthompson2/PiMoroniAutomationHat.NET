using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using static Pimoroni.AutomationHat.SN3218LEDDriver;

namespace Pimoroni.AutomationHat
{
    public class Input
    {
        private GpioPin _pin;

        private Led _noLed;

        public delegate void OnChanged(bool b);
        public event OnChanged Changed;

        private int _pinNumber;

        public bool State { get; private set; }

        internal Input(int gpioPin, int ledNumber, bool pullDown=true, GpioController controller=null)
        {

            if (controller==null)
            {
                controller = Controllers.GPIOController;
            }

            _pinNumber = gpioPin;
            _pin =controller.OpenPin(gpioPin);
            if (pullDown)
                _pin.SetDriveMode(GpioPinDriveMode.InputPullDown);
            else
                _pin.SetDriveMode(GpioPinDriveMode.InputPullUp);

          //  _pin.DebounceTimeout = TimeSpan.FromMilliseconds(20);

            _pin.ValueChanged += (s, e) =>
            {
                State = e.Edge == GpioPinEdge.RisingEdge;

                Task.Run(() => // Get off the main thread as quickly as possible
                {
                    _noLed.State = State;
                    Changed?.Invoke(State);
                });
            };


            _noLed = new Led(ledNumber);
        }


        public void SetPullDown(bool down)
        {
            if (down)
                _pin.SetDriveMode(GpioPinDriveMode.InputPullDown);
            else
                _pin.SetDriveMode(GpioPinDriveMode.InputPullUp);
        }


    }
}
