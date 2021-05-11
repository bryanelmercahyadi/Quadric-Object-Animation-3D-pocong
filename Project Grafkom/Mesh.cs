using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using LearnOpenTK.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Project_Grafkom;

namespace Project_Grafkom
{
    class Mesh
    {
        List<Vector3> _elliptic_vertices = new List<Vector3>();
        int _vertexBufferObject_elliptic;
        int _vertexArrayObject_elliptic;
        int _elliptic_index;
        Shader _shader_elliptic;
        Matrix4 transform;
        int counter = 0;
        public Mesh()
        {

        }
        public void createEllipticVertices()
        {
            float _positionX = 0.0f;
            float _positionY = 0.0f;
            float _positionZ = 0.62f;

            float _radiusx = 0.09f;
            float _radiusy = 0.09f;
            float _radiusz = 0.134f;
            float _pi = 3.14159f;
            //buat temporary vector
            Vector3 temp_vector;

            for (float u = -_pi; u < _pi; u += _pi / 30)
            {
                for (float v = -_pi / 2; v < _pi / 2; v += _pi / 30)
                {
                    temp_vector.X = _positionX + _radiusx * v * (float)Math.Cos(u); // x
                    temp_vector.Y = _positionY + _radiusy * v * (float)Math.Sin(u); // y
                    temp_vector.Z = _positionZ + v * v * _radiusz; // z
                    _elliptic_vertices.Add(temp_vector);
                }
            }
        }
        public void setupObject()
        {
            transform = Matrix4.Identity;
            // VBO
            _vertexBufferObject_elliptic = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_elliptic);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                _elliptic_vertices.Count * Vector3.SizeInBytes,
                _elliptic_vertices.ToArray(), BufferUsageHint.StaticDraw);

            //VAO
            _vertexArrayObject_elliptic = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject_elliptic);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
                false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //Shader
            _shader_elliptic = new Shader("C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader.vert",
                "C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader.frag");
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(270f));
        }
        public void render()
        {

            _shader_elliptic.Use();
            _shader_elliptic.SetMatrix4("transform", transform);
            //rotate();
            GL.BindVertexArray(_vertexArrayObject_elliptic);
            //perlu diganti di parameter 2

            GL.DrawArrays(PrimitiveType.TriangleFan,
               0, _elliptic_vertices.Count);
            if (counter == 200)
            {
                //translate();
                counter = 0;
            }
            else
            {
                counter++;
            }

        }
        public List<Vector3> getVertices()
        {
            return _elliptic_vertices;
        }
       
        public int getVertexBufferObject()
        {
            return _vertexBufferObject_elliptic;
        }


        public int getVertexArrayObject()
        {
            return _vertexArrayObject_elliptic;
        }

        public Shader getShader()
        {
            return _shader_elliptic;
        }

        public Matrix4 getTransform()
        {
            return transform;
        }

        //public void rotate()
        //{
        //    //sumbu Z
        //    //transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0.1234f));
        //    ////sumbu Y
        //    //transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(0.1235f));
        //    ////sumbu X
        //    //transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0.1235f));
        //}
        public void rotate(float degreeX, float degreeY, float degreeZ)
        {
            transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(degreeZ));
            transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(degreeY));
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(degreeX));

        }
        public void scale()
        {
            transform = transform * Matrix4.CreateScale(0.01f);

        }

        public void translate()
        {

            transform = transform * Matrix4.CreateTranslation(0.1f, 0.1f, 0.0f);
        }
    }
    
}
