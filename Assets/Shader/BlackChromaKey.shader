Shader "Unlit/BlackChromaKey"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("DetectionColor", Color) = (0, 0, 0, 1)
		_Threshold ("Threshold", float) = 0.015
        _Contrast("Contrast", Range(0, 20))=9.99
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

		LOD 200

		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 _Color;
			float _Threshold;
            float _Contrast;

			fixed4 frag (v2f i) : SV_Target
			{

				//RGBの保存
				fixed4 camCol = tex2D(_MainTex, i.uv);

				//グレースケール
				fixed4 c = tex2D(_MainTex, i.uv);
                float gray = c.r * 0.3 + c.g * 0.6 + c.b * 0.1;
                c.rgb = fixed3(gray, gray, gray);

                //コントラスト調整
                c = 1/(1+exp(-_Contrast*(c-0.5)));

				float dist = c.r;

				if (dist <= _Threshold && i.uv.x >= 0.4 && i.uv.x <=0.58 && i.uv.y >= 0.41 && i.uv.y <=0.61) {
					c = fixed4(1.0,1.0,1.0,0.0);
				}else{
					c = camCol;
				}

				return c;

			}
			ENDCG
		}
	}
}
