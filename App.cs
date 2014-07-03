using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	public class App: GameWindow
	{
		WavefrontModel testModel;

		float angle;

		Matrix4 matProjection;
		Matrix4 matView;
		Matrix4 matWorld;
		int uniformProjection;
		int uniformView;
		int uniformWorld;
		int uniformNormalTransform;

		Shader defaultShader;

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
		}

		void HandleKeyDown (object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.Escape) Exit ();
		}

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			GL.ClearDepth(0.0f);
			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Enable(EnableCap.DepthTest);

			testModel = new WavefrontModel ("./suzanne.obj");

			defaultShader = new Shader("default.vert", "default.frag");
			uniformProjection = defaultShader.getUniform("mat_projection");
			uniformView = defaultShader.getUniform("mat_view");
			uniformWorld = defaultShader.getUniform("mat_world");
			uniformNormalTransform = defaultShader.getUniform("mat_normalTransform");

			GL.ClearColor (Color.CornflowerBlue);

//			GL.Enable (EnableCap.DepthTest);
//			GL.Enable (EnableCap.CullFace);
//			GL.CullFace (CullFaceMode.Back);
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Vector3 eye = new Vector3(0.0f, 0.0f, -2.0f);
			Vector3 target = Vector3.Zero;
			Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
			matView = Matrix4.LookAt(eye, target, up);

			matWorld = Matrix4.CreateRotationY(angle);

			Matrix4 matNormalTransform;
			Matrix4.Mult(ref matProjection, ref matView, out matNormalTransform);
			Matrix4.Mult(ref matNormalTransform, ref matWorld, out matNormalTransform);
			matNormalTransform.Invert();

			GL.UniformMatrix4(uniformProjection, false, ref matProjection);
			GL.UniformMatrix4(uniformView, false, ref matView);
			GL.UniformMatrix4(uniformWorld, false, ref matWorld);
			GL.UniformMatrix4(uniformNormalTransform, true, ref matNormalTransform);

			testModel.bindShader (defaultShader);
			testModel.draw ();

			SwapBuffers ();
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

			angle += (float)e.Time;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

			matProjection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 0.1f, 100.0f);
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

