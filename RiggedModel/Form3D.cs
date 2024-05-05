using LSystem.Animate;
using OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;

namespace LSystem
{
    public partial class Form3D : Form
    {
        EngineLoop _gameLoop;
        List<Entity> _entities;
        StaticShader _shader;
        AnimateShader _ashader;
        BoneWeightShader _bwShader;

        AniModel _aniModel;
        AniModel _aniModel1;
        AniModel _selectedAniModel;
        XmlDae xmlDae;

        bool _isDraged = false;
        bool _isShifted = false;
        bool _isIkApply = false;

        ColorPoint[] _point;
        Vertex3f _ikPoint = new Vertex3f(0.6f, 0.0f, 1.8f);
        Vertex3f _centerPoint = new Vertex3f(0.0f, 0.0f, 0.0f);
        float _theta = 0.0f;

        CTrackBar trFov;
        CTrackBar trAxisLength;
        CTrackBar trAxisThick;


        public static float _rot = 0.0f;

        public Form3D()
        {
            InitializeComponent();

            int margin = 5;

            // 화면구성
            // --------------------------------------------------------------------------------------
            trFov = new CTrackBar("Fov", 20, 160, 1);
            trFov.Location = new System.Drawing.Point(this.glControl1.Right + margin, this.ckBoneBindPose.Bottom + margin);
            this.Controls.Add(trFov);

            trAxisLength = new CTrackBar("AxisLength", 1, 60, 1);
            trAxisLength.Location = new System.Drawing.Point(this.trFov.Left, this.trFov.Bottom + margin);
            this.Controls.Add(trAxisLength);

            trAxisThick = new CTrackBar("AxisThick", 1, 600, 1);
            trAxisThick.Location = new System.Drawing.Point(this.trAxisLength.Left, this.trAxisLength.Bottom + margin);
            this.Controls.Add(trAxisThick);

        }

