using PandaLyrics.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using JLyrics;
using System.Drawing;
using WebSocketSharp;
using WebSocketSharp.Server;
using PandaLyrics.Websocket;
using System.Diagnostics;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace PandaLyrics
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int LWA_ALPHA = 0x2;
        public const int LWA_COLORKEY = 0x1;
        private uint DEFAULT_FLAG;
        IntPtr wHandle;

        static int PORT = 8999;
        private NotifyIcon ni = new NotifyIcon();
        private bool moveMode = false;
        private JLyrics.Lyrics fJL = new JLyrics.Lyrics();
        private Component.Lyrics lyrics = new Component.Lyrics();
        private WebSocketServer wssv;
        private System.Windows.Forms.MenuItem lyricSelectMenu;
        private LyricInfo lyricInfo;
        private uint prevTime = 0;

        private class LyricEntity
        {
            public uint time;
            public string content;
        };
        private List<LyricEntity> lyricEntities = new List<LyricEntity>();

        public MainWindow()
        {
            InitializeComponent();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        private void SetNotification()
        {
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            ni.Visible = true;
            ni.Text = "Panda Lyrics";

            // Set context menu
            ni.ContextMenu = new System.Windows.Forms.ContextMenu();
            ni.ContextMenu.MenuItems.Add("숨기기/보이기").Click += ToggleWindow;
            lyricSelectMenu = ni.ContextMenu.MenuItems.Add("가사 선택");
            ni.ContextMenu.MenuItems.Add("위치 이동").Click += ToggleMoveMode;
            ni.ContextMenu.MenuItems.Add("폰트 변경").Click += ChangeFont;
            ni.ContextMenu.MenuItems.Add("그림자 색 변경").Click += ChangeShadow;
            ni.ContextMenu.MenuItems.Add("종료").Click += (sender, e) => this.Close();

            lyricSelectMenu.MenuItems.Add("비어있음").Enabled = false;

            ni.DoubleClick += (sender, e) => ni.ContextMenu.MenuItems[0].PerformClick();
            ni.ContextMenu.MenuItems[0].DefaultItem = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.windowLeft != -1 && Properties.Settings.Default.windowTop != -1)
            {
                this.Left = Properties.Settings.Default.windowLeft;
                this.Top = Properties.Settings.Default.windowTop;
            }
            SetNotification();
            stackPanel.Children.Add(lyrics);

            this.LocationChanged += this.Window_LocationChanged;

            SetupServer();

            wHandle = new WindowInteropHelper(this).Handle;
            DEFAULT_FLAG = GetWindowLong(wHandle, GWL_EXSTYLE);
            setClickThruAble(true);
        }

        private void setClickThruAble(bool value)
        {
            if (value)
            {
                SetWindowLong(wHandle, GWL_EXSTYLE,
                    (IntPtr)(DEFAULT_FLAG | WS_EX_LAYERED | WS_EX_TRANSPARENT));
            }
            else
            {
                SetWindowLong(wHandle, GWL_EXSTYLE, (IntPtr)DEFAULT_FLAG);
            }
        }

        private void SetupServer()
        {
            try
            {
                wssv = new WebSocketServer(System.Net.IPAddress.Loopback, PORT);
                wssv.AddWebSocketService("/pandaLyrics", () =>
                {
                    var lyricsReceiver = new LyricsReceiver();
                    lyricsReceiver.TickEvent += this.TickEvent;
                    lyricsReceiver.SongChangedEvent += this.SongChangedEvent;
                    return lyricsReceiver;
                });
                wssv.Start();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private string Escape(string value)
        {
            return value.Replace("&", "&amp;");
        }

        private void LoadLyric(int lyricID)
        {
            Regex rx = new Regex(@"(?:\[(?<min>\d\d):(?<sec>\d\d\.\d\d)\])(?<content>.*)",
              RegexOptions.Compiled | RegexOptions.IgnoreCase);

            lyricEntities.Clear();
            lyrics.Content = "\n가사를 불러오는 중...\n";
            uint prevTime = 0;

            lyricInfo = fJL.GetLyricsFromID(lyricID);
            MatchCollection matches = rx.Matches(lyricInfo.Lyric);

            foreach (Match match in matches)
            {

                int min = int.Parse(match.Groups["min"].Value);
                float sec = float.Parse(match.Groups["sec"].Value);
                uint msec = ((uint)((min * 60 + sec) * 1000.0f));
                string content = match.Groups["content"].Value;

                if (lyricEntities.Count > 0 && msec == prevTime)
                {
                    lyricEntities.Last().content += "\n" + content;
                    continue;
                }
                lyricEntities.Add(new LyricEntity
                {
                    content = content,
                    time = msec
                });
                prevTime = msec;
            }
            lyricEntities.Reverse();
        }

        private void TickEvent(object sender, LyricsReceiver.TickEventArgs e)
        {
            uint time = e.Time;

            if (time == prevTime)
            {
                return;
            }

            foreach (LyricEntity entity in lyricEntities)
            {
                if (entity.time == 0)
                {
                    continue;
                }
                if (entity.time <= time)
                {
                    lyrics.Content = entity.content;
                    break;
                }
            }

            prevTime = time;
        }
        private void SongChangedEvent(object sender, LyricsReceiver.SongChangedEventArgs e)
        {
            lyricEntities.Clear();
            lyricSelectMenu.MenuItems.Clear();
            List<LyricBasicInfo> lyricList;
            prevTime = 0;

            try
            {
                lyrics.Content = "\n가사를 검색중...\n";
                try
                {
                    lyricList = fJL.GetLyricsSearch(Escape(e.Artist), Escape(e.Title));
                }
                catch
                {
                    lyricList = fJL.GetLyricsSearch(string.Empty, Escape(e.Title));
                }
                Debug.WriteLine(lyricList);

                if (lyricList.Count <= 0)
                {
                    throw new Exception("검색된 가사가 없습니다.");
                }

                foreach (var lyric in lyricList)
                {
                    lyricSelectMenu.MenuItems.Add(lyric.Title + lyric.Artist + "[" + lyric.Album + "]").Click += (s, ev) =>
                    {
                        System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)s;

                        foreach (System.Windows.Forms.MenuItem menu in lyricSelectMenu.MenuItems)
                        {
                            menu.Checked = false;
                        }
                        item.Checked = true;
                        LoadLyric(lyric.LyricID);
                    };
                }

                lyricSelectMenu.MenuItems[0].PerformClick();
            }
            catch
            {
                lyricSelectMenu.MenuItems.Clear();
                lyricSelectMenu.MenuItems.Add("비어있음").Enabled = false;
                lyrics.Content = "\n가사를 찾지 못했습니다.\n";
                return;
            }

        }

        private void ToggleWindow(object sender, EventArgs e)
        {
            if (this.IsVisible)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }
        private void ToggleMoveMode(object sender, EventArgs e)
        {
            var item = sender as System.Windows.Forms.MenuItem;
            if (this.moveMode)
            {
                moveMode = false;
                Properties.Settings.Default.Save();
                this.Cursor = null;
                this.Background = System.Windows.Media.Brushes.Transparent;
                setClickThruAble(true);
                item.Text = "위치 이동";
            }
            else
            {
                moveMode = true;
                this.Cursor = System.Windows.Input.Cursors.SizeAll;
                this.Background = System.Windows.Media.Brushes.Black;
                setClickThruAble(false);
                item.Text = "위치 이동 끝내기";
            }
        }
        private void ChangeFont(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();

            fd.ShowColor = true;

            System.Windows.Media.Color mColor = lyrics.FontColor;
            System.Drawing.Color dColor = System.Drawing.Color.FromArgb(mColor.A, mColor.R, mColor.G, mColor.B);

            fd.Font = new System.Drawing.Font(
               lyrics.FontFamily.Source,
               (float)lyrics.FontSize,
               System.Drawing.FontStyle.Bold
            );
            fd.Color = dColor;

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                float fontSize = fd.Font.SizeInPoints;
                string hexColor = ColorTranslator.ToHtml(fd.Color);
                lyrics.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                lyrics.FontSize = fontSize;
                lyrics.FontColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColor);

                Properties.Settings.Default.fontFamily = fd.Font.Name;
                Properties.Settings.Default.fontSize = fontSize;
                Properties.Settings.Default.fontColor = hexColor;
                Properties.Settings.Default.Save();
            }
        }
        private void ChangeShadow(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string hexColor = ColorTranslator.ToHtml(cd.Color);
                lyrics.ShadowColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColor);
                Properties.Settings.Default.shadowColor = hexColor;
                Properties.Settings.Default.Save();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (moveMode && e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ni.Visible = false;
            ni.Dispose();
            if (wssv != null)
            {
                wssv.Stop();
            }
            Properties.Settings.Default.Save();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.windowLeft = this.Left;
            Properties.Settings.Default.windowTop = this.Top;
        }
    }
}
