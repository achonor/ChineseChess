// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Achonor/UnlitWithShadowsAplha" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
		SubShader{
			Tags {"Queue" = "AlphaTest" "RenderType" = "Opaque"}
			Pass {
				Tags {"LightMode" = "ForwardBase"}
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma multi_compile_fwdbase
					#pragma fragmentoption ARB_fog_exp2
					#pragma fragmentoption ARB_precision_hint_fastest

					#include "UnityCG.cginc"
					#include "AutoLight.cginc"

					struct v2f
					{
						float4	pos			: SV_POSITION;
						float2	uv			: TEXCOORD0;
						LIGHTING_COORDS(1,2)
					};
					float4 _MainTex_ST;
					v2f vert(appdata_tan v)
					{
						v2f o;

						o.pos = UnityObjectToClipPos(v.vertex);
						o.uv = TRANSFORM_TEX(v.texcoord, _MainTex).xy;
						TRANSFER_VERTEX_TO_FRAGMENT(o);
						return o;
					}
					sampler2D _MainTex;
					float _Cutoff;
					fixed4 frag(v2f i) : COLOR
					{
						float4 color;
						fixed atten = LIGHT_ATTENUATION(i);	// Light attenuation + shadows.
						//fixed atten = SHADOW_ATTENUATION(i); // Shadows ONLY.
						color = tex2D(_MainTex, i.uv) * atten;
						clip(color.a - _Cutoff);
						return color;
					}
				ENDCG
			}
			Pass {
				Tags {"Queue" = "AlphaTest" "LightMode" = "ForwardAdd"}
				Blend One One
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma multi_compile_fwdadd_fullshadows
					#pragma fragmentoption ARB_fog_exp2
					#pragma fragmentoption ARB_precision_hint_fastest

					#include "UnityCG.cginc"
					#include "AutoLight.cginc"

					struct v2f
					{
						float4	pos			: SV_POSITION;
						float2	uv			: TEXCOORD0;
						LIGHTING_COORDS(1,2)
					};
					float4 _MainTex_ST;
					v2f vert(appdata_tan v)
					{
						v2f o;

						o.pos = UnityObjectToClipPos(v.vertex);
						o.uv = TRANSFORM_TEX(v.texcoord, _MainTex).xy;
						TRANSFER_VERTEX_TO_FRAGMENT(o);
						return o;
					}
					sampler2D _MainTex;
					float _Cutoff;
					fixed4 frag(v2f i) : COLOR
					{
						float4 color;
						fixed atten = LIGHT_ATTENUATION(i);	// Light attenuation + shadows.
						//fixed atten = SHADOW_ATTENUATION(i); // Shadows ONLY.
						color = tex2D(_MainTex, i.uv) * atten;
						clip(color.a - _Cutoff);
						return color;
					}
				ENDCG
			}
	}
		FallBack "VertexLit"
}