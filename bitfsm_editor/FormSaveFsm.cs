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
    public partial class FormSaveFsm : Form
    {
        private Bitfsm bitfsm = null;

        private DialogResult dialogResult = DialogResult.Cancel;

        public int StatusCount
        {
            get { return (int)numStatusCount.Value; }
        }

        public int CommandCount
        {
            get { return (int)numCommandCount.Value; }
        }

        public string FileName
        {
            get { return txtPath.Text; }
        }

        public FormSaveFsm()
        {
            InitializeComponent();
        }

        public FormSaveFsm(Bitfsm bf)
        {
            InitializeComponent();

            bitfsm = bf;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Bitfsm save file(*.fsm)|*.fsm|(All files(*.*)|*.*";
            if (DialogResult.OK == sfd.ShowDialog(this))
            {
                txtPath.Text = sfd.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.OK;
            Close();
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Bitfsm save file(*.fsm)|*.fsm|(All files(*.*)|*.*";
            if (DialogResult.OK == sfd.ShowDialog(this))
            {
                txtPath.Text = sfd.FileName;

                base.ShowDialog(owner);
            }

            return dialogResult;
        }
    }
}
