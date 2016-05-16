using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RangerUp.Properties;

namespace RangerUp
{
    public sealed partial class Form1 : Form
    {
        
        // Difficulty variables.
        private int _minPlaneTimeSpawn;
        private int _maxPlaneTimeSpawn;
        private int _minAtomBombTimeSpawn;
        private int _maxAtomBombTimeSpawn;
        private int _minBulletTimeSpawn;
        private int _maxBulletTimeSpawn;
        private int _planeDropPresent;

        // Graphics
        private Image _backBuffer;
        private readonly Graphics _imageGraphics;
        
        // gameTimer vars.
        private readonly long _interval = (long) TimeSpan.FromSeconds(1.0/60).TotalMilliseconds;
        private long _startTime;
        private readonly HiResTimer _gameTimer = new HiResTimer();

        // planeTimer
        private HiResTimer _planeTimer = new HiResTimer();

        private BackgroundLoop _backgroundLoop;
        private bool _gameActive;
        private bool _gameOver;
        private Hero _hero;
        public long Score;
        public bool SaveHighScore;

        // Lists of objects in game
        private List<Plane> _listOfPlanes;
        private List<Plane> _listToDeletePlanes;
        private List<Bullet> _listOfBullets;
        private List<Bullet> _listToDeleteBullets;
        private List<Missile> _listOfMissiles;
        private List<Missile> _listToDeleteMissiles;
        private List<AtomBomb> _listOfAtomBombs;
        private List<AtomBomb> _listToDeleteAtomBombs;
        private List<Present> _listOfPresents;
        private List<Present> _listToDeletePresents;

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
            _gameActive = true;
            GenerateDifficultyValues(difficulty);
            CreateGraphics();
            _backBuffer = new Bitmap(Width, Height);
            _imageGraphics = Graphics.FromImage(_backBuffer);
            _imageGraphics.SmoothingMode = SmoothingMode.HighQuality;
            Cursor = new Cursor(Resources.Cursor1.GetHicon());

            _backgroundLoop = new BackgroundLoop
            {
                FormWidth = Width
            };
            _hero = new Hero();

            // Initializing Lists of objects in game
            _listOfPlanes = new List<Plane>();
            _listToDeletePlanes = new List<Plane>();
            _listOfBullets = new List<Bullet>();
            _listToDeleteBullets = new List<Bullet>();
            _listOfMissiles = new List<Missile>();
            _listToDeleteMissiles =new List<Missile>();
            _listOfAtomBombs = new List<AtomBomb>();
            _listToDeleteAtomBombs = new List<AtomBomb>();
            _listOfPresents = new List<Present>();
            _listToDeletePresents = new List<Present>();

            _planeTimer.Start();
        }

