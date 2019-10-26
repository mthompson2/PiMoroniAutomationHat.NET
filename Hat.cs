using System;
using Windows.Devices;
using Windows.System.Threading;
using static Pimoroni.AutomationHat.SN3218LEDDriver;

namespace Pimoroni.AutomationHat
{
    public static class Hat
    {
        private static ThreadPoolTimer _refreshTimer = null;
        private static SN3218LEDDriver _ledDriver;
        private static ADS1015 _a2dController;

        private static Led _power;
        private static Led _comms;
        private  static Led _warn;

        
        public static bool PowerLED
        {
            get
            {
                return _power.State;
            }
            set
            {
                _power.State = value;
            }
        }

        public static bool CommsLED
        {
            get
            {
                return _comms.State;
            }
            set
            {
                _comms.State = value;
            }
        }

        public static bool WarnLED
        {
            get
            {
                return _warn.State;
            }
            set
            {
                _warn.State = value;
            }
        }

        public static Relay Relay1 { get; private set; }
        public static Relay Relay2 { get; private set; }
        public static Relay Relay3 { get; private set; }


        public static Output Output1 { get; private set; }
        public static Output Output2 { get; private set; }
        public static Output Output3 { get; private set; }

        public static Input Input1 { get; private set; }
        public static Input Input2 { get; private set; }
        public static Input Input3 { get; private set; }


        public static AnalogInput AnalogInput1 { get { return _a2dController[0]; } }
        public static AnalogInput AnalogInput2 { get { return _a2dController[1]; } }

        public static AnalogInput AnalogInput3 { get { return _a2dController[2]; } }

        public static AnalogInput AnalogInput4 { get { return _a2dController[3]; } }
    
        public static async void Init()
        {

            try
            {
      

                _power = new Led(17);
                _comms = new Led(16);
                _warn = new Led(15);

                Relay1 = new Relay(13, 6, 7);
                Relay2 = new Relay(19, 8, 9);
                Relay3 = new Relay(16, 10, 11);

                Output1 = new Output(5, 3);
                Output2 = new Output(12, 4);
                Output3 = new Output(6, 5);


                Input1 = new Input(26, 14, true, Controllers.GPIOController);
                Input2 = new Input(20, 13, true, Controllers.GPIOController);
                Input3 = new Input(21, 12, true, Controllers.GPIOController);

                _a2dController[0] = new AnalogInput(0, 25.85, new Led(0)); ;
                _a2dController[1] = new AnalogInput(1, 25.85, new Led(1)); ;
                _a2dController[2] = new AnalogInput(2, 25.85, new Led(2)); ;
                _a2dController[3] = new AnalogInput(3, 3.3, null);


                // Turn the power light on
                _ledDriver = await SN3218LEDDriver.Open();
                _ledDriver.Enable();
                _ledDriver.EnableLeds();

                _a2dController = await ADS1015.Open();


                _refreshTimer = ThreadPoolTimer.CreatePeriodicTimer(x => Tick(), TimeSpan.FromMilliseconds(20));


            }
            catch (Exception ex)
            {
            }
        }

        public static bool IsGPIOSupported
        {
            get
            {
                return Controllers.GPIOController != null;
            }
        }


        private static void Tick()
        {
            _ledDriver.Refresh();
            _a2dController.ReadVoltages();

        }
        
    }
}
