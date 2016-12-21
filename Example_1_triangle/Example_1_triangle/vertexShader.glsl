#version 400

in vec2 a_vertices;

void main()
{
	gl_Position = vec4(a_vertices, -1.0, 1.0);
}