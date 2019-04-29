// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effects/Explosions/Particles/Alpha Blended Light Soft" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_ColorStrength ("Color strength", Float) = 1.0
	_DiffuseThreshold ("Lighting Threshold", Range(-1.1,1)) = 0.1
    _Diffusion ("Diffusion", Range(0.1,10)) = 1
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
}

SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		
		Pass {
			Tags {"LightMode" = "ForwardBase"}
		  
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off 
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma fragmentoption ARB_precision_hint_fastest
			//#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			fixed4 _LightColor0;
			fixed _DiffuseThreshold;
            fixed4 _SpecColor;
            fixed _Diffusion;
			fixed _ColorStrength;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				fixed3 normalDir : TEXCOORD1;
                fixed4 lightDir : TEXCOORD2;
                fixed3 viewDir : TEXCOORD3;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD4;
				#endif

			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				
				o.normalDir = normalize( mul( half4( v.normal, 0.0 ), unity_WorldToObject ).xyz );
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.viewDir = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz );
                half3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz;
                o.lightDir = fixed4( normalize( lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w) ), lerp(1.0 , 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w) );

				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : COLOR
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif
				
				fixed nDotL = saturate(dot(i.normalDir, i.lightDir.xyz)) * _LightColor0.xyz;
				fixed diffuseCutoff = saturate( ( max(_DiffuseThreshold, nDotL) - _DiffuseThreshold ) * _Diffusion);

				fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);
				col.a = saturate(col.a * _TintColor.a);
				
				fixed3 ambientLight = diffuseCutoff * _TintColor + _TintColor;
                fixed3 diffuseReflection = col.xyz * diffuseCutoff;
                fixed3 lightFinal = ambientLight + diffuseReflection;

				return fixed4(saturate(lightFinal * _ColorStrength * _LightColor0.rgb * _LightColor0.w * 4), col.a);
			}
			ENDCG 
		}
		 
		 Pass {
            Tags {"LightMode" = "ForwardAdd"}
			ZWrite Off
			Cull Off 
            Blend One One
           
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdadd
				#pragma multi_compile_particles
                #pragma fragmentoption ARB_precision_hint_fastest
               
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
				
				sampler2D _MainTex;
				fixed4 _TintColor;
				fixed4 _LightColor0;
				fixed _DiffuseThreshold;
				fixed4 _SpecColor;
				fixed _Diffusion;
				fixed _ColorStrength;

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					float3 normal : NORMAL;
				};
               
				float4 _MainTex_ST;
				sampler2D _CameraDepthTexture;
				float _InvFade;

                struct v2f
                {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					fixed3 normalDir : TEXCOORD1;
					fixed4 lightDir : TEXCOORD2;
					fixed3 viewDir : TEXCOORD3;
                    LIGHTING_COORDS(4,5)
					//#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD6;
					//#endif
                };
 
                v2f vert (appdata_t v)
                {
                    v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					
					#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos (o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;

					o.normalDir = normalize( mul( half4( v.normal, 0.0 ), unity_WorldToObject ).xyz );
					float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
					o.viewDir = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz );
					half3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz;
					o.lightDir = fixed4( normalize( lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w) ), lerp(1.0 , 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w) );
                    
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }
               
				float4 frag(v2f i) : COLOR
				{
					#ifdef SOFTPARTICLES_ON
					float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
					float partZ = i.projPos.z;
					float fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;
					#endif

					float atten = LIGHT_ATTENUATION(i);
					fixed nDotL = saturate(dot(i.normalDir, i.lightDir.xyz)) * _LightColor0.xyz;
					fixed diffuseCutoff = saturate( ( max(_DiffuseThreshold, nDotL) - _DiffuseThreshold ) * _Diffusion);

					fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord);
					col.a = saturate(col.a * _TintColor.a);
					fixed3 ambientLight = diffuseCutoff * _TintColor + _TintColor;
					return fixed4(saturate(ambientLight * _ColorStrength * _LightColor0.rgb * _LightColor0.w * atten * col.a), 1);
					
				}
           ENDCG
        }

	}	
}