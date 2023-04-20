Shader "Custom/Silhouette"
{
   Properties {
     _Color ("Color Tint", Color) = (1.0,1.0,1.0,1.0)
     _MainTex ("Diffuse Texture", 2D) = "white" {}
     _SpecColor ("Specular Color", Color) = (1.0,1.0,1.0,1.0)
     _Shininess ("Shininess", Float) = 10

     _SilhouetteColor ("SilhouetteColor", Color) = (0, 0, 0, 1.0)
   }
   SubShader {
     Pass {
       Tags {"LightMode" = "ForwardBase"}

       ZWrite On

       Stencil
       {
            Ref 2
            Pass Replace
       }

       CGPROGRAM
       #pragma vertex vert
       #pragma fragment frag

       uniform sampler2D _MainTex;
       uniform float4 _MainTex_ST;
       uniform float4 _Color;
       uniform float4 _SpecColor;
       uniform float _Shininess;
       uniform float4 _LightColor0;

       struct vertexInput{
         float4 vertex : POSITION;
         float3 normal : NORMAL;
         float4 texcoord : TEXCOORD0;
       };

       struct vertexOutput{
         float4 pos : SV_POSITION;
         float4 tex : TEXCOORD0;
         float4 posWorld : TEXCOORD1;
         float3 normalDir : TEXCOORD2;
       };

       vertexOutput vert(vertexInput v){
         vertexOutput o;
     
         o.posWorld = mul(unity_ObjectToWorld, v.vertex);
         o.normalDir = normalize( mul( float4( v.normal, 0.0 ), unity_WorldToObject ).xyz );
         o.pos = UnityObjectToClipPos(v.vertex);
         o.tex = v.texcoord;
     
         return o;
       }

       float4 frag(vertexOutput i) : SV_Target
       {
         float3 normalDirection = i.normalDir;
         float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
         float3 lightDirection;
         
         lightDirection = normalize(_WorldSpaceLightPos0.xyz);
     
         //Lighting
         float3 diffuseReflection = _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
         float3 specularReflection = diffuseReflection * _SpecColor.xyz * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)) , _Shininess);
           
         float3 lightFinal = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseReflection + specularReflection;
     
         //Texture Maps
         float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
     
         return float4(tex.xyz * lightFinal * _Color.xyz, 1.0);
       }
   
       ENDCG
       }
        
        Tags {"Queue"="Transparent"}
        Pass
        {
            Cull Front
            ZWrite Off
            ZTest Greater
            
            Stencil
            {
                Ref 2
                Comp NotEqual
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _SilhouetteColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _SilhouetteColor;
            }


            ENDCG
        }
    }
        FallBack "Diffuse"
}
