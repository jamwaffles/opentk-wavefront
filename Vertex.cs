using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	public struct Vertex
	{
		public Vector3 pos, norm;

		public static readonly int Stride = Marshal.SizeOf(default(Vertex));
	}
}

