using System;
using System.Threading.Tasks;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using TorizonGtkSharp.Services;

namespace TorizonGtkSharp.ViewModel
{
    class ControlsViewModel : ViewModel
    {
        [UI]
        private Box _box_menu = null;

        [UI]
        private Image _image_lamp = null;

        [UI]
        private Label _label_light = null;

        [UI]
        private ProgressBar _progress_light = null;

        [UI]
        private Button _button_blue = null;
        
        [UI]
        private Button _button_yellow = null;

        private double _sensor_level = 0d;
        public int SensorLevel { get; set; }

        // dotnet/iot
        GPIODeviceService gpioDevice;
        // ldr sensor
        LDRDeviceService ldrDevice;

        public ControlsViewModel (Window fatherWindow)
            : base(fatherWindow)
        {
            gpioDevice = new GPIODeviceService();
            ldrDevice = new LDRDeviceService();
        }

        public override void Init ()
        {
            // signals
            _button_blue.Clicked += ButtonBlueClicked;
            _button_yellow.Clicked += ButtonYellowClicked;

            // first signal on
            ldrDevice.NotifyDataChanged += OnNotifyDataChangedLDR;
            gpioDevice.NotifyButtonRedPressed += OnNotifyButtonPressed;
        }

        private void ButtonYellowClicked (object sender, EventArgs a)
        {
            gpioDevice.LEDYellowToggle();
        }

        private void ButtonBlueClicked (object sender, EventArgs a)
        {
            gpioDevice.LEDBlueToggle();
        }

        public Task OnNotifyDataChangedLDR()
        {
            return Task.Run(() => {
                // image opacity
                int x = ldrDevice.LevelPercentage / 10;
                _image_lamp.StyleContext.RemoveClass($"percent{SensorLevel * 10}");
                _image_lamp.StyleContext.AddClass($"percent{x * 10}");
                SensorLevel = x;
    
                // update the label and progress bar
                _progress_light.Fraction = ldrDevice.LevelPercentage / 100d;
                _label_light.Text = $"{ldrDevice.LevelPercentage} %";

                Console.WriteLine($"{ldrDevice.LevelPercentage} %");
            });
        }

        public Task OnNotifyButtonPressed()
        {
            return Task.Run(() => {
                _box_menu.StyleContext.AddClass("nav-bar-red");
            });
        }
    }
}
