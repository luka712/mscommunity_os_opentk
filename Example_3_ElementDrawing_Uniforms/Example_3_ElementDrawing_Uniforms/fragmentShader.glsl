#version 400

in vec3 colors;

out vec4 final_colors;

void main()
{
	final_colors = vec4(colors, 1.0);
}