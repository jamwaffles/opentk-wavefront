using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	public class App: GameWindow
	{
		WavefrontModel testModel;

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			Title = "Wavefront model loader test";

			testModel = new WavefrontModel ("./sphere-lowpoly.obj");

			GL.ClearColor (Color.Black);
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);

			GL.MatrixMode(MatrixMode.Modelview);

			GL.LoadMatrix(ref modelview);

			testModel.draw ();

			SwapBuffers ();
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