        private void Form3D_Load(object sender, EventArgs e)
        {
            // ### 초기화 ###
            IniFile.SetFileName("setup.ini");

            // ### 초기화 ###
            _gameLoop = new EngineLoop();
            _shader = new StaticShader();
            _ashader = new AnimateShader();
            _bwShader = new BoneWeightShader();
            _entities = new List<Entity>();

            // 바닥
            TexturedModel floor = new TexturedModel(Loader3d.LoadPlane(0, 20.0f), new Texture(EngineLoop.PROJECT_PATH + "\\Res\\grass.png"));
            _entities.Add(new Entity("floor", floor) { Material = Material.White });

            string[] cfiles = Directory.GetFiles(EngineLoop.PROJECT_PATH + "\\Res\\");
            foreach (string fn in cfiles)
            {
                if (Path.GetExtension(fn) == ".dae")
                {
                    string file = Path.GetFileName(fn);
                    this.cbCharacter.Items.Add(file);
                }
            }

            string[] files = Directory.GetFiles(EngineLoop.PROJECT_PATH + "\\Res\\Action\\");
            foreach (string fn in files)
            {
                if (Path.GetExtension(fn) == ".dae")
                    this.cbAction.Items.Add(Path.GetFileNameWithoutExtension(fn));
            }

            if (this.cbCharacter.Items.Count > 0)
            {
                xmlDae = new XmlDae(EngineLoop.PROJECT_PATH + $"\\Res\\{cbCharacter.Items[0]}", isLoadAnimation: false);
                xmlDae.AddAction(EngineLoop.PROJECT_PATH + "\\Res\\Action\\Interpolation Pose.dae");
                Entity daeEntity = new Entity("aniModel", xmlDae.Models.ToArray());

                _aniModel = new AniModel("main1", daeEntity, xmlDae);
                _aniModel.SetMotion(xmlDae.DefaultMotion.Name);

                Entity daeEntity1 = new Entity("aniModel1", xmlDae.Models.ToArray());
                daeEntity1.Position = new Vertex3f(1, 2, 0);
                _aniModel1 = new AniModel("main2", daeEntity1, xmlDae);
                _aniModel1.SetMotion(xmlDae.DefaultMotion.Name);

                _selectedAniModel = _aniModel;
                //this.cbCharacter.Text = "heroNasty.dae"; // xmlDae.DefaultMotion.Name;
                //this.cbAction.Text = (string)this.cbAction.Items[0];
            }


            // 설정 읽어오기
            // -------------------------------------------------------------------------------------------------------
            float cx = float.Parse(IniFile.GetPrivateProfileString("camera", "x", "0.0"));
            float cy = float.Parse(IniFile.GetPrivateProfileString("camera", "y", "0.0"));
            float cz = float.Parse(IniFile.GetPrivateProfileString("camera", "z", "0.0"));
            float yaw = float.Parse(IniFile.GetPrivateProfileString("camera", "yaw", "0.0"));
            float pitch = float.Parse(IniFile.GetPrivateProfileString("camera", "pitch", "0.0"));
            float distance = float.Parse(IniFile.GetPrivateProfileString("camera", "distance", "3.0"));
            float fov = float.Parse(IniFile.GetPrivateProfileString("camera", "fov", "90.0"));
            this.ckBoneBindPose.Checked = bool.Parse(IniFile.GetPrivateProfileString("control", "isvisibleBindBone", "true"));
            this.ckBoneVisible.Checked = bool.Parse(IniFile.GetPrivateProfileString("control", "isvisibleBone", "true"));
            this.ckSkinVisible.Checked = bool.Parse(IniFile.GetPrivateProfileString("control", "isvisibleSkin", "true"));
            this.ckBoneParentCurrentVisible.Checked = bool.Parse(IniFile.GetPrivateProfileString("control", "isvisibleCPBone", "true"));
            this.nmChainLength.Value = (decimal)IniFile.GetPrivateProfileFloat("control", "ChainLength", 3.0f);
            this.nmIternation.Value = (decimal)IniFile.GetPrivateProfileFloat("control", "Iternation", 1.0f);
            //_renderingMode = (RenderingMode)int.Parse(IniFile.GetPrivateProfileString("Rendering", "RenderingMode", "0"));
            //_drawThick = float.Parse(IniFile.GetPrivateProfileString("Rendering", "drawThick", "1.0"));
            //_axisLength = float.Parse(IniFile.GetPrivateProfileString("Rendering", "axisLength", "1.0"));
            //_polygonMode = (PolygonMode)int.Parse(IniFile.GetPrivateProfileString("PolygonMode", "PolygonMode", "6912"));

            float px = float.Parse(IniFile.GetPrivateProfileString("PickupPoint", "x", "1.0"));
            float py = float.Parse(IniFile.GetPrivateProfileString("PickupPoint", "y", "1.0"));
            float pz = float.Parse(IniFile.GetPrivateProfileString("PickupPoint", "z", "1.0"));

            //
            _ikPoint = new Vertex3f(px, py, pz);
            // -------------------------------------------------------------------------------------------------------

            _gameLoop.Camera = new OrbitCamera("", cx, cy, cz, distance);
            _gameLoop.Camera.CameraPitch = pitch;
            _gameLoop.Camera.CameraYaw = yaw;
            _gameLoop.Camera.FOV = fov;
            
            // 컨트롤 설정부
            trFov.ValueChanged += (oo, ee) =>
            {
                Camera camera = _gameLoop.Camera;
                camera.FOV = trFov.Value;
                IniFile.WritePrivateProfileString("camera", "fov", camera.FOV);
            };

            trAxisLength.ValueChanged += (oo, ee) =>
            {
                //_axisLength = trAxisLength.Value * 0.1f;
                //IniFile.WritePrivateProfileString("Rendering", "axisLength", _axisLength);
            };

            trAxisThick.ValueChanged += (oo, ee) =>
            {
                //_drawThick = (float)trAxisThick.Value * 0.01f;
                //IniFile.WritePrivateProfileString("Rendering", "drawThick", _drawThick);
            };

            this.trFov.Value = (int)fov;
            //this.trAxisLength.Value = (int)(_axisLength * 10.0f);

            trFov.Draw();
            trAxisLength.Draw();
            trAxisThick.Draw();

            this.cbCharacter.SelectedIndex = 0;
            this.cbAction.SelectedIndex = 0;
            foreach (string boneName in xmlDae.BoneNames)
            {
                this.cbBone.Items.Add(boneName);
            }
            this.cbBone.Text = IniFile.GetPrivateProfileString("control", "EndBoneName", "mixamorig_LeftHand");


            //---------------------------------------------------------------------------------------
            // Update
            //---------------------------------------------------------------------------------------
            _gameLoop.UpdateFrame = (deltaTime) =>
            {
                float milliSecond = deltaTime * 0.001f;
                int w = this.glControl1.Width;
                int h = this.glControl1.Height;

                if (_gameLoop.Width * _gameLoop.Height == 0)
                {
                    _gameLoop.Init(w, h);
                    _gameLoop.Camera.Init(w, h);
                }

                _aniModel.Update(deltaTime);
                //_aniModel1.Update(deltaTime);


                if (_isIkApply)
                {
                    Bone boneEnd = _aniModel.GetBoneByName(this.cbBone.Text);
                    if (boneEnd != null)
                    {
                        if (_aniModel.Animator.IsPlaying)
                        {
                            _theta += 0.1f;
                            _ikPoint = new Vertex3f(_ikPoint.x, (float)(_ikPoint.y + 0.000f * Math.Cos(_theta)), 
                                (float)(_ikPoint.z + 0.000f * Math.Sin(_theta)));

                            Form3D._rot++;
                        }

                        Vertex3f ikpoint = (_aniModel.GetEntity("main").ModelMatrix.Inverse * _ikPoint.Vertex4f()).Vertex3f();

                        _point = Kinetics.IKSolvedByFABRIK(target: ikpoint, boneEnd, 
                            chainLength: (int)this.nmChainLength.Value,
                            iternations: (int)this.nmIternation.Value);

                        //_aniModel.RootBone.UpdateChildBone(false);
                        //Kinetics.BoneRotate(boneEnd, 1);
                    }
                }

                Entity entity = _entities.Count > 0 ? _entities[0] : null;
                if (Keyboard.IsKeyDown(Key.D1)) entity.Roll(1);
                if (Keyboard.IsKeyDown(Key.D2)) entity.Roll(-1);
                if (Keyboard.IsKeyDown(Key.D3)) entity.Yaw(1);
                if (Keyboard.IsKeyDown(Key.D4)) entity.Yaw(-1);
                if (Keyboard.IsKeyDown(Key.D5)) entity.Pitch(1);
                if (Keyboard.IsKeyDown(Key.D6)) entity.Pitch(-1);

                OrbitCamera camera = _gameLoop.Camera as OrbitCamera;
                this.lblTime.Text = "time=" + _aniModel.MotionTime.Round(3) + "/" + _aniModel.Animator.CurrentMotion.Length.Round(3);
                this.Text = $"{FramePerSecond.FPS}fps, t={FramePerSecond.GlobalTick} p={camera.Position}, distance={camera.Distance}";

            };

            //---------------------------------------------------------------------------------------
            // Render
            //---------------------------------------------------------------------------------------
            _gameLoop.RenderFrame = (deltaTime) =>
            {
                Camera camera = _gameLoop.Camera;

                Gl.Enable(EnableCap.CullFace);
                Gl.CullFace(CullFaceMode.Back);

                Gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
                Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                Gl.Enable(EnableCap.DepthTest);

                Gl.Enable(EnableCap.Blend);
                Gl.BlendEquation(BlendEquationMode.FuncAdd);
                Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                if (this.ckWorldAxis.Checked) Renderer.RenderAxis(_shader, camera);

                // 사물에 대한 렌더링
                foreach (Entity entity in _entities)
                {
                    Renderer.Render(_shader, entity, camera);
                }

                _aniModel.Render(camera, _shader, _ashader, _bwShader, this.ckSkinVisible.Checked, this.ckBoneVisible.Checked);
                //_aniModel1.Render(camera, _shader, _ashader, _bwShader, this.ckSkinVisible.Checked, this.ckBoneVisible.Checked);


                if (camera is OrbitCamera)
                {
                    Renderer.RenderPoint(_shader, camera.Position, camera, new Vertex4f(1, 0, 0, 1), 0.01f);
                }
            };

        }

