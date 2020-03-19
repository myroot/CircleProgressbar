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

        public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(DailyActivityLabels), defaultValue: string.Empty, propertyChanged: (b, o, n) => ((DailyActivityLabels)b).InvalidateSurface());
        public static readonly BindableProperty LeftUnitProperty = BindableProperty.Create(nameof(LeftUnit), typeof(string), typeof(DailyActivityLabels), defaultValue: string.Empty, propertyChanged: (b, o, n) => ((DailyActivityLabels)b).InvalidateSurface());

        public static readonly BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(DailyActivityLabels), defaultValue: string.Empty, propertyChanged: (b, o, n) => ((DailyActivityLabels)b).InvalidateSurface());
        public static readonly BindableProperty RightUnitProperty = BindableProperty.Create(nameof(RightUnit), typeof(string), typeof(DailyActivityLabels), defaultValue: string.Empty, propertyChanged: (b, o, n) => ((DailyActivityLabels)b).InvalidateSurface());

        public static readonly BindableProperty BottomTextProperty = BindableProperty.Create(nameof(BottomText), typeof(string), typeof(DailyActivityLabels), defaultValue: string.Empty, propertyChanged: (b, o, n) => ((DailyActivityLabels)b).InvalidateSurface());
        public static readonly BindableProperty BottomUnitProperty = BindableProperty.Create(nameof(BottomUnit), typeof(string), typeof(DailyActivityLabels), defaultValue: string.Empty, propertyChanged: (b, o, n) => ((DailyActivityLabels)b).InvalidateSurface());


        const float s_largeInnerRadian = 141;
        const float s_largeOutterRadian = 180;
        const float s_smallInnerRadian = 113;

        SKColor _leftItemColor = SKColor.FromHsv(321, 71, 100); // normal
        SKColor _rightItemColor = SKColor.FromHsv(105, 66, 98); // normal
        SKColor _bottomItemColor = SKColor.FromHsv(217, 87, 100); // normal

        float _mainTextSize = 39;
        float _subTextSize = 22;
        float _unitSpacing = 6;

        public DailyActivityLabels()
        {
            PaintSurface += OnPaint;
        }


        public string LeftText
        {
            get
            {
                return (string)GetValue(LeftTextProperty);
            }
            set
            {
                SetValue(LeftTextProperty, value);
            }
        }

        public string LeftUnit
        {
            get
            {
                return (string)GetValue(LeftUnitProperty);
            }
            set
            {
                SetValue(LeftUnitProperty, value);
            }
        }

        public string RightText
        {
            get
            {
                return (string)GetValue(RightTextProperty);
            }
            set
            {
                SetValue(RightTextProperty, value);
            }
        }

        public string RightUnit
        {
            get
            {
                return (string)GetValue(RightUnitProperty);
            }
            set
            {
                SetValue(RightUnitProperty, value);
            }
        }

        public string BottomText
        {
            get
            {
                return (string)GetValue(BottomTextProperty);
            }
            set
            {
                SetValue(BottomTextProperty, value);
            }
        }

        public string BottomUnit
        {
            get
            {
                return (string)GetValue(BottomUnitProperty);
            }
            set
            {
                SetValue(BottomUnitProperty, value);
            }
        }

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
