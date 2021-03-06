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
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItems = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCommands = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExact = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRelation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuDeleteRelation = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSetBegin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetTerminal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.menuRelation.SuspendLayout();
            this.menuStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuItems,
            this.menuCommands});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(788, 25);
            this.menuStripMain.TabIndex = 0;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuClear,
            this.menuLoad,
            this.menuSave,
            this.toolStripSeparator1,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(39, 21);
            this.menuFile.Text = "&File";
            // 
            // menuClear
            // 
            this.menuClear.Name = "menuClear";
            this.menuClear.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuClear.Size = new System.Drawing.Size(152, 22);
            this.menuClear.Text = "&Clear";
            this.menuClear.Click += new System.EventHandler(this.menuClear_Click);
            // 
            // menuLoad
            // 
            this.menuLoad.Name = "menuLoad";
            this.menuLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuLoad.Size = new System.Drawing.Size(152, 22);
            this.menuLoad.Text = "&Load";
            this.menuLoad.Click += new System.EventHandler(this.menuLoad_Click);
            // 
            // menuSave
            // 
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(152, 22);
            this.menuSave.Text = "&Save";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(152, 22);
            this.menuExit.Text = "&Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuItems
            // 
            this.menuItems.Name = "menuItems";
            this.menuItems.Size = new System.Drawing.Size(52, 21);
            this.menuItems.Text = "&Items";
            // 
            // menuCommands
            // 
            this.menuCommands.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExact,
            this.menuReset,
            this.toolStripSeparator2});
            this.menuCommands.Name = "menuCommands";
            this.menuCommands.Size = new System.Drawing.Size(86, 21);
            this.menuCommands.Text = "&Commands";
            // 
            // menuExact
            // 
            this.menuExact.Name = "menuExact";
            this.menuExact.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.menuExact.Size = new System.Drawing.Size(153, 22);
            this.menuExact.Text = "&Exact";
            this.menuExact.Click += new System.EventHandler(this.menuExact_Click);
            // 
            // menuReset
            // 
            this.menuReset.Name = "menuReset";
            this.menuReset.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuReset.Size = new System.Drawing.Size(153, 22);
            this.menuReset.Text = "&Reset";
            this.menuReset.Click += new System.EventHandler(this.menuReset_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // menuRelation
            // 
            this.menuRelation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDeleteRelation});
            this.menuRelation.Name = "menuRelation";
            this.menuRelation.Size = new System.Drawing.Size(114, 26);
            // 
            // menuDeleteRelation
            // 
            this.menuDeleteRelation.Name = "menuDeleteRelation";
            this.menuDeleteRelation.Size = new System.Drawing.Size(113, 22);
            this.menuDeleteRelation.Text = "&Delete";
            this.menuDeleteRelation.Click += new System.EventHandler(this.menuDeleteRelation_Click);
            // 
            // menuStatus
            // 
            this.menuStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetBegin,
            this.menuSetTerminal});
            this.menuStatus.Name = "menuStatus";
            this.menuStatus.Size = new System.Drawing.Size(163, 48);
            // 
            // menuSetBegin
            // 
            this.menuSetBegin.Name = "menuSetBegin";
            this.menuSetBegin.Size = new System.Drawing.Size(162, 22);
            this.menuSetBegin.Text = "Set as &begin";
            this.menuSetBegin.Click += new System.EventHandler(this.menuSetBegin_Click);
            // 
            // menuSetTerminal
            // 
            this.menuSetTerminal.Name = "menuSetTerminal";
            this.menuSetTerminal.Size = new System.Drawing.Size(162, 22);
            this.menuSetTerminal.Text = "Set as &terminal";
            this.menuSetTerminal.Click += new System.EventHandler(this.menuSetTerminal_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(788, 553);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitfsm Editor";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.menuRelation.ResumeLayout(false);
            this.menuStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItems;
        private System.Windows.Forms.ToolStripMenuItem menuCommands;
        private System.Windows.Forms.ToolStripMenuItem menuLoad;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuExact;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip menuRelation;
        private System.Windows.Forms.ContextMenuStrip menuStatus;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteRelation;
        private System.Windows.Forms.ToolStripMenuItem menuSetBegin;
        private System.Windows.Forms.ToolStripMenuItem menuSetTerminal;
        private System.Windows.Forms.ToolStripMenuItem menuReset;
        private System.Windows.Forms.ToolStripMenuItem menuClear;

    }
}

