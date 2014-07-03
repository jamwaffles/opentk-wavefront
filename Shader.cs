using System;
using System.IO;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace ObjLoader
{
	public class Shader
	{
		public int program { get; private set; }
		private int vertexShader;
		private int fragmentShader;

		public Shader(string vertexShaderPath, string fragmentShaderPath)
		{
			// load and compile vertex shader
			vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, new StreamReader(vertexShaderPath).ReadToEnd());
			GL.CompileShader(vertexShader);

			Console.WriteLine (GL.GetShaderInfoLog (vertexShader));

			// load and compile fragment shader
			fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, new StreamReader(fragmentShaderPath).ReadToEnd());
			GL.CompileShader(fragmentShader);

			Console.WriteLine (GL.GetShaderInfoLog (fragmentShader));

			// create shader program
			program = GL.CreateProgram();
			GL.AttachShader(program, vertexShader);
			GL.AttachShader(program, fragmentShader);
			GL.LinkProgram(program);

			// check for errors
			string log;
			GL.GetProgramInfoLog(program, out log);
			Console.WriteLine(log);
		}

		public void use()
		{
			GL.UseProgram(program);
		}

		public int getUniform(string uniform)
		{
			return GL.GetUniformLocation(program, uniform);
		}
	}
}