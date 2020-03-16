using System;
using System.Collections.Generic;
using SkiaSharp.Views.Forms;
using SkiaSharp;

namespace CircleProgressBar
{
    public class HeartChart : SKCanvasView
    {
        public HeartChart()
        {
            PaintSurface += OnPaint;
        }

        void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            //canvas.Clear(SKColor.Parse("#eeeeee"));

            var width = info.Width;
            var height = info.Height;
            var thickness = 0.125f * width;
            var middleRadian = 0.2851f * width - thickness / 2.0f;
            var smallRadian = 0.203f * width - thickness / 2.0f;

            using (var primaryPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = thickness,
            })
            using (var shadowPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 3, 3, SKColor.Parse("#222222"), SKDropShadowImageFilterShadowMode.DrawShadowOnly),
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = thickness,
            })
            {
                var leftItemColor = SKColor.FromHsv(321, 71, 100); // normal
                var leftItemBGColor = SKColor.FromHsv(321, 60, 27); // dim


                var rightItemColor = SKColor.FromHsv(105, 66, 98); // normal
                var rightItemBGColor = SKColor.FromHsv(99, 88, 20); // dim

                var bottomItemColor = SKColor.FromHsv(217, 87, 100); // normal
                var bottomItemBGColor = SKColor.FromHsv(218, 83, 36); // dim

                var leftArc = new SKRect(thickness / 2.0f, thickness / 2.0f, thickness / 2.0f + 2 * middleRadian, thickness / 2.0f + 2 * middleRadian);
                var rightArc = new SKRect(width - middleRadian * 2 - thickness / 2.0f, thickness / 2.0f, width - thickness / 2.0f, thickness / 2.0f + 2 * middleRadian);
                var bottomArc = new SKRect(0.5f * width - smallRadian, 0.82f * width - 2 * smallRadian - thickness / 2.0f, 0.5f * width + smallRadian, 0.82f * width - thickness / 2.0f);

                var dx = (float)Math.Sqrt(Math.Pow(middleRadian, 2) / 2);
                var dx2 = (float)Math.Sqrt(Math.Pow(smallRadian, 2) / 2);

                using (var leftArcPath = new SKPath())
                {
                    var startX = 0.178f * width;
                    var startY = 0.610f * height;
                    var lineX = leftArc.MidX - dx;
                    var lineY = leftArc.MidY + dx;

                    leftArcPath.MoveTo(startX, startY);
                    leftArcPath.LineTo(lineX, lineY);

                    leftArcPath.AddArc(leftArc, 135, 180);

                    primaryPaint.Color = leftItemColor;
                    canvas.DrawPath(leftArcPath, primaryPaint);
                }

                using (var rightArcPath = new SKPath())
                {
                    var startX = 0.5f * width;
                    var startY = 0.226f * height;
                    var lineX = rightArc.MidX - dx;
                    var lineY = rightArc.MidY - dx;
                    rightArcPath.MoveTo(startX, startY);
                    rightArcPath.LineTo(lineX, lineY);

                    rightArcPath.AddArc(rightArc, -135, 180);

                    primaryPaint.Color = rightItemColor;
                    canvas.DrawPath(rightArcPath, primaryPaint);
                }

                // LeftArc End Point
                using (var leftArcEndPath = new SKPath())
                {
                    var startX = leftArc.MidX + dx;
                    var startY = leftArc.MidY - dx;
                    var lineX = 0.5f * width;
                    var lineY = 0.226f * height;
                    leftArcEndPath.MoveTo(startX, startY);
                    leftArcEndPath.LineTo(lineX, lineY);
                    primaryPaint.Color = leftItemColor;

                    canvas.DrawLine(lineX, lineY, lineX - thickness / 4.0f, lineY - thickness / 4.0f, shadowPaint);
                    canvas.DrawPath(leftArcEndPath, primaryPaint);
                }


                using (var bottomArcPath = new SKPath())
                {
                    var startX = 0.82f * width;
                    var startY = 0.61f * height;
                    var lineX = bottomArc.MidX + dx2;
                    var lineY = bottomArc.MidY + dx2;
                    bottomArcPath.MoveTo(startX, startY);
                    bottomArcPath.LineTo(lineX, lineY);
                    bottomArcPath.AddArc(bottomArc, 45, 90);
                    primaryPaint.Color = bottomItemColor;
                    canvas.DrawPath(bottomArcPath, primaryPaint);
                }

                // Right End point
                using (var rightArcEndPath = new SKPath())
                {
                    var startX = rightArc.MidX + dx;
                    var startY = rightArc.MidY + dx;
                    var lineX = 0.82f * width;
                    var lineY = 0.61f * height;

                    canvas.DrawLine(lineX, lineY, lineX, lineY, shadowPaint);

                    primaryPaint.Color = rightItemColor;
                    rightArcEndPath.MoveTo(startX, startY);
                    rightArcEndPath.LineTo(lineX, lineY);                    
                    canvas.DrawPath(rightArcEndPath, primaryPaint);
                }

                // Bottom End Point
                using (var bottomArcEndPath = new SKPath())
                {
                    var startX = bottomArc.MidX - dx2;
                    var startY = bottomArc.MidY + dx2;
                    var lineX = 0.178f * width;
                    var lineY = 0.610f * height;

                    canvas.DrawLine(lineX, lineY, lineX, lineY, shadowPaint);

                    bottomArcEndPath.MoveTo(startX, startY);
                    bottomArcEndPath.LineTo(lineX, lineY);
                    primaryPaint.Color = bottomItemColor;
                    canvas.DrawPath(bottomArcEndPath, primaryPaint);
                }
            }


        }
    }
}
