using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	struct Face 
	{
		public int v1, v2, v3;
		public int vn1, vn2, vn3;
	}

	public class WavefrontModel
	{
		List<Vector3d> vertices = new List<Vector3d>();
		List<Vector3d> normals = new List<Vector3d>();
		List<Face> faces = new List<Face>();

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
				string[] tokens = line.Split (' ');

				switch (tokens [0]) {
				case "v":
					vertices.Add (new Vector3d() {
						X = Convert.ToDouble (tokens [1]),
						Y = Convert.ToDouble (tokens [2]),
						Z = Convert.ToDouble (tokens [3])
					});
					break;
				case "vn":
					normals.Add (new Vector3d () {
						X = Convert.ToDouble (tokens [1]),
						Y = Convert.ToDouble (tokens [2]),
						Z = Convert.ToDouble (tokens [3])
					});
					break;
				case "f":
					string[] point0 = tokens [1].Split ('/');
					string[] point1 = tokens [2].Split ('/');
					string[] point2 = tokens [3].Split ('/');

					faces.Add (new Face () {
						v1 = Convert.ToInt32(point0[0]),
						vn1 = Convert.ToInt32(point0[2]),

						v2 = Convert.ToInt32(point1[0]),
						vn2 = Convert.ToInt32(point1[2]),

						v3 = Convert.ToInt32(point2[0]),
						vn3 = Convert.ToInt32(point2[2])
					});

					break;
				default:
					Console.WriteLine ("Parse info: " + line);
					break;
				}
			}

			Console.WriteLine ("Number of vertices: " + vertices.Count);
			Console.WriteLine ("Number of normals: " + normals.Count);
			Console.WriteLine ("Number of faces: " + faces.Count);

			return 0;
		}

		public void draw() 
		{
			GL.Begin (PrimitiveType.Triangles);

			GL.Color3(1.0f, 0.0f, 0.0f); 
			GL.Vertex3(-1.0f, -1.0f, 4.0f);

			GL.Color3(0.0f, 1.0f, 0.0f);
			GL.Vertex3(1.0f, -1.0f, 4.0f);

			GL.Color3(0.0f, 0.0f, 1.0f);
			GL.Vertex3(0.0f, 1.0f, 4.0f);

			GL.End();
		}
	}
}

