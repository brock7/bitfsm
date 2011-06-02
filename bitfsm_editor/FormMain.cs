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
            bitfsm.Stepped += new Bitfsm.StepHandler(bitfsm_Stepped);

            relations = new List<Relation>();
        }

        private void bitfsm_Stepped(object sender, Bitfsm.StepEventArgs e)
        {
            StatusItem srci = Controls[e.sourceTag] as StatusItem;
            StatusItem tgti = Controls[e.targetTag] as StatusItem;
            srci.SteppingText = string.Empty;

            tgti.SteppingText = bitfsm.terminated() ? "Done" : "Current";
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
            if (!File.Exists(globalCfg))
            {
                globalCfg = Directory.GetParent(Application.ExecutablePath).FullName + "\\global_config.cfg";
                if (!File.Exists(globalCfg))
                {
                    throw new Exception("Cannot find file global_config.cfg");
                }
            }
            if (!File.Exists(statusCfg))
            {
                statusCfg = Directory.GetParent(Application.ExecutablePath).FullName + "\\status_config.cfg";
                if (!File.Exists(statusCfg))
                {
                    throw new Exception("Cannot find file status_config.cfg");
                }
            }
            if (!File.Exists(commandCfg))
            {
                statusCfg = Directory.GetParent(Application.ExecutablePath).FullName + "\\command_config.cfg";
                if (!File.Exists(commandCfg))
                {
                    throw new Exception("Cannot find file command_config.cfg");
                }
            }
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
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            if (bitfsm.walk(mi.Name, menuExact.Checked))
            {
            }
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            string text = (sender as ToolStripMenuItem).Text;
            addStatusItem(text);
        }

        StatusItem addStatusItem(string text)
        {
            foreach (Control c in Controls)
            {
                if (c.Name == text)
                {
                    MessageBox.Show(this, "A status item named " + text + " already exists", "Bitfsm Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return null;
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
            si.MouseDown += (_sender, _e) =>
                {
                    if (_e.Button == MouseButtons.Right)
                    {
                        menuStatus.Tag = si;
                        menuStatus.Show(si, 30, 20);
                    }
                };

            return si;
        }

        private void link(StatusItem src, StatusItem tgt)
        {
            FormCondition fc = new FormCondition(bitfsm);
            if (DialogResult.OK == fc.ShowDialog(this))
            {
                link(src, fc.Commands, tgt, fc.Exact, false);
            }
        }

        private void link(StatusItem src, HashSet<string> commands, StatusItem tgt, bool exact, bool force)
        {
            if (bitfsm.addRuleStep(src.Name, commands.ToArray(), tgt.Name, exact) || force)
            {
                string ln = "DUMMY_" + src.Name + "_" + tgt.Name;
                Label dummy = null;
                if (!Controls.ContainsKey(ln))
                {
                    dummy = new Label();
                    dummy.Name = ln;
                    dummy.AutoSize = true;
                    dummy.TextAlign = ContentAlignment.MiddleCenter;
                    dummy.BorderStyle = BorderStyle.FixedSingle;
                    dummy.MouseDown += (_sender, _e) =>
                        {
                            if (_e.Button == MouseButtons.Right)
                            {
                                menuRelation.Tag = dummy;
                                menuRelation.Show(dummy, 30, 20);
                            }
                        };
                    Controls.Add(dummy);
                    dummy.Tag = new List<Relation>();
                }
                else
                {
                    dummy = Controls[ln] as Label;
                }
                Relation r = new Relation(src, tgt, commands, exact);
                relations.Add(r);
                (dummy.Tag as List<Relation>).Add(r);

                Refresh();
            }
            else
            {
                MessageBox.Show(this, "Command(s) add failed", "Bitfsm Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                Color col = Color.FromArgb(
                    (r.Source.TitleColor.R + r.Target.TitleColor.R) / 2,
                    (r.Source.TitleColor.G + r.Target.TitleColor.G) / 2,
                    (r.Source.TitleColor.B + r.Target.TitleColor.B) / 2
                );
                e.Graphics.DrawBezier(new Pen(col), p1, p2, p3, p4);

                int cr = 5;
                e.Graphics.FillEllipse(Brushes.Red, p4.X - cr, p4.Y - cr, cr * 2, cr * 2);
            }
        }

        public override void Refresh()
        {
            foreach (Control l in Controls)
            {
                if (l.GetType() == typeof(Label))
                {
                    l.Text = string.Empty;
                }
            }
            foreach (Relation r in relations)
            {
                Point p1 = r.Source.BottomrightLocation;
                Point p4 = r.Target.TopleftLocation;
                Point p2 = new Point(p1.X + (p4.X - p1.X) / 2, p1.Y);
                Point p3 = new Point(p2.X + (p4.X - p1.X) / 2, p4.Y);

                string ln = "DUMMY_" + r.Source.Name + "_" + r.Target.Name;
                if (Controls.ContainsKey(ln))
                {
                    Label dummy = Controls[ln] as Label;
                    if (!string.IsNullOrEmpty(dummy.Text))
                    {
                        dummy.Text += "\nor\n";
                    }
                    Color col = Color.FromArgb(
                        (r.Source.TitleColor.R + r.Target.TitleColor.R) / 2,
                        (r.Source.TitleColor.G + r.Target.TitleColor.G) / 2,
                        (r.Source.TitleColor.B + r.Target.TitleColor.B) / 2
                    );
                    dummy.ForeColor = col;
                    dummy.Text += r.CommandsString;
                    dummy.Location = new Point(p2.X, p1.Y + (p4.Y - p1.Y) / 2);
                }
            }

            base.Refresh();
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
                menuClear_Click(sender, e);

                if (bitfsm.readRuleSteps(ofd.FileName))
                {
                    int sn = bitfsm.getStatusCount();
                    for (int i = 0; i < sn; ++i)
                    {
                        int cn = bitfsm.getCommandCount(i);
                        string srcStr = bitfsm.getStatusTag(i);
                        for (int j = 0; j < cn; ++j)
                        {
                            bool exact = bitfsm.getStepCommandExact(i, j);
                            int next = bitfsm.getStepCommandNext(i, j);
                            List<string> condition = bitfsm.getStepCommandCondition(i, j);

                            string tgtStr = bitfsm.getStatusTag(next);

                            StatusItem src = Controls[srcStr] as StatusItem;
                            StatusItem tgt = Controls[tgtStr] as StatusItem;
                            if (src == null)
                            {
                                src = addStatusItem(srcStr);
                            }
                            if (tgt == null)
                            {
                                tgt = addStatusItem(tgtStr);
                            }
                            HashSet<string> commands = new HashSet<string>();
                            foreach (string c in condition)
                            {
                                commands.Add(c);
                            }
                            link(src, commands, tgt, exact, true);
                        }
                    }
                }
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
                List<Relation> _relations = dummy.Tag as List<Relation>;
                foreach (Relation r in _relations)
                {
                    if (bitfsm.removeRuleStep(r.Source.Name, r.Commands.ToArray()))
                    {
                        relations.Remove(r);
                    }
                }
                _relations.Clear();

                Controls.Remove(dummy);
                dummy.Dispose();

                Refresh();
            }
        }

        private void menuSetBegin_Click(object sender, EventArgs e)
        {
            StatusItem si = menuStatus.Tag as StatusItem;
            if (bitfsm.setCurrentStep(si.Name))
            {
                foreach (Control c in Controls)
                {
                    if (c.GetType() == typeof(StatusItem) && si != c && (c as StatusItem).StatusText != "T")
                    {
                        StatusItem sit = c as StatusItem;
                        if (sit.StatusText == "ST")
                        {
                            sit.StatusText = "T";
                        }
                        else
                        {
                            sit.StatusText = string.Empty;
                        }

                        sit.SteppingText = string.Empty;
                    }
                }

                if (si.StatusText == "T" || si.StatusText == "ST")
                {
                    si.StatusText = "ST";
                }
                else
                {
                    si.StatusText = "S";
                }

                si.SteppingText = "Begin";
            }
        }

        private void menuSetTerminal_Click(object sender, EventArgs e)
        {
            StatusItem si = menuStatus.Tag as StatusItem;
            if (bitfsm.setTerminalStep(si.Name))
            {
                foreach (Control c in Controls)
                {
                    if (c.GetType() == typeof(StatusItem) && si != c && (c as StatusItem).StatusText != "S")
                    {
                        StatusItem sit = c as StatusItem;
                        if (sit.StatusText == "ST")
                        {
                            sit.StatusText = "S";
                        }
                        else
                        {
                            sit.StatusText = string.Empty;
                        }

                        if (sit.SteppingText != "Begin")
                        {
                            sit.SteppingText = string.Empty;
                        }
                    }
                }

                if (si.StatusText == "S" || si.StatusText == "ST")
                {
                    si.StatusText = "ST";
                }
                else
                {
                    si.StatusText = "T";
                }
            }
        }

        private void menuReset_Click(object sender, EventArgs e)
        {
            menuClear_Click(sender, e);
            bitfsm.reset();
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            List<Control> tobecleared = new List<Control>();
            foreach (Control c in Controls)
            {
                if (c != menuStripMain)
                {
                    tobecleared.Add(c);
                }
            }
            foreach (Control c in tobecleared)
            {
                Controls.Remove(c);
            }
            relations.Clear();

            bitfsm.clear();

            Refresh();
        }
    }
}
