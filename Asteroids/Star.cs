using Asteroids.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size){}

        public override void Draw()
        {
            //base.Draw();
            Game.Buffer.Graphics.DrawImage (Resources.star2, Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(Resources.star3, Pos.X + 50, Pos.Y + 50, Size.Width, Size.Height);
        }

        public override void Update()
        {
            //base.Update();

            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;

            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;

            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;

        }

    }
}
