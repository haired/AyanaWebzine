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
        // Constructeur
        public MainPage()
        {                                                                        
            InitializeComponent();
            this.DataContext = new MainPageViewModel();
            (this.DataContext as MainPageViewModel).LoadData(true);     

            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();
        }  

        private void ListCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = this.ListCategories.SelectedItem as Category;

            if (selectedCategory!= null)
            {
                this.ListCategories.SelectedItem = null;
                Frame.Navigate(typeof(CategoryPage), selectedCategory);
                //NavigationService.Navigate(new Uri("/CategoryPage.xaml?category=" + selectedCategory.ID.ToString(), UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode == NavigationMode.Back)
            {
                this.EditoPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            base.OnNavigatedTo(e);
        }

        
        //void HardwareButtons_BackPressed_edito(object sender, BackPressedEventArgs e)
        //{
        //    // If we are not on Edito page, cancel quitting the app and show the Edito
        //    if (this.EditoPage.Opacity != 1)
        //    {
        //        this.EditoPage.Opacity = 1;
        //        this.EditoPage.Visibility = Visibility.Visible;
        //        this.BottomAppBar.Visibility = Visibility.Collapsed;
        //        e.Handled = true;
        //    } 
        //}

        

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

        // Exemple de code pour la conception d'une ApplicationBar localisée
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Définit l'ApplicationBar de la page sur une nouvelle instance d'ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crée un bouton et définit la valeur du texte sur la chaîne localisée issue d'AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crée un nouvel élément de menu avec la chaîne localisée d'AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}