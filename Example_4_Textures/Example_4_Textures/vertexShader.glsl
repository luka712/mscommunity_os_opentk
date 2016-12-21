#version 400

layout(location = 0)in vec2 a_vertices;
layout(location = 1)in vec2 a_texCoords;

uniform mat4 u_transformationMatrix;

out vec2 tex_coords;

void main()
{
	gl_Position = u_transformationMatrix * vec4(a_vertices, -1, 1);
	tex_coords = a_texCoords;
}
