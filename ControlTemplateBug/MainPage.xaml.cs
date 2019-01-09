using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ControlTemplateBug
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<Model> Source;
        private TextBlock _text;

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _text = new TextBlock() { Text = "hello world" };

            Source = new ObservableCollection<Model>();
            for (int i = 0; i < 2000; i++)
            {
                Source.Add(new Model()
                {
                    txt = i.ToString()
                });
            }

            xamlListView.ItemsSource = Source;
        }

        private ViewItem _lastItem;
        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as Model;
            var index = Source.IndexOf(data);
            var next = Source.ElementAt(index + 1);

            Source.Remove(data);

            var item = xamlListView.ContainerFromItem(next) as ListViewItem;
            var viewitem = (item.ContentTemplateRoot as StackPanel).Children[1] as ViewItem;

            if (_lastItem != viewitem)
            {
                _lastItem?.Remove();

                _lastItem = viewitem;
                _lastItem.Add(_text);
            }
        }
    }


    class ViewItem : Windows.UI.Xaml.Controls.Control
    {
        private ContentPresenter _placeholder;

        public ViewItem()
        {
            DefaultStyleKey = typeof(ViewItem);
        }

        protected override void OnApplyTemplate()
        {
            if (_placeholder == null)
            {
                _placeholder = GetTemplateChild("Placeholder") as ContentPresenter;
            }
            else
            {
                Debug.WriteLine("OnApplyTemplate " + GetHashCode());
            }

            base.OnApplyTemplate();
        }

        public void Add(UIElement element)
        {
            _placeholder.Content = element;
        }

        public void Remove()
        {
            _placeholder.Content = null;
        }
    }

    class Model
    {
        public string txt { get; set; }
    }
}