        private void glControl1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Camera camera = _gameLoop.Camera;
            if (camera is FpsCamera) camera?.GoForward(0.02f * e.Delta);
            if (camera is OrbitCamera) (camera as OrbitCamera)?.FarAway(-0.005f * e.Delta);
        }

        private void glControl1_Render(object sender, GlControlEventArgs e)
        {
            int glLeft = this.Width - this.glControl1.Width;
            int glTop = this.Height - this.glControl1.Height;
            int glWidth = this.glControl1.Width;
            int glHeight = this.glControl1.Height;
            _gameLoop.DetectInput(this.Left + glLeft, this.Top + glTop, glWidth, glHeight);

            // 엔진 루프, 처음 로딩시 deltaTime이 커지는 것을 방지
            if (FramePerSecond.DeltaTime < 1000)
            {
                _gameLoop.Update(deltaTime: FramePerSecond.DeltaTime);
                _gameLoop.Render(deltaTime: FramePerSecond.DeltaTime);
            }
            FramePerSecond.Update();
        }

        private void WriteEnv()
        {
            // 종료 설정 저장
            IniFile.WritePrivateProfileString("camera", "x", _gameLoop.Camera.Position.x);
            IniFile.WritePrivateProfileString("camera", "y", _gameLoop.Camera.Position.y);
            IniFile.WritePrivateProfileString("camera", "z", _gameLoop.Camera.Position.z);
            IniFile.WritePrivateProfileString("camera", "yaw", _gameLoop.Camera.CameraYaw);
            IniFile.WritePrivateProfileString("camera", "pitch", _gameLoop.Camera.CameraPitch);
            IniFile.WritePrivateProfileString("camera", "distance", ((OrbitCamera)_gameLoop.Camera).Distance);
        }

