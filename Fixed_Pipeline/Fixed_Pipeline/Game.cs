
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Input;

namespace Fixed_Pipeline
{
    public class Game : GameWindow
    {
        Matrix4 transformationMatrix = Matrix4.Identity;
        float cameraZ = -4f;
        float angle = 0.0f;
        KeyboardState keyboardState;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Fixed pipeline example";

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Viewport(0, 0, Width, Height);

            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, (float)(Width / Height), 0.1f, 1000f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.S))
            {
                cameraZ -= .1f;
            }
            else if (keyboardState.IsKeyDown(Key.W))
            {
                cameraZ += .1f;
            }

            angle += .01f;
            transformationMatrix = Matrix4.Identity * Matrix4.CreateRotationZ(angle) * Matrix4.CreateRotationY(angle);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            var modelViewMatrix = Matrix4.LookAt(-new Vector3(0, 0, cameraZ), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);

            GL.MultMatrix(ref transformationMatrix);

            GL.Begin(PrimitiveType.Triangles);

            DrawTriangle();
            //DrawCube();

            GL.End();

            SwapBuffers();
        }

        private void DrawTriangle()
        {
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, 0.0f);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(1.0f, -1.0f, 0.0f);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
        }

        private void DrawCube()
        {
            var frontLeftDown = new Vector3(-1f, -1f, 1f);
            var frontRightDown = new Vector3(1f, -1f, 1f);
            var frontLeftUp = new Vector3(-1f, 1f, 1f);
            var frontRightUp = new Vector3(1f, 1f, 1f);

            var backLeftDown = new Vector3(-1f, -1f, -1f);
            var backRightDown = new Vector3(1f, -1f, -1f);
            var backLeftUp = new Vector3(-1f, 1f, -1f);
            var backRightUp = new Vector3(1f, 1f, -1f);

            var cubeCoordinates = new[]
             {
                 // Front face
                 frontLeftDown,
                 frontLeftUp, 
                 frontRightUp,
                 frontRightUp,
                 frontRightDown,
                 frontLeftDown,

                 // Back face
                 backLeftDown,
                 backLeftUp, 
                 backRightUp,
                 backRightUp,
                 backRightDown,
                 backLeftDown,

                 //LeftFace
                 backLeftDown, 
                 backLeftUp,
                 frontLeftUp,
                 frontLeftUp,
                 frontLeftDown,
                 backLeftDown,

                 // RightFace
                 backRightDown, 
                 backRightUp,
                 frontRightUp,
                 frontRightUp,
                 frontRightDown,
                 backRightDown,

                 // Up face
                 frontLeftUp,
                 backLeftUp,
                 backRightUp,
                 backRightUp,
                 frontRightUp,
                 frontLeftUp,

                 // Down face
                 frontLeftDown,
                 backLeftDown,
                 backRightDown,
                 backRightDown,
                 frontRightDown,
                 frontLeftDown
             };

            for (int i = 0; i < cubeCoordinates.Length;i++ )
            {
                GL.Color3(
                    (float)MathHelper.Clamp(cubeCoordinates[i].X, 0,1),
                    (float)MathHelper.Clamp(cubeCoordinates[i].Y, 0,1),
                    (float)MathHelper.Clamp(cubeCoordinates[i].Z, 0,1));
                GL.Vertex3(cubeCoordinates[i]);

            }
        }
    }
}
