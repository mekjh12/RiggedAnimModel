using OpenGL;
using System.Collections.Generic;
using System.IO;

namespace LSystem.Animate
{
    class AniModel
    {
        Dictionary<string, Entity> _models;
        Bone _rootBone;
        int _jointCount;
        Animator _animator;
        XmlDae _xmlDae;

        public Entity Wear(string fileName, float expandValue = 0.00005f)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            List<TexturedModel> texturedModels = _xmlDae.WearCloth(fileName, expandValue);

            Entity clothEntity = new Entity("aniModel_" + name, texturedModels[0]);
            clothEntity.Material = new Material();
            clothEntity.Position = Vertex3f.Zero;
            clothEntity.IsAxisVisible = true;

            if (_models.ContainsKey(name))
            {
                _models.Remove(name);
                _models.Add(name, clothEntity);
            }
            else
            {
                _models.Add(name, clothEntity);
            }

            return clothEntity;
        }

        public Entity Transplant(string fileName, string parentBoneName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);

            Bone parentBone = GetBoneByName(parentBoneName);
            Bone curBone = new Bone($"bone_{name}", _xmlDae.BoneCount);
            curBone.LocalTransform = Matrix4x4f.Translated(1, 0, -0.5f);
            curBone.BindTransform = Matrix4x4f.Translated(1, 0, -0.5f);
            curBone.InverseBindTransform = curBone.BindTransform.Inverse;
            parentBone.AddChild(curBone);
            _jointCount++;

            List<TexturedModel> texturedModels = _xmlDae.Load(fileName, boneIndex: curBone.Index);            


            return null;
        }

        /// <summary>
        /// 본이름으로부터 본을 가져온다.
        /// </summary>
        /// <param name="boneName"></param>
        /// <returns></returns>
        public Bone GetBoneByName(string boneName) => _xmlDae.GetBoneByName(boneName);

        /// <summary>
        /// Animator를 가져온다.
        /// </summary>
        public Animator Animator => _animator;

        /// <summary>
        /// * 최상위본을 위한 바이딩 행렬<br/>
        /// - 최상위뼈도 캐릭터 공간에서의 바인딩 행렬이 필요하다.<br/>
        /// - BindShapeMatrix -> _rootBoneTransform -> ... -> ...<br/>
        /// </summary>
        public Matrix4x4f PoseRootMatrix => _xmlDae.RootMatirix * _xmlDae.BindShapeMatrix;

        /// <summary>
        /// 모션의 총 시간 길이를 가져온다.
        /// </summary>
        public float MotionTime => _animator.MotionTime;

        /// <summary>
        /// Entity들을 모두 가져온다.
        /// </summary>
        public Dictionary<string, Entity> Entities => _models;

        /// <summary>
        /// 루트본을 가져온다.
        /// </summary>
        public Bone RootBone => _rootBone;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xmlDae"></param>
        public AniModel(string name, Entity model, XmlDae xmlDae)
        {
            _models = new Dictionary<string, Entity>();
            _models.Add(name, model);

            _xmlDae = xmlDae;
            _rootBone = xmlDae.RootBone;
            _jointCount = xmlDae.BoneCount;
            _animator = new Animator(this);
        }

        /// <summary>
        /// 이름으로 Entity를 가져온다.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Entity GetEntity(string name) => _models[name];

        /// <summary>
        /// 모션을 설정한다.
        /// </summary>
        /// <param name="motionName"></param>
        public void SetMotion(string motionName)
        {
            Motion motion = _xmlDae.GetAnimation(motionName);
            if (motion == null) motion = _xmlDae.DefaultMotion;
            _animator.SetMotion(motion);
        }

        /// <summary>
        /// 업데이트를 통하여 애니메이션 행렬을 업데이트한다.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(int deltaTime)
        {
            _animator.Update(0.001f * deltaTime);
        }

        /// <summary>
        /// * bone.AnimatedTransform * bone.InverseBindTransform<br/>
        /// * 캐릭터 공간에서의 애니메이션을 포즈행렬을 최종적으로 가져온다.<br/>
        /// * v' = Ma(i) Md^-1(i) v (Ma 애니메이션행렬, Md 바이딩포즈행렬)<br/>
        /// * 정점들을 바인딩포즈행렬을 이용하여 뼈 공간으로 정점을 변환 후, 애니메이션 행렬을 이용하여 뼈의 캐릭터 공간으로의 변환행렬을 가져온다.<br/>
        /// </summary>
        public Matrix4x4f[] JointTransformMatrix
        {
            get
            {
                Matrix4x4f[] jointMatrices = new Matrix4x4f[_jointCount];
                foreach (KeyValuePair<string, Bone> item in _xmlDae.DicBones)
                {
                    Bone bone = item.Value;
                    if (bone.Index >= 0)
                        jointMatrices[bone.Index] = bone.AnimatedTransform * bone.InverseBindTransform;
                }
                return jointMatrices;
            }
        }

        /// <summary>
        /// * 애니매이션에서 뼈들의 뼈공간 ==> 캐릭터 공간으로의 변환 행렬<br/>
        /// * 뼈들의 포즈를 렌더링하기 위하여 사용할 수 있다.<br/>
        /// </summary>
        public Matrix4x4f[] BoneAnimationTransforms
        {
            get
            {
                Matrix4x4f[] jointMatrices = new Matrix4x4f[_jointCount];
                foreach (KeyValuePair<string, Bone> item in _xmlDae.DicBones)
                {
                    Bone bone = item.Value;
                    if (bone.Index >= 0)
                        jointMatrices[bone.Index] = bone.AnimatedTransform;
                }
                return jointMatrices;
            }
        }

        /// <summary>
        /// * 초기의 캐릭터공간에서의 바인드 포즈행렬을 가져온다. <br/>
        /// - 포즈행렬이란 한 뼈공간에서의 점의 상대적 좌표를 가져오는 행렬이다.<br/>
        /// </summary>
        public Matrix4x4f[] InverseBindPoseTransforms
        {
            get
            { 
                Matrix4x4f[] jointMatrices = new Matrix4x4f[_jointCount];
                foreach (KeyValuePair<string, Bone> item in _xmlDae.DicBones)
                {
                    Bone bone = item.Value;
                    jointMatrices[bone.Index] = bone.InverseBindTransform;
                }
                return jointMatrices;
            }
        }
    }
}
