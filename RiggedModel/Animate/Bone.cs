﻿using OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LSystem.Animate
{
    /// <summary>
    /// Bone과 동일하게 뼈로서 부모와 자식을 연결하여 Armature를 구성하는 요소이다.
    /// </summary>
    public class Bone
    {
        int _index;
        string _name;
        List<Bone> _children = new List<Bone>();
        Bone _parent;

        Matrix4x4f _localTransform = Matrix4x4f.Identity;
        Matrix4x4f _animatedTransform = Matrix4x4f.Identity;
        Matrix4x4f _bindTransform = Matrix4x4f.Identity;
        Matrix4x4f _inverseBindTransform = Matrix4x4f.Identity;

        BoneAngle _restrictAngle;

        
        public BoneAngle RestrictAngle
        {
            get => _restrictAngle;
            set => _restrictAngle = value;
        }

        public bool IsLeaf => _children.Count == 0;

        /// <summary>
        /// 현재 뼈에 캐릭터 공간 회전행렬을 적용하여 부모뻐로부터의 뒤틀림(twist) 회전각을 가져온다.
        /// </summary>
        /// <param name="rotAMat"></param>
        /// <returns></returns>
        public float TwistAngle(Matrix4x4f rotAMat)
        {
            Matrix4x4f BrAMat = _animatedTransform;
            BrAMat = _parent.AnimatedInverseTransform * rotAMat * BrAMat;
            Vertex3f nX = BrAMat.Column0.Vertex3f().Normalized;
            float theta = (float)Math.Acos(Vertex3f.UnitX.Dot(nX)) * 180.0f / 3.141502f;
            theta = nX.z < 0 ? theta : -theta;
            return theta;
        }

        /// <summary>
        ///  캐릭터 공간의 뼈의 끝부분 위치를 가져온다. 만약, 자식이 없으면 15의 값으로 지정된다.
        /// </summary>
        public Vertex3f EndPosition
        {
            get
            {
                if (_children.Count == 0)
                {
                    Matrix4x4f a = _animatedTransform * Matrix4x4f.Translated(0, 15, 0);
                    return a.Column3.Vertex3f();
                }
                else
                {
                    Vertex3f g = Vertex3f.Zero;
                    foreach (Bone bone in _children)
                    {
                        g += bone._animatedTransform.Column3.Vertex3f();
                    }
                    return g * (1.0f/ _children.Count);
                }
            }
        }

        public Bone Parent
        {
            get => _parent; 
            set => _parent = value;
        }

        public bool IsArmature => (_parent == null);

        public bool IsRootArmature => (_parent.Parent == null);
        
        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// 캐릭터 공간의 PivotPosition
        /// </summary>
        public Vertex3f PivotPosition
        {
            get => _animatedTransform.Column3.Vertex3f();
            set
            {
                _animatedTransform[3, 0] = value.x;
                _animatedTransform[3, 1] = value.y;
                _animatedTransform[3, 2] = value.z;
            }
        }

        public List<Bone> Childrens => _children;

        /// <summary>
        /// 캐릭터공간에서의 변환 행렬
        /// </summary>
        public Matrix4x4f AnimatedTransform
        {
            get => _animatedTransform;
            set => _animatedTransform = value;
        }

        public Matrix4x4f AnimatedInverseTransform => (_animatedTransform * 100.0f).Inverse * 100.0f;

        /// <summary>
        /// 뼈공간에서의 변환행렬
        /// </summary>
        public Matrix4x4f LocalTransform
        {
            get => _localTransform;
            set => _localTransform = value;
        }

        /// <summary>
        /// 바인딩포즈의 뼈공간의 역행렬
        /// </summary>
        public Matrix4x4f InverseBindTransform
        {
            get => _inverseBindTransform;
            set => _inverseBindTransform = value;
        }

        /// <summary>
        /// 바인딩 포즈의 변환행렬
        /// </summary>
        public Matrix4x4f BindTransform
        {
            get => _bindTransform;
            set => _bindTransform = value;
        }

        public Bone(string name, int index, Matrix4x4f bindTransform)
        {
            _name = name;
            _index = index;
            _bindTransform = bindTransform;
            _restrictAngle = new BoneAngle(-180, 180, -180, 180, -90, 90);
        }

        public void AddChild(Bone child)
        {
            _children.Add(child);
        }

        /// <summary>
        /// 회전행렬을 이용하여 뼈의 중심점을 기준으로 회전하여 캐릭터 변환한다.
        /// </summary>
        /// <param name="mat">캐릭터 공간에서의 변환행렬</param>
        /// <param name="endPosition">캐릭터 공간에서의 뼈의 끝점</param>
        /// <returns>캐릭터 공간에서의 뼈의 끝점이 회전된 끝점을 반환한다.</returns>
        public Vertex3f RotateBy(Matrix4x4f rotAMat, Vertex3f endPosition)
        {
            Vertex3f P = PivotPosition;
            Vertex3f e = endPosition - P;

            // 캐릭터 변환을 업데이트한다.
            Matrix4x4f Br = _animatedTransform;
            Br[3, 0] = Br[3, 1] = Br[3, 2] = 0.0f;
            Br = rotAMat * Br;
            Br[3, 0] = P.x;
            Br[3, 1] = P.y;
            Br[3, 2] = P.z;
            _animatedTransform = Br;

            // 회전한 끝점을 반환한다.
            return (rotAMat * e.Vertex4f()).Vertex3f() + P;
        }

        /// <summary>
        /// 캐릭터 변환행렬로부터 로컬행렬을 계산하여 업데이트한다.
        /// </summary>
        public void UpdateLocalTransform()
        {
            if (_parent != null)
                _localTransform = (_parent._animatedTransform * 100.0f).Inverse * 100.0f * _animatedTransform;
            else
                _localTransform = _animatedTransform;
        }


        public void ModifyPitch(float phi)
        {
            if (phi < _restrictAngle.ConstraintAngle.x )
            {
                Matrix4x4f localMat = _localTransform;
                Vertex3f lowerYAxis = localMat.Column1.Vertex3f();
                lowerYAxis.y = 0;
                lowerYAxis = lowerYAxis.Normalized * -Math.Sin(_restrictAngle.ConstraintAngle.x.ToRadian());
                lowerYAxis.y = (float)Math.Cos(_restrictAngle.ConstraintAngle.x.ToRadian());
                Matrix4x4f localRotMat = localMat.Column1.Vertex3f().RotateBetween(lowerYAxis);
                _localTransform = localMat * localRotMat;
                UpdateChildBone(isSelfIncluded: true);
            }            
            else if (phi > _restrictAngle.ConstraintAngle.y)
            {
                Matrix4x4f localMat = _localTransform;
                Vertex3f lowerYAxis = localMat.Column1.Vertex3f();
                lowerYAxis.y = 0;
                lowerYAxis = lowerYAxis.Normalized * Math.Sin(_restrictAngle.ConstraintAngle.y.ToRadian());
                lowerYAxis.y = (float)Math.Cos(_restrictAngle.ConstraintAngle.y.ToRadian());
                Matrix4x4f localRotMat = localMat.Column1.Vertex3f().RotateBetween(lowerYAxis);
                _localTransform = localMat * localRotMat;
                UpdateChildBone(isSelfIncluded: true);
            }
        }

        public void ModifyYaw(float phi)
        {
            if (phi < _restrictAngle.ConstraintAngle.z)
            {
                Matrix4x4f localMat = _localTransform;
                Vertex3f lowerZAxis = localMat.Column2.Vertex3f();
                lowerZAxis.z = 0;
                lowerZAxis = lowerZAxis.Normalized * -Math.Sin(_restrictAngle.ConstraintAngle.z.ToRadian());
                lowerZAxis.y = (float)Math.Cos(_restrictAngle.ConstraintAngle.z.ToRadian());
                Matrix4x4f localRotMat = localMat.Column2.Vertex3f().RotateBetween(lowerZAxis);
                _localTransform = localMat * localRotMat;
                UpdateChildBone(isSelfIncluded: true);
            }
            else if (phi > _restrictAngle.ConstraintAngle.w)
            {
                Matrix4x4f localMat = _localTransform;
                Vertex3f lowerZAxis = localMat.Column2.Vertex3f();
                lowerZAxis.z = 0;
                lowerZAxis = lowerZAxis.Normalized * Math.Sin(_restrictAngle.ConstraintAngle.w.ToRadian());
                lowerZAxis.z = (float)Math.Cos(_restrictAngle.ConstraintAngle.w.ToRadian());
                Matrix4x4f localRotMat = localMat.Column2.Vertex3f().RotateBetween(lowerZAxis);
                _localTransform = localMat * localRotMat;
                UpdateChildBone(isSelfIncluded: true);
            }
        }

        public void ModifyRoll(float phi)
        {
            if (phi < _restrictAngle.TwistAngle.x)
            {
                Matrix4x4f localMat = _localTransform;
                Vertex3f lowerZAxis = localMat.Column2.Vertex3f();
                lowerZAxis.z = 0;
                lowerZAxis = lowerZAxis.Normalized * -Math.Sin(_restrictAngle.ConstraintAngle.z.ToRadian());
                lowerZAxis.y = (float)Math.Cos(_restrictAngle.ConstraintAngle.z.ToRadian());
                Matrix4x4f localRotMat = localMat.Column2.Vertex3f().RotateBetween(lowerZAxis);
                _localTransform = localMat * localRotMat;
                UpdateChildBone(isSelfIncluded: true);
            }
            else if (phi > _restrictAngle.TwistAngle.y)
            {
                Matrix4x4f localMat = _localTransform;
                Vertex3f lowerZAxis = localMat.Column2.Vertex3f();
                lowerZAxis.z = 0;
                lowerZAxis = lowerZAxis.Normalized * Math.Sin(_restrictAngle.ConstraintAngle.w.ToRadian());
                lowerZAxis.z = (float)Math.Cos(_restrictAngle.ConstraintAngle.w.ToRadian());
                Matrix4x4f localRotMat = localMat.Column2.Vertex3f().RotateBetween(lowerZAxis);
                _localTransform = localMat * localRotMat;
                UpdateChildBone(isSelfIncluded: true);
            }
        }
        /// <summary>
        /// 로컬 행렬로부터 애니메이션 행렬을 계산한다.
        /// </summary>
        /// <param name="isSelfIncluded"></param>
        /// <param name="exceptBone"></param>
        public void UpdateChildBone(bool isSelfIncluded = false, Bone exceptBone = null)
        {
            Stack<Bone> stack = new Stack<Bone>();

            if (isSelfIncluded) // 자신도 업데이트할지?
                stack.Push(this);
            else
                foreach (Bone childJoint in _children) stack.Push(childJoint);

            while (stack.Count > 0)
            {
                Bone b = stack.Pop(); // 행렬곱을 누적하기 위하여, 순서는 자식부터  v' = ... P2 P1 L v
                if (b == exceptBone) continue;
                b.AnimatedTransform = b.Parent == null ? b.LocalTransform : b.Parent.AnimatedTransform * b.LocalTransform;
                foreach (Bone childJoint in b.Childrens) stack.Push(childJoint);
            }
        }

        public override string ToString()
        {
            string txt = "";
            Matrix4x4f m = _bindTransform;
            for (uint i = 0; i < 4; i++)
            {
                txt += $"{Cut(m[0, i])} {Cut(m[1, i])} {Cut(m[2, i])} {Cut(m[3, i])}" 
                    + ((i < 3) ? " / ": "");
            }
            return $"[{_index},{_name}] BindMatrix {txt}";

            float Cut(float a) => ((float)Math.Abs(a) < 0.000001f) ? 0.0f : a;
        }
    }
}
