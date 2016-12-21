using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_7_Many_Cubes
{
    public class Game : GameWindow
    {
        private int programId;
        private int transformationMatrixLocation;
        private int viewMatrixLocation;
        private int projectionMatrixLocation;

        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix;

        private List<Cube> cubes = new List<Cube>();

        Vector3[] vertices = new[]
            {
                new Vector3(-1, -1, -1),
                new Vector3(-1, 1, -1),
                new Vector3(1, 1, -1),
                new Vector3(1,- 1, -1),
                new Vector3(-1, -1, 1),
                new Vector3(-1, 1, 1),
                new Vector3(1, 1, 1),
                new Vector3(1,-1, 1)
            };

        int[] indices = new[]
        {
                // front face
                0,1,2,
                2,3,0,

                // back face
                4,5,6,
                6,7,4,

                // left face
                0,1,5,
                5,4,0,

                // right face
                3,2,6,
                6,7,3,

                // up face
                1,5,6,
                6,2,1,

                // down face
                0,4,7,
                7,3,0
        };

        private Vector3[] colors = new Vector3[]
        {
            Vector3.UnitX,
            Vector3.UnitY, 
            Vector3.UnitZ,
            new Vector3(1,0,1),
            new Vector3(1,1,0),
            new Vector3(0,0,1),
            new Vector3(.5f, 1,.5f),
            new Vector3(.5f,1,1)
        };

       


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Random rand = new Random();

            int index = 0;
            while(index < 10)
            {
                index++;
                Matrix4 transform = Matrix4.Identity * Matrix4.CreateScale((float)rand.NextDouble());
                transform *= Matrix4.CreateRotationX((float)rand.NextDouble());
                transform *= Matrix4.CreateRotationY((float)rand.NextDouble());
                transform *= Matrix4.CreateRotationZ((float)rand.NextDouble());
                transform *= Matrix4.CreateTranslation(rand.Next(-5, 5), rand.Next(-5, 5), rand.Next(-20, -5));

                cubes.Add(new Cube() { TransformationMatrix = transform , Angle = 0f, Speed = (float)rand.NextDouble() });
            }

            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 4), Width / Height, .1f, 100f);
            viewMatrix = Matrix4.LookAt(new Vector3(0, 0, 7), Vector3.Zero, Vector3.UnitY);

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
            projectionMatrixLocation = GL.GetUniformLocation(programId, "u_projectionMatrix");
            viewMatrixLocation = GL.GetUniformLocation(programId, "u_viewMatrix");

            GL.ClearColor(1, 1, 1, 1);
            GL.Viewport(0, 0, Width, Height);
        }

        private void BufferData()
        {
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

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

            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(programId);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            foreach (var cube in cubes)
            {
                cube.Angle += cube.Speed * 0.01f;
                var t = cube.TransformationMatrix * Matrix4.CreateTranslation(new Vector3((float)Math.Sin(cube.Angle) * 3, (float)Math.Cos(cube.Angle) * 3, 0));
                GL.UniformMatrix4(transformationMatrixLocation, false, ref t);
                GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
                GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);

                GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            }

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            SwapBuffers();
        }

    }

    public class Cube
    {
        public Matrix4 TransformationMatrix { get; set; }
        public float Angle { get; set; }
        public float Speed { get; set; }
    }
}
