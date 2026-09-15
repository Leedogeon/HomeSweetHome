// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/Sword_V"
{
	Properties
	{
		_Texture_Main("Texture_Main", 2D) = "white" {}
		_Main_Pow("Main_Pow", Float) = 1
		_Main_Ins("Main_Ins", Float) = 1
		[HDR]_Color_Main("Color_Main", Color) = (1,1,1,1)
		_Texture_Noise("Texture_Noise", 2D) = "white" {}
		_Nosie_Speed("Nosie_Speed", Vector) = (1,0,0,0)
		_Noise_Str("Noise_Str", Float) = 0.15
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
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Texture_Main;
		uniform sampler2D _Texture_Noise;
		uniform float2 _Nosie_Speed;
		uniform float4 _Texture_Noise_ST;
		uniform float _Noise_Str;
		uniform float4 _Texture_Main_ST;
		uniform float _Main_Pow;
		uniform float _Main_Ins;
		uniform float4 _Color_Main;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture_Noise = i.uv_texcoord * _Texture_Noise_ST.xy + _Texture_Noise_ST.zw;
			float2 panner15 = ( 1.0 * _Time.y * _Nosie_Speed + uv_Texture_Noise);
			float2 uv_Texture_Main = i.uv_texcoord * _Texture_Main_ST.xy + _Texture_Main_ST.zw;
			float4 tex2DNode1 = tex2D( _Texture_Main, ( ( (tex2D( _Texture_Noise, panner15 ).rgb).xy * _Noise_Str ) + uv_Texture_Main ) );
			o.Emission = ( float4( ( ( pow( tex2DNode1.r , _Main_Pow ) * _Main_Ins ) * _Color_Main.rgb ) , 0.0 ) * i.vertexColor ).rgb;
			o.Alpha = ( tex2DNode1.r * i.vertexColor.a );
		}

		ENDCG
	}
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2560,-224;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;16;-2512,-80;Inherit;False;Property;_Nosie_Speed;Nosie_Speed;6;0;Create;True;0;0;0;False;0;False;1,0;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;15;-2272,-176;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-2064,-224;Inherit;True;Property;_Texture_Noise;Texture_Noise;5;0;Create;True;0;0;0;False;0;False;-1;f4963e89695b94f4cb0f49967ca289ff;b91cd3643992b5a47bfa9019be1d4f4d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;20;-1920,48;Inherit;False;Property;_Noise_Str;Noise_Str;7;0;Create;True;0;0;0;False;0;False;0.15;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;18;-1760,-208;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1520,-96;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1520,160;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-1266,53.5;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1017,-47.5;Inherit;False;Property;_Main_Pow;Main_Pow;2;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1120,48;Inherit;True;Property;_Texture_Main;Texture_Main;1;0;Create;True;0;0;0;False;0;False;-1;None;148f1274f28b03c4e8410ed07cf50c8b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;5;-800,-48;Inherit;False;Property;_Main_Ins;Main_Ins;3;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;2;-800,32;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-624,16;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-736,-272;Inherit;False;Property;_Color_Main;Color_Main;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-430,-93.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;9;-512,144;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-240,48;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-256,208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;0;Standard;Amplify Shader/Sword_V;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;14;0
WireConnection;15;2;16;0
WireConnection;13;1;15;0
WireConnection;18;0;13;5
WireConnection;19;0;18;0
WireConnection;19;1;20;0
WireConnection;22;0;19;0
WireConnection;22;1;21;0
WireConnection;1;1;22;0
WireConnection;2;0;1;1
WireConnection;2;1;4;0
WireConnection;3;0;2;0
WireConnection;3;1;5;0
WireConnection;6;0;3;0
WireConnection;6;1;7;5
WireConnection;10;0;6;0
WireConnection;10;1;9;0
WireConnection;11;0;1;1
WireConnection;11;1;9;4
WireConnection;0;2;10;0
WireConnection;0;9;11;0
ASEEND*/
//CHKSM=5B81E5E33AB1F0A941A236E880F1A5F5395318AC