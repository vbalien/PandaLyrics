using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Reflection;
using JLyrics;
using WebSocketSharp;
using WebSocketSharp.Server;
using PandaLyrics.Websocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Diagnostics;

namespace PandaLyrics
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        static int PORT = 8999;
        private NotifyIcon ni = new NotifyIcon();
        private bool moveMode = false;
        private Lyrics fJL = new Lyrics();
        private Component.Lyrics lyrics = new Component.Lyrics();
        private WebSocketServer wssv;
        private MenuItem lyricSelectMenu;
        private MenuItem toggleMoveMenu;
        private LyricInfo lyricInfo;
        private uint prevTime = 0;
        private uint DEFAULT_FLAG = 0;
        private IntPtr wHandle;
        internal AppSetting appSetting = new AppSetting();
        private HistoryManager history = new HistoryManager();

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
            SetNotification();

            Width = appSetting.BgWidth;
            Opacity = appSetting.WinOpacity;

            if (!Utils.HasSpicetify())
            {
                System.Windows.MessageBox.Show("Spicetify가 설치되어있지 않습니다.", "PandaLyrics", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.Close();
                return;
            }

            if (!Utils.HasExtension())
            {
                var answer = System.Windows.MessageBox.Show("pandaLyrics.js가 설치되어있지 않습니다.\n설치하고 적용하시겠습니까?", "PandaLyrics", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (answer == MessageBoxResult.Yes)
                {
                    if (!Utils.InstallExtension())
                    {
                        this.Close();
                        return;
                    }
                    System.Windows.MessageBox.Show("pandaLyrics.js를 설치 및 적용하였습니다.");
                }
                else
                {
                    System.Windows.MessageBox.Show("프로그램을 종료합니다.");
                    this.Close();
                }
            }
        }
        private void SetNotification()
        {
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            ni.Visible = true;
            ni.Text = "Panda Lyrics";

            // Set context menu
            ni.ContextMenu = new ContextMenu();
            var toggleMenu = ni.ContextMenu.MenuItems.Add("표시");
            toggleMenu.Click += ToggleWindow;
            toggleMenu.Checked = appSetting.AppVisible;
            lyricSelectMenu = ni.ContextMenu.MenuItems.Add("가사 선택");
            toggleMoveMenu = ni.ContextMenu.MenuItems.Add("위치 이동");
            toggleMoveMenu.Click += ToggleMoveMode;
            ni.ContextMenu.MenuItems.Add("환경설정").Click += OpenSettings;
            ni.ContextMenu.MenuItems.Add("종료").Click += (sender, e) => this.Close();

            lyricSelectMenu.MenuItems.Add("비어있음").Enabled = false;

            ni.DoubleClick += (sender, e) => ni.ContextMenu.MenuItems[0].PerformClick();
            ni.ContextMenu.MenuItems[0].DefaultItem = true;
        }
        private void OpenSettings(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm(this);
            settingForm.ShowDialog();
        }
        private void ToggleBackground(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;

            if (appSetting.BgVisible)
            {
                item.Checked = false;
                appSetting.BgVisible = false;
            }
            else
            {
                item.Checked = true;
                appSetting.BgVisible = true;
            }
            appSetting.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (appSetting.WindowLeft != -1 && appSetting.WindowTop != -1)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = appSetting.WindowLeft;
                this.Top = appSetting.WindowTop;
            }
            else
            {
                this.Top = SystemParameters.WorkArea.Height - Height;
                this.Left = SystemParameters.WorkArea.Width - Width - 30;
            }

            SetLyricsVisible(false);
            stackPanel.Children.Add(lyrics);
            DataContext = appSetting;

            SetupServer();

            wHandle = new WindowInteropHelper(this).Handle;
            DEFAULT_FLAG = Utils.GetWindowFlag(wHandle);
            Utils.SetClickThruAble(wHandle, DEFAULT_FLAG, true);

            if (!appSetting.AppVisible)
            {
                this.Visibility = Visibility.Collapsed;
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
                    lyricsReceiver.CloseEvent += this.WS_CloseEvent;
                    lyricsReceiver.OpenEvent += this.WS_OpenEvent;
                    return lyricsReceiver;
                });
                wssv.Start();
            }
            catch (SocketException)
            {
                System.Windows.MessageBox.Show("8999포트가 이미 사용중입니다!\n이미 이 프로그램이 실행중인지 확인해주세요!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        private void LoadLyric(int lyricID)
        {
            Regex rx = new Regex(@"(?:\[(?<min>\d\d):(?<sec>\d\d\.\d\d)\])(?<content>.*)",
              RegexOptions.Compiled | RegexOptions.IgnoreCase);

            lyricEntities.Clear();
            lyrics.Content = "가사를 불러오는 중...";
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
            lyrics.Content = "";
        }

        private void TickEvent(object sender, LyricsReceiver.TickEventArgs e)
        {
            if (moveMode)
            {
                return;
            }
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
            lyrics.Content = "";
            List<LyricBasicInfo> lyricList;
            prevTime = 0;
            SetLyricsVisible(true);
            var lyricID = history.Get(e.SongID);

            try
            {
                lyrics.Content = "가사를 검색중...";
                try
                {
                    lyricList = fJL.GetLyricsSearch(Utils.Escape(e.Artist), Utils.Escape(e.Title));
                }
                catch
                {
                    lyricList = fJL.GetLyricsSearch(string.Empty, Utils.Escape(e.Title));
                }

                if (lyricList == null || lyricList.Count <= 0)
                {
                    throw new Exception("검색된 가사가 없습니다.");
                }

                foreach (var lyric in lyricList)
                {
                    var menu = lyricSelectMenu.MenuItems.Add(lyric.Title + " - " + lyric.Artist + " [" + lyric.Album + "]");
                    menu.Click += (s, ev) =>
                    {
                        MenuItem item = (MenuItem)s;

                        foreach (MenuItem cur in lyricSelectMenu.MenuItems)
                        {
                            cur.Checked = false;
                        }
                        item.Checked = true;
                        history.Set(e.SongID, lyric.LyricID);
                        LoadLyric(lyric.LyricID);
                    };
                    if (lyricID != null && lyricID == lyric.LyricID)
                    {
                        menu.PerformClick();
                    }
                }
                if (lyricID == null)
                {
                    lyricSelectMenu.MenuItems[0].PerformClick();
                }
            }
            catch
            {
                lyricSelectMenu.MenuItems.Clear();
                lyricSelectMenu.MenuItems.Add("비어있음").Enabled = false;
                lyrics.Content = "가사를 찾지 못했습니다.";
                SetLyricsVisible(false);
                return;
            }
        }
        private void WS_CloseEvent(object sender, CloseEventArgs e)
        {
            this.lyricEntities.Clear();
            this.lyrics.Content = "";
            SetLyricsVisible(false);
        }
        private void WS_OpenEvent(object sender, EventArgs e)
        {
            this.lyricEntities.Clear();
            this.lyrics.Content = "";
        }
        private void SetLyricsVisible(bool value)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(delegate
                {
                    if (value)
                    {
                        this.lyrics.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.lyrics.Visibility = Visibility.Collapsed;

                    }
                }));

        }

        private void ToggleWindow(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            if (this.IsVisible)
            {
                this.Visibility = Visibility.Collapsed;
                item.Checked = false;
                appSetting.AppVisible = false;
            }
            else
            {
                this.Visibility = Visibility.Visible;
                item.Checked = true;
                appSetting.AppVisible = true;
            }
            appSetting.Save();
        }
        private void ToggleMoveMode(object sender, EventArgs e)
        {
            var item = sender as MenuItem;
            if (this.moveMode)
            {
                moveMode = false;
                item.Checked = false;
                this.Cursor = null;
                this.Background = System.Windows.Media.Brushes.Transparent;
                Utils.SetClickThruAble(wHandle, DEFAULT_FLAG, true);

                appSetting.WindowLeft = this.Left;
                appSetting.WindowTop = this.Top;
                appSetting.Save();
                lyrics.Content = "";
            }
            else
            {
                moveMode = true;
                item.Checked = true;
                this.Cursor = System.Windows.Input.Cursors.SizeAll;
                this.Background = System.Windows.Media.Brushes.Black;
                Utils.SetClickThruAble(wHandle, DEFAULT_FLAG, false);
                lyrics.Content = "더블클릭시 위치이동을 완료합니다.";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (moveMode && e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ni.Visible = false;
            ni.Dispose();
            if (wssv != null)
            {
                wssv.Stop();
            }
            appSetting.Save();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (moveMode && e.ChangedButton == MouseButton.Left)
            {
                toggleMoveMenu.PerformClick();
            }
        }
    }
}
