using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Device.I2c;
using Iot.Device.Ssd13xx;
using Iot.Device.Ssd13xx.Commands;
using Iot.Device.Ssd13xx.Samples;
using Ssd1306Cmnds = Iot.Device.Ssd13xx.Commands.Ssd1306Commands;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace i2cTest
{
    internal static class Program6432
    {
        private static I2cDevice GetI2CDevice()
        {
            Console.WriteLine("Using I2C protocol");

            var connectionSettings = new I2cConnectionSettings(0, 0x3C);
            return I2cDevice.Create(connectionSettings);
        }

        private static Ssd1306 GetSsd1306WithI2c()
        {
            return new Ssd1306(GetI2CDevice());
        }

        private static void Initialize(Ssd1306 device)
        {
            device.SendCommand(new SetDisplayOff());
            device.SendCommand(new Ssd1306Cmnds.SetDisplayClockDivideRatioOscillatorFrequency(0x00, 0x08));
            // OLED 64x32
            device.SendCommand(new SetMultiplexRatio(0x1f));
            device.SendCommand(new Ssd1306Cmnds.SetDisplayOffset(0x00));
            device.SendCommand(new Ssd1306Cmnds.SetDisplayStartLine(0x00));
            device.SendCommand(new Ssd1306Cmnds.SetChargePump(true));
            device.SendCommand(new Ssd1306Cmnds.SetMemoryAddressingMode(Ssd1306Cmnds.SetMemoryAddressingMode.AddressingMode.Horizontal));
            
            // OLED 64x32
            device.SendCommand(new Ssd1306Cmnds.SetSegmentReMap(true));
            // C0H
            device.SendCommand(new Ssd1306Cmnds.SetComOutputScanDirection(false));
            device.SendCommand(new Ssd1306Cmnds.SetComPinsHardwareConfiguration(true, false));

            device.SendCommand(new SetContrastControlForBank0(0x8F));          
            device.SendCommand(new Ssd1306Cmnds.SetPreChargePeriod(0x01, 0x0F));
            device.SendCommand(new Ssd1306Cmnds.SetVcomhDeselectLevel(Ssd1306Cmnds.SetVcomhDeselectLevel.DeselectLevel.Vcc1_00));
            device.SendCommand(new Ssd1306Cmnds.EntireDisplayOn(false));
            device.SendCommand(new Ssd1306Cmnds.SetNormalDisplay());
            device.SendCommand(new SetDisplayOn());
            
            // OLED 64x32
            ResetPageColumnAddress(device);
            ClearScreen(device);
            //ResetPageColumnAddress(device);
        }

        private static void ResetPageColumnAddress (Ssd1306 device)
        {
            device.SendCommand(new Ssd1306Cmnds.SetColumnAddress(0x20, 0x5f));
            device.SendCommand(new Ssd1306Cmnds.SetPageAddress(Ssd1306Cmnds.PageAddress.Page0, Ssd1306Cmnds.PageAddress.Page3));
        }

        private static void ClearScreen(Ssd1306 device)
        {
            // OLED 64x32
            // for each page
            for (int i = 0; i < 4; i++) {
                device.SendData(new byte[64]);
                // for each column
                /*for (int j = 0; j < 64; j++) {
                    device.SendData(new byte[] {0x00});
                }*/
            }
        }

        private static void SendMessage(Ssd1306 device, string message)
        {
            foreach (char character in message)
            {
                device.SendData(BasicFont.GetCharacterBytes(character));
            }
        }

        private static void SendMessageLine(Ssd1306 device, string message)
        {
            int bytesSend = 0;
            
            foreach (char character in message)
            {
                byte[] chars = BasicFont.GetCharacterBytes(character);
                device.SendData(chars);
                bytesSend += chars.Length;
            }

            int bytesDiff = Math.Abs(bytesSend - 64);
            byte[] bytesComplete = new byte[bytesDiff];
            device.SendData(bytesComplete);
        }

        private static void SendImages (Ssd1306 device)
        {
            foreach (var image_name in Directory.GetFiles(".", "*.bmp").OrderBy(f => f))
            {
                using (Image<Gray16> image = Image.Load<Gray16>(image_name))
                {
                    device.DisplayImage(image);
                    Thread.Sleep(1000);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (Ssd1306 device = GetSsd1306WithI2c())
            {
                Initialize(device);
                //SendMessage(device, "Hello .NET IoT!!!");
                SendMessageLine(device, "Tech Community");
                //SendMessageLine(device, "PAGE 1");
                //SendMessageLine(device, "PAGE 2");
                //SendImages(device);

                // for each page
                //for (int i = 0; i < 4; i++) {
                //    // for each column
                //    for (int j = 0; j < 64; j++) {
                //        device.SendData(new byte[] {0xff});
                //    }
                //    Console.WriteLine($"Page {i}");
                //    Thread.Sleep(100);
                //}

                

                Thread.Sleep(10000);
            }
        }

        internal static void DisplayImage(this Ssd1306 s, Image<Gray16> image)
        {
            Int16 width = 64;
            Int16 pages = 4;
            List<byte> buffer = new List<byte>();

            for (int page = 0; page < pages; page++)
            {
                for (int x = 0; x < width; x++)
                {
                    int bits = 0;
                    for (byte bit = 0; bit < 8; bit++)
                    {
                        bits = bits << 1;
                        bits |= image[x, page * 8 + 7 - bit].PackedValue > 0 ? 1 : 0;
                    }

                    buffer.Add((byte)bits);
                }
            }

            int chunk_size = 16;
            for (int i = 0; i < buffer.Count; i += chunk_size)
            {
                s.SendData(buffer.Skip(i).Take(chunk_size).ToArray());
            }
        }
    }
}
