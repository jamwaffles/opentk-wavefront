using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	public class Shader
	{
		int programId, vertexId, fragmentId;

		public Shader (String vertexPath, String fragmentPath)
		{
			compile (vertexPath, fragmentPath);
		}

		private int compile(String vertexPath, String fragmentPath)
		{
			programId = GL.CreateProgram();

			vertexId = GL.CreateShader(ShaderType.VertexShader);
			fragmentId = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(vertexId, new StreamReader(vertexPath).ReadToEnd());
			GL.ShaderSource(fragmentId, new StreamReader(fragmentPath).ReadToEnd());

			GL.CompileShader(vertexId);
			GL.CompileShader(fragmentId);
			Console.WriteLine(GL.GetShaderInfoLog(vertexId));
			Console.WriteLine(GL.GetShaderInfoLog(fragmentId));

			GL.AttachShader(programId, vertexId);
			GL.AttachShader(programId, fragmentId);
			GL.LinkProgram(programId);
			Console.WriteLine(GL.GetProgramInfoLog(programId));
			GL.UseProgram(programId);

			return 0;
		}

		public int GetUniform(String uniform) 
		{
			return GL.GetUniformLocation(programId, uniform);
		}

		public int GetAttribute(String attr)
		{
			return GL.GetUniformLocation(programId, attr);
		}

		public void BindAttribute(int index, String attributeName)
		{
			GL.BindAttribLocation (programId, index, attributeName);
		}

		public void Use() 
		{
			GL.UseProgram (programId);
		}
	}
}

