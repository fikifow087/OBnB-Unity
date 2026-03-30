Shader "UI/FIKIFOW_BlendModes"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // UI Stencil properties untuk Masking bawaan Canvas
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15

        // Parameter Blend dari Script C#
        [HideInInspector] _SrcBlend ("SrcBlend", Float) = 5 // SrcAlpha
        [HideInInspector] _DstBlend ("DstBlend", Float) = 10 // OneMinusSrcAlpha
        [HideInInspector] _BlendOp ("BlendOp", Float) = 0 // Add
        [HideInInspector] _SrcBlendAlpha ("SrcBlendAlpha", Float) = 1 // One
        [HideInInspector] _DstBlendAlpha ("DstBlendAlpha", Float) = 10 // OneMinusSrcAlpha
        [HideInInspector] _BlendOpAlpha ("BlendOpAlpha", Float) = 0 // Add
        [HideInInspector] _BlendModeID ("Blend Mode ID", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]

        // Di sinilah keajaiban Hardware Blending terjadi secara instan!
        BlendOp [_BlendOp], [_BlendOpAlpha]
        Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float _BlendModeID;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = v.texcoord;

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                int mode = (int)_BlendModeID;

                if (mode == 3) // Multiply
                {
                    // Multiply: Blend DstColor Zero -> FrameBuffer = Dst * Frag
                    // Agar Opacity bekerja (saat alpha 0, warna jadi background asli), Frag harus lerp ke Putih
                    color.rgb = lerp(half3(1,1,1), color.rgb, color.a);
                }
                else if (mode == 8 || mode == 10 || mode == 20 || mode == 21) 
                {
                    // Screen, LinearDodge(Add), Exclusion, Subtract
                    // Semua rumus Blend mode ini di Hardware mewajibkan Fragment Alpha dikalikan ke RGB
                    color.rgb = color.rgb * color.a;
                }

                return color;
            }
            ENDCG
        }
    }
}
