Shader "UI/FIKIFOW_BlendAdvanced"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15

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

        // Blend mode diatur dari script C# untuk setiap blend mode
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add

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

            // --- Fungsi Matematika Photoshop ---
            float ColorDodge(float b, float s) { return (s >= 1.0) ? 1.0 : min(b / max(1.0 - s, 0.001), 1.0); }
            float ColorBurn(float b, float s) { return (s <= 0.0) ? 0.0 : max(1.0 - ((1.0 - b) / max(s, 0.001)), 0.0); }
            float Overlay(float b, float s) { return (b < 0.5) ? (2.0 * b * s) : (1.0 - 2.0 * (1.0 - b) * (1.0 - s)); }
            float SoftLight(float b, float s) { return (s < 0.5) ? (2.0 * b * s + b * b * (1.0 - 2.0 * s)) : (2.0 * b * (1.0 - s) + sqrt(b) * (2.0 * s - 1.0)); }
            float HardLight(float b, float s) { return Overlay(s, b); }
            float VividLight(float b, float s) { return (s < 0.5) ? ColorBurn(b, 2.0 * s) : ColorDodge(b, 2.0 * (s - 0.5)); }
            float LinearLight(float b, float s) { return saturate(b + 2.0 * s - 1.0); }
            float PinLight(float b, float s) { return (s < 0.5) ? min(b, 2.0 * s) : max(b, 2.0 * (s - 0.5)); }
            
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

                // Output fragment color dengan alpha intact
                // Hardware blending nanti akan handle blend mode sesuai Blend statement di Pass
                return color;
            }
            ENDCG
        }
    }
}
