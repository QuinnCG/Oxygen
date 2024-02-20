using OpenTK.Graphics.OpenGL4;

namespace Oxygen;

internal class VertexArray
{
	public int Handle { get; }
	public int IndexCount { get; }

	private readonly int _vbo, _ibo;

	public VertexArray(Vertex[] vertices, uint[] indices)
	{
		IndexCount = indices.Length;

		Handle = GL.GenVertexArray();
		GL.BindVertexArray(Handle);

		_vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Size * vertices.Length, GetVertexData(vertices), BufferUsageHint.StaticDraw);

		_ibo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.Size, 0);

		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 2);
	}

	~VertexArray()
	{
		GL.DeleteVertexArray(Handle);
		GL.DeleteBuffer(_vbo);
		GL.DeleteBuffer(_ibo);
	}

	private static float[] GetVertexData(Vertex[] vertices)
	{
		float[] data = new float[Vertex.FloatCount * vertices.Length];

		for (int i = 0; i < vertices.Length; i++)
		{
			Vertex vertex = vertices[i];

			data[i + 0] = vertex.Position.X;
			data[i + 1] = vertex.Position.Y;

			data[i + 2] = vertex.UV.X;
			data[i + 3] = vertex.UV.Y;
		}

		return data;
	}

	public void Bind()
	{
		GL.BindVertexArray(Handle);
	}
}
