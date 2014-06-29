using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace ObjLoader
{
	struct Face 
	{
		int v1, v2, v3;
		int vn1, vn2, vn3;
	}

	public class WavefrontModel
	{
		List<Vector3d> vertices;
		List<Vector3d> normals;
		List<Face> faces;

		public WavefrontModel (string filename)
		{
			load (filename);
		}

		// Load and parse the .OBJ file
		private int load(string filename) 
		{
			// Load entire file into an array (maybe bad for memory?)
			string[] lines = System.IO.File.ReadAllLines(filename);

			foreach (string line in lines)
			{
				Console.WriteLine(line);
			}

			return 0;
		}

		public void draw() 
		{

		}
	}
}

