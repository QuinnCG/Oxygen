using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Oxygen;

public unsafe class Application
{
	public static (int width, int height) WindowSize
	{
		get
		{
			GLFW.GetWindowSize(_window, out int width, out int height);
			return (width, height);
		}
	}

	public static Window* _window;

	public void Run()
	{
		GLFW.Init();

		GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
		GLFW.WindowHint(WindowHintBool.Resizable, false);

		_window = GLFW.CreateWindow(800, 600, "Oxygen", null, null);
		GLFW.MakeContextCurrent(_window);

		GL.LoadBindings(new GLFWBindingsContext());

		int lastID = -1;
		GL.Enable(EnableCap.DebugOutput);
		GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
		{
			if (id != lastID && (severity is not DebugSeverity.DebugSeverityNotification or DebugSeverity.DebugSeverityLow or DebugSeverity.DontCare))
			{
				lastID = id;
				string msg = Encoding.Default.GetString((byte*)message, length);
				Console.WriteLine(msg);
			}
		}, 0);

		GL.FrontFace(FrontFaceDirection.Cw);

		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		var vao = new VertexArray(
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

		var shader = new Shader(vs, fs);
		var texture = new Texture(Resource.LoadBytes("Logo.png"));

		while (!GLFW.WindowShouldClose(_window))
		{
			Renderer.Draw();

			GLFW.SwapBuffers(_window);
			GLFW.PollEvents();
		}

		GLFW.Terminate();
	}
}
