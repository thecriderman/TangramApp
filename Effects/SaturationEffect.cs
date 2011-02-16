using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Effects;
using System.Windows;
using System.Windows.Media;

namespace TangramApp1._35.Effects
{
    public class SaturationEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(SaturationEffect), 0);
        public static readonly DependencyProperty RatioProperty = DependencyProperty.Register("Ratio", typeof(double), typeof(SaturationEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));
        
        public SaturationEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("pack://application:,,,/Resources/Shaders/Saturation.ps", UriKind.Absolute);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(RatioProperty);
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
        public double Ratio
        {
            get
            {
                return ((double)(this.GetValue(RatioProperty)));
            }
            set
            {
                this.SetValue(RatioProperty, value);
            }
        }
    }
}
