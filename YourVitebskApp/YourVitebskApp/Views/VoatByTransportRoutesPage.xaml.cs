﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.ViewModels;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoatByTransportRoutesPage : ContentPage
    {
        public VoatByTransportRoutesPage()
        {
            InitializeComponent();
            BindingContext = new VoatByTransportRoutesViewModel();
        }
    }
}