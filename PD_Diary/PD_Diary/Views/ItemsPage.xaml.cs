using PD_Diary.Models;
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
        private int CategoryId;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        public ItemsPage(int id)
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
            CategoryId = id;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            Nutrient item;
                if (args.SelectedItem is Tuple<int, string> tuple)
                    item = Nutrient.Get(tuple.Item1);
                else
                    return;

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage() { ReadOnly = false }));
        }

        async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        public string CategoryName => "XX";
        protected override void OnAppearing()
        {
            if( CategoryId == 0)
            NutrientList.ItemsSource = App.Database.GetItems<Nutrient>();
            else
            {
                NutrientList.ItemsSource = App.Database.GetNutrientsByCategory(CategoryId);
            }
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}