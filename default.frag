#version 330

// in vec3 frag_colour;

in vec3 LightDirection_cameraspace;
in vec3 Normal_cameraspace;

out vec3 out_colour;

// Normal of the computed fragment, in camera space
vec3 n = normalize( Normal_cameraspace );

// Direction of the light (from the fragment to the light)
vec3 l = normalize( LightDirection_cameraspace );

float cosTheta = clamp( dot( n,l ), 0,1 );

const vec3 material_diffuse = vec3(1.0, 0.0, 0.0);
const vec3 light_colour = vec3(0.8, 0.8, 1.0);

void main()
{
	out_colour = material_diffuse * light_colour * cosTheta;
}