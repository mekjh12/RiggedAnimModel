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
            this.glControl1.Location = new System.Drawing.Point(15, 15);
            this.glControl1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.glControl1.MultisampleBits = ((uint)(0u));
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(1022, 856);
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
            this.ckBoneVisible.Location = new System.Drawing.Point(1056, 70);
            this.ckBoneVisible.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckBoneVisible.Name = "ckBoneVisible";
            this.ckBoneVisible.Size = new System.Drawing.Size(110, 19);
            this.ckBoneVisible.TabIndex = 1;
            this.ckBoneVisible.Text = "Bone Visible";
            this.ckBoneVisible.UseVisualStyleBackColor = true;
            this.ckBoneVisible.CheckedChanged += new System.EventHandler(this.ckBoneVisible_CheckedChanged);
            // 
            // ckBoneBindPose
            // 
            this.ckBoneBindPose.AutoSize = true;
            this.ckBoneBindPose.Location = new System.Drawing.Point(1056, 98);
            this.ckBoneBindPose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckBoneBindPose.Name = "ckBoneBindPose";
            this.ckBoneBindPose.Size = new System.Drawing.Size(139, 19);
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
            this.cbAction.Location = new System.Drawing.Point(1056, 408);
            this.cbAction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(327, 30);
            this.cbAction.TabIndex = 11;
            this.cbAction.SelectedIndexChanged += new System.EventHandler(this.cbAction_SelectedIndexChanged);
            // 
            // btnIKSolved
            // 
            this.btnIKSolved.Location = new System.Drawing.Point(1149, 328);
            this.btnIKSolved.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnIKSolved.Name = "btnIKSolved";
            this.btnIKSolved.Size = new System.Drawing.Size(86, 32);
            this.btnIKSolved.TabIndex = 12;
            this.btnIKSolved.Text = "IKSolved";
            this.btnIKSolved.UseVisualStyleBackColor = true;
            // 
            // lbPrint
            // 
            this.lbPrint.AutoSize = true;
            this.lbPrint.Font = new System.Drawing.Font("D2Coding", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPrint.Location = new System.Drawing.Point(27, 24);
            this.lbPrint.Name = "lbPrint";
            this.lbPrint.Size = new System.Drawing.Size(73, 19);
            this.lbPrint.TabIndex = 14;
            this.lbPrint.Text = "LB Print";
            // 
            // cbCharacter
            // 
            this.cbCharacter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCharacter.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCharacter.FormattingEnabled = true;
            this.cbCharacter.Location = new System.Drawing.Point(1056, 368);
            this.cbCharacter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbCharacter.Name = "cbCharacter";
            this.cbCharacter.Size = new System.Drawing.Size(327, 30);
            this.cbCharacter.TabIndex = 15;
            this.cbCharacter.SelectedIndexChanged += new System.EventHandler(this.cbCharacter_SelectedIndexChanged);
            // 
            // btnCharacterDelete
            // 
            this.btnCharacterDelete.Location = new System.Drawing.Point(1056, 328);
            this.btnCharacterDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCharacterDelete.Name = "btnCharacterDelete";
            this.btnCharacterDelete.Size = new System.Drawing.Size(86, 32);
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
            this.lblTime.Location = new System.Drawing.Point(886, 24);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(123, 21);
            this.lblTime.TabIndex = 17;
            this.lblTime.Text = "Time=0.0f/0.0f";
            // 
            // ckWorldAxis
            // 
            this.ckWorldAxis.AutoSize = true;
            this.ckWorldAxis.Checked = true;
            this.ckWorldAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckWorldAxis.Location = new System.Drawing.Point(1056, 15);
            this.ckWorldAxis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckWorldAxis.Name = "ckWorldAxis";
            this.ckWorldAxis.Size = new System.Drawing.Size(147, 19);
            this.ckWorldAxis.TabIndex = 18;
            this.ckWorldAxis.Text = "World Axis Visible";
            this.ckWorldAxis.UseVisualStyleBackColor = true;
            // 
            // ckSkinVisible
            // 
            this.ckSkinVisible.AutoSize = true;
            this.ckSkinVisible.Checked = true;
            this.ckSkinVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckSkinVisible.Location = new System.Drawing.Point(1056, 42);
            this.ckSkinVisible.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckSkinVisible.Name = "ckSkinVisible";
            this.ckSkinVisible.Size = new System.Drawing.Size(103, 19);
            this.ckSkinVisible.TabIndex = 21;
            this.ckSkinVisible.Text = "Skin Visible";
            this.ckSkinVisible.UseVisualStyleBackColor = true;
            this.ckSkinVisible.CheckedChanged += new System.EventHandler(this.ckSkinVisible_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1056, 719);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(63, 42);
            this.button3.TabIndex = 22;
            this.button3.Text = "락해제";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1056, 769);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(63, 42);
            this.button4.TabIndex = 23;
            this.button4.Text = "락설정";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // nmChainLength
            // 
            this.nmChainLength.Location = new System.Drawing.Point(1274, 769);
            this.nmChainLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.nmChainLength.Size = new System.Drawing.Size(109, 25);
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
            this.nmIternation.Location = new System.Drawing.Point(1274, 802);
            this.nmIternation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nmIternation.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmIternation.Name = "nmIternation";
            this.nmIternation.Size = new System.Drawing.Size(109, 25);
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
            this.label1.Location = new System.Drawing.Point(1169, 769);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 19);
            this.label1.TabIndex = 26;
            this.label1.Text = "ChainLength";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("D2Coding", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(1171, 802);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 19);
            this.label2.TabIndex = 27;
            this.label2.Text = "Iternation";
            // 
            // cbBone
            // 
            this.cbBone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBone.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBone.FormattingEnabled = true;
            this.cbBone.Location = new System.Drawing.Point(1056, 448);
            this.cbBone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbBone.Name = "cbBone";
            this.cbBone.Size = new System.Drawing.Size(327, 30);
            this.cbBone.TabIndex = 28;
            this.cbBone.SelectedIndexChanged += new System.EventHandler(this.cbBone_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1274, 508);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 42);
            this.button1.TabIndex = 29;
            this.button1.Text = "LeftHand";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1169, 488);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 42);
            this.button2.TabIndex = 30;
            this.button2.Text = "Head";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1073, 508);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(86, 42);
            this.button5.TabIndex = 31;
            this.button5.Text = "RightHand";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1126, 719);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(86, 42);
            this.button6.TabIndex = 32;
            this.button6.Text = "RightFoot";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1218, 719);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(86, 42);
            this.button7.TabIndex = 33;
            this.button7.Text = "LeftFoot";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(1274, 598);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(106, 32);
            this.button8.TabIndex = 34;
            this.button8.Text = "LFingerMiddle";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(1274, 558);
            this.button9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(106, 32);
            this.button9.TabIndex = 35;
            this.button9.Text = "LFingerIndex";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(1274, 639);
            this.button10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(106, 32);
            this.button10.TabIndex = 36;
            this.button10.Text = "LFingerRing";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(1274, 680);
            this.button11.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(106, 32);
            this.button11.TabIndex = 37;
            this.button11.Text = "LFingerPinky";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // ckBoneParentCurrentVisible
            // 
            this.ckBoneParentCurrentVisible.AutoSize = true;
            this.ckBoneParentCurrentVisible.Location = new System.Drawing.Point(1218, 15);
            this.ckBoneParentCurrentVisible.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckBoneParentCurrentVisible.Name = "ckBoneParentCurrentVisible";
            this.ckBoneParentCurrentVisible.Size = new System.Drawing.Size(189, 19);
            this.ckBoneParentCurrentVisible.TabIndex = 38;
            this.ckBoneParentCurrentVisible.Text = "현재뼈와 부모뼈만 보기";
            this.ckBoneParentCurrentVisible.UseVisualStyleBackColor = true;
            this.ckBoneParentCurrentVisible.CheckedChanged += new System.EventHandler(this.ckBoneParentCurrentVisible_CheckedChanged);
            // 
            // Form3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1398, 897);
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
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
    }
}