/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

#if !BESTHTTP_DISABLE_SIGNALR

using System.Collections.Generic;

using LitJson;

namespace BestHTTP.SignalR.JsonEncoders
{
    public sealed class LitJsonEncoder : IJsonEncoder
    {
        public string Encode(object obj)
        {
            JsonWriter writer = new JsonWriter();
            JsonMapper.ToJson(obj, writer);

            return writer.ToString();
        }

        public IDictionary<string, object> DecodeMessage(string json)
        {
            JsonReader reader = new JsonReader(json);

            return JsonMapper.ToObject<Dictionary<string, object>>(reader);
        }
    }
}

#endif
