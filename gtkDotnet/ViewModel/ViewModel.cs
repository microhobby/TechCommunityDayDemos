using System;
using Gtk;

namespace TorizonGtkSharp.ViewModel
{
    abstract class ViewModel : IViewModel
    {
        public Window FatherWindow { get; set; }

        public ViewModel (Window fatherWindow)
        {
            this.FatherWindow = fatherWindow;
        }

        public abstract void Init ();
    }
}
