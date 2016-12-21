#version 400

in vec2 texCoords;

uniform sampler2D textureSampler;

out vec4 out_color;

void main()
{
	out_color = texture2D(textureSampler, texCoords);

	if(out_color.a <= 0.2){
		out_color = vec4(1,1,1,1);
	}
}