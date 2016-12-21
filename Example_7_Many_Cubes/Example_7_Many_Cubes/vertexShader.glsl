#version 400

layout(location = 0)in vec3 a_vertices;
layout(location = 1)in vec3 a_colors;

uniform mat4 u_projectionMatrix;
uniform mat4 u_viewMatrix;
uniform mat4 u_transformationMatrix;

out vec3 colors;

void main()
{
	gl_Position = u_projectionMatrix * u_viewMatrix * u_transformationMatrix * vec4(a_vertices, 1);
	colors = a_colors;
}
