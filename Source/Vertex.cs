using OpenTK.Mathematics;

namespace Oxygen;

internal struct Vertex
{
    public const int FloatCount = 4;
    public const int Size = sizeof(float) * FloatCount;

	public Vector2 Position;
	public Vector2 UV;

    public readonly override string ToString() => $"<{Position}, {UV}>";
}
