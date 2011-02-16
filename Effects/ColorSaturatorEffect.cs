using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace TangramApp1._35.Effects
{
    /// <summary>
    /// NOTE: All attempts to allow this class to have the ability to animate via storyboard has failed.
    /// It cannot be animated through that means. If I find a way, I will be sure you let you know.
    /// </summary>
    public class ColorSaturatorEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorSaturatorEffect), 0);
        public static readonly DependencyProperty ColorSaturationProperty = DependencyProperty.Register("ColorSaturation", typeof(double), typeof(ColorSaturatorEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty FilterColorProperty = DependencyProperty.Register("FilterColor", typeof(Color), typeof(ColorSaturatorEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(1)));

        public ColorSaturatorEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/Resources/Shaders/ColorSaturator.ps", UriKind.Absolute);
            this.PixelShader = pixelShader;

            DependencyPropertyDescriptor ColorSaturationPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(
                ColorSaturationProperty, typeof(ColorSaturatorEffect));

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(ColorSaturationProperty);
            this.UpdateShaderValue(FilterColorProperty);
        }

        public ColorSaturatorEffect(Color filter, double csat)
            : this()
        {
            ColorSaturation = csat;
            FilterColor = filter;
        }

        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        public double ColorSaturation
        {
            get
            {
                return ((double)(this.GetValue(ColorSaturationProperty)));
            }
            set
            {
                this.SetValue(ColorSaturationProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color FilterColor
        {
            get
            {
                return ((Color)(this.GetValue(FilterColorProperty)));
            }
            set
            {
                this.SetValue(FilterColorProperty, value);
            }
        }
    }
}
