using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_6_Textured_Cube
{
    public class Game : GameWindow
    {
        private int programId;
        private int transformationMatrixLocation;
        private int viewMatrixLocation;
        private int projectionMatrixLocation;
        private int texture;

        private float angle;

        private Matrix4 transformationMatrix;
        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix;

        Vector3[] vertices = new[]
            {
                //left
            new Vector3(-0.5f, -0.5f,  -0.5f),
            new Vector3(0.5f, 0.5f,  -0.5f),
            new Vector3(0.5f, -0.5f,  -0.5f),
            new Vector3(-0.5f, 0.5f,  -0.5f),
 
            //back
            new Vector3(0.5f, -0.5f,  -0.5f),
            new Vector3(0.5f, 0.5f,  -0.5f),
            new Vector3(0.5f, 0.5f,  0.5f),
            new Vector3(0.5f, -0.5f,  0.5f),
 
            //right
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3(0.5f, -0.5f,  0.5f),
            new Vector3(0.5f, 0.5f,  0.5f),
            new Vector3(-0.5f, 0.5f,  0.5f),
 
            //top
            new Vector3(0.5f, 0.5f,  -0.5f),
            new Vector3(-0.5f, 0.5f,  -0.5f),
            new Vector3(0.5f, 0.5f,  0.5f),
            new Vector3(-0.5f, 0.5f,  0.5f),
 
            //front
            new Vector3(-0.5f, -0.5f,  -0.5f),
            new Vector3(-0.5f, 0.5f,  0.5f),
            new Vector3(-0.5f, 0.5f,  -0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f),
 
            //bottom
            new Vector3(-0.5f, -0.5f,  -0.5f),
            new Vector3(0.5f, -0.5f,  -0.5f),
            new Vector3(0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f)
            };

        int[] indices = new[]
        {
            //left
            0,1,2,0,3,1,
 
            //back
            4,5,6,4,6,7,
 
            //right
            8,9,10,8,10,11,
 
            //top
            13,14,12,13,15,14,
 
            //front
            16,17,18,16,19,17,
 
            //bottom
            20,21,22,20,22,23
        };

        private Vector2[] texCoords = new Vector2[]
        {   
             // left
            new Vector2(0.0f, 0.0f),
            new Vector2(-1.0f, 1.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
 
            // back
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 1.0f),
            new Vector2(-1.0f, 0.0f),
 
            // right
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 1.0f),
 
            // top
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(-1.0f, 1.0f),
 
            // front
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
 
            // bottom
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 1.0f),
            new Vector2(-1.0f, 0.0f)
        };



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 4), Width / Height, .1f, 100f);
            viewMatrix = Matrix4.LookAt(new Vector3(0, 0, 3), Vector3.Zero, Vector3.UnitY);

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

            texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            var image = new Bitmap(Image.FromFile("fatcat.png"));
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.ClearColor(1, 1, 1, 1);
            GL.Viewport(0, 0, Width, Height);
        }

        private void BufferData()
        {
            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            int texBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, texBuffer);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texCoords.Count() * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

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

            angle += .01f;
            transformationMatrix = Matrix4.Identity * Matrix4.CreateRotationZ(angle) * Matrix4.CreateRotationY(angle * .5f);
            GL.UniformMatrix4(transformationMatrixLocation, false, ref transformationMatrix);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixLocation, false, ref viewMatrix);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            SwapBuffers();
        }
    }
}
