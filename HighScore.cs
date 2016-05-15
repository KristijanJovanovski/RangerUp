using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace RangerUp
{
    public partial class HighScore : Form
    {
        public bool saveScore;
        public long score;
        private List<Player> highScoreItems;

        public HighScore(bool ss)
        {
            InitializeComponent();
            saveScore = ss;
            if (!saveScore)
            {
                label1.Hide();
                textBox1.Hide();
                button1.Hide();
            }
            else
                textBox1.Focus();

            LoadHighScore();
            
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("No.", 30);
            listView1.Columns.Add("Player Name", 200);
            listView1.Columns.Add("Score", 200);
            
            ShowHighScore();


        }

        private void LoadHighScore()
        {
            highScoreItems = new List<Player>();
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(@"..\HighScore.hs",FileMode.OpenOrCreate, FileAccess.Read);
                sr = new StreamReader(fs,Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    string[] tmp = sr.ReadLine().Split(' ');
                    highScoreItems.Add(new Player(tmp[1],int.Parse(tmp[2])));
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
                foreach (var item in highScoreItems)
                {
                    sw.WriteLine(k++ + ". "+item.name+" "+item.score);
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
            for (int i = 0; i < highScoreItems.Count; i++)
            {
                ListViewItem lvi = new ListViewItem(new string[]
                {
                    i+1 +". ",
                    highScoreItems[i].name,
                    highScoreItems[i].score.ToString()
                });
                listView1.Items.Add(lvi);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Hide();
            textBox1.Hide();
            button1.Hide();

            Player player = new Player(textBox1.Text,score);
            highScoreItems.Add(player);
            highScoreItems.Sort(player.Compare);
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
