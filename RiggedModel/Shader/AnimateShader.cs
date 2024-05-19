﻿using OpenGL;
using System.Windows.Media.Converters;

namespace LSystem
{
    public class AnimateShader : ShaderProgram
    {
        private static int MAX_JOINTS = 128;
        const string VERTEX_FILE = @"\Shader\ani.vert";
        const string FRAGMENT_FILE = @"\Shader\ani.frag";
        //const string GEOMETRY_FILE = @"\Shader\ani.geom";

        public AnimateShader() : base(EngineLoop.PROJECT_PATH + VERTEX_FILE,
            EngineLoop.PROJECT_PATH + FRAGMENT_FILE, 
            "") //EngineLoop.PROJECT_PATH + GEOMETRY_FILE
        {

        }

        protected override void BindAttributes()
        {
            base.BindAttribute(0, "in_position");
            base.BindAttribute(1, "in_textureCoords");
            base.BindAttribute(2, "in_normal");
            base.BindAttribute(4, "in_jointIndices");
            base.BindAttribute(5, "in_weights");
        }

        protected override void GetAllUniformLocations()
        {
            UniformLocations("model", "view", "proj", "bind", "pmodel");
            UniformLocations("lightDirection");
            UniformLocations("diffuseMap");
            UniformLocations("isOnlyOneJointWeight", "jointIndex");

            for (int i = 0; i < MAX_JOINTS; i++)
                UniformLocation($"jointTransforms[{i}]");
        }

        public void LoadTexture(string textureUniformName, TextureUnit textureUnit, uint texture)
        {
            base.LoadInt(_location[textureUniformName], textureUnit - TextureUnit.Texture0);
            Gl.ActiveTexture(textureUnit);
            Gl.BindTexture(TextureTarget.Texture2d, texture);
        }

        public void PushBoneMatrix(int index, Matrix4x4f matrix)
        {
            base.LoadMatrix(_location[$"jointTransforms[{index}]"], matrix);
        }

        public void LoadProjMatrix(Matrix4x4f matrix)
        {
            base.LoadMatrix(_location["proj"], matrix);
        }

        public void LoadViewMatrix(Matrix4x4f matrix)
        {
            base.LoadMatrix(_location["view"], matrix);
        }

        public void LoadModelMatrix(Matrix4x4f matrix)
        {
            base.LoadMatrix(_location["model"], matrix);
        }

        public void LoadIsOnlyOneJointWeight(bool isOnlyOneJointWeight)
        {
            base.LoadBoolean(_location["isOnlyOneJointWeight"], isOnlyOneJointWeight);
        }

        public void LoadJointIndex(int jointIndex)
        {
            base.LoadInt(_location["jointIndex"], jointIndex);
        }

        public void LoadBindShapeMatrix(Matrix4x4f bindShapeMatrix)
        {
            base.LoadMatrix(_location["bind"], bindShapeMatrix);
        }

        public void LoadLight(Vertex3f lightDirection)
        {
            base.LoadVector(_location["lightDirection"], lightDirection);
        }

        /// <summary>
        /// GPU position.xyz를 제일 처음 직접 변환하는 행렬를 넘겨준다.
        /// </summary>
        /// <param name="pmodel"></param>
        public void LoadPosModel(Matrix4x4f pmodel)
        {
            base.LoadMatrix(_location["pmodel"], pmodel);
        }
    }
}
