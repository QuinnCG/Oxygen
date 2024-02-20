using OpenTK.Graphics.OpenGL4;

namespace Oxygen;

internal class Shader
{
    public int Handle { get; }

    public Shader(string vertexSource, string fragmentSource)
    {
		int vs = CreateShader(ShaderType.VertexShader, vertexSource);
		int fs = CreateShader(ShaderType.FragmentShader, fragmentSource);

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vs);
        GL.AttachShader(Handle, fs);
        GL.LinkProgram(Handle);
        GL.ValidateProgram(Handle);

		string info = GL.GetProgramInfoLog(Handle);
        if (!string.IsNullOrWhiteSpace(info))
        {
            Console.WriteLine(info);
        }

        GL.DeleteShader(vs);
        GL.DeleteShader(fs);
    }

    ~Shader()
    {
        GL.DeleteShader(Handle);
    }

    private static int CreateShader(ShaderType type, string source)
    {
		int handle = GL.CreateShader(type);
        GL.ShaderSource(handle, source);
        GL.CompileShader(handle);

		string info = GL.GetShaderInfoLog(handle);
        if (!string.IsNullOrEmpty(info))
        {
            Console.WriteLine(info);
        }

        return handle;
    }

    public void Bind()
    {
        GL.UseProgram(Handle);
    }
}
