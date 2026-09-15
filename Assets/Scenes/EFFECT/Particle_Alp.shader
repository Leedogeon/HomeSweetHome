// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/Particle_alp"
{
	Properties
	{
		_Particle("Particle", 2D) = "white" {}
		_Paricle_Pow("Paricle_Pow", Float) = 1
		_Particle_Ins("Particle_Ins", Float) = 1
		[HDR]_Particle_Color("Particle_Color", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.5
		#define ASE_VERSION 19801
		#pragma surface surf Standard keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Particle_Color;
		uniform sampler2D _Particle;
		uniform float4 _Particle_ST;
		uniform float _Paricle_Pow;
		uniform float _Particle_Ins;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Particle = i.uv_texcoord * _Particle_ST.xy + _Particle_ST.zw;
			float4 tex2DNode1 = tex2D( _Particle, uv_Particle );
			float3 temp_cast_0 = (_Paricle_Pow).xxx;
			o.Emission = ( ( _Particle_Color * float4( ( pow( tex2DNode1.rgb , temp_cast_0 ) * _Particle_Ins ) , 0.0 ) ) * i.vertexColor ).rgb;
			o.Alpha = ( tex2DNode1.rgb * i.vertexColor.a ).x;
		}

		ENDCG
	}
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.SamplerNode;1;-1056,112;Inherit;True;Property;_Particle;Particle;1;0;Create;True;0;0;0;False;0;False;-1;5b83ddaf1d8e1cd46966b393b3d30ffd;5b83ddaf1d8e1cd46966b393b3d30ffd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;5;-944,-48;Inherit;False;Property;_Paricle_Pow;Paricle_Pow;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;3;-704,48;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-704,-64;Inherit;False;Property;_Particle_Ins;Particle_Ins;3;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-496,0;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;8;-496,-256;Inherit;False;Property;_Particle_Color;Particle_Color;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-208,-96;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;10;-224,112;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;0,256;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-16,64;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;288,0;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;0;Standard;Amplify Shader/Particle_alp;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;5
WireConnection;3;1;5;0
WireConnection;4;0;3;0
WireConnection;4;1;6;0
WireConnection;9;0;8;0
WireConnection;9;1;4;0
WireConnection;12;0;1;5
WireConnection;12;1;10;4
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;0;2;11;0
WireConnection;0;9;12;0
ASEEND*/
//CHKSM=433BC6F84CC5DD4917CEF6B03177CDED6BD9601A