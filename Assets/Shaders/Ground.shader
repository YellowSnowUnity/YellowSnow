Shader "YellowSnow/Ground" {
	Properties {
		_SnowTex ("Snow", 2D) = "white" {}
		_SnowColor ("SnowColor", Color) = (1,1,1,1)
		_YellowTex ("Yellow", 2D) = "white" {}
		_YellowColor ("YellowColor", Color) = (1,1,0,1)
        _Splat ("Splat", 2D) = "gray" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:disp
        #pragma target 3.0

        /*
        If you want to use tesselation to add points to mesh to deform instead of
        using a pre-tesselated plane, you can add this to #pragma surface:

            tessellate:tessDistance

        Change the target to the following:

            #pragma target 4.6

        Add the parameter:

            _Tess ("Tessellation", Range(1,32)) = 4
            float _Tess;

        And the tesselation function:

            #include "Tessellation.cginc"
            float4 tessDistance (appdata v0, appdata v1, appdata v2) {
                float minDist = 10.0;
                float maxDist = 25.0;
                return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
            }


        Note that as of this time (2018-12-7) tesselation shaders will not run on WebGL
        */

        float _Displacement;
        sampler2D _YellowTex;
        fixed4 _YellowColor;
		sampler2D _SnowTex;
		fixed4 _SnowColor;
        sampler2D _Splat;
		half _Glossiness;
		half _Metallic;

        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        void disp (inout appdata v)
        {
            float d = tex2Dlod(_Splat, float4(v.texcoord.xy,0,0)).r * _Displacement;
            v.vertex.xyz -= v.normal * d;
        }

		struct Input {
            float2 uv_SnowTex;
			float2 uv_YellowTex;
            float2 uv_Splat;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
            half amt = 1 - tex2D(_Splat, IN.uv_Splat).r;
			fixed4 c = lerp(tex2D (_YellowTex, IN.uv_YellowTex) * _YellowColor, tex2D (_SnowTex, IN.uv_SnowTex) * _SnowColor, amt);

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}

}
