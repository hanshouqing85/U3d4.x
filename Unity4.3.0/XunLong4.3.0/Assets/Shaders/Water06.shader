Shader "FX/Water06"
{
	Properties 
	{
_Color("_Color", Color) = (1,1,1,1)
_Texture1("_Texture1", 2D) = "black" {}
_BumpMap1("_BumpMap1", 2D) = "black" {}
_Texture2("_Texture2", 2D) = "black" {}
_BumpMap2("_BumpMap2", 2D) = "black" {}
_MainTexSpeed("_MainTexSpeed", Float) = 0
_Bump1Speed("_Bump1Speed", Float) = 0
_Texture2Speed("_Texture2Speed", Float) = 0
_Bump2Speed("_Bump2Speed", Float) = 0
_DistortionMap("_DistortionMap", 2D) = "black" {}
_DistortionSpeed("_DistortionSpeed", Float) = 0
_DistortionPower("_DistortionPower", Range(0,0.02) ) = 0
_Specular("_Specular", Range(0,7) ) = 1
_Gloss("_Gloss", Range(0.3,2) ) = 0.3
_Opacity("_Opacity", Range(-0.2,1) ) = 0
_Reflection("_Reflection", 2D) = "black" {}
_ReflectPower("_ReflectPower", Range(0,1) ) = 0




_ReflectionTex ("Internal reflection", 2D) = "white" {}
	
	_MainTex ("Fallback texture", 2D) = "black" {}	
	_ShoreTex ("Shore & Foam texture ", 2D) = "black" {}	
	_BumpMap ("Normals ", 2D) = "bump" {}
				
	_DistortParams ("Distortions (Bump waves, Reflection, Fresnel power, Fresnel bias)", Vector) = (1.0 ,1.0, 2.0, 1.15)
	_InvFadeParemeter ("Auto blend parameter (Edge, Shore, Distance scale)", Vector) = (0.15 ,0.15, 0.5, 1.0)
	
	_AnimationTiling ("Animation Tiling (Displacement)", Vector) = (2.2 ,2.2, -1.1, -1.1)
	_AnimationDirection ("Animation Direction (displacement)", Vector) = (1.0 ,1.0, 1.0, 1.0)

	_BumpTiling ("Bump Tiling", Vector) = (1.0 ,1.0, -2.0, 3.0)
	_BumpDirection ("Bump Direction & Speed", Vector) = (1.0 ,1.0, -1.0, 1.0)
	
	_FresnelScale ("FresnelScale", Range (0.15, 4.0)) = 0.75	

	_BaseColor ("Base color", COLOR)  = ( .54, .95, .99, 0.5)	
	_ReflectionColor ("Reflection color", COLOR)  = ( .54, .95, .99, 0.5)	
	_SpecularColor ("Specular color", COLOR)  = ( .72, .72, .72, 1)
	
	_WorldLightDir ("Specular light direction", Vector) = (0.0, 0.1, -0.5, 0.0)
	_Shininess ("Shininess", Range (2.0, 500.0)) = 200.0	
	
	_Foam ("Foam (intensity, cutoff)", Vector) = (0.1, 0.375, 0.0, 0.0)
	
	_GerstnerIntensity("Per vertex displacement", Float) = 1.0
	_GAmplitude ("Wave Amplitude", Vector) = (0.3 ,0.35, 0.25, 0.25)
	_GFrequency ("Wave Frequency", Vector) = (1.3, 1.35, 1.25, 1.25)
	_GSteepness ("Wave Steepness", Vector) = (1.0, 1.0, 1.0, 1.0)
	_GSpeed ("Wave Speed", Vector) = (1.2, 1.375, 1.1, 1.5)
	_GDirectionAB ("Wave Direction", Vector) = (0.3 ,0.85, 0.85, 0.25)
	_GDirectionCD ("Wave Direction", Vector) = (0.1 ,0.9, 0.5, 0.5)	

	}
	
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	//#include "WaterInclude.cginc"

	struct appdata 
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	// interpolator structs
	
	struct v2f 
	{
		float4 pos : SV_POSITION;
		float4 normalInterpolator : TEXCOORD0;
		float4 viewInterpolator : TEXCOORD1; 	
		float4 bumpCoords : TEXCOORD2;
		float4 screenPos : TEXCOORD3;	
		float4 grabPassPos : TEXCOORD4;
	};

	struct v2f_noGrab
	{
		float4 pos : SV_POSITION;
		float4 normalInterpolator : TEXCOORD0;
		float3 viewInterpolator : TEXCOORD1; 	
		float4 bumpCoords : TEXCOORD2;
		float4 screenPos : TEXCOORD3;	
	};
		
	struct v2f_simple
	{
		float4 pos : SV_POSITION;
		float4 viewInterpolator : TEXCOORD0; 	
		float4 bumpCoords : TEXCOORD1;
	};	

	// textures
	sampler2D _BumpMap;
	sampler2D _ReflectionTex;
	sampler2D _RefractionTex;
	sampler2D _ShoreTex;
	sampler2D _CameraDepthTexture;

	// colors in use
	uniform float4 _RefrColorDepth;
	uniform float4 _SpecularColor;
	uniform float4 _BaseColor;
	uniform float4 _ReflectionColor;
	
	// edge & shore fading
	uniform float4 _InvFadeParemeter;

	// specularity
	uniform float _Shininess;
	uniform float4 _WorldLightDir;

	// fresnel, vertex & bump displacements & strength
	uniform float4 _DistortParams;
	uniform float _FresnelScale;	
	uniform float4 _BumpTiling;
	uniform float4 _BumpDirection;

	uniform float4 _GAmplitude;
	uniform float4 _GFrequency;
	uniform float4 _GSteepness; 									
	uniform float4 _GSpeed;					
	uniform float4 _GDirectionAB;		
	uniform float4 _GDirectionCD;
	
	// foam
	uniform float4 _Foam;
	
	// shortcuts
	#define PER_PIXEL_DISPLACE _DistortParams.x
	#define REALTIME_DISTORTION _DistortParams.y
	#define FRESNEL_POWER _DistortParams.z
	#define VERTEX_WORLD_NORMAL i.normalInterpolator.xyz
	#define FRESNEL_BIAS _DistortParams.w
	#define NORMAL_DISPLACEMENT_PER_VERTEX _InvFadeParemeter.z
	
	//
	// HQ VERSION
	//
		
	v2f vert(appdata_full v)
	{
		v2f o;
		
		half3 worldSpaceVertex = mul(_Object2World,(v.vertex)).xyz;
		half3 vtxForAni = (worldSpaceVertex).xzz * 1.0; 		

		half3 nrml;
		half3 offsets;
		Gerstner (
			offsets, nrml, v.vertex.xyz, vtxForAni, 					// offsets, nrml will be written
			_GAmplitude,					 							// amplitude
			_GFrequency,				 								// frequency
			_GSteepness, 												// steepness
			_GSpeed,													// speed
			_GDirectionAB,												// direction # 1, 2
			_GDirectionCD												// direction # 3, 4
		);
				
		v.vertex.xyz += offsets;		
							
		// one can also use worldSpaceVertex.xz here (speed!), albeit it'll end up a little skewed	
		half2 tileableUv = mul(_Object2World,(v.vertex)).xz;
				
		o.bumpCoords.xyzw = (tileableUv.xyxy + _Time.xxxx * _BumpDirection.xyzw) * _BumpTiling.xyzw;	

		o.viewInterpolator.xyz = worldSpaceVertex - _WorldSpaceCameraPos;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		ComputeScreenAndGrabPassPos(o.pos, o.screenPos, o.grabPassPos);
		
		o.normalInterpolator.xyz = nrml;
		
		o.viewInterpolator.w = saturate(offsets.y); 
		o.normalInterpolator.w = 1;//GetDistanceFadeout(o.screenPos.w, DISTANCE_SCALE); 
		
		return o;
	}

	half4 frag( v2f i ) : COLOR
	{				
		half3 worldNormal = PerPixelNormal(_BumpMap, i.bumpCoords, VERTEX_WORLD_NORMAL, PER_PIXEL_DISPLACE);
		half3 viewVector = normalize(i.viewInterpolator.xyz);

		half4 distortOffset = half4(worldNormal.xz * REALTIME_DISTORTION * 10.0, 0, 0);
		half4 screenWithOffset = i.screenPos + distortOffset;
		half4 grabWithOffset = i.grabPassPos + distortOffset;
		
		half4 rtRefractionsNoDistort = tex2Dproj(_RefractionTex, UNITY_PROJ_COORD(i.grabPassPos));
		half refrFix = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(grabWithOffset)));
		half4 rtRefractions = tex2Dproj(_RefractionTex, UNITY_PROJ_COORD(grabWithOffset));
		
		#ifdef WATER_REFLECTIVE
			half4 rtReflections = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(screenWithOffset));	
		#endif

		#ifdef WATER_EDGEBLEND_ON
		if (LinearEyeDepth(refrFix) < i.screenPos.z) 
			rtRefractions = rtRefractionsNoDistort;	
		#endif
		
		half3 reflectVector = normalize(reflect(viewVector, worldNormal));          
		half3 h = normalize ((_WorldLightDir.xyz) + viewVector.xyz);
		float nh = max (0, dot (worldNormal, -h));
		float spec = max(0.0,pow (nh, _Shininess));		
		
		half4 edgeBlendFactors = half4(1.0, 0.0, 0.0, 0.0);
		
		#ifdef WATER_EDGEBLEND_ON
			half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
			depth = LinearEyeDepth(depth);
			edgeBlendFactors = saturate(_InvFadeParemeter * (depth-i.screenPos.w));		
			edgeBlendFactors.y = 1.0-edgeBlendFactors.y;
		#endif	
		
		// shading for fresnel term
		worldNormal.xz *= _FresnelScale;
		half refl2Refr = Fresnel(viewVector, worldNormal, FRESNEL_BIAS, FRESNEL_POWER);
				
		// base, depth & reflection colors
		half4 baseColor = ExtinctColor (_BaseColor, i.viewInterpolator.w * _InvFadeParemeter.w);
		#ifdef WATER_REFLECTIVE
			half4 reflectionColor = lerp (rtReflections,_ReflectionColor,_ReflectionColor.a);
		#else
			half4 reflectionColor = _ReflectionColor;
		#endif
		
		baseColor = lerp (lerp (rtRefractions, baseColor, baseColor.a), reflectionColor, refl2Refr);
		baseColor = baseColor + spec * _SpecularColor;
		
		// handle foam
		half4 foam = Foam(_ShoreTex, i.bumpCoords * 2.0);
		baseColor.rgb += foam.rgb * _Foam.x * (edgeBlendFactors.y + saturate(i.viewInterpolator.w - _Foam.y));
		
		baseColor.a = edgeBlendFactors.x;
		return baseColor;
	}
	
	//
	// MQ VERSION
	//
	
	v2f_noGrab vert300(appdata_full v)
	{
		v2f_noGrab o;
		
		half3 worldSpaceVertex = mul(_Object2World,(v.vertex)).xyz;
		half3 vtxForAni = (worldSpaceVertex).xzz * 1.0; 			

		half3 nrml;
		half3 offsets;
		Gerstner (
			offsets, nrml, v.vertex.xyz, vtxForAni, 					// offsets, nrml will be written
			_GAmplitude,					 							// amplitude
			_GFrequency,				 								// frequency
			_GSteepness, 												// steepness
			_GSpeed,													// speed
			_GDirectionAB,												// direction # 1, 2
			_GDirectionCD												// direction # 3, 4
		);
				
		v.vertex.xyz += offsets;		
							
		// one can also use worldSpaceVertex.xz here (speed!), albeit it'll end up a little skewed	
		half2 tileableUv = mul(_Object2World,v.vertex).xz;					
		o.bumpCoords.xyzw = (tileableUv.xyxy + _Time.xxxx * _BumpDirection.xyzw) * _BumpTiling.xyzw;	

		o.viewInterpolator.xyz = worldSpaceVertex - _WorldSpaceCameraPos;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.screenPos = ComputeScreenPos(o.pos);
		
		o.normalInterpolator.xyz = nrml;
		o.normalInterpolator.w = 1;//GetDistanceFadeout(o.screenPos.w, DISTANCE_SCALE); 
		
		return o;
	}

	half4 frag300( v2f_noGrab i ) : COLOR
	{		
		half3 worldNormal = PerPixelNormal(_BumpMap, i.bumpCoords, normalize(VERTEX_WORLD_NORMAL), PER_PIXEL_DISPLACE);

		half3 viewVector = normalize(i.viewInterpolator.xyz);

		half4 distortOffset = half4(worldNormal.xz * REALTIME_DISTORTION * 10.0, 0, 0);
		half4 screenWithOffset = i.screenPos + distortOffset;
		
		#ifdef WATER_REFLECTIVE		
			half4 rtReflections = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(screenWithOffset));	
		#endif
		
		half3 reflectVector = normalize(reflect(viewVector, worldNormal));          
		half3 h = normalize (_WorldLightDir.xyz + viewVector.xyz);
		float nh = max (0, dot (worldNormal, -h));
		float spec = max(0.0,pow (nh, _Shininess));	
		
		half4 edgeBlendFactors = half4(1.0, 0.0, 0.0, 0.0);
		
		#ifdef WATER_EDGEBLEND_ON
			half depth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
			depth = LinearEyeDepth(depth);
			edgeBlendFactors = saturate(_InvFadeParemeter * (depth-i.screenPos.z));		
			edgeBlendFactors.y = 1.0-edgeBlendFactors.y;
		#endif		
		
		worldNormal.xz *= _FresnelScale;		
		half refl2Refr = Fresnel(viewVector, worldNormal, FRESNEL_BIAS, FRESNEL_POWER);
		
		half4 baseColor = _BaseColor;
		#ifdef WATER_REFLECTIVE	
			baseColor = lerp (baseColor, lerp (rtReflections,_ReflectionColor,_ReflectionColor.a), saturate(refl2Refr * 2.0));
		#else
			baseColor = lerp (baseColor, _ReflectionColor, saturate(refl2Refr * 2.0));		
		#endif
		
		baseColor = baseColor + spec * _SpecularColor;
		
		baseColor.a = edgeBlendFactors.x * saturate(0.5 + refl2Refr * 1.0);
		return baseColor;
	}	
	
	//
	// LQ VERSION
	//
	
	v2f_simple vert200(appdata_full v)
	{ 
		v2f_simple o;
		
		half3 worldSpaceVertex = mul(_Object2World, v.vertex).xyz;
		half2 tileableUv = worldSpaceVertex.xz;

		o.bumpCoords.xyzw = (tileableUv.xyxy + _Time.xxxx * _BumpDirection.xyzw) * _BumpTiling.xyzw;	

		o.viewInterpolator.xyz = worldSpaceVertex-_WorldSpaceCameraPos;
		
		o.pos = mul(UNITY_MATRIX_MVP,  v.vertex);
		
		o.viewInterpolator.w = 1;//GetDistanceFadeout(ComputeScreenPos(o.pos).w, DISTANCE_SCALE); 
		
		return o;

	}

	half4 frag200( v2f_simple i ) : COLOR
	{		
		half3 worldNormal = PerPixelNormal(_BumpMap, i.bumpCoords, half3(0,1,0), PER_PIXEL_DISPLACE);
		half3 viewVector = normalize(i.viewInterpolator.xyz);

		half3 reflectVector = normalize(reflect(viewVector, worldNormal));          
		half3 h = normalize ((_WorldLightDir.xyz) + viewVector.xyz);
		float nh = max (0, dot (worldNormal, -h));
		float spec = max(0.0,pow (nh, _Shininess));	

		worldNormal.xz *= _FresnelScale;		
		half refl2Refr = Fresnel(viewVector, worldNormal, FRESNEL_BIAS, FRESNEL_POWER);	

		half4 baseColor = _BaseColor;
		baseColor = lerp(baseColor, _ReflectionColor, saturate(refl2Refr * 2.0));
		baseColor.a = saturate(2.0 * refl2Refr + 0.5);

		baseColor.rgb += spec * _SpecularColor.rgb;
		return baseColor;	
	}
			
