﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace LCARSToolkit.Controls
{
    public class Button : Windows.UI.Xaml.Controls.Button
    {
        private bool _IsLit;
        public bool IsLit
        {
            get
            {
                return _IsLit;
            }
            private set
            {
                if (_IsLit != value)
                {
                    _IsLit = value;
                    SetMasks(value);
                }                    
            }
        }

        public Illumination Illumination
        {
            get { return (Illumination)GetValue(IlluminationProperty); }
            set { SetValue(IlluminationProperty, value); }
        }
        public static readonly DependencyProperty IlluminationProperty = DependencyProperty.Register("Illumination", typeof(Illumination), typeof(Button), new PropertyMetadata(Illumination.On));
        
        public Stumps Stumps
        {
            get { return (Stumps)GetValue(StumpsProperty); }
            set
            {
                SetValue(StumpsProperty, value);
                UpdateCorners();
            }
        }
        public static readonly DependencyProperty StumpsProperty = DependencyProperty.Register("Stumps", typeof(Stumps), typeof(Button), new PropertyMetadata(Stumps.Both, StumpsChanged));


        public Corner ContentAlignment
        {
            get { return (Corner)GetValue(ContentAlignmentProperty); }
            set
            {
                SetValue(ContentAlignmentProperty, value);
                this.HorizontalContentAlignment = (value == Corner.TopLeft || value == Corner.BottomLeft) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                this.VerticalContentAlignment = (value == Corner.TopLeft || value == Corner.TopRight) ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            }
        }
        public static readonly DependencyProperty ContentAlignmentProperty = DependencyProperty.Register("ContentAlignment", typeof(Corner), typeof(Button), new PropertyMetadata(null));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Button), new PropertyMetadata(null));
        
        public MediaElement SoundElement
        {
            get { return (MediaElement)GetValue(SoundElementProperty); }
            set { SetValue(SoundElementProperty, value); }
        }
        public static readonly DependencyProperty SoundElementProperty = DependencyProperty.Register(nameof(SoundElement), typeof(MediaElement), typeof(Button), new PropertyMetadata(null));

        public Button()
        {
            this.DefaultStyleKey = typeof(Button);
            this.SizeChanged += delegate { UpdateCorners(); };
            this.Click += delegate
            {
                SoundElement?.Play();
            };
            // subscribe to the single, global FlashTimer such that everything flashes synchronously
            Extensions.FlashTimer.Tick += delegate { IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On); };
        }
        private static void StumpsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Button).UpdateCorners();
        }
        internal virtual void SetMasks(bool isLit)
        {
            try
            {
                (this.GetTemplateChild("mask") as Border).Background = (isLit) ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Color.FromArgb(0x7F, 0x00, 0x00, 0x00));
            }
            catch
            { }
        }

        internal virtual void UpdateCorners()
        {
            double radius = this.ActualHeight / 2;
            switch (Stumps)
            {
                case Stumps.None:
                    this.CornerRadius = new CornerRadius(0);
                    this.Padding = new Thickness(5,0,5,0);
                    break;
                case Stumps.Left:
                    this.CornerRadius = new CornerRadius(radius, 0, 0, radius);
                    this.Padding = new Thickness(radius,0,5,0);
                    break;
                case Stumps.Right:
                    this.CornerRadius = new CornerRadius(0, radius, radius, 0);
                    this.Padding = new Thickness(5, 0, radius, 0);
                    break;
                case Stumps.Both:
                    this.CornerRadius = new CornerRadius(radius);
                    this.Padding = new Thickness(radius, 0, radius, 0);
                    break;
            }
        }
    }
}
