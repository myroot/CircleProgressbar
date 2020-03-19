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
        const float s_largeInnerRadian = 141;
        const float s_largeOutterRadian = 180;
        const float s_smallInnerRadian = 113;
        const float s_smallOutterRadian = 113;



        SKColor _leftItemColor = SKColor.FromHsv(321, 71, 100); // normal
        SKColor _rightItemColor = SKColor.FromHsv(105, 66, 98); // normal
        SKColor _bottomItemColor = SKColor.FromHsv(217, 87, 100); // normal

        float _mainTextSize = 39;
        float _subTextSize = 22;
        float _unitSpacing = 6;

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

        


        void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            SKRect innerOval;
            SKRect outterOval;

            Console.WriteLine($"Draw Labels : {info.Height}");

            if (info.Height > 230)
            {
                // Large case
                _unitSpacing = 6;
                _mainTextSize = 39;
                _subTextSize = 22;
                innerOval = SKRect.Create(new SKPoint { X = (info.Width - s_largeInnerRadian * 2 ) / 2.0f, Y = (info.Height - s_largeInnerRadian * 2) /2.0f }, new SKSize { Width = s_largeInnerRadian * 2, Height = s_largeInnerRadian * 2 });
                outterOval = SKRect.Create(new SKPoint { X = (info.Width - s_largeOutterRadian * 2) / 2.0f, Y = (info.Height - s_largeOutterRadian * 2) / 2.0f }, new SKSize { Width = s_largeOutterRadian * 2, Height = s_largeOutterRadian * 2 });
            }
            else
            {
                // Small case
                _unitSpacing = 3;
                _mainTextSize = 28;
                _subTextSize = 20;
                innerOval = SKRect.Create(new SKPoint { X = (info.Width - s_smallInnerRadian * 2) / 2.0f, Y = (info.Height - s_smallInnerRadian * 2) / 2.0f }, new SKSize { Width = s_smallInnerRadian * 2, Height = s_smallInnerRadian * 2 });
                outterOval = innerOval;

            }

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

                var diffAngle = (float)((mainTextWidth + subTextWidth + _unitSpacing) / (Math.PI * oval.Width) * 360) / 2.0f;
                using (var path = new SKPath())
                {
                    path.AddArc(oval, -90 - diffAngle + angle, 45);
                    canvas.DrawTextOnPath(mainText, path, 0, -0.1f * mainFontPaint.TextSize, mainFontPaint);
                    canvas.DrawTextOnPath(subText, path, mainTextWidth + (float)_unitSpacing, -0.1f * subFontPaint.TextSize, subFontPaint);
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

                var diffAngle = (float)((mainTextWidth + subTextWidth + _unitSpacing) / (Math.PI * oval.Width) * 360) / 2.0f;
                using (var path = new SKPath())
                {
                    path.AddArc(oval, 90 + diffAngle + angle, -45);
                    canvas.DrawTextOnPath(mainText, path, 0, -0.1f * mainFontPaint.TextSize, mainFontPaint);
                    canvas.DrawTextOnPath(subText, path, mainTextWidth + (float)_unitSpacing, -0.1f * subFontPaint.TextSize, subFontPaint);
                }
            }
        }
    }
}
