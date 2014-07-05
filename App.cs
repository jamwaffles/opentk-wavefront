using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ObjLoader {
	public class ObjLoaderApp: GameWindow
	{
		Vector3[] positionData = new Vector3[]{
			// Front face
			new Vector3 (-1.0f, -1.0f, 1.0f), 
			new Vector3 (1.0f, -1.0f, 1.0f), 
			new Vector3 (1.0f, 1.0f, 1.0f), 
			new Vector3 (-1.0f, 1.0f, 1.0f),
			// Right face
			new Vector3 (1.0f, -1.0f, 1.0f), 
			new Vector3 (1.0f, -1.0f, -1.0f), 
			new Vector3 (1.0f, 1.0f, -1.0f), 
			new Vector3 (1.0f, 1.0f, 1.0f),
			// Back face
			new Vector3 (1.0f, -1.0f, -1.0f), 
			new Vector3 (-1.0f, -1.0f, -1.0f), 
			new Vector3 (-1.0f, 1.0f, -1.0f), 
			new Vector3 (1.0f, 1.0f, -1.0f),
			// Left face
			new Vector3 (-1.0f, -1.0f, -1.0f), 
			new Vector3 (-1.0f, -1.0f, 1.0f), 
			new Vector3 (-1.0f, 1.0f, 1.0f), 
			new Vector3 (-1.0f, 1.0f, -1.0f),
			// Top Face	
			new Vector3 (-1.0f, 1.0f, 1.0f), 
			new Vector3 (1.0f, 1.0f, 1.0f),
			new Vector3 (1.0f, 1.0f, -1.0f), 
			new Vector3 (-1.0f, 1.0f, -1.0f),
			// Bottom Face
			new Vector3 (1.0f, -1.0f, 1.0f), 
			new Vector3 (-1.0f, -1.0f, 1.0f),
			new Vector3 (-1.0f, -1.0f, -1.0f), 
			new Vector3 (1.0f, -1.0f, -1.0f) };

		Vector3[] normalData = new Vector3[] {
			// Front face
			new Vector3 ( 0f, 0f, 1f), 
			new Vector3 ( 0f, 0f, 1f),
			new Vector3 ( 0f, 0f, 1f),
			new Vector3 ( 0f, 0f, 1f), 
			// Right face
			new Vector3 ( 1f, 0f, 0f), 
			new Vector3 ( 1f, 0f, 0f), 
			new Vector3 ( 1f, 0f, 0f), 
			new Vector3 ( 1f, 0f, 0f),
			// Back face
			new Vector3 ( 0f, 0f, -1f), 
			new Vector3 ( 0f, 0f, -1f), 
			new Vector3 ( 0f, 0f, -1f),  
			new Vector3 ( 0f, 0f, -1f), 
			// Left face
			new Vector3 ( -1f, 0f, 0f),  
			new Vector3 ( -1f, 0f, 0f), 
			new Vector3 ( -1f, 0f, 0f),  
			new Vector3 ( -1f, 0f, 0f),
			// Top Face	
			new Vector3 ( 0f, 1f, 0f),  
			new Vector3 ( 0f, 1f, 0f), 
			new Vector3 ( 0f, 1f, 0f),  
			new Vector3 ( 0f, 1f, 0f),
			// Bottom Face
			new Vector3 ( 0f, -1f, 0f),  
			new Vector3 ( 0f, -1f, 0f), 
			new Vector3 ( 0f, -1f, 0f),  
			new Vector3 ( 0f, -1f, 0f)
		};

		int[] indexData = new int[] {
			// Font face
			0, 1, 2, 2, 3, 0, 
			// Right face
			7, 6, 5, 5, 4, 7, 
			// Back face
			11, 10, 9, 9, 8, 11, 
			// Left face
			15, 14, 13, 13, 12, 15, 
			// Top Face	
			19, 18, 17, 17, 16, 19,
			// Bottom Face
			23, 22, 21, 21, 20, 23
		};

//		int shaderProgramHandle, vertexShaderHandle, fragmentShaderHandle;
		Shader shader;
		int posVbo, /*normVbo,*/ indexVbo;
		int vaoId;

		int modelviewMatrixLoc, projectionMatrixLoc;
		Matrix4 modelviewMatrix, projectionMatrix;

		public ObjLoaderApp(): base(
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

			shader = new Shader ("default.vert", "default.frag");
		}

		void HandleKeyDown (object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.Escape) Exit ();
		}

		void CreateVertexBuffer()
		{
			// VBOs
			GL.GenBuffers (1, out posVbo);
			GL.BindBuffer (BufferTarget.ArrayBuffer, posVbo);
			GL.BufferData<Vector3> (BufferTarget.ArrayBuffer, new IntPtr (positionData.Length * Vector3.SizeInBytes), positionData, BufferUsageHint.StaticDraw);

//			GL.GenBuffers (1, out normVbo);
//			GL.BindBuffer (BufferTarget.ArrayBuffer, posVbo);
//			GL.BufferData<Vector3> (BufferTarget.ArrayBuffer, new IntPtr (normalData.Length * Vector3.SizeInBytes), normalData, BufferUsageHint.StaticDraw);

			GL.GenBuffers (1, out indexVbo);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, indexVbo);
			GL.BufferData (BufferTarget.ElementArrayBuffer, new IntPtr (indexData.Length * sizeof(int)), indexData, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			// VAO
			GL.GenVertexArrays (1, out vaoId);
			GL.BindVertexArray (vaoId);

			GL.EnableVertexAttribArray (0);
			GL.BindBuffer (BufferTarget.ArrayBuffer, posVbo);
			GL.VertexAttribPointer (0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
//			GL.BindAttribLocation (shaderProgramHandle, 0, "in_position");
			shader.BindAttribute (0, "in_position");

//			GL.EnableVertexAttribArray (1);
//			GL.BindBuffer (BufferTarget.ArrayBuffer, normVbo);
//			GL.VertexAttribPointer (1, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
//			GL.BindAttribLocation (shaderProgramHandle, 1, "in_normal");

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, indexVbo);
			GL.BindVertexArray (0);
		}

		void CreateShaders()
		{
			projectionMatrixLoc = shader.GetUniform ("projection_matrix");
			modelviewMatrixLoc = shader.GetUniform ("modelview_matrix");

			float aspectRatio = ClientSize.Width / (float)(ClientSize.Height);
			Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 1, 100, out projectionMatrix);
			modelviewMatrix = Matrix4.LookAt(new Vector3(0, 3, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

			GL.UniformMatrix4(projectionMatrixLoc, false, ref projectionMatrix);
			GL.UniformMatrix4(modelviewMatrixLoc, false, ref modelviewMatrix);
		}

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			GL.ClearColor (Color.CornflowerBlue);

			CreateVertexBuffer ();
			CreateShaders ();
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

			shader.Use ();

			GL.BindVertexArray (vaoId);
			GL.DrawElements (PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			SwapBuffers ();
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

			Matrix4 rotation = Matrix4.CreateRotationY((float)e.Time);
			Matrix4.Mult(ref rotation, ref modelviewMatrix, out modelviewMatrix);
			GL.UniformMatrix4(modelviewMatrixLoc, false, ref modelviewMatrix);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
		}
	}

	public class App {
		[STAThread]
		public static void Main() {
			using (ObjLoaderApp app = new ObjLoaderApp ()) 
			{
				app.Run (60.0);
			}
		}
	}
}