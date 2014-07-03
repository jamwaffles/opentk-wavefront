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

		public App(): base(800, 600, new OpenTK.Graphics.GraphicsMode(32, 16, 0, 4))
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

			Title = "Wavefront model loader test";

			testModel = new WavefrontModel ("./suzanne.obj");

			defaultShader = new Shader("default.vert", "default.frag");
			uniformProjection = defaultShader.getUniform("mat_projection");
			uniformView = defaultShader.getUniform("mat_view");
			uniformWorld = defaultShader.getUniform("mat_world");
			uniformNormalTransform = defaultShader.getUniform("mat_normalTransform");

			GL.ClearColor (Color.CornflowerBlue);

			GL.Enable (EnableCap.DepthTest);
			GL.Enable (EnableCap.CullFace);
			GL.CullFace (CullFaceMode.Back);
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	
//			Matrix4 modelview = Matrix4.LookAt(0f, 0f, 5f, 0f, 0f, 0f, 0f, 1f, 0f);
//			GL.MatrixMode(MatrixMode.Modelview);
//			GL.LoadMatrix(ref modelview);
//
//			GL.Rotate (angle, new Vector3d (0.5, 1, 0));

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Vector3 eye = new Vector3(0.0f, 0.0f, -1.0f);
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

			testModel.draw ();

			SwapBuffers ();
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

			angle += 0.5f;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

			matProjection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 0.1f, 100.0f);
		}
	}
}