        private void glControl1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Shift) _isShifted = true;

            if (e.KeyCode == Keys.Escape)
            {
                if (MessageBox.Show("정말로 끝내시겠습니까?", "종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    WriteEnv();
                    Application.Exit();
                }
            }
            else if (e.KeyCode == Keys.H)
            {
                _selectedAniModel?.Transform.Roll(1);
            }
            else if (e.KeyCode == Keys.K)
            {
                _selectedAniModel?.Transform.Roll(-1);
            }
            else if (e.KeyCode == Keys.U)
            {
                _selectedAniModel?.Transform.IncreasePosition(0, 0.05f, 0);
            }
            else if (e.KeyCode == Keys.J)
            {
                _selectedAniModel?.Transform.IncreasePosition(0, -0.05f, 0);
            }
            else if (e.KeyCode == Keys.I)
            {
                _isIkApply = !_isIkApply;
            }
            else if (e.KeyCode == Keys.F)
            {
                IniFile.WritePrivateProfileString("PolygonMode", "PolygonMode", (int)_selectedAniModel.PolygonMode);
                _selectedAniModel.PopPolygonMode();
            }
            else if (e.KeyCode == Keys.Space)
            {
                _selectedAniModel?.Animator.Toggle();
            }
            else if (e.KeyCode == Keys.Oemplus)
            {
                _selectedAniModel.SelectedBoneIndex++;
                Console.WriteLine(_selectedAniModel.SelectedBoneIndex);
            }
            else if (e.KeyCode == Keys.OemMinus)
            {
                _selectedAniModel.SelectedBoneIndex--;
                Console.WriteLine(_selectedAniModel.SelectedBoneIndex);
            }
            else if (e.KeyCode == Keys.R)
            {
                IniFile.WritePrivateProfileString("Rendering", "RenderingMode", (int)_aniModel.RenderMode);
                _selectedAniModel.PopRenderingMode();
            }
            else if (e.KeyCode == Keys.Z)
            {
                _selectedAniModel.Animator.Play();
                _gameLoop.Update(-10);
                _selectedAniModel.Animator.Stop();
            }
            else if (e.KeyCode == Keys.X)
            {
                _selectedAniModel.Animator.Play();
                _gameLoop.Update(-100);
                _selectedAniModel.Animator.Stop();
            }
            else if (e.KeyCode == Keys.C)
            {
                _selectedAniModel.Animator.Play();
                _gameLoop.Update(100);
                _selectedAniModel.Animator.Stop();
            }
            else if (e.KeyCode == Keys.V)
            {
                _selectedAniModel.Animator.Play();
                _gameLoop.Update(10);
                _selectedAniModel.Animator.Stop();
            }
            else if (e.KeyCode == Keys.D1)
            {
                _selectedAniModel = (_selectedAniModel == _aniModel) ? _aniModel1 : _aniModel;
                Console.WriteLine(_selectedAniModel.Name);
            }
            else if (e.KeyCode == Keys.D3)
            {
                Bone bone = _aniModel.GetBoneByName("mixamorig_LeftHand_end");
                Vertex3f G = _ikPoint;

                // 말단뼈로부터 최상위 뼈까지 리스트를 만들고 Chain Length를 구함.
                List<Bone> bones = new List<Bone>();
                Bone parent = bone;
                bones.Add(parent);
                while (parent.Parent != null)
                {
                    bones.Add(parent.Parent);
                    parent = parent.Parent;
                }

                // 가능한 Chain Length
                int rootChainLength = bones.Count;
                int N = Math.Min(3, rootChainLength);

                // 뼈의 리스트 (말단의 뼈로부터 최상위 뼈로의 순서로)
                // 0번째가 말단뼈 --> ... --> N-1이 최상위 뼈
                // [초기값 설정] 캐릭터 공간 행렬과 뼈 공간 행렬을 만듦 
                Bone[] Bn = new Bone[N];
                for (int i = 0; i < N; i++) Bn[i] = bones[i];

                
            }

            //
        }

        private void ckBoneVisible_CheckedChanged(object sender, EventArgs e)
        {
            IniFile.WritePrivateProfileString("control", "isvisibleBone", this.ckBoneVisible.Checked.ToString());
        }

        private void glControl1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Camera camera = _gameLoop.Camera;
            Bone bone = _aniModel.GetBoneByName(this.cbBone.Text);
            camera.GoTo(bone.AnimatedTransform.Position);
            if (camera is OrbitCamera) (camera as OrbitCamera).Distance = 1.0f;
        }

        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _isDraged = true;
            if (e.Button == MouseButtons.Left)
            {
                Camera camera = _gameLoop.Camera;
                _ikPoint = Picker3d.PickUpPoint(camera, e.X, e.Y, glControl1.Width, glControl1.Height);
                this.lbPrint.Text = _ikPoint.ToString();
                IniFile.WritePrivateProfileString("PickupPoint", "x", _ikPoint.x);
                IniFile.WritePrivateProfileString("PickupPoint", "y", _ikPoint.y);
                IniFile.WritePrivateProfileString("PickupPoint", "z", _ikPoint.z);
            }
        }

        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _isDraged = false;
        }

        private void glControl1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Mouse.CurrentPosition = new Vertex2i(e.X, e.Y);

            if (e.Button == MouseButtons.Middle && _isShifted)
            {
                Camera camera = _gameLoop.Camera;
                float sensity = 0.3f;
                Vertex2i delta = Mouse.DeltaPosition;
                camera?.GoRight(-sensity * delta.x);
                camera?.GoForward(sensity * delta.y);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                Camera camera = _gameLoop.Camera;
                Vertex2i delta = Mouse.DeltaPosition;
                camera?.Yaw(-delta.x);
                camera?.Pitch(delta.y);
            }

            Mouse.PrevPosition = new Vertex2i(e.X, e.Y);
        }

        private void glControl1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!e.Shift) _isShifted = false;
        }

        private void cbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            _aniModel.SetMotion(cbAction.Text);
        }

        private void ckBoneBindPose_CheckedChanged(object sender, EventArgs e)
        {
            IniFile.WritePrivateProfileString("control", "isvisibleBindBone", this.ckBoneBindPose.Checked.ToString());
        }

        private void cbCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            xmlDae = new XmlDae(EngineLoop.PROJECT_PATH + $"\\Res\\{this.cbCharacter.Text}", isLoadAnimation: false);
            string[] files = Directory.GetFiles(EngineLoop.PROJECT_PATH + "\\Res\\Action\\");
            this.cbAction.Items.Clear();
            xmlDae.AddAction(EngineLoop.PROJECT_PATH + "\\Res\\Action\\Interpolation Pose.dae");
            foreach (string fn in files)
            {
                if (Path.GetExtension(fn) == ".dae")
                    this.cbAction.Items.Add(xmlDae.AddAction(fn));
            }

            Entity daeEntity = new Entity("aniModel", xmlDae.Models.ToArray());
            daeEntity.Material = new Material();
            daeEntity.Position = new Vertex3f(0, 0, 0);
            daeEntity.IsAxisVisible = true;

            _aniModel = new AniModel("main", daeEntity, xmlDae);
            _aniModel.SetMotion(xmlDae.DefaultMotion.Name);
        }

        private void btnCharacterDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("정말로 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string fn = EngineLoop.PROJECT_PATH + $"\\Res\\{this.cbCharacter.Text}";
                if (File.Exists(fn))
                {
                    File.Delete(fn);
                    this.cbCharacter.Items.RemoveAt(this.cbCharacter.SelectedIndex);
                    if (this.cbCharacter.Items.Count > 0) this.cbCharacter.SelectedIndex = 0;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _aniModel.GetBoneByName("mixamorig_LeftHand").RestrictAngle = new BoneAngle(-180, 180, -180, 180, -180, 180);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _aniModel.GetBoneByName("mixamorig_LeftHand").RestrictAngle = new BoneAngle(-45, 45, -30, 30, 0, 0);
            _aniModel.GetBoneByName("mixamorig_LeftForeArm").RestrictAngle = new BoneAngle(-90, 90, 0, 0, 0, 0);
            _aniModel.GetBoneByName("mixamorig_LeftArm").RestrictAngle = new BoneAngle(-90, 90, -90, 90, 0, 0);
        }

        private void ckSkinVisible_CheckedChanged(object sender, EventArgs e)
        {
            IniFile.WritePrivateProfileString("control", "isvisibleSkin", this.ckSkinVisible.Checked.ToString());
        }

        private void nmChainLength_ValueChanged(object sender, EventArgs e)
        {
            IniFile.WritePrivateProfileString("control", "ChainLength", this.nmChainLength.Value.ToString());
        }

        private void nmIternation_ValueChanged(object sender, EventArgs e)
        {
            IniFile.WritePrivateProfileString("control", "Iternation", this.nmIternation.Value.ToString());
        }

        private void cbBone_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniFile.WritePrivateProfileString("control", "EndBoneName", this.cbBone.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_Head";
            this.nmChainLength.Value = 2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_RightHand";
            this.nmChainLength.Value = 3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_LeftHand";
            this.nmChainLength.Value = 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_RightToeBase";
            this.nmChainLength.Value = 4;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_LeftToeBase";
            this.nmChainLength.Value = 4;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_LeftHandMiddle3";
            this.nmChainLength.Value = 6;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_LeftHandIndex3";
            this.nmChainLength.Value = 6;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_LeftHandRing3";
            this.nmChainLength.Value = 6;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.cbBone.Text = "mixamorig_LeftHandPinky3";
            this.nmChainLength.Value = 6;
        }

        private void ckBoneParentCurrentVisible_CheckedChanged(object sender, EventArgs e)
        {
            IniFile.WritePrivateProfileString("control", "isvisibleCPBone", this.ckBoneParentCurrentVisible.Checked.ToString());
        }

        private void button12_Click(object sender, EventArgs e)
        {
        }

        private void button13_Click(object sender, EventArgs e)
        {
            _aniModel.Attach(EngineLoop.PROJECT_PATH + "\\Res\\Clothes\\jeogori.dae");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            _aniModel.Attach(EngineLoop.PROJECT_PATH + "\\Res\\Clothes\\nobiHairBand.dae");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            _aniModel.Attach(EngineLoop.PROJECT_PATH + "\\Res\\Clothes\\pants.dae");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            _aniModel.Attach(EngineLoop.PROJECT_PATH + "\\Res\\Clothes\\gipshin.dae");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            _aniModel.Entities.Remove("pants");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            _aniModel.Entities.Remove("jeogori");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            _aniModel.Transplant(EngineLoop.PROJECT_PATH + "\\Res\\Clothes\\eye.dae", "mixamorig_Head");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Bone cBone = _aniModel.GetBoneByName("eyeRight");
            cBone.LocalBindTransform = cBone.LocalBindTransform * Matrix4x4f.Translated(0, 0.1f, 0);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Bone cBone = _aniModel.GetBoneByName("eyeRight");
            cBone.LocalBindTransform = cBone.LocalBindTransform * Matrix4x4f.Translated(0, -0.1f, 0);

        }

        private void button22_Click(object sender, EventArgs e)
        {
            Bone cBone = _aniModel.GetBoneByName("eyeRight");
            cBone.LocalBindTransform = cBone.LocalBindTransform * Matrix4x4f.Translated(0.1f, 0, 0);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Bone cBone = _aniModel.GetBoneByName("eyeRight");
            cBone.LocalBindTransform = cBone.LocalBindTransform * Matrix4x4f.Translated(-0.1f, 0, 0);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Bone cBone = _aniModel.GetBoneByName("eyeRight");
            cBone.LocalBindTransform = cBone.LocalBindTransform * Matrix4x4f.Translated(0, 0, 0.1f);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Bone cBone = _aniModel.GetBoneByName("eyeRight");
            cBone.LocalBindTransform = cBone.LocalBindTransform * Matrix4x4f.Translated(0, 0, -0.1f);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Bone cBone = _aniModel.GetBoneByName("eyeRight");
            Console.WriteLine(cBone.LocalBindTransform);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            
            Motion motion = _aniModel.CurrentMotion;
            KeyFrame firstKeyFrame = motion.FirstKeyFrame;
            KeyFrame lastKeyFrame = motion.LastKeyFrame;
            KeyFrame middleKeyFrame = motion.MiddleKeyFrame;

            Bone bone = _aniModel.GetBoneByName("eyeLeft");

            float n = motion.Keyframes.Count;
            float i = 0.0f;
            foreach (KeyValuePair<float, KeyFrame> item in motion.Keyframes)
            {
                KeyFrame keyFrame = item.Value;
                float t = i / n;
                float rad = (float)(40.0f * Math.Sin(3.141502f *t) - 20);
                keyFrame.AddBoneTransform("eyeLeft", new BonePose(bone.LocalBindTransform.Position, new Quaternion(Vertex3f.UnitY, rad)));
                i += 1.0f;
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {

        }
    }
}