ENDCG

	
	
	
	
	
	
	
	
	
	
	
	
	SubShader 
	{
		Tags
		{
		"Queue"="Transparent"
		"IgnoreProjector"="True"
		"RenderType"="Transparent"
		
		}

		
		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGBA
		Blend SrcAlpha OneMinusSrcAlpha
		
		
		CGPROGRAM
		#pragma surface surf BlinnPhongEditor
		#pragma target 3.0
		
		
		fixed4 _Color;
		uniform sampler2D _Texture1;
		uniform sampler2D _BumpMap1;
		uniform sampler2D _Texture2;
		uniform sampler2D _BumpMap2;
		half _MainTexSpeed;
		half _Bump1Speed;
		half _Texture2Speed;
		half _Bump2Speed;
		uniform sampler2D _DistortionMap;
		half _DistortionSpeed;
		half _DistortionPower;
		fixed _Specular;
		fixed _Gloss;
		float _Opacity;
		uniform sampler2D _Reflection;
		float _ReflectPower;

		struct EditorSurfaceOutput {
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Gloss;
			half Specular;
			half Alpha;
			half4 Custom;
		};
			
		inline fixed4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, fixed4 light)
		{
			fixed3 spec = light.a * s.Gloss;
			fixed4 c;
			c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
			c.a = s.Alpha;
			return c;
		}

		inline fixed4 LightingBlinnPhongEditor (EditorSurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{
			fixed3 h = normalize (lightDir + viewDir);
			
			fixed diff = max (0, dot ( lightDir, s.Normal ));
			
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0);
			
			fixed4 res;
			res.rgb = _LightColor0.rgb * diff;
			res.w = spec * Luminance (_LightColor0.rgb);
			res *= atten * 2.0;

			return LightingBlinnPhongEditor_PrePass( s, res );
		}
		
		struct Input {
			float3 viewDir;
			float2 uv_DistortionMap;
			float2 uv_Texture1;
			float2 uv_Texture2;
			float2 uv_BumpMap1;
			float2 uv_BumpMap2;
		};

		void surf (Input IN, inout EditorSurfaceOutput o) {
			o.Normal = float3(0.0,0.0,1.0);
			o.Alpha = 1.0;
			o.Albedo = 0.0;
			o.Emission = 0.0;
			o.Gloss = 0.0;
			o.Specular = 0.0;
			o.Custom = 0.0;
			
			float4 ViewDirection=float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z *10,0 );
			float4 Normalize0=normalize(ViewDirection);			
			
			
			float2 ReflexUV= float2((Normalize0.x + 1) * 0.5, (Normalize0.y + 1) * 0.5);
			
			// Animate distortionMap 
			float DistortSpeed=_DistortionSpeed * _Time;
			float2 DistortUV=(IN.uv_DistortionMap.xy) + DistortSpeed;
			// Create Normal for DistorionMap
			float4 DistortNormal = tex2D(_DistortionMap,DistortUV);
			// Multiply Tex2DNormal effect by DistortionPower
			float2 FinalDistortion = DistortNormal.xy * _DistortionPower;
			
			// Apply DistorionMap in Reflection's UV
			float4 Tex2D2=tex2D(_Reflection,ReflexUV + FinalDistortion);
			
			// Get Fresnel from viewDirection angle
			float3 Fresnel0_1_NoInput = float3(0,0,1);
			float Fresnel0=(1.0 - dot( normalize( float3( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z ).xyz), normalize( Fresnel0_1_NoInput.xyz ) ));
			
			// Multiply reflection by fresnel so it's stronger when it's far
			float Multiply12 =_ReflectPower * Fresnel0;
			float4 FinalReflex = Tex2D2 * Multiply12;
			
			// Animate MainTex
			float Multiply2=_Time * _MainTexSpeed;
			float2 MainTexUV=(IN.uv_Texture1.xy) + Multiply2; 
			
			// Apply Distorion in MainTex
			float4 Tex2D0=tex2D(_Texture1,MainTexUV + FinalDistortion);
			
			// Animate Texture2
			float Multiply3=_Time * _Texture2Speed;
			float2 Tex2UV=(IN.uv_Texture2.xy) + Multiply3;
			
			// Apply Distorion in Texture2
			// float2 Add1=Tex2UV + FinalDistortion;
			float4 Tex2D1=tex2D(_Texture2,Tex2UV + FinalDistortion); 
			
			// Merge MainTex and Texture2
			float4 TextureMix=Tex2D0 * Tex2D1;
			
			// Add TextureMix with Reflection
			float4 TexNReflex = FinalReflex + TextureMix;
			TexNReflex.xy = TexNReflex.xy + FinalDistortion.xy;  
			
			
			
			// Merge Textures, Reflection and Color
			float4 FinalDiffuse=_Color * TexNReflex; 			
			
			
			// Animate BumpMap1
			float Multiply8=_Time * _Bump1Speed;
			float2 Bump1UV=(IN.uv_BumpMap1.xy) + Multiply8;
			
			// Apply Distortion to BumpMap			
			half4 Tex2D3=tex2D(_BumpMap1,Bump1UV + FinalDistortion);
			
			// Animate BumpMap2
			half Multiply9=_Time * _Bump2Speed;
			half2 Bump2UV=(IN.uv_BumpMap2.xy) + Multiply9;
			
			// Apply Distortion to BumpMap2						
			fixed4 Tex2D4=tex2D(_BumpMap2,Bump2UV + FinalDistortion);
			
			// Get Average from BumpMap1 and BumpMap2
			fixed4 AvgBump= (Tex2D3 + Tex2D4) / 2;
			
			// Unpack Normals
			fixed4 UnpackNormal1=float4(UnpackNormal(AvgBump).xyz, 1.0);
			
						
			o.Albedo = FinalDiffuse;
			o.Normal = UnpackNormal1;
			o.Emission = FinalReflex;
			o.Specular = _Gloss;
			o.Gloss = _Specular;
			o.Alpha = _Opacity;

			o.Normal = normalize(o.Normal);
		}
	ENDCG
	}
	Fallback "Diffuse"
}