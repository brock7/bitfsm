/*
** This source file is part of BITFSM
**
** For the latest info, see http://code.google.com/p/bitfsm/
**
** Copyright (c) 2011 Tony & Tony's Toy Game Development Team
**
** Permission is hereby granted, free of charge, to any person obtaining a copy of
** this software and associated documentation files (the "Software"), to deal in
** the Software without restriction, including without limitation the rights to
** use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
** the Software, and to permit persons to whom the Software is furnished to do so,
** subject to the following conditions:
**
** The above copyright notice and this permission notice shall be included in all
** copies or substantial portions of the Software.
**
** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
** IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
** FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
** COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
** IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
** CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace fsm
{
    partial class StatusItem
    {
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        protected new void InitializeComponent()
        {
            this.labelIn = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.labelOut = new System.Windows.Forms.Label();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelIn
            // 
            this.labelIn.AutoSize = true;
            this.labelIn.BackColor = System.Drawing.Color.Lime;
            this.labelIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelIn.Location = new System.Drawing.Point(0, 0);
            this.labelIn.Name = "labelIn";
            this.labelIn.Size = new System.Drawing.Size(17, 12);
            this.labelIn.TabIndex = 1;
            this.labelIn.Text = "in";
            this.labelIn.DragDrop += new System.Windows.Forms.DragEventHandler(this.labelIn_DragDrop);
            this.labelIn.DragEnter += new System.Windows.Forms.DragEventHandler(this.labelIn_DragEnter);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.labelIn);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 17);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(18, 63);
            this.panelLeft.TabIndex = 2;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.labelOut);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(98, 17);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(23, 63);
            this.panelRight.TabIndex = 3;
            // 
            // labelOut
            // 
            this.labelOut.AutoSize = true;
            this.labelOut.BackColor = System.Drawing.Color.Cyan;
            this.labelOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelOut.Location = new System.Drawing.Point(0, 51);
            this.labelOut.Name = "labelOut";
            this.labelOut.Size = new System.Drawing.Size(23, 12);
            this.labelOut.TabIndex = 1;
            this.labelOut.Text = "out";
            this.labelOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelOut_MouseDown);
            // 
            // StatusItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Name = "StatusItem";
            this.TitleColor = System.Drawing.Color.White;
            this.Move += new System.EventHandler(this.StatusItem_Move);
            this.Controls.SetChildIndex(this.panelLeft, 0);
            this.Controls.SetChildIndex(this.panelRight, 0);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelIn;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label labelOut;

    }
}
