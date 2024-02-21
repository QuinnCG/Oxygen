using OpenTK.Graphics.OpenGL4;

namespace Oxygen;

internal class VertexArray
{
	public int Handle { get; }
	public int IndexCount { get; private set; }

	private readonly int _vbo, _ibo;
	private int _vertexSize = 2, _indexSize = 2;

	public VertexArray()
	{
		Handle = GL.GenVertexArray();
		GL.BindVertexArray(Handle);

		_vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, 0, 0, BufferUsageHint.DynamicDraw);

		_ibo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, 0, 0, BufferUsageHint.DynamicDraw);

		GL.EnableVertexAttribArray(0);
		GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, Vertex.Size, 0);

		GL.EnableVertexAttribArray(1);
		GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 2);
	}
	public VertexArray(Vertex[] vertices, uint[] indices)
	{
		IndexCount = indices.Length;

		Handle = GL.GenVertexArray();
		GL.BindVertexArray(Handle);

		_vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, Vertex.Size * vertices.Length, Vertex.GetVertexData(vertices), BufferUsageHint.StaticDraw);

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

	public void Bind()
	{
		GL.BindVertexArray(Handle);
	}

	public void Update(float[] vertices, uint[] indices)
	{
		UpdateBufferSizes(sizeof(float) * vertices.Length, sizeof(uint) * indices.Length);
		Bind();

		GL.BufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * vertices.Length, vertices);
		GL.BufferSubData(BufferTarget.ElementArrayBuffer, 0, sizeof(uint) * indices.Length, indices);
		IndexCount = indices.Length;
	}

	private void UpdateBufferSizes(int vertexSize, int indexSize)
	{
		Bind();

		int newVertexSize = _vertexSize;
		int newIndexSize = _indexSize;

		if (vertexSize > _vertexSize)
		{
			while (newVertexSize < vertexSize)
			{
				newVertexSize *= 2;
			}
		}
		else if (vertexSize < (_vertexSize / 4))
		{
			while (newVertexSize > vertexSize)
			{
				newVertexSize /= 2;
			}
		}

		if (vertexSize > _indexSize)
		{
			while (newIndexSize < indexSize)
			{
				newIndexSize *= 2;
			}
		}
		else if (indexSize < (_indexSize / 4))
		{
			while (newIndexSize > indexSize)
			{
				newIndexSize /= 2;
			}
		}

		newVertexSize = Math.Max(2, vertexSize);
		newIndexSize = Math.Max(2, indexSize);

		_vertexSize = newVertexSize;
		_indexSize = newIndexSize;

		if (newVertexSize != _vertexSize)
		{
			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * newVertexSize, 0, BufferUsageHint.DynamicDraw);
		}

		if (newIndexSize != _indexSize)
		{
			GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * newIndexSize, 0, BufferUsageHint.DynamicDraw);
		}
	}
}
