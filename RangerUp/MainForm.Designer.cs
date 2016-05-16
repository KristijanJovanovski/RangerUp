using System.ComponentModel;
using System.Windows.Forms;

namespace RangerUp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.newGame_bttn = new System.Windows.Forms.Button();
            this.difficulty_groupbox = new System.Windows.Forms.GroupBox();
            this.hard_radioBttn = new System.Windows.Forms.RadioButton();
            this.medium_radioBttn = new System.Windows.Forms.RadioButton();
            this.easy_radioBttn = new System.Windows.Forms.RadioButton();
            this.highscore_bttn = new System.Windows.Forms.Button();
            this.quit_bttn = new System.Windows.Forms.Button();
            this.instructions_bttn = new System.Windows.Forms.Button();
            this.difficulty_groupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // newGame_bttn
            // 
            this.newGame_bttn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.newGame_bttn.Location = new System.Drawing.Point(23, 33);
            this.newGame_bttn.Name = "newGame_bttn";
            this.newGame_bttn.Size = new System.Drawing.Size(96, 29);
            this.newGame_bttn.TabIndex = 0;
            this.newGame_bttn.Text = "New Game";
            this.newGame_bttn.UseVisualStyleBackColor = true;
            this.newGame_bttn.Click += new System.EventHandler(this.newGame_bttn_Click);
            // 
            // difficulty_groupbox
            // 
            this.difficulty_groupbox.Controls.Add(this.hard_radioBttn);
            this.difficulty_groupbox.Controls.Add(this.medium_radioBttn);
            this.difficulty_groupbox.Controls.Add(this.easy_radioBttn);
            this.difficulty_groupbox.Location = new System.Drawing.Point(237, 33);
            this.difficulty_groupbox.Name = "difficulty_groupbox";
            this.difficulty_groupbox.Size = new System.Drawing.Size(105, 198);
            this.difficulty_groupbox.TabIndex = 1;
            this.difficulty_groupbox.TabStop = false;
            this.difficulty_groupbox.Text = "Difficulty";
            // 
            // hard_radioBttn
            // 
            this.hard_radioBttn.AutoSize = true;
            this.hard_radioBttn.Location = new System.Drawing.Point(16, 163);
            this.hard_radioBttn.Name = "hard_radioBttn";
            this.hard_radioBttn.Size = new System.Drawing.Size(48, 17);
            this.hard_radioBttn.TabIndex = 2;
            this.hard_radioBttn.Text = "Hard";
            this.hard_radioBttn.UseVisualStyleBackColor = true;
            // 
            // medium_radioBttn
            // 
            this.medium_radioBttn.AutoSize = true;
            this.medium_radioBttn.Location = new System.Drawing.Point(16, 90);
            this.medium_radioBttn.Name = "medium_radioBttn";
            this.medium_radioBttn.Size = new System.Drawing.Size(62, 17);
            this.medium_radioBttn.TabIndex = 1;
            this.medium_radioBttn.Text = "Medium";
            this.medium_radioBttn.UseVisualStyleBackColor = true;
            // 
            // easy_radioBttn
            // 
            this.easy_radioBttn.AutoSize = true;
            this.easy_radioBttn.Checked = true;
            this.easy_radioBttn.Location = new System.Drawing.Point(16, 17);
            this.easy_radioBttn.Name = "easy_radioBttn";
            this.easy_radioBttn.Size = new System.Drawing.Size(48, 17);
            this.easy_radioBttn.TabIndex = 0;
            this.easy_radioBttn.TabStop = true;
            this.easy_radioBttn.Text = "Easy";
            this.easy_radioBttn.UseVisualStyleBackColor = true;
            // 
            // highscore_bttn
            // 
            this.highscore_bttn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.highscore_bttn.Location = new System.Drawing.Point(23, 93);
            this.highscore_bttn.Name = "highscore_bttn";
            this.highscore_bttn.Size = new System.Drawing.Size(96, 29);
            this.highscore_bttn.TabIndex = 2;
            this.highscore_bttn.Text = "High Score";
            this.highscore_bttn.UseVisualStyleBackColor = true;
            this.highscore_bttn.Click += new System.EventHandler(this.highscore_bttn_Click);
            // 
            // quit_bttn
            // 
            this.quit_bttn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quit_bttn.Location = new System.Drawing.Point(23, 202);
            this.quit_bttn.Name = "quit_bttn";
            this.quit_bttn.Size = new System.Drawing.Size(96, 29);
            this.quit_bttn.TabIndex = 3;
            this.quit_bttn.Text = "Quit";
            this.quit_bttn.UseVisualStyleBackColor = true;
            this.quit_bttn.Click += new System.EventHandler(this.quit_bttn_Click);
            // 
            // instructions_bttn
            // 
            this.instructions_bttn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.instructions_bttn.Location = new System.Drawing.Point(23, 149);
            this.instructions_bttn.Name = "instructions_bttn";
            this.instructions_bttn.Size = new System.Drawing.Size(96, 29);
            this.instructions_bttn.TabIndex = 4;
            this.instructions_bttn.Text = "Instructions";
            this.instructions_bttn.UseVisualStyleBackColor = true;
            this.instructions_bttn.Click += new System.EventHandler(this.instructions_bttn_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.newGame_bttn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.CancelButton = this.quit_bttn;
            this.ClientSize = new System.Drawing.Size(354, 243);
            this.Controls.Add(this.instructions_bttn);
            this.Controls.Add(this.quit_bttn);
            this.Controls.Add(this.highscore_bttn);
            this.Controls.Add(this.difficulty_groupbox);
            this.Controls.Add(this.newGame_bttn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RangerUp";
            this.TopMost = true;
            this.difficulty_groupbox.ResumeLayout(false);
            this.difficulty_groupbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button newGame_bttn;
        private GroupBox difficulty_groupbox;
        private RadioButton hard_radioBttn;
        private RadioButton medium_radioBttn;
        private RadioButton easy_radioBttn;
        private Button highscore_bttn;
        private Button quit_bttn;
        private Button instructions_bttn;
    }
}