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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fsm
{
    public partial class StatusItem : IDragableItem
    {
        public delegate void DragItemEventHandler(object sender, MouseEventArgs e);

        public event DragItemEventHandler OnDragItem;

        public delegate void DragEnterItemEventHandler(object sender, DragEventArgs e);

        public event DragEnterItemEventHandler OnDragEnterItem;

        public delegate void DragDropItemEventHandler(object sender, DragEventArgs e);

        public event DragDropItemEventHandler OnDragDropItem;

        public Point CenterLocation
        {
            get { return new Point(Location.X + Width / 2, Location.Y + Height / 2); }
        }

        public Point TopleftLocation
        {
            get { return new Point(Location.X, Location.Y); }
        }

        public Point ToprightLocation
        {
            get { return new Point(Location.X + Width, Location.Y); }
        }

        public Point BottomleftLocation
        {
            get { return new Point(Location.X, Location.Y + Height); }
        }

        public Point BottomrightLocation
        {
            get { return new Point(Location.X + Width, Location.Y + Height); }
        }

        public StatusItem()
        {
            InitializeComponent();

            labelOut.AllowDrop = true;
            labelIn.AllowDrop = true;
        }

        protected override void Dispose(bool disposing)
        {
            (Program.Form as FormMain).RemoveItem(this);

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void StatusItem_Move(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                Parent.Refresh();
            }
        }

        private void labelOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (null != OnDragItem)
            {
                OnDragItem(this, e);
            }
        }

        private void labelIn_DragEnter(object sender, DragEventArgs e)
        {
            if (null != OnDragEnterItem)
            {
                OnDragEnterItem(this, e);
            }
        }

        private void labelIn_DragDrop(object sender, DragEventArgs e)
        {
            if (null != OnDragDropItem)
            {
                OnDragDropItem(this, e);
            }
        }
    }
}
