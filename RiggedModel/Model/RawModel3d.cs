using OpenGL;
using System;

namespace LSystem
{
    public class RawModel3d
    {
        uint _vbo;
        uint _ibo;
        uint _vao;

        public uint VAO => _vao;

        bool _isDrawElement;

        public bool IsDrawElement
        {
            get => _isDrawElement;
            set => _isDrawElement = value;
        }

        int _vertexCount;

        public int VertexCount
        {
            get => _vertexCount;
            set => _vertexCount = value;
        }

        int _indexCount;
       
        Vertex3f[] _vertices;
        Vertex2f[] _texCoords;
        Vertex3f[] _normals;
        Vertex3f[] _colors;
        Vertex4i[] _boneIndex;
        Vertex4f[] _boneWeight;

        public Vertex3f[] Vertices => _vertices;

        public Vertex2f[] TexCoords => _texCoords;

        public Vertex3f[] Normals => _normals;

        public Vertex3f[] Colors => _colors;

        public Vertex4i[] BoneIndices => _boneIndex;

        public Vertex4f[] BoneWeights => _boneWeight;

        public RawModel3d(bool isDrawElement = false)// Vertex3f[] vertices, Vertex2f[] texCoords = null, Vertex3f[] normals = null, Vertex4i[] boneIndex = null, Vertex4f[] boneWeight = null)
        {
            _vao = Gl.GenVertexArray();
            _isDrawElement = false;
        }

        public void Init(Vertex3f[] vertices = null, Vertex2f[] texCoords = null, Vertex3f[] normals = null, Vertex3f[] colors = null,
            Vertex4i[] boneIndex = null, Vertex4f[] boneWeight = null)
        {
            if (vertices != null)
            {
                _vertexCount = vertices.Length;
                _vertices = new Vertex3f[_vertexCount];
                for (int i = 0; i < vertices.Length; i++)
                    _vertices[i] = vertices[i];
            }

            if (texCoords != null)
            {
                _texCoords = new Vertex2f[texCoords.Length];
                for (int i = 0; i < texCoords.Length; i++)
                    _texCoords[i] = texCoords[i];
            }

            if (normals != null)
            {
                _normals = new Vertex3f[normals.Length];
                for (int i = 0; i < normals.Length; i++)
                    _normals[i] = normals[i];
            }

            if (colors != null)
            {
                _colors = new Vertex3f[colors.Length];
                for (int i = 0; i < colors.Length; i++)
                    _colors[i] = colors[i];
            }

            if (boneIndex != null)
            {
                _boneIndex = new Vertex4i[boneIndex.Length];
                for (int i = 0; i < boneIndex.Length; i++)
                    _boneIndex[i] = boneIndex[i];
            }

            if (boneWeight != null)
            {
                _boneWeight = new Vertex4f[boneWeight.Length];
                for (int i = 0; i < boneWeight.Length; i++)
                    _boneWeight[i] = boneWeight[i];
            }
        }

        /// <summary>
        /// VAO, VBO, IBO의 메모리를 해제한다.
        /// </summary>
        public void Clean()
        {
            if (_vao >= 0)
            {
                if (_vbo >= 0) Gl.DeleteBuffers(_vbo);
                if (_ibo >= 0) Gl.DeleteBuffers(_ibo);
                Gl.DeleteVertexArrays(_vao);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GpuBind()
        {
            if (_vao >= 0) Clean();

            float[] postions;
            float[] texcoords;
            float[] normals;
            float[] colors;
            uint[] boneIndices;
            float[] boneWeights;

            _vao = Gl.GenVertexArray();
            Gl.BindVertexArray(_vao);

            if (_vertices != null)
            {
                postions = new float[_vertices.Length * 3];
                for (int i = 0; i < _vertices.Length; i++)
                {
                    postions[3 * i + 0] = _vertices[i].x;
                    postions[3 * i + 1] = _vertices[i].y;
                    postions[3 * i + 2] = _vertices[i].z;
                }
                GpuLoader.StoreDataInAttributeList(0, 3, postions, BufferUsage.StaticDraw);
            }

            if (_texCoords != null)
            {
                texcoords = new float[_texCoords.Length * 2];
                for (int i = 0; i < _texCoords.Length; i++)
                {
                    texcoords[2 * i + 0] = _texCoords[i].x;
                    texcoords[2 * i + 1] = _texCoords[i].y;
                }
                GpuLoader.StoreDataInAttributeList(1, 2, texcoords, BufferUsage.StaticDraw);
            }

            if (_normals != null)
            {
                normals = new float[_normals.Length * 3];
                for (int i = 0; i < _normals.Length; i++)
                {
                    normals[3 * i + 0] = _normals[i].x;
                    normals[3 * i + 1] = _normals[i].y;
                    normals[3 * i + 2] = _normals[i].z;
                }
                GpuLoader.StoreDataInAttributeList(2, 3, normals, BufferUsage.StaticDraw);
            }

            if (_colors != null)
            {
                colors = new float[_colors.Length * 3];
                for (int i = 0; i < _colors.Length; i++)
                {
                    colors[3 * i + 0] = _colors[i].x;
                    colors[3 * i + 1] = _colors[i].y;
                    colors[3 * i + 2] = _colors[i].z;
                }
                GpuLoader.StoreDataInAttributeList(3, 3, colors, BufferUsage.StaticDraw);
            }


            if (_boneIndex != null)
            {
                boneIndices = new uint[_boneIndex.Length * 4];
                for (int i = 0; i < _boneIndex.Length; i++)
                {
                    boneIndices[4 * i + 0] = (uint)_boneIndex[i].x;
                    boneIndices[4 * i + 1] = (uint)_boneIndex[i].y;
                    boneIndices[4 * i + 2] = (uint)_boneIndex[i].z;
                    boneIndices[4 * i + 3] = (uint)_boneIndex[i].w;
                }
                GpuLoader.StoreDataInAttributeList(4, 4, boneIndices, BufferUsage.StaticDraw);
            }

            if (_boneWeight != null)
            {
                boneWeights = new float[_boneWeight.Length * 4];
                for (int i = 0; i < _boneWeight.Length; i++)
                {
                    boneWeights[4 * i + 0] = _boneWeight[i].x;
                    boneWeights[4 * i + 1] = _boneWeight[i].y;
                    boneWeights[4 * i + 2] = _boneWeight[i].z;
                    boneWeights[4 * i + 3] = _boneWeight[i].w;
                }
                GpuLoader.StoreDataInAttributeList(5, 4, boneWeights, BufferUsage.StaticDraw);
            }

            Gl.BindVertexArray(0);
        }

    }
}
