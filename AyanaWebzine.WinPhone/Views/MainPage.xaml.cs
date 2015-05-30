using AyanaWebzine.Lib.Model;
using AyanaWebzine.WinPhone.ViewModels;
using System;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AyanaWebzine.WinPhone.Views
{
    public partial class MainPage : Page
    {   
        public MainPage()
        {                                                                        
            InitializeComponent();
            this.DataContext = new MainPageViewModel();
            (this.DataContext as MainPageViewModel).LoadData(true);

            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }  

        private void ListCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategoryIndex = this.ListCategories.SelectedIndex;
            if (selectedCategoryIndex >= 0)
            {
                Frame.Navigate(typeof(CategoryPage), selectedCategoryIndex);
                this.ListCategories.SelectedIndex = -1;                      
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode == NavigationMode.Back)
            {
                this.EditoPage.Visibility = Visibility.Collapsed;
                this.BottomAppBar.Visibility = Visibility.Visible;
            }
            base.OnNavigatedTo(e);
        }
        
        private void Reload(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainPageViewModel).ReloadArticles();             
        }

        private void EditoPage_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += (s, args) =>
            {
                if (this.EditoPage.Opacity > 0)
                {
                    this.EditoPage.Opacity -= (1.0 / 10);
                }
                else
                {
                    this.EditoPage.Visibility = Visibility.Collapsed;
                    this.BottomAppBar.Visibility = Visibility.Visible;
                    timer.Stop();
                }
            };
            timer.Start();
        }         
    }
}