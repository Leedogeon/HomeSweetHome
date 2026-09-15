// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/EFFECT"
{
	Properties
	{
		_TEST("TEST", 2D) = "white" {}
		_Test_Pow("Test_Pow", Float) = 1
		_Test_Ins("Test_Ins", Float) = 1
		[HDR]_TEST_Color("TEST_Color", Color) = (1,1,1,1)
		_TEST_Noise("TEST_Noise", 2D) = "white" {}
		_Noise_Speed("Noise_Speed", Vector) = (0.2,-0.2,0,0)
		_Noise_Str("Noise_Str", Float) = 0.03
		_Test_Dissolve("Test_Dissolve", 2D) = "white" {}
		_Dissolve_Speed("Dissolve_Speed", Vector) = (0.2,-0.2,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#define ASE_VERSION 19801
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv3_texcoord3;
		};

		uniform float4 _TEST_Color;
		uniform sampler2D _TEST;
		uniform sampler2D _TEST_Noise;
		uniform float2 _Noise_Speed;
		uniform float4 _TEST_Noise_ST;
		uniform float _Noise_Str;
		uniform float4 _TEST_ST;
		uniform float _Test_Pow;
		uniform float _Test_Ins;
		uniform sampler2D _Test_Dissolve;
		uniform float2 _Dissolve_Speed;
		uniform float4 _Test_Dissolve_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TEST_Noise = i.uv_texcoord * _TEST_Noise_ST.xy + _TEST_Noise_ST.zw;
			float2 panner15 = ( 1.0 * _Time.y * _Noise_Speed + uv_TEST_Noise);
			float2 temp_output_19_0 = ( (tex2D( _TEST_Noise, panner15 ).rgb).xy * _Noise_Str );
			float2 uv_TEST = i.uv_texcoord * _TEST_ST.xy + _TEST_ST.zw;
			float4 tex2DNode1 = tex2D( _TEST, ( temp_output_19_0 + uv_TEST ) );
			o.Emission = ( ( _TEST_Color * ( pow( tex2DNode1.r , _Test_Pow ) * _Test_Ins ) ) * i.vertexColor ).rgb;
			float2 uv_Test_Dissolve = i.uv_texcoord * _Test_Dissolve_ST.xy + _Test_Dissolve_ST.zw;
			float2 panner24 = ( 1.0 * _Time.y * _Dissolve_Speed + uv_Test_Dissolve);
			o.Alpha = ( saturate( ( tex2DNode1.r * ( ( tex2D( _Test_Dissolve, ( temp_output_19_0 + panner24 ) ).r + ( 1.0 - ( i.uv_texcoord.x + 0.0 ) ) ) + i.uv3_texcoord3.x ) ) ) * i.vertexColor.a );
		}

		ENDCG
	}
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.Vector2Node;16;-2272,-288;Inherit;False;Property;_Noise_Speed;Noise_Speed;6;0;Create;True;0;0;0;False;0;False;0.2,-0.2;0.1,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2320,-448;Inherit;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;15;-2016,-384;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-1808,-432;Inherit;True;Property;_TEST_Noise;TEST_Noise;5;0;Create;True;0;0;0;False;0;False;-1;da221c15092aadf45acbc4ef46ca263e;da221c15092aadf45acbc4ef46ca263e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.Vector2Node;25;-2176,256;Inherit;False;Property;_Dissolve_Speed;Dissolve_Speed;9;0;Create;True;0;0;0;False;0;False;0.2,-0.2;-0.2,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ComponentMaskNode;18;-1504,-384;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1664,-224;Inherit;False;Property;_Noise_Str;Noise_Str;7;0;Create;True;0;0;0;False;0;False;0.03;0.015;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-2224,96;Inherit;False;0;28;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1264,-368;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;24;-1920,160;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1776,464;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1648,160;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-1520,464;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1296,-80;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-994.9199,-170.2052;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;28;-1488,144;Inherit;True;Property;_Test_Dissolve;Test_Dissolve;8;0;Create;True;0;0;0;False;0;False;-1;5f2eb7a294b27004d9e226b0efac5da1;5f2eb7a294b27004d9e226b0efac5da1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.OneMinusNode;32;-1360,384;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-672,-160;Inherit;False;Property;_Test_Pow;Test_Pow;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-800,-64;Inherit;True;Property;_TEST;TEST;1;0;Create;True;0;0;0;False;0;False;-1;fae3fe042f6beec4888e50f6eba93f2c;fae3fe042f6beec4888e50f6eba93f2c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1120,272;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;34;-1120,512;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;5;-496,-80;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-448,-160;Inherit;False;Property;_Test_Ins;Test_Ins;3;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-784,352;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-256,-112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-544,272;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-256,-384;Inherit;False;Property;_TEST_Color;TEST_Color;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;2,2,2,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;24.07996,-205.2052;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;10;-64,0;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;36;-112,272;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;208,-112;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;176,96;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;416,-192;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;0;Unlit;Amplify Shader/EFFECT;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;8;5;False;;1;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;14;0
WireConnection;15;2;16;0
WireConnection;17;1;15;0
WireConnection;18;0;17;5
WireConnection;19;0;18;0
WireConnection;19;1;20;0
WireConnection;24;0;26;0
WireConnection;24;2;25;0
WireConnection;27;0;19;0
WireConnection;27;1;24;0
WireConnection;31;0;29;1
WireConnection;22;0;19;0
WireConnection;22;1;23;0
WireConnection;28;1;27;0
WireConnection;32;0;31;0
WireConnection;1;1;22;0
WireConnection;30;0;28;1
WireConnection;30;1;32;0
WireConnection;5;0;1;1
WireConnection;5;1;7;0
WireConnection;33;0;30;0
WireConnection;33;1;34;1
WireConnection;6;0;5;0
WireConnection;6;1;8;0
WireConnection;35;0;1;1
WireConnection;35;1;33;0
WireConnection;11;0;9;0
WireConnection;11;1;6;0
WireConnection;36;0;35;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;13;0;36;0
WireConnection;13;1;10;4
WireConnection;0;2;12;0
WireConnection;0;9;13;0
ASEEND*/
//CHKSM=B615B079AA5E0032CD5855F63C11C768401D2F85