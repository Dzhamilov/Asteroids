using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Asteroid : BaseObject
    {

        private int index;
        Random r = new Random();
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

            index = r.Next(1, 5);
        }
        public override void Draw()
        {
            switch (index)
            {
                case 1:
                    Game.Buffer.Graphics.DrawImage(Properties.Resources.asteroid01, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
                case 2:
                    Game.Buffer.Graphics.DrawImage(Properties.Resources.asteroid02, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
                case 3:
                    Game.Buffer.Graphics.DrawImage(Properties.Resources.asteroid03, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
                case 4:
                    Game.Buffer.Graphics.DrawImage(Properties.Resources.asteroid04, Pos.X, Pos.Y, Size.Width, Size.Height);
                    break;
            }
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;

            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;

            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }
    }
}
