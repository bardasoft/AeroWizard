﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AeroWizard
{
    /// <summary>
    /// ImageButton
    /// </summary>
    [ToolboxBitmap(typeof(Button))]
    internal class ImageButton : ButtonBase
    {
        private const string defaultToolTip = "Returns to a previous page";

        private ToolTip toolTip;

        /// <summary>
        /// ImageButton
        /// </summary>
        public ImageButton()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Text = null;
            this.ButtonState = PushButtonState.Normal;
            toolTip = new ToolTip();
            toolTip.SetToolTip(this, defaultToolTip);
        }

        [DefaultValue((string)null),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return base.Text; } set { base.Text = value; }
        }

        [DefaultValue(defaultToolTip)]
        public string ToolTipText
        {
            get { return toolTip.GetToolTip(this); } set { toolTip.SetToolTip(this, value); }
        }

        protected PushButtonState ButtonState
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The default <see cref="T:System.Drawing.Size"/> of the control.
        /// </returns>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(30, 30);
            }
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>
        /// An ordered pair of type <see cref="T:System.Drawing.Size"/> representing the width and height of a rectangle.
        /// </returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            return DefaultSize;
        }

        /// <summary>
        /// For button user use to simulate a click operate.
        /// </summary>
        public void PerformClicked()
        {
            base.OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Process Enabled property changed 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            ButtonState = Enabled ? PushButtonState.Normal : PushButtonState.Disabled;
            Invalidate();
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;
            ButtonState = PushButtonState.Pressed;
            Invalidate();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseEnter(System.EventArgs)"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            ButtonState = PushButtonState.Hot;
            Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            ButtonState = Enabled ? PushButtonState.Normal : PushButtonState.Disabled;
            Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;
            ButtonState = this.Enabled ? PushButtonState.Hot : PushButtonState.Disabled;
            Invalidate();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            PaintButton(e.Graphics, e.ClipRectangle);
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected virtual void PaintButton(Graphics graphics, Rectangle bounds)
        {
            VisualStyleRenderer rnd = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
            rnd.DrawParentBackground(graphics, bounds, this);
            if (this.Image != null || (this.ImageList != null && this.ImageList.Images.Count == 1))
            {
                Image img = (this.Image != null) ? this.Image : this.ImageList.Images[0];
                if (Enabled)
                    rnd.DrawImage(graphics, bounds, img);
                else
                    ControlPaint.DrawImageDisabled(graphics, img, 0, 0, this.BackColor);
            }
            else if (this.ImageList != null && this.ImageList.Images.Count > 1)
            {
                int idx = (int)ButtonState - 1;
                if (this.ImageList.Images.Count == 2)
                    idx = ButtonState == PushButtonState.Disabled ? 1 : 0;
                if (this.ImageList.Images.Count == 3)
                    idx = ButtonState == PushButtonState.Normal ? 0 : idx - 1;
                rnd.DrawImage(graphics, bounds, this.ImageList, idx);
            }
            else
            {
                rnd.SetParameters(rnd.Class, rnd.Part, (int)ButtonState);
                rnd.DrawBackground(graphics, bounds);
            }
        }
    }
}