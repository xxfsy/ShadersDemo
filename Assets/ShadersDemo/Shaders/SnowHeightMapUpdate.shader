Shader "Custom Render Texture Effect/Snow Height Map Update"
{
    Properties
    { 
        _DrawPosition ("Draw Position", Vector) = (-1,-1,0,0)
        _GeneralBrushRadius ("General Brush Radius", float) = 0.055
        _BlackBrushRadius ("Black Brush Radius", float) = 0.02
    }

    SubShader
    {
        Lighting Off
        Blend One Zero
        
        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
            
            float4 _DrawPosition;
            float _GeneralBrushRadius;
            float _BlackBrushRadius;
            
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
               float4 previousTexture = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
               float4 currentTexture = smoothstep(_BlackBrushRadius, _GeneralBrushRadius, distance(IN.localTexcoord.xy, _DrawPosition));

                
                return min(previousTexture, currentTexture);
            }
            ENDCG
        }
    }
}