using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Emgu.CV;
using Microsoft.VisualBasic.CompilerServices;
using ShortGenerator.Error;
using Utils = ShortGenerator.Util.Utils;

namespace ShortGenerator.Video
{
    public class FrameVideo
    {
        public string File { get; }
        public int Frames { get; }
        public int Fps { get; }
        public int Width { get; }
        public int Height { get; }
        private Bitmap[] FrameImages { get; }
        
        public FrameVideo(string file, int frames = 3600, int fps = 60, int width = 1080, int height = 1920)
        {
            if (!System.IO.File.Exists(file)) throw new VideoLoadException(file);
            
            File = file;
            Frames = frames;
            Fps = fps;
            Width = width;
            Height = height;

            FrameImages = new Bitmap[Frames];

            using var video = new VideoCapture(file);
            using var img = new Mat();
            int frame = 0;
            while (video.Grab() && frame < Frames)
            {
                video.Retrieve(img);
                FrameImages[frame] = img.ToBitmap();
                frame++;
            }

            Frames = frame;
        }

        public void Clear(Color color, int start = 0, int end = -1)
        {
            if (end == -1 || end > Frames) end = Frames;
            for (int i = start; i < end; i++)
            {
                using var graphics = Graphics.FromImage(FrameImages[i]);
                graphics.Clear(color);
            }
        }
        
        public void Rectangle(Color color, int x, int y, int width, int height, int start = 0, int end = -1, bool centered = true)
        {
            if (end == -1 || end > Frames) end = Frames;
            if (centered)
            {
                x -= width / 2;
                y -= height / 2;
            }

            Brush brush = new SolidBrush(color);
            for (int i = start; i < end; i++)
            {
                using var graphics = Graphics.FromImage(FrameImages[i]);
                graphics.FillRectangle(brush, x, y, width, height);
            }
        }
        
        public void RectangleRounded(Color color, int x, int y, int width, int height, int radius, int start = 0, int end = -1, bool centered = true)
        {
            if (end == -1 || end > Frames) end = Frames;
            if (centered)
            {
                x -= width / 2;
                y -= height / 2;
            }
            
            var path = new GraphicsPath();
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(new Point(x, y), size);
            path.AddArc(arc, 180, 90);
            arc.X = x + width - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = y + height - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = x;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            
            Brush brush = new SolidBrush(color);
            for (int i = start; i < end; i++)
            {
                using var graphics = Graphics.FromImage(FrameImages[i]);
                graphics.FillPath(brush, path);
            }
        }
        
        public void Text(string text, FontFamily fontFamily, FontStyle style, int size, int x, int y, Color color, int start = 0, int end = -1, bool centered = true)
        {
            if (end == -1 || end > Frames) end = Frames;
            Brush brush = new SolidBrush(color);
            Font font = new Font(fontFamily, size, style, GraphicsUnit.Pixel);
            for (int i = start; i < end; i++)
            {
                using var graphics = Graphics.FromImage(FrameImages[i]);
                var tx = x;
                var ty = y;
                if (centered)
                {
                    var ms = graphics.MeasureString(text, font);
                    tx -= (int)ms.Width / 2;
                    ty -= (int)ms.Height / 2;
                }
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.DrawString(text, font, brush, tx, ty);
            }
        }
        
        public void OutlineText(string text, FontFamily fontFamily, FontStyle style, int size, int x, int y, Color color, float outlineWidth, Color outlineColor, int start = 0, int end = -1, bool centered = true)
        {
            if (end == -1 || end > Frames) end = Frames;
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(outlineColor, outlineWidth);
            Font font = new Font(fontFamily, size, style, GraphicsUnit.Pixel);
            for (int i = start; i < end; i++)
            {
                using var graphics = Graphics.FromImage(FrameImages[i]);
                var tx = x;
                var ty = y;
                if (centered)
                {
                    var ms = graphics.MeasureString(text, font);
                    tx -= (int)ms.Width / 2;
                    ty -= (int)ms.Height / 2;
                }
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                GraphicsPath path = new GraphicsPath();
                path.AddString(text, fontFamily, (int) style, size, new Point(tx, ty), new StringFormat());
                graphics.FillPath(brush, path);
                graphics.DrawPath(pen, path);
            }
        }

        public SizeF MeasureText(string text, FontFamily fontFamily, FontStyle style, int size)
        {
            using var graphics = Graphics.FromImage(FrameImages[0]);
            Font font = new Font(fontFamily, size, style, GraphicsUnit.Pixel);
            return graphics.MeasureString(text, font);
        }

        public void WriteToFile(string path)
        {
            var codec = VideoWriter.Fourcc('a', 'v', 'c', '1');
            using var videoWriter = new VideoWriter(path, codec, Fps, new Size(Width, Height), true);
            for (int i = 0; i < 7; i++)
            {
                Utils.ClearLine();
            }
            for (int i = 0; i < Frames; i++)
            {
                using var mat = FrameImages[i].ToMat();
                videoWriter.Write(mat);
            }
        }
    }
}