using System;

namespace ObjLoader
{
	class DemoLoader
	{
		public static void Main (string[] args)
		{
			WavefrontModel model = new WavefrontModel ("./sphere-lowpoly.obj");

			Console.ReadKey ();
		}
	}
}
