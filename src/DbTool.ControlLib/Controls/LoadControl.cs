// Copyright (c) Juster zhu. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DbTool.ControlLib.Controls;
[TemplatePart(Name = "PART_ELLIPSE1", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE2", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE3", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE4", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE5", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE6", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE7", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE8", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ELLIPSE9", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_ROTATE", Type = typeof(RotateTransform))]
public class LoadControl : Control
{
    private Ellipse? PART_ELLIPSE1;
    private Ellipse? PART_ELLIPSE2;
    private Ellipse? PART_ELLIPSE3;
    private Ellipse? PART_ELLIPSE4;
    private Ellipse? PART_ELLIPSE5;
    private Ellipse? PART_ELLIPSE6;
    private Ellipse? PART_ELLIPSE7;
    private Ellipse? PART_ELLIPSE8;
    private Ellipse? PART_ELLIPSE9;
    private static RotateTransform? PART_ROTATE;
    
    private static DispatcherTimer? AnimationTimer;
    private static DependencyProperty IsLoadProperty = DependencyProperty.Register("IsLoad", typeof(bool), typeof(LoadControl), new PropertyMetadata(false, new PropertyChangedCallback(OnIsEnableChanged)));
  
    public LoadControl()
    {
        DefaultStyleKey = typeof(LoadControl);
        Unloaded += OnUnLoadControl;
        AnimationTimer ??= new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
        AnimationTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);        
    }

    public bool IsLoad
    {
        get { return (bool)GetValue(IsLoadProperty); }
        set { SetValue(IsLoadProperty, value); }
    }

    public override void OnApplyTemplate()
    {
        PART_ELLIPSE1 = EnforceInstance<Ellipse>("PART_ELLIPSE1");
        PART_ELLIPSE2 = EnforceInstance<Ellipse>("PART_ELLIPSE2");
        PART_ELLIPSE3 = EnforceInstance<Ellipse>("PART_ELLIPSE3");
        PART_ELLIPSE4 = EnforceInstance<Ellipse>("PART_ELLIPSE4");
        PART_ELLIPSE5 = EnforceInstance<Ellipse>("PART_ELLIPSE5");
        PART_ELLIPSE6 = EnforceInstance<Ellipse>("PART_ELLIPSE6");
        PART_ELLIPSE7 = EnforceInstance<Ellipse>("PART_ELLIPSE7");
        PART_ELLIPSE8 = EnforceInstance<Ellipse>("PART_ELLIPSE8");
        PART_ELLIPSE9 = EnforceInstance<Ellipse>("PART_ELLIPSE9");
        PART_ROTATE = EnforceInstance<RotateTransform>("PART_ROTATE");
        const double offset = Math.PI;
        const double step = Math.PI * 2 / 10.0;
        SetPosition(PART_ELLIPSE1, offset, 0.0, step);
        SetPosition(PART_ELLIPSE2, offset, 1.0, step);
        SetPosition(PART_ELLIPSE3, offset, 2.0, step);
        SetPosition(PART_ELLIPSE4, offset, 3.0, step);
        SetPosition(PART_ELLIPSE5, offset, 4.0, step);
        SetPosition(PART_ELLIPSE6, offset, 5.0, step);
        SetPosition(PART_ELLIPSE7, offset, 6.0, step);
        SetPosition(PART_ELLIPSE8, offset, 7.0, step);
        SetPosition(PART_ELLIPSE9, offset, 8.0, step);
    }

    #region Private Methods

    private void OnUnLoadControl(object sender, RoutedEventArgs e) => Stop();
      
    private static void OnIsEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue == null) return;
        bool.TryParse(e.NewValue.ToString(), out var isEnable);

        if (isEnable)
            Start();
        else
            Stop();
    }

    private static void Start()
    {
        if (AnimationTimer is null) return;
        Mouse.OverrideCursor = Cursors.Wait;        
        AnimationTimer.Tick += HandleAnimationTick;
        AnimationTimer.Start();
    }

    private static void Stop()
    {
        if (AnimationTimer is null) return;
        AnimationTimer.Stop();
        Mouse.OverrideCursor = Cursors.Arrow;
        AnimationTimer.Tick -= HandleAnimationTick;
    }

    private static void HandleAnimationTick(object? sender, EventArgs e)
    {
        if (PART_ROTATE == null) return;
        PART_ROTATE.Angle = (PART_ROTATE.Angle + 36) % 360;
    }

    private static void SetPosition(Ellipse ellipse, double offset,
        double posOffSet, double step)
    {
        if (ellipse == null) return;
        ellipse.SetValue(Canvas.LeftProperty, 50.0 + Math.Sin(offset + posOffSet * step) * 50.0);
        ellipse.SetValue(Canvas.TopProperty, 50 + Math.Cos(offset + posOffSet * step) * 50.0);
    }

    private T EnforceInstance<T>(string partName) where T : class, new()
    {
        if(GetTemplateChild(partName) is T t)
        {
            return t;
        }
        throw new ArgumentException("Unexpected type", nameof(partName));
    }

    #endregion

}
