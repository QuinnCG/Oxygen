// Vertex
#version 330 core

layout (location = 0) in vec2 a_position;
layout (location = 1) in vec2 a_uv;

out vec2 v_uv;

uniform mat4 u_mvp;

void main()
{
	gl_Position =u_mvp * vec4(a_position, 0.0, 1.0);
	v_uv = a_uv;
}

// Fragment
#version 330 core

in vec2 v_uv;
out vec4 f_color;

uniform sampler2D u_texture;

void main()
{
	f_color = texture(u_texture, v_uv);
}
