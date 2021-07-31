Shader "Custom/ObjectOutline" {

    Properties{  

        _Diffuse("Diffuse", Color) = (1,1,1,1)  

        _OutlineCol("OutlineCol", Color) = (1,0,0,1)  

        _OutlineFactor("OutlineFactor", Range(0,1)) = 0.1  

        _MainTex("Base 2D", 2D) = "white"{}  

    }  

      //����ɫ��    

    SubShader  

    {  

                  //���ʹ������Pass����һ��pass�ط��߼���һ�㣬ֻ�����ߵ���ɫ  

        Pass  

        {  

            //�޳����棬ֻ��Ⱦ���棬���ڴ����ģ�����ã����������Ҫ����ģ�����������  

            Cull Front  

                          CGPROGRAM  

            //ʹ��vert������frag����  

            #pragma vertex vert  

            #pragma fragment frag  

            #include "UnityCG.cginc"  

            fixed4 _OutlineCol;  

            float _OutlineFactor;  

                          struct v2f  

            {  

                float4 pos : SV_POSITION;  

            };  

                          v2f vert(appdata_full v)  

            {  

                v2f o;  

                //��vertex�׶Σ�ÿ�����㰴�շ��ߵķ���ƫ��һ���֣��������ֻ���ɽ���ԶС��͸������  

                //v.vertex.xyz += v.normal * _OutlineFactor;  

                o.pos = UnityObjectToClipPos(v.vertex); 

                //�����߷���ת�����ӿռ�  

                float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  

                //���ӿռ䷨��xy����ת����ͶӰ�ռ䣬ֻ��xy��Ҫ��z��Ȳ���Ҫ��  

                float2 offset = TransformViewToProjection(vnormal.xy);  

                //������ͶӰ�׶��������ƫ�Ʋ���  

                o.pos.xy += offset * _OutlineFactor;  

                return o;  

            }                

            fixed4 frag(v2f i) : SV_Target  

            {  

                //���Passֱ����������ɫ  

                return _OutlineCol;  

            }  

                          ENDCG  

        }  
    }  

    //ǰ���ShaderʧЧ�Ļ���ʹ��Ĭ�ϵ�Diffuse  

    FallBack "Diffuse"  

}