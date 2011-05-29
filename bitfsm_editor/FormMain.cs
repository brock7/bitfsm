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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using fsm;

using Dict = System.Collections.Generic.Dictionary<string, int>.KeyCollection;

namespace fsm
{
    public partial class FormMain : Form
    {
        private Bitfsm bitfsm = null;

        private List<Relation> relations = null;

        public FormMain()
        {
            InitializeComponent();

            bitfsm = new Bitfsm();

            relations = new List<Relation>();
        }

        protected override void Dispose(bool disposing)
        {
            bitfsm.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            string globalCfg = Directory.GetParent(Application.ExecutablePath).FullName + "\\..\\global_config.cfg";
            string statusCfg = Directory.GetParent(Application.ExecutablePath).FullName + "\\..\\status_config.cfg";
            string commandCfg = Directory.GetParent(Application.ExecutablePath).FullName + "\\..\\command_config.cfg";
            bitfsm.config(globalCfg, statusCfg, commandCfg);

            Dict statusColl = bitfsm.StatusColl;
            Dict commandColl = bitfsm.CommandColl;

            foreach (string status in statusColl)
            {
                ToolStripMenuItem subMenu = new ToolStripMenuItem();
                subMenu.Text = status;
                subMenu.Name = status;
                subMenu.Click += new EventHandler(menuAdd_Click);
                menuItems.DropDownItems.Add(subMenu);
            }

            foreach (string cmd in commandColl)
            {
                ToolStripMenuItem subMenu = new ToolStripMenuItem();
                subMenu.Text = cmd;
                subMenu.Name = cmd;
                subMenu.Click += new EventHandler(menuAdd_Click);
                menuCommands.DropDownItems.Add(subMenu);
            }
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            string text = (sender as ToolStripMenuItem).Text;

            StatusItem si = new StatusItem();
            si.Location = new Point(Width / 2, Height / 2);
            si.TitleText = text;
            si.Name = text;
            si.TitleColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            Controls.Add(si);

            si.AllowDrop = true;
            si.MouseDown += new MouseEventHandler(status_MouseDown);
            si.DragEnter += new DragEventHandler(status_DragEnter);
            si.DragDrop += new DragEventHandler(status_DragDrop);
        }

        private void status_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(sender, DragDropEffects.Link);
        }

        private void status_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(StatusItem)))
            {
                StatusItem si = e.Data.GetData(typeof(StatusItem)) as StatusItem;
                if (si == sender)
                {
                    e.Effect = DragDropEffects.None;
                }
                else
                {
                    e.Effect = DragDropEffects.Link;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void status_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(StatusItem)))
            {
                StatusItem si = e.Data.GetData(typeof(StatusItem)) as StatusItem;
                if (si != sender)
                {
                    link(si, sender as StatusItem);
                }
            }
        }

        private void link(StatusItem src, StatusItem tgt)
        {
            FormCondition fc = new FormCondition(bitfsm);
            if (DialogResult.OK == fc.ShowDialog(this))
            {
                relations.Add(new Relation(src, tgt, fc.Commands));
                Refresh();
            }
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            foreach (Relation r in relations)
            {
                Point p1 = r.Source.BottomrightLocation;
                Point p4 = r.Target.TopleftLocation;
                Point p2 = new Point(p1.X + (p4.X - p1.X) / 2, p1.Y);
                Point p3 = new Point(p2.X + (p4.X - p1.X) / 2, p4.Y);

                e.Graphics.DrawBezier(Pens.White, p1, p2, p3, p4);

                int cr = 5;
                e.Graphics.FillEllipse(Brushes.Red, p4.X - cr, p4.Y - cr, cr * 2, cr * 2);
            }
        }

        public void RemoveItem(StatusItem si)
        {
            List<Relation> tl = new List<Relation>();

            foreach (Relation r in relations)
            {
                if (r.Source == si || r.Target == si)
                {
                    tl.Add(r);
                }
            }

            if (tl.Count != 0)
            {
                foreach (Relation r in tl)
                {
                    relations.Remove(r);
                }
                tl.Clear();

                Refresh();
            }
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {

        }

        private void menuSave_Click(object sender, EventArgs e)
        {

        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
