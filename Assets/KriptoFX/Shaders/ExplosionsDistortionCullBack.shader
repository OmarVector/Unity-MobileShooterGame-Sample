// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effects/Explosions/Distortion/CullBack" {
Properties {
        _TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "black" {}
        _DuDvMap ("DuDv Map", 2D) = "black" {}
		_ColorStrength ("Color Strength", Float) = 1
		_BumpAmt ("Distortion", Float) = 10
		_InvFade ("Soft Particles Factor", Float) = 1.0
}

Category {

	Tags { "Queue"="Transparent"  "IgnoreProjector"="True"  "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Back 
	Lighting Off 
	ZWrite Off 
	Fog { Mode Off}

	SubShader {
		GrabPass {							
			Name "_GrabTexture"
 		}
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_particles
#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord: TEXCOORD0;
	float4 color : COLOR;
};

struct v2f {
	float4 vertex : POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uvmain : TEXCOORD2;
	float4 color : COLOR;
	#ifdef SOFTPARTICLES_ON
		float4 projPos : TEXCOORD3;
	#endif
};

sampler2D _MainTex;
sampler2D _DuDvMap;

float _BumpAmt;
float _ColorStrength;
sampler2D _GrabTexture;
float4 _GrabTexture_TexelSize;
float4 _TintColor;


float4 _DuDvMap_ST;
float4 _MainTex_ST;

v2f vert (appdata_t v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	#ifdef SOFTPARTICLES_ON
		o.projPos = ComputeScreenPos (UnityObjectToClipPos(v.vertex));
		COMPUTE_EYEDEPTH(o.projPos.z);
	#endif
	o.color = v.color;
	#if UNITY_UV_STARTS_AT_TOP
	float scale = -1.0;
	#else
	float scale = 1.0;
	#endif
	o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
	o.uvgrab.zw = o.vertex.zw;
	o.uvbump = TRANSFORM_TEX( v.texcoord, _DuDvMap );
	o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
	
	return o;
}

sampler2D _CameraDepthTexture;
float _InvFade;

half4 frag( v2f i ) : COLOR
{
	#ifdef SOFTPARTICLES_ON
		if(_InvFade > 0.0001)	{
		float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
		float depth = saturate (_InvFade * (sceneZ-i.projPos.z));
		i.color.a *= depth * depth;
	}
	#endif
	
	float3 bump = (tex2D( _DuDvMap, i.uvbump )).rgb;
	int2 rg = bump.rg * 255.0;
	float2 offset = bump.rg * 2 - 1;
	if(rg.x == rg.y && (rg.x == 127 || rg.x == 128)) {
		offset = float2(0.0, 0.0);
	}
	offset = offset * _BumpAmt * _GrabTexture_TexelSize.xy * i.color.a;
	i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

	
	half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
	fixed4 tex = tex2D(_MainTex, i.uvmain) * i.color;
	col.rgb *= i.color.rgb;
	fixed4 res = col + tex * _ColorStrength * _TintColor * i.color.a;
    res.a = saturate(res.a);
	return res;
}
ENDCG
		}
	}

	FallBack "Effects/Distortion/Free/CullOff"

}

}

