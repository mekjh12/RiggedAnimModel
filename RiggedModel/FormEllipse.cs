using Assimp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.TextFormatting;

namespace LSystem
{
    public partial class FormEllipse : Form
    {
        public FormEllipse()
        {
            InitializeComponent();
        }

        public Random random = new Random();

        private void FormEllipse_Load(object sender, EventArgs e)
        {

        
        }

        public void DrawPoint(Graphics g, float x, float y, float width, Color color)
        {
            g.DrawEllipse(new Pen(color, width), this.Width * 0.5f + x, this.Height * 0.5f - y, 1, 1);
        }

        public void DrawLine(Graphics g, float x1, float y1, float x2, float y2, float width, Color color)
        {
            g.DrawLine(new Pen(color, width), this.Width * 0.5f + x1, this.Height * 0.5f - y1, this.Width * 0.5f + x2, this.Height * 0.5f - y2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.Clear(Color.Black);
            float RADIAN = 3.141502f / 180.0f;

            float a = random.Next(1, 200); ;
            float b = random.Next(1, 200); ;

            float i = 0.0f;
            float j = 0.0f;
            int num = 0;
            while (num<100)
            {
                i = random.Next(-300, 300);
                j = random.Next(-200, 200);
                if (i * i / (a * a) + j * j / (b * b) > 1) break;
                num++;
            }

            Console.WriteLine($"{num} target=({i},{j})");
            DrawPoint(g, i, j, 3, Color.Red);

            for (int tt = 0; tt < 360; tt++)
            {
                DrawPoint(g, (float)(a * Math.Cos(tt * RADIAN)), (float)(b * Math.Sin(tt * RADIAN)), 1, Color.Red);
            }

            float theta1 = 0.0f;
            float theta2 = 90.0f;
            float epsilon = 0.1f;
            int iter = 0;

            if (i > 0 && j >= 0) { theta1 = 0.0f; theta2 = 90.0f; }
            if (i <= 0 && j > 0) { theta1 = 90.0f; theta2 = 180.0f; }
            if (i < 0 && j <= 0) { theta1 = 180.0f; theta2 = 270.0f; }
            if (i >= 0 && j < 0) { theta1 = 270.0f; theta2 = 360.0f; }

            while (Math.Abs(theta1 - theta2) > epsilon && iter <12)
            {
                float theta0 = (theta1 + theta2) * 0.5f;
                float d1 = Dot(theta1);
                float d2 = Dot(theta2);
                float d0 = Dot(theta0);
                if (d1 * d0 < 0)
                {
                    theta2 = theta0;
                }
                else if (d2 * d0 < 0)
                {
                    theta1 = theta0;
                }
                else if (d1 * d2 == 0)
                {
                    break;
                }
                iter++;
            }

            DrawPoint(g, (float)(a * Math.Cos(theta1 * RADIAN)), (float)(b * Math.Sin(theta1 * RADIAN)), 3, Color.Yellow);

            float Dot(float t)
            {
                //float RADIAN = 3.141502f / 180.0f;
                float c = (float)Math.Cos(t * RADIAN);
                float s = (float)Math.Sin(t * RADIAN);
                float dot = (j - b * s) * b * c - a * s * (i - a * c);
                float xn = (float)Math.Sqrt((i - a * c) * (i - a * c) + (j - b * s) * (j - b * s));
                float yn = (float)Math.Sqrt(a * a * s * s + b * b * c * c);
                dot = dot / (xn * yn);
                return dot;
            }
        }
    }
}
