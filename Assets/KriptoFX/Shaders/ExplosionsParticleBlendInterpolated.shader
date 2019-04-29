// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effects/Explosions/Particles/InterpolatedAlphaBlended" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_ColorStrength ("Color strength", Float) = 1.0
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Float) = 0.5
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off 
	ZWrite Off

	SubShader {
		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			fixed4 _LightColor0;
			fixed _ColorStrength;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD1;
				#endif
			};
			
			float4 _MainTex_ST;
			float4 _MainTex_NextFrame;
			float InterpolationValue;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord.zw = v.texcoord * _MainTex_NextFrame.xy + _MainTex_NextFrame.zw;
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;
			
			half4 frag (v2f i) : COLOR
			{
				#ifdef SOFTPARTICLES_ON
				if(_InvFade > 0.0001)	{
					float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
					float partZ = i.projPos.z;
					fixed fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;
				}
				#endif

				fixed4 currentFrame =  tex2D(_MainTex, i.texcoord.xy);
				fixed4 nextFrame =  tex2D(_MainTex, i.texcoord.zw);
				
				half4 col = 2.0f * i.color * lerp(currentFrame, nextFrame, InterpolationValue);
				col.rgb *= _TintColor.rgb;
				col.a = saturate(col.a * _TintColor.a);
				fixed gray = 0.299*col.r + 0.587*col.g + 0.114*col.b;
				
				#if UNITY_VERSION >= 500
				half3 lightCol = lerp(col.rgb * _LightColor0.rgb * _LightColor0.w , col.rgb, gray);
				#else 
				half3 lightCol = lerp(col.rgb * _LightColor0.rgb * _LightColor0.w * 4, col.rgb, gray);
				#endif

				return half4(lightCol*_ColorStrength, col.a);
			}
			ENDCG 
		}
	}	
}
}
