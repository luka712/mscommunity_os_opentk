using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_3_ElementDrawing_Uniforms
{
    public class Game : GameWindow
    {
        private int programId;
        private int transformationMatrixLocation;

        private float angle;

        private Matrix4 transformationMatrix;

        private Vector2[] vertices = new Vector2[]
        {
            new Vector2(-.5f, -.5f),
            new Vector2(-.5f, .5f),
            new Vector2(.5f, .5f),
            new Vector2(.5f, -.5f)
        };

        private Vector3[] colors = new Vector3[]
        {
            // red
            Vector3.UnitX,
            // green
            Vector3.UnitY, 
            // blue
            Vector3.UnitZ,
            // purple I Guess
            new Vector3(1,0,1),
        };

        private int[] indices = new int[]
        {
            0,1,2,
            2,3,0
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

            transformationMatrixLocation = GL.GetUniformLocation(programId, "u_transformationMatrix");

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

            int indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData<int>(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(programId);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            angle += .01f;
            transformationMatrix = Matrix4.Identity * Matrix4.CreateRotationZ(angle);
            GL.UniformMatrix4(transformationMatrixLocation, false, ref transformationMatrix);

            GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            SwapBuffers();
        }
    }
}
