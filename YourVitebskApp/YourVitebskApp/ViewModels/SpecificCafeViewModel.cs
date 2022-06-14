﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class SpecificCafeViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private IEnumerable<Comment> _commentsList;
        private Cafe _cafe;
        private bool _isBusy;
        private bool _isMainLayoutVisible;
        private bool _isInternetNotConnected;
        private readonly CafeService _cafeService;
        private readonly CommentService _commentService;
        public event PropertyChangedEventHandler PropertyChanged;
        public Command TapCommand { get; set; }
        public Command AddCommentCommand { get; set; }
        public int CafeId { get; set; }

        public IEnumerable<Comment> CommentsList
        {
            get { return _commentsList; }
            set
            {
                _commentsList = value;
                OnPropertyChanged();
            }
        }

        public Cafe Cafe
        {
            get { return _cafe; }
            set
            {
                _cafe = value;
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

        public SpecificCafeViewModel()
        {
            IsBusy = true;
            _cafeService = new CafeService();
            _commentService = new CommentService();
            TapCommand = new Command<string>(async (url) => await Launcher.OpenAsync(url));
            AddCommentCommand = new Command(AddComment);
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            IsInternetNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
            IsBusy = false;
        }

        private async void LoadData()
        {
            Cafe = await _cafeService.Get(CafeId);
            CommentsList = await _commentService.Get(1, CafeId, 0, 10);
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsInternetNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.TryGetValue("CafeId", out string param))
            {
                int.TryParse(param, out int id);
                CafeId = id;
                LoadData();
            }
        }

        public async void AddComment()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(SpecificCafePage)}/{nameof(AddCommentPage)}?ServiceId={1}&CafeId={CafeId}");
            IsBusy = false;
        }
    }
}
