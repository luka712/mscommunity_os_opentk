#version 400

layout(location = 0)in vec2 a_vertices;
layout(location = 1)in vec3 a_colors;

uniform mat4 u_transformationMatrix;

out vec3 colors;

void main()
{
	gl_Position = u_transformationMatrix * vec4(a_vertices, -1, 1);
	colors = a_colors;
}
