using OpenTK.Mathematics;

namespace Oxygen;

internal class RenderObject(Shape shape, Shader shader, Texture? texture = null)
{
	public Vector2 Position;
	public float Rotation;
	public Vector2 Scale = Vector2.One;

	public Shape Shape = shape;
	public Shader Shader = shader;
	public Texture? Texture = texture;
}
