#define myDebug

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RangerUp
{
    public partial class Form1 : Form
    {
        
        // Difficulty variables.
        private int minPlaneTimeSpawn;
        private int maxPlaneTimeSpawn;
        private int minAtomBombTimeSpawn;
        private int maxAtomBombTimeSpawn;
        private int minBulletTimeSpawn;
        private int maxBulletTimeSpawn;
        private int PlaneDropPresent;

        // Graphics
        private Image backBuffer;
        private Graphics graphics;
        private readonly Graphics imageGraphics;
        
        // gameTimer vars.
        private readonly long interval = (long) TimeSpan.FromSeconds(1.0/60).TotalMilliseconds;
        private long startTime;
        private readonly HiResTimer gameTimer = new HiResTimer();
        // planeTimer
        private HiResTimer planeTimer = new HiResTimer();

        private BackgroundLoop backgroundLoop;
        private bool _gameActive;
        private bool GameOver;
        private Hero hero;
        public long score;
        public bool saveHighScore;

        // Lists of objects in game
        private List<Plane> listOfPlanes;
        private List<Plane> listToDeletePlanes;
        private List<Bullet> listOfBullets;
        private List<Bullet> listToDeleteBullets;
        private List<Missile> listOfMissiles;
        private List<Missile> listToDeleteMissiles;
        private List<AtomBomb> listOfAtomBombs;
        private List<AtomBomb> listToDeleteAtomBombs;
        private List<Present> listOfPresents;
        private List<Present> listToDeletePresents;

        // Do not change down of this line
        #region
#if myDebug
        private int tmp = 0;
        int framesRendered = 0;
        long startTimeRender = Environment.TickCount;
        long endTimeRender = 0;
        Label label = new Label();

#endif
#endregion

        public Form1(int difficulty)
        {
            InitializeComponent();

            GenerateDifficultyValues(difficulty);
            _gameActive = true;
            graphics = CreateGraphics();
            backBuffer = (Image)new Bitmap(Width, Height);
            imageGraphics = Graphics.FromImage(backBuffer);
            imageGraphics.SmoothingMode = SmoothingMode.HighQuality;
            Cursor = new Cursor(Properties.Resources.Cursor1.GetHicon());


            backgroundLoop = new BackgroundLoop
            {
                FormWidth = Width,
                FormHeight = Height
            };
            hero = new Hero();

            // Initializing Lists of objects in game
            listOfPlanes = new List<Plane>();
            listToDeletePlanes = new List<Plane>();
            listOfBullets = new List<Bullet>();
            listToDeleteBullets = new List<Bullet>();
            listOfMissiles = new List<Missile>();
            listToDeleteMissiles =new List<Missile>();
            listOfAtomBombs = new List<AtomBomb>();
            listToDeleteAtomBombs = new List<AtomBomb>();
            listOfPresents = new List<Present>();
            listToDeletePresents = new List<Present>();

            planeTimer.Start();
            
            // Do not change down of this line
            #region
#if myDebug
            label.Location = new Point(200, 65);
            label.Width = 250;
            label.BackColor = Color.Transparent;
            Controls.Add(label);
#endif
#endregion

        }

        public void GameLoop()
        {
            gameTimer.Start();
            while (Created)
            {
                if (!GameOver)
                {
                    if (_gameActive)
                    {
                        startTime = gameTimer.ElapsedMilliseconds;
                        GameLogic();
                        RenderScene();
                        while (gameTimer.ElapsedMilliseconds - startTime < interval) ;
                    }
                    Application.DoEvents();
                }
                else
                {
                    score += gameTimer.ElapsedMilliseconds/1000;
                    if (MessageBox.Show("Do you want to save your score in Highscore list?", "Save score?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        saveHighScore = true;
                    }
                    DialogResult = DialogResult.OK;
                    hero.Dispose();
                    Close();
                }
            }
        }

        private void GameLogic()
        {
            #region Load, Unload and GameMoves
            // Generate planes on 3-7s time window.
            if (planeTimer.ElapsedMilliseconds > (new Random().Next(minPlaneTimeSpawn, maxPlaneTimeSpawn)))
            {
                planeTimer.Stop();
                planeTimer.Start();
                listOfPlanes.Add(new Plane
                {
                    X = Width, Y = new Random().Next(50,150),
                    minAtomBombTimeSpawn = minAtomBombTimeSpawn,
                    maxAtomBombTimeSpawn = maxAtomBombTimeSpawn,
                    minBulletTimeSpawn = minBulletTimeSpawn,
                    maxBulletTimeSpawn = maxBulletTimeSpawn,
                    PlaneDropPresent = PlaneDropPresent,
            });
            }
            
            hero.Move();
            if (hero.isDead)
            {
                GameOver = true;
            }

            if (hero.bullet != null)
            {
                listOfBullets.Add(hero.bullet);
                hero.bullet = null;
            }
            if (hero.missile != null)
            {
                listOfMissiles.Add(hero.missile);
                hero.missile = null;
            }
            if (hero.X > (float)Width/2)
            {
                hero.X = (float)Width /2;
                backgroundLoop.XPosition -= 10;
                foreach (var present in listOfPresents)
                {
                    present.X -= 10;
                }
                foreach (var atomBomb in listOfAtomBombs)
                {
                    atomBomb.X -= 10;
                }
            }
            // Planes.
            foreach (var plane in listOfPlanes)
            {
                plane.Move(hero.centerPoint);
               
                if (plane.atomBomb != null)
                {
                    listOfAtomBombs.Add(plane.atomBomb);
                    plane.atomBomb = null;
                }
                if (plane.bullet != null)
                {
                    listOfBullets.Add(plane.bullet);
                    plane.bullet = null;
                }
                if (plane.isDead)
                {
                    score += 25;
                    listToDeletePlanes.Add(plane);
                    if(plane.present !=null)
                        listOfPresents.Add(plane.present);
                }else if(plane.outOfScreen)
                    listToDeletePlanes.Add(plane);
            }
            foreach (var plane in listToDeletePlanes)
            {
                listOfPlanes.Remove(plane);
                plane.Dispose();
            }
            listToDeletePlanes.Clear();

            // Bullets.
            foreach (var bullet in listOfBullets)
            {
                bullet.Move();
                if(bullet.outOfScreen || bullet.didHit)
                    listToDeleteBullets.Add(bullet);
            }
            foreach (var bullet in listToDeleteBullets)
            {
                listOfBullets.Remove(bullet);
                bullet.Dispose();
            }
            listToDeleteBullets.Clear();

            // Missiles.
            foreach (var missile in listOfMissiles)
            {
                missile.Move();
                if(missile.outOfScreen || missile.didHit)
                    listToDeleteMissiles.Add(missile);
            }
            foreach (var missile in listToDeleteMissiles)
            {
                listOfMissiles.Remove(missile);
                missile.Dispose();
            }
            listToDeleteMissiles.Clear();

            // AtomBombs.
            foreach (var atomBomb in listOfAtomBombs)
            {
                atomBomb.Move();
                if (atomBomb.isDead)
                {
                    hero.AssessDamage(atomBomb.centerPoint);
                    listToDeleteAtomBombs.Add(atomBomb);
                }
            }
            foreach (var atomBomb in listToDeleteAtomBombs)
            {
                listOfAtomBombs.Remove(atomBomb);
                atomBomb.Dispose();
            }
            listToDeleteAtomBombs.Clear();

            // Presents.
            foreach (var present in listOfPresents)
            {
                present.Move();
                if(present.isDead)
                    listToDeletePresents.Add(present);
                if(present.outOfScreen)
                    listToDeletePresents.Add(present);
            }
            foreach (var present in listToDeletePresents)
            {
                listOfPresents.Remove(present);
                present.Dispose();
            }
            listToDeletePresents.Clear();
            #endregion
            #region CollisionDetection

            foreach (var bullet in listOfBullets)
            {
                if (!bullet.Collision(Width, Height))
                {
                    hero.Collision(bullet);
                    foreach (var plane in listOfPlanes)
                    {
                        plane.Collision(bullet);
                    }
                    foreach (var present in listOfPresents)
                    {
                        present.Collision(bullet);
                    }
                }
            }
            foreach (var missile in listOfMissiles)
            {
                if (!missile.Collision(Width, Height))
                {
                    hero.Collision(missile);
                    foreach (var plane in listOfPlanes)
                    {
                        plane.Collision(missile);
                    }
                    foreach (var present in listOfPresents)
                    {
                        present.Collision(missile);
                    }
                }
            }
            foreach (var present in listOfPresents)
            {
                if(present.PickUpPresent((int) hero.centerPoint.X,(int) hero.centerPoint.Y))
                    hero.PickedUpPresent(present);
            }
            #endregion
            // Do not change down of this line
            #region

#if myDebug
            //backgroundLoop.XPosition-=20;
            //hero.HeroHealth--;
#endif

            #endregion
        }

        private void RenderScene()
        {
            backgroundLoop.Draw(imageGraphics);
            foreach (var plane in listOfPlanes)
            {
                plane.Draw(imageGraphics);
            }
            foreach (var bullet in listOfBullets)
            {
                bullet.Draw(imageGraphics);
            }
            foreach (var missile in listOfMissiles)
            {
                missile.Draw(imageGraphics);
            }
            foreach (var atomBomb in listOfAtomBombs)
            {
                atomBomb.Draw(imageGraphics);
            }
            foreach (var present in listOfPresents)
            {
                present.Draw(imageGraphics);
            }
            hero.Draw(imageGraphics);
            
            // Do not change down of this line
            #region
#if myDebug
            TextRenderer.DrawText(imageGraphics, "GameTime : " + gameTimer.ElapsedMilliseconds/1000 + " s",
                                    new Font(Font.OriginalFontName, 10f, FontStyle.Regular),
                                    new Point(200, 50), Color.Black, Color.Transparent);
                        TextRenderer.DrawText(imageGraphics, "XPosition = " + backgroundLoop.XPosition +
                                    "/tbackgroundWidth = " + backgroundLoop.BackgroundCoverImage.Width, Form1.DefaultFont,
                                    new Point(400, 50), Color.Black, Color.Transparent);
                        TextRenderer.DrawText(imageGraphics, "Hero Coordinates: X: " + hero.X + "  Y:  " + hero.Y,
                                    Form1.DefaultFont, new Point(400,65), Color.Black);
            TextRenderer.DrawText(imageGraphics, "Score: "+score, Form1.DefaultFont, new Point(600, 65), Color.Black);
            framesRendered++;
                        if (Environment.TickCount >= startTimeRender + 1000)
                        {
                            label.Text = string.Format("Frames Rendered : {0}  fps", framesRendered);
                            framesRendered = 0;
                            startTimeRender = Environment.TickCount;
                        }
                            
#endif
            #endregion
            BackgroundImage = backBuffer;
            Invalidate();
        }

        private void GenerateDifficultyValues(int difficulty )
        {
            if (difficulty == 1)
            {
                minPlaneTimeSpawn = 4000;
                maxPlaneTimeSpawn = 9000;
                minAtomBombTimeSpawn = 2500;
                maxAtomBombTimeSpawn = 5000;
                minBulletTimeSpawn = 1000;
                maxBulletTimeSpawn = 3000;
                PlaneDropPresent = 0;
            }
            else if (difficulty == 2)
            {
                minPlaneTimeSpawn = 3000;
                maxPlaneTimeSpawn = 7000;
                minAtomBombTimeSpawn = 1500;
                maxAtomBombTimeSpawn = 3000;
                minBulletTimeSpawn = 750;
                maxBulletTimeSpawn = 2000;
                PlaneDropPresent = 3;
            }
            else
            {
                minPlaneTimeSpawn = 2000;
                maxPlaneTimeSpawn = 5000;
                minAtomBombTimeSpawn = 1200;
                maxAtomBombTimeSpawn = 2500;
                minBulletTimeSpawn = 400;
                maxBulletTimeSpawn = 1500;
                PlaneDropPresent = 5;
            }
        }

        // Creating temp files from resources
        public string LoadFile(UnmanagedMemoryStream ums)
        {
            var temporaryFilePath = string.Format(
                "{0}{1}{2}", Path.GetTempPath(), Guid.NewGuid().ToString("N"), ".mp3");
            using (var memoryStream = ums)
            using (var tempFileStream = new FileStream(temporaryFilePath, FileMode.Create, FileAccess.Write))
            {
                memoryStream.Position = 0;
                memoryStream.CopyTo(tempFileStream);
            }
            return temporaryFilePath;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameActive)
            {
                if (e.KeyCode == Keys.R){hero.Reload();}
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) { hero.left = true;}
                if(e.KeyCode == Keys.D || e.KeyCode == Keys.Right) { hero.right = true;}
                if (!hero.jump)
                {
                    if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                    {
                        hero.jump = true;
                        hero.Force = hero.G;
                    }
                }
            }
            if (e.KeyCode == Keys.P)
            {
                if (_gameActive)
                {
                    gameTimer.Stop();
                    planeTimer.Stop();
                    if(hero.ReloadingBullets)
                        hero.bulletReloadTimer.Stop();
                    if(hero.ReloadingMissiles)
                        hero.missileReloadTimer.Stop();
                    foreach (var plane in listOfPlanes)
                    {
                        plane.AtomBombTimer.Stop();
                        plane.BulletTimer.Stop();
                    }
                    foreach (var present in listOfPresents)
                    {
                        present.PresentTimer.Stop();
                    }
                }
                else
                {
                    gameTimer.Continue();
                    planeTimer.Continue();
                    if (hero.ReloadingBullets)
                        hero.bulletReloadTimer.Continue();
                    if (hero.ReloadingMissiles)
                        hero.missileReloadTimer.Continue();

                    foreach (var plane in listOfPlanes)
                    {
                        plane.AtomBombTimer.Continue();
                        plane.BulletTimer.Continue();
                    }
                    foreach (var present in listOfPresents)
                    {
                        present.PresentTimer.Continue();
                    }
                }
                _gameActive = !_gameActive;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A) { hero.left = false;}
            if(e.KeyCode == Keys.D) { hero.right = false;}
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            hero.mouseLocation = e.Location;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                hero.FireBurst(e.Location);
            }
            if (e.Button == MouseButtons.Right)
            {
                hero.FireMissile(e.Location);
            }
        }
    }
}