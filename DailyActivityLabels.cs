using System;
using System.Collections.Generic;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
namespace CircleProgressBar
{
    public class DailyActivityLabels : SKCanvasView
    {
        SKColor _leftItemColor = SKColor.FromHsv(321, 71, 100); // normal
        SKColor _rightItemColor = SKColor.FromHsv(105, 66, 98); // normal
        SKColor _bottomItemColor = SKColor.FromHsv(217, 87, 100); // normal

        float _mainTextSize = 39;
        float _subTextSize = 22;

        public DailyActivityLabels()
        {
            PaintSurface += OnPaint;
            /*
            foreach (var font in SKFontManager.Default.FontFamilies)
            {
                Console.WriteLine($"Font : {font}");
            }
            */
        }


        public string LeftText { get; set; } = "333";
        public string LeftUnit { get; set; } = "cal";


        public string RightText { get; set; } = "18";
        public string RightUnit { get; set; } = "mins";


        public string BottomText { get; set; } = "7";
        public string BottomUnit { get; set; } = "times";

        public double UnitSpacing { get; set; } = 6;


        void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            var innerOval = SKRect.Create(new SKPoint { X = 39, Y = 39 }, new SKSize { Width = info.Width - 39 * 2,  Height = info.Height - 39 * 2 });
            var outterOval = SKRect.Create(new SKPoint { X = 0, Y = 0 }, new SKSize { Width = info.Width, Height = info.Height});

            canvas.Clear();

            DrawSideLabel(canvas, innerOval, -60, _leftItemColor, LeftText, LeftUnit);
            DrawSideLabel(canvas, innerOval, 60, _rightItemColor, RightText, RightUnit);
            DrawBottomLabel(canvas, outterOval, 0, _bottomItemColor, BottomText, BottomUnit);
        }

        void DrawSideLabel(SKCanvas canvas, SKRect oval, float angle, SKColor fontColor, string mainText, string subText)
        {
            using (var mainFontPaint = new SKPaint
            {
                IsAntialias = true,
                Color = fontColor,
                TextSize = _mainTextSize,
            })
            using (var subFontPaint = new SKPaint
            {
                IsAntialias = true,
                Color = fontColor,
                TextSize = _subTextSize,
            })
            {
                var mainTextWidth = mainFontPaint.MeasureText(mainText);
                var subTextWidth = subFontPaint.MeasureText(subText);

                var diffAngle = (float)((mainTextWidth + subTextWidth + UnitSpacing) / (Math.PI * oval.Width) * 360) / 2.0f;
                using (var path = new SKPath())
                {
                    path.AddArc(oval, -90 - diffAngle + angle, 45);
                    canvas.DrawTextOnPath(mainText, path, 0, -0.1f * mainFontPaint.TextSize, mainFontPaint);
                    canvas.DrawTextOnPath(subText, path, mainTextWidth + (float)UnitSpacing, -0.1f * subFontPaint.TextSize, subFontPaint);
                }
            }
        }

        void DrawBottomLabel(SKCanvas canvas, SKRect oval, float angle, SKColor fontColor, string mainText, string subText)
        {
            using (var mainFontPaint = new SKPaint
            {
                IsAntialias = true,
                Color = fontColor,
                TextSize = _mainTextSize,
            })
            using (var subFontPaint = new SKPaint
            {
                IsAntialias = true,
                Color = fontColor,
                TextSize = _subTextSize,
            })
            {
                var mainTextWidth = mainFontPaint.MeasureText(mainText);
                var subTextWidth = subFontPaint.MeasureText(subText);

                var diffAngle = (float)((mainTextWidth + subTextWidth + UnitSpacing) / (Math.PI * oval.Width) * 360) / 2.0f;
                using (var path = new SKPath())
                {
                    path.AddArc(oval, 90 + diffAngle + angle, -45);
                    canvas.DrawTextOnPath(mainText, path, 0, -0.1f * mainFontPaint.TextSize, mainFontPaint);
                    canvas.DrawTextOnPath(subText, path, mainTextWidth + (float)UnitSpacing, -0.1f * subFontPaint.TextSize, subFontPaint);
                }
            }
        }
    }
}
