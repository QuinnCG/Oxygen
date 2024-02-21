using OpenTK.Mathematics;

namespace Oxygen;

internal class Shape(Vertex[] vertices, uint[] indices)
{
	public static Shape Quad { get; } = new(
	[
		new Vertex() { Position = new Vector2(-0.5f, -0.5f), UV = new Vector2(0f, 0f) },
		new Vertex() { Position = new Vector2(-0.5f,  0.5f), UV = new Vector2(0f, 1f) },
		new Vertex() { Position = new Vector2( 0.5f,  0.5f), UV = new Vector2(1f, 1f) },
		new Vertex() { Position = new Vector2( 0.5f, -0.5f), UV = new Vector2(1f, 0f) }
	],
	[
		0, 1, 2,
		3, 0, 2
	]);

	public Vertex[] Vertices = vertices;
	public uint[] Indices = indices;
}
