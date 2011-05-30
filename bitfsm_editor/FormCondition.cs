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

namespace fsm
{
    public partial class FormCondition : Form
    {
        private Bitfsm bitfsm = null;

        private DialogResult dialogResult = DialogResult.Cancel;

        private HashSet<string> commands = null;

        public HashSet<string> Commands
        {
            get { return commands; }
        }

        private bool exact = false;

        public bool Exact
        {
            get { return exact; }
        }

        public FormCondition()
        {
            InitializeComponent();

            commands = new HashSet<string>();
        }

        public FormCondition(Bitfsm bf)
        {
            InitializeComponent();

            commands = new HashSet<string>();

            bitfsm = bf;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string[] cmds = txtConditions.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (cmds.Count() != 0)
            {
                foreach (string c in cmds)
                {
                    if ((Program.Form as FormMain).Bitfsm.CommandColl.Contains(c))
                    {
                        commands.Add(c);
                    }
                    else
                    {
                        MessageBox.Show(this, "There is no command named " + c, "Bitfsm Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                exact = cbExact.Checked;

                dialogResult = DialogResult.OK;
            }

            Close();
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return dialogResult;
        }

        private void FormCondition_Load(object sender, EventArgs e)
        {
            foreach (string s in bitfsm.CommandColl)
            {
                cmbSelector.Items.Add(s);
            }
        }

        private void cmbSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtConditions.Text))
            {
                txtConditions.Text += cmbSelector.SelectedItem.ToString();
            }
            else
            {
                txtConditions.Text += "\r\n" + cmbSelector.SelectedItem.ToString();
            }
        }
    }
}
