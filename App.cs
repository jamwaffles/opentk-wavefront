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

		Vector3[] colourData = new Vector3[] {
			new Vector3(0.583f, 0.771f, 0.014f),
			new Vector3(0.609f, 0.115f, 0.436f),
			new Vector3(0.327f, 0.483f, 0.844f),
			new Vector3(0.822f, 0.569f, 0.201f),
			new Vector3(0.435f, 0.602f, 0.223f),
			new Vector3(0.310f, 0.747f, 0.185f),
			new Vector3(0.597f, 0.770f, 0.761f),
			new Vector3(0.559f, 0.436f, 0.730f),
			new Vector3(0.359f, 0.583f, 0.152f),
			new Vector3(0.483f, 0.596f, 0.789f),
			new Vector3(0.559f, 0.861f, 0.639f),
			new Vector3(0.195f, 0.548f, 0.859f),
			new Vector3(0.014f, 0.184f, 0.576f),
			new Vector3(0.771f, 0.328f, 0.970f),
			new Vector3(0.406f, 0.615f, 0.116f),
			new Vector3(0.676f, 0.977f, 0.133f),
			new Vector3(0.971f, 0.572f, 0.833f),
			new Vector3(0.140f, 0.616f, 0.489f),
			new Vector3(0.997f, 0.513f, 0.064f),
			new Vector3(0.945f, 0.719f, 0.592f),
			new Vector3(0.543f, 0.021f, 0.978f),
			new Vector3(0.279f, 0.317f, 0.505f),
			new Vector3(0.167f, 0.620f, 0.077f),
			new Vector3(0.347f, 0.857f, 0.137f),
			new Vector3(0.055f, 0.953f, 0.042f),
			new Vector3(0.714f, 0.505f, 0.345f),
			new Vector3(0.783f, 0.290f, 0.734f),
			new Vector3(0.722f, 0.645f, 0.174f),
			new Vector3(0.302f, 0.455f, 0.848f),
			new Vector3(0.225f, 0.587f, 0.040f),
			new Vector3(0.517f, 0.713f, 0.338f),
			new Vector3(0.053f, 0.959f, 0.120f),
			new Vector3(0.393f, 0.621f, 0.362f),
			new Vector3(0.673f, 0.211f, 0.457f),
			new Vector3(0.820f, 0.883f, 0.371f),
			new Vector3(0.982f, 0.099f, 0.879f)
		};

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
			
		Shader shader;
		int posVbo, indexVbo, colourVbo, normalVbo;
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

			GL.GenBuffers (1, out colourVbo);
			GL.BindBuffer (BufferTarget.ArrayBuffer, colourVbo);
			GL.BufferData<Vector3> (BufferTarget.ArrayBuffer, new IntPtr (colourData.Length * Vector3.SizeInBytes), colourData, BufferUsageHint.StaticDraw);

			GL.GenBuffers (1, out normalVbo);
			GL.BindBuffer (BufferTarget.ArrayBuffer, normalVbo);
			GL.BufferData<Vector3> (BufferTarget.ArrayBuffer, new IntPtr (normalData.Length * Vector3.SizeInBytes), normalData, BufferUsageHint.StaticDraw);

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
			shader.BindAttribute (0, "in_position");

			GL.EnableVertexAttribArray (1);
			GL.BindBuffer (BufferTarget.ArrayBuffer, colourVbo);
			GL.VertexAttribPointer (1, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
			shader.BindAttribute (1, "in_colour");

			GL.EnableVertexAttribArray (2);
			GL.BindBuffer (BufferTarget.ArrayBuffer, normalVbo);
			GL.VertexAttribPointer (2, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
			shader.BindAttribute (1, "in_normal");

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
			GL.BindVertexArray (0);
		}

		void CreateShaders()
		{
			projectionMatrixLoc = shader.GetUniform ("projection_matrix");
			modelviewMatrixLoc = shader.GetUniform ("modelview_matrix");

			float aspectRatio = ClientSize.Width / (float)(ClientSize.Height);
			Matrix4.CreatePerspectiveFieldOfView(0.75f, aspectRatio, 1, 100, out projectionMatrix);
			modelviewMatrix = Matrix4.LookAt(new Vector3(0, 3, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

			GL.UniformMatrix4(projectionMatrixLoc, false, ref projectionMatrix);
			GL.UniformMatrix4(modelviewMatrixLoc, false, ref modelviewMatrix);
		}

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			GL.ClearColor (Color.CornflowerBlue);

			GL.Enable (EnableCap.DepthTest);
			GL.DepthFunc (DepthFunction.Less);

			CreateVertexBuffer ();
			CreateShaders ();
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

			shader.Use ();

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, indexVbo);
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