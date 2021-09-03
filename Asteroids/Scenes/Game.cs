using Asteroids.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids
{
    public class Game : BaseScene
    {
        //private static BufferedGraphicsContext _context;
        //public static BufferedGraphics Buffer;

        private List<BaseObject> _asteroids = new List<BaseObject>();
        private BaseObject[] _stars;
        private List<Bullet> _bullets = new List<Bullet>();
        private Ship _ship;
        private Timer _timer;
        private Random random = new Random();


        public override void Init(Form form)
        {
            base.Init(form);

            Load();

            _timer = new Timer { Interval = 100 };
            _timer.Start();
            _timer.Tick += Timer_Tick;
            Ship.DieEvent += Finish;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public override void SceneKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 21), new Point(5, 0), new Size(54, 9)));
            }
            if (e.KeyCode == Keys.Up)
            {
                _ship.Up();
            }
            if (e.KeyCode == Keys.Down)
            {
                _ship.Down();
            }
            if (e.KeyCode == Keys.Back)
            {
                SceneManager
                    .Get()
                    .Init<MenuScene>(_form)
                    .Draw();
            }
        }

        //private static void OnTime(object sender, EventArgs e)
        //{
        //    Draw();
        //    Update();
        //}

        public override void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawImage(Properties.Resources.ski, new Rectangle(0, 0, Width, Height));
            Buffer.Graphics.DrawImage(Properties.Resources.planet, new Rectangle(500, 70, 200, 200));


            foreach (var asteroid in _asteroids)
                    asteroid.Draw();

            foreach (var obj in _stars)
                obj.Draw();

            foreach(var boolet in _bullets)
                boolet.Draw();


            if (_ship != null)
            {
                _ship.Draw();
                Buffer.Graphics.DrawString($"Energy: {_ship.Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
            }
            Buffer.Render();
        }

        public void Load()
        {
            

             _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(45, 50));
            Ship.DieEvent += Finish;

            //_bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(44, 10));
            
            var random = new Random();
           // _asteroids = new BaseObject[15];
            for (int i = 0; i < 15; i++)
            {
                var size = random.Next(15, 40);
                //_asteroids[i] = new Asteroid(new Point(0, i * 20), new Point(-i, -i), new Size(size, size));
                _asteroids.Add(new Asteroid(new Point(0, i * 20), new Point(-i, -i), new Size(size, size)));

            }

            _stars = new Star[20];
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new Star(new Point(600, i * 40), new Point(-i, -i), new Size(7, 7));
            }

            
        }

        //private static void Ship_DieEvent(object sender, EventArgs e)
        //{
        //    timer.Stop();
        //    Buffer.Graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
        //    Buffer.Render();
        //}

        //public static void Update()
        //{
        //    foreach (BaseObject asteroid in _asteroids) 
        //    {
        //        asteroid.Update();
        //        if (_bullet != null && asteroid.Collision(_bullet))
        //        {
        //            System.Media.SystemSounds.Hand.Play();
        //            Debug.WriteLine("Пересечении астероида с пулей");
        //        }
        //    }




        //    foreach (BaseObject star in _stars)
        //        star.Update();

        //    if (_bullet != null)
        //        _bullet.Update();
        //}

        public void Update()
        {
            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                if (_asteroids[i].Collision(_ship)) 
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    _ship.EnergyLow(random.Next(1, 10));
                    _asteroids.RemoveAt(i);
                    if (_ship.Energy <= 0)
                      _ship.Die();
                }
                else 
                {
                    for (int j = _bullets.Count - 1; j >= 0; j--) 
                    {
                        if (_asteroids[i].Collision(_bullets[j])) 
                        {
                            System.Media.SystemSounds.Hand.Play();
                            _asteroids.RemoveAt(i);
                            _bullets.RemoveAt(j);
                            break;
                        }
                    }


                }
            }
            

            //for (int i = 0; i < _asteroids.Length; i++)
            //{
            //    if (_asteroids[i] == null) continue;

            //    _asteroids[i].Update();

            //    if (_bullet != null && _bullet.Collision(_asteroids[i]))
            //    {
            //        System.Media.SystemSounds.Hand.Play();
            //        Debug.WriteLine($"{i} -> X:{_asteroids[i].Rect.X} Y:{_asteroids[i].Rect.Y}");
            //        _asteroids[i] = null;
            //        _bullet = null;
            //        continue;
            //    }

            //    if (_ship != null && _ship.Collision(_asteroids[i]))
            //    {
            //        _ship.EnergyLow(random.Next(1, 10));
            //        System.Media.SystemSounds.Asterisk.Play();

            //        if (_ship.Energy <= 0)
            //            _ship.Die();
            //    }

                //_ship.EnergyLow(random.Next(1, _asteroids[i].Rect.Width / 2));
                


            //}



            foreach (var obj in _stars)
                obj.Update();

            foreach (var asteroid in _asteroids)
                asteroid.Update();

            foreach (var boolet in _bullets)
                boolet.Update();

            //if (_bullet != null) 
            //{
            //    _bullet.Update();
            //    //if (_bullet.Rect.X > Width)
            //    //    _bullet = null;
            //}

        }

        private void Finish(object sender, EventArgs e)
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Bold), Brushes.White, 180, 100);
            Buffer.Graphics.DrawString("<Backspace> - в меню", new Font(FontFamily.GenericSansSerif, 40, FontStyle.Underline), Brushes.White, 80, 200);
            Buffer.Render();
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer.Stop();
        }

    }
}
