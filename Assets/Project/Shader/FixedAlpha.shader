Shader "UI/Icon/FixedAlpha"
{ 
    Properties
    {
        [PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
		
		// Soft Mask Support
        // Soft Mask determines that shader supports soft masking by presence of this property.
        [PerRendererData] _SoftMask("Mask", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="true"
            "RenderType"="Transparent"
        }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        ColorMask [_ColorMask]

        ZWrite Off
        Blend One OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			 // Soft Mask Support
            // You also can use full path (Assets/...)
			#include "Assets/Third-Part/SoftMask/Shaders/SoftMask.cginc"
			
			// Soft Mask Support
            #pragma multi_compile __ SOFTMASK_SIMPLE SOFTMASK_SLICED SOFTMASK_TILED

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                half2 texcoord  : TEXCOORD0;
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
				float4 worldPosition : TEXCOORD1;
				// Soft Mask Support
                // The number in braces determines what TEXCOORDn Soft Mask may use
                // (it required only one TEXCOORD).
                SOFTMASK_COORDS(2)
            };

            sampler2D _MainTex;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.worldPosition = IN.vertex;
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
				
				SOFTMASK_CALCULATE_COORDS(OUT, IN.vertex) // Soft Mask Support
				
                return OUT;
            }

            float4 frag (v2f i) : COLOR
            {
                float4 tex = tex2D(_MainTex, i.texcoord.xy) * i.color;
				tex *= SOFTMASK_GET_MASK(i); // Soft Mask Support
				tex.rgb *= i.color.a;
				return tex;
            }
            ENDCG
        }
    }
    Fallback "Sprites/Default"
}
