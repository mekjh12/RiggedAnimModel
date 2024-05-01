namespace LSystem
{
    partial class Form3D
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.glControl1 = new OpenGL.GlControl();
            this.ckBoneVisible = new System.Windows.Forms.CheckBox();
            this.ckBoneBindPose = new System.Windows.Forms.CheckBox();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.btnIKSolved = new System.Windows.Forms.Button();
            this.lbPrint = new System.Windows.Forms.Label();
            this.cbCharacter = new System.Windows.Forms.ComboBox();
            this.btnCharacterDelete = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.ckWorldAxis = new System.Windows.Forms.CheckBox();
            this.ckSkinVisible = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.nmChainLength = new System.Windows.Forms.NumericUpDown();
            this.nmIternation = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBone = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.ckBoneParentCurrentVisible = new System.Windows.Forms.CheckBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmChainLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmIternation)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Animation = true;
            this.glControl1.BackColor = System.Drawing.Color.Gray;
            this.glControl1.ColorBits = ((uint)(24u));
            this.glControl1.DepthBits = ((uint)(24u));
            this.glControl1.Location = new System.Drawing.Point(13, 12);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.glControl1.MultisampleBits = ((uint)(0u));
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(894, 624);
            this.glControl1.StencilBits = ((uint)(8u));
            this.glControl1.TabIndex = 0;
            this.glControl1.Render += new System.EventHandler<OpenGL.GlControlEventArgs>(this.glControl1_Render);
            this.glControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyDown);
            this.glControl1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glControl1_KeyUp);
            this.glControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDoubleClick);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseWheel);
            // 
            // ckBoneVisible
            // 
            this.ckBoneVisible.AutoSize = true;
            this.ckBoneVisible.Checked = true;
            this.ckBoneVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckBoneVisible.Location = new System.Drawing.Point(924, 56);
            this.ckBoneVisible.Name = "ckBoneVisible";
            this.ckBoneVisible.Size = new System.Drawing.Size(95, 16);
            this.ckBoneVisible.TabIndex = 1;
            this.ckBoneVisible.Text = "Bone Visible";
            this.ckBoneVisible.UseVisualStyleBackColor = true;
            this.ckBoneVisible.CheckedChanged += new System.EventHandler(this.ckBoneVisible_CheckedChanged);
            // 
            // ckBoneBindPose
            // 
            this.ckBoneBindPose.AutoSize = true;
            this.ckBoneBindPose.Location = new System.Drawing.Point(924, 78);
            this.ckBoneBindPose.Name = "ckBoneBindPose";
            this.ckBoneBindPose.Size = new System.Drawing.Size(118, 16);
            this.ckBoneBindPose.TabIndex = 7;
            this.ckBoneBindPose.Text = "BindPose visible";
            this.ckBoneBindPose.UseVisualStyleBackColor = true;
            this.ckBoneBindPose.CheckedChanged += new System.EventHandler(this.ckBoneBindPose_CheckedChanged);
            // 
            // cbAction
            // 
            this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAction.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAction.FormattingEnabled = true;
            this.cbAction.Location = new System.Drawing.Point(924, 326);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(287, 26);
            this.cbAction.TabIndex = 11;
            this.cbAction.SelectedIndexChanged += new System.EventHandler(this.cbAction_SelectedIndexChanged);
            // 
            // btnIKSolved
            // 
            this.btnIKSolved.Location = new System.Drawing.Point(1005, 262);
            this.btnIKSolved.Name = "btnIKSolved";
            this.btnIKSolved.Size = new System.Drawing.Size(75, 26);
            this.btnIKSolved.TabIndex = 12;
            this.btnIKSolved.Text = "IKSolved";
            this.btnIKSolved.UseVisualStyleBackColor = true;
            // 
            // lbPrint
            // 
            this.lbPrint.AutoSize = true;
            this.lbPrint.Font = new System.Drawing.Font("D2Coding", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPrint.Location = new System.Drawing.Point(24, 19);
            this.lbPrint.Name = "lbPrint";
            this.lbPrint.Size = new System.Drawing.Size(63, 15);
            this.lbPrint.TabIndex = 14;
            this.lbPrint.Text = "LB Print";
            // 
            // cbCharacter
            // 
            this.cbCharacter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCharacter.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCharacter.FormattingEnabled = true;
            this.cbCharacter.Location = new System.Drawing.Point(924, 294);
            this.cbCharacter.Name = "cbCharacter";
            this.cbCharacter.Size = new System.Drawing.Size(287, 26);
            this.cbCharacter.TabIndex = 15;
            this.cbCharacter.SelectedIndexChanged += new System.EventHandler(this.cbCharacter_SelectedIndexChanged);
            // 
            // btnCharacterDelete
            // 
            this.btnCharacterDelete.Location = new System.Drawing.Point(924, 262);
            this.btnCharacterDelete.Name = "btnCharacterDelete";
            this.btnCharacterDelete.Size = new System.Drawing.Size(75, 26);
            this.btnCharacterDelete.TabIndex = 16;
            this.btnCharacterDelete.Text = "DeleteFile";
            this.btnCharacterDelete.UseVisualStyleBackColor = true;
            this.btnCharacterDelete.Click += new System.EventHandler(this.btnCharacterDelete_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Font = new System.Drawing.Font("D2Coding", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTime.Location = new System.Drawing.Point(775, 19);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(107, 17);
            this.lblTime.TabIndex = 17;
            this.lblTime.Text = "Time=0.0f/0.0f";
            // 
            // ckWorldAxis
            // 
            this.ckWorldAxis.AutoSize = true;
            this.ckWorldAxis.Checked = true;
            this.ckWorldAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckWorldAxis.Location = new System.Drawing.Point(924, 12);
            this.ckWorldAxis.Name = "ckWorldAxis";
            this.ckWorldAxis.Size = new System.Drawing.Size(126, 16);
            this.ckWorldAxis.TabIndex = 18;
            this.ckWorldAxis.Text = "World Axis Visible";
            this.ckWorldAxis.UseVisualStyleBackColor = true;
            // 
            // ckSkinVisible
            // 
            this.ckSkinVisible.AutoSize = true;
            this.ckSkinVisible.Checked = true;
            this.ckSkinVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckSkinVisible.Location = new System.Drawing.Point(924, 34);
            this.ckSkinVisible.Name = "ckSkinVisible";
            this.ckSkinVisible.Size = new System.Drawing.Size(90, 16);
            this.ckSkinVisible.TabIndex = 21;
            this.ckSkinVisible.Text = "Skin Visible";
            this.ckSkinVisible.UseVisualStyleBackColor = true;
            this.ckSkinVisible.CheckedChanged += new System.EventHandler(this.ckSkinVisible_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(924, 575);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(55, 34);
            this.button3.TabIndex = 22;
            this.button3.Text = "락해제";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1147, 575);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(55, 34);
            this.button4.TabIndex = 23;
            this.button4.Text = "락설정";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // nmChainLength
            // 
            this.nmChainLength.Location = new System.Drawing.Point(1161, 615);
            this.nmChainLength.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nmChainLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmChainLength.Name = "nmChainLength";
            this.nmChainLength.Size = new System.Drawing.Size(51, 21);
            this.nmChainLength.TabIndex = 24;
            this.nmChainLength.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nmChainLength.ValueChanged += new System.EventHandler(this.nmChainLength_ValueChanged);
            // 
            // nmIternation
            // 
            this.nmIternation.Location = new System.Drawing.Point(1012, 615);
            this.nmIternation.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmIternation.Name = "nmIternation";
            this.nmIternation.Size = new System.Drawing.Size(51, 21);
            this.nmIternation.TabIndex = 25;
            this.nmIternation.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmIternation.ValueChanged += new System.EventHandler(this.nmIternation_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("D2Coding", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(1069, 615);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 26;
            this.label1.Text = "ChainLength";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("D2Coding", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(922, 615);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 27;
            this.label2.Text = "Iternation";
            // 
            // cbBone
            // 
            this.cbBone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBone.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBone.FormattingEnabled = true;
            this.cbBone.Location = new System.Drawing.Point(924, 358);
            this.cbBone.Name = "cbBone";
            this.cbBone.Size = new System.Drawing.Size(287, 26);
            this.cbBone.TabIndex = 28;
            this.cbBone.SelectedIndexChanged += new System.EventHandler(this.cbBone_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1115, 406);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 29;
            this.button1.Text = "LeftHand";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1023, 390);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 34);
            this.button2.TabIndex = 30;
            this.button2.Text = "Head";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(939, 406);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 34);
            this.button5.TabIndex = 31;
            this.button5.Text = "RightHand";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(985, 575);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 34);
            this.button6.TabIndex = 32;
            this.button6.Text = "RightFoot";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1066, 575);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 34);
            this.button7.TabIndex = 33;
            this.button7.Text = "LeftFoot";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(1115, 478);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(93, 26);
            this.button8.TabIndex = 34;
            this.button8.Text = "LFingerMiddle";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(1115, 446);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(93, 26);
            this.button9.TabIndex = 35;
            this.button9.Text = "LFingerIndex";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(1115, 511);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(93, 26);
            this.button10.TabIndex = 36;
            this.button10.Text = "LFingerRing";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(1115, 544);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(93, 26);
            this.button11.TabIndex = 37;
            this.button11.Text = "LFingerPinky";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // ckBoneParentCurrentVisible
            // 
            this.ckBoneParentCurrentVisible.AutoSize = true;
            this.ckBoneParentCurrentVisible.Location = new System.Drawing.Point(1066, 12);
            this.ckBoneParentCurrentVisible.Name = "ckBoneParentCurrentVisible";
            this.ckBoneParentCurrentVisible.Size = new System.Drawing.Size(152, 16);
            this.ckBoneParentCurrentVisible.TabIndex = 38;
            this.ckBoneParentCurrentVisible.Text = "현재뼈와 부모뼈만 보기";
            this.ckBoneParentCurrentVisible.UseVisualStyleBackColor = true;
            this.ckBoneParentCurrentVisible.CheckedChanged += new System.EventHandler(this.ckBoneParentCurrentVisible_CheckedChanged);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(12, 652);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 34);
            this.button12.TabIndex = 39;
            this.button12.Text = "바지입기";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(173, 652);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(103, 34);
            this.button13.TabIndex = 40;
            this.button13.Text = "저고리입기";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(362, 652);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 34);
            this.button14.TabIndex = 41;
            this.button14.Text = "헤어밴드";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(93, 652);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 34);
            this.button15.TabIndex = 42;
            this.button15.Text = "바지입기";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(282, 652);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 34);
            this.button16.TabIndex = 43;
            this.button16.Text = "짚신";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(93, 692);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(75, 34);
            this.button17.TabIndex = 44;
            this.button17.Text = "바지제거";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(173, 692);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(103, 34);
            this.button18.TabIndex = 45;
            this.button18.Text = "저고리제거";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(443, 652);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(75, 34);
            this.button19.TabIndex = 46;
            this.button19.Text = "눈붙이기";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // Form3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1223, 844);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.ckBoneParentCurrentVisible);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbBone);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nmIternation);
            this.Controls.Add(this.nmChainLength);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ckSkinVisible);
            this.Controls.Add(this.ckWorldAxis);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnCharacterDelete);
            this.Controls.Add(this.cbCharacter);
            this.Controls.Add(this.lbPrint);
            this.Controls.Add(this.btnIKSolved);
            this.Controls.Add(this.cbAction);
            this.Controls.Add(this.ckBoneBindPose);
            this.Controls.Add(this.ckBoneVisible);
            this.Controls.Add(this.glControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3D";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rigged Model";
            this.Load += new System.EventHandler(this.Form3D_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmChainLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmIternation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenGL.GlControl glControl1;
        private System.Windows.Forms.CheckBox ckBoneVisible;
        private System.Windows.Forms.CheckBox ckBoneBindPose;
        private System.Windows.Forms.ComboBox cbAction;
        private System.Windows.Forms.Button btnIKSolved;
        private System.Windows.Forms.Label lbPrint;
        private System.Windows.Forms.ComboBox cbCharacter;
        private System.Windows.Forms.Button btnCharacterDelete;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.CheckBox ckWorldAxis;
        private System.Windows.Forms.CheckBox ckSkinVisible;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.NumericUpDown nmChainLength;
        private System.Windows.Forms.NumericUpDown nmIternation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbBone;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.CheckBox ckBoneParentCurrentVisible;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
    }
}