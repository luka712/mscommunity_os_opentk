#version 400

in vec4 color;

out vec4 out_color;

void main()
{
	out_color = vec4(color.xyz, 0.2);
}