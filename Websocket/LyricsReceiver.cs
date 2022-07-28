using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace PandaLyrics.Websocket
{
    internal class LyricsReceiver : WebSocketBehavior
    {
        public event EventHandler<CloseEventArgs> CloseEvent;
        public event EventHandler OpenEvent;
        public class SongChangedEventArgs : EventArgs
        {
            public string Artist { get; set; }
            public string Title { get; set; }
        }
        public event EventHandler<SongChangedEventArgs> SongChangedEvent;
        public class TickEventArgs : EventArgs
        {
            public uint Time { get; set; }
        }
        public event EventHandler<TickEventArgs> TickEvent;

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            JObject res = JObject.Parse(e.Data);
            switch (res["type"].ToString())
            {
                case "songchange":
                    {
                        SongChangedEventArgs args = res["data"].ToObject<SongChangedEventArgs>();
                        SongChangedEvent(this, args);
                    }
                    break;
                case "tick":
                    {
                        TickEventArgs args = res["data"].ToObject<TickEventArgs>();
                        TickEvent(this, args);
                    }
                    break;
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            CloseEvent(this, e);
        }
        protected override void OnOpen()
        {
            OpenEvent(this, null);
        }
    }
}
