using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PandaLyrics.Component
{
    public partial class Lyrics : UserControl
    {
        private string content = "";

        public new string Content
        {
            get => content;
            set
            {
                content = value.Trim();
                UpdateContent();
            }
        }

        public Lyrics()
        {
            InitializeComponent();
        }

        private void UpdateContent()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(delegate
                {
                    lyricsText.Text = Content;
                }));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                DataContext = parentWindow.appSetting;
            }
        }
    }
}
