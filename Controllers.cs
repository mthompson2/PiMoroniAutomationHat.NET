using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Pimoroni.AutomationHat
{
    public static class Controllers
    {
        private static GpioController _controller = null;

        public static GpioController GPIOController
        {
            get
            {
                if (_controller == null)
                {
                    _controller = GpioController.GetDefault();
                }
                return _controller;
            }
        }

    }
}
