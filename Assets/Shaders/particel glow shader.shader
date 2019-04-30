// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Particle glow shader"
{
	Properties
	{
		[HDR]_Level3Glow("Level 3 Glow", Color) = (0,0,0,0)
		[HDR]_Level5Glow("Level 5 Glow", Color) = (0,0,0,0)
		[HDR]_Level4Glow("Level 4 Glow", Color) = (0,0,0,0)
		[HDR]_Level6Glow("Level 6 Glow", Color) = (0,0,0,0)
		[HDR]_Level1Glow("Level 1 Glow", Color) = (0,0,0,0)
		[HDR]_Level2Glow("Level 2 Glow", Color) = (0,0,0,0)
		_GlowShadowColor("Glow Shadow Color", 2D) = "white" {}
		_WhiteGlowCore("White Glow Core", 2D) = "white" {}
		_45("4-5", Range( 0 , 1)) = 0
		_56("5-6", Range( 0 , 1)) = 0
		_12("1-2", Range( 0 , 1)) = 0
		_23("2-3", Range( 0 , 1)) = 0
		_34("3-4", Range( 0 , 1)) = 0
		_GlowPower("Glow Power", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 2.0
		#pragma exclude_renderers xbox360 xboxone ps4 psp2 n3ds wiiu 
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient nolightmap  nodynlightmap nodirlightmap 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Level1Glow;
		uniform float4 _Level2Glow;
		uniform float _12;
		uniform float4 _Level3Glow;
		uniform float _23;
		uniform float4 _Level4Glow;
		uniform float _34;
		uniform float4 _Level5Glow;
		uniform float _45;
		uniform float4 _Level6Glow;
		uniform float _56;
		uniform sampler2D _GlowShadowColor;
		uniform float4 _GlowShadowColor_ST;
		uniform sampler2D _WhiteGlowCore;
		uniform float4 _WhiteGlowCore_ST;
		uniform float _GlowPower;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 lerpResult11 = lerp( _Level1Glow , _Level2Glow , _12);
			float4 lerpResult14 = lerp( lerpResult11 , _Level3Glow , _23);
			float4 lerpResult18 = lerp( lerpResult14 , _Level4Glow , _34);
			float4 lerpResult23 = lerp( lerpResult18 , _Level5Glow , _45);
			float4 lerpResult27 = lerp( lerpResult23 , _Level6Glow , _56);
			float2 uv_GlowShadowColor = i.uv_texcoord * _GlowShadowColor_ST.xy + _GlowShadowColor_ST.zw;
			float4 tex2DNode6 = tex2D( _GlowShadowColor, uv_GlowShadowColor );
			float2 uv_WhiteGlowCore = i.uv_texcoord * _WhiteGlowCore_ST.xy + _WhiteGlowCore_ST.zw;
			o.Emission = ( ( ( lerpResult27 * tex2DNode6 ) + tex2D( _WhiteGlowCore, uv_WhiteGlowCore ) ) * _GlowPower ).rgb;
			o.Alpha = tex2DNode6.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
212.8;0.8;1081;795;2045.638;1309.768;1.465897;True;False
Node;AmplifyShaderEditor.ColorNode;13;-2124.513,-720.2801;Float;False;Property;_Level3Glow;Level 3 Glow;0;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,1,0.7524569,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-2377.714,-883.321;Float;False;Property;_Level2Glow;Level 2 Glow;5;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-2125.422,-824.6635;Float;False;Property;_12;1-2;10;0;Create;False;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-2347.21,-1043.509;Float;False;Property;_Level1Glow;Level 1 Glow;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.1665316,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-1758.805,-747.6892;Float;False;Property;_Level4Glow;Level 4 Glow;2;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.04428701,0,0.7735849,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;11;-2003.002,-939.6694;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1793.619,-823.3116;Float;False;Property;_23;2-3;11;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;16;-1851.219,-839.3113;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;21;-1458.064,-745.1371;Float;False;Property;_Level5Glow;Level 5 Glow;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.4795277,0,0.6792453,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;-1671.42,-937.712;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;20;-1521.942,-840.9316;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1495.449,-821.0983;Float;False;Property;_34;3-4;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-1133.963,-728.8557;Float;False;Property;_Level6Glow;Level 6 Glow;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.09433961,0.005784979,0.08653124,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;24;-1238.286,-848.1531;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;18;-1382.75,-934.2894;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1191.586,-821.6535;Float;False;Property;_45;4-5;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-841.4619,-832.8577;Float;False;Property;_56;5-6;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;-1074.786,-938.9519;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;28;-916.0716,-841.8884;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-776.0635,16.81111;Float;True;Property;_GlowShadowColor;Glow Shadow Color;6;0;Create;True;0;0;False;0;None;31890676c5b178840848afa665cb5a2f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;27;-733.7398,-941.147;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;4;-799.749,-309.6451;Float;True;Property;_WhiteGlowCore;White Glow Core;7;0;Create;True;0;0;False;0;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-237.9452,-314.2291;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-96.60664,-214.6347;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-312.5489,-101.8688;Float;False;Property;_GlowPower;Glow Power;13;0;Create;True;0;0;False;0;0;3.64;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;68.58432,-219.1405;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;8;299.9405,-55.97966;Float;False;True;0;Float;ASEMaterialInspector;0;0;Unlit;Particle glow shader;False;False;False;False;True;False;True;True;True;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.78;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;False;False;False;False;False;False;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;3;0
WireConnection;11;1;10;0
WireConnection;11;2;12;0
WireConnection;16;0;13;0
WireConnection;14;0;11;0
WireConnection;14;1;16;0
WireConnection;14;2;15;0
WireConnection;20;0;17;0
WireConnection;24;0;21;0
WireConnection;18;0;14;0
WireConnection;18;1;20;0
WireConnection;18;2;19;0
WireConnection;23;0;18;0
WireConnection;23;1;24;0
WireConnection;23;2;22;0
WireConnection;28;0;25;0
WireConnection;27;0;23;0
WireConnection;27;1;28;0
WireConnection;27;2;26;0
WireConnection;5;0;27;0
WireConnection;5;1;6;0
WireConnection;7;0;5;0
WireConnection;7;1;4;0
WireConnection;29;0;7;0
WireConnection;29;1;31;0
WireConnection;8;2;29;0
WireConnection;8;9;6;4
ASEEND*/
//CHKSM=37996DE5276E36E3CFD4B6E63DB5624C2FADFF10