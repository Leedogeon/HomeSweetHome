// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/EFFECT/BH"
{
	Properties
	{
		_Texture_Main("Texture_Main", 2D) = "white" {}
		_Color_Main("Color_Main", Color) = (1,1,1,1)
		_Main_Pow("Main_Pow", Float) = 1
		_Main_Ins("Main_Ins", Float) = 1
		_Main_Upanner("Main_Upanner", Float) = 0
		_Main_Vpanner("Main_Vpanner", Float) = 0
		_Texture_Mask("Texture_Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#define ASE_VERSION 19801
		#pragma surface surf Standard keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Color_Main;
		uniform sampler2D _Texture_Main;
		uniform float _Main_Upanner;
		uniform float _Main_Vpanner;
		uniform float4 _Texture_Main_ST;
		uniform float _Main_Pow;
		uniform float _Main_Ins;
		uniform sampler2D _Texture_Mask;
		uniform float4 _Texture_Mask_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult13 = (float2(_Main_Upanner , _Main_Vpanner));
			float2 uv_Texture_Main = i.uv_texcoord * _Texture_Main_ST.xy + _Texture_Main_ST.zw;
			float2 panner12 = ( 1.0 * _Time.y * appendResult13 + uv_Texture_Main);
			float4 tex2DNode1 = tex2D( _Texture_Main, panner12 );
			o.Emission = ( ( _Color_Main * ( pow( tex2DNode1.r , _Main_Pow ) * _Main_Ins ) ) * i.vertexColor ).rgb;
			float2 uv_Texture_Mask = i.uv_texcoord * _Texture_Mask_ST.xy + _Texture_Mask_ST.zw;
			o.Alpha = ( ( tex2DNode1.r * tex2D( _Texture_Mask, uv_Texture_Mask ).r ) * i.vertexColor.a );
		}

		ENDCG
	}
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.RangedFloatNode;14;-1680,288;Inherit;False;Property;_Main_Upanner;Main_Upanner;5;0;Create;True;0;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1680,368;Inherit;False;Property;_Main_Vpanner;Main_Vpanner;6;0;Create;True;0;0;0;False;0;False;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-1440,272;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1584,96;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;12;-1232,208;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-992,144;Inherit;True;Property;_Texture_Main;Texture_Main;1;0;Create;True;0;0;0;False;0;False;-1;None;5f2eb7a294b27004d9e226b0efac5da1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;5;-863,1.5;Inherit;False;Property;_Main_Pow;Main_Pow;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;2;-656,80;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-560,-16;Inherit;False;Property;_Main_Ins;Main_Ins;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-369,69.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-384,-176;Inherit;False;Property;_Color_Main;Color_Main;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;16;-608,416;Inherit;True;Property;_Texture_Mask;Texture_Mask;7;0;Create;True;0;0;0;False;0;False;-1;None;2ae0469d59be6064c9f1c87772190a5a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-128,16;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;9;-448,192;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-288,384;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;32,128;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;16,272;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;224,0;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;0;Standard;Amplify Shader/EFFECT/BH;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;14;0
WireConnection;13;1;15;0
WireConnection;12;0;11;0
WireConnection;12;2;13;0
WireConnection;1;1;12;0
WireConnection;2;0;1;1
WireConnection;2;1;5;0
WireConnection;3;0;2;0
WireConnection;3;1;6;0
WireConnection;7;0;4;0
WireConnection;7;1;3;0
WireConnection;17;0;1;1
WireConnection;17;1;16;1
WireConnection;8;0;7;0
WireConnection;8;1;9;0
WireConnection;10;0;17;0
WireConnection;10;1;9;4
WireConnection;0;2;8;0
WireConnection;0;9;10;0
ASEEND*/
//CHKSM=BA1F0125D870F197FE8E22F24261B753A632CFF0