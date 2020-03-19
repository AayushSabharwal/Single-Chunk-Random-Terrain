Shader "Custom/Terrain"
{
    Properties
    {
		
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		const static int maxColourCount = 8;	//max colours allowed
		const static float epsilon = 1E-4;
		int baseColourCount;	//actual number of colours used
		float3 baseColours[maxColourCount];	//the colours being used
		float baseStartHeights[maxColourCount];	//the start heights of each colour
		float baseBlends[maxColourCount];

		float minHeight;	//minimum height point in mesh
		float maxHeight;	//max height point in mesh

        struct Input
        {
            float3 worldPos;	//in surf we take input of the world position of each point
        };

		float inverseLerp(float a, float b, float val)	//function to return 1 if val is b and 0 if val is a. Linear interpolation between the two limits
		{
			return saturate((val - a) / (b - a));	//saturate clamps in range (0, 1)
		}
		
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
			for (int i = 0; i < baseColourCount; i++)
			{
				float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, heightPercent - baseStartHeights[i]);
																							//blend the colour/texture using inverse lerp
																							//we subtract epsilon to prevent issues when the baseBlends value
																							//is 0. This would cause dividing by 0 in inverseLerp
				o.Albedo = o.Albedo *(1-drawStrength) +	baseColours[i] * drawStrength;	//if our drawStrength became 0, it would reset the previously set
																						//colour to black. To prevent this, we use this expression. In the
																						//particular case when drawStrength is 0, it retains the colour
			}
        }
        ENDCG
    }
    FallBack "Diffuse"
}
