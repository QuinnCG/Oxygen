using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Oxygen;

internal static class Renderer
{
	private static readonly HashSet<RenderBatch> _batches = [];
	private static readonly Dictionary<RenderBatch, VertexArray> _vaos = [];
	private static readonly Dictionary<RenderBatch, float> _batchDeletionTimes = [];
	private static readonly VertexArray _vao;

	static Renderer()
	{
		var quad = Shape.Quad;
		_vao = new VertexArray(quad.Vertices, quad.Indices);
	}

	public static void Submit(RenderObject renderObject)
	{
		Submit(new RenderBatch(renderObject.Shape, renderObject.Shader, renderObject)
		{
			Texture = renderObject.Texture
		});
	}
	public static void Submit(RenderBatch renderBatch)
	{
		foreach (var batch in _batches)
		{
			if (batch.Shader == renderBatch.Shader && batch.Texture == renderBatch.Texture)
			{
				foreach (var renderObject in renderBatch.RenderObjects)
				{
					batch.RenderObjects.Add(renderObject);
				}

				// If batch is marked for deletion, unmark it.
				_batchDeletionTimes.Remove(batch);

				return;
			}
		}

		// TODO: How to decide when to discard existing batches? How can you tell if its an existing batch?

		_batches.Add(renderBatch);
		_vaos.Add(renderBatch, new VertexArray());
	}

	public static void Draw()
	{
		GL.ClearColor(0f, 0f, 0f, 1f);
		GL.Clear(ClearBufferMask.ColorBufferBit);

		foreach (var batch in _batches)
		{
			batch.Texture?.Bind();
			batch.Shader.Bind();
			_vao.Bind();

			var mvp = Matrix4.Identity;

			mvp *= Matrix4.CreateRotationZ(MathF.PI / 180f * -60f);
			mvp *= Matrix4.CreateScale(1f);
			mvp *= Matrix4.CreateTranslation(0f, 0f, 0f);

			float orthoScale = 2f;
			var (width, height) = Application.WindowSize;
			mvp *= Matrix4.CreateOrthographic((float)width / height * orthoScale, orthoScale, 0f, 1f);

			batch.Shader.SetUniform("u_mvp", mvp);
			GL.DrawElements(PrimitiveType.Triangles, _vao.IndexCount, DrawElementsType.UnsignedInt, 0);
		}

		// Mark unused batches for deletion and clear the rest.
		foreach (var batch in _batches)
		{
			if (batch.RenderObjects.Count == 0)
			{
				if (!_batchDeletionTimes.ContainsKey(batch))
				{
					const float batchDeletionDelay = 5f;
					_batchDeletionTimes.Add(batch, Application.Time + batchDeletionDelay);
				}

				continue;
			}

			batch.RenderObjects.Clear();
		}

		// Delete batches marked for deletion.
		foreach (var deletionEntry in _batchDeletionTimes)
		{
			if (Application.Time >= deletionEntry.Value)
			{
				_batches.Remove(deletionEntry.Key);
				_batchDeletionTimes.Remove(deletionEntry.Key);
				break;
			}
		}
	}
}
