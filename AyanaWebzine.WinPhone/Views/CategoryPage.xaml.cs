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
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {                   
                //string selectedCategoryId = NavigationContext.QueryString["category"];
                //var selectedCategory = dataStore.Categories.FirstOrDefault(c => c.ID.ToString() == selectedCategoryId); //DataSource.GetCategory(Guid.Parse(selectedCategoryId));
                Category selectedCategory = e.Parameter as Category;
                if(selectedCategory != null)
                    this.CategoriesPivot.SelectedIndex = dataStore.Categories.IndexOf(selectedCategory);                     
            }
            
        }
          
        private void ArticlesList_Tap(object sender, TappedRoutedEventArgs e)
        {
            ListBox articlesList = ((ListBox)sender);
            var selectedArticle = articlesList.SelectedItem as Article;
            if(selectedArticle != null)
            {
                articlesList.SelectedItem = null;
                Frame.Navigate(typeof(ArticleDetails), selectedArticle);
                //NavigationService.Navigate(new Uri("/ArticleDetails.xaml?&articleId=" + selectedArticle.Id, UriKind.Relative));
            }
        }     
    }
}