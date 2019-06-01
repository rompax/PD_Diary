﻿using PD_Diary.Models;
using PD_Diary.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PD_Diary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Nutrient item))
            {
                return;
            }

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(null) { ReadOnly = false }));
            //await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}