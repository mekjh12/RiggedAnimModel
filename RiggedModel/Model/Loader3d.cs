using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystem
{
    class Loader3d
    {
        public static RawModel3d LoadAxis(float size)
        {
            float[] positions = new float[]
            {
                0, 0, 0, size, 0, 0,
                0, 0, 0, 0, size, 0,
                0, 0, 0, 0, 0, size, 
            };

            float[] colors = new float[]
            {
                1, 0, 0, 1, 0, 0,
                0, 1, 0, 0, 1, 0,
                0, 0, 1, 0, 0, 1,
            };

            List<Vertex3f> lstVertices = new List<Vertex3f>();
            for (int i = 0; i < positions.Length; i += 3)
            {
                lstVertices.Add(new Vertex3f(positions[i], positions[i + 1], positions[i + 2]));
            }

            List<Vertex3f> lstColors = new List<Vertex3f>();
            for (int i = 0; i < colors.Length; i += 3)
            {
                lstColors.Add(new Vertex3f(colors[i], colors[i + 1], colors[i + 2]));
            }

            RawModel3d rawModel = new RawModel3d();
            rawModel.Init(vertices: lstVertices.ToArray(), colors: lstColors.ToArray());
            rawModel.GpuBind();
            return rawModel;
        }

        public static RawModel3d LoadLine(float sx, float sy, float sz, float ex, float ey, float ez)
        {
            float[] positions = new float[] { sx, sy, sz, ex, ey, ez };

            List<Vertex3f> lstVertices = new List<Vertex3f>();
            for (int i = 0; i < positions.Length; i += 3)
            {
                lstVertices.Add(new Vertex3f(positions[i], positions[i + 1], positions[i + 2]));
            }

            RawModel3d rawModel = new RawModel3d();
            rawModel.Init(vertices: lstVertices.ToArray());
            rawModel.GpuBind();
            return rawModel;
        }

        /// <summary>
        /// 반시계 방향으로 회전하여 적용한다.
        /// </summary>
        /// <param name="rotDeg">degree</param>
        /// <returns></returns>
        public static RawModel3d LoadPlane(float rotDeg = 0, float texCoordRepeatNum = 1.0f)
        {
            float Cos(float radian) => (float)Math.Cos(radian);
            float Sin(float radian) => (float)Math.Sin(radian);

            float[] positions =
            {
                -1.0f, -1.0f, 0.0f,
                1.0f, -1.0f, 0.0f,
                1.0f, 1.0f,  0.0f,
                1.0f, 1.0f,  0.0f,
                -1.0f, 1.0f,  0.0f,
                -1.0f, -1.0f,  0.0f
            };

            float[] normals =
            {
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f
            };

            float[] textures =
            {
                0.0f, 0.0f,
                texCoordRepeatNum, 0.0f,
                texCoordRepeatNum, texCoordRepeatNum,
                texCoordRepeatNum, texCoordRepeatNum,
                0.0f, texCoordRepeatNum,
                0.0f, 0.0f
            };

            TangentSpace.CalculateTangents(positions, textures, normals, out float[] tangents, out float[] bitangents);

            // (tu,tv)를 회전하여 텍스처링한다.
            for (int i = 0; i < textures.Length; i += 2)
            {
                float tu = textures[i + 0];
                float tv = textures[i + 1];
                float deg = rotDeg.ToRadian();
                textures[i + 0] = Cos(deg) * tu - Sin(deg) * tv;
                textures[i + 1] = Sin(deg) * tu + Cos(deg) * tv;
            }

            List<Vertex3f> lstVertices = new List<Vertex3f>();
            for (int i = 0; i < positions.Length; i += 3)
                lstVertices.Add(new Vertex3f(positions[i], positions[i + 1], positions[i + 2]));

            List<Vertex2f> lstTexCoords = new List<Vertex2f>();
            for (int i = 0; i < textures.Length; i += 2)
                lstTexCoords.Add(new Vertex2f(textures[i], textures[i + 1]));

            List<Vertex3f> lstNormals = new List<Vertex3f>();
            for (int i = 0; i < normals.Length; i += 3)
                lstNormals.Add(new Vertex3f(normals[i], normals[i + 1], normals[i + 2]));

            RawModel3d rawModel = new RawModel3d();
            rawModel.Init(vertices: lstVertices.ToArray(), texCoords: lstTexCoords.ToArray(), normals: lstNormals.ToArray());
            rawModel.GpuBind();
            return rawModel;
        }

        /// <summary>
        /// 구면을 만든다.
        /// </summary>
        /// <param name="r">구의 반지름</param>
        /// <param name="piece">위도0도부터 90도까지 n등분을 한다. 이것은 360도를 4n등분한다.</param>
        /// <param name="outer">외부 또는 내부</param>
        /// <returns></returns>
        public static RawModel3d LoadSphere(float r = 1.0f, int piece = 3, float Tu = 1.0f, float Tv = 1.0f, bool outer = true)
        {
            float unitAngle = (float)(Math.PI / 2.0f) / piece;
            float deltaTheta = 360.ToRadian() / (4.0f * piece);

            List<float> vertices = new List<float>();
            List<float> textures = new List<float>();

            // <image url="$(ProjectDir)_PictureComment\SphereCoordinate.png" scale="0.8" />
            // 반시계 방향으로 면을 생성한다.
            for (int i = -piece; i < piece; i++) // phi
            {
                for (int j = 0; j < piece * 4; j++) // theta
                {
                    float theta1 = j * unitAngle;
                    float theta2 = (j + 1) * unitAngle;
                    float phi1 = i * unitAngle;
                    float phi2 = (i + 1) * unitAngle;
                    float tu1 = Tu * (deltaTheta * (j + 0)) / 360.ToRadian();
                    float tu2 = Tu * deltaTheta * (j + 1) / 360.ToRadian();
                    float tv1 = Tv * i * unitAngle / 90.ToRadian();
                    float tv2 = Tv * (i + 1) * unitAngle / 90.ToRadian();

                    if (outer)
                    {
                        if (i == (piece - 1))
                        {
                            vertices.AddRange(SphereCoordination(r, theta2, phi1));
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            textures.AddRange(TextureCoordination(tu2, tv1));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                        }
                        else if (i == -piece)
                        {
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                        }
                        else
                        {
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            vertices.AddRange(SphereCoordination(r, theta2, phi1));
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                            textures.AddRange(TextureCoordination(tu2, tv1));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                        }
                    }
                    else
                    {
                        if (i == (piece - 1))
                        {
                            vertices.AddRange(SphereCoordination(r, theta2, phi1));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            textures.AddRange(TextureCoordination(tu2, tv1));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                        }
                        else if (i == -piece)
                        {
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            vertices.AddRange(SphereCoordination(r, theta1, phi2));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                            textures.AddRange(TextureCoordination(tu1, tv2));
                        }
                        else
                        {
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            vertices.AddRange(SphereCoordination(r, theta1, phi2));
                            vertices.AddRange(SphereCoordination(r, theta2, phi1));
                            vertices.AddRange(SphereCoordination(r, theta1, phi1));
                            vertices.AddRange(SphereCoordination(r, theta2, phi2));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                            textures.AddRange(TextureCoordination(tu1, tv2));
                            textures.AddRange(TextureCoordination(tu2, tv1));
                            textures.AddRange(TextureCoordination(tu1, tv1));
                            textures.AddRange(TextureCoordination(tu2, tv2));
                        }
                    }
                }
            }

            float[] positions = vertices.ToArray();
            float[] texCoords = textures.ToArray();

            TangentSpace.CalculateTangents(positions, texCoords, positions, out float[] tangents, out float[] bitangents);

            List<Vertex3f> lstVertices = new List<Vertex3f>();
            for (int i = 0; i < positions.Length; i += 3)
                lstVertices.Add(new Vertex3f(positions[i], positions[i + 1], positions[i + 2]));

            RawModel3d rawModel = new RawModel3d();
            rawModel.Init(vertices: lstVertices.ToArray());
            rawModel.GpuBind();
            return rawModel;

            float[] TextureCoordination(float tu, float tv)
            {
                float[] verts = new float[2];
                verts[0] = tu;
                verts[1] = tv;
                return verts;
            }

            float[] SphereCoordination(float radius, float theta, float phi)
            {
                float[] verts = new float[3];
                float z = radius * (float)Math.Sin(phi);
                float R = radius * (float)Math.Cos(phi);
                float x = R * (float)Math.Cos(theta);
                float y = R * (float)Math.Sin(theta);
                verts[0] = x;
                verts[1] = y;
                verts[2] = z;
                return verts;
            }
        }


    }
}
