using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Redbox.Rental.UI.Controls.Animations
{
    public partial class EMVSwipeCardUserControl : UserControl
    {
        public enum CardReadAnimations
        {
            SwipeCard
        }

        private bool m_isAnimated;

        public EMVSwipeCardUserControl()
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

        public void BeginAnimation()
        {
            var storyboard = new Storyboard();
            storyboard.RepeatBehavior = RepeatBehavior.Forever;
            var translateTransform = new TranslateTransform();
            var rotateTransform = new RotateTransform();
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            SwipeCard.RenderTransform = transformGroup;
            var animationInfo = CreateSwipeCardAnimationInfo();
            FirstAnimationInfo = null;
            if (ShowAnimations == CardReadAnimations.SwipeCard) FirstAnimationInfo = animationInfo;
            CreateSwipeCardAnimation(storyboard, animationInfo);
            Timeline timeline = storyboard;
            var firstAnimationInfo = FirstAnimationInfo;
            timeline.Duration = firstAnimationInfo != null ? firstAnimationInfo.TotalDuration : TimeSpan.Zero;
            storyboard.Begin();
        }

        private static AnimationInfo CreateSwipeCardAnimationInfo()
        {
            var animationInfo = new AnimationInfo();
            animationInfo.Durations["InitialDelay"] = TimeSpan.FromSeconds(0.5);
            animationInfo.Durations["FirstMotion"] = TimeSpan.FromSeconds(0.5);
            animationInfo.Durations["PauseDelay"] = TimeSpan.FromSeconds(0.5);
            animationInfo.Durations["FadeDelay"] = TimeSpan.FromSeconds(0.5);
            animationInfo.Durations["EndDelay"] = TimeSpan.FromSeconds(0.5);
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

        private void CreateSwipeCardAnimation(Storyboard storyboard, AnimationInfo swipeCardAnimationInfo)
        {
            var doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["StartTime"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["EndOfInitialDelay"]),
                Value = -50.0
            });
            doubleAnimationUsingKeyFrames.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["EndOfFirstMotion"]),
                Value = -25.0
            });
            var doubleAnimationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["StartTime"]),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["EndOfInitialDelay"]),
                Value = 40.0
            });
            doubleAnimationUsingKeyFrames2.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["EndOfFirstMotion"]),
                Value = 310.0
            });
            var doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["StartTime"] > TimeSpan.Zero
                    ? swipeCardAnimationInfo.KeyTimes["StartTime"] - TimeSpan.FromMilliseconds(10.0)
                    : TimeSpan.Zero),
                Value = 0.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["StartTime"]),
                Value = 1.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["EndOfPauseDelay"]),
                Value = 1.0
            });
            doubleAnimationUsingKeyFrames3.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(swipeCardAnimationInfo.KeyTimes["EndOfFade"]),
                Value = 0.0
            });
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames, SwipeCard);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames);
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, SwipeCard);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
            Storyboard.SetTarget(doubleAnimationUsingKeyFrames3, SwipeCard);
            Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames3, new PropertyPath("(Opacity)"));
            storyboard.Children.Add(doubleAnimationUsingKeyFrames3);
        }

        public void StopAnimation()
        {
            SwipeCard.RenderTransform = null;
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