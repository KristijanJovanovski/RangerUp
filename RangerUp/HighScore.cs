using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RangerUp
{
    public partial class HighScore : Form
    {
        public bool SaveScore;
        public long Score;
        private List<Player> _highScoreItems;
        public string Difficulty;

        public HighScore(bool ss)
        {
            InitializeComponent();
            SaveScore = ss;
            if (!SaveScore)
            {
                label1.Hide();
                textBox1.Hide();
                button1.Hide();
                button2.Select();
                button2.Focus();
            }
            else
            {
                textBox1.Select();
                textBox1.Focus();
            }

            LoadHighScore();
            
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("No.", 30);
            listView1.Columns.Add("Player Name", 150);
            listView1.Columns.Add("Score", 150);
            listView1.Columns.Add("Difficulty", 100);
            
            ShowHighScore();


        }

        private void LoadHighScore()
        {
            _highScoreItems = new List<Player>();
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(@"..\HighScore.hs",FileMode.OpenOrCreate, FileAccess.Read);
                sr = new StreamReader(fs,Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    var readLine = sr.ReadLine();
                    if (readLine != null)
                    {
                        string[] tmp = readLine.Split(' ');
                        Player player = new Player(tmp[1], int.Parse(tmp[2]),tmp[3]);
                        _highScoreItems.Add(player);
                    }
                }
            }
            finally
            {
                if(fs != null) fs.Close();
                if (sr != null) sr.Close();
            }

        }
        private void SaveHighScore()
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(@"..\HighScore.hs", FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs, Encoding.UTF8);
                int k = 1;
                foreach (var item in _highScoreItems)
                {
                    sw.WriteLine(k++ + ". "+item.Name+" "+item.Score +" "+item.Difficulty);
                }
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }

        private void ShowHighScore()
        {
            listView1.Items.Clear();
            for (int i = 0; i < _highScoreItems.Count; i++)
            {
                var lvi = new ListViewItem(new[]
                {
                    i+1 +". ",
                    _highScoreItems[i].Name,
                    _highScoreItems[i].Score.ToString(),
                    _highScoreItems[i].Difficulty
                });
                listView1.Items.Add(lvi);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Hide();
            textBox1.Hide();
            button1.Hide();

            Player player = new Player(textBox1.Text,Score,Difficulty);
            _highScoreItems.Add(player);
            _highScoreItems.Sort(0,_highScoreItems.Count,player);
            _highScoreItems.Reverse();
            ShowHighScore();

        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(textBox1, "Enter Name");
                e.Cancel = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveHighScore();
            Close();
        }

        private void HighScore_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
