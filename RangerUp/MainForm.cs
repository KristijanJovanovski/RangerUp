using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.IsolatedStorage;

namespace RangerUp
{
    public partial class MainForm : Form
    {
        private bool saveHighScore;
        private int difficulty;
        public MainForm()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            foreach (RadioButton radioButton in groupBox1.Controls)
            {
                
                if (radioButton.Checked)
                {
                    if (radioButton.Text.Equals("Easy")) {difficulty=1; }
                    else if (radioButton.Text.Equals("Medium")) { difficulty = 2; }
                    else{ difficulty = 3; }
                }
            }

            using (var form1 = new Form1(difficulty))
            {
                Hide();
                form1.Show();
                form1.GameLoop();
                if (form1.DialogResult == DialogResult.OK)
                {
                    saveHighScore = form1.saveHighScore;
                }
                if (saveHighScore)
                {
                    HighScore highScore = new HighScore(saveHighScore);
                    highScore.score = form1.score;
                    if (highScore.ShowDialog() == DialogResult.OK)
                    {
                        Show();
                    }
                }
                else
                {
                    Show();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HighScore highScore = new HighScore(false);
            Hide();
            if(highScore.ShowDialog()== DialogResult.OK)
                Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
