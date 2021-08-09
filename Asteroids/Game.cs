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
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        static BaseObject[] _asteroids;
        static BaseObject[] _stars;
        static Bullet _bullet;
        static Ship _ship;
        static Timer timer;

        public static int Width { get; set; }
        public static int Height { get; set; }
        static Random random = new Random();

        public static void Init(Form form)
        {
            Graphics g = form.CreateGraphics();
            _context = BufferedGraphicsManager.Current;
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Load();

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += OnTime;
            timer.Start();

            form.KeyDown += Form_KeyDown;

        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                if(_bullet == null)
                    _bullet = new Bullet(new Point(_ship.Rect.X +10, _ship.Rect.Y +20), new Point(5, 0), new Size(36, 10));
            }
            if (e.KeyCode == Keys.Up)
            {
                _ship.Up();
            }
            if (e.KeyCode == Keys.Down)
            {
                _ship.Down();
            }
        }

        private static void OnTime(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawImage(Properties.Resources.ski, new Rectangle(0, 0, Width, Height));
            Buffer.Graphics.DrawImage(Properties.Resources.planet, new Rectangle(500, 70, 200, 200));


            foreach (var asteroid in _asteroids)
                if (asteroid != null)
                    asteroid.Draw();

            foreach (BaseObject star in _stars)
                star.Draw();
            
            if(_bullet != null)
                _bullet.Draw();

            _ship.Draw();
            Buffer.Graphics.DrawString("Energy: " + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);

            Buffer.Render();
        }

        public static void Load()
        {
            var random = new Random();

             _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(45, 50));
            Ship.DieEvent += Ship_DieEvent;

            //_bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(44, 10));

            _asteroids = new BaseObject[18];
            for (int i = 0; i < _asteroids.Length; i++)
            {
                var size = random.Next(15, 40);
                _asteroids[i] = new Asteroid(new Point(0, i * 10 + 5), new Point(-i - 2, -i - 2), new Size(size, size));
            }

            _stars = new BaseObject[20];
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new Star(new Point(600, i * 15 + 1), new Point(-i - 1, -i - 1), new Size(10, 10));
            }

            
        }

        private static void Ship_DieEvent(object sender, EventArgs e)
        {
            timer.Stop();
            Buffer.Graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }

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

        public static void Update()
        {

            for (int i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null) continue;

                _asteroids[i].Update();

                if (_bullet != null && _asteroids[i].Collision(_bullet))
                {
                    System.Media.SystemSounds.Hand.Play();
                    Debug.WriteLine($"{i} -> X:{_asteroids[i].Rect.X} Y:{_asteroids[i].Rect.Y}");
                    _asteroids[i] = null;
                    _bullet = null;
                    continue;
                }

                if (!_ship.Collision(_asteroids[i])) continue;



                _ship.EnergyLow(random.Next(1, _asteroids[i].Rect.Width / 2));
                System.Media.SystemSounds.Asterisk.Play();

                if (_ship.Energy <= 0)
                    _ship.Die();


            }



            foreach (var obj in _stars)
                obj.Update();

            if (_bullet != null) 
            {
                _bullet.Update();
                if (_bullet.Rect.X > Width)
                    _bullet = null;
            }
                
        }

        

    }
}
