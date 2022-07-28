using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace PandaLyrics
{
    internal class AppSetting : INotifyPropertyChanged
    {
        private FontFamily fontFamily = Properties.Settings.Default.fontFamily != "" ?
            new FontFamily(Properties.Settings.Default.fontFamily) :
            SystemFonts.MessageFontFamily;
        private double fontSize = Properties.Settings.Default.fontSize;
        private string fontColor = Properties.Settings.Default.fontColor;
        private string shadowColor = Properties.Settings.Default.shadowColor;
        private bool bgVisible = Properties.Settings.Default.bgVisible;
        private bool appVisible = Properties.Settings.Default.appVisible;
        private double windowLeft = Properties.Settings.Default.windowLeft;
        private double windowTop = Properties.Settings.Default.windowTop;
        private double bgWidth = Properties.Settings.Default.bgWidth;
        private string bgColor = Properties.Settings.Default.bgColor;
        private double bgAlpha = Properties.Settings.Default.bgAlpha;
        private double winOpacity = Properties.Settings.Default.winOpacity;

        public AppSetting()
        {
            fontSize = Properties.Settings.Default.fontSize;
        }

        public double FontSize
        {
            get => fontSize;
            set
            {
                if (FontSize != value)
                {
                    fontSize = value;
                    Properties.Settings.Default.fontSize = fontSize;
                    OnPropertyChanged("FontSize");
                    OnPropertyChanged("FontSizeWpf");
                }
            }
        }
        public double FontSizeWpf
        {
            get => (double)new FontSizeConverter().ConvertFrom(fontSize + "pt");
        }
        public FontFamily FontFamily
        {
            get => fontFamily;
            set
            {
                if (FontFamily != value)
                {
                    fontFamily = value;
                    Properties.Settings.Default.fontFamily = fontFamily.Source;
                    OnPropertyChanged("FontFamily");
                }
            }
        }
        public Color FontColor
        {
            get => (Color)ColorConverter.ConvertFromString(fontColor);
            set
            {
                if (FontColor != value)
                {
                    fontColor = new ColorConverter().ConvertToString(value);
                    Properties.Settings.Default.fontColor = fontColor;
                    OnPropertyChanged("FontColor");
                    OnPropertyChanged("FontColorBrush");
                }
            }
        }
        public Brush FontColorBrush
        {
            get => new SolidColorBrush(FontColor);
        }
        public Color ShadowColor
        {
            get => (Color)ColorConverter.ConvertFromString(shadowColor);
            set
            {
                if (ShadowColor != value)
                {
                    shadowColor = new ColorConverter().ConvertToString(value);
                    Properties.Settings.Default.shadowColor = shadowColor;
                    OnPropertyChanged("ShadowColor");
                }
            }
        }
        public bool AppVisible
        {
            get => appVisible;
            set
            {
                if (AppVisible != value)
                {
                    appVisible = value;
                    Properties.Settings.Default.appVisible = appVisible;
                    OnPropertyChanged("AppVisible");
                }
            }
        }
        public bool BgVisible
        {
            get => bgVisible;
            set
            {
                if (BgVisible != value)
                {
                    bgVisible = value;
                    Properties.Settings.Default.bgVisible = bgVisible;
                    OnPropertyChanged("BgVisible");
                    OnPropertyChanged("BgColorBrush");
                }
            }
        }
        public Color BgColor
        {
            get => (Color)ColorConverter.ConvertFromString(bgColor);
            set
            {
                if (BgColor != value)
                {
                    bgColor = new ColorConverter().ConvertToString(value);
                    Properties.Settings.Default.bgColor = bgColor;
                    OnPropertyChanged("BgColor");
                    OnPropertyChanged("BgColorBrush");
                }
            }
        }
        public double BgAlpha
        {
            get => bgAlpha;
            set
            {
                if (BgAlpha != value)
                {
                    bgAlpha = value;
                    Properties.Settings.Default.bgAlpha = bgAlpha;
                    OnPropertyChanged("BgAlpha");
                    OnPropertyChanged("BgColorBrush");
                }
            }
        }
        public double WinOpacity
        {
            get => winOpacity;
            set
            {
                if (WinOpacity != value)
                {
                    winOpacity = value;
                    Properties.Settings.Default.winOpacity = winOpacity;
                    OnPropertyChanged("WinOpacity");
                }
            }
        }
        public Brush BgColorBrush
        {
            get
            {
                if (BgVisible)
                {
                    Brush brush = new SolidColorBrush(BgColor);
                    brush.Opacity = BgAlpha;
                    return brush;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
        }
        public double BgWidth
        {
            get => bgWidth;
            set
            {
                if (BgWidth != value)
                {
                    bgWidth = value;
                    Properties.Settings.Default.bgWidth = bgWidth;
                    OnPropertyChanged("BgWidth");
                }
            }
        }
        public double WindowLeft
        {
            get => windowLeft;
            set
            {
                if (WindowLeft != value)
                {
                    windowLeft = value;
                    Properties.Settings.Default.windowLeft = windowLeft;
                    OnPropertyChanged("WindowLeft");
                }
            }
        }
        public double WindowTop
        {
            get => windowTop;
            set
            {
                if (WindowTop != value)
                {
                    windowTop = value;
                    Properties.Settings.Default.windowTop = windowTop;
                    OnPropertyChanged("WindowTop");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
