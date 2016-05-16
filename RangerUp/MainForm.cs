using System;
using System.Windows.Forms;

namespace RangerUp
{
    public partial class MainForm : Form
    {
        private bool _saveHighScore;
        private int _difficulty;
        public MainForm()
        {
            InitializeComponent();
        }

        private void newGame_bttn_Click(object sender, EventArgs e)
        {
            foreach (RadioButton radioButton in difficulty_groupbox.Controls)
            {
                if (radioButton.Checked)
                {
                    if (radioButton.Text.Equals("Easy")) { _difficulty = 1; }
                    else if (radioButton.Text.Equals("Medium")) { _difficulty = 2; }
                    else { _difficulty = 3; }
                }
            }
            using (var form1 = new Form1(_difficulty))
            {
                Hide();
                form1.Show();
                form1.GameLoop();
                if (form1.DialogResult == DialogResult.OK)
                {
                    _saveHighScore = form1.SaveHighScore;
                }
                if (_saveHighScore)
                {
                    HighScore highScore = new HighScore(_saveHighScore);
                    highScore.Score = form1.Score;
                    foreach (RadioButton radioButton in difficulty_groupbox.Controls)
                    {
                        if (radioButton.Checked)
                        {
                            highScore.Difficulty = radioButton.Text;
                            break;
                        }
                    }
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

        private void highscore_bttn_Click(object sender, EventArgs e)
        {
            HighScore highScore = new HighScore(false);
            Hide();
            if (highScore.ShowDialog() == DialogResult.OK)
                Show();
        }

        private void instructions_bttn_Click(object sender, EventArgs e)
        {
            InstructionsForm instructions = new InstructionsForm();
            instructions.Show();
        }

        private void quit_bttn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
