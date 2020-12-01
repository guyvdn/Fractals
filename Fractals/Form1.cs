using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals
{
    public partial class Form1 : Form
    {
        private int _zoom = 1;
        private Area _visibleArea;
        private const int ImageWidth = 1600;
        private const int ImageHeight = 1200;
        private readonly object _lockObject = new object();

        public Form1()
        {
            InitializeComponent();
            _visibleArea = new Area(-2.5, 1.5, -1.5, 1.5);
            DrawImage();
        }

        private void DrawImage()
        {
            Text = $"Fractals - Mandelbrot - Zoom {_zoom}";

            tableLayoutPanel1.ColumnStyles[1].Width = ImageWidth;
            tableLayoutPanel1.RowStyles[1].Height = ImageHeight;

            var bitmap = new Bitmap(ImageWidth, ImageHeight);

            var visibleWidth = _visibleArea.Width;
            var visibleHeight = _visibleArea.Height;
            var maxIterations = 100* _zoom;

            Parallel.For(0, ImageWidth, x =>
            {
                Parallel.For(0, ImageHeight, y =>
                     {
                         var xScaled = (x * visibleWidth / ImageWidth) + _visibleArea.X1;
                         var yScaled = (y * visibleHeight / ImageHeight) + _visibleArea.Y1;

                         var value = Mandelbrot.GetValue(xScaled, yScaled, maxIterations);

                         if (value == maxIterations)
                             lock(_lockObject)
                             {
                                 bitmap.SetPixel(x, y, Color.Black);
                             }
                         else
                         {
                             var redColor = value * 255 / maxIterations;
                             var blueColor = redColor / 3;
                             var greenColor = redColor / 2;

                             lock(_lockObject)
                             {
                                 bitmap.SetPixel(x, y, Color.FromArgb(redColor, greenColor, blueColor));
                             }
                         }
                     });
            });

            pictureBox1.Image = bitmap;
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            if (e is not MouseEventArgs mouseEventArgs) 
                return;

            var visibleWidth = _visibleArea.Width;
            var visibleHeight = _visibleArea.Height;

            var newWidth = visibleWidth / 2;
            var newHeight = visibleHeight / 2;
            var newCenterX = _visibleArea.X1 + mouseEventArgs.X * visibleWidth / ImageWidth;
            var newCenterY = _visibleArea.Y1 + mouseEventArgs.Y * visibleHeight / ImageHeight;

            _visibleArea = new Area(newCenterX - newWidth / 2, newCenterX + newWidth / 2, newCenterY - newHeight / 2, newCenterY + newHeight / 2);
            _zoom++;

            DrawImage();
        }
    }
}
