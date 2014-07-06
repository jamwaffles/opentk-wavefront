#version 330
 
layout (location = 0) in vec3 in_position;
layout (location = 1) in vec3 in_colour;
layout (location = 2) in vec3 in_normal;

uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;

out vec3 Normal_cameraspace;
out vec3 EyeDirection_cameraspace;
out vec3 LightDirection_cameraspace;
out vec3 Position_worldspace;

const vec3 LightPosition_worldspace = vec3(2.0, 1.0, 2.0);
 
void main()
{
 	// Output position of the vertex, in clip space : MVP * position
	gl_Position =  projection_matrix * modelview_matrix * vec4(in_position,1);
	 
	// Position of the vertex, in worldspace : modelview_matrix * position
	Position_worldspace = (modelview_matrix * vec4(in_position,1)).xyz;
	 
	// Vector that goes from the vertex to the camera, in camera space.
	// In camera space, the camera is at the origin (0,0,0).
	vec3 vertexPosition_cameraspace = ( projection_matrix * modelview_matrix * vec4(in_position,1)).xyz;
	EyeDirection_cameraspace = vec3(0,0,0) - vertexPosition_cameraspace;
	 
	// Vector that goes from the vertex to the light, in camera space. M is ommited because it's identity.
	vec3 LightPosition_cameraspace = ( projection_matrix * vec4(LightPosition_worldspace,1)).xyz;
	LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;
	 
	// Normal of the the vertex, in camera space
	Normal_cameraspace = ( projection_matrix * modelview_matrix * vec4(in_normal,0)).xyz; // Only correct if ModelMatrix does not scale the model ! Use its inverse transpose if not.
}