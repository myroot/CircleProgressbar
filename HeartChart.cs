using System;
using System.Collections.Generic;
using SkiaSharp.Views.Forms;
using SkiaSharp;

namespace CircleProgressBar
{
    public class HeartChart : SKCanvasView
    {


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

        void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;


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
                primaryPaint.Color = _rightItemColor;
                DrawRightBGPart(canvas, primaryPaint, shadowPaint);

                // DrawLeftBGPart
                primaryPaint.Color = _leftItemColor;
                DrawLeftBGPart(canvas, primaryPaint, shadowPaint);

                // DrawBottomBGPart
                primaryPaint.Color = _bottomItemColor;
                DrawBottomBGPart(canvas, primaryPaint, shadowPaint);

                // Right End point
                primaryPaint.Color = _rightItemColor;
                DrawRightBGEndPart(canvas, primaryPaint, shadowPaint);
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
