﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using YourVitebskApp.Models;
using YourVitebskApp.Services;
using YourVitebskApp.Views;

namespace YourVitebskApp.ViewModels
{
    public class CafesViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Cafe> _cafesList;
        private bool _isBusy;
        private bool _isRefreshing;
        private readonly CafeService _cafesService;
        public AsyncCommand<Cafe> ItemTappedCommand { get; }
        public Command RefreshCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Cafe> CafesList
        {
            get { return _cafesList; }
            set
            {
                _cafesList = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
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

        public CafesViewModel()
        {
            IsBusy = true;
            _cafesService = new CafeService();
            ItemTappedCommand = new AsyncCommand<Cafe>(ItemTapped);
            RefreshCommand = new Command(Refresh);
            AddData();
            IsBusy = false;
        }

        private async void AddData()
        {
            CafesList = await _cafesService.Get();
        }

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private async Task ItemTapped(Cafe cafe)
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"{nameof(CafesPage)}/{nameof(SpecificCafePage)}?CafeId={cafe.CafeId}");
            IsBusy = false;
        }

        private void Refresh()
        {
            IsRefreshing = true;
            AddData();
            IsRefreshing = false;
        }
    }
}