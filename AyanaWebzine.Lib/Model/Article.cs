using System;
using System.Collections.ObjectModel;

namespace AyanaWebzine.Lib.Model
{
    public class Article : AyanaWebzine.Lib.Common.BindableBase
    {
        //[JsonProperty(PropertyName="handle")]
        public string Handle { get; set; }

        private Guid id;
        public Guid Id
        {
            get { return id; }
            set { this.SetProperty(ref this.id, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { this.SetProperty(ref this.title, value); }
        }
        
        private string thumbnailUri;
        public string ThumbnailUri
        {
            get { return thumbnailUri; }
            set { this.SetProperty(ref this.thumbnailUri, value); }
        }

        private byte[] thumbnailBytes;  
        public byte[] ThumbnailBytes
        {
            get { return thumbnailBytes; }
            set { thumbnailBytes = value; }
        }       
        
        private string introduction;
        public string Introduction
        {
            get { return introduction; }
            set { this.SetProperty(ref this.introduction, value); }
        }
        
        private string content;
        public string Content
        {
            get { return content; }
            set { this.SetProperty(ref this.content, value); }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set { this.SetProperty(ref this.date, value); }
        }

        private ObservableCollection<CompositeContent> compositeContent;
        public ObservableCollection<CompositeContent> CompositeContent
        {
            get 
            {
                return compositeContent;
            }
            set { compositeContent = value; }
        }

        public Uri Url
        { get; set; }

        public Article()
        {
            this.Id = Guid.NewGuid();
        }
    }

    public class CompositeContent
    {
        public ContentType Type { get; set; }
        public string Content { get; set; }

        public byte[] ContentBinaryData { get; set; }      
    }

    public enum ContentType
    {
        Text,
        Image
    }
}
