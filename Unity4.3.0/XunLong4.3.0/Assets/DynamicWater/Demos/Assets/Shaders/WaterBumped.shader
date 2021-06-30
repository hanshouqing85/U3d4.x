Shader "Custom/Water Reflective Bumped" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
		_BumpMap ("Normalmap", 2D) = "bump" { }
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite on Cull back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0
		
		sampler2D _BumpMap;
		samplerCUBE _Cube;
		
		fixed4 _Color;
		fixed4 _ReflectColor;
		
		struct Input {
			float2 uv_BumpMap;
			float3 worldRefl; INTERNAL_DATA
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color.rgb;
			
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			
			// Calculating reflection vector
			half3 worldRefl = WorldReflectionVector (IN, o.Normal);
			fixed4 reflcol = texCUBE (_Cube, worldRefl);
			
			o.Emission = reflcol.rgb * (_ReflectColor.rgb * _ReflectColor.rgb);
			o.Alpha = length(reflcol.rgb) * _ReflectColor.a * _Color.a;
		}
		ENDCG
	}

	FallBack "Reflective/Bumped Specular"
}
