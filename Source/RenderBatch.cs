namespace Oxygen;

internal class RenderBatch
{
	public Shape Shape;
	public Shader Shader;
	public Texture? Texture;

	public HashSet<RenderObject> RenderObjects = [];

	public RenderBatch(Shape shape, Shader shader, params RenderObject[] renderObjects)
	{
		Shape = shape;
		Shader = shader;

		foreach (var renderObject in renderObjects)
		{
			RenderObjects.Add(renderObject);
		}
	}

	public void GenerateBufferData(out float[] vertices, out uint[] indices)
	{
		var vertexList = new List<float>();
		var indexList = new List<uint>();

		uint objectIndex = 0;
		foreach (var renderObject in RenderObjects)
		{
			var shape = renderObject.Shape;
			vertexList.AddRange(Vertex.GetVertexData(shape.Vertices));

			var rawIndices = new uint[shape.Indices.Length];
			Array.Copy(shape.Indices, rawIndices, rawIndices.Length);

			for (int i = 0; i < rawIndices.Length; i++)
			{
				rawIndices[i] += objectIndex;
			}

			indexList.AddRange(rawIndices);

			objectIndex++;
		}

		vertices = [.. vertexList];
		indices = [.. indexList];
	}
}
