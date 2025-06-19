using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Redbox.Controls;
using Redbox.Core;
using Redbox.KioskEngine.ComponentModel;
using Redbox.Rental.Model;
using Redbox.Rental.Model.Browse;
using Redbox.Rental.UI.Models;

namespace Redbox.Rental.UI.Controls
{
    public partial class BrowseFilterBarUserControl : UserControl, IWPFActor
    {
        public BrowseFilterBarUserControl()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private FilterBarModel FilterBarModel => DataContext as FilterBarModel;

        private static bool IsADAMode
        {
            get
            {
                var service = ServiceLocator.Instance.GetService<IApplicationState>();
                return service != null && service.IsADAAccessible;
            }
        }

        public event WPFHitHandler OnWPFHit;

        public IActor Actor { get; set; }

        private void ApplyTheme()
        {
            var service = ServiceLocator.Instance.GetService<IThemeService>();
            var theme = service != null ? service.CurrentTheme : null;
            if (theme != null) theme.SetStyle(startover_button);
            if (theme == null) return;
            theme.SetStyle(BackButton);
        }

        private void HandleWPFHit()
        {
            var onWPFHit = OnWPFHit;
            if (onWPFHit == null) return;
            onWPFHit(Actor);
        }

        private void StartOverCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filterBarModel = FilterBarModel;
            if (filterBarModel != null) filterBarModel.ProcessOnStartOverButtonClicked();
            HandleWPFHit();
        }

        private void StartOverCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var filterBarModel = e.NewValue as FilterBarModel;
            if (filterBarModel != null)
            {
                AZFilterButtonContainer.Children.Clear();
                FilterButtonContainer.Children.Clear();
                if (filterBarModel.Filters != null)
                {
                    ServiceLocator.Instance.GetService<IConfiguration>();
                    var service = ServiceLocator.Instance.GetService<IThemeService>();
                    var theme = service != null ? service.CurrentTheme : null;
                    var num = filterBarModel.Filters.Count(browseFilter => browseFilter.IsVisible) +
                              (filterBarModel.FormatFilterButtonVisibility == Visibility.Visible ? 1 : 0) +
                              (filterBarModel.PriceRangeFilterButtonVisibility == Visibility.Visible ? 1 : 0);
                    FilterButtonPanel.Margin = new Thickness(0.0, 0.0, num <= 4 ? 178 : 0, 0.0);
                    foreach (var browseFilter2 in filterBarModel.Filters)
                    {
                        var roundedButton = new RoundedButton
                        {
                            DataContext = browseFilter2,
                            Style = GetStyle(this, "purple_with_purple_border_button_style"),
                            CornerRadius = 16.0,
                            MinWidth = browseFilter2.Width,
                            Height = browseFilter2.Height,
                            Margin = filterBarModel.FilterMargin,
                            Command = Commands.BrowseFilterCommand
                        };
                        roundedButton.SetBinding(IsEnabledProperty, new Binding
                        {
                            Path = new PropertyPath("IsButtonEnabled"),
                            Source = browseFilter2
                        });
                        roundedButton.SetBinding(VisibilityProperty, new Binding
                        {
                            Path = new PropertyPath("ButtonVisibility"),
                            Source = browseFilter2
                        });
                        var textBlock = new TextBlock
                        {
                            Text = browseFilter2.Text.ToUpper(),
                            Style = GetStyle(this, "font_montserrat_extrabold_14_correct"),
                            Margin = new Thickness(10.0, 0.0, 10.0, 0.0),
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        textBlock.SetBinding(TextBlock.TextProperty, new Binding
                        {
                            Path = new PropertyPath("Text"),
                            Source = browseFilter2
                        });
                        if (browseFilter2.ButtonType == BrowseFilterType.AtoZ)
                        {
                            var stackPanel = new StackPanel
                            {
                                Orientation = Orientation.Horizontal
                            };
                            if (!IsADAMode)
                                stackPanel.Children.Add(new Image
                                {
                                    Source = new BitmapImage(new Uri(
                                        "pack://Application:,,,/Redbox.Rental.UI;component/Assets/Images/magnifying-glass-icon.png")),
                                    Width = 17.0,
                                    Height = 17.0
                                });
                            stackPanel.Children.Add(textBlock);
                            roundedButton.Content = stackPanel;
                            var stackPanel2 = new StackPanel
                            {
                                Orientation = Orientation.Horizontal
                            };
                            stackPanel2.Children.Add(roundedButton);
                            if (browseFilter2.IsSelected && browseFilter2.IsVisible)
                            {
                                roundedButton.Style = GetStyle(this, "purple_with_purple_border_button_style_disabled");
                                stackPanel2.Children.Add(new Image
                                {
                                    Source = new BitmapImage(new Uri(
                                        "pack://Application:,,,/Redbox.Rental.UI;component/Assets/Images/purple-white-checkmark-sm.png")),
                                    Width = 19.0,
                                    Height = 18.0,
                                    Margin = new Thickness(-11.0, -15.0, -9.0, 0.0)
                                });
                            }

                            AZFilterButtonContainer.Children.Add(stackPanel2);
                        }
                        else
                        {
                            roundedButton.Content = textBlock;
                            var stackPanel3 = new StackPanel
                            {
                                Orientation = Orientation.Horizontal
                            };
                            stackPanel3.Children.Add(roundedButton);
                            if (browseFilter2.ButtonType == BrowseFilterType.Deals && browseFilter2.IsVisible &&
                                !browseFilter2.IsSelected)
                            {
                                stackPanel3.Children.Add(new Image
                                {
                                    Source = new BitmapImage(new Uri(
                                        "pack://Application:,,,/Redbox.Rental.UI;component/Assets/Images/purple-white-new-sm.png")),
                                    Width = 36.0,
                                    Height = 20.0,
                                    Margin = new Thickness(-11.0, -15.0, -9.0, 0.0)
                                });
                            }
                            else if (browseFilter2.IsVisible && browseFilter2.IsSelected)
                            {
                                roundedButton.Style = GetStyle(this, "purple_with_purple_border_button_style_disabled");
                                stackPanel3.Children.Add(new Image
                                {
                                    Source = new BitmapImage(new Uri(
                                        "pack://Application:,,,/Redbox.Rental.UI;component/Assets/Images/purple-white-checkmark-sm.png")),
                                    Width = 19.0,
                                    Height = 18.0,
                                    Margin = new Thickness(-11.0, -15.0, -9.0, 0.0)
                                });
                            }

                            FilterButtonContainer.Children.Add(stackPanel3);
                        }

                        if (theme != null) theme.SetStyle(roundedButton);
                    }
                }
            }
        }

