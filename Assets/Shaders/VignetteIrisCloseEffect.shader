Shader "Custom/UnifiedVignetteClosingEffect"
{
    Properties
    {
        // Default (soft) vignette properties:
        _VignetteCenter("Vignette Center", Vector) = (0.5, 0.5, 0, 0)
        _VignetteRadius("Vignette Radius", Range(0, 1)) = 0.5

        // Closing (iris) properties:
        _CloseCenter("Closing Center", Vector) = (0.5, 0.5, 0, 0)
        // Set Iris Radius above 1 so that by default the closing effect is off-screen.
        _IrisRadius("Iris Radius", Range(0, 2)) = 1.2

        // Interpolation factor between default and closing states:
        _CloseAmount("Close Amount", Range(0, 1)) = 0.0

        // Feather for the smooth edge:
        _Feather("Feather", Range(0.001, 0.2)) = 0.01
        
        _Color ("Overlay Color", COLOR) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        Pass
        {
            // Draw on top regardless of depth.
            ZTest Always Cull Off ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Default vignette parameters
            float4 _VignetteCenter;
            float _VignetteRadius;

            // Closing (iris) parameters
            float4 _CloseCenter;
            float _IrisRadius;

            // Interpolation between states
            float _CloseAmount;

            // Feather for the transition edge
            float _Feather;

            //Color of the overlay circle
            fixed4 _Color;

            fixed4 frag(v2f_img i) : SV_Target
            {
                // Get the current UV coordinates.
                float2 uv = i.uv;

                // Interpolate between the default vignette center and the closing center.
                float2 effectiveCenter = lerp(_VignetteCenter.xy, _CloseCenter.xy, _CloseAmount);

                // Similarly, interpolate between the default radius and the iris (closing) radius.
                float effectiveRadius = lerp(_VignetteRadius, _IrisRadius, _CloseAmount);

                // Compute the aspect ratio and adjust the x coordinate.
                float aspect = _ScreenParams.x / _ScreenParams.y;
                float2 diff = uv - effectiveCenter;
                diff.x *= aspect;

                // Compute the distance from the effective center.
                float d = length(diff);

                // Compute the alpha (blackness) using smoothstep.
                // Pixels with a distance less than (effectiveRadius - _Feather) will be clear.
                // Outside effectiveRadius, they become fully black.
                float alpha = smoothstep(effectiveRadius - _Feather, effectiveRadius, d);
                
                // Output pure black with the computed alpha.
                return fixed4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
}
