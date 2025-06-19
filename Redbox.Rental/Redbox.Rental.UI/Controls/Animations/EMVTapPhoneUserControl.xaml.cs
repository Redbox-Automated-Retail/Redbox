using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Redbox.Rental.UI.Controls.Animations
{
    public partial class EMVTapPhoneUserControl : UserControl
    {
        public enum CardReadAnimations
        {
            TapPhone
        }

        private bool m_isAnimated;

        public EMVTapPhoneUserControl()
        {
            InitializeComponent();
        }

        public bool IsAnimated
        {
            get => m_isAnimated;
            set
            {
                if (m_isAnimated != value)
                {
                    m_isAnimated = value;
                    if (value)
                    {
                        BeginAnimation();
                        return;
                    }

                    StopAnimation();
                }
            }
        }

        public CardReadAnimations ShowAnimations { get; set; }

        private AnimationInfo FirstAnimationInfo { get; set; }

        private AnimationInfo SecondAnimationInfo { get; set; }

        public void BeginAnimation()
        {
            var storyboard = new Storyboard();
            storyboard.RepeatBehavior = RepeatBehavior.Forever;
            var translateTransform = new TranslateTransform();
            var rotateTransform = new RotateTransform();
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            TapPhone.RenderTransform = transformGroup;
            var animationInfo = CreateTapPhoneAnimationInfo();
            var animationInfo2 = CreateTapPhoneCheckmarkAnimationInfo();
            FirstAnimationInfo = null;
            SecondAnimationInfo = null;
            if (ShowAnimations == CardReadAnimations.TapPhone)
            {
                FirstAnimationInfo = animationInfo;
                SecondAnimationInfo = animationInfo2;
            }

            if (FirstAnimationInfo != null && SecondAnimationInfo != null)
            {
                var list = new List<string>();
                foreach (var keyValuePair in SecondAnimationInfo.KeyTimes) list.Add(keyValuePair.Key);
                list.ForEach(delegate(string key)
                {
                    var keyTimes = SecondAnimationInfo.KeyTimes;
                    keyTimes[key] += FirstAnimationInfo.TotalDuration;
                });
            }

            CreateTapPhoneAnimation(storyboard, animationInfo);
            CreateTapPhoneCheckmarkAnimation(storyboard, animationInfo2);
            Timeline timeline = storyboard;
            var firstAnimationInfo = FirstAnimationInfo;
            var timeSpan = firstAnimationInfo != null ? firstAnimationInfo.TotalDuration : TimeSpan.Zero;
            var secondAnimationInfo = SecondAnimationInfo;
            timeline.Duration =
                timeSpan + (secondAnimationInfo != null ? secondAnimationInfo.TotalDuration : TimeSpan.Zero);
            storyboard.Begin();
        }

        private AnimationInfo CreateTapPhoneAnimationInfo()
        {
            var animationInfo = new AnimationInfo();
            animationInfo.Durations["InitialDelay"] = TimeSpan.FromSeconds(0.75);
            animationInfo.Durations["FirstMotion"] = TimeSpan.FromSeconds(0.5);
            animationInfo.Durations["PauseDelay"] = TimeSpan.FromSeconds(0.25);
            animationInfo.Durations["FadeDelay"] = TimeSpan.FromSeconds(0.0);
            animationInfo.Durations["EndDelay"] = TimeSpan.FromSeconds(0.0);
            animationInfo.KeyTimes["StartTime"] = TimeSpan.Zero;
            animationInfo.KeyTimes["EndOfInitialDelay"] =
                animationInfo.KeyTimes["StartTime"] + animationInfo.Durations["InitialDelay"];
            animationInfo.KeyTimes["EndOfFirstMotion"] =
                animationInfo.KeyTimes["EndOfInitialDelay"] + animationInfo.Durations["FirstMotion"];
            animationInfo.KeyTimes["EndOfPauseDelay"] =
                animationInfo.KeyTimes["EndOfFirstMotion"] + animationInfo.Durations["PauseDelay"];
            animationInfo.KeyTimes["EndOfFade"] =
                animationInfo.KeyTimes["EndOfPauseDelay"] + animationInfo.Durations["FadeDelay"];
            animationInfo.KeyTimes["EndOfAnimation"] =
                animationInfo.KeyTimes["EndOfFade"] + animationInfo.Durations["EndDelay"];
            return animationInfo;
        }

        private void CreateTapPhoneAnimation(Storyboard storyboard, AnimationInfo tapPhoneAnimationInfo)
        {
            var translateTransform = new TranslateTransform();
            var rotateTransform = new RotateTransform();
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            TapPhone.RenderTransform = transformGroup;
            var doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["StartTime"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfInitialDelay"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfFirstMotion"]),
                Value = 153.0
            });
            var doubleAnimationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["StartTime"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfInitialDelay"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfFirstMotion"]),
                Value = -230.0
            });
            var doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["StartTime"] > TimeSpan.Zero
                    ? tapPhoneAnimationInfo.KeyTimes["StartTime"] - TimeSpan.FromMilliseconds(10.0)
                    : TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["StartTime"]),
                Value = 1.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfPauseDelay"]),
                Value = 1.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfFade"]),
                Value = 0.0
            });
            var doubleAnimationUsingKeyFrames4 = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames4.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames4.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["StartTime"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames4.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfInitialDelay"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames4.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneAnimationInfo.KeyTimes["EndOfFirstMotion"]),
                Value = 18.0
            });
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames, TapPhone);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames);
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, TapPhone);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames3, TapPhone);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames3, new PropertyPath("(Opacity)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames3);
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames4, TapPhone);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames4,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames4);
        }

        private AnimationInfo CreateTapPhoneCheckmarkAnimationInfo()
        {
            var animationInfo = new AnimationInfo();
            animationInfo.Durations["InitialDelay"] = TimeSpan.FromSeconds(0.0);
            animationInfo.Durations["FirstMotion"] = TimeSpan.FromSeconds(0.0);
            animationInfo.Durations["PauseDelay"] = TimeSpan.FromSeconds(1.75);
            animationInfo.Durations["FadeDelay"] = TimeSpan.FromSeconds(0.25);
            animationInfo.Durations["EndDelay"] = TimeSpan.FromSeconds(0.25);
            animationInfo.KeyTimes["StartTime"] = TimeSpan.Zero;
            animationInfo.KeyTimes["EndOfInitialDelay"] =
                animationInfo.KeyTimes["StartTime"] + animationInfo.Durations["InitialDelay"];
            animationInfo.KeyTimes["EndOfFirstMotion"] =
                animationInfo.KeyTimes["EndOfInitialDelay"] + animationInfo.Durations["FirstMotion"];
            animationInfo.KeyTimes["EndOfPauseDelay"] =
                animationInfo.KeyTimes["EndOfFirstMotion"] + animationInfo.Durations["PauseDelay"];
            animationInfo.KeyTimes["EndOfFade"] =
                animationInfo.KeyTimes["EndOfPauseDelay"] + animationInfo.Durations["FadeDelay"];
            animationInfo.KeyTimes["EndOfAnimation"] =
                animationInfo.KeyTimes["EndOfFade"] + animationInfo.Durations["EndDelay"];
            return animationInfo;
        }

        private void CreateTapPhoneCheckmarkAnimation(Storyboard storyboard,
            AnimationInfo tapPhoneCheckmarkAnimationInfo)
        {
            var translateTransform = new TranslateTransform();
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform);
            TapPhoneCheckmark.RenderTransform = transformGroup;
            var doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 175.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["StartTime"]),
                Value = 175.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["EndOfInitialDelay"]),
                Value = 175.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["EndOfFirstMotion"]),
                Value = 175.0
            });
            var doubleAnimationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = -150.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["StartTime"]),
                Value = -150.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["EndOfInitialDelay"]),
                Value = -150.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["EndOfFirstMotion"]),
                Value = -150.0
            });
            var doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["StartTime"] > TimeSpan.Zero
                    ? tapPhoneCheckmarkAnimationInfo.KeyTimes["StartTime"] - TimeSpan.FromMilliseconds(10.0)
                    : TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["StartTime"]),
                Value = 1.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["EndOfPauseDelay"]),
                Value = 1.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(tapPhoneCheckmarkAnimationInfo.KeyTimes["EndOfFade"]),
                Value = 0.0
            });
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames, TapPhoneCheckmark);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames);
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, TapPhoneCheckmark);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames3, TapPhoneCheckmark);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames3, new PropertyPath("(Opacity)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames3);
        }

        public void StopAnimation()
        {
            TapPhone.RenderTransform = null;
        }

        private class DurationConstants
        {
            public const string InitialDelay = "InitialDelay";

            public const string FirstMotion = "FirstMotion";

            public const string PauseDelay = "PauseDelay";

            public const string FadeDelay = "FadeDelay";

            public const string EndDelay = "EndDelay";
        }

        private class KeyTimeConstants
        {
            public const string StartTime = "StartTime";

            public const string EndOfInitialDelay = "EndOfInitialDelay";

            public const string EndOfFirstMotion = "EndOfFirstMotion";

            public const string EndOfPauseDelay = "EndOfPauseDelay";

            public const string EndOfFade = "EndOfFade";

            public const string EndOfAnimation = "EndOfAnimation";
        }

        private class AnimationInfo
        {
            public Dictionary<string, TimeSpan> Durations { get; } = new Dictionary<string, TimeSpan>();

            public Dictionary<string, TimeSpan> KeyTimes { get; } = new Dictionary<string, TimeSpan>();

            public TimeSpan TotalDuration
            {
                get
                {
                    var timeSpan = TimeSpan.Zero;
                    if (Durations.Count > 0)
                        foreach (var keyValuePair in Durations)
                            timeSpan += keyValuePair.Value;

                    return timeSpan;
                }
            }
        }
    }
}