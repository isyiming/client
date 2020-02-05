Shader "Custom/Road" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "whrite" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque"
			"Queue" = "Geometry+1"
         }
		LOD 200
		Offset -1, -1

		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows decal:blend 

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;

		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

        float River (float2 riverUV, sampler2D noiseTex) {
	        float2 uv = riverUV;
	        uv.x = uv.x * 0.0625 + _Time.y * 0.005;
	        uv.y -= _Time.y * 0.25;
	        float4 noise = tex2D(noiseTex, uv);

	        float2 uv2 = riverUV;
	        uv2.x = uv2.x * 0.0625 - _Time.y * 0.0052;
	        uv2.y -= _Time.y * 0.23;
	        float4 noise2 = tex2D(noiseTex, uv2);
	
	        return noise.x * noise2.w;
        }

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float river = River(IN.uv_MainTex, _MainTex);

			float4 noise = tex2D(_MainTex, IN.worldPos.xz * 0.025);

			fixed4 c = saturate(_Color + river);
			float blend = IN.uv_MainTex.x;
			blend *= noise.x + 0.5;
			blend = smoothstep(0.4, 0.7, blend);

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = blend;

		}
		ENDCG
	}
	FallBack "Diffuse"
}