using Assimp;
using Assimp.Configs;
using OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Xml;
using System.Xml.Linq;

namespace LSystem.Animate
{
    public class XmlDae
    {
        string _filename; // dae
        string _diffuseFileName;        
        Dictionary<string, Motion> _motions;
        List<TexturedModel> _texturedModels;
        RawModel3d _rawModel;
        Bone _rootBone;
        Dictionary<string, int> _dicBoneIndex;
        Dictionary<string, Bone> _dicBones;
        string[] _boneNames;
        Matrix4x4f _bindShapeMatrix;        
        float _hipScaled = 1.0f; // 비율을 얻는다.
        List<Vertex3f> _vertices;
        List<Vertex4f> _boneIndices;
        List<Vertex4f> _boneWeights;

        public Dictionary<string, int> DicBoneIndex => _dicBoneIndex;

        public List<Vertex3f> Vertices => _vertices;

        public List<Vertex4f> BoneIndices => _boneIndices;

        public List<Vertex4f> BoneWeights => _boneWeights;

        public Bone GetBoneByName(string boneName) => _dicBones[boneName];

        public Dictionary<string, Bone> DicBones => _dicBones;

        public Bone RootBone => _rootBone;

        public int BoneCount => _boneNames.Length;

        public string[] BoneNames => _boneNames;

        public List<TexturedModel> Models => _texturedModels;

        public Motion DefaultMotion
        {
            get
            {
                List<Motion> list = new List<Motion>(_motions.Values);
                return list.Count > 0 ? list[0] : null;
            }
        }
 
        public Motion GetAnimation(string animationName)
        {
            return (_motions.ContainsKey(animationName))? _motions[animationName] : null;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="filename"></param>
        public XmlDae(string filename, bool isLoadAnimation = true)
        {
            _filename = filename;

            List<TexturedModel> models = LoadFile(filename);

            if (_texturedModels == null)
                _texturedModels = new List<TexturedModel>();

            _texturedModels.AddRange(models);
        }

        public List<TexturedModel> WearCloth(string fileName, float expandValue = 0.00005f)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);

            // (1) library_images = textures
            Dictionary<string, Texture> textures = LibraryImages(xml);
            Dictionary<string, string> materialToEffect = LoadMaterials(xml);
            Dictionary<string, string> effectToImage = LoadEffect(xml);

            // (2) library_geometries = position, normal, texcoord, color
            List<MeshTriangles> meshes = LibraryGeometris(xml, out List<Vertex3f> lstPositions, out List<Vertex2f> lstTexCoord);

            // (3) library_controllers = boneIndex, boneWeight, bindShapeMatrix
            LibraryController(xml, out List<string> clothBoneNames, out Dictionary<string, Matrix4x4f> invBindPoses,
                out List<Vertex4i> lstBoneIndex, out List<Vertex4f> lstBoneWeight,  out Matrix4x4f bindShapeMatrix);
            _bindShapeMatrix = bindShapeMatrix;

            // (4-1) boneName, boneIndexDictionary
            Dictionary<int, int> map = new Dictionary<int, int>();
            for (int i = 0; i < clothBoneNames.Count; i++)
            {
                string clothBoneName = clothBoneNames[i].Trim();
                if (_dicBoneIndex.ContainsKey(clothBoneName))
                {
                    map.Add(i, _dicBoneIndex[clothBoneName]);
                }
                else
                {
                    Console.WriteLine($"현재 뼈대에 매칭되는 본({clothBoneName})이 없습니다. ");
                }
            }

            // (4-2) bone-index modify.
            for (int i = 0; i < lstBoneIndex.Count; i++)
            {
                int bx = map[lstBoneIndex[i].x];
                int by = map[lstBoneIndex[i].y];
                int bz = map[lstBoneIndex[i].z];
                int bw = map[lstBoneIndex[i].w];
                lstBoneIndex[i] = new Vertex4i(bx, by, bz, bw);
            }

            // (5) source positions으로부터 
            Matrix4x4f A0 = _rootBone.LocalBindTransform;
            Matrix4x4f S = _bindShapeMatrix;
            Matrix4x4f A0xS = A0 * S;
            for (int i = 0; i < lstPositions.Count; i++)
            {
                lstPositions[i] = A0xS.Multipy(lstPositions[i]);
            }

            // (6) 읽어온 정보의 인덱스를 이용하여 배열을 만든다.
            List<TexturedModel> texturedModels = new List<TexturedModel>();
            foreach (MeshTriangles meshTriangles in meshes)
            {
                _rawModel = Clothes.Expand(lstPositions, lstTexCoord, lstBoneIndex, lstBoneWeight, meshTriangles, expandValue);

                string effect = materialToEffect[meshTriangles.Material].Replace("#", "");
                string imageName = (effectToImage[effect]);

                if (textures.ContainsKey(imageName))
                {
                    TexturedModel texturedModel = new TexturedModel(_rawModel, textures[imageName]);
                    texturedModel.IsDrawElement = _rawModel.IsDrawElement;
                    texturedModel.VertexCount = _rawModel.VertexCount;
                    texturedModels.Add(texturedModel);
                }
            }

            return texturedModels;
        }

