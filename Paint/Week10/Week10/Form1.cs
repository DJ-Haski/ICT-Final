using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Week10
{

    

    enum Tool
    {
        Line,
        Rectangle,
        Pen,
        Fill,
        FastFill,
        Circle,
        Eraser,
        Arc,
        Triangle
    }
    public partial class Form1 : Form
    {
        Color currentColor = Color.Black;
        int currentWidth = 1;
        Bitmap bitmap = default(Bitmap);
        Graphics graphics = default(Graphics);
        Point prevPoint = default(Point);
        Point currentPoint = default(Point);
        bool isMousePressed = false;
        Tool currentTool = Tool.Pen;
        float startAngle = 0;
        float sweepAngle = 180;
        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pictureBox1.Image = bitmap;
            graphics.Clear(Color.White);
            button3.Select();

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            currentTool = Tool.Line;
        }
        Rectangle GetMRectangle(Point pPoint, Point cPoint)
        {
            return new Rectangle
            {
                X = Math.Min(pPoint.X, cPoint.X),
                Y = Math.Min(pPoint.Y, cPoint.Y),
                Width = Math.Abs(pPoint.X - cPoint.X),
                Height = Math.Abs(pPoint.Y - cPoint.Y)
            };
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Location.ToString();
            if (isMousePressed)
            {
                switch (currentTool)
                {
                    case Tool.Line:
                    case Tool.Rectangle:
                        currentPoint = e.Location;
                        break;
                    case Tool.Pen:
                        prevPoint = currentPoint;
                        currentPoint = e.Location;
                        graphics.DrawLine(new Pen(currentColor, currentWidth), prevPoint, currentPoint);
                        break;
                    case Tool.Circle:
                        currentPoint = e.Location;
                        break;
                    case Tool.Arc:
                        currentPoint = e.Location;
                        break;
                    case Tool.Triangle:
                        currentPoint = e.Location;
                        break;
                    case Tool.Eraser:
                        prevPoint = currentPoint;
                        currentPoint = e.Location;
                        graphics.DrawLine(new Pen(Color.White, 20), prevPoint, currentPoint);
                        break;
                    default:
                        break;
                }

                pictureBox1.Refresh();
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            prevPoint = e.Location;
            currentPoint = e.Location;
            isMousePressed = true;
            if(currentTool == Tool.Fill)
            {
                
                bitmap = Utils.Fill(bitmap, e.Location, bitmap.GetPixel(e.X, e.Y), currentColor);
                graphics = Graphics.FromImage(bitmap);
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();   

            } 
            if(currentTool == Tool.FastFill)
            {
                MapFill mf = new MapFill();
                mf.Fill(graphics, e.Location, currentColor, ref bitmap);
                graphics = Graphics.FromImage(bitmap);
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePressed = false;

            switch (currentTool)
            {
                case Tool.Line:
                    graphics.DrawLine(new Pen(currentColor, currentWidth), prevPoint, currentPoint);
                    break;
                case Tool.Rectangle:
                    graphics.DrawRectangle(new Pen(currentColor, currentWidth), GetMRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Pen:
                    break;
                case Tool.Circle:
                    graphics.DrawEllipse(new Pen(currentColor, currentWidth), GetMRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Arc:
                    graphics.DrawArc(new Pen(currentColor, currentWidth), GetMRectangle(prevPoint, currentPoint),startAngle,sweepAngle);
                    break;
                //case Tool.Triangle:
                //    graphics.Drawtriangle(new Pen(currentColor, currentWidth), GetMRectangle(prevPoint, currentPoint));
                //    break;
                case Tool.Eraser:
                    break;
                default:
                    break;
            }
            prevPoint = e.Location;
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            switch (currentTool)
            {
                case Tool.Line:
                    e.Graphics.DrawLine(new Pen(currentColor, currentWidth), prevPoint, currentPoint);
                    break;
                case Tool.Rectangle:
                    e.Graphics.DrawRectangle(new Pen(currentColor, currentWidth), GetMRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Pen:
                    break;
                case Tool.Arc:
                    e.Graphics.DrawRectangle(new Pen(currentColor, currentWidth), GetMRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Triangle:
                    break;
                case Tool.Circle:
                    e.Graphics.DrawEllipse(new Pen(currentColor, currentWidth), GetMRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Eraser:
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentTool = Tool.Rectangle;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentTool = Tool.Pen;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bitmap = Bitmap.FromFile(openFileDialog1.FileName) as Bitmap;
                pictureBox1.Image = bitmap;
                graphics = Graphics.FromImage(bitmap);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            currentTool = Tool.Fill;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentTool = Tool.FastFill;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                currentColor = colorDialog1.Color;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            currentTool = Tool.Circle;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            currentTool = Tool.Eraser;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            currentWidth = Convert.ToInt32(numericUpDown1.Value);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // pictureBox1.Image = null;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //pictureBox1.Image = bitmap;
            //pictureBox1.Invalidate();
            graphics.Clear(Color.White);
            pictureBox1.Image = bitmap;
            graphics = Graphics.FromImage(bitmap);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            currentTool = Tool.Arc;
        }
        private void button11_Click(object sender, EventArgs e)
        {

        }

    }
}
