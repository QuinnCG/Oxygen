using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Oxygen;

internal static class Renderer
{
	private readonly static VertexArray _vao;
	private readonly static Shader _shader;
	private readonly static Texture _texture;

	static Renderer()
	{
		_vao = new VertexArray(
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

		string source = Resource.LoadText("Default.glsl");
		int splitIndex = source.IndexOf("// Fragment");

		var vs = source[..splitIndex];
		var fs = source[splitIndex..];

		_shader = new Shader(vs, fs);
		_texture = new Texture(Resource.LoadBytes("Logo.png"));
	}

	public static void Submit(RenderObject renderObject)
	{

	}

	public static void Draw()
	{
		GL.ClearColor(0f, 0f, 0f, 1f);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		_vao.Bind();
		_shader.Bind();
		_texture.Bind();

		var mvp = Matrix4.Identity;

		mvp *= Matrix4.CreateRotationZ(MathF.PI / 180f * 60f);
		mvp *= Matrix4.CreateScale(1.5f);
		mvp *= Matrix4.CreateTranslation(1f, 1f, 0f);

		float orthoScale = 2f;
		var (width, height) = Application.WindowSize;
		mvp *= Matrix4.CreateOrthographic((float)width / height * orthoScale, orthoScale, 0f, 1f);

		_shader.SetUniform("u_mvp", mvp);

		GL.DrawElements(PrimitiveType.Triangles, _vao.IndexCount, DrawElementsType.UnsignedInt, 0);
	}
}
