using System;

namespace ObjLoader
{
	class DemoLoader
	{
		public static void Main (string[] args)
		{
			using (App app = new App())
			{
				app.Run(60.0);
			}
		}
	}
}
