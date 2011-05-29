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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace fsm
{
    public partial class IDragableItem : UserControl
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hWnd, uint wMsg, uint wParam, uint lParam);
        [DllImport("user32.dll")]
        private static extern int ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        private const uint WM_SYSCOMMAND = 0x0112;
        private const uint SC_MOVE = 0xF010;
        private const uint HTCAPTION = 0x0002;
        private const int GWL_STYLE = (-16);
        private const int WS_THICKFRAME = 0x40000;
        private const int SWP_NOSIZE = 0x1;
        private const int SWP_NOMOVE = 0x2;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_FRAMECHANGED = 0x20;

        public virtual object Data
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Title text
        /// </summary>
        public string TitleText
        {
            get { return labelTitle.Text; }
            set { labelTitle.Text = value; }
        }

        /// <summary>
        /// Color for title bar
        /// </summary>
        public Color TitleColor
        {
            get { return toolStrip.BackColor; }
            set
            {
                toolStrip.BackColor = value;
                labelTitle.BackColor = value;
            }
        }

        /// <summary>
        /// Is this control closable
        /// </summary>
        public bool Closable
        {
            get { return toolStripButtonClose.Visible; }
            set { toolStripButtonClose.Visible = value; }
        }

        public IDragableItem()
        {
            InitializeComponent();
            setSizable();
            this.Click += new EventHandler(bringFront);
        }

        public virtual void AssignTo(Button tgt) { }

        public virtual void PerformPaste() { }

        protected void bringFront(object sender, EventArgs e)
        {
            ReleaseCapture();
            SendMessage((this as Control).Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        // Set control sizable
        private void setSizable()
        {
            int style = GetWindowLong(this.Handle, GWL_STYLE);
            style |= WS_THICKFRAME;
            SetWindowLong(this.Handle, GWL_STYLE, style);
            SetWindowPos(this.Handle, this.Handle, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_FRAMECHANGED);
        }

        // Move items
        private void moveItem(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage((this as Control).Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        // Close
        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
