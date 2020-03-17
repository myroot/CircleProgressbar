using System;
using System.Collections.Generic;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CircleProgressBar
{
    public class HeartChart : SKCanvasView
    {
        public static readonly BindableProperty LeftProgressProperty = BindableProperty.Create(nameof(LeftProgress), typeof(double), typeof(HeartChart), defaultValue: 0.0, coerceValue: (bo, v) => ((double)v).Clamp(0, 1), propertyChanged: (b, o, n) => ((HeartChart)b).InvalidateSurface());
        public static readonly BindableProperty RightProgressProperty = BindableProperty.Create(nameof(RightProgress), typeof(double), typeof(HeartChart), defaultValue: 0.0, coerceValue: (bo, v) => ((double)v).Clamp(0, 1), propertyChanged: (b, o, n) => ((HeartChart)b).InvalidateSurface());
        public static readonly BindableProperty BottomProgressProperty = BindableProperty.Create(nameof(BottomProgress), typeof(double), typeof(HeartChart), defaultValue: 0.0, coerceValue: (bo, v) => ((double)v).Clamp(0, 1), propertyChanged: (b, o, n) => ((HeartChart)b).InvalidateSurface());

        SKColor _leftItemColor = SKColor.FromHsv(321, 71, 100); // normal
        SKColor _leftItemBGColor = SKColor.FromHsv(321, 60, 27); // dim

        SKColor _rightItemColor = SKColor.FromHsv(105, 66, 98); // normal
        SKColor _rightItemBGColor = SKColor.FromHsv(99, 88, 20); // dim

        SKColor _bottomItemColor = SKColor.FromHsv(217, 87, 100); // normal
        SKColor _bottomItemBGColor = SKColor.FromHsv(218, 83, 36); // dim

        public HeartChart()
        {
            PaintSurface += OnPaint;
        }

        public double LeftProgress
        {
            get
            {
                return (double)GetValue(LeftProgressProperty);
            }
            set
            {
                SetValue(LeftProgressProperty, value);
            }
        }

        public double RightProgress
        {
            get
            {
                return (double)GetValue(RightProgressProperty);
            }
            set
            {
                SetValue(RightProgressProperty, value);
            }
        }

        public double BottomProgress
        {
            get
            {
                return (double)GetValue(BottomProgressProperty);
            }
            set
            {
                SetValue(BottomProgressProperty, value);
            }
        }

        float CanvasWidth { get; set; }
        float CanvasHeight { get; set; }

        float Thickness { get; set; }
        float HalfThickness { get; set; }

        ItemLines LeftLines { get; set; }
        ItemLines RightLines { get; set; }
        ItemLines BottomLines { get; set; }
        float MiddleRadian { get; set; }
        float SmallRadian { get; set; }

        float MiddleArcTriangleLine { get; set; }
        float SmallArcTriangleLine { get; set; }

        void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (CanvasWidth != info.Width || CanvasHeight != info.Height)
            {
                CanvasWidth = info.Width;
                CanvasHeight = info.Height;
                UpdateCoordinate();
            }

            using (var primaryPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = Thickness,
            })
            using (var shadowPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 3, 3, SKColor.Parse("#222222"), SKDropShadowImageFilterShadowMode.DrawShadowOnly),
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = Thickness,
            })
            {

                // DrawRightBGPart
                primaryPaint.Color = _rightItemBGColor;
                DrawRightBGPart(canvas, primaryPaint, shadowPaint);
                primaryPaint.Color = _rightItemColor;
                DrawRightProgressPart(canvas, primaryPaint, shadowPaint);

                // DrawLeftBGPart
                primaryPaint.Color = _leftItemBGColor;
                DrawLeftBGPart(canvas, primaryPaint, shadowPaint);
                primaryPaint.Color = _leftItemColor;
                DrawLeftProgressPart(canvas, primaryPaint, shadowPaint);

                // DrawBottomBGPart
                primaryPaint.Color = _bottomItemBGColor;
                DrawBottomBGPart(canvas, primaryPaint, shadowPaint);
                primaryPaint.Color = _bottomItemColor;
                DrawBottomProgressPart(canvas, primaryPaint, shadowPaint);

                // Right End point
                primaryPaint.Color = _rightItemBGColor;
                DrawRightBGEndPart(canvas, primaryPaint, shadowPaint);
                primaryPaint.Color = _rightItemColor;
                DrawRightProgressEndPart(canvas, primaryPaint, shadowPaint);
            }
        }

        void DrawRightBGPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            using (var rightArcPath = new SKPath())
            {
                rightArcPath.MoveTo(RightLines.Start);
                rightArcPath.LineTo(RightLines.Point1);
                rightArcPath.AddArc(RightLines.Arc, -135, 180);
                canvas.DrawPath(rightArcPath, paint);
            }
        }

        void DrawRightBGEndPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            canvas.DrawLine(RightLines.End, RightLines.End, shadowPaint);
            canvas.DrawLine(RightLines.Point2, RightLines.End, paint);
        }

        void DrawLeftBGPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            using (var leftArcPath = new SKPath())
            {
                // shadow
                canvas.DrawLine(LeftLines.Point2, LeftLines.End, shadowPaint);

                leftArcPath.MoveTo(LeftLines.Start);
                leftArcPath.LineTo(LeftLines.Point1);
                leftArcPath.AddArc(LeftLines.Arc, 135, 180);
                leftArcPath.MoveTo(LeftLines.Point2);
                leftArcPath.LineTo(LeftLines.End);
                canvas.DrawPath(leftArcPath, paint);
            }
        }

        void DrawBottomBGPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            using (var bottomArcPath = new SKPath())
            {
                canvas.DrawLine(BottomLines.Point2, BottomLines.End, shadowPaint);

                bottomArcPath.MoveTo(BottomLines.Start);
                bottomArcPath.LineTo(BottomLines.Point1);
                bottomArcPath.AddArc(BottomLines.Arc, 45, 90);
                bottomArcPath.MoveTo(BottomLines.Point2);
                bottomArcPath.LineTo(BottomLines.End);
                canvas.DrawPath(bottomArcPath, paint);
            }
        }

        void DrawLeftProgressPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            var line1Length = GetLineLength(LeftLines.Start, LeftLines.Point1);
            var arcLength = GetLineLength(MiddleRadian);
            var line2Length = GetLineLength(LeftLines.Point2, LeftLines.End);
            var totalLength = line1Length + arcLength + line2Length;
            var currentLength = totalLength * (float)LeftProgress;

            using (var progressPath = new SKPath())
            {
                var line1Rate = currentLength / line1Length;
                AppendLineProgressPath(progressPath, line1Rate, LeftLines.Start, LeftLines.Point1);
                if (line1Rate > 1)
                {
                    var arcRate = (currentLength - line1Length) / arcLength;
                    AppendArcProgressPath(progressPath, arcRate, LeftLines.Arc, 135, 180);
                    if (arcRate > 1)
                    {
                        var line2Rate = (currentLength - line1Length - arcLength) / line2Length;
                        AppendLineProgressPath(progressPath, line2Rate, LeftLines.Point2, LeftLines.End);
                    }
                }
                canvas.DrawPath(progressPath, paint);
            }
        }

        void DrawBottomProgressPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            var line1Length = GetLineLength(BottomLines.Start, BottomLines.Point1);
            var arcLength = GetLineLength(SmallRadian) / 2.0f;
            var line2Length = GetLineLength(BottomLines.Point2, BottomLines.End);
            var totalLength = line1Length + arcLength + line2Length;
            var currentLength = totalLength * (float)BottomProgress;

            using (var progressPath = new SKPath())
            {
                var line1Rate = currentLength / line1Length;
                AppendLineProgressPath(progressPath, line1Rate, BottomLines.Start, BottomLines.Point1);
                if (line1Rate > 1)
                {
                    var arcRate = (currentLength - line1Length) / arcLength;
                    AppendArcProgressPath(progressPath, arcRate, BottomLines.Arc, 45, 90);
                    if (arcRate > 1)
                    {
                        var line2Rate = (currentLength - line1Length - arcLength) / line2Length;
                        AppendLineProgressPath(progressPath, line2Rate, BottomLines.Point2, BottomLines.End);
                    }
                }
                canvas.DrawPath(progressPath, paint);
            }
        }

        void DrawRightProgressPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            var line1Length = GetLineLength(RightLines.Start, RightLines.Point1);
            var arcLength = GetLineLength(MiddleRadian);
            var line2Length = GetLineLength(RightLines.Point2, RightLines.End);
            var totalLength = line1Length + arcLength + line2Length;
            var currentLength = totalLength * (float)RightProgress;

            using (var progressPath = new SKPath())
            {
                var line1Rate = currentLength / line1Length;
                AppendLineProgressPath(progressPath, line1Rate, RightLines.Start, RightLines.Point1);
                if (line1Rate > 1)
                {
                    var arcRate = (currentLength - line1Length) / arcLength;
                    AppendArcProgressPath(progressPath, arcRate, RightLines.Arc, -135, 180);
                }
                canvas.DrawPath(progressPath, paint);
            }
        }

        void DrawRightProgressEndPart(SKCanvas canvas, SKPaint paint, SKPaint shadowPaint)
        {
            var line1Length = GetLineLength(RightLines.Start, RightLines.Point1);
            var arcLength = GetLineLength(MiddleRadian);
            var line2Length = GetLineLength(RightLines.Point2, RightLines.End);
            var totalLength = line1Length + arcLength + line2Length;
            var currentLength = totalLength * (float)RightProgress;

            using (var progressPath = new SKPath())
            {
                var arcRate = (currentLength - line1Length) / arcLength;
                if (arcRate > 0.5f)
                {
                    AppendArcProgressPath(progressPath, (arcRate - 0.5f) * 2, RightLines.Arc, -45, 90);
                    if (arcRate > 1)
                    {
                        var line2Rate = (currentLength - line1Length - arcLength) / line2Length;
                        AppendLineProgressPath(progressPath, line2Rate, RightLines.Point2, RightLines.End);
                    }
                    canvas.DrawPath(progressPath, paint);
                }
            }
        }

        void AppendLineProgressPath(SKPath path, float progress, SKPoint start, SKPoint end)
        {
            if (progress > 1)
                progress = 1;

            var targetX = start.X - ((start.X - end.X) * progress);
            var targetY = start.Y - ((start.Y - end.Y) * progress);

            path.MoveTo(start);
            path.LineTo(targetX, targetY);
        }
        void AppendArcProgressPath(SKPath path, float progress, SKRect oval, float startAngle, float sweepAngle)
        {
            if (progress > 1)
                progress = 1;
            sweepAngle = progress * sweepAngle;
            path.AddArc(oval, startAngle, sweepAngle);
        }

        void UpdateCoordinate()
        {
            Thickness = 0.125f * CanvasWidth;
            HalfThickness = Thickness / 2.0f;
            MiddleRadian = 0.2851f * CanvasWidth - HalfThickness;
            SmallRadian = 0.203f * CanvasWidth - HalfThickness;

            var leftArc = new SKRect(HalfThickness, HalfThickness, HalfThickness + 2 * MiddleRadian, HalfThickness + 2 * MiddleRadian);
            var rightArc = new SKRect(CanvasWidth - MiddleRadian * 2 - HalfThickness, HalfThickness, CanvasWidth - HalfThickness, HalfThickness + 2 * MiddleRadian);
            var bottomArc = new SKRect(0.5f * CanvasWidth - SmallRadian,
                                      0.82f * CanvasWidth - 2 * SmallRadian - HalfThickness,
                                      0.5f * CanvasWidth + SmallRadian,
                                      0.82f * CanvasWidth - HalfThickness);

            MiddleArcTriangleLine = (float)Math.Sqrt(Math.Pow(MiddleRadian, 2) / 2);
            SmallArcTriangleLine = (float)Math.Sqrt(Math.Pow(SmallRadian, 2) / 2);

            LeftLines = new ItemLines
            {
                Arc = leftArc,
                Start = new SKPoint { X = 0.178f * CanvasWidth, Y = 0.610f * CanvasHeight },
                Point1 = new SKPoint
                {
                    X = leftArc.MidX - MiddleArcTriangleLine,
                    Y = leftArc.MidY + MiddleArcTriangleLine
                },
                Point2 = new SKPoint
                {
                    X = leftArc.MidX + MiddleArcTriangleLine,
                    Y = leftArc.MidY - MiddleArcTriangleLine
                },
                End = new SKPoint { X = 0.5f * CanvasWidth, Y = 0.226f * CanvasHeight }
            };

            BottomLines = new ItemLines
            {
                Arc = bottomArc,
                Start = new SKPoint { X = 0.82f * CanvasWidth, Y = 0.61f * CanvasHeight },
                Point1 = new SKPoint
                {
                    X = bottomArc.MidX + SmallArcTriangleLine,
                    Y = bottomArc.MidY + SmallArcTriangleLine
                },
                Point2 = new SKPoint
                {
                    X = bottomArc.MidX - SmallArcTriangleLine,
                    Y = bottomArc.MidY + SmallArcTriangleLine
                },
                End = new SKPoint { X = 0.178f * CanvasWidth, Y = 0.610f * CanvasHeight }
            };

            RightLines = new ItemLines
            {
                Arc = rightArc,
                Start = new SKPoint { X = 0.5f * CanvasWidth, Y = 0.226f * CanvasHeight },
                Point1 = new SKPoint
                {
                    X = rightArc.MidX - MiddleArcTriangleLine,
                    Y = rightArc.MidY - MiddleArcTriangleLine
                },
                Point2 = new SKPoint
                {
                    X = rightArc.MidX + MiddleArcTriangleLine,
                    Y = rightArc.MidY + MiddleArcTriangleLine
                },
                End = new SKPoint { X = 0.82f * CanvasWidth, Y = 0.61f * CanvasHeight }
            };
        }

        static float GetLineLength(SKPoint start, SKPoint end)
        {
            return SKPoint.Distance(start, end);
        }
        static float GetLineLength(float radian)
        {
            return (float)Math.PI * radian;
        }
    }

    struct ItemLines
    {
        public SKPoint Start { get; set; }
        public SKPoint Point1 { get; set; }
        public SKRect Arc { get; set; }
        public SKPoint Point2 { get; set; }
        public SKPoint End { get; set; }
    }
}
