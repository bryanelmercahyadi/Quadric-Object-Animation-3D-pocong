using LearnOpenTK.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using Project_Grafkom;

namespace Project_Grafkom
{
    class Bezier
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> vertices_temporary = new List<Vector3>();
        List<Vector3> textureVertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<uint> vertexIndices = new List<uint>();
        int _vertexBufferObject;
        int _vertexArrayObject;
        Shader _shader;
        Matrix4 transform;
        public List<Bezier> child = new List<Bezier>();
        public Bezier()
        {
        }
        public void createbezierVertices(float rx, float ry, float rz)
        {
            Vector3 temp_vector;
            int i = 0;
            temp_vector.X = vertices_temporary[i].X;
            temp_vector.Y = vertices_temporary[i].Y;
            temp_vector.Z = vertices_temporary[i].Z;
            vertices.Add(temp_vector);
            i++;
            for (float t = 0; t < 1.0f; t += 0.01f)
            {
                Vector3 p;
                p.X = 0;
                p.Y = 0;
                p.Z = 0;
                int[] segitigapascal = new int[vertices_temporary.Count + 1];
                int rows = vertices_temporary.Count - 1, coef = 1;
                for (int j = 0; j <= rows; j++)
                {
                    segitigapascal[j] = coef;
                    coef = coef * (rows - j) / (j + 1);
                }
                for (int j = 0; j < vertices_temporary.Count; j++)
                {
                    float a = segitigapascal[j] * (float)Math.Pow((1 - t), (vertices_temporary.Count - (j + 1))) * (float)Math.Pow(t, j);
                    p.X += a * vertices_temporary[j].X;
                    p.Y += a * vertices_temporary[j].Y;
                    p.Z += a * vertices_temporary[j].Z;
                }

                float _radiusX = rx;
                float _radiusY = ry;
                float _radiusZ = rz;
                float _pi = 3.14159f;
                float _cx = _radiusX;
                float _cy = _radiusY;

                for (float u = -_pi; u <= _pi; u += _pi / 30)
                {
                    for (float v = -_pi / 2; v < _pi / 2; v += _pi / 30)
                    {
                        temp_vector.X = p.X + _radiusX * (_cx + _radiusX * (float)Math.Cos(v)) * (float)Math.Cos(u);// x
                        temp_vector.Y = p.Y + _radiusY * (_cy + _radiusX * (float)Math.Cos(v)) * (float)Math.Sin(u); // y
                        temp_vector.Z = p.Z + _radiusZ * (float)Math.Sin(v); // z
                        vertices.Add(temp_vector);




                    }
                }
                i++;
            }
        }

        public void setupObject()
        {
            transform = Matrix4.Identity;
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                vertices.Count * Vector3.SizeInBytes,
                vertices.ToArray(),
                BufferUsageHint.StaticDraw);
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            _shader = new Shader("C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader.vert",
                "C:/Users/BRYAN/source/repos/Project Grafkom/Project Grafkom/Shaders/shader_putih_kuning.frag");
            _shader.Use();
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f));
        }
        public void render()
        {
            _shader.Use();
            //rotate();
            _shader.SetMatrix4("transform", transform);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.LineStrip,
               0, vertices.Count);
            foreach (var meshobj in child)
            {
                meshobj.render();
            }
        }
        public void addPoint(float x, float y, float z)
        {
            vertices_temporary.Add(new Vector3(x, y, z));
        }
        public void addPointList(List<Vector3> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                vertices_temporary.Add(list[i]);
            }
        }
        public List<Vector3> getVertices()
        {
            return vertices;
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
            return _vertexBufferObject;
        }

        public int getVertexArrayObject()
        {
            return _vertexArrayObject;
        }

        public Shader getShader()
        {
            return _shader;
        }

        public Matrix4 getTransform()
        {
            return transform;
        }

        //public void rotate()
        //{
        //    transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(1f));
        //    foreach (var meshobj in child)
        //    {
        //        meshobj.rotate();
        //    }
        //}
        public void scale(float s)
        {
            transform = transform * Matrix4.CreateScale(s);
            foreach (var meshobj in child)
            {
                meshobj.scale(s);
            }
        }
        public void translate(float x, float y, float z)
        {
            transform = transform * Matrix4.CreateTranslation(x, y, z);
            foreach (var meshobj in child)
            {
                meshobj.translate(x, y, z);
            }
        }
        public void rotate(float x, float y, float z)
        {
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(x));
            transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(y));
            transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(z));
            
        }
    }
}
