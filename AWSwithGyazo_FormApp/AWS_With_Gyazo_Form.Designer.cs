namespace AWSwithGyazo_FormApp
{
    partial class AWS_With_Gyazo_Form
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AWS_With_Gyazo_Form));
            this.notify_AWS = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.機能の再開停止ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.サーバーとデータを同期するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ローカルからS3へToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s3からローカルへToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mStrip_AutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.mStrip_EnableBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.mStrip_DisableBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.プログラムの終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notify_AWS
            // 
            this.notify_AWS.ContextMenuStrip = this.contextMenuStrip1;
            this.notify_AWS.Icon = ((System.Drawing.Icon)(resources.GetObject("notify_AWS.Icon")));
            this.notify_AWS.Text = "AWS_With_Gyazo.exe";
            this.notify_AWS.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.機能の再開停止ToolStripMenuItem,
            this.サーバーとデータを同期するToolStripMenuItem,
            this.mStrip_AutoBackup,
            this.プログラムの終了ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 114);
            // 
            // 機能の再開停止ToolStripMenuItem
            // 
            this.機能の再開停止ToolStripMenuItem.Name = "機能の再開停止ToolStripMenuItem";
            this.機能の再開停止ToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.機能の再開停止ToolStripMenuItem.Text = "監視の再開/停止";
            this.機能の再開停止ToolStripMenuItem.Click += new System.EventHandler(this.StartWatch_toolstripClick);
            // 
            // サーバーとデータを同期するToolStripMenuItem
            // 
            this.サーバーとデータを同期するToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ローカルからS3へToolStripMenuItem,
            this.s3からローカルへToolStripMenuItem});
            this.サーバーとデータを同期するToolStripMenuItem.Name = "サーバーとデータを同期するToolStripMenuItem";
            this.サーバーとデータを同期するToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.サーバーとデータを同期するToolStripMenuItem.Text = "サーバーとデータを同期する";
            this.サーバーとデータを同期するToolStripMenuItem.Click += new System.EventHandler(this.サーバーとデータを同期するToolStripMenuItem_Click);
            // 
            // ローカルからS3へToolStripMenuItem
            // 
            this.ローカルからS3へToolStripMenuItem.Name = "ローカルからS3へToolStripMenuItem";
            this.ローカルからS3へToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.ローカルからS3へToolStripMenuItem.Text = "ローカルからS3へ";
            this.ローカルからS3へToolStripMenuItem.Click += new System.EventHandler(this.ローカルからS3へToolStripMenuItem_Click);
            // 
            // s3からローカルへToolStripMenuItem
            // 
            this.s3からローカルへToolStripMenuItem.Name = "s3からローカルへToolStripMenuItem";
            this.s3からローカルへToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.s3からローカルへToolStripMenuItem.Text = "S3からローカルへ";
            this.s3からローカルへToolStripMenuItem.Click += new System.EventHandler(this.S3からローカルへToolStripMenuItem_Click);
            // 
            // mStrip_AutoBackup
            // 
            this.mStrip_AutoBackup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mStrip_EnableBackup,
            this.mStrip_DisableBackup});
            this.mStrip_AutoBackup.Name = "mStrip_AutoBackup";
            this.mStrip_AutoBackup.Size = new System.Drawing.Size(197, 22);
            this.mStrip_AutoBackup.Text = "自動バックアップ";
            // 
            // mStrip_EnableBackup
            // 
            this.mStrip_EnableBackup.Name = "mStrip_EnableBackup";
            this.mStrip_EnableBackup.Size = new System.Drawing.Size(180, 22);
            this.mStrip_EnableBackup.Text = "行う";
            this.mStrip_EnableBackup.Click += new System.EventHandler(this.mStrip_EnableBackup_Click);
            // 
            // mStrip_DisableBackup
            // 
            this.mStrip_DisableBackup.Name = "mStrip_DisableBackup";
            this.mStrip_DisableBackup.Size = new System.Drawing.Size(180, 22);
            this.mStrip_DisableBackup.Text = "行わない";
            this.mStrip_DisableBackup.Click += new System.EventHandler(this.mStrip_DisableBackup_Click);
            // 
            // プログラムの終了ToolStripMenuItem
            // 
            this.プログラムの終了ToolStripMenuItem.Name = "プログラムの終了ToolStripMenuItem";
            this.プログラムの終了ToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.プログラムの終了ToolStripMenuItem.Text = "プログラムの終了";
            this.プログラムの終了ToolStripMenuItem.Click += new System.EventHandler(this.ExitProgram_toolstripClick);
            // 
            // AWS_With_Gyazo_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AWS_With_Gyazo_Form";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notify_AWS;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 機能の再開停止ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem プログラムの終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem サーバーとデータを同期するToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ローカルからS3へToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem s3からローカルへToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mStrip_AutoBackup;
        private System.Windows.Forms.ToolStripMenuItem mStrip_EnableBackup;
        private System.Windows.Forms.ToolStripMenuItem mStrip_DisableBackup;
    }
}

