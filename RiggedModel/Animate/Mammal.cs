using OpenGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace LSystem.Animate
{
    class Mammal : AniModel
    {
        public enum BODY_PART
        {
            LeftHand, RightHand, Head, Count
        }

        Entity _leftHandEntity;
        Entity _rightHandEntity;
        Entity _headEntity;

        public Entity LeftHandEntity => _leftHandEntity;

        public Mammal(string name, Entity model, XmlDae xmlDae) : base(name, model, xmlDae)
        {
            TransplantEye(EngineLoop.PROJECT_PATH + "\\Res\\Human\\simple_eye.dae", "mixamorig_Head");

            HandGrabItem(_xmlDae, "mixamorig_LeftHand_Item", "mixamorig_LeftHand",
                Matrix4x4f.RotatedY(0), Matrix4x4f.Translated(0, 10, 3) * Matrix4x4f.Scaled(1, 1, 1));
            HandGrabItem(_xmlDae, "mixamorig_RightHand_Item", "mixamorig_RightHand", 
                Matrix4x4f.RotatedY(180), Matrix4x4f.Translated(0, 10, 3) * Matrix4x4f.Scaled(1, 1, 1));

            HandGrabItem(_xmlDae, "mixamorig_Head_Item", "mixamorig_Head",
                Matrix4x4f.RotatedY(0), Matrix4x4f.Translated(0, 18.5f, 7.2f) * Matrix4x4f.Scaled(1, 1, 1));
        }

        public void RemoveItem(BODY_PART hand)
        {
            if (hand == BODY_PART.LeftHand)
            {
                if (_leftHandEntity != null)
                    Remove(_leftHandEntity.Name);
            }

            if (hand == BODY_PART.RightHand)
            {
                if (_rightHandEntity != null)
                    Remove(_rightHandEntity.Name);
            }

            if (hand == BODY_PART.Head)
            {
                if (_headEntity != null)
                    Remove(_headEntity.Name);
            }
        }

        public void Attach(BODY_PART hand, Entity entity)
        {
            entity.IsOnlyOneJointWeight = true;

            if (hand == BODY_PART.LeftHand)
            {
                entity.BoneIndexOnlyOneJoint = GetBoneByName("mixamorig_LeftHand_Item").Index;
                AddEntity(entity.Name, entity);
                _leftHandEntity = entity;
            }

            if (hand == BODY_PART.RightHand)
            {
                entity.BoneIndexOnlyOneJoint = GetBoneByName("mixamorig_RightHand_Item").Index;
                AddEntity(entity.Name, entity);
                _rightHandEntity = entity;
            }

            if (hand == BODY_PART.Head)
            {
                entity.BoneIndexOnlyOneJoint = GetBoneByName("mixamorig_Head_Item").Index;
                AddEntity(entity.Name, entity);
                _headEntity = entity;
            }
        }

        public void LootAtEye(Vertex3f worldPosition)
        {
            _updateAfter += () =>
            {
                GetBoneByName("mixamorig_eyeLeft")?.ApplyCoordinateFrame(_transform.Matrix4x4f.Position, worldPosition, Vertex3f.UnitZ);
                GetBoneByName("mixamorig_eyeRight")?.ApplyCoordinateFrame(_transform.Matrix4x4f.Position, worldPosition, Vertex3f.UnitZ);
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        public void FoldHand(BODY_PART dir)
        {
            _updateAfter += () =>
            {
                Bone hand = GetBoneByName("mixamorig_" + (dir == BODY_PART.LeftHand ? "LeftHand" : "RightHand"));
                Stack<Bone> stack = new Stack<Bone>();
                stack.Push(hand);
                while (stack.Count > 0)
                {
                    Bone bone = stack.Pop();
                    if (bone.Name.IndexOf("Thumb") < 0)
                    {
                        bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedX(60);
                    }
                    else
                    {
                        if (bone.Name.IndexOf("Thumb1") >= 0)
                            bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedY(60);
                        if (bone.Name.IndexOf("Thumb2") >= 0)
                            bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedX(-0);
                        if (bone.Name.IndexOf("Thumb3") >= 0)
                            bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedX(-0);
                    }
                    foreach (Bone item in bone.Childrens) stack.Push(item);
                }
                hand.UpdateChildBone(isSelfIncluded: false);
            };
        }

        public void UnfoldHand(BODY_PART dir)
        {
            _updateAfter += () =>
            {
                Bone hand = GetBoneByName("mixamorig_" + (dir == BODY_PART.LeftHand ? "LeftHand" : "RightHand"));
                Stack<Bone> stack = new Stack<Bone>();
                stack.Push(hand);
                while (stack.Count > 0)
                {
                    Bone bone = stack.Pop();
                    if (bone.Name.IndexOf("Thumb") < 0)
                    {
                        bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedX(0);
                    }
                    else
                    {
                        if (bone.Name.IndexOf("Thumb1") >= 0)
                            bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedY(0);
                        if (bone.Name.IndexOf("Thumb2") >= 0)
                            bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedX(0);
                        if (bone.Name.IndexOf("Thumb3") >= 0)
                            bone.LocalTransform = bone.LocalBindTransform * Matrix4x4f.RotatedX(0);
                    }
                    foreach (Bone item in bone.Childrens) stack.Push(item);
                }
                hand.UpdateChildBone(isSelfIncluded: false);
            };
        }

        /// <summary>
        /// 눈을 이식한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="parentBoneName"></param>
        private void TransplantEye(string fileName, string parentBoneName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"{fileName}이 없어 눈 이식에 실패하였습니다.");
                return;
            }

            TexturedModel texturedModel = XmlLoader.LoadOnlyGeometryMesh(fileName);
            string boneName = $"mixamorig_eyeLeft";
            Bone LEyeBone = _xmlDae.AddBone(boneName, _xmlDae.BoneCount, parentBoneName,
                inverseBindTransform: Matrix4x4f.RotatedY(90).Inverse,
                localBindTransform: Matrix4x4f.Translated(4.4f, 11.8f, 12.5f) * Matrix4x4f.Scaled(0.75f, 0.65f, 0.65f));
            LEyeBone.RestrictAngle = new BoneAngle(-30, 30, -0, 0, -60, 60);
            Entity EntityL = new Entity(boneName, texturedModel);
            EntityL.IsOnlyOneJointWeight = true;
            EntityL.BoneIndexOnlyOneJoint = LEyeBone.Index;
            AddEntity(boneName, EntityL);

            boneName = $"mixamorig_eyeRight";
            Bone REyeBone = _xmlDae.AddBone(boneName, _xmlDae.BoneCount, parentBoneName,
                inverseBindTransform: Matrix4x4f.RotatedY(90).Inverse,
                localBindTransform: Matrix4x4f.Translated(-4.4f, 11.8f, 12.5f) * Matrix4x4f.Scaled(0.75f, 0.65f, 0.65f));
            REyeBone.RestrictAngle = new BoneAngle(-30, 30, -0, 0, -60, 60);
            Entity EntityR = new Entity(boneName, texturedModel);
            EntityR.IsOnlyOneJointWeight = true;
            EntityR.BoneIndexOnlyOneJoint = REyeBone.Index;
            AddEntity(boneName, EntityR);
        }

        /// <summary>
        /// 부모뼈로부터 자식뼈를 생성하고 생성한 뼈의 변환을 지정한다.
        /// </summary>
        /// <param name="xmlDae"></param>
        /// <param name="boneName"></param>
        /// <param name="parentBoneName"></param>
        /// <param name="bindTransform"> 캐릭터 공간의 invBind를 위하여 역행렬이 아닌 바인딩행렬을 지정한다.</param>
        /// <param name="localBindTransform">부모뼈공간에서의 바인딩 행렬을 지정한다.</param>
        /// <returns></returns>
        private Bone HandGrabItem(XmlDae xmlDae, string boneName, string parentBoneName, Matrix4x4f bindTransform, Matrix4x4f localBindTransform)
        {
            Bone bone = xmlDae.AddBone(boneName, xmlDae.BoneCount, parentBoneName,
                inverseBindTransform: bindTransform.Inverse,
                localBindTransform: localBindTransform);
            bone.RestrictAngle = new BoneAngle(-0, 0, -0, 0, -0, 0);
            return bone;
        }

    }
}
