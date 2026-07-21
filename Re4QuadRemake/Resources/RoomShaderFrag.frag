#version 330

out vec4 outputColor;

in vec2 texCoord;
in vec4 color;

in vec3 Normal_cameraspace;
in vec3 LightDirection_cameraspace;
flat in int NormalIsZero;

uniform sampler2D texture0;
uniform sampler2D texture1;

uniform vec4 matColor;
//uniform vec4 smxColor;
uniform bool EnableNormals;
uniform bool EnableVertexColors;
uniform bool EnableAlphaChannel;

void main()
{
    vec3 vnormal = Normal_cameraspace;

    if (gl_FrontFacing)
    {
        vnormal = Normal_cameraspace * -1;
    }

    vec4 texColor = mix(texture(texture0, texCoord), vec4(0.0, 0.0, 0.0, 1.0), 0.05);
    vec4 texAlpha = texture(texture1, texCoord);

    // Default alpha comes from the diffuse texture's own alpha channel, exactly like the
    // original RoomShaderFrag.frag (texColor.a) - this is what makes Xcar/PMD tree/leaf cutout
    // textures (transparent background, alpha-tested edges) work correctly. EnableAlphaChannel
    // below is an ADDITIONAL override for SMD/UHD materials that have a real separate opacity_map
    // (texture1) - it must stay off for Xcar/PMD meshes, which never have one.
    float alphaValue = texColor.a;

    // Matches NewAge's RoomShaderFrag.frag exactly: matColor and the per-vertex color (RGB+A) are
    // ONLY multiplied in when EnableVertexColors is on - this is a separate toggle from
    // EnableAlphaChannel below, not always-on. Vertex color (baked into the BIN's
    // Vertex_Color_Array, byte order a,r,g,b) is a per-vertex tint/fade mechanism, distinct from
    // the opacity_map (texture1) used below. Always (1,1,1,1) for BINs with no vertex colors (see
    // RoomSmdLoader.cs) and for all Xcar/PMD meshes, so leaving this off has no visible effect on
    // rooms that don't actually carry vertex colors.
    if (EnableVertexColors)
    {
        texColor = texColor * matColor * color;
        texAlpha = texAlpha * matColor * color;
    }

    if (EnableAlphaChannel)
    {
        // matches NewAge's RoomShaderFrag.frag: alpha comes from the
        // material's opacity_map (texture1), not from the diffuse texture.
        alphaValue = texAlpha.a;
    }

    // ambient-only result: used as a fallback when a mesh has no real normal data (Xcar/PMD,
    // or any UHD vertex whose normal is (0,0,0)), and as the base color before lighting below.
    vec4 ambient_result = vec4(texColor.r, texColor.g, texColor.b, alphaValue);
    if (ambient_result.a < 0.1)
    {
        discard;
    }

    // Matches NewAge's RoomShaderFrag.frag lighting model exactly: a simple point light fixed at
    // the camera position (a "flashlight" look), ambient + diffuse, no specular. This is what
    // makes baked-shadow/dark-corner surfaces render as actual dark shading (their normal facing
    // away from the camera/light) instead of a flat, undifferentiated color.
    vec3 LightColor = vec3(1.0, 1.0, 1.0);
    float LightPower = 50.0;

    vec3 MaterialDiffuseColor = texColor.rgb;
    vec3 MaterialAmbientColor = vec3(0.1, 0.1, 0.1) * MaterialDiffuseColor;

    float lightDistance = 7.0;

    vec3 n = normalize(vnormal);
    vec3 l = normalize(LightDirection_cameraspace);
    float cosTheta = clamp(dot(n, l), 0.0, 1.0);

    outputColor = vec4(
        MaterialAmbientColor +
        MaterialDiffuseColor * LightColor * LightPower * cosTheta / (lightDistance * lightDistance)
        , alphaValue);

    if (!EnableNormals || NormalIsZero != 0)
    {
        outputColor = ambient_result;
    }
}
