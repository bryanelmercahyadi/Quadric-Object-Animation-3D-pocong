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
    class Mesh6
    {
        List<Vector3> _ellipsoid_vertices = new List<Vector3>();
        int _vertexBufferObject_ellipsoid;
        int _vertexArrayObject_ellipsoid;
        int _ellipsoid_index;
        Shader _shader_ellipsoid;
        Matrix4 transform;
        int counter = 0;
        public Mesh6()
        {

        }
        public void createEllipsoidVertices()
        {
            float _positionX = 0.0f;
            float _positionY = 0.0f;
            float _positionZ = -0.1f;

            float _radiusX = 0.22f;
            float _radiusY = 0.22f;
            float _radiusZ = 0.35f;

            float _pi = 3.14159f;
            //buat temporary vector
            Vector3 temp_vector;

            for (float u = -_pi; u <= _pi; u += _pi / 50)
            {
                for (float v = -_pi / 2; v < _pi / 2; v += _pi / 50)
                {

                    temp_vector.X = _positionX + _radiusX * (float)Math.Cos(v) * (float)Math.Cos(u); // x
                    temp_vector.Y = _positionY + _radiusY * (float)Math.Cos(v) * (float)Math.Sin(u); // y
                    temp_vector.Z = _positionZ + _radiusZ * (float)Math.Sin(v); // z
                    _ellipsoid_vertices.Add(temp_vector);
                }
            }
        }
        public void setupObject()
        {
            transform = Matrix4.Identity;
            // VBO
            _vertexBufferObject_ellipsoid = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_ellipsoid);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                _ellipsoid_vertices.Count * Vector3.SizeInBytes,
                _ellipsoid_vertices.ToArray(), BufferUsageHint.StaticDraw);

            //VAO
            _vertexArrayObject_ellipsoid = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject_ellipsoid);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
                false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //Shader
            _shader_ellipsoid = new Shader("C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader.vert",
                "C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader.frag");
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f));
        }
        public void render()
        {

            //_shader_ellipsoid.Use();
            _shader_ellipsoid.SetMatrix4("transform", transform);
            //rotate();
            GL.BindVertexArray(_vertexArrayObject_ellipsoid);
            //perlu diganti di parameter 2

            GL.DrawArrays(PrimitiveType.TriangleFan,
               0, _ellipsoid_vertices.Count);
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
            return _ellipsoid_vertices;
        }

        public int getVertexBufferObject()
        {
            return _vertexBufferObject_ellipsoid;
        }


        public int getVertexArrayObject()
        {
            return _vertexArrayObject_ellipsoid;
        }

        public Shader getShader()
        {
            return _shader_ellipsoid;
        }

        public Matrix4 getTransform()
        {
            return transform;
        }

        //public void rotate()
        //{
        //    //sumbu Z
        //    //transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(20f));
        //    ////sumbu Y
        //    transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(0.05f));
        //    ////sumbu X
        //    //transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0.05f));
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
