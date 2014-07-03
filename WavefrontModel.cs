using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace ObjLoader
{
	public struct Vertex
	{
		public Vector3 pos, norm;

		public static readonly int Stride = Marshal.SizeOf(default(Vertex));
	}

	struct Face 
	{
		public int v1, v2, v3;
		public int vn1, vn2, vn3;
	}

	public class WavefrontModel
	{
		List<int> indices = new List<int> ();

		List<Vector3> vertices = new List<Vector3> ();
		List<Vector3> normals = new List<Vector3> ();

		List<Vertex> modelData = new List<Vertex> ();

		int iboId;
//		int normalBufferId;
		int vaoId;
		int vboId;

//		Vbo vbo;

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

			Console.Write ("Loading model '" + filename + "'... ");

			// Load entire file into an array (maybe bad for memory?)
			string[] lines = System.IO.File.ReadAllLines(filename);

			foreach (string line in lines)
			{
				string[] tokens = line.Split (' ');

				switch (tokens [0]) {
				case "v":
					vertices.Add (new Vector3 (Convert.ToSingle (tokens [1]), Convert.ToSingle (tokens [2]), Convert.ToSingle (tokens [3])));
					numVertices++;
					break;
				case "vn":
					normals.Add (new Vector3 (Convert.ToSingle (tokens [1]), Convert.ToSingle (tokens [2]), Convert.ToSingle (tokens [3])));
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

					modelData.Add (new Vertex () { norm = normals [face.vn1], pos = vertices [face.v1] });
					modelData.Add (new Vertex () { norm = normals [face.vn2], pos = vertices [face.v2] });
					modelData.Add (new Vertex () { norm = normals [face.vn3], pos = vertices [face.v3] });

					indices.Add (face.v1);
					indices.Add (face.v2);
					indices.Add (face.v3);

					numFaces += 3;

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
//			vbo = new Vbo ();
//
//			vbo.loadInterleaved (ref vertices);
//			vbo.loadIndexData (ref faces);

			// Vertex index data
			GL.GenBuffers (1, out iboId);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, iboId);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(int) * indices.Count), indices.ToArray (), BufferUsageHint.StaticDraw);

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, iboId);
			Console.WriteLine (GL.GetError ());

			// Vertex position data
			GL.GenBuffers (1, out vboId);
			GL.BindBuffer (BufferTarget.ArrayBuffer, vboId);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(Vertex.Stride * modelData.Count), modelData.ToArray(), BufferUsageHint.StaticDraw);

			GL.BindBuffer (BufferTarget.ArrayBuffer, vboId);
			Console.WriteLine (GL.GetError ());

			// VAO
			GL.GenVertexArrays (1, out vaoId);
			GL.BindVertexArray (vaoId);

			GL.BindBuffer (BufferTarget.ArrayBuffer, vboId);
			GL.VertexAttribPointer (0, 3, VertexAttribPointerType.Float, false, Vertex.Stride, IntPtr.Zero);
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.Stride, new IntPtr(Vector3.SizeInBytes));

			GL.EnableVertexAttribArray (0);
			GL.EnableVertexAttribArray (1);
			GL.DisableVertexAttribArray (2);
			GL.DisableVertexAttribArray (3);

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, iboId);

			GL.BindVertexArray (0);
			GL.EnableVertexAttribArray (0);
			GL.EnableVertexAttribArray (1);
			GL.EnableVertexAttribArray (2);
			GL.EnableVertexAttribArray (3);

			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		public void draw() 
		{
			// vbo.draw ();

			GL.BindVertexArray (vaoId);

			GL.DrawRangeElements (PrimitiveType.Triangles, 0, indices.Count, indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
		}
	}
}

