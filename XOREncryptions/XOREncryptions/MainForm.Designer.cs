
namespace Cheng
{
    partial class MainForm
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
            System.Windows.Forms.Label col_label1;
            System.Windows.Forms.Label col_label2;
            System.Windows.Forms.Label col_label3;
            this.col_textBox_inKey = new System.Windows.Forms.TextBox();
            this.col_button_importKey = new System.Windows.Forms.Button();
            this.col_button_saveKey = new System.Windows.Forms.Button();
            this.col_textBox_OriginalFile = new System.Windows.Forms.TextBox();
            this.col_textBox_OutputFile = new System.Windows.Forms.TextBox();
            this.col_button_checkOriginalFile = new System.Windows.Forms.Button();
            this.col_button_checkOutputFile = new System.Windows.Forms.Button();
            this.col_button_Start = new System.Windows.Forms.Button();
            this.col_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.col_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.col_saveFileDialogQukeEnc = new System.Windows.Forms.OpenFileDialog();
            this.col_button_qukeStart = new System.Windows.Forms.Button();
            col_label1 = new System.Windows.Forms.Label();
            col_label2 = new System.Windows.Forms.Label();
            col_label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // col_label1
            // 
            col_label1.AutoSize = true;
            col_label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            col_label1.Location = new System.Drawing.Point(12, 52);
            col_label1.Name = "col_label1";
            col_label1.Size = new System.Drawing.Size(82, 23);
            col_label1.TabIndex = 2;
            col_label1.Text = "加密密钥:";
            // 
            // col_label2
            // 
            col_label2.AutoSize = true;
            col_label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            col_label2.Location = new System.Drawing.Point(10, 146);
            col_label2.Name = "col_label2";
            col_label2.Size = new System.Drawing.Size(99, 23);
            col_label2.TabIndex = 6;
            col_label2.Text = "原文件路径:";
            // 
            // col_label3
            // 
            col_label3.AutoSize = true;
            col_label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            col_label3.Location = new System.Drawing.Point(27, 201);
            col_label3.Name = "col_label3";
            col_label3.Size = new System.Drawing.Size(82, 23);
            col_label3.TabIndex = 8;
            col_label3.Text = "输出路径:";
            // 
            // col_textBox_inKey
            // 
            this.col_textBox_inKey.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.col_textBox_inKey.Location = new System.Drawing.Point(103, 51);
            this.col_textBox_inKey.MaxLength = 16384;
            this.col_textBox_inKey.Name = "col_textBox_inKey";
            this.col_textBox_inKey.Size = new System.Drawing.Size(485, 29);
            this.col_textBox_inKey.TabIndex = 1;
            // 
            // col_button_importKey
            // 
            this.col_button_importKey.Location = new System.Drawing.Point(602, 48);
            this.col_button_importKey.Name = "col_button_importKey";
            this.col_button_importKey.Size = new System.Drawing.Size(100, 34);
            this.col_button_importKey.TabIndex = 3;
            this.col_button_importKey.Text = "导入密钥";
            this.col_button_importKey.UseVisualStyleBackColor = true;
            // 
            // col_button_saveKey
            // 
            this.col_button_saveKey.Location = new System.Drawing.Point(718, 48);
            this.col_button_saveKey.Name = "col_button_saveKey";
            this.col_button_saveKey.Size = new System.Drawing.Size(100, 34);
            this.col_button_saveKey.TabIndex = 4;
            this.col_button_saveKey.Text = "保存密钥";
            this.col_button_saveKey.UseVisualStyleBackColor = true;
            // 
            // col_textBox_OriginalFile
            // 
            this.col_textBox_OriginalFile.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.col_textBox_OriginalFile.Location = new System.Drawing.Point(115, 144);
            this.col_textBox_OriginalFile.Name = "col_textBox_OriginalFile";
            this.col_textBox_OriginalFile.Size = new System.Drawing.Size(545, 29);
            this.col_textBox_OriginalFile.TabIndex = 5;
            // 
            // col_textBox_OutputFile
            // 
            this.col_textBox_OutputFile.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.col_textBox_OutputFile.Location = new System.Drawing.Point(116, 200);
            this.col_textBox_OutputFile.Name = "col_textBox_OutputFile";
            this.col_textBox_OutputFile.Size = new System.Drawing.Size(545, 29);
            this.col_textBox_OutputFile.TabIndex = 7;
            // 
            // col_button_checkOriginalFile
            // 
            this.col_button_checkOriginalFile.Location = new System.Drawing.Point(685, 142);
            this.col_button_checkOriginalFile.Name = "col_button_checkOriginalFile";
            this.col_button_checkOriginalFile.Size = new System.Drawing.Size(124, 34);
            this.col_button_checkOriginalFile.TabIndex = 9;
            this.col_button_checkOriginalFile.Text = "选择文件";
            this.col_button_checkOriginalFile.UseVisualStyleBackColor = true;
            // 
            // col_button_checkOutputFile
            // 
            this.col_button_checkOutputFile.Location = new System.Drawing.Point(685, 196);
            this.col_button_checkOutputFile.Name = "col_button_checkOutputFile";
            this.col_button_checkOutputFile.Size = new System.Drawing.Size(124, 34);
            this.col_button_checkOutputFile.TabIndex = 10;
            this.col_button_checkOutputFile.Text = "选择输出路径";
            this.col_button_checkOutputFile.UseVisualStyleBackColor = true;
            // 
            // col_button_Start
            // 
            this.col_button_Start.BackColor = System.Drawing.SystemColors.ControlLight;
            this.col_button_Start.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.col_button_Start.Location = new System.Drawing.Point(234, 272);
            this.col_button_Start.Name = "col_button_Start";
            this.col_button_Start.Size = new System.Drawing.Size(117, 58);
            this.col_button_Start.TabIndex = 11;
            this.col_button_Start.Text = "开始";
            this.col_button_Start.UseVisualStyleBackColor = false;
            // 
            // col_openFileDialog
            // 
            this.col_openFileDialog.Filter = "所有文件|*.*";
            this.col_openFileDialog.Title = "选择要加密或解密的文件";
            // 
            // col_saveFileDialog
            // 
            this.col_saveFileDialog.AddExtension = false;
            this.col_saveFileDialog.Filter = "所有文件|*.*";
            this.col_saveFileDialog.Title = "选择输出的文件路径";
            // 
            // col_saveFileDialogQukeEnc
            // 
            this.col_saveFileDialogQukeEnc.Filter = "所有文件|*.*";
            this.col_saveFileDialogQukeEnc.Title = "选择一个文件开始加密";
            // 
            // col_button_qukeStart
            // 
            this.col_button_qukeStart.BackColor = System.Drawing.SystemColors.ControlLight;
            this.col_button_qukeStart.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.col_button_qukeStart.Location = new System.Drawing.Point(443, 272);
            this.col_button_qukeStart.Name = "col_button_qukeStart";
            this.col_button_qukeStart.Size = new System.Drawing.Size(117, 58);
            this.col_button_qukeStart.TabIndex = 12;
            this.col_button_qukeStart.Text = "快速开始";
            this.col_button_qukeStart.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(842, 414);
            this.Controls.Add(this.col_button_qukeStart);
            this.Controls.Add(this.col_button_Start);
            this.Controls.Add(this.col_button_checkOutputFile);
            this.Controls.Add(this.col_button_checkOriginalFile);
            this.Controls.Add(this.col_textBox_OutputFile);
            this.Controls.Add(col_label3);
            this.Controls.Add(this.col_textBox_OriginalFile);
            this.Controls.Add(col_label2);
            this.Controls.Add(this.col_textBox_inKey);
            this.Controls.Add(col_label1);
            this.Controls.Add(this.col_button_saveKey);
            this.Controls.Add(this.col_button_importKey);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "轻量级文件加密";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox col_textBox_inKey;
        private System.Windows.Forms.Button col_button_importKey;
        private System.Windows.Forms.Button col_button_saveKey;
        private System.Windows.Forms.TextBox col_textBox_OriginalFile;
        private System.Windows.Forms.TextBox col_textBox_OutputFile;
        private System.Windows.Forms.Button col_button_checkOriginalFile;
        private System.Windows.Forms.Button col_button_checkOutputFile;
        private System.Windows.Forms.Button col_button_Start;
        private System.Windows.Forms.OpenFileDialog col_openFileDialog;
        private System.Windows.Forms.SaveFileDialog col_saveFileDialog;
        private System.Windows.Forms.OpenFileDialog col_saveFileDialogQukeEnc;
        private System.Windows.Forms.Button col_button_qukeStart;
    }
}

