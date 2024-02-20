using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Oxygen;

public unsafe class Application
{
	private Window* _window;

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

		string source = File.ReadAllText("Resources/Default.glsl");

		var shader = new Shader("""
			#version 330 core

			layout (location = 0) in vec2 a_position;
			layout (location = 1) in vec2 a_uv;

			out vec2 v_uv;

			void main()
			{
				v_uv = a_uv;
				gl_Position = vec4(a_position, 0.0, 1.0);
			}
			""", """
			#version 330 core
			
			in vec2 v_uv;
			out vec4 f_color;

			void main()
			{
				f_color = vec4(1.0, 0.0, 0.0, 1.0);
			}
			""");

		while (!GLFW.WindowShouldClose(_window))
		{
			GL.ClearColor(0f, 0f, 0f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			vao.Bind();
			shader.Bind();

			GL.DrawElements(PrimitiveType.Triangles, vao.IndexCount, DrawElementsType.UnsignedInt, 0);

			GLFW.SwapBuffers(_window);
			GLFW.PollEvents();
		}

		GLFW.Terminate();
	}
}
