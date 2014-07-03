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

			// load and compile fragment shader
			fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, new StreamReader(fragmentShaderPath).ReadToEnd());
			GL.CompileShader(fragmentShader);

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

/*using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace ObjLoader
{
	public class AttributeInfo
	{
		public String name = "";
		public int address = -1;
		public int size = 0;
		public ActiveAttribType type;
	}

	public class UniformInfo
	{
		public String name = "";
		public int address = -1;
		public int size = 0;
		public ActiveUniformType type;
	}

	public class Shader
	{
		int programId = -1;
		int vertexShaderId = -1;
		int fragmentShaderId = -1;
		public int attributeCount = 0;
		public int uniformCount = 0;

		public Dictionary<String, AttributeInfo> Attributes = new Dictionary<string, AttributeInfo>();
		public Dictionary<String, UniformInfo> Uniforms = new Dictionary<string, UniformInfo>();
		public Dictionary<String, uint> Buffers = new Dictionary<string, uint>();

		public Shader ()
		{
			programId = GL.CreateProgram();
		}

		public Shader (String vshader, String fshader)
		{
			programId = GL.CreateProgram();

			loadShaderFromFile(vshader, ShaderType.VertexShader);
			loadShaderFromFile(fshader, ShaderType.FragmentShader);

			link();
			genBuffers();
		}

		private void loadShader(String code, ShaderType type, out int address)
		{
			address = GL.CreateShader(type);
			GL.ShaderSource(address, code);
			GL.CompileShader(address);
			GL.AttachShader(programId, address);
			Console.WriteLine(GL.GetShaderInfoLog(address));
		}

		public void loadShaderFromFile(String filename, ShaderType type)
		{
			using (StreamReader sr = new StreamReader(filename))
			{
				if (type == ShaderType.VertexShader)
				{
					loadShader(sr.ReadToEnd(), type, out vertexShaderId);
				}
				else if (type == ShaderType.FragmentShader)
				{
					loadShader(sr.ReadToEnd(), type, out fragmentShaderId);
				}
			}
		}

		public void link()
		{
			GL.LinkProgram(programId);

			Console.WriteLine(GL.GetProgramInfoLog(programId));

			GL.GetProgram(programId, GetProgramParameterName.ActiveAttributes, out attributeCount);
			GL.GetProgram(programId, GetProgramParameterName.ActiveUniforms, out uniformCount);

			for (int i = 0; i < attributeCount; i++)
			{
				AttributeInfo info = new AttributeInfo();
				int length = 0;

				StringBuilder name = new StringBuilder();

				GL.GetActiveAttrib(programId, i, 256, out length, out info.size, out info.type, name);

				info.name = name.ToString();
				info.address = GL.GetAttribLocation(programId, info.name);
				Attributes.Add(name.ToString(), info);
			}

			for (int i = 0; i < uniformCount; i++)
			{
				UniformInfo info = new UniformInfo();
				int length = 0;

				StringBuilder name = new StringBuilder();

				GL.GetActiveUniform(programId, i, 256, out length, out info.size, out info.type, name);

				info.name = name.ToString();
				Uniforms.Add(name.ToString(), info);
				info.address = GL.GetUniformLocation(programId, info.name);
			}
		}

		public void genBuffers()
		{
			for (int i = 0; i < Attributes.Count; i++)
			{
				uint buffer = 0;
				GL.GenBuffers(1, out buffer);

				Buffers.Add(Attributes.Values.ElementAt(i).name, buffer);
			}

			for (int i = 0; i < Uniforms.Count; i++)
			{
				uint buffer = 0;
				GL.GenBuffers(1, out buffer);

				Buffers.Add(Uniforms.Values.ElementAt(i).name, buffer);
			}
		}

		public void enableVertexAttribArrays()
		{
			for (int i = 0; i < Attributes.Count; i++)
			{
				GL.EnableVertexAttribArray(Attributes.Values.ElementAt(i).address);
			}
		}

		public void disableVertexAttribArrays()
		{
			for (int i = 0; i < Attributes.Count; i++)
			{
				GL.DisableVertexAttribArray(Attributes.Values.ElementAt(i).address);
			}
		}

		public int getAttribute(string name)
		{
			if (Attributes.ContainsKey(name))
			{
				return Attributes[name].address;
			}
			else
			{
				return -1;
			}
		}

		public int getUniform(string name)
		{
			if (Uniforms.ContainsKey(name))
			{
				return Uniforms[name].address;
			}
			else
			{
				return -1;
			}
		}

		public uint getBuffer(string name)
		{
			if (Buffers.ContainsKey(name))
			{
				return Buffers[name];
			}
			else
			{
				return 0;
			}
		}
	}
}

*/