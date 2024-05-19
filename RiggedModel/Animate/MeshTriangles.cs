using System.Collections.Generic;

namespace LSystem.Animate
{
    class MeshTriangles
    {
        string _material;
        List<uint> _vertices;
        List<uint> _normals;
        List<uint> _texcoords;
        List<uint> _colors;

        public string Material
        {
            get => _material;
            set => _material = value;
        }

        public List<uint> Vertices => _vertices;

        public List<uint> Texcoords => _texcoords;

        public List<uint> Normals => _normals;

        public MeshTriangles()
        {
            _vertices = new List<uint>();
            _normals = new List<uint>();
            _texcoords = new List<uint>();
            _colors = new List<uint>();
        }

        public void AddVertices(params uint[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                _vertices.Add(values[i]);
            }
        }

        public void AddTexCoords(params uint[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                _texcoords.Add(values[i]);
            }
        }

        public void AddNormals(params uint[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                _normals.Add(values[i]);
            }
        }
    }
}
