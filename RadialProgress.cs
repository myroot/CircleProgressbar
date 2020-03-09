using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms.Internals;
using System;

namespace Sample
{
    public class RadialProgress : SKCanvasView
    {
        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(double), typeof(RadialProgress), defaultValue: 0.0, coerceValue: (bo, v) => ((double)v).Clamp(0, 1), propertyChanged : (b,o,n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(nameof(Thickness), typeof(double), typeof(RadialProgress), defaultValue: 30d, propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty RadialBackgroundColorProperty = BindableProperty.Create(nameof(RadialBackgroundColor), typeof(Color), typeof(RadialProgress), defaultValue: Color.Default, propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty RadialStartColorProperty = BindableProperty.Create(nameof(RadialStartColor), typeof(Color), typeof(RadialProgress), defaultValue: Color.FromHex("#9cff00"), propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty RadialMiddleColorProperty = BindableProperty.Create(nameof(RadialMiddleColor), typeof(Color), typeof(RadialProgress), defaultValue: Color.FromHex("#aaff00"), propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty RadialEndColorProperty = BindableProperty.Create(nameof(RadialEndColor), typeof(Color), typeof(RadialProgress), defaultValue: Color.FromHex("#d8fe00"), propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RadialProgress), defaultValue: Color.Accent, propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty TextFormatProperty = BindableProperty.Create(nameof(TextFormat), typeof(string), typeof(RadialProgress), defaultValue: "{0:P0}", propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(RadialProgress), defaultValue: 20d, propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty HasLabelProperty = BindableProperty.Create(nameof(HasLabel), typeof(bool), typeof(RadialProgress), defaultValue: true, propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty StartAngleProperty = BindableProperty.Create(nameof(StartAngle), typeof(double), typeof(RadialProgress), defaultValue: 0d, propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());
        public static readonly BindableProperty SweepAngleProperty = BindableProperty.Create(nameof(SweepAngle), typeof(double), typeof(RadialProgress), defaultValue: 365d, propertyChanged: (b, o, n) => ((RadialProgress)b).InvalidateSurface());

        public RadialProgress()
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

        public double Thickness
        {
            get
            {
                return (double)GetValue(ThicknessProperty);
            }
            set
            {
                SetValue(ThicknessProperty, value);
            }

        }

        public Color RadialBackgroundColor
        {
            get
            {
                return (Color)GetValue(RadialBackgroundColorProperty);
            }
            set
            {
                SetValue(RadialBackgroundColorProperty, value);
            }
        }

        public Color RadialStartColor
        {
            get
            {
                return (Color)GetValue(RadialStartColorProperty);
            }
            set
            {
                SetValue(RadialStartColorProperty, value);
            }
        }

        public Color RadialMiddleColor
        {
            get
            {
                return (Color)GetValue(RadialMiddleColorProperty);
            }
            set
            {
                SetValue(RadialMiddleColorProperty, value);
            }
        }

        public Color RadialEndColor
        {
            get
            {
                return (Color)GetValue(RadialEndColorProperty);
            }
            set
            {
                SetValue(RadialEndColorProperty, value);
            }
        }

        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        public string TextFormat
        {
            get
            {
                return (string)GetValue(TextFormatProperty);
            }
            set
            {
                SetValue(TextFormatProperty, value);
            }
        }

        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        public bool HasLabel
        {
            get
            {
                return (bool)GetValue(HasLabelProperty);
            }
            set
            {
                SetValue(HasLabelProperty, value);
            }
        }

        public double StartAngle
        {
            get
            {
                return (double)GetValue(StartAngleProperty);
            }
            set
            {
                SetValue(StartAngleProperty, value);
            }
        }

        public double SweepAngle
        {
            get
            {
                return (double)GetValue(SweepAngleProperty);
            }
            set
            {
                SetValue(SweepAngleProperty, value);
            }
        }

        void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            using (var primaryPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = (float)Thickness,
            })
            using (var path = new SKPath())
            using (var backgroundPath = new SKPath())
            {
                var margin = (float)Thickness / 2.0f;

                if (RadialBackgroundColor != Color.Default && RadialBackgroundColor != Color.Transparent)
                {
                    primaryPaint.Color = RadialBackgroundColor.ToSKColor();
                    backgroundPath.AddArc(new SKRect(margin, margin, info.Width - margin, info.Height - margin), -90 + (float)StartAngle, (float)SweepAngle);
                    canvas.DrawPath(backgroundPath, primaryPaint);
                }


                SKColor[] colors = new SKColor[]
                {
                    RadialStartColor.ToSKColor(),
                    RadialStartColor.ToSKColor(),
                    RadialMiddleColor.ToSKColor(),
                    RadialMiddleColor.ToSKColor(),
                    RadialEndColor.ToSKColor(),
                    RadialEndColor.ToSKColor(),
                    RadialStartColor.ToSKColor(),
                };

                float[] colorPos = new float[]
                {
                    0,                // start
                    (1 / 6.0f) * 1,   // start
                    (1 / 6.0f) * 2,   // mid
                    (1 / 6.0f) * 3,   // mid
                    (1 / 6.0f) * 4,   // end
                    (1 / 6.0f) * 5.7f,// end
                    0.95f              // start
                };

                primaryPaint.Shader = SKShader.CreateSweepGradient(new SKPoint(info.Rect.MidX, info.Rect.MidY), colors, colorPos, SKShaderTileMode.Repeat, -90 + (float)StartAngle, -90 + (float)StartAngle + (float)SweepAngle);

                path.AddArc(new SKRect(margin, margin, info.Width - margin, info.Height - margin), -90 + (float)StartAngle, (float)SweepAngle * (float)Progress);
                canvas.DrawPath(path, primaryPaint);
                if (HasLabel)
                {
                    using (var fontPaint = new SKPaint
                    {
                        Color = TextColor.ToSKColor(),
                        TextSize = (float)FontSize,
                    })
                    using (var fontPath = new SKPath())
                    {
                        fontPath.AddPathReverse(path);
                        canvas.DrawTextOnPath(string.Format(TextFormat, Progress), fontPath, 0, (float)FontSize / 3.0f, fontPaint);
                    }
                }
            }
        }
    }
}
