using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace LSystem.Animate
{
    public class Transform
    {
        Matrix4x4f _transform;

        public Matrix4x4f Matrix4x4f => _transform;

        public Transform()
        {
            _transform = Matrix4x4f.Identity;
        }

        public void SetPosition(Vertex3f pos)
        {
            _transform[3, 0] = pos.x;
            _transform[3, 1] = pos.y;
            _transform[3, 2] = pos.z;
        }

        public void IncreasePosition(float dx, float dy, float dz)
        {
            SetPosition(_transform.Position + new Vertex3f(dx, dy, dz));
        }


        public void Yaw(float deltaDegree)
        {
            Vertex3f up = -_transform.Column1.Vertex3f(); // 오른손 법칙으로
            Quaternion q = new Quaternion(up, deltaDegree);
            _transform = ((Matrix4x4f)(q * _transform.ToQuaternion()));
        }

        public virtual void Roll(float deltaDegree)
        {
            Vertex3f forward = _transform.Column2.Vertex3f();
            Quaternion q = new Quaternion(forward, deltaDegree);
            _transform = ((Matrix4x4f)(q * _transform.ToQuaternion()));
        }

        public virtual void Pitch(float deltaDegree)
        {
            Vertex3f right = _transform.Column0.Vertex3f();
            Quaternion q = new Quaternion(right, deltaDegree);
            _transform = ((Matrix4x4f)(q * _transform.ToQuaternion()));
        }

    }
}
