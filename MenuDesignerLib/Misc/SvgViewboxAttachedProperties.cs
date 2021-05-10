using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpVectors.Converters;

namespace MenuDesigner
{
    /// <MetaDataID>{e7779430-3f89-4847-8ed9-885fe1fc8cd8}</MetaDataID>
    public class SvgViewboxAttachedProperties : DependencyObject
    {
        public static string GetSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var svgControl = obj as SvgViewbox;
            if (svgControl != null)
            {
                try
                {
                    if (e.NewValue != null && !string.IsNullOrWhiteSpace(e.NewValue as string))
                        svgControl.Source = new Uri((string)e.NewValue);
                    else
                        svgControl.Source = null;
                }
                catch (Exception error)
                {
                }
            }
        }
        public static readonly DependencyProperty SourceProperty =
        DependencyProperty.RegisterAttached("Source",
            typeof(string), typeof(SvgViewboxAttachedProperties),
            // default value: null
            new PropertyMetadata(null, OnSourceChanged));



        public static System.IO.Stream GetSourceStream(DependencyObject obj)
        {
            return obj.GetValue(SourceStreamProperty) as System.IO.Stream;
        }

        public static void SetSourceStream(DependencyObject obj, System.IO.Stream value)
        {
            obj.SetValue(SourceStreamProperty, value);
        }

        private static void OnSourceStreamChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var svgControl = obj as SvgViewbox;
            if (svgControl != null)
            {
                try
                {
                    if (e.NewValue != null)
                    {
                        if((e.NewValue as System.IO.Stream).CanRead)
                            svgControl.SourceStream = e.NewValue as System.IO.Stream;
                        else
                        {
                            int rt =3;

                        }
                    }
                    else
                        svgControl.Source = null;
                }
                catch (Exception error)
                {
                }
            }
        }

        public static readonly DependencyProperty SourceStreamProperty =
    DependencyProperty.RegisterAttached("SourceStream",
        typeof(System.IO.Stream), typeof(SvgViewboxAttachedProperties),
        // default value: null
        new PropertyMetadata(null, OnSourceStreamChanged));


        public static System.Windows.Media.DrawingGroup GetDrawing(DependencyObject obj)
        {
            return obj.GetValue(DrawingProperty) as System.Windows.Media.DrawingGroup;
        }

        public static void SetDrawing(DependencyObject obj, System.Windows.Media.DrawingGroup value)
        {
            obj.SetValue(DrawingProperty, value);
        }

        private static void OnDrawingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var svgControl = obj as SvgViewbox;
            if (svgControl != null)
            {
                try
                {
                    if (e.NewValue != null)
                        svgControl.Drawing = e.NewValue as System.Windows.Media.DrawingGroup;
                    else
                        svgControl.Source = null;
                }
                catch (Exception error)
                {
                }
            }
        }

        public static readonly DependencyProperty DrawingProperty =
        DependencyProperty.RegisterAttached("Drawing",
        typeof(System.Windows.Media.DrawingGroup), typeof(SvgViewboxAttachedProperties),
        // default value: null
        new PropertyMetadata(null, OnDrawingChanged));
    }
}
