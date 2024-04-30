using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using static Khronos.Platform;

namespace LSystem.Animate
{
    class Clothes
    {
        public static RawModel3d MergeOneTopology(List<Vertex3f> lstPositions, List<Vertex2f> lstTexCoord, 
            List<Vertex4i> lstBoneIndex, List<Vertex4f> lstBoneWeight,
            MeshTriangles meshTriangles, float expandValue = 0.0001f)
        {
            // 중복되는 점을 제거하여 단일 점리스트를 만든다.
            Dictionary<uint, uint> map = new Dictionary<uint, uint>();
            List<Vertex3f> pList = new List<Vertex3f>();

            for (uint i = 0; i < lstPositions.Count; i++)
            {
                bool isEqual = false;
                Vertex3f v = lstPositions[(int)i];
                for (uint j = 0; j < pList.Count; j++)
                {
                    Vertex3f p = pList[(int)j];
                    if (p.IsEqual(v, 0.00001f))
                    {
                        isEqual = true;
                        map[i] = j;
                        break;
                    }
                }

                if (!isEqual)
                {
                    map[i] = (uint)pList.Count;
                    pList.Add(v);
                }
            }

            // indices리스트를 만든다.
            List<uint> indices = new List<uint>();
            for (int i = 0; i < meshTriangles.Vertices.Count; i++)
            {
                indices.Add(map[meshTriangles.Vertices[i]]);
            }

            // normal 계산
            Vertex4f[] normals = new Vertex4f[pList.Count];
            for (int i = 0; i < indices.Count; i+=3)
            {
                int a = (int)indices[i];
                int b = (int)indices[i+1];
                int c = (int)indices[i+2];
                Vertex3f va = pList[a];
                Vertex3f vb = pList[b];
                Vertex3f vc = pList[c];
                Vertex3f n = (vb - va).Cross(vc - va).Normalized;

                normals[a].x += n.x;
                normals[a].y += n.y;
                normals[a].z += n.z;
                normals[a].w += 1.0f;

                normals[b].x += n.x;
                normals[b].y += n.y;
                normals[b].z += n.z;
                normals[b].w += 1.0f;

                normals[c].x += n.x;
                normals[c].y += n.y;
                normals[c].z += n.z;
                normals[c].w += 1.0f;
            }

            for (int i = 0; i < normals.Length; i++)
            {
                float w = normals[i].w;
                normals[i].x /= w;
                normals[i].y /= w;
                normals[i].z /= w;
            }

            for (int i = 0; i < pList.Count; i++)
            {
                pList[i] += new Vertex3f(normals[i].x, normals[i].y, normals[i].z) * expandValue;
            }

            for (uint i = 0; i < lstPositions.Count; i++)
            {
                lstPositions[(int)i] = pList[(int)map[i]];
            }

            int count = meshTriangles.Vertices.Count;
            float[] postions = new float[count * 3];
            float[] texcoords = new float[count * 2];
            uint[] boneIndices = new uint[count * 4];
            float[] boneWeights = new float[count * 4];
            for (int i = 0; i < count; i++)
            {
                int idx = (int)meshTriangles.Vertices[i];
                int tidx = (int)meshTriangles.Texcoords[i];
                postions[3 * i + 0] = lstPositions[idx].x;
                postions[3 * i + 1] = lstPositions[idx].y;
                postions[3 * i + 2] = lstPositions[idx].z;

                texcoords[2 * i + 0] = lstTexCoord[tidx].x;
                texcoords[2 * i + 1] = lstTexCoord[tidx].y;

                boneIndices[4 * i + 0] = (uint)lstBoneIndex[idx].x;
                boneIndices[4 * i + 1] = (uint)lstBoneIndex[idx].y;
                boneIndices[4 * i + 2] = (uint)lstBoneIndex[idx].z;
                boneIndices[4 * i + 3] = (uint)lstBoneIndex[idx].w;

                boneWeights[4 * i + 0] = (float)lstBoneWeight[idx].x;
                boneWeights[4 * i + 1] = (float)lstBoneWeight[idx].y;
                boneWeights[4 * i + 2] = (float)lstBoneWeight[idx].z;
                boneWeights[4 * i + 3] = (float)lstBoneWeight[idx].w;
            }

            // 로딩한 postions, boneIndices, boneWeights를 버텍스로
            List<Vertex3f>  _vertices = new List<Vertex3f>();
            List<Vertex4f>  _boneIndices = new List<Vertex4f>();
            List<Vertex4f> _boneWeights = new List<Vertex4f>();
            int vertexNum = postions.Length / 3;
            for (int i = 0; i < vertexNum; i++)
            {
                _vertices.Add(new Vertex3f(postions[i * 3 + 0], postions[i * 3 + 1], postions[i * 3 + 2]));
                _boneIndices.Add(new Vertex4f(boneIndices[i * 4 + 0], boneIndices[i * 4 + 1], boneIndices[i * 4 + 2], boneIndices[i * 4 + 3]));
                _boneWeights.Add(new Vertex4f(boneWeights[i * 4 + 0], boneWeights[i * 4 + 1], boneWeights[i * 4 + 2], boneWeights[i * 4 + 3]));
            }

            // VAO, VBO로 Raw3d 모델을 만든다.
            uint vao = Gl.GenVertexArray();
            Gl.BindVertexArray(vao);
            GpuLoader.StoreDataInAttributeList(0, 3, postions, BufferUsage.StaticDraw);
            GpuLoader.StoreDataInAttributeList(1, 2, texcoords, BufferUsage.StaticDraw);
            GpuLoader.StoreDataInAttributeList(3, 4, boneIndices, BufferUsage.StaticDraw);
            GpuLoader.StoreDataInAttributeList(4, 4, boneWeights, BufferUsage.StaticDraw);
            //GpuLoader.BindIndicesBuffer(lstVertexIndices.ToArray());
            Gl.BindVertexArray(0);
            RawModel3d _rawModel = new RawModel3d(vao, postions);

            return _rawModel;
        }
    }
}
