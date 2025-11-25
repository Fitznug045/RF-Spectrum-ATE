using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SpectrumAnalyzerGUI
{
    public static class MessageBox
    {
        public enum ToastIcon { Info, Warning, Error, Success, None }

        public static async Task ShowAsync(
            string message,
            string caption = "",
            ToastIcon icon = ToastIcon.Info,
            int Timeout = 1500,
            Form parent = null,
            bool bottomRight = false)
        {
            int radius = 20;

            Font captionFont = new Font("Segoe UI", 9, FontStyle.Bold);
            Font msgFont = new Font("Segoe UI", 8);

            Size captionSize, msgSize;
            using (Bitmap bmp = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                captionSize = Size.Ceiling(g.MeasureString(caption, captionFont));
                msgSize = Size.Ceiling(g.MeasureString(message, msgFont));
            }

            int iconW = 48;
            int padding = 20;
            int spacing = 10;

            int textWidth = Math.Max(captionSize.Width, msgSize.Width);
            int boxWidth = iconW + spacing + textWidth + padding * 2;
            int boxHeight = padding * 2 + captionSize.Height + msgSize.Height + 10;

            if (boxHeight < 70) boxHeight = 70;
            if (boxWidth < 210) boxWidth = 210;

            int borderMargin = 4;  
            Form toast = new Form();
            toast.FormBorderStyle = FormBorderStyle.None;
            toast.ShowInTaskbar = false;
            toast.Owner = parent;
            toast.TopMost = parent?.TopMost ?? false;
            toast.Width = boxWidth + borderMargin * 2;
            toast.Height = boxHeight + borderMargin *2;
            toast.Opacity = 0;
            toast.BackColor = Color.White;
            toast.StartPosition = FormStartPosition.Manual;

            toast.Region = Region.FromHrgn(CreateRoundRectRgn(
                0, 0, toast.Width, toast.Height, radius, radius
            ));

            Point pos;
            if (parent != null)
            {
                Rectangle r = parent.Bounds;
                int x = r.Left + (r.Width - toast.Width) / 2;

                if (x + toast.Width > r.Right - 20)
                    x = r.Right - toast.Width - 20;

                if (x < r.Left + 20)
                    x = r.Left + 20;

                int y = r.Top + (r.Height - toast.Height) / 2;

                pos = new Point(x, y);
            }
            else
            {
                Rectangle r = Screen.PrimaryScreen.WorkingArea;
                pos = new Point(r.Right - toast.Width - 20, r.Bottom - toast.Height - 20);
            }

            toast.Location = pos;

            Panel box = new Panel();
            box.BackColor = Color.White;
            box.Location = new Point(borderMargin, borderMargin);
            box.Size = new Size(boxWidth, boxHeight);
            
            box.Paint += (s, e) =>
            {
                int rr = radius - 8;
                var path = new System.Drawing.Drawing2D.GraphicsPath();

                path.AddArc(0, 0, rr, rr, 180, 90);
                path.AddArc(box.Width - rr - 1, 0, rr, rr, 270, 90);
                path.AddArc(box.Width - rr - 1, box.Height - rr - 1, rr, rr, 0, 90);
                path.AddArc(0, box.Height - rr - 1, rr, rr, 90, 90);
                path.CloseFigure();

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };

            int leftMargin = 12;     
            int iconLeft = leftMargin;
            int textLeft = leftMargin + iconW + 8;   

            PictureBox pic = new PictureBox();
            pic.Width = iconW;
            pic.Height = iconW;
            pic.Location = new Point(iconLeft, (boxHeight - iconW) / 2);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Image = GetIconImage(icon);

            Label lblCaption = new Label();
            lblCaption.Text = caption;
            lblCaption.Font = captionFont;
            lblCaption.ForeColor = Color.Black;
            lblCaption.AutoSize = true;
            lblCaption.Location = new Point(textLeft, padding - 3);

            Label lblMsg = new Label();
            lblMsg.Text = message;
            lblMsg.Font = msgFont;
            lblMsg.ForeColor = Color.Black;
            lblMsg.AutoSize = true;
            lblMsg.Location = new Point(
                textLeft,
                padding + captionSize.Height + 2
            );

            box.Controls.Add(pic);
            box.Controls.Add(lblCaption);
            box.Controls.Add(lblMsg);
            toast.Controls.Add(box);

            toast.Show();

            await Fade(toast, 0.95, true);
            await Task.Delay(Timeout);
            await Fade(toast, 0, false);

            toast.Close();
            toast.Dispose();
        }
        
        private static async Task Fade(Form box, double target, bool fadeIn)
        {
            double step = 0.06 * (fadeIn ? 1 : -1);

            while ((fadeIn && box.Opacity < target) ||
                   (!fadeIn && box.Opacity > target))
            {
                double v = box.Opacity + step;

                if (v < 0) v = 0;
                if (v > 1) v = 1;

                box.Opacity = v;

                await Task.Delay(10);
            }

            box.Opacity = target;
        }

        private static Image GetIconImage(ToastIcon icon)
        {
            string path = null;
            switch (icon)
            {
                case ToastIcon.Info:
                    path = "Icons/info.png";
                    break;
                case ToastIcon.Warning:
                    path = "Icons/warning.png";
                    break;
                case ToastIcon.Error:
                    path = "Icons/error.png";
                    break;
                case ToastIcon.Success:
                    path = "Icons/check.png";
                    break;
                default:
                    return null;
            }
            try
            {
                if (System.IO.File.Exists(path))
                    return Image.FromFile(path);
            }
            catch
            {

            }
            return null;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int left, int top, int right, int bottom,
            int widthEllipse, int heightEllipse);
    }
}