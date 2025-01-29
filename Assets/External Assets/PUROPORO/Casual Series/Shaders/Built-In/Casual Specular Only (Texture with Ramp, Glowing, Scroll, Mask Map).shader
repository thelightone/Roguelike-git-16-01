// Made with Amplify Shader Editor v1.9.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PUROPORO/Built-in/Casual Specular Only (Texture with Ramp, Glowing, Scroll, Mask Map)"
{
	Properties
	{
		_BaseMap("Base (RGB)", 2D) = "white" {}
		_MaskMap("Mask Map", 2D) = "white" {}
		_SpecularRampOffsetH("Specular Ramp Offset (Horizontal)", Range( 0 , 1)) = 0.25
		_VFXGlowingColor("VFX Glowing Color", Color) = (1,1,1,1)
		_VFXGlowingPower("VFX Glowing Power", Range( -1 , 1)) = 0
		_VFXScrollRampTexture("VFX Scroll Ramp Texture", 2D) = "white" {}
		_VFXScrollSpeed("VFX Scroll Speed", Range( -1 , 1)) = 0
		[ASEEnd]_VFXScrollRotation("VFX Scroll Rotation", Range( 0 , 6.4)) = 1.6
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			#define ASE_USING_SAMPLING_MACROS 1


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityShaderVariables.cginc"
			#include "AutoLight.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
			#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
			#else//ASE Sampling Macros
			#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
			#endif//ASE Sampling Macros
			


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			//This is a late directive
			
			UNITY_DECLARE_TEX2D_NOSAMPLER(_BaseMap);
			uniform float4 _BaseMap_ST;
			SamplerState sampler_BaseMap;
			uniform float _SpecularRampOffsetH;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_MaskMap);
			uniform float4 _MaskMap_ST;
			SamplerState sampler_MaskMap;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_VFXScrollRampTexture);
			uniform float _VFXScrollSpeed;
			uniform float _VFXScrollRotation;
			SamplerState sampler_VFXScrollRampTexture;
			uniform float _VFXGlowingPower;
			uniform float4 _VFXGlowingColor;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_BaseMap = i.ase_texcoord1.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
				float4 ase_lightColor = 0;
				#else //aselc
				float4 ase_lightColor = _LightColor0;
				#endif //aselc
				float3 ase_worldNormal = i.ase_texcoord2.xyz;
				float3 worldSpaceLightDir = UnityWorldSpaceLightDir(WorldPosition);
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult15 = normalize( ( worldSpaceLightDir + ase_worldViewDir ) );
				float dotResult13 = dot( ase_worldNormal , normalizeResult15 );
				float4 appendResult46 = (float4((dotResult13*0.5 + 0.5) , _SpecularRampOffsetH , 0.0 , 0.0));
				float2 uv_MaskMap = i.ase_texcoord1.xy * _MaskMap_ST.xy + _MaskMap_ST.zw;
				float4 tex2DNode63 = SAMPLE_TEXTURE2D( _MaskMap, sampler_MaskMap, uv_MaskMap );
				float mulTime72 = _Time.y * _VFXScrollSpeed;
				float cos76 = cos( _VFXScrollRotation );
				float sin76 = sin( _VFXScrollRotation );
				float2 rotator76 = mul( WorldPosition.xy - float2( 0,0 ) , float2x2( cos76 , -sin76 , sin76 , cos76 )) + float2( 0,0 );
				
				
				finalColor = ( ( ( ( SAMPLE_TEXTURE2D( _BaseMap, sampler_BaseMap, uv_BaseMap ) * ase_lightColor ) + ( SAMPLE_TEXTURE2D( _BaseMap, sampler_BaseMap, appendResult46.xy ) * tex2DNode63.r ) ) + ( SAMPLE_TEXTURE2D( _VFXScrollRampTexture, sampler_VFXScrollRampTexture, ( (0.0*1.0 + mulTime72) + rotator76 ) ) * tex2DNode63.g ) ) + ( _VFXGlowingPower * _VFXGlowingColor ) );
				return finalColor;
			}
			ENDCG
		}
	}
	
	
	Fallback Off
}
/*ASEBEGIN
Version=19100
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-1677.945,322.4286;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;9;-1730.6,-117.3199;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1455.497,304.7621;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;8;-1701.057,117.8141;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;15;-1288.711,178.2173;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;13;-1035.42,116.9019;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1279.923,27.71151;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;60;-899.2222,198.2066;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1084.791,394.046;Inherit;False;Property;_SpecularRampOffsetH;Specular Ramp Offset (Horizontal);2;0;Create;False;0;0;0;False;0;False;0.25;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;46;-705.6564,334.629;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-544.1091,109.7355;Inherit;True;Property;_BaseMap;Base (RGB);0;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;62;-452.1084,593.6271;Inherit;True;Property;_MaskMap;Mask Map;1;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;4;-273.3439,306.0976;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;63;-223.2639,593.6274;Inherit;True;Property;_TextureSample3;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;112.8236,311.3622;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;396.6843,287.2068;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;48;-270.0274,-42.29554;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;10;33.88691,158.8705;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;215.5693,56.94099;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;555.5164,285.4534;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;379.0965,621.9624;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;69;38.02541,862.5156;Inherit;True;Property;_TextureSample2;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-214.3348,1073.694;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;71;-449.5829,934.4219;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;72;-640.4543,982.167;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;73;-632.7212,1096.263;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RotatorNode;76;-423.1706,1096.955;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-921.6236,976.9139;Inherit;False;Property;_VFXScrollSpeed;VFX Scroll Speed;6;0;Create;False;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-727.9764,1234.377;Inherit;False;Property;_VFXScrollRotation;VFX Scroll Rotation;7;0;Create;False;0;0;0;False;0;False;1.6;0;0;6.4;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;79;-239.2848,861.778;Inherit;True;Property;_VFXScrollRampTexture;VFX Scroll Ramp Texture;5;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;868.8233,448.2054;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;64;635.7899,496.2067;Inherit;False;Property;_VFXGlowingColor;VFX Glowing Color;3;0;Create;False;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;566.6165,423.7085;Inherit;False;Property;_VFXGlowingPower;VFX Glowing Power;4;0;Create;False;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;45;1224.187,285.0791;Float;False;True;-1;2;;100;5;PUROPORO/Built-in/Casual Specular Only (Texture with Ramp, Glowing, Scroll, Mask Map);0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;;0;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;True;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;1045.897,284.73;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
WireConnection;14;0;9;0
WireConnection;14;1;7;0
WireConnection;15;0;14;0
WireConnection;13;0;8;0
WireConnection;13;1;15;0
WireConnection;60;0;13;0
WireConnection;60;1;61;0
WireConnection;60;2;61;0
WireConnection;46;0;60;0
WireConnection;46;1;22;0
WireConnection;4;0;1;0
WireConnection;4;1;46;0
WireConnection;63;0;62;0
WireConnection;67;0;4;0
WireConnection;67;1;63;1
WireConnection;6;0;49;0
WireConnection;6;1;67;0
WireConnection;48;0;1;0
WireConnection;49;0;48;0
WireConnection;49;1;10;0
WireConnection;68;0;6;0
WireConnection;68;1;80;0
WireConnection;80;0;69;0
WireConnection;80;1;63;2
WireConnection;69;0;79;0
WireConnection;69;1;70;0
WireConnection;70;0;71;0
WireConnection;70;1;76;0
WireConnection;71;2;72;0
WireConnection;72;0;77;0
WireConnection;76;0;73;0
WireConnection;76;2;78;0
WireConnection;65;0;66;0
WireConnection;65;1;64;0
WireConnection;45;0;81;0
WireConnection;81;0;68;0
WireConnection;81;1;65;0
ASEEND*/
//CHKSM=90E172C73AB515EB9FEBB42759931BD924CE0B7D