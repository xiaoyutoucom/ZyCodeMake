namespace ZyCodeMake
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnRead = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDbConnect = new System.Windows.Forms.TextBox();
            this.clb_Tables = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCodeMakeAll = new System.Windows.Forms.Button();
            this.btnOpenPath = new System.Windows.Forms.Button();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAllTables = new System.Windows.Forms.CheckBox();
            this.chkModel = new System.Windows.Forms.CheckBox();
            this.chkRepository = new System.Windows.Forms.CheckBox();
            this.chkInterface = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(811, 25);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "连接字符串：";
            // 
            // txtDbConnect
            // 
            this.txtDbConnect.Location = new System.Drawing.Point(97, 27);
            this.txtDbConnect.Name = "txtDbConnect";
            this.txtDbConnect.Size = new System.Drawing.Size(695, 21);
            this.txtDbConnect.TabIndex = 2;
            this.txtDbConnect.Text = "Server=192.168.1.244;Port=5432;Database=CityLawEnforce;User Id=postgres;Password=" +
    "admin;";
            // 
            // clb_Tables
            // 
            this.clb_Tables.CheckOnClick = true;
            this.clb_Tables.FormattingEnabled = true;
            this.clb_Tables.Location = new System.Drawing.Point(16, 143);
            this.clb_Tables.Name = "clb_Tables";
            this.clb_Tables.Size = new System.Drawing.Size(872, 276);
            this.clb_Tables.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "数据库表：";
            // 
            // btnCodeMakeAll
            // 
            this.btnCodeMakeAll.ForeColor = System.Drawing.Color.Crimson;
            this.btnCodeMakeAll.Location = new System.Drawing.Point(482, 443);
            this.btnCodeMakeAll.Name = "btnCodeMakeAll";
            this.btnCodeMakeAll.Size = new System.Drawing.Size(236, 23);
            this.btnCodeMakeAll.TabIndex = 5;
            this.btnCodeMakeAll.Text = "【生成选择表的数据仓储代码】";
            this.btnCodeMakeAll.UseVisualStyleBackColor = true;
            this.btnCodeMakeAll.Click += new System.EventHandler(this.btnCodeMakeAll_Click);
            // 
            // btnOpenPath
            // 
            this.btnOpenPath.Location = new System.Drawing.Point(742, 443);
            this.btnOpenPath.Name = "btnOpenPath";
            this.btnOpenPath.Size = new System.Drawing.Size(144, 23);
            this.btnOpenPath.TabIndex = 6;
            this.btnOpenPath.Text = "打开代码所在文件夹";
            this.btnOpenPath.UseVisualStyleBackColor = true;
            this.btnOpenPath.Click += new System.EventHandler(this.btnOpenPath_Click);
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(97, 68);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(439, 21);
            this.txtNameSpace.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "命名空间：";
            // 
            // chkAllTables
            // 
            this.chkAllTables.AutoSize = true;
            this.chkAllTables.Location = new System.Drawing.Point(96, 111);
            this.chkAllTables.Name = "chkAllTables";
            this.chkAllTables.Size = new System.Drawing.Size(48, 16);
            this.chkAllTables.TabIndex = 9;
            this.chkAllTables.Text = "全选";
            this.chkAllTables.UseVisualStyleBackColor = true;
            this.chkAllTables.CheckedChanged += new System.EventHandler(this.chkAllTables_CheckedChanged);
            // 
            // chkModel
            // 
            this.chkModel.AutoSize = true;
            this.chkModel.Checked = true;
            this.chkModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkModel.Location = new System.Drawing.Point(85, 443);
            this.chkModel.Name = "chkModel";
            this.chkModel.Size = new System.Drawing.Size(54, 16);
            this.chkModel.TabIndex = 10;
            this.chkModel.Text = "Model";
            this.chkModel.UseVisualStyleBackColor = true;
            // 
            // chkRepository
            // 
            this.chkRepository.AutoSize = true;
            this.chkRepository.Checked = true;
            this.chkRepository.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRepository.Location = new System.Drawing.Point(158, 443);
            this.chkRepository.Name = "chkRepository";
            this.chkRepository.Size = new System.Drawing.Size(84, 16);
            this.chkRepository.TabIndex = 11;
            this.chkRepository.Text = "Repository";
            this.chkRepository.UseVisualStyleBackColor = true;
            // 
            // chkInterface
            // 
            this.chkInterface.AutoSize = true;
            this.chkInterface.Checked = true;
            this.chkInterface.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInterface.Location = new System.Drawing.Point(248, 443);
            this.chkInterface.Name = "chkInterface";
            this.chkInterface.Size = new System.Drawing.Size(78, 16);
            this.chkInterface.TabIndex = 12;
            this.chkInterface.Text = "InterFace";
            this.chkInterface.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 443);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "生成文件：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(552, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "【必填】";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 479);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkInterface);
            this.Controls.Add(this.chkRepository);
            this.Controls.Add(this.chkModel);
            this.Controls.Add(this.chkAllTables);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNameSpace);
            this.Controls.Add(this.btnOpenPath);
            this.Controls.Add(this.btnCodeMakeAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.clb_Tables);
            this.Controls.Add(this.txtDbConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.Text = "PostgreSQL数据库C#代码生成器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDbConnect;
        private System.Windows.Forms.CheckedListBox clb_Tables;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCodeMakeAll;
        private System.Windows.Forms.Button btnOpenPath;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkAllTables;
        private System.Windows.Forms.CheckBox chkModel;
        private System.Windows.Forms.CheckBox chkRepository;
        private System.Windows.Forms.CheckBox chkInterface;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

