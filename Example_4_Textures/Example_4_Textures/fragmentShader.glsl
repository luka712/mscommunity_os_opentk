#version 400

in vec2 tex_coords;

uniform sampler2D u_texSampler;

out vec4 final_colors;

void main()
{
	final_colors = texture(u_texSampler, tex_coords);
}