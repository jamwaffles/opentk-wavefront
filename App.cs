using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ObjLoader {
	public class ObjLoaderApp: GameWindow
	{
		string vertexShaderSource = @"
#version 330
 
layout (location = 0) in vec3 in_position;

uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;
 
void main()
{
	gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
}";

		string fragmentShaderSource = @"
#version 330

out vec4 out_frag_color;
 
void main()
{
	out_frag_color = vec4(1.0, 0.0, 0.0, 1.0);
}";

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

		int shaderProgramHandle, vertexShaderHandle, fragmentShaderHandle;
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
			GL.BindAttribLocation (shaderProgramHandle, 0, "in_position");

//			GL.EnableVertexAttribArray (1);
//			GL.BindBuffer (BufferTarget.ArrayBuffer, normVbo);
//			GL.VertexAttribPointer (1, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
//			GL.BindAttribLocation (shaderProgramHandle, 1, "in_normal");

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, indexVbo);
			GL.BindVertexArray (0);
		}

		void CreateShaders()
		{
			shaderProgramHandle = GL.CreateProgram();

			vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
			fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
			GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);

			GL.CompileShader(vertexShaderHandle);
			GL.CompileShader(fragmentShaderHandle);
			Console.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
			Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));

			GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
			GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);
			GL.LinkProgram(shaderProgramHandle);
			Console.WriteLine(GL.GetProgramInfoLog(shaderProgramHandle));
			GL.UseProgram(shaderProgramHandle);

			// Set uniforms
			projectionMatrixLoc = GL.GetUniformLocation(shaderProgramHandle, "projection_matrix");
			modelviewMatrixLoc = GL.GetUniformLocation(shaderProgramHandle, "modelview_matrix");

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