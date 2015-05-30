using AyanaWebzine.Lib.Model;
using AyanaWebzine.Lib.Data;
using AyanaWebzine.WinPhone.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Windows.Phone.UI.Input;

namespace AyanaWebzine.WinPhone.Views
{
    public partial class CategoryPage : Page
    {
        DataStore dataStore = DataStore.GetInstance();
        
        public CategoryPage()
        {
            InitializeComponent();
            this.DataContext = new CategoriesPageViewModel(true);

            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {                  
                int? selectedCategoryIndex = e.Parameter as int?;
                if (selectedCategoryIndex != null && selectedCategoryIndex >= 0)
                {                                               
                    this.CategoriesPivot.SelectedIndex = (int)selectedCategoryIndex;
                }
            }
        }

        private void ArticlesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView selectedCategory = sender as ListView;
            Article selectedArticle = selectedCategory.SelectedItem as Article;
            if (selectedArticle != null)
            {
                Frame.Navigate(typeof(ArticleDetails), selectedArticle);
                selectedCategory.SelectedItem = null;
            }
        }     
    }
}