using LSystem.Animate;
using OpenGL;
using System;
using System.Collections.Generic;

namespace LSystem
{
    static class Extension
    {
        private static float DegreeToRadian = (float)Math.PI / 180.0f;
        private static float RadianToDegree = 180.0f / (float)Math.PI;

        public static bool IsEqual(this Vertex3f a, Vertex3f b, float kEpsilon = 0.0001f)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) < kEpsilon;
        }

        public static bool IsEqual(this Vertex2f a, Vertex2f b, float kEpsilon = 0.0001f)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) < kEpsilon;
        }

        /// <summary>
        /// 소숫점 아래의 숫자를 버린다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalLength"></param>
        /// <returns></returns>
        public static float Round(this float value, uint decimalLength)
        {
            float exp = (float)Math.Pow(10, decimalLength);
            return (float)((int)(value * exp) * (1.0f / exp));
        }

        public static float ClampCycle(this float value, float min, float max)
        {
            if (value < min) return max;
            if (value > max) return min;
            return value;
        }

        public static float Clamp(this float value, float min, float max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }
        public static Vertex4f Vertex4f(this Vertex3f a)
        {
            return new Vertex4f(a.x, a.y, a.z, 1.0f);
        }

        public static Quaternion RotateQuaternionBetween(this Vertex3f a, Vertex3f b)
        {
            // 월드공간의 좌표로 변환을 해야 함.
            float af = a.Norm();
            float bf = b.Norm();
            if (af == 0 || bf == 0) return Quaternion.Identity;

            Vertex3f R = a.Cross(b).Normalized;
            float cf = (a - b).Norm();
            float cos = (af * af + bf * bf - cf * cf) / (2 * af * bf);
            float theta = (-1 <= cos && cos <= 1) ? ((float)Math.Acos(cos)).ToDegree() : 0.0f;
            return new Quaternion(R, theta);
        }

        public static Matrix4x4f RotateBetween(this Vertex3f a, Vertex3f b)
        {
            // 월드공간의 좌표로 변환을 해야 함.
            float af = a.Norm();
            float bf = b.Norm();
            if (af == 0 || bf == 0) return Matrix4x4f.Identity;

            Vertex3f R = a.Cross(b).Normalized;
            float cf = (a - b).Norm();
            float cos = (af * af + bf * bf - cf * cf) / (2 * af * bf);
            float theta = (-1 <= cos && cos <= 1) ? ((float)Math.Acos(cos)).ToDegree() : 0.0f;
            return (Matrix4x4f)new Quaternion(R, theta);
        }

        public static int Clamp(this int value, int min, int max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }

        public static Vertex3f Cross(this Vertex3f a, Vertex3f b)
        {
            //외적의 방향은 왼손으로 감는다.
            return new Vertex3f(a.y * b.z - a.z * b.y, -a.x * b.z + a.z * b.x, a.x * b.y - a.y * b.x);
        }

        public static float ToRadian(this int degree)
        {
            return (float)degree * DegreeToRadian;
        }

        public static float ToRadian(this float degree)
        {
            return degree * DegreeToRadian;
        }

        public static float ToDegree(this float radian)
        {
            return radian * RadianToDegree;
        }
        public static float Dot(this Vertex3f a, Vertex3f b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vertex3f Vertex3f(this Vertex4f vec)
        {
            return new Vertex3f(vec.x, vec.y, vec.z);
        }

        public static float Norm(this Vertex3f vec)
        {
            return (float)Math.Sqrt(vec.Dot(vec));
        }

        public static Matrix4x4f Scaled(this Matrix4x4f src, Vertex3f scale)
        {
            Matrix4x4f mat = Matrix4x4f.Identity;
            mat[0, 0] = scale.x;
            mat[1, 1] = scale.y;
            mat[2, 2] = scale.z;
            return mat * src;
        }

        /// <summary>
        /// 자신의 로컬공간에서 x축으로 회전한 각을 가져온다. (yz평면에서)
        /// </summary>
        /// <param name="pmat"></param>
        /// <returns></returns>
        public static float Pitch(this Matrix4x4f pmat)
        {
            Vertex3f yAxisChild = pmat.Column1.Vertex3f().Normalized;
            float phi = ((float)Math.Acos(OpenGL.Vertex3f.UnitY.Dot(yAxisChild))).ToDegree();
            phi = yAxisChild.z > 0 ? phi : -phi;
            return phi;
        }

        /// <summary>
        /// 자신의 로컬공간에서 z축으로 회전한 각을 가져온다. (xy평면에서)
        /// </summary>
        /// <param name="pmat"></param>
        /// <returns></returns>
        public static float Yaw(this Matrix4x4f pmat)
        {
            Vertex3f yAxisChild = pmat.Column1.Vertex3f().Normalized;
            float phi = ((float)Math.Acos(OpenGL.Vertex3f.UnitX.Dot(yAxisChild))).ToDegree();
            phi = yAxisChild.y > 0 ? phi : -phi;
            return phi;
        }

        /// <summary>
        /// 자신의 로컬공간에서 y축으로 회전한 각을 가져온다. (zx평면에서)
        /// </summary>
        /// <param name="pmat"></param>
        /// <returns></returns>
        public static float Roll(this Matrix4x4f pmat)
        {
            Vertex3f xAxisChild = pmat.Column0.Vertex3f();
            xAxisChild.y = 0.0f;
            xAxisChild.Normalize();
            float phi = ((float)Math.Acos(OpenGL.Vertex3f.UnitX.Dot(xAxisChild))).ToDegree();
            phi = xAxisChild.z > 0 ? -phi : phi;
            return phi;
        }


        public static Matrix4x4f ToMatrix4x4f(this Assimp.Matrix4x4 mat)  
        {
            Matrix4x4f m = Matrix4x4f.Identity;
            m[0, 0] = mat.A1;
            m[1, 0] = mat.A2;
            m[2, 0] = mat.A3;
            m[3, 0] = mat.A4;
            m[0, 1] = mat.B1;
            m[1, 1] = mat.B2;
            m[2, 1] = mat.B3;
            m[3, 1] = mat.B4;
            m[0, 2] = mat.C1;
            m[1, 2] = mat.C2;
            m[2, 2] = mat.C3;
            m[3, 2] = mat.C4;
            m[0, 3] = mat.D1;
            m[1, 3] = mat.D2;
            m[2, 3] = mat.D3;
            m[3, 3] = mat.D4;
            return m;
        }
        
        public static Vertex3f Multipy(this Matrix4x4f mat, Vertex3f p)
        {
            return (mat * p.Vertex4f()).Vertex3f();
        }

        public static Matrix4x4f ToMat4x4f(this Matrix3x3f mat, Vertex3f transpose)
        {
            Matrix4x4f m = Matrix4x4f.Identity;
            m[0, 0] = mat[0, 0];
            m[0, 1] = mat[0, 1];
            m[0, 2] = mat[0, 2];
            m[1, 0] = mat[1, 0];
            m[1, 1] = mat[1, 1];
            m[1, 2] = mat[1, 2];
            m[2, 0] = mat[2, 0];
            m[2, 1] = mat[2, 1];
            m[2, 2] = mat[2, 2];
            m[3, 0] = transpose.x;
            m[3, 1] = transpose.y;
            m[3, 2] = transpose.z;
            return m;
        }

        public static Matrix3x3f Frame(this Matrix3x3f mat, Vertex3f x, Vertex3f y, Vertex3f z)
        {
            mat[0, 0] = x.x;
            mat[0, 1] = x.y;
            mat[0, 2] = x.z;
            mat[1, 0] = y.x;
            mat[1, 1] = y.y;
            mat[1, 2] = y.z;
            mat[2, 0] = z.x;
            mat[2, 1] = z.y;
            mat[2, 2] = z.z;
            return mat;
        }

        /// <summary>
        /// 4x4행렬에서 3x3행렬의 회전행렬만 가져온다.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix3x3f Rot3x3f(this Matrix4x4f mat)
        {
            Matrix3x3f m = Matrix3x3f.Identity;
            m[0, 0] = mat[0, 0];
            m[0, 1] = mat[0, 1];
            m[0, 2] = mat[0, 2];
            m[1, 0] = mat[1, 0];
            m[1, 1] = mat[1, 1];
            m[1, 2] = mat[1, 2];
            m[2, 0] = mat[2, 0];
            m[2, 1] = mat[2, 1];
            m[2, 2] = mat[2, 2];
            return m;
        }

        public static Matrix3x3f Nomalized(this Matrix3x3f mat)
        {
            Matrix3x3f m = Matrix3x3f.Identity;
            float sx = 1.0f / mat.Column0.Norm();
            float sy = 1.0f / mat.Column1.Norm();
            float sz = 1.0f / mat.Column2.Norm();
            m[0, 0] = mat[0, 0] * sx;
            m[0, 1] = mat[0, 1] * sx;
            m[0, 2] = mat[0, 2] * sx;
            m[1, 0] = mat[1, 0] * sy;
            m[1, 1] = mat[1, 1] * sy;
            m[1, 2] = mat[1, 2] * sy;
            m[2, 0] = mat[2, 0] * sz;
            m[2, 1] = mat[2, 1] * sz;
            m[2, 2] = mat[2, 2] * sz;
            return m;
        }

        public static Quaternion QuaternionFromFrame(this Vertex3f z, Vertex3f x, Vertex3f y)
        {
            Matrix4x4f m = Matrix4x4f.Identity;
            m[0, 0] = x.x;
            m[0, 1] = x.y;
            m[0, 2] = x.z;
            m[1, 0] = y.x;
            m[1, 1] = y.y;
            m[1, 2] = y.z;
            m[2, 0] = z.x;
            m[2, 1] = z.y;
            m[2, 2] = z.z;
            return m.ToQuaternion();
        }


        public static Matrix4x4f CreateViewMatrix(Vertex3f pos, Vertex3f right, Vertex3f up, Vertex3f forward)
        {
            Matrix4x4f view = Matrix4x4f.Identity;
            view[0, 0] = right.x;
            view[1, 0] = right.y;
            view[2, 0] = right.z;
            view[0, 1] = up.x;
            view[1, 1] = up.y;
            view[2, 1] = up.z;
            view[0, 2] = forward.x;
            view[1, 2] = forward.y;
            view[2, 2] = forward.z;
            view[3, 0] = -right.Dot(pos);
            view[3, 1] = -up.Dot(pos);
            view[3, 2] = -forward.Dot(pos);
            return view;
        }

        /// <summary>
        /// [0, 1] 0:near 1:far
        /// </summary>
        /// <param name="fovy"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        /// <returns></returns>
        public static Matrix4x4f CreateProjectionMatrix(float fovy, float aspectRatio, float near, float far)
        {
            //   --------------------------
            //   g/s  0      0       0
            //   0    g      0       0
            //   0    0   f(f-n)  -nf/(f-n)
            //   0    0      1       0
            //   --------------------------
            float s = aspectRatio;// (float)_width / (float)_height;
            float g = 1.0f / (float)Math.Tan(fovy.ToRadian() * 0.5f); // g = 1/tan(fovy/2)
            float f = far;
            float n = near;
            Matrix4x4f m = new Matrix4x4f();
            m[0, 0] = g / s;
            m[1, 1] = g;
            m[2, 2] = f / (f - n);
            m[3, 2] = -(n * f) / (f - n);
            m[2, 3] = 1;
            return m;
        }

        /// <summary>
        /// Quaternions for Computer Graphics by John Vince. p199 참고
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Quaternion ToQuaternion(this Matrix4x4f mat)
        {
            Quaternion q = Quaternion.Identity;

            float a11 = mat[0, 0];
            float a12 = mat[2, 0];
            float a13 = mat[1, 0];

            float a21 = mat[0, 1];
            float a22 = mat[1, 1];
            float a23 = mat[2, 1];

            float a31 = mat[0, 2];
            float a32 = mat[1, 2];
            float a33 = mat[2, 2];

            float trace = a11 + a22 + a33;
            if (trace >= -1)
            {
                // I changed M_EPSILON to 0
                float s = 0.5f / (float)Math.Sqrt(trace + 1.0f);
                q.W = 0.25f / s;
                q.X = (a32 - a23) * s;
                q.Y = (a13 - a31) * s;
                q.Z = (a21 - a12) * s;
            }
            else
            {
                if (1 + a11 - a22 - a33 >= 0)
                {
                    float s = 2.0f * (float)Math.Sqrt(1.0f + a11 - a22 - a33);
                    q.X = 0.25f * s;
                    q.Y = (a12 + a21) / s;
                    q.Z = (a13 + a31) / s;
                    q.W = (a32 - a23) / s;
                }
                else if (1 - a11 + a22 - a33 >= 0)
                {
                    float s = 2.0f * (float)Math.Sqrt(1 - a11 + a22 - a33);
                    q.Y = 0.25f * s;
                    q.X = (a12 + a21) / s;
                    q.Z = (a23 + a32) / s;
                    q.W = (a13 - a31) / s;
                }
                else
                {
                    float s = 2.0f * (float)Math.Sqrt(1 - a11 - a22 + a33);
                    q.Z = 0.25f * s;
                    q.X = (a13 + a31) / s;
                    q.Y = (a23 + a32) / s;
                    q.W = (a21 - a12) / s;
                }
            }
            return q;
        }
    }
}
