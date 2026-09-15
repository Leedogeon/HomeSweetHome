// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify Shader/EFFECT_FX_Thunder"
{
	Properties
	{
		_FX_Thunder("FX_Thunder", 2D) = "white" {}
		_Pow("Pow", Float) = 1
		_Ins("Ins", Float) = 1
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Texture_Noise("Texture_Noise", 2D) = "white" {}
		_Noise_Upaner("Noise_Upaner", Float) = 0
		_Noise_Vpaner("Noise_Vpaner", Float) = 0
		_Noise_Str("Noise_Str", Range( 0 , 1)) = 0
		_Texture_Vertex_Noise("Texture_Vertex_Noise", 2D) = "white" {}
		_VNoise_Upaner("VNoise_Upaner", Float) = 0
		_VNoise_Vpaner("VNoise_Vpaner", Float) = 0
		_VNoise_Str("VNoise_Str", Range( 0 , 3)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#define ASE_VERSION 19801
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Texture_Vertex_Noise;
		uniform float _VNoise_Upaner;
		uniform float _VNoise_Vpaner;
		uniform float4 _Texture_Vertex_Noise_ST;
		uniform float _VNoise_Str;
		uniform float4 _Color;
		uniform sampler2D _FX_Thunder;
		uniform sampler2D _Texture_Noise;
		uniform float _Noise_Upaner;
		uniform float _Noise_Vpaner;
		uniform float4 _Texture_Noise_ST;
		uniform float _Noise_Str;
		uniform float4 _FX_Thunder_ST;
		uniform float _Pow;
		uniform float _Ins;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_normalOS = v.normal.xyz;
			float2 appendResult25 = (float2(_VNoise_Upaner , _VNoise_Vpaner));
			float2 uv_Texture_Vertex_Noise = v.texcoord.xy * _Texture_Vertex_Noise_ST.xy + _Texture_Vertex_Noise_ST.zw;
			float2 panner26 = ( 1.0 * _Time.y * appendResult25 + uv_Texture_Vertex_Noise);
			v.vertex.xyz += ( ase_normalOS * ( tex2Dlod( _Texture_Vertex_Noise, float4( panner26, 0, 0.0) ).r * _VNoise_Str ) );
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult13 = (float2(_Noise_Upaner , _Noise_Vpaner));
			float2 uv_Texture_Noise = i.uv_texcoord * _Texture_Noise_ST.xy + _Texture_Noise_ST.zw;
			float2 panner12 = ( 1.0 * _Time.y * appendResult13 + uv_Texture_Noise);
			float2 uv_FX_Thunder = i.uv_texcoord * _FX_Thunder_ST.xy + _FX_Thunder_ST.zw;
			float4 tex2DNode1 = tex2D( _FX_Thunder, ( ( (tex2D( _Texture_Noise, panner12 ).rgb).xy * _Noise_Str ) + uv_FX_Thunder ) );
			o.Emission = ( ( _Color * ( pow( tex2DNode1.r , _Pow ) * _Ins ) ) * i.vertexColor ).rgb;
			o.Alpha = ( tex2DNode1.r * i.vertexColor.a );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.RangedFloatNode;14;-2752,-224;Inherit;False;Property;_Noise_Upaner;Noise_Upaner;6;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2752,-128;Inherit;False;Property;_Noise_Vpaner;Noise_Vpaner;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2624,-384;Inherit;False;0;10;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;13;-2528,-224;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;12;-2336,-288;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;10;-2096,-352;Inherit;True;Property;_Texture_Noise;Texture_Noise;5;0;Create;True;0;0;0;False;0;False;-1;None;f4963e89695b94f4cb0f49967ca289ff;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ComponentMaskNode;16;-1712,-304;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1824,-128;Inherit;False;Property;_Noise_Str;Noise_Str;8;0;Create;True;0;0;0;False;0;False;0;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1488,-224;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1872,-48;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-1920,640;Inherit;False;Property;_VNoise_Upaner;VNoise_Upaner;10;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1920,736;Inherit;False;Property;_VNoise_Vpaner;VNoise_Vpaner;11;0;Create;True;0;0;0;False;0;False;0;0.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-1320,-104.5;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1696,640;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-1792,480;Inherit;False;0;21;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1200,-16;Inherit;True;Property;_FX_Thunder;FX_Thunder;1;0;Create;True;0;0;0;False;0;False;-1;b0308bf2aa7c38445a57b8a1dbd04bac;b0308bf2aa7c38445a57b8a1dbd04bac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;31;-1088,-176;Inherit;False;Property;_Pow;Pow;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;26;-1504,576;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;30;-880,-64;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-862.0632,-176.9799;Inherit;False;Property;_Ins;Ins;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-1232,480;Inherit;True;Property;_Texture_Vertex_Noise;Texture_Vertex_Noise;9;0;Create;True;0;0;0;False;0;False;-1;None;5f2eb7a294b27004d9e226b0efac5da1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;28;-1216,736;Inherit;False;Property;_VNoise_Str;VNoise_Str;12;0;Create;True;0;0;0;False;0;False;0;0.5;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-784,-400;Inherit;False;Property;_Color;Color;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;4,4,4,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-704,-112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;6;-624,144;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;19;-896,304;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-848,592;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-400,-224;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-576,480;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-192,16;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-352,208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;128,-112;Float;False;True;-1;2;AmplifyShaderEditor.MaterialInspector;0;0;Unlit;Amplify Shader/EFFECT_FX_Thunder;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;14;0
WireConnection;13;1;15;0
WireConnection;12;0;11;0
WireConnection;12;2;13;0
WireConnection;10;1;12;0
WireConnection;16;0;10;5
WireConnection;17;0;16;0
WireConnection;17;1;18;0
WireConnection;9;0;17;0
WireConnection;9;1;2;0
WireConnection;25;0;22;0
WireConnection;25;1;23;0
WireConnection;1;1;9;0
WireConnection;26;0;24;0
WireConnection;26;2;25;0
WireConnection;30;0;1;1
WireConnection;30;1;31;0
WireConnection;21;1;26;0
WireConnection;29;0;30;0
WireConnection;29;1;32;0
WireConnection;27;0;21;1
WireConnection;27;1;28;0
WireConnection;4;0;5;0
WireConnection;4;1;29;0
WireConnection;20;0;19;0
WireConnection;20;1;27;0
WireConnection;7;0;4;0
WireConnection;7;1;6;0
WireConnection;8;0;1;1
WireConnection;8;1;6;4
WireConnection;0;2;7;0
WireConnection;0;9;8;0
WireConnection;0;11;20;0
ASEEND*/
//CHKSM=F0E546F38D91325F576E092FF9EC1C2727F8533C