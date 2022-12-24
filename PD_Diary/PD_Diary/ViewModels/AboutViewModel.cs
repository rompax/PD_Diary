using System;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace PD_Diary.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() =>  Launcher.OpenAsync(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}