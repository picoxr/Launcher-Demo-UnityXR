/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

#if !BESTHTTP_DISABLE_SOCKETIO

namespace BestHTTP.SocketIO
{
    public sealed class Error
    {
        public SocketIOErrors Code { get; private set; }
        public string Message { get; private set; }

        public Error(SocketIOErrors code, string msg)
        {
            this.Code = code;
            this.Message = msg;
        }

        public override string ToString()
        {
            return string.Format("Code: {0} Message: \"{1}\"", this.Code.ToString(), this.Message);
        }
    }
}

#endif
