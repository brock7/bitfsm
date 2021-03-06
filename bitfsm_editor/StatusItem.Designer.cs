﻿/*
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
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
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
            this.panelLeft.Size = new System.Drawing.Size(18, 49);
            this.panelLeft.TabIndex = 2;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.labelOut);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(93, 17);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(23, 49);
            this.panelRight.TabIndex = 3;
            // 
            // labelOut
            // 
            this.labelOut.AutoSize = true;
            this.labelOut.BackColor = System.Drawing.Color.Cyan;
            this.labelOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelOut.Location = new System.Drawing.Point(0, 37);
            this.labelOut.Name = "labelOut";
            this.labelOut.Size = new System.Drawing.Size(23, 12);
            this.labelOut.TabIndex = 1;
            this.labelOut.Text = "out";
            this.labelOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelOut_MouseDown);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelStatus.Location = new System.Drawing.Point(18, 54);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(53, 12);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "        ";
            this.labelStatus.Click += new System.EventHandler(this.label_Click);
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCurrent.Location = new System.Drawing.Point(18, 17);
            this.labelCurrent.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(53, 12);
            this.labelCurrent.TabIndex = 5;
            this.labelCurrent.Text = "        ";
            this.labelCurrent.Click += new System.EventHandler(this.label_Click);
            // 
            // StatusItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.labelCurrent);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.MinimumSize = new System.Drawing.Size(116, 40);
            this.Name = "StatusItem";
            this.Size = new System.Drawing.Size(116, 66);
            this.TitleColor = System.Drawing.Color.White;
            this.Move += new System.EventHandler(this.StatusItem_Move);
            this.Controls.SetChildIndex(this.panelLeft, 0);
            this.Controls.SetChildIndex(this.panelRight, 0);
            this.Controls.SetChildIndex(this.labelStatus, 0);
            this.Controls.SetChildIndex(this.labelCurrent, 0);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIn;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label labelOut;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelCurrent;

    }
}
