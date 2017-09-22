Shader "KJSshader/Voxel" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _LightMap ("LightMap", 2D) = "gray" {}   
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
     
      struct Input {
          float2 uv_MainTex;
      float2 uv2_LightMap;        
      };
      sampler2D _MainTex;      
      sampler2D _LightMap;
     
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;      
      o.Albedo *= tex2D (_LightMap, IN.uv2_LightMap).rgb * 4;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }