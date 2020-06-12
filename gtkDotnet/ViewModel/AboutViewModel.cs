using System;
using System.Runtime.InteropServices;
using Gtk;

namespace TorizonGtkSharp.ViewModel
{
    class AboutViewModel : ViewModel
    {
        public AboutViewModel (Window viewFather)
            : base(viewFather)
        { }

        public override void Init ()
        {
            // search for the classes
            checkAllChildren(FatherWindow);
        }

        private void checkAllChildren (Container child)
        {
            for (var i = 0; i < child.Children.Length; i++)
            {
                System.Console.WriteLine($"{child.Children[i].Name}");

                if (child.Children[i].StyleContext.HasClass("desc_arch"))
                {
                    if (child.Children[i] is Label archl)
                        archl.Text = $"{RuntimeInformation.OSArchitecture}";
                }

                if (child.Children[i].StyleContext.HasClass("desc_os"))
                {
                    if (child.Children[i] is Label osl)
                        osl.Text = $"{RuntimeInformation.OSDescription.ToString().Substring(0, 25)}";
                }

                if (child.Children[i].StyleContext.HasClass("desc_dotnet"))
                {
                    if (child.Children[i] is Label netl)
                        netl.Text = $"{RuntimeInformation.FrameworkDescription}";
                }

                // continue the search
                if (child.Children[i] is Container c)
                {
                    checkAllChildren(c);
                }
            }
        }
    }
}
