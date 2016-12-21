using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Graphics;
using System.Drawing;

namespace OpenTK_Intro
{
    public class Game : GameWindow
    {
        int programId;
        int vertexArrayId;
        int texId;

        Matrix4 projectionMatrix;
        Matrix4 viewMatrix;
        Matrix4 transformationMatrix;

        int projectionMatrixLocation;
        int viewMatrixLocation;
        int transformMatrixLocation;

        float angle;

        Vector3[] cubeVertices = new[]
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

        Vector2[] texCoords = new[]
        {
            // front
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),

            // back
             new Vector2(0.0f, 1.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f),

            // left
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),

            // right
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),

            // front
          new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(0.0f, 0.0f),

            // bottom
             new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(0.0f, 0.0f),
        };

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            programId = GL.CreateProgram();

            var vertexShaderText = File.ReadAllText("vertexShader.glsl");
            var fragmentShaderText = File.ReadAllText("fragmentShader.glsl");

            var vertexShaderId = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vertexShaderId, vertexShaderText);
            GL.CompileShader(vertexShaderId);
            Console.WriteLine(GL.GetShaderInfoLog(vertexShaderId));
            GL.AttachShader(programId, vertexShaderId);

            var fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(fragmentShaderId, fragmentShaderText);
            GL.CompileShader(fragmentShaderId);
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderId));
            GL.AttachShader(programId, fragmentShaderId);

            GL.LinkProgram(programId);

            LoadBuffers();

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Viewport(0, 0, Width, Height);

            LoadUniforms();
            LoadTexture();
        }

        private void LoadBuffers()
        {
            var positionAttributeLocation = GL.GetAttribLocation(programId, "a_verts");
            var texCoordsAttribLocation = GL.GetAttribLocation(programId, "a_texCoords");

            GL.BindAttribLocation(programId, positionAttributeLocation, "a_verts");
            GL.BindAttribLocation(programId, texCoordsAttribLocation, "a_texCoords");

            vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayId);

            var positionBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(cubeVertices.Count() * Vector3.SizeInBytes),
                cubeVertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttributeLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

            var textureBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texCoords.Count() * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(texCoordsAttribLocation, 2, VertexAttribPointerType.Float, false, 0,0);

            var indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData<int>(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(positionAttributeLocation);
            GL.EnableVertexAttribArray(texCoordsAttribLocation);
        }

        private void LoadTexture()
        {
            var bitmap = new Bitmap(Bitmap.FromFile("fatcat.png"));
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);
           

            bitmap.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }


        private void LoadUniforms()
        {
            projectionMatrixLocation = GL.GetUniformLocation(programId, "u_projectionMatrix");
            viewMatrixLocation = GL.GetUniformLocation(programId, "u_viewMatrix");
            transformMatrixLocation = GL.GetUniformLocation(programId, "u_transformationMatrix");

            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / Height, .1f, 100f);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);

            var viewModelMatrix = Matrix4.LookAt(new Vector3(0, 0, -5), Vector3.Zero, Vector3.UnitY);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewModelMatrix);

            var transformationMatrix = Matrix4.Identity;
            GL.UniformMatrix4(transformMatrixLocation, false, ref transformationMatrix);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            angle += 0.01f;

            GL.UseProgram(programId);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);


            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / Height, .1f, 100f);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);

            var viewModelMatrix = Matrix4.LookAt(new Vector3(0, 0, -5), Vector3.Zero, Vector3.UnitY);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewModelMatrix);

            var transformationMatrix = Matrix4.Identity * Matrix4.CreateRotationY(angle); //* Matrix4.CreateRotationZ(.2f);
            GL.UniformMatrix4(transformMatrixLocation, false, ref transformationMatrix);

            GL.BindVertexArray(vertexArrayId);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texId);
            
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();

            GL.UseProgram(0);
        }


    }
}
