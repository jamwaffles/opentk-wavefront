using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	public class Vbo
	{
		public int vertexBufferID;
		public int normalBufferID;

		public int interleavedBufferID;
		public int elementBufferID;

		int numElements;
		List<int> indices = new List<int> ();

		public void loadInterleaved (ref List<Vertex> interleaved)
		{
			int bufferSize;

			// Generate Array Buffer Id
			GL.GenBuffers(1, out interleavedBufferID);

			// Bind current context to Array Buffer ID
			GL.BindBuffer(BufferTarget.ArrayBuffer, interleavedBufferID);

			// Send data to buffer
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(interleaved.Count * Vertex.Stride), interleaved.ToArray(), BufferUsageHint.StaticDraw);

			// Validate that the buffer is the correct size
			GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);

			if (interleaved.Count * Vertex.Stride != bufferSize)
				throw new ApplicationException("Interleaved buffer data not uploaded properly");

			// Clear the buffer Binding
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void loadIndexData(ref List<int> data)
		{
			indices = data;

			int bufferSize;

			// Generate Array Buffer Id
			GL.GenBuffers(1, out elementBufferID);

			// Bind current context to Array Buffer ID
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferID);

			// Send data to buffer
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(int)), indices.ToArray(), BufferUsageHint.StaticDraw);

			// Validate that the buffer is the correct size
			GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
			if (indices.Count * sizeof(int) != bufferSize)
				throw new ApplicationException("Element array not uploaded correctly");

			// Clear the buffer Binding
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			numElements = indices.Count;
		}

		public void draw() {
			GL.EnableClientState(ArrayCap.NormalArray);
			GL.EnableClientState(ArrayCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, interleavedBufferID);
			GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(Vector3.SizeInBytes));
			GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, new IntPtr(0));
			GL.DrawArrays(PrimitiveType.Triangles, 0, numElements);
		}
	}
}

