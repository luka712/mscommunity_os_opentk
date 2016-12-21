#version 400

in vec2 tex_coords;

uniform sampler2D sampler;

out vec4 final_colors;

void main()
{
	final_colors = texture2D(sampler, tex_coords * 2.0);
	if(final_colors.r == 0 && final_colors.g == 0 && final_colors.b == 0){
		final_colors = vec4(0.1,0.1,0.5,0.5);
	}
}