using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Text;

namespace TorizonGtkSharp.Services
{
    public class LDRDeviceService
    {
        const string IIO_PATH = "/sys/bus/iio/devices/iio:device0/in_voltage4_raw";

        private Thread LDRRoutine;
        public int LevelPercentage { get; set; }

        public LDRDeviceService ()
        {
            Console.WriteLine(File.ReadAllText(IIO_PATH));

            LDRRoutine = new Thread(() => {
                while (true)
                {
                    var val = Int32.Parse(File.ReadAllText(IIO_PATH));
                    val = (val * 100) / 3000;
                    val = Math.Abs(100 - val);

                    if (val != LevelPercentage)
                    {
                        LevelPercentage = val;
                        NotifyDataChanged?.Invoke();
                    }

                    // sleep 1s
                    Thread.Sleep(1000);
                }
            });

            LDRRoutine.Start();
        }

        public event Func<Task> NotifyDataChanged;
    }
}
