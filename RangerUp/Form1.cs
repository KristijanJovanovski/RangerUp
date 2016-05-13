using System;
using System.Drawing;
using System.Windows.Forms;

namespace RangerUp
{
    public partial class Form1 : Form
    {
        private Image backBuffer;
        private Graphics graphics;
        private readonly Graphics imageGraphics;

        // Timer vars.
        private readonly long interval = (long) TimeSpan.FromSeconds(1.0/60).TotalMilliseconds;
        private long startTime;
        private readonly HiResTimer timer = new HiResTimer();


        public Form1()
        {
            InitializeComponent();
            graphics = CreateGraphics();
        }

        public void GameLoop()
        {
            timer.Start();

            while (Created)
            {
                startTime = timer.ElapsedMilliseconds;
                GameLogic();
                RenderScene();
                Application.DoEvents();
                while (timer.ElapsedMilliseconds - startTime < interval) ;
            }
        }

        private void GameLogic()
        {
            //
        }

        private void RenderScene()
        {
            //
            Invalidate();
        }
    }
}