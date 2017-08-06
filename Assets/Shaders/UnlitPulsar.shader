// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-7241-RGB,custl-2302-OUT,alpha-3502-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32003,y:32682,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Time,id:2359,x:31707,y:32943,varname:node_2359,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4424,x:32221,y:32854,varname:node_4424,prsc:2|A-7241-RGB,B-8175-OUT;n:type:ShaderForge.SFN_Sin,id:8175,x:32077,y:33004,varname:node_8175,prsc:2|IN-1530-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:2302,x:32528,y:32961,varname:node_2302,prsc:2|IN-4424-OUT,IMIN-7667-OUT,IMAX-624-OUT,OMIN-4568-OUT,OMAX-6598-OUT;n:type:ShaderForge.SFN_Vector1,id:7667,x:32337,y:32929,varname:node_7667,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:624,x:32337,y:32982,varname:node_624,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:6598,x:32329,y:33354,ptovrint:False,ptlb:MaxGlow,ptin:_MaxGlow,varname:node_6598,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.5,cur:1,max:1.5;n:type:ShaderForge.SFN_Slider,id:4568,x:32329,y:33242,ptovrint:False,ptlb:MinGlow,ptin:_MinGlow,varname:_MaxGlow_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.5;n:type:ShaderForge.SFN_Slider,id:3086,x:31615,y:33197,ptovrint:False,ptlb:Pulse Speed,ptin:_PulseSpeed,varname:node_3086,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:20;n:type:ShaderForge.SFN_Multiply,id:1530,x:31907,y:33023,varname:node_1530,prsc:2|A-2359-T,B-3086-OUT;n:type:ShaderForge.SFN_ValueProperty,id:133,x:31964,y:33242,ptovrint:False,ptlb:node_133,ptin:_node_133,varname:node_133,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Slider,id:3502,x:32225,y:32610,ptovrint:False,ptlb:Transpearancy,ptin:_Transpearancy,varname:node_3502,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:7241-4568-6598-3086-3502;pass:END;sub:END;*/

Shader "Shader Forge/UnlitPulsar" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _MinGlow ("MinGlow", Range(0, 0.5)) = 0
        _MaxGlow ("MaxGlow", Range(0.5, 1.5)) = 1
        _PulseSpeed ("Pulse Speed", Range(0, 20)) = 1
        _Transpearancy ("Transpearancy", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _MaxGlow;
            uniform float _MinGlow;
            uniform float _PulseSpeed;
            uniform float _Transpearancy;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float3 emissive = _Color.rgb;
                float4 node_2359 = _Time + _TimeEditor;
                float node_7667 = (-1.0);
                float3 finalColor = emissive + (_MinGlow + ( ((_Color.rgb*sin((node_2359.g*_PulseSpeed))) - node_7667) * (_MaxGlow - _MinGlow) ) / (1.0 - node_7667));
                return fixed4(finalColor,_Transpearancy);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
