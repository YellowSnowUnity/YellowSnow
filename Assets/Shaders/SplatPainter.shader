Shader "YellowSnow/SplatPainter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Coordinate ("Coordinate", Vector) = (0,0,0,0)
		_DrawColor ("DrawColor", Color) = (1,0,0,1)
        _BGColor("BGColor", Color) = (0,0,0,1)
        _DropOff("Dropoff", int) = 30
        _Drawing("Drawing", int) = 1
        _Regenerating("Regenerating", int) = 1
        _RegenAmount("Regen amount", float) = 0
        _RegenOpacity("Regen opacity", float) = 0
        _FPS("FPS Factor", float) = 60

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Coordinate;
            fixed4 _DrawColor;
            fixed4 _BGColor;
            int _DropOff;
            int _Drawing;
            int _Regenerating;
            half _RegenAmount;
            half _RegenOpacity;
            float _FPS;

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

			v2f vert (appdata v)
			{
                // trivial vertex shader
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 output;
                fixed4 current = tex2D(_MainTex, i.uv);

                output = current;

                if (_Drawing) {
                    // draw a gradient circle around the _Coordinate we are drawing to
                    float closeness = saturate(1 - distance(i.uv, _Coordinate.xy));
                    fixed4 light = pow(closeness, _DropOff) * _DrawColor;
                    output = saturate(_BGColor + current + light);
                }

                if (_Regenerating) {
                    // re-generate snow over time by turning colors back to black
                    float rValue = ceil(rand(i.uv * _Time.x) - (1 - _RegenAmount));
                    float fpsFactor =  60.0 / _FPS;
                    output = saturate(output - (rValue *  fpsFactor * _RegenOpacity));
                }

                return output;
			}
			ENDCG
		}
	}
}
