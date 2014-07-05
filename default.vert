#version 330
 
layout (location = 0) in vec3 in_position;
layout (location = 1) in vec3 in_colour;

uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;

out vec3 frag_colour;
 
void main()
{
 	frag_colour = in_colour;

	gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
}