// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|emission-2-RGB,clip-2-A;n:type:ShaderForge.SFN_Tex2d,id:2,x:33012,y:32762,ptlb:node_2,ptin:_node_2,tex:33c099eb04992974a99e40ad1b331f9e,ntxv:0,isnm:False|UVIN-18-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:17,x:33253,y:32818,uv:0;n:type:ShaderForge.SFN_Rotator,id:18,x:33191,y:32712|UVIN-19-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:19,x:32647,y:32484,uv:0;proporder:2;pass:END;sub:END;*/

Shader "Custom/samich" {
    Properties {
        _node_2 ("node_2", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_2; uniform float4 _node_2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float4 node_23 = _Time + _TimeEditor;
                float node_18_ang = node_23.g;
                float node_18_spd = 1.0;
                float node_18_cos = cos(node_18_spd*node_18_ang);
                float node_18_sin = sin(node_18_spd*node_18_ang);
                float2 node_18_piv = float2(0.5,0.5);
                float2 node_18 = (mul(i.uv0.rg-node_18_piv,float2x2( node_18_cos, -node_18_sin, node_18_sin, node_18_cos))+node_18_piv);
                float4 node_2 = tex2D(_node_2,TRANSFORM_TEX(node_18, _node_2));
                clip(node_2.a - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = node_2.rgb;
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_2; uniform float4 _node_2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float4 node_24 = _Time + _TimeEditor;
                float node_18_ang = node_24.g;
                float node_18_spd = 1.0;
                float node_18_cos = cos(node_18_spd*node_18_ang);
                float node_18_sin = sin(node_18_spd*node_18_ang);
                float2 node_18_piv = float2(0.5,0.5);
                float2 node_18 = (mul(i.uv0.rg-node_18_piv,float2x2( node_18_cos, -node_18_sin, node_18_sin, node_18_cos))+node_18_piv);
                float4 node_2 = tex2D(_node_2,TRANSFORM_TEX(node_18, _node_2));
                clip(node_2.a - 0.5);
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Cull Off
            Offset 1, 1
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_2; uniform float4 _node_2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float4 node_25 = _Time + _TimeEditor;
                float node_18_ang = node_25.g;
                float node_18_spd = 1.0;
                float node_18_cos = cos(node_18_spd*node_18_ang);
                float node_18_sin = sin(node_18_spd*node_18_ang);
                float2 node_18_piv = float2(0.5,0.5);
                float2 node_18 = (mul(i.uv0.rg-node_18_piv,float2x2( node_18_cos, -node_18_sin, node_18_sin, node_18_cos))+node_18_piv);
                float4 node_2 = tex2D(_node_2,TRANSFORM_TEX(node_18, _node_2));
                clip(node_2.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