        private static Style GetStyle(FrameworkElement frameworkElement, string styleName)
        {
            Style style = null;
            var obj = frameworkElement.FindResource(styleName);
            if (obj != null) style = obj as Style;
            return style;
        }

        private void BrowseFilterCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var roundedButton = e.OriginalSource as RoundedButton;
            if (roundedButton != null)
            {
                var browseFilter = roundedButton.DataContext as BrowseFilter;
                if (browseFilter != null)
                {
                    var filterBarModel = FilterBarModel;
                    if (filterBarModel != null) filterBarModel.ProcessOnBrowseFilterButtonClicked(browseFilter);
                    Commands.ResetIdleTimerCommand.Execute(null, this);
                }
            }
        }

        private void GoBackCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filterBarModel = FilterBarModel;
            if (filterBarModel != null) filterBarModel.ProcessOnBackButtonClicked();
            Commands.ResetIdleTimerCommand.Execute(null, this);
        }

        private void HomeCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filterBarModel = FilterBarModel;
            if (filterBarModel != null) filterBarModel.ProcessOnHomeButtonClicked();
            Commands.ResetIdleTimerCommand.Execute(null, this);
        }

        private void FormatFilterPopupCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filterBarModel = FilterBarModel;
            if (filterBarModel != null) filterBarModel.ProcessOnFormatFilterButtonClicked();
            Commands.ResetIdleTimerCommand.Execute(null, this);
        }

        private void ForSaleFilterPopupCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filterBarModel = FilterBarModel;
            if (filterBarModel != null) filterBarModel.ProcessOnPriceRangeFilterButtonClicked();
            Commands.ResetIdleTimerCommand.Execute(null, this);
        }
    }
}