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
 
layout (location = 0) in vec3 Position;
 
void main()
{
	gl_Position = vec4(0.5 * Position.x,
	                   0.5 * Position.y,
	                   Position.z, 1.0);
}";

		string fragmentShaderSource = @"
#version 330
 
out vec4 FragColor;
 
void main()
{
	FragColor = vec4(0.5, 0.8, 1.0, 1.0);
}";


		int vbo, 
		shaderProgramHandle, vertexShaderHandle, fragmentShaderHandle;

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
			Vector3[] vertices = new Vector3[3];
			vertices[0] = new Vector3(-1f, -1f, 0f);
			vertices[1] = new Vector3( 1f, -1f, 0f);
			vertices[2] = new Vector3( 0f,  1f, 0f);

			GL.GenBuffers(1, out vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
				new IntPtr(vertices.Length * Vector3.SizeInBytes),
				vertices, BufferUsageHint.StaticDraw);
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

			GL.EnableVertexAttribArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

			GL.DisableVertexAttribArray(0);

			SwapBuffers ();
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