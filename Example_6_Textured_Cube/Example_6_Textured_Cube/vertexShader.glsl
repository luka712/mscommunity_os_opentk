#version 400

layout(location = 0)in vec3 a_vertices;
layout(location = 1)in vec2 a_texCoords;

uniform mat4 u_transformationMatrix;

uniform mat4 u_projectionMatrix;

uniform mat4 u_viewMatrix;

out vec2 tex_coords;

void main()
{
	gl_Position = u_projectionMatrix * u_viewMatrix * u_transformationMatrix * vec4(a_vertices, 1);
	tex_coords = a_texCoords;
}
