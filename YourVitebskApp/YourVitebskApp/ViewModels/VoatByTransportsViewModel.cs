﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class VoatByTransportsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<VoatByTransportData> _transportList;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private bool _isRefreshing;
        private readonly TransportSheduleService _transportSheduleService;
        public AsyncCommand<VoatByTransportItem> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<VoatByTransportData> TransportList
        {
            get { return _transportList; }
            set
            {
                _transportList = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                IsMainLayoutVisible = !_isBusy;
                OnPropertyChanged();
            }
        }

        public bool IsMainLayoutVisible
        {
            get { return _isMainLayoutVisible; }
            set
            {
                _isMainLayoutVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsInternetNotConnected
        {
            get { return _isInternetNotConnected; }
            set
            {
                _isInternetNotConnected = value;
                OnPropertyChanged();
                IsMainLayoutVisible = !IsInternetNotConnected;
                IsBusy = !IsInternetNotConnected;
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public VoatByTransportsViewModel()
        {
            IsBusy = true;
            _transportSheduleService = new TransportSheduleService();
            ItemTappedCommand = new AsyncCommand<VoatByTransportItem>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            LoadData();
            IsBusy = false;
        }

        private async void LoadData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                IsBusy = true;
                try
                {
                    TransportList = await _transportSheduleService.GetTransportsInfo();
                }
                catch
                {

                }

                IsBusy = false;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadData();
            IsRefreshing = false;
        }

        private async Task ItemTapped(VoatByTransportItem sender)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(VoatByTransportRoutesPage)}?TransportType={sender.vid_tr}&TransportId={sender.mar_id}");
            IsBusy = false;
        }
    }
}
