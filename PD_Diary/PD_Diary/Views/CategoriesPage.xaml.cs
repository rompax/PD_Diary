using PD_Diary.Models;
using PD_Diary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PD_Diary.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriesPage : ContentPage
	{
        ItemsViewModel viewModel;

        public CategoriesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
                if (args.SelectedItem is Tuple<int, string> tuple)
                    await Navigation.PushModalAsync(new NavigationPage(new ItemsPage(tuple.Item1)));
                else
                    return;
            // Manually deselect item.
            //ItemsListView.SelectedItem = null;
        }

        void AddItem_Clicked(object sender, EventArgs e)
        {

            //await Navigation.PushModalAsync(new NavigationPage(new CategoryPage() { ReadOnly = false }));
           
        }


        public string CategoryName => "XX";
        protected override void OnAppearing()
        {
            CategoryList.ItemsSource = App.Database.GetItems<CategoryData>();

            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}