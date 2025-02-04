Shader "Custom/UnifiedVignetteClosingEffect_Color_Roundness"
{
    Properties
    {
        // Default (soft) vignette properties:
        _VignetteCenter("Vignette Center", Vector) = (0.5, 0.5, 0, 0)
        _VignetteRadius("Vignette Radius", Range(0, 1)) = 0.5

        // Closing (iris) properties:
        _CloseCenter("Closing Center", Vector) = (0.5, 0.5, 0, 0)
        // Set Iris Radius above 1 so that the closing effect is initially off-screen.
        _IrisRadius("Iris Radius", Range(0, 2)) = 1.2

        // Interpolation factor between default and closing states:
        _CloseAmount("Close Amount", Range(0, 1)) = 0.0

        // Feather for the smooth edge:
        _Feather("Feather", Range(0.001, 0.2)) = 0.01

        // Overlay color: change this to control the color of the vignette.
        _OverlayColor("Overlay Color", Color) = (0, 0, 0, 1)

        // New: Roundness factor:
        // 1 means fully round (aspect ratio is fully corrected)
        // 0 means no correction (effect computed in raw UV space, likely oval)
        _Roundness("Roundness", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        Pass
        {
            // Always draw on top regardless of depth.
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

            // Interpolation parameter
            float _CloseAmount;

            // Feather for the transition edge
            float _Feather;

            // Overlay color
            float4 _OverlayColor;

            // New roundness factor (0: oval, 1: round)
            float _Roundness;

            fixed4 frag(v2f_img i) : SV_Target
            {
                // Get the normalized UV coordinates.
                float2 uv = i.uv;

                // Interpolate between the default vignette center and the closing center.
                float2 effectiveCenter = lerp(_VignetteCenter.xy, _CloseCenter.xy, _CloseAmount);

                // Interpolate between the default radius and the iris (closing) radius.
                float effectiveRadius = lerp(_VignetteRadius, _IrisRadius, _CloseAmount);

                // Compute the screen aspect ratio.
                float aspect = _ScreenParams.x / _ScreenParams.y;
                // Compute the difference vector from the effective center.
                float2 diff = uv - effectiveCenter;

                // Blend the x-coordinate correction between "no correction" (1.0) and "full correction" (aspect).
                // When _Roundness == 0, roundFactor = 1 and no correction is applied (resulting in an oval shape).
                // When _Roundness == 1, roundFactor = aspect and full correction is applied (resulting in a circle).
                float roundFactor = lerp(1.0, aspect, _Roundness);
                diff.x *= roundFactor;

                // Compute the distance from the effective center.
                float d = length(diff);

                // Compute the alpha (mask strength) using smoothstep.
                // Pixels with a distance less than (effectiveRadius - _Feather) remain clear,
                // and outside effectiveRadius they become fully overlay-colored.
                float alpha = smoothstep(effectiveRadius - _Feather, effectiveRadius, d);

                // Output the overlay color with the computed alpha.
                return fixed4(_OverlayColor.rgb, alpha);
            }
            ENDCG
        }
    }
}
