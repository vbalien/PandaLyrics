using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PandaLyrics.Component
{
    public partial class Lyrics : UserControl
    {
        private FontFamily fontFamily = Properties.Settings.Default.fontFamily != "" ?
            new FontFamily(Properties.Settings.Default.fontFamily) :
            SystemFonts.MessageFontFamily;
        private double fontSize = Properties.Settings.Default.fontSize;
        private string fontColor = Properties.Settings.Default.fontColor;
        private string shadowColor = Properties.Settings.Default.shadowColor;
        private string content = "준비중...";

        public new double FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                UpdateStyle();
            }
        }
        public Color FontColor
        {
            get => (Color)ColorConverter.ConvertFromString(fontColor);
            set
            {
                fontColor = new ColorConverter().ConvertToString(value);
                UpdateStyle();
            }
        }
        public Color ShadowColor
        {
            get => (Color)ColorConverter.ConvertFromString(shadowColor);
            set
            {
                shadowColor = new ColorConverter().ConvertToString(value);
                UpdateStyle();
            }
        }
        public new string Content
        {
            get => content;
            set
            {
                content = value;
                UpdateContent();
            }
        }
        public new FontFamily FontFamily
        {
            get => fontFamily; set
            {
                fontFamily = value;
                UpdateStyle();
            }
        }

        public Lyrics()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateStyle();
        }
        private void UpdateContent()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(delegate
                {
                    lyricsText.Text = Content;
                }));
        }

        private void UpdateStyle()
        {
            lyricsText.FontSize = FontSize;
            lyricsText.FontFamily = FontFamily;
            lyricsText.Foreground = new SolidColorBrush(FontColor);
            ((DropShadowEffect)lyricsText.Effect).Color = ShadowColor;
        }
    }
}
