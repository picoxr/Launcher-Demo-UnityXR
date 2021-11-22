/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DHKeyGenerationParameters
		: KeyGenerationParameters
    {
        private readonly DHParameters parameters;

		public DHKeyGenerationParameters(
            SecureRandom	random,
            DHParameters	parameters)
			: base(random, GetStrength(parameters))
        {
            this.parameters = parameters;
        }

		public DHParameters Parameters
        {
            get { return parameters; }
        }

		internal static int GetStrength(
			DHParameters parameters)
		{
			return parameters.L != 0 ? parameters.L : parameters.P.BitLength;
		}
    }
}

#endif
