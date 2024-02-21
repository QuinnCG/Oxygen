namespace Oxygen;

internal static class Resource
{
	public static string LoadText(string name)
	{
		return File.ReadAllText(GetPath(name));
	}
	public static string[] LoadLines(string name)
	{
		return File.ReadAllLines(GetPath(name));
	}
	public static byte[] LoadBytes(string name)
	{
		return File.ReadAllBytes(GetPath(name));
	}

	private static string GetPath(string name) => $"Resources/{name}";
}
