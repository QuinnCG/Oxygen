using System.Reflection;
using System.Reflection.Emit;

namespace Oxygen;

internal class Program
{
	private static void Main()
	{
		//CreateAndSave(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/MyAssembly.exe");
		//Console.ReadKey(true);
	}

	public static void CreateAndSave(string assemblyPath)
	{
		var assembly = AssemblyBuilder.DefinePersistedAssembly(new AssemblyName("MyAssembly"), typeof(object).Assembly);
		var module = assembly.DefineDynamicModule("MyModule");

		var type = module.DefineType("Program", TypeAttributes.Public | TypeAttributes.Class);
		var method = type.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(int), [typeof(int), typeof(int)]);

		var il = method.GetILGenerator();
		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldarg_1);
		il.Emit(OpCodes.Add);
		//il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", [typeof(int)]));
		il.Emit(OpCodes.Ret);

		type.CreateType();

		assembly.Save(assemblyPath);
	}


}
