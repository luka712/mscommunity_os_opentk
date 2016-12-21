using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_2_colored_triangle
{
    public class Game : GameWindow
    {
        private int programId;

        private Vector2[] vertices = new Vector2[]
        {
            new Vector2(-.5f, 0f),
            new Vector2(0f, .8f),
            new Vector2(.5f, 0f)
        };

        private Vector3[] colors = new Vector3[]
        {
            // red
            Vector3.UnitX,
            // green
            Vector3.UnitY, 
            // blue
            Vector3.UnitZ
        };

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string vertexShaderSource = File.ReadAllText("vertexShader.glsl");
            string fragmentShaderSource = File.ReadAllText("fragmentShader.glsl");

            programId = GL.CreateProgram();

            int vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderId, vertexShaderSource);
            GL.CompileShader(vertexShaderId);
            Console.WriteLine(GL.GetShaderInfoLog(vertexShaderId));
            GL.AttachShader(programId, vertexShaderId);

            int fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderId, fragmentShaderSource);
            GL.CompileShader(fragmentShaderId);
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderId));
            GL.AttachShader(programId, fragmentShaderId);

            GL.LinkProgram(programId);

            BufferData();

            GL.ClearColor(1, 1, 1, 1);
            GL.Viewport(0, 0, Width, Height);
        }

        private void BufferData()
        {
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector2.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

            int colorBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length * Vector3.SizeInBytes), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(programId);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            SwapBuffers();
        }
    }
}
