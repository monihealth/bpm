using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace MoniHealth.iOS
{
    class PlotViewRenderer : OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            this.Control.InvalidatePlot(false);
        }
    }
}