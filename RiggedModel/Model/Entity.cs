using OpenGL;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace LSystem
{
    public class Entity
    {
        protected string _name;
        protected uint _guid = 0;
        protected Vertex3f _color;
        protected List<RawModel3d> _models;

        protected Matrix4x4f _bind = Matrix4x4f.Identity;
        protected Vertex3f rot;
        protected Vertex3f sc;
        protected Vertex3f pos;

        protected Pose _pose;

        protected Material _material;
        protected bool _isAxisVisible = false;
        protected PolygonMode _polygonMode = (PolygonMode)0;
        protected float _drawThick = 1.0f;


        bool _isOnlyOneJointWeight = false;
        int _boneIndexOnlyOneJoint;

        public Vertex3f Rot => rot;

        public Vertex3f Sc => sc;

        public Vertex3f Pos => pos;

        public Matrix4x4f BindMatrix => _bind;

        public int BoneIndexOnlyOneJoint
        {
            get => _boneIndexOnlyOneJoint;
            set => _boneIndexOnlyOneJoint = value;
        }

        public bool IsOnlyOneJointWeight
        {
            get => _isOnlyOneJointWeight;
            set => _isOnlyOneJointWeight = value;
        }

        public float DrawThick
        {
            get => _drawThick;
            set => _drawThick = value;
        }

        public uint OBJECT_GUID => _guid;

        public bool IsAxisVisible
        {
            get => _isAxisVisible;
            set => _isAxisVisible = value;
        }

        public PolygonMode PolygonMode
        {
            get => _polygonMode;
            set => _polygonMode = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public List<RawModel3d> Models
        {
            get => _models;
            set
            {
                _models = value;
            }
        }

        public Vertex3f Position
        {
            get => _pose.Postiton;
            set => _pose.Postiton = value;
        }

        public Matrix4x4f ModelMatrix
        {
            get 
            {
                Matrix4x4f S = Matrix4x4f.Identity;// Matrix4x4f.Scaled(_scale.x, _scale.y, _scale.z); 
                Matrix4x4f R = _pose.Rotation;
                Matrix4x4f T = Matrix4x4f.Translated(_pose.Postiton.x, _pose.Postiton.y, _pose.Postiton.z);
                return T * R * S; // [순서 중요] 연산순서는 S->R->T순이다.
            }
        }

        public Material Material
        {
            get => _material;
            set => _material = value;
        }

        public Entity(string name, params TexturedModel[] rawModel3Ds)
        {
            _pose = new Pose(Quaternion.Identity, Vertex3f.Zero);
            _guid = GUID.GenID;
            _pose.Postiton = Vertex3f.Zero;
            _material = Material.White;

            if (_models == null) 
                _models = new List<RawModel3d>();
            _models.AddRange(rawModel3Ds);
            _name = (name == "") ? $"Entity" + _guid : name;
            _color = Rand.NextColor3f;
        }

        public void BindTransform(float sx = 1.0f, float sy = 1.0f, float sz = 1.0f, float rotx = 0, float roty = 0, float rotz = 0, float x =0, float y = 0, float z = 0)
        {
            sc = new Vertex3f(sx, sy, sz);
            rot = new Vertex3f(rotx, roty, rotz);
            pos = new Vertex3f(x, y, z);

            _bind = Matrix4x4f.Translated(x, y, z) * Matrix4x4f.RotatedX(rotx) * Matrix4x4f.RotatedY(roty) * Matrix4x4f.RotatedZ(rotz) * Matrix4x4f.Scaled(sx, sy, sz);
        }

        public void IncreasePosition(float dx, float dy, float dz)
        {
            _pose.Postiton += new Vertex3f(dx, dy, dz);
        }

        public virtual void Yaw(float deltaDegree)
        {
            Vertex3f up = -_pose.Rotation.Column1.Vertex3f(); // 오른손 법칙으로
            Quaternion q = new Quaternion(up, deltaDegree);
            _pose.Quaternion = q * _pose.Quaternion;
        }

        public virtual void Roll(float deltaDegree)
        {
            Vertex3f forward = _pose.Rotation.Column2.Vertex3f();
            Quaternion q = new Quaternion(forward, deltaDegree);
            _pose.Quaternion = q * _pose.Quaternion;
        }

        public virtual void Pitch(float deltaDegree)
        {
            Vertex3f right = _pose.Rotation.Column0.Vertex3f();
            Quaternion q = new Quaternion(right, deltaDegree);
            _pose.Quaternion = q * _pose.Quaternion;
        }

    }
}
