namespace BugsBugs
{
    partial class mainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.cmbBitrate = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkboxMkDir = new System.Windows.Forms.CheckBox();
            this.chkBoxAlert = new System.Windows.Forms.CheckBox();
            this.chkBoxTop100 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabSearchMusic = new System.Windows.Forms.TabPage();
            this.btnTop100 = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtBoxKeyword = new System.Windows.Forms.TextBox();
            this.cmbBoxSearchType = new System.Windows.Forms.ComboBox();
            this.tabDownload = new System.Windows.Forms.TabPage();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.ColmArtist = new System.Windows.Forms.ColumnHeader();
            this.ColmAlbum = new System.Windows.Forms.ColumnHeader();
            this.ColmMusic = new System.Windows.Forms.ColumnHeader();
            this.lstViewSearch = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.tabConfig.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabSearchMusic.SuspendLayout();
            this.mainTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.cmbBitrate);
            this.tabConfig.Controls.Add(this.groupBox2);
            this.tabConfig.Controls.Add(this.label5);
            this.tabConfig.Controls.Add(this.button1);
            this.tabConfig.Controls.Add(this.label1);
            this.tabConfig.Controls.Add(this.textBox1);
            this.tabConfig.Location = new System.Drawing.Point(4, 21);
            this.tabConfig.Margin = new System.Windows.Forms.Padding(0);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Size = new System.Drawing.Size(546, 276);
            this.tabConfig.TabIndex = 2;
            this.tabConfig.Text = "환경설정";
            this.tabConfig.UseVisualStyleBackColor = true;
            // 
            // cmbBitrate
            // 
            this.cmbBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBitrate.FormattingEnabled = true;
            this.cmbBitrate.Items.AddRange(new object[] {
            "128 kbps",
            "192 kbps",
            "320 kbps"});
            this.cmbBitrate.Location = new System.Drawing.Point(70, 156);
            this.cmbBitrate.Name = "cmbBitrate";
            this.cmbBitrate.Size = new System.Drawing.Size(81, 20);
            this.cmbBitrate.TabIndex = 15;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkboxMkDir);
            this.groupBox2.Controls.Add(this.chkBoxAlert);
            this.groupBox2.Controls.Add(this.chkBoxTop100);
            this.groupBox2.Location = new System.Drawing.Point(10, 56);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 87);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "기타옵션";
            // 
            // chkboxMkDir
            // 
            this.chkboxMkDir.AutoSize = true;
            this.chkboxMkDir.Checked = true;
            this.chkboxMkDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxMkDir.Location = new System.Drawing.Point(6, 20);
            this.chkboxMkDir.Name = "chkboxMkDir";
            this.chkboxMkDir.Size = new System.Drawing.Size(220, 16);
            this.chkboxMkDir.TabIndex = 3;
            this.chkboxMkDir.Text = "Album 다운로드시 폴더 생성후 저장";
            this.chkboxMkDir.UseVisualStyleBackColor = true;
            // 
            // chkBoxAlert
            // 
            this.chkBoxAlert.AutoSize = true;
            this.chkBoxAlert.Checked = true;
            this.chkBoxAlert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxAlert.Location = new System.Drawing.Point(6, 64);
            this.chkBoxAlert.Name = "chkBoxAlert";
            this.chkBoxAlert.Size = new System.Drawing.Size(366, 16);
            this.chkBoxAlert.TabIndex = 4;
            this.chkBoxAlert.Text = "중복된 음악 추가시 알림창 보이지 않기 (옵션 체크시 자동무시)";
            this.chkBoxAlert.UseVisualStyleBackColor = true;
            // 
            // chkBoxTop100
            // 
            this.chkBoxTop100.AutoSize = true;
            this.chkBoxTop100.Checked = true;
            this.chkBoxTop100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxTop100.Location = new System.Drawing.Point(6, 42);
            this.chkBoxTop100.Name = "chkBoxTop100";
            this.chkBoxTop100.Size = new System.Drawing.Size(227, 16);
            this.chkBoxTop100.TabIndex = 5;
            this.chkBoxTop100.Text = "TOP100 다운로드시 폴더 생성후 저장";
            this.chkBoxTop100.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "음질선택";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(414, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 22);
            this.button1.TabIndex = 2;
            this.button1.Text = "변경";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "다운로드 위치 :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(305, 21);
            this.textBox1.TabIndex = 0;
            // 
            // tabSearchMusic
            // 
            this.tabSearchMusic.Controls.Add(this.lstViewSearch);
            this.tabSearchMusic.Controls.Add(this.btnTop100);
            this.tabSearchMusic.Controls.Add(this.btnSearch);
            this.tabSearchMusic.Controls.Add(this.txtBoxKeyword);
            this.tabSearchMusic.Controls.Add(this.cmbBoxSearchType);
            this.tabSearchMusic.Location = new System.Drawing.Point(4, 21);
            this.tabSearchMusic.Margin = new System.Windows.Forms.Padding(0);
            this.tabSearchMusic.Name = "tabSearchMusic";
            this.tabSearchMusic.Size = new System.Drawing.Size(546, 276);
            this.tabSearchMusic.TabIndex = 1;
            this.tabSearchMusic.Text = "음원검색";
            this.tabSearchMusic.UseVisualStyleBackColor = true;
            // 
            // btnTop100
            // 
            this.btnTop100.Location = new System.Drawing.Point(474, 4);
            this.btnTop100.Name = "btnTop100";
            this.btnTop100.Size = new System.Drawing.Size(69, 19);
            this.btnTop100.TabIndex = 5;
            this.btnTop100.Text = "TOP 100";
            this.btnTop100.UseVisualStyleBackColor = true;
            this.btnTop100.Click += new System.EventHandler(this.btnTop100_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(399, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 19);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtBoxKeyword
            // 
            this.txtBoxKeyword.Location = new System.Drawing.Point(118, 3);
            this.txtBoxKeyword.Name = "txtBoxKeyword";
            this.txtBoxKeyword.Size = new System.Drawing.Size(273, 21);
            this.txtBoxKeyword.TabIndex = 3;
            // 
            // cmbBoxSearchType
            // 
            this.cmbBoxSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxSearchType.FormattingEnabled = true;
            this.cmbBoxSearchType.Items.AddRange(new object[] {
            "음악검색",
            "앨범검색",
            "가수검색"});
            this.cmbBoxSearchType.Location = new System.Drawing.Point(8, 3);
            this.cmbBoxSearchType.Name = "cmbBoxSearchType";
            this.cmbBoxSearchType.Size = new System.Drawing.Size(104, 20);
            this.cmbBoxSearchType.TabIndex = 1;
            // 
            // tabDownload
            // 
            this.tabDownload.Location = new System.Drawing.Point(4, 21);
            this.tabDownload.Margin = new System.Windows.Forms.Padding(0);
            this.tabDownload.Name = "tabDownload";
            this.tabDownload.Size = new System.Drawing.Size(546, 276);
            this.tabDownload.TabIndex = 0;
            this.tabDownload.Text = "다운로드";
            this.tabDownload.UseVisualStyleBackColor = true;
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.tabDownload);
            this.mainTab.Controls.Add(this.tabSearchMusic);
            this.mainTab.Controls.Add(this.tabConfig);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Margin = new System.Windows.Forms.Padding(0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(554, 301);
            this.mainTab.TabIndex = 0;
            // 
            // ColmArtist
            // 
            this.ColmArtist.Text = "아티스트";
            this.ColmArtist.Width = 84;
            // 
            // ColmAlbum
            // 
            this.ColmAlbum.Text = "앨범";
            this.ColmAlbum.Width = 119;
            // 
            // ColmMusic
            // 
            this.ColmMusic.Text = "곡명";
            this.ColmMusic.Width = 233;
            // 
            // lstViewSearch
            // 
            this.lstViewSearch.BackColor = System.Drawing.Color.White;
            this.lstViewSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstViewSearch.FullRowSelect = true;
            this.lstViewSearch.Location = new System.Drawing.Point(8, 28);
            this.lstViewSearch.Name = "lstViewSearch";
            this.lstViewSearch.Size = new System.Drawing.Size(530, 248);
            this.lstViewSearch.TabIndex = 6;
            this.lstViewSearch.UseCompatibleStateImageBehavior = false;
            this.lstViewSearch.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "아티스트";
            this.columnHeader1.Width = 84;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "앨범";
            this.columnHeader2.Width = 119;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "곡명";
            this.columnHeader3.Width = 233;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 301);
            this.Controls.Add(this.mainTab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainForm";
            this.Text = "Bug\'s Bugs ver 2.0";
            this.tabConfig.ResumeLayout(false);
            this.tabConfig.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabSearchMusic.ResumeLayout(false);
            this.tabSearchMusic.PerformLayout();
            this.mainTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.ComboBox cmbBitrate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkboxMkDir;
        private System.Windows.Forms.CheckBox chkBoxAlert;
        private System.Windows.Forms.CheckBox chkBoxTop100;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage tabSearchMusic;
        private System.Windows.Forms.Button btnTop100;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtBoxKeyword;
        private System.Windows.Forms.ComboBox cmbBoxSearchType;
        private System.Windows.Forms.TabPage tabDownload;
        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.ColumnHeader ColmArtist;
        private System.Windows.Forms.ColumnHeader ColmAlbum;
        private System.Windows.Forms.ColumnHeader ColmMusic;
        private System.Windows.Forms.ListView lstViewSearch;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;

    }
}