        public string AddAction(string filename)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            string motionName = Path.GetFileNameWithoutExtension(filename);
            if (_motions == null) _motions = new Dictionary<string, Motion>();
            LibraryAnimationWithoutSkin(xml, motionName, ref _motions);
            return motionName;
        }


        public List<TexturedModel> LoadFile(string filename)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);

            // (1) library_images = textures
            Dictionary<string, Texture> textures = LibraryImages(xml);
            Dictionary<string, string> materialToEffect = LoadMaterials(xml);
            Dictionary<string, string> effectToImage = LoadEffect(xml);

            // (2) library_geometries = position, normal, texcoord, color
            List<MeshTriangles> meshes = LibraryGeometris(xml, out List<Vertex3f> lstPositions, out List<Vertex2f> lstTexCoord);

            // (3) library_controllers = boneNames, InvBindPoses, boneIndex, boneWeight
            // invBindPoses는 계산할 수 있으므로 생략가능하다.
            LibraryController(xml, out List<string> boneNames, out Dictionary<string, Matrix4x4f> invBindPoses,
                    out List<Vertex4i> lstBoneIndex, out List<Vertex4f> lstBoneWeight, out Matrix4x4f bindShapeMatrix);
            _bindShapeMatrix = bindShapeMatrix;

            // (3-1) BoneIndex 딕셔너리 
            _boneNames = new string[boneNames.Count];
            _dicBoneIndex = new Dictionary<string, int>();
            for (int i = 0; i < boneNames.Count; i++)
            {
                _boneNames[i] = boneNames[i].Trim();
                _dicBoneIndex.Add(_boneNames[i], i);
            }

            // (4) library_animations
            if (_motions == null) _motions = new Dictionary<string, Motion>();
            LibraryAnimations(xml, ref _motions);

            // (5) library_visual_scenes = bone hierarchy + rootBone
            _rootBone = LibraryVisualScenes(xml, invBindPoses);

            // (6) source positions으로부터 
            Matrix4x4f A0 = _rootBone.LocalBindTransform;
            Matrix4x4f S = _bindShapeMatrix;
            Matrix4x4f A0xS = A0 * S;
            for (int i = 0; i < lstPositions.Count; i++)
            {
                lstPositions[i] = A0xS.Multipy(lstPositions[i]);
            }

            // (7) invBindPose 행렬을 계산한다.
            Stack<Bone> bStack = new Stack<Bone>();
            bStack.Push(_rootBone);
            while (bStack.Count > 0)
            {
                Bone cBone = bStack.Pop();
                Matrix4x4f prevAnimatedMat = (cBone.Parent == null ? Matrix4x4f.Identity : cBone.Parent.AnimatedBindTransform);
                cBone.AnimatedBindTransform = prevAnimatedMat * cBone.LocalBindTransform;
                cBone.InverseBindTransform = (cBone.AnimatedBindTransform * 1000.0f).Inverse * 1000.0f;
                foreach (Bone child in cBone.Childrens) bStack.Push(child);
            }

            // 읽어온 정보의 인덱스를 이용하여 GPU에 데이터를 전송한다.
            List<TexturedModel> texturedModels = new List<TexturedModel>();
            foreach (MeshTriangles meshTriangles in meshes)
            {
                int count = meshTriangles.Vertices.Count;
                float[] postions = new float[count * 3];
                float[] texcoords = new float[count * 2];
                uint[] boneIndices = new uint[count * 4];
                float[] boneWeights = new float[count * 4];
                for (int i = 0; i < count; i++)
                {
                    int idx = (int)meshTriangles.Vertices[i];
                    int tidx = (int)meshTriangles.Texcoords[i];
                    postions[3 * i + 0] = lstPositions[idx].x;
                    postions[3 * i + 1] = lstPositions[idx].y;
                    postions[3 * i + 2] = lstPositions[idx].z;

                    texcoords[2 * i + 0] = lstTexCoord[tidx].x;
                    texcoords[2 * i + 1] = lstTexCoord[tidx].y;

                    boneIndices[4 * i + 0] = (uint)lstBoneIndex[idx].x;
                    boneIndices[4 * i + 1] = (uint)lstBoneIndex[idx].y;
                    boneIndices[4 * i + 2] = (uint)lstBoneIndex[idx].z;
                    boneIndices[4 * i + 3] = (uint)lstBoneIndex[idx].w;

                    boneWeights[4 * i + 0] = (float)lstBoneWeight[idx].x;
                    boneWeights[4 * i + 1] = (float)lstBoneWeight[idx].y;
                    boneWeights[4 * i + 2] = (float)lstBoneWeight[idx].z;
                    boneWeights[4 * i + 3] = (float)lstBoneWeight[idx].w;
                }

                // 로딩한 postions, boneIndices, boneWeights를 버텍스로
                _vertices = new List<Vertex3f>();
                _boneIndices = new List<Vertex4f>();
                _boneWeights = new List<Vertex4f>();
                int vertexNum = postions.Length / 3;
                for (int i = 0; i < vertexNum; i++)
                {
                    _vertices.Add(new Vertex3f(postions[i * 3 + 0], postions[i * 3 + 1], postions[i * 3 + 2]));
                    _boneIndices.Add(new Vertex4f(boneIndices[i * 4 + 0], boneIndices[i * 4 + 1], boneIndices[i * 4 + 2], boneIndices[i * 4 + 3]));
                    _boneWeights.Add(new Vertex4f(boneWeights[i * 4 + 0], boneWeights[i * 4 + 1], boneWeights[i * 4 + 2], boneWeights[i * 4 + 3]));
                }

                // VAO, VBO로 Raw3d 모델을 만든다.
                uint vao = Gl.GenVertexArray();
                Gl.BindVertexArray(vao);
                GpuLoader.StoreDataInAttributeList(0, 3, postions, BufferUsage.StaticDraw);
                GpuLoader.StoreDataInAttributeList(1, 2, texcoords, BufferUsage.StaticDraw);
                GpuLoader.StoreDataInAttributeList(3, 4, boneIndices, BufferUsage.StaticDraw);
                GpuLoader.StoreDataInAttributeList(4, 4, boneWeights, BufferUsage.StaticDraw);
                //GpuLoader.BindIndicesBuffer(lstVertexIndices.ToArray());
                Gl.BindVertexArray(0);
                _rawModel = new RawModel3d(vao, postions);

                string effect = materialToEffect[meshTriangles.Material].Replace("#", "");
                string imageName = (effectToImage[effect]);

                if (textures.ContainsKey(imageName))
                {
                    TexturedModel texturedModel = new TexturedModel(_rawModel, textures[imageName]);
                    texturedModel.IsDrawElement = false;
                    texturedModels.Add(texturedModel);
                }
            }

            return texturedModels;
        }


        /// <summary>
        /// * TextureStorage에 텍스처를 로딩한다. <br/>
        /// - 딕셔너리의 키는 전체파일명으로 한다.<br/>
        /// </summary>
        /// <param name="xml"></param>
        private Dictionary<string, Texture> LibraryImages(XmlDocument xml)
        {
            Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

            XmlNodeList libraryImagesNode = xml.GetElementsByTagName("library_images");
            if (libraryImagesNode.Count > 0)
            {
                foreach (XmlNode imageNode in libraryImagesNode[0].ChildNodes)
                {
                    _diffuseFileName = Path.GetDirectoryName(_filename) + "\\" + imageNode["init_from"].InnerText;
                    _diffuseFileName = _diffuseFileName.Replace("%20", " ");

                    if (!File.Exists(_diffuseFileName))
                    {
                        Console.WriteLine($"[로딩에러] library_image가 존재하지 않습니다. {_diffuseFileName}");
                    }

                    string iamgeId = imageNode.Attributes["id"].Value;
                    Dictionary<TextureType, Texture> _textures = new Dictionary<TextureType, Texture>();
                    Texture texture;
                    if (TextureStorage.TexturesLoaded.ContainsKey(_diffuseFileName)) // 로드한 적이 있음
                    {
                        _textures[TextureType.Diffuse] = TextureStorage.TexturesLoaded[_diffuseFileName];
                        texture = _textures[TextureType.Diffuse];
                    }
                    else  // 로드한 적이 없음
                    {
                        texture = new Texture(_diffuseFileName);
                        _textures.Add(TextureType.Diffuse, texture);
                        TextureStorage.TexturesLoaded.Add(_diffuseFileName, texture);
                    }
                    textures.Add(iamgeId, texture);
                }
            }

            return textures;
        }

        private Dictionary<string, string> LoadMaterials(XmlDocument xml)
        {
            Dictionary<string, string> materials = new Dictionary<string, string>();
            XmlNodeList libraryMaterials = xml.GetElementsByTagName("library_materials");

            if (libraryMaterials.Count > 0)
            {
                foreach (XmlNode material in libraryMaterials[0].ChildNodes)
                {
                    string key = material.Attributes["id"].Value;
                    string value = material["instance_effect"].Attributes["url"].Value;
                    materials.Add(key, value);
                }
            }

            return materials;
        }

        private Dictionary<string, string> LoadEffect(XmlDocument xml)
        {
            Dictionary<string, string> effects = new Dictionary<string, string>();
            XmlNodeList libraryEffects = xml.GetElementsByTagName("library_effects");

            if (libraryEffects.Count > 0)
            {
                foreach (XmlNode effect in libraryEffects[0].ChildNodes)
                { 
                    string key = effect.Attributes["id"].Value;
                    string value = effect["profile_COMMON"]["newparam"].InnerText;
                    effects.Add(key, value);
                }
            }

            return effects;
        }

        private List<MeshTriangles> LibraryGeometris(XmlDocument xml, out List<Vertex3f> lstPositions, out List<Vertex2f> lstTexCoord)
        {
            List<MeshTriangles> meshTriangles = new List<MeshTriangles>();

            List<uint>  lstVertexIndices = new List<uint>();
            List<uint> texcoordIndices = new List<uint>();
            lstPositions = new List<Vertex3f>();
            lstTexCoord = new List<Vertex2f>();

            XmlNodeList libraryGeometries = xml.GetElementsByTagName("library_geometries");

            if (libraryGeometries.Count > 0)
            {
                XmlNode library_geometries = libraryGeometries[0];
                XmlNode geometry = library_geometries["geometry"];

                XmlNode vertices = geometry["mesh"]["vertices"];
                string positionName = vertices["input"].Attributes["source"].Value;
                string vertexName = positionName.Replace("-positions", "-vertices");
                string normalName = positionName.Replace("-positions", "-normals");
                string texcoordName = positionName.Replace("-positions", "-map-0");
                string colorName = positionName.Replace("-positions", "-colors-Col");

                foreach (XmlNode node in geometry["mesh"])
                {
                    // 기본 데이터 source를 읽어옴.
                    if (node.Name == "source")
                    {
                        // 소스텍스트로부터 실수 배열을 만든다.
                        string sourcesId = node.Attributes["id"].Value.Replace(" ", "");
                        string[] value = node["float_array"].InnerText.Split(' ');
                        float[] items = new float[value.Length];
                        for (int i = 0; i < value.Length; i++) 
                            items[i] = float.Parse(value[i]);

                        if ("#" + sourcesId == positionName)
                        {
                            for (int i = 0; i < items.Length; i += 3)
                            {
                                Vertex3f pos = new Vertex3f(items[i], items[i + 1], items[i + 2]);
                                lstPositions.Add(pos);
                            }
                        }
                        else if ("#" + sourcesId == texcoordName)
                        {
                            for (int i = 0; i < items.Length; i += 2)
                            {
                                lstTexCoord.Add(new Vertex2f(items[i], 1.0f - items[i + 1]));
                            }
                        }
                        else if ("#" + sourcesId == normalName)
                        {
                        }
                        else if ("#" + sourcesId == colorName)
                        {
                        }
                    }

                    // triangles만 처리
                    if (node.Name == "triangles")
                    {
                        XmlNode triangles = node;
                        
                        int vertexOffset = -1;
                        int normalOffset = -1;
                        int texcoordOffset = -1;
                        int colorOffset = -1;

                        // offset 읽어온다. pos, tex, nor, color                        
                        foreach (XmlNode input in triangles.ChildNodes)
                        {
                            if (input.Name == "input")
                            {
                                if (input.Attributes["semantic"].Value == "VERTEX")
                                {
                                    vertexName = input.Attributes["source"].Value;
                                    vertexOffset = int.Parse(input.Attributes["offset"].Value);
                                }
                                if (input.Attributes["semantic"].Value == "NORMAL")
                                {
                                    normalName = input.Attributes["source"].Value;
                                    normalOffset = int.Parse(input.Attributes["offset"].Value);
                                }
                                if (input.Attributes["semantic"].Value == "TEXCOORD")
                                {
                                    texcoordName = input.Attributes["source"].Value;
                                    texcoordOffset = int.Parse(input.Attributes["offset"].Value);
                                }
                                if (input.Attributes["semantic"].Value == "COLOR")
                                {
                                    colorName = input.Attributes["source"].Value;
                                    colorOffset = int.Parse(input.Attributes["offset"].Value);
                                }
                            }
                        }

                        XmlNode p = node["p"]; 
                        string[] values = p.InnerText.Split(new char[] { ' ' });
                        int total = (vertexOffset >= 0 ? 1 : 0) + (normalOffset >= 0 ? 1 : 0)
                            + (texcoordOffset >= 0 ? 1 : 0) + (colorOffset >= 0 ? 1 : 0);

                        for (int i = 0; i < values.Length; i += total)
                        {
                            if (vertexOffset >= 0) lstVertexIndices.Add(uint.Parse(values[i + vertexOffset]));
                            //if (normalOffset >= 0) normalIndices.Add(uint.Parse(values[i + normalOffset]));
                            if (texcoordOffset >= 0) texcoordIndices.Add(uint.Parse(values[i + texcoordOffset]));
                            //if (colorOffset >= 0) colorIndices.Add(uint.Parse(values[i + colorOffset]));
                        }

                        string materialName = node.Attributes["material"].Value;

                        MeshTriangles tri = new MeshTriangles();
                        tri.Material = materialName;
                        tri.AddVertices(lstVertexIndices.ToArray());
                        tri.AddTexCoords(texcoordIndices.ToArray());
                        meshTriangles.Add(tri);
                    }
                }
            }

            return meshTriangles;
        }

        private void LibraryController(XmlDocument xml, out List<string> boneNames, out Dictionary<string, Matrix4x4f> invBindPoses,
            out List<Vertex4i> lstBoneIndex, out List<Vertex4f> lstBoneWeight, out Matrix4x4f bindShapeMatrix)
        {
            bindShapeMatrix = Matrix4x4f.Identity;

            lstBoneIndex = new List<Vertex4i>();
            lstBoneWeight = new List<Vertex4f>();

            string jointsName = "";
            string inverseBindMatrixName = "";
            string weightName = "";
            int jointsOffset = -1;
            int weightOffset = -1;
            invBindPoses = new Dictionary<string, Matrix4x4f>();
            boneNames = new List<string>();

            List<float> weightList = new List<float>();

            XmlNodeList libraryControllers = xml.GetElementsByTagName("library_controllers");
            if (libraryControllers.Count > 0)
            {
                XmlNode libraryController = libraryControllers[0];
                XmlNode geometry = libraryController["controller"];
                XmlNode joints = geometry["skin"]["joints"];
                XmlNode vertex_weights = geometry["skin"]["vertex_weights"];

                string[] eles = geometry["skin"]["bind_shape_matrix"].InnerText.Split(' ');
                float[] eleValues = new float[eles.Length];
                for (int i = 0; i < eles.Length; i++)
                    eleValues[i] = float.Parse(eles[i]);

                bindShapeMatrix = new Matrix4x4f(eleValues).Transposed;

                // joints 읽어옴.
                foreach (XmlNode input in joints.ChildNodes)
                {
                    if (input.Name == "input")
                    {
                        // name 가져오기
                        if (input.Attributes["semantic"].Value == "JOINT")
                        {
                            jointsName = input.Attributes["source"].Value;
                        }
                        if (input.Attributes["semantic"].Value == "INV_BIND_MATRIX")
                        {
                            inverseBindMatrixName = input.Attributes["source"].Value;
                        }

                        foreach (XmlNode source in geometry["skin"].ChildNodes)
                        {
                            if (source.Name == "source")
                            {
                                string sourcesId = source.Attributes["id"].Value;

                                if (source["Name_array"] != null)
                                {
                                    string[] value = source["Name_array"].InnerText.Split(' ');

                                    // BoneName가져오기
                                    if ("#" + sourcesId == jointsName)
                                    {
                                        boneNames.Clear();
                                        boneNames.AddRange(value);
                                    }
                                }

                                if (source["float_array"] != null)
                                {
                                    string[] value = source["float_array"].InnerText.Split(' ');
                                    float[] items = new float[value.Length];
                                    for (int i = 0; i < value.Length; i++)
                                        items[i] = float.Parse(value[i]);

                                    // INV_BIND_MATRIX
                                    if ("#" + sourcesId == inverseBindMatrixName)
                                    {
                                        for (int i = 0; i < items.Length; i += 16)
                                        {
                                            List<float> mat = new List<float>();
                                            for (int j = 0; j < 16; j++) mat.Add(items[i + j]);
                                            Matrix4x4f bindpose = new Matrix4x4f(mat.ToArray());
                                            invBindPoses.Add(boneNames[i / 16], bindpose.Transposed);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // vertex_weights 읽어옴.
                foreach (XmlNode input in vertex_weights.ChildNodes)
                {
                    if (input.Name == "input")
                    {
                        // name 가져오기
                        if (input.Attributes["semantic"].Value == "WEIGHT") weightName = input.Attributes["source"].Value;
                        foreach (XmlNode source in geometry["skin"].ChildNodes)
                        {
                            if (source.Name == "source")
                            {
                                string sourcesId = source.Attributes["id"].Value;
                                if (source["float_array"] != null)
                                {
                                    string[] value = source["float_array"].InnerText.Split(' ');
                                    float[] items = new float[value.Length];
                                    for (int i = 0; i < value.Length; i++)
                                        items[i] = float.Parse(value[i]);

                                    // WEIGHT
                                    if ("#" + sourcesId == weightName) weightList.AddRange(items);
                                }
                            }
                        }
                    }
                }

                // vertex_weights - vcount, v 읽어옴.
                XmlNode vcount = geometry["skin"]["vertex_weights"]["vcount"];
                XmlNode v = geometry["skin"]["vertex_weights"]["v"];
                string[] vcountArray = vcount.InnerText.Trim().Split(' ');
                int[] vcountIntArray = new int[vcountArray.Length];
                uint total = 0;
                for (int i = 0; i < vcountArray.Length; i++)
                {
                    vcountIntArray[i] = int.Parse(vcountArray[i].Trim());
                    total += (uint)vcountIntArray[i];
                }

                foreach (XmlNode input in geometry["skin"]["vertex_weights"].ChildNodes)
                {
                    if (input.Name == "input")
                    {
                        if (input.Attributes["semantic"].Value == "JOINT") jointsOffset = int.Parse(input.Attributes["offset"].Value);
                        if (input.Attributes["semantic"].Value == "WEIGHT") weightOffset = int.Parse(input.Attributes["offset"].Value);
                    }
                }

                string[] vArray = v.InnerText.Split(' ');
                int sum = 0;
                for (int i = 0; i < vcountIntArray.Length; i++)
                {
                    int vertexCount = vcountIntArray[i];
                    List<int> boneIndexList = new List<int>();
                    List<int> boneWeightList = new List<int>();

                    for (int j = 0; j < vertexCount; j++)
                    {
                        if (jointsOffset >= 0)
                            boneIndexList.Add(int.Parse(vArray[sum + 2 * j + jointsOffset].Trim()));
                        if (weightOffset >= 0)
                            boneWeightList.Add(int.Parse(vArray[sum + 2 * j + weightOffset].Trim()));
                    }

                    // 정렬하기
                    List<Vertex2f> bwList = new List<Vertex2f>();
                    for (int k = 0; k < boneWeightList.Count; k++)
                    {
                        float w = weightList[boneWeightList[k]];
                        bwList.Add(new Vertex2f(boneIndexList[k], w));
                    }

                    bwList.Sort((a, b) => b.y.CompareTo(a.y));
                    if (bwList.Count > 4)
                    {
                        for (int k = 4; k < bwList.Count; k++)
                        {
                            bwList[0] = new Vertex2f(bwList[0].x, bwList[0].y + bwList[k].y);
                        }
                        bwList.RemoveRange(4, bwList.Count - 4);
                    }

                    // 정렬된 가중치 정보를 리스트에 담는다.
                    Vertex4i jointId = Vertex4i.Zero;
                    Vertex4f weight = Vertex4f.Zero;

                    for (int k = 0; k < bwList.Count; k++)
                    {
                        if (k == 0) jointId.x = (int)bwList[k].x;
                        if (k == 1) jointId.y = (int)bwList[k].x;
                        if (k == 2) jointId.z = (int)bwList[k].x;
                        if (k == 3) jointId.w = (int)bwList[k].x;
                        if (k == 0) weight.x = bwList[k].y;
                        if (k == 1) weight.y = bwList[k].y;
                        if (k == 2) weight.z = bwList[k].y;
                        if (k == 3) weight.w = bwList[k].y;
                    }

                    lstBoneIndex.Add(jointId);
                    lstBoneWeight.Add(weight);
                    sum += 2 * vertexCount;
                }
            }
        }

        private void LibraryAnimations(XmlDocument xml, ref Dictionary<string, Motion> animations)
        {
            XmlNodeList libraryAnimations = xml.GetElementsByTagName("library_animations");
            if (libraryAnimations.Count == 0)
            {
                Console.WriteLine($"[에러] dae파일구조에서 애니메이션구조를 읽어올 수 없습니다.");
                return;
            }

            Dictionary<string, Dictionary<float, Matrix4x4f>> ani = new Dictionary<string, Dictionary<float, Matrix4x4f>>();
            foreach (XmlNode libraryAnimation in libraryAnimations[0])
            {
                string animationName = libraryAnimation.Attributes["name"].Value;
                float maxTimeLength = 0.0f;
                string motionName = "";

                // bone마다 순회
                foreach (XmlNode boneAnimation in libraryAnimation.ChildNodes)
                {
                    string boneName = boneAnimation.Attributes["id"].Value.Substring(animationName.Length + 1);
                    int fIdx = boneName.IndexOf("_");
                    motionName = (fIdx >= 0) ? boneName.Substring(0, fIdx) : "";
                    boneName = (fIdx >= 0) ? boneName.Substring(fIdx + 1) : boneName;
                    boneName = boneName.Replace("_pose_matrix", "");
                    List<float> sourceInput = new List<float>(); // time interval
                    List<Matrix4x4f> sourceOutput = new List<Matrix4x4f>();
                    List<string> interpolationInput = new List<string>();

                    XmlNode channel = boneAnimation["channel"];
                    string channelName = channel.Attributes["source"].Value;

                    XmlNode sampler = boneAnimation["sampler"];
                    if (channelName != "#" + sampler.Attributes["id"].Value) continue;

                    string inputName = "";
                    string outputName = "";
                    string interpolationName = "";

                    // semantic의 Name을 읽어옴.
                    foreach (XmlNode input in sampler.ChildNodes)
                    {
                        if (input.Attributes["semantic"].Value == "INPUT") inputName = input.Attributes["source"].Value;
                        if (input.Attributes["semantic"].Value == "OUTPUT") outputName = input.Attributes["source"].Value;
                        if (input.Attributes["semantic"].Value == "INTERPOLATION") interpolationName = input.Attributes["source"].Value;
                    }

                    // bone의 애니메이션 소스를 읽어온다.
                    foreach (XmlNode source in boneAnimation.ChildNodes)
                    {
                        if (source.Name == "source")
                        {
                            string sourcesId = source.Attributes["id"].Value;
                            if ("#" + sourcesId == inputName)
                            {
                                string[] value = source["float_array"].InnerText.Split(' ');
                                float[] items = new float[value.Length];
                                for (int i = 0; i < value.Length; i++)
                                {
                                    items[i] = float.Parse(value[i]);
                                    maxTimeLength = Math.Max(items[i], maxTimeLength);
                                }
                                sourceInput.AddRange(items);
                            }

                            if ("#" + sourcesId == outputName)
                            {
                                string[] value = source["float_array"].InnerText.Split(' ');
                                float[] items = new float[value.Length];
                                for (int i = 0; i < value.Length; i++) items[i] = float.Parse(value[i]);
                                for (int i = 0; i < value.Length; i += 16)
                                {
                                    List<float> mat = new List<float>();
                                    for (int j = 0; j < 16; j++) mat.Add(items[i + j]);
                                    Matrix4x4f matrix = new Matrix4x4f(mat.ToArray());
                                    sourceOutput.Add(matrix.Transposed);
                                }
                            }

                            if ("#" + sourcesId == interpolationName)
                            {
                                string[] value = source["Name_array"].InnerText.Split(' ');
                                interpolationInput.AddRange(value);
                            }
                        }
                    }

                    // 가져온 소스로 키프레임을 만든다.
                    Dictionary<float, Matrix4x4f> keyframe = new Dictionary<float, Matrix4x4f>();
                    for (int i = 0; i < sourceInput.Count; i++)
                    {
                        keyframe.Add(sourceInput[i], sourceOutput[i]);
                    }

                    ani.Add(boneName, keyframe);
                }

                Motion animation = new Motion(motionName, maxTimeLength);
                if (maxTimeLength > 0)
                {
                    foreach (KeyValuePair<string, Dictionary<float, Matrix4x4f>> item in ani)
                    {
                        string boneName = item.Key;
                        Dictionary<float, Matrix4x4f> source = item.Value;
                        foreach (KeyValuePair<float, Matrix4x4f> subsource in source)
                        {
                            float time = subsource.Key;
                            Matrix4x4f mat = subsource.Value;
                            animation.AddKeyFrame(time);

                            Quaternion q = ToQuaternion(mat);
                            q.Normalize();
                            BonePose bonePose = new BonePose();
                            bonePose.Position = new Vertex3f(mat[3, 0], mat[3, 1], mat[3, 2]);
                            bonePose.Rotation = q;
                            animation[time].AddBoneTransform(boneName, bonePose);
                        }
                    }
                }
                animations.Add(motionName, animation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private Bone LibraryVisualScenes(XmlDocument xml, Dictionary<string, Matrix4x4f> invBindPoses)
        {
            XmlNodeList library_visual_scenes = xml.GetElementsByTagName("library_visual_scenes");
            if (library_visual_scenes.Count == 0)
            {
                Console.WriteLine($"[에러] dae파일구조에서 뼈대구조를 읽어올 수 없습니다.");
                return null;
            }

            Stack<XmlNode> nStack = new Stack<XmlNode>();
            Stack<Bone> bStack = new Stack<Bone>();
            XmlNode nodes = library_visual_scenes[0]["visual_scene"];
            XmlNode rootNode = null;

            _dicBones = new Dictionary<string, Bone>();

            // Find Root Node
            foreach (XmlNode item in nodes) 
                if (item.Attributes["id"].Value == "Armature") rootNode = item;
            if (rootNode == null) return null;

            nStack.Push(rootNode);
            Bone rootBone = new Bone("Armature", 0);
            bStack.Push(rootBone);

            while (nStack.Count > 0)
            {
                XmlNode node = nStack.Pop();
                Bone bone = bStack.Pop();

                // 행렬읽기
                string[] value = node["matrix"].InnerText.Split(' ');
                float[] items = new float[value.Length];
                for (int i = 0; i < value.Length; i++) items[i] = float.Parse(value[i]);
                Matrix4x4f mat = new Matrix4x4f(items).Transposed;
                
                string boneName = node.Attributes["sid"]?.Value;

                if (boneName != null)
                    _dicBones.Add(boneName, bone);

                if (boneName == null)
                {
                    if (node.Attributes["name"].Value == "Armature")
                    {
                        bone.Name = "Armature";
                        bone.LocalBindTransform = mat;
                        bone.Index = 0;
                    }
                }
                else
                {
                    bone.Name = boneName;
                    bone.LocalBindTransform = mat;
                    bone.Index = _dicBoneIndex.ContainsKey(boneName) ? _dicBoneIndex[boneName] : -1;
                }

                bone.PivotPosition = mat.Column3.Vertex3f();

                if (invBindPoses.ContainsKey(bone.Name))
                {
                    bone.InverseBindTransform = invBindPoses[bone.Name];                    
                }

                // 하위 노드를 순회한다.
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != "node") continue;
                    nStack.Push(child);
                    Bone childBone = new Bone("", 0);
                    childBone.Parent = bone;
                    bone.AddChild(childBone);
                    bStack.Push(childBone);
                }
            }


            return rootBone;
        }

        private void LoadDefaultScaleTransform(string motionName, Dictionary<string, Dictionary<float, Matrix4x4f>> ani, float maxTimeLength)
        {
            if (_dicBones==null) return;

            Motion animation = new Motion(motionName, maxTimeLength);
            if (maxTimeLength > 0)
            {
                // 뼈마다 순회
                foreach (KeyValuePair<string, Dictionary<float, Matrix4x4f>> item in ani)
                {
                    string boneName = item.Key;
                    Dictionary<float, Matrix4x4f> source = item.Value;

                    Bone bone = _dicBones.ContainsKey(boneName) ? _dicBones[boneName] : null;
                    if (bone == null) continue;

                    // 뼈의 시간에 따른 순횐
                    foreach (KeyValuePair<float, Matrix4x4f> subsource in source)
                    {
                        float time = subsource.Key;
                        Matrix4x4f mat = subsource.Value;

                        if (bone.IsRootArmature)
                        {
                            float dstSize = bone.PivotPosition.Norm();
                            float srcSize = source[0.0f].Position.Norm();
                            _hipScaled = dstSize / srcSize;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// * Mixamo에서 Export한 Dae파일을 그대로 읽어온다. <br/>
        /// - Without Skin, Only Armature <br/>
        /// - "3D Mesh Processing and Character Animation", p.183 Animation Retargeting
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="animations"></param>
        private void LibraryAnimationWithoutSkin(XmlDocument xml, string motionName, ref Dictionary<string, Motion> animations)
        {
            XmlNodeList libraryAnimations = xml.GetElementsByTagName("library_animations");
            if (libraryAnimations.Count == 0)
            {
                Console.WriteLine($"[에러] dae파일구조에서 애니메이션구조를 읽어올 수 없습니다.");
                return;
            }

            Dictionary<string, Dictionary<float, Matrix4x4f>> ani = new Dictionary<string, Dictionary<float, Matrix4x4f>>();
            float maxTimeLength = 0.0f;
            //string animationName = libraryAnimation.Attributes["name"].Value;

            // bone마다 순회
            foreach (XmlNode boneAnimation in libraryAnimations[0].ChildNodes)
            {
                string boneName = boneAnimation.Attributes["id"].Value;
                boneName = boneName.Substring(0, boneName.Length - 5);
                if (boneName == "Armature") continue;

                List<float> sourceInput = new List<float>(); // time interval
                List<Matrix4x4f> sourceOutput = new List<Matrix4x4f>();
                List<string> interpolationInput = new List<string>();

                XmlNode channel = boneAnimation["channel"];
                string channelName = channel.Attributes["source"].Value;

                XmlNode sampler = boneAnimation["sampler"];
                if (channelName != "#" + sampler.Attributes["id"].Value) continue;

                string inputName = "";
                string outputName = "";
                string interpolationName = "";

                // semantic의 Name을 읽어옴.
                foreach (XmlNode input in sampler.ChildNodes)
                {
                    if (input.Attributes["semantic"].Value == "INPUT") inputName = input.Attributes["source"].Value;
                    if (input.Attributes["semantic"].Value == "OUTPUT") outputName = input.Attributes["source"].Value;
                    if (input.Attributes["semantic"].Value == "INTERPOLATION") interpolationName = input.Attributes["source"].Value;
                }

                // bone의 애니메이션 소스를 읽어온다.
                foreach (XmlNode source in boneAnimation.ChildNodes)
                {
                    if (source.Name == "source")
                    {
                        string sourcesId = source.Attributes["id"].Value;
                        if ("#" + sourcesId == inputName)
                        {
                            string[] value = source["float_array"].InnerText.Trim().Replace("\n"," ").Split(' ');
                            float[] items = new float[value.Length];
                            for (int i = 0; i < value.Length; i++)
                            {
                                items[i] = float.Parse(value[i].Trim());
                                maxTimeLength = Math.Max(items[i], maxTimeLength);
                            }
                            sourceInput.AddRange(items);
                        }

                        if ("#" + sourcesId == outputName)
                        {
                            string[] value = source["float_array"].InnerText.Trim().Replace("\n", " ").Split(' ');
                            float[] items = new float[value.Length];
                            for (int i = 0; i < value.Length; i++) items[i] = float.Parse(value[i]);
                            for (int i = 0; i < value.Length; i += 16)
                            {
                                List<float> mat = new List<float>();
                                for (int j = 0; j < 16; j++) mat.Add(items[i + j]);
                                Matrix4x4f matrix = new Matrix4x4f(mat.ToArray());

                                sourceOutput.Add(matrix.Transposed);
                            }
                        }

                        if ("#" + sourcesId == interpolationName)
                        {
                            string[] value = source["Name_array"].InnerText.Trim().Replace("\n", " ").Split(' ');
                            interpolationInput.AddRange(value);
                        }
                    }
                }

                // 가져온 소스로 키프레임을 만든다.
                Dictionary<float, Matrix4x4f> keyframe = new Dictionary<float, Matrix4x4f>();
                for (int i = 0; i < sourceInput.Count; i++)
                {
                    keyframe.Add(sourceInput[i], sourceOutput[i]);
                }

                ani.Add(boneName, keyframe);
            }

            // hipScale을 구한다.
            if (motionName == "Interpolation Pose")
            {
                LoadDefaultScaleTransform(motionName, ani, maxTimeLength);
            }

            Motion animation = new Motion(motionName, maxTimeLength);
            if (maxTimeLength > 0 && _dicBones != null)
            {
                // 뼈마다 순회
                foreach (KeyValuePair<string, Dictionary<float, Matrix4x4f>> item in ani)
                {
                    string boneName = item.Key;
                    Dictionary<float, Matrix4x4f> source = item.Value;

                    Bone bone = _dicBones.ContainsKey(boneName) ? _dicBones[boneName] : null;
                    if (bone == null) continue;

                    // 뼈의 시간에 따른 순회
                    foreach (KeyValuePair<float, Matrix4x4f> subsource in source)
                    {
                        float time = subsource.Key;
                        Matrix4x4f mat = subsource.Value;
                        animation.AddKeyFrame(time);

                        Vertex3f position = bone.IsRootArmature ? 
                            (mat.Position) * _hipScaled : bone.PivotPosition;

                        Quaternion q = ToQuaternion(mat);
                        q.Normalize();

                        BonePose bonePose = new BonePose(position, q);
                        animation[time].AddBoneTransform(boneName, bonePose);
                    }
                }
            }

            if (!animations.ContainsKey(motionName)) // 동일한 키가 없으면 애니메이션을 추가함. 
            {
                animations.Add(motionName, animation);  // 키가 있으면 중복되어 제일 먼저 로딩한 모션으로 정함.
            } 
        }

        /// <summary>
        /// Quaternions for Computer Graphics by John Vince. p199 참고
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        private Quaternion ToQuaternion(Matrix4x4f mat)
        {
            Quaternion q = Quaternion.Identity;
            float a11 = mat[0, 0];
            float a12 = mat[1, 0];
            float a13 = mat[2, 0];

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
