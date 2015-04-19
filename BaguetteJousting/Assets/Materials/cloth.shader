// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32519,y:32615|emission-2-RGB,amdfl-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:2,x:32844,y:32541,ptlb:MainTex,ptin:_MainTex,tex:3d1d33dabd29a8747af5312eb93385d6,ntxv:0,isnm:False|UVIN-13-UVOUT;n:type:ShaderForge.SFN_Time,id:12,x:33122,y:32846;n:type:ShaderForge.SFN_Panner,id:13,x:33061,y:32641,spu:1,spv:1|DIST-15-OUT;n:type:ShaderForge.SFN_Multiply,id:15,x:33245,y:32681|A-17-OUT,B-12-TSL;n:type:ShaderForge.SFN_ValueProperty,id:17,x:33452,y:32712,ptlb:timescale,ptin:_timescale,glob:False,v1:20;proporder:2-17;pass:END;sub:END;*/

Shader "Shader Forge/cloth" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _timescale ("timescale", Float ) = 20
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _timescale;
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
////// Lighting:
////// Emissive:
                float4 node_12 = _Time + _TimeEditor;
                float2 node_13 = (i.uv0.rg+(_timescale*node_12.r)*float2(1,1));
                float4 node_2 = tex2D(_MainTex,TRANSFORM_TEX(node_13, _MainTex));
                float3 emissive = node_2.rgb;
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
