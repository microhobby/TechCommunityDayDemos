using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using TorizonGtkSharp;
using TorizonGtkSharp.ViewModel;

namespace TorizonGtkSharp
{
    class MainWindow : Window
    {
        [UI]
        private Notebook _notebook_pages = null;

        [UI]
        private Button _button_menu_control = null;

        [UI]
        private Button _button_menu_about = null;

        // bind all
        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder)
            : base(builder.GetObject("MainWindow").Handle)
        {
            // custom css
            var provider = new CssProvider();
            provider.LoadFromPath("Assets/app.css");
            Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, provider, 800);

            // window views
            var controlsViewModel = new ControlsViewModel(this);
            var aboutViewModel = new AboutViewModel(this);

            builder.Autoconnect(this);
            builder.Autoconnect(controlsViewModel);

            // events
            DeleteEvent += Window_DeleteEvent;

            // menu events
            _button_menu_control.Clicked += ButtonMenuControlClicked;
            _button_menu_about.Clicked += ButtonMenuAboutClicked;

            controlsViewModel.Init();
            aboutViewModel.Init();
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
        private void ButtonMenuControlClicked (object sender, EventArgs a)
        {
            _notebook_pages.Page = 0;
        }

        private void ButtonMenuAboutClicked (object sender, EventArgs a)
        {
            _notebook_pages.Page = 1;
        }
    }
}
