﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.ViewModels
{
    internal class SpecificNewsViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private News _news;
        private bool _isLinkAvailable;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private readonly NewsService _newsService;
        public event PropertyChangedEventHandler PropertyChanged;
        public AsyncCommand<string> TapCommand { get; set; }
        public int NewsId { get; set; }

        public News News
        {
            get { return _news; }
            set
            {
                _news = value;
                OnPropertyChanged();
            }
        }

        public bool IsLinkAvailable
        {
            get { return _isLinkAvailable; }
            set
            {
                _isLinkAvailable = value;
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
            }
        }

        public SpecificNewsViewModel()
        {
            IsBusy = true;
            _newsService = new NewsService();
            TapCommand = new AsyncCommand<string>(OpenURL);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsBusy = false;
        }

        private async Task LoadData()
        {
            News = await _newsService.Get(NewsId);
            IsLinkAvailable = News.ExternalLink != null;
        }

        private async Task OpenURL(string url)
        {
            await Browser.OpenAsync(url, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show
            });
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            IsBusy = true;
            if (query.TryGetValue("NewsId", out string param))
            {
                int.TryParse(param, out int id);
                NewsId = id;
                await LoadData();
            }

            IsBusy = false;
        }
    }
}
