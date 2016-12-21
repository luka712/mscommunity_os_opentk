using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_1_triangle
{
    public class Game : GameWindow
    {
        private int programId;
        private int vertexAttributeLocation;
        private Vector2[] vertices = new Vector2[]
        {
            new Vector2(-.5f, 0f),
            new Vector2(0f, .8f),
            new Vector2(.5f, 0f)
        };

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string vertexShaderSource = File.ReadAllText("vertexShader.glsl");
            string fragmengShaderSource = File.ReadAllText("fragmentShader.glsl");

            programId = GL.CreateProgram();

            int vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderId, vertexShaderSource);
            GL.CompileShader(vertexShaderId);
            Console.WriteLine(GL.GetShaderInfoLog(vertexShaderId));
            GL.AttachShader(programId, vertexShaderId);

            int fragmetShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmetShaderId, fragmengShaderSource);
            GL.CompileShader(fragmetShaderId);
            Console.WriteLine(GL.GetShaderInfoLog(fragmetShaderId));
            GL.AttachShader(programId, fragmetShaderId);

            GL.LinkProgram(programId);

            BufferData();

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Viewport(0, 0, Width, Height);
        }

        private void BufferData()
        {
            vertexAttributeLocation = GL.GetAttribLocation(programId, "a_vertices");

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector2.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(vertexAttributeLocation, 2, VertexAttribPointerType.Float, false, 0,0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(programId);
            GL.EnableVertexAttribArray(vertexAttributeLocation);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

            GL.DisableVertexAttribArray(vertexAttributeLocation);

            SwapBuffers();
        }
    }
}
