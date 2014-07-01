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
		double angle = 0;

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

			testModel = new WavefrontModel ("./sphere-lowpoly.obj");

			/*GL.Light(LightName.Light0, LightParameter.Position, new float[] { 2.0f, 2.0f, 1.0f });
			GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.3f, 0.3f, 0.3f, 1.0f });
			GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
			GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
			GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);*/

			GL.ClearColor (Color.CornflowerBlue);

			GL.Enable (EnableCap.CullFace);

			GL.EnableClientState(EnableCap.VertexArray);
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	
			Matrix4 modelview = Matrix4.LookAt(0f, 0f, 5f, 0f, 0f, 0f, 0f, 1f, 0f);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref modelview);

			GL.Rotate (angle, new Vector3d (0, 1, 0));

			testModel.draw ();

			SwapBuffers ();
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

			angle += 0.5;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);
		}
	}
}

