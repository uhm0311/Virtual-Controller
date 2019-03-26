namespace VirtualController.GUI
{
    partial class VirtualControllerForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuKeyConfigK = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPlayer1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewP1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenP1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditP1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteP1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPlayer2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewP2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenP2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditP2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteP2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPlayer3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewP3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenP3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditP3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteP3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPlayer4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewP4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenP4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditP4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteP4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpH = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpV = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInfoR = new System.Windows.Forms.ToolStripMenuItem();
            this.gpBoxWiFi = new System.Windows.Forms.GroupBox();
            this.btnUSB = new System.Windows.Forms.Button();
            this.btnWiFi = new System.Windows.Forms.Button();
            this.lblMaxPlayers = new System.Windows.Forms.Label();
            this.maxPlayers = new System.Windows.Forms.DomainUpDown();
            this.gpBoxKeyConfigs = new System.Windows.Forms.GroupBox();
            this.chckBoxP4 = new System.Windows.Forms.CheckBox();
            this.chckBoxP3 = new System.Windows.Forms.CheckBox();
            this.chckBoxP2 = new System.Windows.Forms.CheckBox();
            this.chckBoxP1 = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.gpBoxWiFi.SuspendLayout();
            this.gpBoxKeyConfigs.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuKeyConfigK,
            this.menuHelpH});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(457, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuKeyConfigK
            // 
            this.menuKeyConfigK.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPlayer1,
            this.menuPlayer2,
            this.menuPlayer3,
            this.menuPlayer4});
            this.menuKeyConfigK.Name = "menuKeyConfigK";
            this.menuKeyConfigK.Size = new System.Drawing.Size(74, 20);
            this.menuKeyConfigK.Text = "키 설정(&K)";
            this.menuKeyConfigK.Click += new System.EventHandler(this.menuKeyConfigK_Click);
            // 
            // menuPlayer1
            // 
            this.menuPlayer1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewP1,
            this.menuOpenP1,
            this.menuEditP1,
            this.menuDeleteP1});
            this.menuPlayer1.Name = "menuPlayer1";
            this.menuPlayer1.Size = new System.Drawing.Size(113, 22);
            this.menuPlayer1.Text = "Player&1";
            // 
            // menuNewP1
            // 
            this.menuNewP1.Name = "menuNewP1";
            this.menuNewP1.Size = new System.Drawing.Size(155, 22);
            this.menuNewP1.Text = "새로 만들기(&N)";
            this.menuNewP1.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpenP1
            // 
            this.menuOpenP1.Name = "menuOpenP1";
            this.menuOpenP1.Size = new System.Drawing.Size(155, 22);
            this.menuOpenP1.Text = "불러오기(&O)";
            // 
            // menuEditP1
            // 
            this.menuEditP1.Name = "menuEditP1";
            this.menuEditP1.Size = new System.Drawing.Size(155, 22);
            this.menuEditP1.Text = "편집하기(&E)";
            // 
            // menuDeleteP1
            // 
            this.menuDeleteP1.Name = "menuDeleteP1";
            this.menuDeleteP1.Size = new System.Drawing.Size(155, 22);
            this.menuDeleteP1.Text = "삭제하기(&D)";
            // 
            // menuPlayer2
            // 
            this.menuPlayer2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewP2,
            this.menuOpenP2,
            this.menuEditP2,
            this.menuDeleteP2});
            this.menuPlayer2.Name = "menuPlayer2";
            this.menuPlayer2.Size = new System.Drawing.Size(113, 22);
            this.menuPlayer2.Text = "Player&2";
            // 
            // menuNewP2
            // 
            this.menuNewP2.Name = "menuNewP2";
            this.menuNewP2.Size = new System.Drawing.Size(155, 22);
            this.menuNewP2.Text = "새로 만들기(&N)";
            this.menuNewP2.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpenP2
            // 
            this.menuOpenP2.Name = "menuOpenP2";
            this.menuOpenP2.Size = new System.Drawing.Size(155, 22);
            this.menuOpenP2.Text = "불러오기(&O)";
            // 
            // menuEditP2
            // 
            this.menuEditP2.Name = "menuEditP2";
            this.menuEditP2.Size = new System.Drawing.Size(155, 22);
            this.menuEditP2.Text = "편집하기(&E)";
            // 
            // menuDeleteP2
            // 
            this.menuDeleteP2.Name = "menuDeleteP2";
            this.menuDeleteP2.Size = new System.Drawing.Size(155, 22);
            this.menuDeleteP2.Text = "삭제하기(&D)";
            // 
            // menuPlayer3
            // 
            this.menuPlayer3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewP3,
            this.menuOpenP3,
            this.menuEditP3,
            this.menuDeleteP3});
            this.menuPlayer3.Name = "menuPlayer3";
            this.menuPlayer3.Size = new System.Drawing.Size(113, 22);
            this.menuPlayer3.Text = "Player&3";
            // 
            // menuNewP3
            // 
            this.menuNewP3.Name = "menuNewP3";
            this.menuNewP3.Size = new System.Drawing.Size(155, 22);
            this.menuNewP3.Text = "새로 만들기(&N)";
            this.menuNewP3.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpenP3
            // 
            this.menuOpenP3.Name = "menuOpenP3";
            this.menuOpenP3.Size = new System.Drawing.Size(155, 22);
            this.menuOpenP3.Text = "불러오기(&O)";
            // 
            // menuEditP3
            // 
            this.menuEditP3.Name = "menuEditP3";
            this.menuEditP3.Size = new System.Drawing.Size(155, 22);
            this.menuEditP3.Text = "편집하기(&E)";
            // 
            // menuDeleteP3
            // 
            this.menuDeleteP3.Name = "menuDeleteP3";
            this.menuDeleteP3.Size = new System.Drawing.Size(155, 22);
            this.menuDeleteP3.Text = "삭제하기(&D)";
            // 
            // menuPlayer4
            // 
            this.menuPlayer4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewP4,
            this.menuOpenP4,
            this.menuEditP4,
            this.menuDeleteP4});
            this.menuPlayer4.Name = "menuPlayer4";
            this.menuPlayer4.Size = new System.Drawing.Size(113, 22);
            this.menuPlayer4.Text = "Player&4";
            // 
            // menuNewP4
            // 
            this.menuNewP4.Name = "menuNewP4";
            this.menuNewP4.Size = new System.Drawing.Size(155, 22);
            this.menuNewP4.Text = "새로 만들기(&N)";
            this.menuNewP4.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpenP4
            // 
            this.menuOpenP4.Name = "menuOpenP4";
            this.menuOpenP4.Size = new System.Drawing.Size(155, 22);
            this.menuOpenP4.Text = "불러오기(&O)";
            // 
            // menuEditP4
            // 
            this.menuEditP4.Name = "menuEditP4";
            this.menuEditP4.Size = new System.Drawing.Size(155, 22);
            this.menuEditP4.Text = "편집하기(&E)";
            // 
            // menuDeleteP4
            // 
            this.menuDeleteP4.Name = "menuDeleteP4";
            this.menuDeleteP4.Size = new System.Drawing.Size(155, 22);
            this.menuDeleteP4.Text = "삭제하기(&D)";
            // 
            // menuHelpH
            // 
            this.menuHelpH.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpV,
            this.menuInfoR});
            this.menuHelpH.Name = "menuHelpH";
            this.menuHelpH.Size = new System.Drawing.Size(72, 20);
            this.menuHelpH.Text = "도움말(&H)";
            // 
            // menuHelpV
            // 
            this.menuHelpV.Name = "menuHelpV";
            this.menuHelpV.Size = new System.Drawing.Size(154, 22);
            this.menuHelpV.Text = "도움말 보기(&V)";
            // 
            // menuInfoR
            // 
            this.menuInfoR.Name = "menuInfoR";
            this.menuInfoR.Size = new System.Drawing.Size(154, 22);
            this.menuInfoR.Text = "정보(&R)";
            // 
            // gpBoxWiFi
            // 
            this.gpBoxWiFi.Controls.Add(this.btnUSB);
            this.gpBoxWiFi.Controls.Add(this.btnWiFi);
            this.gpBoxWiFi.Controls.Add(this.lblMaxPlayers);
            this.gpBoxWiFi.Controls.Add(this.maxPlayers);
            this.gpBoxWiFi.Location = new System.Drawing.Point(12, 27);
            this.gpBoxWiFi.Name = "gpBoxWiFi";
            this.gpBoxWiFi.Size = new System.Drawing.Size(199, 135);
            this.gpBoxWiFi.TabIndex = 2;
            this.gpBoxWiFi.TabStop = false;
            // 
            // btnUSB
            // 
            this.btnUSB.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUSB.Location = new System.Drawing.Point(16, 86);
            this.btnUSB.Name = "btnUSB";
            this.btnUSB.Size = new System.Drawing.Size(169, 35);
            this.btnUSB.TabIndex = 3;
            this.btnUSB.Text = "USB 연결 시작";
            this.btnUSB.UseVisualStyleBackColor = true;
            this.btnUSB.Click += new System.EventHandler(this.btnUSB_Click);
            // 
            // btnWiFi
            // 
            this.btnWiFi.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWiFi.Location = new System.Drawing.Point(16, 48);
            this.btnWiFi.Name = "btnWiFi";
            this.btnWiFi.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnWiFi.Size = new System.Drawing.Size(169, 35);
            this.btnWiFi.TabIndex = 3;
            this.btnWiFi.Text = "WiFi 연결 시작";
            this.btnWiFi.UseVisualStyleBackColor = true;
            this.btnWiFi.Click += new System.EventHandler(this.btnWiFi_Click);
            // 
            // lblMaxPlayers
            // 
            this.lblMaxPlayers.AutoSize = true;
            this.lblMaxPlayers.BackColor = System.Drawing.SystemColors.Window;
            this.lblMaxPlayers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMaxPlayers.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMaxPlayers.Location = new System.Drawing.Point(16, 22);
            this.lblMaxPlayers.Name = "lblMaxPlayers";
            this.lblMaxPlayers.Size = new System.Drawing.Size(131, 18);
            this.lblMaxPlayers.TabIndex = 2;
            this.lblMaxPlayers.Text = "최대 연결 인원 : ";
            // 
            // maxPlayers
            // 
            this.maxPlayers.Items.Add("4");
            this.maxPlayers.Items.Add("3");
            this.maxPlayers.Items.Add("2");
            this.maxPlayers.Items.Add("1");
            this.maxPlayers.Location = new System.Drawing.Point(153, 21);
            this.maxPlayers.Name = "maxPlayers";
            this.maxPlayers.Size = new System.Drawing.Size(32, 21);
            this.maxPlayers.TabIndex = 1;
            // 
            // gpBoxKeyConfigs
            // 
            this.gpBoxKeyConfigs.Controls.Add(this.chckBoxP4);
            this.gpBoxKeyConfigs.Controls.Add(this.chckBoxP3);
            this.gpBoxKeyConfigs.Controls.Add(this.chckBoxP2);
            this.gpBoxKeyConfigs.Controls.Add(this.chckBoxP1);
            this.gpBoxKeyConfigs.Location = new System.Drawing.Point(217, 27);
            this.gpBoxKeyConfigs.Name = "gpBoxKeyConfigs";
            this.gpBoxKeyConfigs.Size = new System.Drawing.Size(228, 135);
            this.gpBoxKeyConfigs.TabIndex = 3;
            this.gpBoxKeyConfigs.TabStop = false;
            // 
            // chckBoxP4
            // 
            this.chckBoxP4.AutoSize = true;
            this.chckBoxP4.Enabled = false;
            this.chckBoxP4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chckBoxP4.Location = new System.Drawing.Point(6, 102);
            this.chckBoxP4.Name = "chckBoxP4";
            this.chckBoxP4.Size = new System.Drawing.Size(188, 20);
            this.chckBoxP4.TabIndex = 3;
            this.chckBoxP4.Text = "Player4 - 키 설정 없음";
            this.chckBoxP4.UseVisualStyleBackColor = true;
            // 
            // chckBoxP3
            // 
            this.chckBoxP3.AutoSize = true;
            this.chckBoxP3.Enabled = false;
            this.chckBoxP3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chckBoxP3.Location = new System.Drawing.Point(6, 73);
            this.chckBoxP3.Name = "chckBoxP3";
            this.chckBoxP3.Size = new System.Drawing.Size(187, 20);
            this.chckBoxP3.TabIndex = 2;
            this.chckBoxP3.Text = "Player3 - 키 설정 없음";
            this.chckBoxP3.UseVisualStyleBackColor = true;
            // 
            // chckBoxP2
            // 
            this.chckBoxP2.AutoSize = true;
            this.chckBoxP2.Enabled = false;
            this.chckBoxP2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chckBoxP2.Location = new System.Drawing.Point(6, 47);
            this.chckBoxP2.Name = "chckBoxP2";
            this.chckBoxP2.Size = new System.Drawing.Size(187, 20);
            this.chckBoxP2.TabIndex = 1;
            this.chckBoxP2.Text = "Player2 - 키 설정 없음";
            this.chckBoxP2.UseVisualStyleBackColor = true;
            // 
            // chckBoxP1
            // 
            this.chckBoxP1.AutoSize = true;
            this.chckBoxP1.Enabled = false;
            this.chckBoxP1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chckBoxP1.Location = new System.Drawing.Point(6, 20);
            this.chckBoxP1.Name = "chckBoxP1";
            this.chckBoxP1.Size = new System.Drawing.Size(187, 20);
            this.chckBoxP1.TabIndex = 0;
            this.chckBoxP1.Text = "Player1 - 키 설정 없음";
            this.chckBoxP1.UseVisualStyleBackColor = true;
            // 
            // VirtualControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 174);
            this.Controls.Add(this.gpBoxKeyConfigs);
            this.Controls.Add(this.gpBoxWiFi);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VirtualControllerForm";
            this.Text = "Virtual Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VirtualControllerForm_FormClosing);
            this.Load += new System.EventHandler(this.VirtualControllerForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gpBoxWiFi.ResumeLayout(false);
            this.gpBoxWiFi.PerformLayout();
            this.gpBoxKeyConfigs.ResumeLayout(false);
            this.gpBoxKeyConfigs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuHelpH;
        private System.Windows.Forms.ToolStripMenuItem menuHelpV;
        private System.Windows.Forms.ToolStripMenuItem menuInfoR;
        private System.Windows.Forms.GroupBox gpBoxWiFi;
        private System.Windows.Forms.DomainUpDown maxPlayers;
        private System.Windows.Forms.Label lblMaxPlayers;
        private System.Windows.Forms.Button btnWiFi;
        private System.Windows.Forms.ToolStripMenuItem menuKeyConfigK;
        private System.Windows.Forms.ToolStripMenuItem menuPlayer1;
        private System.Windows.Forms.ToolStripMenuItem menuNewP1;
        private System.Windows.Forms.ToolStripMenuItem menuOpenP1;
        private System.Windows.Forms.ToolStripMenuItem menuEditP1;
        private System.Windows.Forms.ToolStripMenuItem menuPlayer2;
        private System.Windows.Forms.ToolStripMenuItem menuPlayer3;
        private System.Windows.Forms.ToolStripMenuItem menuPlayer4;
        private System.Windows.Forms.ToolStripMenuItem menuNewP2;
        private System.Windows.Forms.ToolStripMenuItem menuNewP3;
        private System.Windows.Forms.ToolStripMenuItem menuNewP4;
        private System.Windows.Forms.ToolStripMenuItem menuOpenP2;
        private System.Windows.Forms.ToolStripMenuItem menuOpenP3;
        private System.Windows.Forms.ToolStripMenuItem menuOpenP4;
        private System.Windows.Forms.ToolStripMenuItem menuEditP2;
        private System.Windows.Forms.ToolStripMenuItem menuEditP3;
        private System.Windows.Forms.ToolStripMenuItem menuEditP4;
        private System.Windows.Forms.GroupBox gpBoxKeyConfigs;
        private System.Windows.Forms.CheckBox chckBoxP4;
        private System.Windows.Forms.CheckBox chckBoxP3;
        private System.Windows.Forms.CheckBox chckBoxP2;
        private System.Windows.Forms.CheckBox chckBoxP1;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteP1;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteP2;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteP3;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteP4;
        private System.Windows.Forms.Button btnUSB;
    }
}

