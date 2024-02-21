using OpenTK.Graphics.OpenGL4;
using StbiSharp;

namespace Oxygen;

internal class Texture
{
	public int Handle { get; }

	public Texture(byte[] data)
	{
		var span = new ReadOnlySpan<byte>(data);

		Stbi.SetFlipVerticallyOnLoad(true);
		StbiImage image = Stbi.LoadFromMemory(span, 4);

		Handle = GL.GenTexture();
		GL.BindTexture(TextureTarget.Texture2D, Handle);

		GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		GL.TextureParameter(Handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
		GL.TextureParameter(Handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

		GL.TexImage2D(
			TextureTarget.Texture2D, 
			0,
			PixelInternalFormat.Rgba, 
			image.Width, 
			image.Height, 
			0, 
			PixelFormat.Rgba,
			PixelType.UnsignedByte, 
			image.Data.ToArray());

		GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
	}

	~Texture()
	{
		GL.DeleteTexture(Handle);
	}

	public void Bind()
	{
		GL.BindTexture(TextureTarget.Texture2D, Handle);
	}
}
