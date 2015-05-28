using System;
using Windows.UI.Xaml.Controls;
using AyanaWebzine.Lib.Model;
using Windows.UI.Xaml.Navigation;
using AyanaWebzine.Lib.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;


namespace AyanaWebzine.WinPhone.Views
{
    public partial class ArticleDetails : Page
    {
        Article selectedArticle;

        public ArticleDetails()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Article selectedArticle = e.Parameter as Article;

            DataTransferManager shareTask = DataTransferManager.GetForCurrentView();
            shareTask.DataRequested += shareTask_DataRequested;
            if(selectedArticle != null)
            {
                this.DataContext = selectedArticle;
                CreateArticles(selectedArticle);
            }
        }

        void shareTask_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.SetWebLink(this.selectedArticle.Url);
            args.Request.Data.Properties.Title = string.Format("Partagé depuis l'application Windows Phone -- {0}", selectedArticle.Title);
            args.Request.Data.Properties.Description = selectedArticle.Introduction;
        }  


        private void CreateArticles(Article article)
        {   
            article.CompositeContent = ContentCreator.CreateCompositeContent(article.Content);
            //Creation of the composite content
            foreach (CompositeContent paragraph in article.CompositeContent)
            {
                if (paragraph.Type == ContentType.Text)
                {
                    articleStack.Children.Add(new TextBlock()
                    {
                        Text = paragraph.Content,
                        TextWrapping = TextWrapping.Wrap,
                        FontFamily = new FontFamily("Segoe UI Light"),
                        FontSize = 18,
                        Foreground = new SolidColorBrush(Colors.Black)
                    });
                }
                else if (paragraph.Type == ContentType.Image)
                {
                    articleStack.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri(paragraph.Content, UriKind.RelativeOrAbsolute)),
                        Stretch = Stretch.Uniform
                    });
                }
            }

            this.selectedArticle = article;
        }

        private void Share(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void SeeOnWeb(object sender, RoutedEventArgs e)
        {
            if (selectedArticle != null)
            {
                Launcher.LaunchUriAsync(selectedArticle.Url); 
            }
        }
    }
}