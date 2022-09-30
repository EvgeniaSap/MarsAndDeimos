using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using OpenTK.Input;
using GLTools;

namespace курсовая_тпм_3_сем
{
    public partial class Form1 : Form
     {
         bool pressed = false;
         double AngleX = 0;
         double AngleY = 0;
         double AngleZ = 0;

         private int texture; // переменная для хранения GL.GenTexture();
         Timer timer;

         double fi = 3;//скорость вокруг оси марса
         double fi2 = 3;//скорость вокруг оси деймоса
         const double Speed_Deimos = 0.006;//скорость движения по орбите деймоса
         const double Speed_Mars = 0.015;//скорость движения по орбите марса

         const double Step = 0.2;   // шаг перемещения наблюдателя в лабиринте
         const double AngleDl = 5;

         // координаты положения наблюдателя в лабиринте
         PointFloat Pos;
         Bitmap bmpMars;
         Bitmap bmpDeimos;
         Bitmap bmpStar;
         Bitmap bmpMe;

        // координаты углов лабиринта
        public struct PointFloat
        {
            public double X;
            public double Y;
            public double Z;
        }
    
      public Form1()
      {
          InitializeComponent();
      }

      private void Form1_Load(object sender, EventArgs e)
      {

      }

      private void movesatellite(object sender, EventArgs e)
      {
          fi += Speed_Deimos;
          fi2 += Speed_Mars;
          glControl1.Invalidate();
      }

      private void glControl1_Paint(object sender, PaintEventArgs e)
      {
          GL.ClearColor(0.5f, 0.5f, 0.75f, 1.0f); // цвет фона
          // очистка буферов цвета и глубины
          GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
          // поворот изображения
          GL.LoadIdentity();
          GL.Rotate(AngleY, 0.0, 1.0, 0.0);
          GL.Translate(Pos.X, Pos.Z, Pos.Y);

          // Пространство
          GLTexture.LoadTexture(bmpStar);
          GL.Enable(EnableCap.Texture2D);
          GL.PushMatrix();
          Sphere(20, 100, 100, 0, 0, 0, false);
          GL.PopMatrix();
          GL.Disable(EnableCap.Lighting);
          GL.Disable(EnableCap.Texture2D);

          // Марс
          GLTexture.LoadTexture(bmpMars);
          GL.Enable(EnableCap.Texture2D);
          GL.Enable(EnableCap.Lighting);
          GL.Rotate(-50, 1.0, 1.0, 1.0);
          Sphere(2, 100, 100, 0, 0, 0, true);
          GL.PopMatrix();
          GL.Disable(EnableCap.Lighting);
          GL.Disable(EnableCap.Texture2D);

          // Деймос
          GLTexture.LoadTexture(bmpDeimos);
          GL.Enable(EnableCap.Texture2D);
          GL.Enable(EnableCap.Lighting);
          GL.Rotate(-45, 1.0, -1.0, 1.0);
           Sphere(0.6, 15, 15, Math.Sin(fi) * 8, Math.Cos(fi) * 8, 0, true); // радиус,растяжение объекта по x,y,траектория движения,вращение вокруг своей оси
          GL.PopMatrix();
          GL.Disable(EnableCap.Lighting);
          GL.Disable(EnableCap.Texture2D);

          // имя студента, выполневшего курсовую
          GLTexture.LoadTexture(bmpMe);
          GL.Enable(EnableCap.Texture2D);
          GL.PushMatrix();
          GL.Rotate(50.0, 30.0, 15.0, -7.0);
          GL.Translate(2, 1.5, 4);
          GL.Begin(PrimitiveType.QuadStrip);

          GL.TexCoord2(0, 1);
          GL.Vertex3(-0.25, 0.2, 3);

          GL.TexCoord2(1, 1);
          GL.Vertex3(-0.25, 1.2, 3);

          GL.TexCoord2(0, 0);
          GL.Vertex3(0.25, 0.2, 3);

          GL.TexCoord2(1, 0);
          GL.Vertex3(0.25, 1.2, 3);
          GL.End();

     GL.PopMatrix();
     GL.Disable(EnableCap.Texture2D);


     GL.Flush();
     GL.Finish();

     glControl1.SwapBuffers();
 }

 private void glControl1_Load(object sender, EventArgs e)
 {
     GL.ClearColor(1f, 1f, 1f, 1f); // цвет фона
     GL.Enable(EnableCap.DepthTest);
     GL.Enable(EnableCap.Light0); // освещенность

     bmpMars = new Bitmap("Mars1.bmp");
     bmpDeimos = new Bitmap("Deimos1.bmp");
     bmpStar = new Bitmap("Star1.bmp");
     bmpMe = new Bitmap("Myname1.bmp");

     texture = GL.GenTexture();
     GL.BindTexture(TextureTarget.ProxyTexture2D, texture);

     GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
     GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

     GL.Enable(EnableCap.Light0);

     Pos.Y = -8;

     timer = new Timer();
     timer.Interval = 1;
     timer.Tick += movesatellite;
     timer.Enabled = true;
 }

