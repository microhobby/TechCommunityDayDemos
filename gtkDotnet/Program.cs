using System;
using System.IO;
using System.Threading;
using Gtk;

namespace TorizonGtkSharp
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var dp = Environment.GetEnvironmentVariable("WAYLAND_DISPLAY");
            Console.WriteLine($"We will try to connect to {dp}");

            Application.Init();
            
            var app = new Application("org.TorizonGtkSharp.TorizonGtkSharp",
                                      GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            win.Fullscreen();
            Application.Run();
        }
    }
}
