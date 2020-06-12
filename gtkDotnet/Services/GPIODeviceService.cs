using System;
using System.Linq;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;

namespace TorizonGtkSharp.Services
{
    public class GPIODeviceService
    {
        private const int LED_BLUE_PIN = 6;
        private const int LED_YELLOW_PIN = 5;
        private const int BUTTON_RED_PIN = 4;

        private PinValue ledBlueState = PinValue.Low;
        public PinValue LEDBlueState
        { 
            get
            {
                return ledBlueState;
            }
            
            set
            {
                ledBlueState = value;
                gpiochip1.Write(LED_BLUE_PIN, ledBlueState);
            }
        }

        private PinValue ledYellowState = PinValue.Low;
        public PinValue LEDYellowState
        {
            get
            {
                return ledYellowState;
            }
            
            set
            {
                ledYellowState = value;
                gpiochip1.Write(LED_YELLOW_PIN, ledYellowState);
            }
        }

        public PinValue ButtonRed { get; set; }

        private LibGpiodDriver libGpiod;
        private GpioController gpiochip1;

        public GPIODeviceService ()
        {
            // reference to /dev/gpiochip1
            libGpiod = new LibGpiodDriver(1);
            // use reference on this controller
            gpiochip1 = new GpioController(PinNumberingScheme.Logical, libGpiod);

            // open output pins
            gpiochip1.OpenPin(LED_BLUE_PIN, PinMode.Output);
            gpiochip1.OpenPin(LED_YELLOW_PIN, PinMode.Output);

            // open input pins
            gpiochip1.OpenPin(BUTTON_RED_PIN, PinMode.Input);

            gpiochip1.RegisterCallbackForPinValueChangedEvent(BUTTON_RED_PIN,
                PinEventTypes.Rising,
                (sender, pinValueChangedEventArgs) => {
                    NotifyButtonRedPressed?.Invoke();
                });
        }

        public event Func<Task> NotifyButtonRedPressed;

        public void LEDYellowToggle ()
        {
            if (LEDYellowState == PinValue.High)
                LEDYellowState = PinValue.Low;
            else
                LEDYellowState = PinValue.High;
        }

        public void LEDBlueToggle ()
        {
            if (LEDBlueState == PinValue.High)
                LEDBlueState = PinValue.Low;
            else
                LEDBlueState = PinValue.High;
        }
    }
}
