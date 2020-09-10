using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlResource.CustomControls.PagingControl
{
    public class PagingPanel : Panel
    {
        public PagingPanel()
        {
            LayoutUpdated += new EventHandler(ZapPanel_LayoutUpdated);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return null;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size max = new Size();

            UIElement child;
            for (int i = 0; i < this.InternalChildren.Count; i++)
            {
                child = this.InternalChildren[i];
                child.Measure(availableSize);
                max.Width = Math.Max(max.Width, child.DesiredSize.Width);
                max.Height = Math.Max(max.Height, child.DesiredSize.Height);
            }

            Size returnSize;
            if (double.IsInfinity(availableSize.Width) || double.IsInfinity(availableSize.Height))
            {
                returnSize = new Size(max.Width * this.InternalChildren.Count, max.Height);
            }
            else
            {
                returnSize = new Size(availableSize.Width * this.InternalChildren.Count, availableSize.Height);
            }

            return returnSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_visualParent != null)
            {
                _lastVisualParentSize = _visualParent.RenderSize;
            }
            else
            {
                _lastVisualParentSize = finalSize;
            }

            UIElement child;
            for (int i = 0; i < this.InternalChildren.Count; i++)
            {
                child = this.InternalChildren[i];
                Rect arrangeRect = new Rect(new Point(_lastVisualParentSize.Width * i, 0), _lastVisualParentSize);
                child.Arrange(arrangeRect);
            }

            return new Size(_lastVisualParentSize.Width * InternalChildren.Count, _lastVisualParentSize.Height);
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            _visualParent = this.VisualParent as FrameworkElement;
        }

        #region implementation

        private void ZapPanel_LayoutUpdated(object sender, EventArgs e)
        {
            if (_visualParent != null)
            {
                if (_visualParent.RenderSize != _lastVisualParentSize)
                {
                    InvalidateArrange();
                }
            }
        }

        private Size _lastVisualParentSize;
        private FrameworkElement _visualParent;

        #endregion
    }
}
