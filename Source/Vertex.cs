using OpenTK.Mathematics;

namespace Oxygen;

internal struct Vertex
{
    public const int FloatCount = 4;
    public const int Size = sizeof(float) * FloatCount;

	public Vector2 Position;
	public Vector2 UV;

    public readonly override string ToString() => $"<{Position}, {UV}>";

	public static float[] GetVertexData(Vertex[] vertices)
	{
		float[] data = new float[Vertex.FloatCount * vertices.Length];

		for (int i = 0; i < vertices.Length; i++)
		{
			var vertex = vertices[i];

			data[(i * FloatCount) + 0] = vertex.Position.X;
			data[(i * FloatCount) + 1] = vertex.Position.Y;

			data[(i * FloatCount) + 2] = vertex.UV.X;
			data[(i * FloatCount) + 3] = vertex.UV.Y;
		}

		return data;
	}
}
