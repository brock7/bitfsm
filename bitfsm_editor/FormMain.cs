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

        public Bitfsm Bitfsm
        {
            get { return bitfsm; }
        }

        private List<Relation> relations = null;

        public FormMain()
        {
            InitializeComponent();

            bitfsm = new Bitfsm();

            relations = new List<Relation>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            bitfsm.Dispose();
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
                subMenu.Click += new EventHandler(menuCmd_Click);
                menuCommands.DropDownItems.Add(subMenu);
            }
        }

        private void menuCmd_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            string text = (sender as ToolStripMenuItem).Text;

            foreach (Control c in Controls)
            {
                if (c.Name == text)
                {
                    MessageBox.Show(this, "A status item named " + text + " already exists", "Bitfsm Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }
            }

            Random rand = new Random();
            int r = rand.Next(255);
            int g = rand.Next(255);
            int b = rand.Next(255);

            StatusItem si = new StatusItem();
            si.Location = new Point(Width / 2, Height / 2);
            si.TitleText = text;
            si.Name = text;
            si.TitleColor = Color.FromArgb(r, g, b);
            si.TitleTextColor = Color.FromArgb(255 - r, 255 - g, 255 - b);
            Controls.Add(si);

            si.AllowDrop = true;
            si.OnDragItem += (_sender, _e) =>
                {
                    (_sender as Control).DoDragDrop(_sender, DragDropEffects.Link);
                };
            si.OnDragEnterItem += (_sender, _e)=>
                {
                    if (_e.Data.GetDataPresent(typeof(StatusItem)))
                    {
                        StatusItem _si = _e.Data.GetData(typeof(StatusItem)) as StatusItem;
                        if (_si == _sender)
                        {
                            _e.Effect = DragDropEffects.None;
                        }
                        else
                        {
                            _e.Effect = DragDropEffects.Link;
                        }
                    }
                    else
                    {
                        _e.Effect = DragDropEffects.None;
                    }
                };
            si.OnDragDropItem += (_sender, _e) =>
                {
                    if (_e.Data.GetDataPresent(typeof(StatusItem)))
                    {
                        StatusItem _si = _e.Data.GetData(typeof(StatusItem)) as StatusItem;
                        if (_si != _sender)
                        {
                            link(_si, _sender as StatusItem);
                        }
                    }
                };
            si.Resize += (_sender, _e) =>
                {
                    Refresh();
                };
        }

        private void link(StatusItem src, StatusItem tgt)
        {
            FormCondition fc = new FormCondition(bitfsm);
            if (DialogResult.OK == fc.ShowDialog(this))
            {
                if (bitfsm.addRuleStep(src.Name, fc.Commands.ToArray(), tgt.Name, fc.Exact))
                {
                    Label dummy = new Label();
                    dummy.AutoSize = true;
                    dummy.Click += (sender, e) =>
                        {
                            menuRelation.Tag = dummy;
                            menuRelation.Show(dummy, 30, 20);
                        };
                    Controls.Add(dummy);
                    Relation r = new Relation(src, tgt, fc.Commands, fc.Exact, dummy);
                    relations.Add(r);
                    dummy.Tag = r;

                    Refresh();
                }
                else
                {
                    MessageBox.Show(this, "Command(s) add failed", "Bitfsm Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

                r.Dummy.Text = r.CommandsString;
                r.Dummy.ForeColor = r.Exact ? Color.Red : Color.White;
                r.Dummy.Location = new Point(p2.X, p1.Y + (p4.Y - p1.Y) / 2);
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

                    bitfsm.removeRuleStep(r.Source.Name, r.Commands.ToArray());
                }
                tl.Clear();

                Refresh();
            }
        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitfsm save file(*.fsm)|*.fsm|(All files(*.*)|*.*";
            if (DialogResult.OK == ofd.ShowDialog(this))
            {
                bitfsm.readRuleSteps(ofd.FileName);

                // TODO
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            FormSaveFsm fsf = new FormSaveFsm(bitfsm);
            if (DialogResult.OK == fsf.ShowDialog(this))
            {
                bitfsm.writeRuleSteps(fsf.FileName, fsf.StatusCount, fsf.CommandCount);
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuExact_Click(object sender, EventArgs e)
        {
            menuExact.Checked = !menuExact.Checked;
        }

        private void menuDeleteRelation_Click(object sender, EventArgs e)
        {
            Label dummy = menuRelation.Tag as Label;
            if (DialogResult.Yes == MessageBox.Show(this, "Delete relation " + dummy.Text + "?", "Bitfsm Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Relation r = dummy.Tag as Relation;
                if (bitfsm.removeRuleStep(r.Source.Name, r.Commands.ToArray()))
                {
                    relations.Remove(r);

                    Controls.Remove(dummy);
                    dummy.Dispose();

                    Refresh();
                }
            }
        }

        private void menuSetBegin_Click(object sender, EventArgs e)
        {

        }

        private void menuSetTerminal_Click(object sender, EventArgs e)
        {

        }
    }
}
