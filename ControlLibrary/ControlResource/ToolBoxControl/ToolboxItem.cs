using Careful.Controls.ActivityControl;
using Careful.Controls.Common.Activity;
using Careful.Core.Extensions;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Careful.Controls.ToolBoxControl
{
    public class ToolboxItem : ContentControl
    {
        // caches the start point of the drag operation
        private Point? dragStartPoint = null;

        static ToolboxItem()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ToolboxItem), new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            if (this.dragStartPoint.HasValue)
            {
                if (!(e.OriginalSource is TextBlock))
                    return;
                Activity activity = new Activity();
                activity.ActivityName = (e.OriginalSource as TextBlock).Text;
                //var sb = new StringBuilder();
                //var writer = XmlWriter.Create(sb, new XmlWriterSettings
                //{
                //    Indent = true,
                //    ConformanceLevel = ConformanceLevel.Fragment,
                //    OmitXmlDeclaration = true,
                //    NamespaceHandling = NamespaceHandling.OmitDuplicates,
                //});
                //var mgr = new XamlDesignerSerializationManager(writer);
                //mgr.XamlWriterMode = XamlWriterMode.Expression;
                //XamlWriter.Save(activity, mgr);
                //Object content = XamlReader.Load(XmlReader.Create(new StringReader(sb.ToString())));
                DragObject dataObject = new DragObject();
                dataObject.DesignControl = activity;

                WrapPanel panel = VisualTreeHelper.GetParent(this) as WrapPanel;
                if (panel != null)
                {
                    // desired size for DesignerCanvas is the stretched Toolbox item size
                    double scale = 1.3;
                    dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
                }

                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);

                e.Handled = true;
            }
            
        }
    }
}
