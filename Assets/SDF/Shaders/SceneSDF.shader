Shader "SDF/SceneSDF"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector]
        _Radius("Radius", Range(0.0, 1.0)) = 1
        [HideInInspector]
        _Smooth("Smoothness", Range(0.0, 1.0)) = 0.01
        [HideInInspector]
        _Size("Size", Vector) = (0,0,0,0)
        [HideInInspector]
        _Fill("Fill", Range(0.0, 1.0)) = 0.0
        _InsideColor("InsideColor", Color) = (1,0,0,1)
        _OutsideColor("OutsideColor", Color) = (0,1,0,1)
        _Debug("Debug", int) = 0
        _LineDistance("LineDistance", float) = 1.0
        _LineThickness("LineThickness", float) = 1.0
        _SubLines("Sublines", float) = 1.0
        _SubLineThickness("SublineThickness", float) = 1.0
        _Shape("Shape", int) = 0
        _IsMask("Is Mask", int) = 1
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent"
        }
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
        ColorMask [_ColorMask]
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "SDF_2D.cginc"
            
            struct appdata
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 samplePosition : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            float _Radius;
            float _Smooth;
            Texture2D _MainTex;
            SamplerState my_point_repeat_sampler;
            float4 _MainTex_ST;
            float4 _Size;
            float _Fill;
            int _Debug;
            fixed4 _InsideColor;
            fixed4 _OutsideColor;
            float _LineDistance;
            float _LineThickness;
            int _Shape;

            float _SubLines;
            float _SubLineThickness;

            int _IsMask;
       
            v2f vert (appdata vertex)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(vertex.position);
                output.uv = TRANSFORM_TEX(vertex.uv / _Size.xy, _MainTex);
                output.samplePosition = 2.0f * (vertex.uv - 0.5 *_Size.xy);
                output.color = vertex.color;
                return output;
            }

            float scene(float2 position)
            {
                float distance;
                switch (_Shape)
                {
                case 0:
                    distance = circle(position, _Fill, _Smooth);
                    break;
                case 1:
                    distance = rectangle(position, float2(_Size.x, _Size.y), _Radius, _Fill, _Smooth);
                    break;
                case 2:
                    distance = equitriangle(position, _Radius, _Fill, _Smooth);   
                    break;
                default:
                    distance = circle(position, _Fill, _Smooth);
                    break;
                }
                return distance; 
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float distance = scene(i.samplePosition);
                

                if(_Debug > 0)
                {
                    return debugColor(distance);
                }else
                {
                    fixed4 color;
                    color = _MainTex.Sample(my_point_repeat_sampler, i.uv) * i.color;
                    distance = 1.0 - smoothstep(0.0, _Smooth, distance);
                    color *= distance;
                    if(_IsMask)
                    {
                        if (!(color.a > 0))
                            discard;
                    }
                    return color;
                }
                
            }
            ENDCG
        }
    }
}
