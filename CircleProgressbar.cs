using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms.Internals;

namespace Sample
{
    public class CircleProgressbar : SKCanvasView
    {
        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(double), typeof(CircleProgressbar), defaultValue: 0.0, coerceValue: (bo, v) => ((double)v).Clamp(0, 1), propertyChanged : (b,o,n) => ((CircleProgressbar)b).UpdateProgress());

        public CircleProgressbar()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            PaintSurface += OnPaint;
        }

        public double Progress
        {
            get
            {
                return (double)GetValue(ProgressProperty);
            }
            set
            {
                SetValue(ProgressProperty, value);
            }
        }
        
        void UpdateProgress()
        {
            InvalidateSurface();
        }

        void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            SKColor[] colors = new SKColor[8];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = SKColor.FromHsl(i * 360f / 7, 100, 50);
            }

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = 23,
                ImageFilter = SKImageFilter.CreateDropShadow(1, 1, 2, 2, SKColor.Parse("#222222"), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground),
                Shader = SKShader.CreateSweepGradient(new SKPoint(125, 125), colors, null)
            })
            {

                var path = new SKPath();
                path.AddArc(new SKRect(11, 11, info.Width - 11, info.Height - 11), -90, 360 * (float)Progress);
                canvas.DrawPath(path, paint);
                var fontPath = new SKPath();
                fontPath.AddPathReverse(path);
                var fontPaint = new SKPaint
                {
                    Color = Color.White.ToSKColor(),
                    TextSize = 20,
                };
                canvas.DrawTextOnPath(string.Format("{0:P0}", Progress), fontPath, 0, 7, fontPaint);

                path.Dispose();
                fontPath.Dispose();
                fontPaint.Dispose();
            }
        }
    }
}
