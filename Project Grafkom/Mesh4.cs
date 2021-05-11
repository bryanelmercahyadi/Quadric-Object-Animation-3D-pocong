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
    class Mesh4
    {
        List<Vector3> _elliptic_cylinder_vertices = new List<Vector3>();
        List<Vector3> textureVertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<uint> vertexIndices = new List<uint>();
        int _vertexBufferObject_elliptic_cylinder;
        int _vertexArrayObject_elliptic_cylinder;
        int _elliptic_cylinder_index;
        Shader _shader_elliptic_cylinder;
        Matrix4 transform;
        int counter = 0;

        public Mesh4()
        {

        }
        public void createEllipticCylinderVertices()
        {
            float _positionX = 0.0f;
            float _positionY = 0.0f;
            float _positionZ = 0.37f;
            float _radiusX = 0.17f;
            float _radiusY = 0.17f;
            float _radiusZ = 0.2f;
            float _pi = 3.14159f;
            Vector3 temp_vector;
            for (float u = -_pi; u <= _pi; u += _pi / 30)
            {
                for (float v = -_pi / 2; v < _pi / 2; v += _pi / 30)
                {
                    temp_vector.X = _positionX + (float)Math.Cos(u) * _radiusX; // x
                    temp_vector.Y = _positionY +  (float)Math.Sin(u) * _radiusY; // y
                    temp_vector.Z = _positionZ + v * _radiusZ; // z
                    _elliptic_cylinder_vertices.Add(temp_vector);




                }
            }


        }
        public void setupObject()
        {
            transform = Matrix4.Identity;
            // VBO
            _vertexBufferObject_elliptic_cylinder = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_elliptic_cylinder);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                _elliptic_cylinder_vertices.Count * Vector3.SizeInBytes,
                _elliptic_cylinder_vertices.ToArray(), BufferUsageHint.StaticDraw);

            //VAO
            _vertexArrayObject_elliptic_cylinder = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject_elliptic_cylinder);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,
                false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //Shader
            _shader_elliptic_cylinder = new Shader("C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader.vert",
                "C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader.frag");
            _shader_elliptic_cylinder.Use();
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f));
        }
        public void render()
        {

            _shader_elliptic_cylinder.Use();
            _shader_elliptic_cylinder.SetMatrix4("transform", transform);
            //rotate();
            GL.BindVertexArray(_vertexArrayObject_elliptic_cylinder);
            //perlu diganti di parameter 2

            GL.DrawArrays(PrimitiveType.TriangleFan,
               0, _elliptic_cylinder_vertices.Count);
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
            return _elliptic_cylinder_vertices;
        }
        public List<uint> getVertexIndices()
        {
            return vertexIndices;
        }

        public void setVertexIndices(List<uint> temp)
        {
            vertexIndices = temp;
        }
        public int getVertexBufferObject()
        {
            return _vertexBufferObject_elliptic_cylinder;
        }


        public int getVertexArrayObject()
        {
            return _vertexArrayObject_elliptic_cylinder;
        }

        public Shader getShader()
        {
            return _shader_elliptic_cylinder;
        }

        public Matrix4 getTransform()
        {
            return transform;
        }

        //public void rotate()
        //{
        //    //sumbu Z
        //    //transform = transform * aMatrix4.CreateRotationZ(MathHelper.DegreesToRadians(20f));
        //    ///sumbu Y
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
