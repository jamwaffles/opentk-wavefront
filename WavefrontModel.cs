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
		List<int> faces = new List<int> ();

		List<Vertex> vertices = new List<Vertex> ();

		Vbo vbo;

		public WavefrontModel (string filename)
		{
			load (filename);
		}

		// Load and parse the .OBJ file
		private int load(string filename) 
		{
			int numVertices = 0;
			int numNormals = 0;
			int numFaces = 0;

			List<Vector3> verts = new List<Vector3>();
			List<Vector3> norms = new List<Vector3>();

			Console.Write ("Loading model '" + filename + "'... ");

			// Load entire file into an array (maybe bad for memory?)
			string[] lines = System.IO.File.ReadAllLines(filename);

			foreach (string line in lines)
			{
				string[] tokens = line.Split (' ');

				switch (tokens [0]) {
				case "v":
					verts.Add (new Vector3 (Convert.ToSingle (tokens [1]), Convert.ToSingle (tokens [2]), Convert.ToSingle (tokens [3])));
					numVertices++;
					break;
				case "vn":
					norms.Add (new Vector3 (Convert.ToSingle (tokens [1]), Convert.ToSingle (tokens [2]), Convert.ToSingle (tokens [3])));
					numNormals++;
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

					vertices.Add (new Vertex () { norm = norms [face.vn1], pos = verts [face.v1] });
					vertices.Add (new Vertex () { norm = norms [face.vn2], pos = verts [face.v2] });
					vertices.Add (new Vertex () { norm = norms [face.vn3], pos = verts [face.v3] });

					faces.Add (face.v1);
					faces.Add (face.v2);
					faces.Add (face.v3);

					numFaces++;

					break;
				}
			}

			loadVbo ();

			Console.Write ("done\n");

			Console.WriteLine (numVertices);
			Console.WriteLine (numNormals);
			Console.WriteLine (numFaces);

			return 0;
		}

		private void loadVbo() {
			vbo = new Vbo ();

			vbo.loadInterleaved (ref vertices);
			vbo.loadIndexData (ref faces);
		}

		public void draw() 
		{
			vbo.draw ();
		}
	}
}

