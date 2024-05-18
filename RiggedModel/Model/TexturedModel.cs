namespace LSystem
{
    public class TexturedModel : RawModel3d
    {
        Texture _texture;

        public Texture Texture => _texture;

        public bool IsTextured => _texture != null;

        public TexturedModel(RawModel3d model, Texture texture) : base()
        {
            _texture = texture;
            Init(model.Vertices, model.TexCoords, model.Normals, model.Colors, model.BoneIndices, model.BoneWeights);
            GpuBind();
        }
    }
}
