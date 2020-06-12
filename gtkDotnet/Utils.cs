using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace TorizonGtkSharp
{
    class Utils
    {
        static public Gdk.RGBA RGBToGDKRGB (double red, double green,
                                             double blue)
        {
            var r = red / 255d;
            var g = green / 255d;
            var b = blue / 255d;

            var rgba = new Gdk.RGBA();
            rgba.Red = r;
            rgba.Green = g;
            rgba.Blue = b;
            rgba.Alpha = 1;

            return rgba;
        }
    }
}
