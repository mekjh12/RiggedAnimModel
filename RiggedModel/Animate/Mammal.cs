using Assimp;
using OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Assimp.Metadata;

namespace LSystem.Animate
{
    class Mammal : AniModel
    {
        public enum Hand
        {
            LeftHand = 1, 
            RightHand = 2,
            Head = 3,
        }


        public Mammal(string name, Entity model, XmlDae xmlDae) : base(name, model, xmlDae)
        {
            TransplantEye(EngineLoop.PROJECT_PATH + "\\Res\\Human\\simple_eye.dae", "mixamorig_Head");

            HandGrabItem(_xmlDae, "mixamorig_LeftHand_Item", "mixamorig_LeftHand",
                Matrix4x4f.RotatedY(-90), Matrix4x4f.Translated(0, 10, 3) * Matrix4x4f.Scaled(15, 2, 2));
            HandGrabItem(_xmlDae, "mixamorig_RightHand_Item", "mixamorig_RightHand", 
                Matrix4x4f.RotatedY(90), Matrix4x4f.Translated(0, 10, 3) * Matrix4x4f.Scaled(15, 2, 2));

            HandGrabItem(_xmlDae, "mixamorig_Head_Item", "mixamorig_Head",
                Matrix4x4f.RotatedY(0), Matrix4x4f.Translated(0, 18.5f, 7.2f) * Matrix4x4f.Scaled(135, 135, 135));
        }

        public void RemoveHandGrab(Hand hand)
        {
            if (hand == Hand.LeftHand)
            {
                Remove("mixamorig_LeftHand_Item");
            }

            if (hand == Hand.RightHand)
            {
                Remove("mixamorig_RightHand_Item");
            }

            if (hand == Hand.Head)
            {
                Remove("mixamorig_Head_Item");
            }

        }

        public void HandGrab(Hand hand, Entity entity)
        {
            entity.IsOnlyOneJointWeight = true;

            if (hand == Hand.LeftHand)
            {
                entity.BoneIndexOnlyOneJoint = GetBoneByName("mixamorig_LeftHand_Item").Index;
                AddEntity("mixamorig_LeftHand_Item", entity);
            }

            if (hand == Hand.RightHand)
            {
                entity.BoneIndexOnlyOneJoint = GetBoneByName("mixamorig_RightHand_Item").Index;
                AddEntity("mixamorig_RightHand_Item", entity);
            }

            if (hand == Hand.Head)
            {
                entity.BoneIndexOnlyOneJoint = GetBoneByName("mixamorig_Head_Item").Index;
                AddEntity("mixamorig_Head_Item", entity);
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
        public void FoldHand(Hand dir)
        {
            _updateAfter += () =>
            {
                Bone hand = GetBoneByName("mixamorig_" + (dir == Hand.LeftHand ? "LeftHand" : "RightHand"));
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

        public void UnfoldHand(Hand dir)
        {
            _updateAfter += () =>
            {
                Bone hand = GetBoneByName("mixamorig_" + (dir == Hand.LeftHand ? "LeftHand" : "RightHand"));
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
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="parentBoneName"></param>
        private void TransplantEye(string fileName, string parentBoneName)
        {
            TexturedModel texturedModel = _xmlDae.LoadFileOnly(fileName);
            string boneName = $"mixamorig_eyeLeft";
            Bone LEyeBone = _xmlDae.AddBone(boneName, _xmlDae.BoneCount, parentBoneName,
                inverseBindTransform: Matrix4x4f.RotatedY(90).Inverse,
                localBindTransform: Matrix4x4f.Translated(4.2f, 11.8f, 13.5f) * Matrix4x4f.Scaled(0.5f, 0.5f, 0.5f));
            LEyeBone.RestrictAngle = new BoneAngle(-30, 30, -0, 0, -60, 60);
            Entity EntityL = new Entity(boneName, texturedModel);
            EntityL.IsOnlyOneJointWeight = true;
            EntityL.BoneIndexOnlyOneJoint = LEyeBone.Index;
            AddEntity(boneName, EntityL);

            boneName = $"mixamorig_eyeRight";
            Bone REyeBone = _xmlDae.AddBone(boneName, _xmlDae.BoneCount, parentBoneName,
                inverseBindTransform: Matrix4x4f.RotatedY(90).Inverse,
                localBindTransform: Matrix4x4f.Translated(-4.2f, 11.8f, 13.5f) * Matrix4x4f.Scaled(0.5f, 0.5f, 0.5f));
            REyeBone.RestrictAngle = new BoneAngle(-30, 30, -0, 0, -60, 60);
            Entity EntityR = new Entity(boneName, texturedModel);
            EntityR.IsOnlyOneJointWeight = true;
            EntityR.BoneIndexOnlyOneJoint = REyeBone.Index;
            AddEntity(boneName, EntityR);
        }

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
