#version 400

layout(location = 0) in vec2 a_position;
layout(location = 1) in vec3 a_colors;

out vec3 colors;

void main()
{
	gl_Position = vec4(a_position, -1,1);
	colors = a_colors;
}