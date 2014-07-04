using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace ObjLoader
{
	public struct TestVertex {
		public float x, y, z;
		public float nx, ny, nz;
	}

	public class App: GameWindow
	{
		float angle;

		List<TestVertex> triangle = new List<TestVertex> ();
		List<ushort> triangleIndex = new List<ushort> ();

		uint vaoId, iboId, vboId;
		int shaderProgramId, vertexShaderId, fragmentShaderId;
		int uniformProjectionModelView;

		public App(): base(
			800, 
			600, 
			new OpenTK.Graphics.GraphicsMode(32, 16, 0, 4), 
			"Obj Loader", 
			GameWindowFlags.Default, 
			DisplayDevice.Default, 
			3,		// Major version
			3,		// Minor version
			OpenTK.Graphics.GraphicsContextFlags.Default | OpenTK.Graphics.GraphicsContextFlags.Debug)
		{
			Keyboard.KeyDown += HandleKeyDown;

			TestVertex v1, v2, v3;

			v1.x=0.0f;
			v1.y=0.5f;
			v1.z=-1.0f;
			v1.nx=0.0f;
			v1.ny=0.0f;
			v1.nz=1.0f;

			v2.x=0.3f;
			v2.y=-0.5f;
			v2.z=-1.0f;
			v2.nx=0.0f;
			v2.ny=0.0f;
			v2.nz=1.0f;

			v3.x=0.8f;
			v3.y=0.5f;
			v3.z=-1.0f;
			v3.nx=0.0f;
			v3.ny=0.0f;
			v3.nz=1.0f;

			triangle.Add (v1);
			triangle.Add (v2);
			triangle.Add (v3);

			triangleIndex.Add (0);
			triangleIndex.Add (1);
			triangleIndex.Add (2);
		}

		void HandleKeyDown (object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.Escape) Exit ();
		}

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			GL.Enable (EnableCap.DepthTest);
			GL.Enable (EnableCap.CullFace);
			GL.CullFace (CullFaceMode.Back);

			GL.ClearColor (Color.CornflowerBlue);

			GL.GenBuffers (1, out iboId);
			GL.BindBuffer (BufferTarget.ArrayBuffer, iboId);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(ushort) * triangleIndex.Count), triangleIndex.ToArray (), BufferUsageHint.StaticDraw);

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);

			GL.GenBuffers (1, out vboId);
			GL.BindBuffer (BufferTarget.ArrayBuffer, vboId);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * 6), triangle.ToArray (), BufferUsageHint.StaticDraw);

			// VAO
			GL.GenVertexArrays (1, out vaoId);
			GL.BindVertexArray (vaoId);

			GL.BindBuffer (BufferTarget.ArrayBuffer, vboId);
			GL.VertexAttribPointer (0, 3, VertexAttribPointerType.Float, false, (sizeof(float) * 6), IntPtr.Zero);
			GL.VertexAttribPointer (1, 3, VertexAttribPointerType.Float, false, (sizeof(float) * 6), (IntPtr)(sizeof(float) * 3));

			GL.EnableVertexAttribArray (0);
			GL.EnableVertexAttribArray (0);

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, iboId);

			GL.BindVertexArray (0);
			GL.DisableVertexAttribArray (0);
			GL.DisableVertexAttribArray (1);
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);

			// Load shaders
			vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShaderId, new StreamReader("default.vert").ReadToEnd());
			GL.CompileShader(vertexShaderId);

			fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShaderId, new StreamReader("default.frag").ReadToEnd());
			GL.CompileShader(fragmentShaderId);

			shaderProgramId = GL.CreateProgram ();
			GL.AttachShader (shaderProgramId, vertexShaderId);
			GL.AttachShader (shaderProgramId, fragmentShaderId);

			GL.BindAttribLocation (shaderProgramId, 0, "InVertex");
			GL.BindAttribLocation (shaderProgramId, 1, "InNormal");

			GL.LinkProgram (shaderProgramId);

			Console.WriteLine (GL.GetProgramInfoLog (shaderProgramId));

			uniformProjectionModelView = GL.GetUniformLocation (shaderProgramId, "ProjectionModelviewMatrix");
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.EnableClientState (ArrayCap.VertexArray);

			Matrix4 projectionModelView = new Matrix4 (
				                              new Vector4 (1, 0, 0, 0),
				                              new Vector4 (0, 1, 0, 0),
				                              new Vector4 (0, 0, 1, 0),
				                              new Vector4 (0, 0, 0, 1)
			                              );

			GL.UseProgram (shaderProgramId);

			GL.UniformMatrix4(uniformProjectionModelView, false, ref projectionModelView);

			GL.BindVertexArray (vaoId);

			GL.DrawRangeElements (PrimitiveType.Triangles, 0, triangle.Count, triangleIndex.Count, DrawElementsType.UnsignedShort, triangleIndex.ToArray ());

			SwapBuffers ();
		}
			
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
		}
	}

	class ObjLoader
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

