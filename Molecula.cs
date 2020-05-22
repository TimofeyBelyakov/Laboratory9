using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrownianMotion
{
    public class Molecula : Form
    {
        public PictureBox square;
        public double x;
        public double y;
        public int angle;
        public int speed;

        public Molecula() { }

        public Molecula(PictureBox square, double x, double y, int angle, int speed)
        {
            this.square = square;
            this.x = x;
            this.y = y;
            this.angle = angle;
            this.speed = speed;
        }
    }
}
