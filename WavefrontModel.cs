using System;
using System.Drawing;
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
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Face> faces = new List<Face>();

		List<int> faceIndices = new List<int> ();

		Vbo vbo;

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
					vertices.Add (new Vector3(Convert.ToSingle (tokens [1]), Convert.ToSingle (tokens [2]), Convert.ToSingle (tokens [3])));
					break;
				case "vn":
					normals.Add (new Vector3(Convert.ToSingle (tokens [1]), Convert.ToSingle (tokens [2]), Convert.ToSingle (tokens [3])));
					break;
				case "f":
					string[] point0 = tokens [1].Split ('/');
					string[] point1 = tokens [2].Split ('/');
					string[] point2 = tokens [3].Split ('/');

					Face face = new Face () {
						v1 = Convert.ToInt32 (point0 [0]) - 1,
						vn1 = Convert.ToInt32 (point0 [2]) - 1,

						v2 = Convert.ToInt32 (point1 [0]) - 1,
						vn2 = Convert.ToInt32 (point1 [2]) - 1,

						v3 = Convert.ToInt32 (point2 [0]) - 1,
						vn3 = Convert.ToInt32 (point2 [2]) - 1
					};

					faces.Add (face);

					faceIndices.Add (face.v1);
					faceIndices.Add (face.v2);
					faceIndices.Add (face.v3);

					break;
				default:
					Console.WriteLine ("Parse info: " + line);
					break;
				}
			}

			Console.WriteLine ("Number of vertices: " + vertices.Count);
			Console.WriteLine ("Number of normals: " + normals.Count);
			Console.WriteLine ("Number of faces: " + faces.Count);

			loadVbo ();

			return 0;
		}

		private void loadVbo() {
			vbo = new Vbo ();

			vbo.loadNormalData (ref normals);
			vbo.loadVertexData (ref vertices);
			vbo.loadIndexData (ref faceIndices);
		}

		public void draw() 
		{
			vbo.draw ();
		}
	}
}

