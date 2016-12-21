#version 400

in vec3 a_verts;
in vec2 a_texCoords;

uniform mat4 u_projectionMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_transformationMatrix;

out vec2 texCoords; 

void main()
{
	gl_Position = u_projectionMatrix * u_viewMatrix * u_transformationMatrix * vec4(a_verts, 1.0); 
	texCoords = a_texCoords;
}