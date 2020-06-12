using System;
using Gtk;

namespace TorizonGtkSharp.ViewModel
{
    interface IViewModel
    {
        void Init();
        Window FatherWindow { get; set; }
    }
}
