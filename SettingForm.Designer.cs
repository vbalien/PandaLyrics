namespace PandaLyrics
{
    partial class SettingForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selectShadowColor = new System.Windows.Forms.PictureBox();
            this.selectFont = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.selectFontColor = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.selectStartUp = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.selectBgColor = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.selectBgVisible = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.selectWidth = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.selectBgAlpha = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.selectWinOpacity = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.selectShadowColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectFontColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBgColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBgAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectWinOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "그림자색 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "글자 설정";
            // 
            // selectShadowColor
            // 
            this.selectShadowColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.selectShadowColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectShadowColor.Location = new System.Drawing.Point(107, 66);
            this.selectShadowColor.Name = "selectShadowColor";
            this.selectShadowColor.Size = new System.Drawing.Size(24, 24);
            this.selectShadowColor.TabIndex = 3;
            this.selectShadowColor.TabStop = false;
            this.selectShadowColor.Click += new System.EventHandler(this.shadowColorSelcect_Click);
            // 
            // selectFont
            // 
            this.selectFont.Location = new System.Drawing.Point(107, 115);
            this.selectFont.Name = "selectFont";
            this.selectFont.Size = new System.Drawing.Size(178, 27);
            this.selectFont.TabIndex = 4;
            this.selectFont.UseVisualStyleBackColor = true;
            this.selectFont.Click += new System.EventHandler(this.selectFont_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "폰트 :";
            // 
            // selectFontColor
            // 
            this.selectFontColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.selectFontColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectFontColor.Location = new System.Drawing.Point(107, 36);
            this.selectFontColor.Name = "selectFontColor";
            this.selectFontColor.Size = new System.Drawing.Size(24, 24);
            this.selectFontColor.TabIndex = 7;
            this.selectFontColor.TabStop = false;
            this.selectFontColor.Click += new System.EventHandler(this.selectFontColorClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "글자색 :";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(201, 409);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 25);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "확인";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // selectStartUp
            // 
            this.selectStartUp.AutoSize = true;
            this.selectStartUp.Location = new System.Drawing.Point(107, 384);
            this.selectStartUp.Name = "selectStartUp";
            this.selectStartUp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.selectStartUp.Size = new System.Drawing.Size(150, 19);
            this.selectStartUp.TabIndex = 9;
            this.selectStartUp.Text = "시작프로그램으로 등록";
            this.selectStartUp.UseVisualStyleBackColor = true;
            this.selectStartUp.CheckedChanged += new System.EventHandler(this.selectStartUp_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(12, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "창 설정";
            // 
            // selectBgColor
            // 
            this.selectBgColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.selectBgColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectBgColor.Location = new System.Drawing.Point(107, 312);
            this.selectBgColor.Name = "selectBgColor";
            this.selectBgColor.Size = new System.Drawing.Size(24, 24);
            this.selectBgColor.TabIndex = 12;
            this.selectBgColor.TabStop = false;
            this.selectBgColor.Click += new System.EventHandler(this.selectBgColor_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "배경색 :";
            // 
            // selectBgVisible
            // 
            this.selectBgVisible.AutoSize = true;
            this.selectBgVisible.Location = new System.Drawing.Point(107, 289);
            this.selectBgVisible.Name = "selectBgVisible";
            this.selectBgVisible.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.selectBgVisible.Size = new System.Drawing.Size(15, 14);
            this.selectBgVisible.TabIndex = 13;
            this.selectBgVisible.UseVisualStyleBackColor = true;
            this.selectBgVisible.CheckedChanged += new System.EventHandler(this.slelctBgVisible_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(62, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "너비 :";
            // 
            // selectWidth
            // 
            this.selectWidth.Location = new System.Drawing.Point(107, 199);
            this.selectWidth.Maximum = 100;
            this.selectWidth.Name = "selectWidth";
            this.selectWidth.Size = new System.Drawing.Size(183, 45);
            this.selectWidth.TabIndex = 15;
            this.selectWidth.TickStyle = System.Windows.Forms.TickStyle.None;
            this.selectWidth.Value = 50;
            this.selectWidth.Scroll += new System.EventHandler(this.selectWidth_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "배경표시 :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(12, 259);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 15);
            this.label9.TabIndex = 17;
            this.label9.Text = "배경 설정";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(50, 344);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 15);
            this.label10.TabIndex = 18;
            this.label10.Text = "투명도 :";
            // 
            // selectBgAlpha
            // 
            this.selectBgAlpha.Location = new System.Drawing.Point(107, 344);
            this.selectBgAlpha.Maximum = 100;
            this.selectBgAlpha.Name = "selectBgAlpha";
            this.selectBgAlpha.Size = new System.Drawing.Size(183, 45);
            this.selectBgAlpha.TabIndex = 19;
            this.selectBgAlpha.TickStyle = System.Windows.Forms.TickStyle.None;
            this.selectBgAlpha.Value = 1;
            this.selectBgAlpha.Scroll += new System.EventHandler(this.selectBgAlpha_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(51, 229);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 15);
            this.label11.TabIndex = 20;
            this.label11.Text = "투명도 :";
            // 
            // selectWinOpacity
            // 
            this.selectWinOpacity.Location = new System.Drawing.Point(107, 229);
            this.selectWinOpacity.Maximum = 100;
            this.selectWinOpacity.Name = "selectWinOpacity";
            this.selectWinOpacity.Size = new System.Drawing.Size(183, 45);
            this.selectWinOpacity.TabIndex = 21;
            this.selectWinOpacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.selectWinOpacity.Value = 1;
            this.selectWinOpacity.Scroll += new System.EventHandler(this.selectWinOpacity_Scroll);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 443);
            this.Controls.Add(this.selectBgVisible);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.selectWinOpacity);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.selectBgColor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.selectStartUp);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.selectFontColor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.selectFont);
            this.Controls.Add(this.selectShadowColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectWidth);
            this.Controls.Add(this.selectBgAlpha);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PandaLyrics 옵션";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.selectShadowColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectFontColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBgColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBgAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectWinOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox selectShadowColor;
        private System.Windows.Forms.Button selectFont;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox selectFontColor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox selectStartUp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox selectBgColor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox selectBgVisible;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar selectWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar selectBgAlpha;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar selectWinOpacity;
    }
}