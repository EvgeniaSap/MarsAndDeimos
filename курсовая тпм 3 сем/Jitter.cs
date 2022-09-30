using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GLTools
{
    class Jitter
    {
        public struct PointPloat
        {
            public float X;
            public float Y;
        }

        public static PointPloat[] j2 = new PointPloat[2]
            {
                new PointPloat{X=  0.246490f,Y=  0.249999f},
                new PointPloat{X=  0.246490f,Y=  0.249999f}
            };
        public static PointPloat[] j3 = new PointPloat[3]
            {
                new PointPloat{X= -0.373411f, Y= -0.250550f},
                new PointPloat{X=  0.256263f, Y=  0.368119f},
                new PointPloat{X=  0.117148f, Y= -0.117570f}
            };

        public static PointPloat[] j4 = new PointPloat[4]
            {
                new PointPloat{X= -0.208147f, Y=  0.353730f},
                new PointPloat{X=  0.203849f, Y= -0.353780f},
                new PointPloat{X= -0.292626f, Y= -0.149945f},
                new PointPloat{X=  0.296924f, Y=  0.149994f}
            };
        public static PointPloat[] j8 = new PointPloat[8]
              {
                new PointPloat{X= -0.334818f, Y=  0.435331f},
                new PointPloat{X=  0.286438f, Y= -0.393495f},
                new PointPloat{X=  0.459462f, Y=  0.141540f},
                new PointPloat{X= -0.414498f, Y= -0.192829f},
                new PointPloat{X= -0.183790f, Y=  0.082102f},
                new PointPloat{X= -0.079263f, Y= -0.317383f},
                new PointPloat{X=  0.102254f, Y=  0.299133f},
                new PointPloat{X=  0.164216f, Y= -0.054399f}
              };

        public static void AccFrustum(double left, double right, double bottom, double top,
            double anear, double afar, double pdx, double pdy)
        {

            double xwsize, ywsize, dx, dy;
            int[] viewport = new int[4];

            GL.GetInteger(GetPName.Viewport, viewport);
            xwsize = right - left;
            ywsize = top - bottom;
            dx = -(pdx * xwsize / viewport[2]);
            dy = -(pdy * ywsize / viewport[3]);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(left + dx, right + dx, bottom + dy, top + dy, anear, afar);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public static void AccOrtho(double left, double right, double bottom, double top,
            double anear, double afar, double pdx, double pdy)
        {
            double xwsize, ywsize, dx, dy;
            int[] viewport = new int[4];

            GL.GetInteger(GetPName.Viewport, viewport);
            xwsize = right - left;
            ywsize = top - bottom;
            dx = -(pdx * xwsize / viewport[2]);
            dy = -(pdy * ywsize / viewport[3]);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(left + dx, right + dx, bottom + dy, top + dy, anear, afar);
            GL.MatrixMode(MatrixMode.Modelview);
        }

    }
}