 private void glControl1_KeyDown(object sender, KeyEventArgs e)
 {
     switch (e.KeyCode)
     {
         case Keys.Left: AngleY += -AngleDl; break;
         case Keys.Right: AngleY -= -AngleDl; break;
         case Keys.Up:
             Pos.X = Pos.X - Step * Math.Sin(AngleY * Math.PI / 180);
             Pos.Y = Pos.Y + Step * Math.Cos(AngleY * Math.PI / 180);
             break;
         case Keys.Down:
             Pos.X = Pos.X + Step * Math.Sin(AngleY * Math.PI / 180);
             Pos.Y = Pos.Y - Step * Math.Cos(AngleY * Math.PI / 180);
             break;
     }
     if (AngleY >= 360) AngleY -= 360;
     glControl1.Invalidate();
 }

 // изображение сферы
 void Sphere(double r, int nx, int ny, double sx, double sy, double sz, bool rotate_texture = true)
 {
     int ix, iy;
     double x, y, z, tex_x, tex_y;


     for (iy = 0; iy < ny; ++iy)
     {
         tex_y = (double)iy / (double)ny;

         GL.Begin(PrimitiveType.QuadStrip);
         for (ix = 0; ix <= nx; ++ix)
         {
             tex_x = (double)ix / (double)nx + (rotate_texture ? fi - Math.Floor(fi) : 0);

             x = r * Math.Sin(iy * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx) + sx;
             y = r * Math.Sin(iy * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx) + sy;
             z = r * Math.Cos(iy * Math.PI / ny) + sz;
             GL.Normal3(x, y, z);//нормаль направлена от центра
             GL.TexCoord2(tex_x, tex_y);
             GL.Vertex3(x, y, z);

             x = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx) + sx;
             y = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx) + sy;
             z = r * Math.Cos((iy + 1) * Math.PI / ny) + sz;
             GL.Normal3(x, y, z);
             GL.TexCoord2(tex_x, tex_y + 1.0 / (double)ny);
             GL.Vertex3(x, y, z);
         }
         GL.End();
     }
 }

 private void glControl1_Resize(object sender, EventArgs e)
 {
     SetupViewport();

     GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

     GL.MatrixMode(MatrixMode.Projection);
     GL.LoadIdentity();
     GL.Frustum(-0.5, 0.5, -0.5, 0.5, 0.5, 50);
     GL.MatrixMode(MatrixMode.Modelview);
     glControl1.Invalidate();
 }

 private void SetupViewport()
 {
     int w = glControl1.Width;
     int h = glControl1.Height;

     GL.MatrixMode(MatrixMode.Projection);//Матрица проекции
     GL.LoadIdentity();//Для замены текущей матрицы  на единичную матрицу используется команда
     GL.Ortho(-1, 1, -1, 1, -1, 1);//для формирования матрицы параллельной проекции
                                   //левая и правая границы области видимости вдоль оси OX;-1, 1
                                   //нижняя и верхняя границы области видимости вдоль оси OY-1, 1
                                   //ближняя и дальняя границы области видимости вдоль оси OZ.  -1, 1

     GL.MatrixMode(MatrixMode.Modelview);//Видовая матрица

     GL.Viewport(0, 0, w, h); //Координаты области вывода
                              //x, y – координаты верхнего левого угла области вывода
                              //w, h – ширина и высота области вывода

 }

 private void glControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
 {
     switch (e.KeyCode)
     {
         case Keys.S:
             AngleZ -= AngleDl;
             break;

         case Keys.Left:
             AngleY += -AngleDl;
             break;
         case Keys.Right:
             AngleY -= -AngleDl;
             break;
         case Keys.Up:
             Pos.X = Pos.X - Step * Math.Sin(AngleY * Math.PI / 180);
             Pos.Y = Pos.Y + Step * Math.Cos(AngleY * Math.PI / 180);
             break;
         case Keys.Down:
             Pos.X = Pos.X + Step * Math.Sin(AngleY * Math.PI / 180);
             Pos.Y = Pos.Y - Step * Math.Cos(AngleY * Math.PI / 180);
             break;
         case Keys.Insert:
             Pos.Z = Pos.Z + Step;
             break;
         case Keys.End:
             Pos.Z = Pos.Z - Step;
             break;
     }

     if (AngleY >= 360)
         AngleY -= 360;

     glControl1.Invalidate();

 }
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2(); // создание Form2
            newForm.Show(); // открытие Form2
        }
    }
}
