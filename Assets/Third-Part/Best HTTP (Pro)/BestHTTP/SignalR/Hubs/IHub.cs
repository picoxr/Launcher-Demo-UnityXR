/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

#if !BESTHTTP_DISABLE_SIGNALR

using System;

using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs
{
    /// <summary>
    /// Interface to be able to hide internally used functions and properties.
    /// </summary>
    public interface IHub
    {
        Connection Connection { get; set; }

        bool Call(ClientMessage msg);
        bool HasSentMessageId(UInt64 id);
        void Close();
        void OnMethod(MethodCallMessage msg);
        void OnMessage(IServerMessage msg);
    }
}

#endif
