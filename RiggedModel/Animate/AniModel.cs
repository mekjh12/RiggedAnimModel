using Assimp;
using OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace LSystem.Animate
{
    class AniModel
    {
        Dictionary<string, Entity> _models;
        Bone _rootBone;
        Animator _animator;
        XmlDae _xmlDae;
        Transform _transform;
        string _name;

        Action _updateAfter;


        public string Name => _name;

        public Transform Transform => _transform;

        /// <summary>
        /// Rendering Part 
        /// </summary>
        public enum RenderingMode { Animation, BoneWeight, Static, None, Count };
        PolygonMode _polygonMode = PolygonMode.Fill;
        RenderingMode _renderingMode = RenderingMode.Animation;
        int _boneIndex = 0;
        float _axisLength = 10.3f;
        float _drawThick = 1.0f;

        public int BoneCount => _xmlDae.BoneCount;

        public Motion CurrentMotion => _animator.CurrentMotion;

        public int SelectedBoneIndex
        {
            get => _boneIndex;
            set => _boneIndex = value;
        }

        public PolygonMode PolygonMode
        {
            get => _polygonMode;
            set => _polygonMode = value;
        }

        public RenderingMode RenderMode
        {
            get => _renderingMode;
            set => _renderingMode = value;
        }

        public void PopPolygonMode()
        {
            _polygonMode++;
            if (_polygonMode >= (PolygonMode)6915) _polygonMode = (PolygonMode)6912;
        }

        public void PopRenderingMode()
        {
            _renderingMode++;
            if (_renderingMode == RenderingMode.Count - 1) _renderingMode = 0;
        }

        public void LootAtEye(Vertex3f worldPosition)
        {
            _updateAfter = () =>
            {
                GetBoneByName("eyeLeft")?.ApplyFrameMatrix(_transform.Matrix4x4f.Position, worldPosition, Vertex3f.UnitZ);
                GetBoneByName("eyeRight")?.ApplyFrameMatrix(_transform.Matrix4x4f.Position, worldPosition, Vertex3f.UnitZ);
            };
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
            //_boneCount = xmlDae.BoneCount;
            _animator = new Animator(this);
            _transform = new Transform();
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

            if (_updateAfter != null) 
            {
                _updateAfter();
            }
        }

        public void Render(Camera camera, StaticShader staticShader, AnimateShader ashader, BoneWeightShader boneWeightShader, bool isSkinVisible, bool isBoneVisible)
        {
            Matrix4x4f[] jointMatrix = JointTransformMatrix;

            foreach (KeyValuePair<string, Entity> item in _models)
            {
                Gl.PolygonMode(MaterialFace.FrontAndBack, _polygonMode);

                Entity mainEntity = item.Value;
                Matrix4x4f modelMatrix = mainEntity.ModelMatrix;
                
                if (isSkinVisible) // 스킨
                {
                    Gl.Disable(EnableCap.CullFace);
                    if (_renderingMode == RenderingMode.Animation)
                    {
                        Renderer.Render(ashader, _transform.Matrix4x4f, jointMatrix, mainEntity, camera);
                    }
                    else if (_renderingMode == RenderingMode.BoneWeight)
                    {
                        Renderer.Render(boneWeightShader, _boneIndex, mainEntity, camera);
                    }
                    else if (_renderingMode == RenderingMode.Static)
                    {
                        Renderer.Render(staticShader, mainEntity, camera);
                    }
                    Gl.Enable(EnableCap.CullFace);
                }

                // 애니메이션 뼈대 렌더링
                if (isBoneVisible)
                {
                    foreach (Matrix4x4f jointTransform in BoneAnimationTransforms)
                    {
                        Renderer.RenderLocalAxis(staticShader, camera, size: jointTransform.Column3.Vertex3f().Norm() * _axisLength,
                            thick: _drawThick, _transform.Matrix4x4f * modelMatrix * jointTransform);
                    }
                }

                Renderer.RenderLocalAxis(staticShader, camera, size: 100.0f, thick: _drawThick, _rootBone.AnimatedTransform * _transform.Matrix4x4f);

                /*
                // 정지 뼈대
                if (this.ckBoneBindPose.Checked)
                {
                    foreach (Matrix4x4f jointTransform in _aniModel.InverseBindPoseTransforms)
                    {
                        Renderer.RenderLocalAxis(_shader, camera, size: _axisLength, thick: _drawThick,
                            entityModel * jointTransform.Inverse);
                    }
                }

                // 부모와 현재 뼈대 렌더링
                if (this.ckBoneParentCurrentVisible.Checked) 
                {
                    Bone cBone = GetBoneByName(this.cbBone.Text);
                    Bone pBone = cBone.Parent;
                    if (cBone != null)
                    {
                        Renderer.RenderLocalAxis(_shader, camera, size: cBone.AnimatedTransform.Column3.Vertex3f().Norm() * _axisLength, thick: _drawThick,
                             entityModel * cBone.AnimatedTransform);
                    }
                    if (pBone != null)
                    {
                        Renderer.RenderLocalAxis(_shader, camera, size: pBone.AnimatedTransform.Column3.Vertex3f().Norm() * _axisLength, thick: _drawThick,
                             entityModel * pBone.AnimatedTransform);
                    }
                }

                
                */
            }
        }

        /// <summary>
        /// * bone.AnimatedTransform * bone.InverseBindTransform<br/>
        /// * 캐릭터 공간에서의 애니메이션을 포즈행렬을 최종적으로 가져온다.<br/>
        /// * v' = Ma(i) Md^-1(i) v (Ma 애니메이션행렬, Md 바이딩포즈행렬)<br/>
        /// * 정점들을 바인딩포즈행렬을 이용하여 뼈 공간으로 정점을 변환 후, 애니메이션 행렬을 이용하여 뼈의 캐릭터 공간으로의 변환행렬을 가져온다.<br/>
        /// </summary>
        private Matrix4x4f[] JointTransformMatrix
        {
            get
            {
                Matrix4x4f[] jointMatrices = new Matrix4x4f[BoneCount];
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
                Matrix4x4f[] jointMatrices = new Matrix4x4f[BoneCount];
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
                Matrix4x4f[] jointMatrices = new Matrix4x4f[BoneCount];
                foreach (KeyValuePair<string, Bone> item in _xmlDae.DicBones)
                {
                    Bone bone = item.Value;
                    if (bone.Index >=0 && bone.Index < BoneCount)
                        jointMatrices[bone.Index] = bone.InverseBindTransform;
                }
                return jointMatrices;
            }
        }

        private void AddEntity(string name, Entity entity)
        {
            if (_models.ContainsKey(name))
            {
                _models.Remove(name);
                _models.Add(name, entity);
            }
            else
            {
                _models.Add(name, entity);
            }
        }

        public Entity Attach(string fileName, float expandValue = 0.01f)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            List<TexturedModel> texturedModels = _xmlDae.WearCloth(fileName, expandValue);
            Entity clothEntity = new Entity("aniModel_" + name, texturedModels[0]);
            AddEntity(name, clothEntity);
            return clothEntity;
        }

        public void Transplant(string fileName, string parentBoneName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);

            string boneName = name + "Left";
            Bone LEyeBone = _xmlDae.AddBone(boneName, _xmlDae.BoneCount, parentBoneName,
                Matrix4x4f.Translated(4.0f, 11.8f, 11.70002f), 
                Matrix4x4f.Translated(4.0f, 11.8f, 11.70002f));
            Entity EntityL = new Entity(boneName, _xmlDae.LoadFileOnly(fileName, LEyeBone.Index)[0]);
            AddEntity(boneName, EntityL);

            boneName = name + "Right";
            Bone REyeBone = _xmlDae.AddBone(boneName, _xmlDae.BoneCount, parentBoneName, 
                Matrix4x4f.Translated(-4.0f, 11.8f, 11.70002f), 
                Matrix4x4f.Translated(-4.0f, 11.8f, 11.70002f));
            Entity EntityR = new Entity(boneName, _xmlDae.LoadFileOnly(fileName, REyeBone.Index, mirror: 0b_100)[0]);
            AddEntity(boneName, EntityR);
        }

    }
}
