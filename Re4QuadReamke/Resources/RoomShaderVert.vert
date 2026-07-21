#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;
layout(location = 3) in vec4 aColor;

out vec2 texCoord;
out vec4 color;

out vec3 Normal_cameraspace;
out vec3 LightDirection_cameraspace;
flat out int NormalIsZero;

uniform mat4 view;
uniform mat4 projection;
uniform vec3 CameraPosition;

void main(void)
{
    texCoord = aTexCoord;
    color = aColor;

    // aPosition is already in world space (this loader transforms vertices on the CPU, unlike
    // NewAge's RoomShaderVert.vert which transforms them here via mRotation/mPosition/mScale
    // uniforms). So there's no model matrix step here - aPosition IS the world position already.
    vec4 worldPos = vec4(aPosition, 1.0);
    gl_Position = worldPos * view * projection;

    NormalIsZero = 0;
    if (aNormal.x == 0.0 && aNormal.y == 0.0 && aNormal.z == 0.0)
    {
        NormalIsZero = 1;
    }

    // aNormal is already rotated into world space AND normalized on the CPU (see
    // RoomSmdLoader.cs BuildBaseMeshes / normalization step), so it only needs to be carried
    // into camera space here, matching NewAge's Normal_cameraspace exactly (NewAge computes it
    // as `view * vec4(vertex_normal, 1)` after its own vertex-shader rotation step - same math,
    // just with the rotation+normalization already applied before upload instead of here).
    Normal_cameraspace = (view * vec4(aNormal, 1.0)).xyz;

    // Vector from the vertex to the camera (light source), in camera space - matches NewAge's
    // LightDirection_cameraspace computation (light position == camera position).
    vec3 vertexPosition_cameraspace = (view * worldPos).xyz;
    vec3 EyeDirection_cameraspace = vec3(0.0, 0.0, 0.0) - vertexPosition_cameraspace;
    vec3 LightPosition_cameraspace = (view * vec4(CameraPosition, 1.0)).xyz;
    LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;
}
