#version 330

uniform mat4 mat_projection;
uniform mat4 mat_view;
uniform mat4 mat_world;
uniform mat4 mat_normalTransform

in vec4 in_position;
in vec3 in_normal;

out vec3 normal;

void main()
{
	normal = (vec4(in_normal, 1.0f) * mat_normalTransform).xyz;
	vec4 wPos = vec4(in_position, 1.0f) * mat_world;
	vec4 vPos = wPos * mat_view;
	vec4 gl_Position = vPos * mat_projection;
}