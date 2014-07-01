using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	public class Vbo
	{
		public int vertexBufferID;
		// public int colorBufferID;
		// public int texCoordBufferID;
		// public int normalBufferID;
		public int elementBufferID;

		int numElements;

		public void loadVertexData(ref List<Vector3> vertices)
		{
			int bufferSize;

			// Generate Array Buffer Id
			GL.GenBuffers(1, out vertexBufferID);

			// Bind current context to Array Buffer ID
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);

			// Send data to buffer
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * Vector3.SizeInBytes), vertices.ToArray(), BufferUsageHint.StaticDraw);

			// Validate that the buffer is the correct size
			GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);

			if (vertices.Count * Vector3.SizeInBytes != bufferSize)
				throw new ApplicationException("Vertex array not uploaded correctly");

			// Clear the buffer Binding
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void loadIndexData(ref List<int> indices)
		{
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
			// Vertex Array Buffer
			{
				// Bind to the Array Buffer ID
				GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);

				// Set the Pointer to the current bound array describing how the data ia stored
				GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);

				// Enable the client state so it will use this array buffer pointer
				GL.EnableClientState(ArrayCap.VertexArray);
			}

			// Element Array Buffer
			{
				// Bind to the Array Buffer ID
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferID);

				// Draw the elements in the element array buffer
				// Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
				GL.DrawElements(PrimitiveType.Triangles, numElements, DrawElementsType.UnsignedInt, IntPtr.Zero);

				// Could also call GL.DrawArrays which would ignore the ElementArrayBuffer and just use primitives
				// Of course we would have to reorder our data to be in the correct primitive order
			}
		}
	}
}