        public void GameLoop()
        {
            _gameTimer.Start();
            while (Created)
            {
                if (!_gameOver)
                {
                    if (_gameActive)
                    {
                        _startTime = _gameTimer.ElapsedMilliseconds;
                        GameLogic();
                        RenderScene();
                        while (_gameTimer.ElapsedMilliseconds - _startTime < _interval){}
                    }
                    Application.DoEvents();
                }
                else
                {
                    Score += _gameTimer.ElapsedMilliseconds/1000;
                    if (MessageBox.Show(Resources.Form1_GameLoop_Do_you_want_to_save_your_score_in_Highscore_list_, Resources.Form1_GameLoop_Save_score_, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SaveHighScore = true;
                    }
                    DialogResult = DialogResult.OK;
                    _hero.Dispose();
                    Close();
                }
            }
        }

        private void GameLogic()
        {
            #region Load, Unload and GameMoves
            if (_planeTimer.ElapsedMilliseconds > (new Random().Next(_minPlaneTimeSpawn, _maxPlaneTimeSpawn)))
            {
                _planeTimer.Stop();
                _planeTimer.Start();
                _listOfPlanes.Add(new Plane
                {
                    X = Width, Y = new Random().Next(50,150),
                    MinAtomBombTimeSpawn = _minAtomBombTimeSpawn,
                    MaxAtomBombTimeSpawn = _maxAtomBombTimeSpawn,
                    MinBulletTimeSpawn = _minBulletTimeSpawn,
                    MaxBulletTimeSpawn = _maxBulletTimeSpawn,
                    PlaneDropPresent = _planeDropPresent
            });
            }
            
            _hero.Move();
            if (_hero.IsDead)
            {
                _gameOver = true;
            }

            if (_hero.Bullet != null)
            {
                _listOfBullets.Add(_hero.Bullet);
                _hero.Bullet = null;
            }
            if (_hero.Missile != null)
            {
                _listOfMissiles.Add(_hero.Missile);
                _hero.Missile = null;
            }
            if (_hero.X > (float)Width/2)
            {
                _hero.X = (float)Width /2;
                _backgroundLoop.XPosition -= 10;
                foreach (var present in _listOfPresents)
                {
                    present.X -= 10;
                }
                foreach (var atomBomb in _listOfAtomBombs)
                {
                    atomBomb.X -= 10;
                }
            }
            // Planes.
            foreach (var plane in _listOfPlanes)
            {
                plane.Move(_hero.CenterPoint);
               
                if (plane.AtomBomb != null)
                {
                    _listOfAtomBombs.Add(plane.AtomBomb);
                    plane.AtomBomb = null;
                }
                if (plane.Bullet != null)
                {
                    _listOfBullets.Add(plane.Bullet);
                    plane.Bullet = null;
                }
                if (plane.IsDead)
                {
                    Score += 25;
                    _listToDeletePlanes.Add(plane);
                    if(plane.Present !=null)
                        _listOfPresents.Add(plane.Present);
                }else if(plane.OutOfScreen)
                    _listToDeletePlanes.Add(plane);
            }
            foreach (var plane in _listToDeletePlanes)
            {
                _listOfPlanes.Remove(plane);
                plane.Dispose();
            }
            _listToDeletePlanes.Clear();

            // Bullets.
            foreach (var bullet in _listOfBullets)
            {
                bullet.Move();
                if(bullet.OutOfScreen || bullet.DidHit)
                    _listToDeleteBullets.Add(bullet);
            }
            foreach (var bullet in _listToDeleteBullets)
            {
                _listOfBullets.Remove(bullet);
                bullet.Dispose();
            }
            _listToDeleteBullets.Clear();

            // Missiles.
            foreach (var missile in _listOfMissiles)
            {
                missile.Move();
                if(missile.OutOfScreen || missile.DidHit)
                    _listToDeleteMissiles.Add(missile);
            }
            foreach (var missile in _listToDeleteMissiles)
            {
                _listOfMissiles.Remove(missile);
                missile.Dispose();
            }
            _listToDeleteMissiles.Clear();

            // AtomBombs.
            foreach (var atomBomb in _listOfAtomBombs)
            {
                atomBomb.Move();
                if (atomBomb.IsDead)
                {
                    _hero.AssessDamage(atomBomb.CenterPoint);
                    _listToDeleteAtomBombs.Add(atomBomb);
                }
            }
            foreach (var atomBomb in _listToDeleteAtomBombs)
            {
                _listOfAtomBombs.Remove(atomBomb);
                atomBomb.Dispose();
            }
            _listToDeleteAtomBombs.Clear();

            // Presents.
            foreach (var present in _listOfPresents)
            {
                present.Move();
                if(present.IsDead)
                    _listToDeletePresents.Add(present);
                if(present.OutOfScreen)
                    _listToDeletePresents.Add(present);
            }
            foreach (var present in _listToDeletePresents)
            {
                _listOfPresents.Remove(present);
                present.Dispose();
            }
            _listToDeletePresents.Clear();
            #endregion
            #region CollisionDetection

            foreach (var bullet in _listOfBullets)
            {
                if (!bullet.Collision(Width, Height))
                {
                    _hero.Collision(bullet);
                    foreach (var plane in _listOfPlanes)
                    {
                        plane.Collision(bullet);
                    }
                    foreach (var present in _listOfPresents)
                    {
                        present.Collision(bullet);
                    }
                }
            }
            foreach (var missile in _listOfMissiles)
            {
                if (!missile.Collision(Width, Height))
                {
                    _hero.Collision(missile);
                    foreach (var plane in _listOfPlanes)
                    {
                        plane.Collision(missile);
                    }
                    foreach (var present in _listOfPresents)
                    {
                        present.Collision(missile);
                    }
                }
            }
            foreach (var present in _listOfPresents)
            {
                if(present.PickUpPresent((int) _hero.CenterPoint.X,(int) _hero.CenterPoint.Y))
                    _hero.PickedUpPresent(present);
            }
            #endregion
        }

        private void RenderScene()
        {
            _backgroundLoop.Draw(_imageGraphics);
            foreach (var plane in _listOfPlanes)
            {
                plane.Draw(_imageGraphics);
            }
            foreach (var bullet in _listOfBullets)
            {
                bullet.Draw(_imageGraphics);
            }
            foreach (var missile in _listOfMissiles)
            {
                missile.Draw(_imageGraphics);
            }
            foreach (var atomBomb in _listOfAtomBombs)
            {
                atomBomb.Draw(_imageGraphics);
            }
            foreach (var present in _listOfPresents)
            {
                present.Draw(_imageGraphics);
            }
            _hero.Draw(_imageGraphics);

            TextRenderer.DrawText(_imageGraphics, "GameTime : "
                + _gameTimer.ElapsedMilliseconds / 1000 + " s",
                    new Font("Century Gothic", 12.25f),
                        new Point(50, 50), Color.Black, Color.Transparent);

            TextRenderer.DrawText(_imageGraphics, "Score: " + Score, new Font("Century Gothic",12.25f), 
                new Point(50, 75), Color.Black);

            #region
#if myDebug
            framesRendered++;
                        if (Environment.TickCount >= startTimeRender + 1000)
                        {
                            label.Text = string.Format("Frames Rendered : {0}  fps", framesRendered);
                            framesRendered = 0;
                            startTimeRender = Environment.TickCount;
                        }
                            
                #endif
            #endregion
            BackgroundImage = _backBuffer;
            Invalidate();
        }

        private void GenerateDifficultyValues(int difficulty )
        {
            if (difficulty == 1)
            {
                _minPlaneTimeSpawn = 4000;
                _maxPlaneTimeSpawn = 9000;
                _minAtomBombTimeSpawn = 2500;
                _maxAtomBombTimeSpawn = 4000;
                _minBulletTimeSpawn = 1000;
                _maxBulletTimeSpawn = 2500;
                _planeDropPresent = 1;
            }
            else if (difficulty == 2)
            {
                _minPlaneTimeSpawn = 3000;
                _maxPlaneTimeSpawn = 7000;
                _minAtomBombTimeSpawn = 1500;
                _maxAtomBombTimeSpawn = 3000;
                _minBulletTimeSpawn = 750;
                _maxBulletTimeSpawn = 2000;
                _planeDropPresent = 3;
            }
            else
            {
                _minPlaneTimeSpawn = 2000;
                _maxPlaneTimeSpawn = 5500;
                _minAtomBombTimeSpawn = 1200;
                _maxAtomBombTimeSpawn = 2500;
                _minBulletTimeSpawn = 600;
                _maxBulletTimeSpawn = 1500;
                _planeDropPresent = 4;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameActive)
            {
                if (e.KeyCode == Keys.R){_hero.Reload();}
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) { _hero.Left = true;}
                if(e.KeyCode == Keys.D || e.KeyCode == Keys.Right) { _hero.Right = true;}
                if (!_hero.Jump)
                {
                    if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                    {
                        _hero.Jump = true;
                        _hero.Force = _hero.G;
                    }
                }
            }
            if (e.KeyCode == Keys.P)
            {
                if (_gameActive)
                {
                    _gameTimer.Stop();
                    _planeTimer.Stop();
                    if(_hero.ReloadingBullets)
                        _hero.BulletReloadTimer.Stop();
                    if(_hero.ReloadingMissiles)
                        _hero.MissileReloadTimer.Stop();
                    foreach (var plane in _listOfPlanes)
                    {
                        plane.AtomBombTimer.Stop();
                        plane.BulletTimer.Stop();
                    }
                    foreach (var present in _listOfPresents)
                    {
                        present.PresentTimer.Stop();
                    }
                }
                else
                {
                    _gameTimer.Continue();
                    _planeTimer.Continue();
                    if (_hero.ReloadingBullets)
                        _hero.BulletReloadTimer.Continue();
                    if (_hero.ReloadingMissiles)
                        _hero.MissileReloadTimer.Continue();

                    foreach (var plane in _listOfPlanes)
                    {
                        plane.AtomBombTimer.Continue();
                        plane.BulletTimer.Continue();
                    }
                    foreach (var present in _listOfPresents)
                    {
                        present.PresentTimer.Continue();
                    }
                }
                _gameActive = !_gameActive;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A || e.KeyCode == Keys.Left) { _hero.Left = false;}
            if(e.KeyCode == Keys.D || e.KeyCode == Keys.Right) { _hero.Right = false;}
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            _hero.MouseLocation = e.Location;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _hero.FireBurst(e.Location);
            }
            if (e.Button == MouseButtons.Right)
            {
                _hero.FireMissile(e.Location);
            }
        }
    }
}