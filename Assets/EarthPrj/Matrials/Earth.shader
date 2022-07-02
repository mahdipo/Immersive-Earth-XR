Shader "Custom/MyEarth"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BorderMap("BorderMap (RGB)", 2D) = "white" {}
        _BorderMapIntensity("BorderMap Intensity", Range(0,3)) = 1.0

        _CloudMap("CloudMap (RGB)", 2D) = "white" {}
        _CloudMapIntensity("CloudMap Intensity", Range(0,3)) = 1.0

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower("Rim Power", Range(0.5, 64.0)) = 3.0
        _RimIntensity("Rim Intensity", Range(0.0, 100.0)) = 2.0
    }
    SubShader
    {
        
        Tags { "RenderType"="Opaque" } 
        LOD 200

        CGPROGRAM       
        #pragma surface surf Standard fullforwardshadows
              
        #pragma target 3.0
      
        sampler2D _MainTex;
        sampler2D _BorderMap;
        sampler2D _CloudMap;
        float4 _CloudMap_ST;

        half _BorderMapIntensity;
        half _CloudMapIntensity;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float4 _RimColor;
        float _RimPower;
        float _RimIntensity;
               
        UNITY_INSTANCING_BUFFER_START(Props)      
        UNITY_INSTANCING_BUFFER_END(Props)
       
        void surf (Input IN, inout SurfaceOutputStandard o)
        {           
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 borderMap = tex2D(_BorderMap, IN.uv_MainTex);
            fixed4 cloudMap = tex2D(_CloudMap, IN.uv_MainTex+ _CloudMap_ST);

            o.Albedo = c.rgb+(borderMap.rgb* _BorderMapIntensity)+ (cloudMap.rgb * _CloudMapIntensity);
          
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow(rim, _RimPower); 
            o.Albedo += (o.Emission.rgb * _RimIntensity );
        }
        ENDCG
    }
    FallBack "Diffuse"
}
