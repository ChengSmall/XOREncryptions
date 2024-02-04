using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Cheng.Algorithm.Encryptions;
using Cheng.Memorys;
using Cheng.Streams.Parsers.Default;

//using System.Runtime.InteropServices;
//using System.Threading;
//using System.Threading.Tasks;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;

namespace Cheng
{
    public unsafe partial class MainForm : Form
    {
        public MainForm()
        {
            f_lateInit();
            InitializeComponent();
            f_init();
        }

        #region 初始化

        private void f_lateInit()
        {
            sp_mainForm = this;
            p_parser = new StreamParserDefault();
            f_initKeySaveHand();
            Buffer = new byte[131072];
            p_byteListBuffer = new List<byte>(4);
            p_keyEncoding = Encoding.UTF8;
        }

        private void f_init()
        {
            #region 初始化字段
            
            f_load();
            XOREncry = new XOREncryption(new byte[1]);
            XorStream = new XORStreamEncry(Stream.Null, XOREncry);           
            #endregion

            #region 初始化事件
            //TestButton.Click += fe_ButtonClick_Test;

            ButtonSaveFile.Click += fe_ButtonClick_SaveFile;

            ButtonSclectFile.Click += fe_ButtonClick_SclectFile;

            ButtonSaveKey.Click += fe_ButtonClick_SaveKey;

            ButtonReadKey.Click += fe_ButtonClick_ReadKey;

            ButtonStart.Click += fe_ButtonClick_Start;

            ButtonQukeStart.Click += fe_ButtonClick_QukeStart;

            //ButtonSaveExit.Click += fe_ButtonClick_SaveAndExit;

            #endregion
        }

        private void f_initKeySaveHand()
        {
            p_saveHand = new byte[]
            {
                0xA0, 0xEC, 0x98, 0x3C
            };
        }

        private const string saveFileName = "saveFile.bin";

        const string saveKeyKey = "key";
        const string saveOriginFile = "originalFile";
        const string saveToFile = "toFile";
        private void f_loadStream(Stream stream)
        {
            StreamParserDefault par = p_parser;
            try
            {
                object obj = par.ConverToObject(stream);
                bool flag;
                var dict = (Dictionary<string, object>)obj;

                flag = dict.TryGetValue(saveKeyKey, out obj);
                if (flag && obj is string)
                {
                    KeyText = (string)obj;
                }

                flag = dict.TryGetValue(saveOriginFile, out obj);
                if (flag && obj is string)
                {
                    OriginFilePath = (string)obj;
                }

                flag = dict.TryGetValue(saveToFile, out obj);
                if (flag && obj is string)
                {
                    TargetFilePath = (string)obj;
                }

            }
            catch (Exception)
            {

            }
            

        }

        private FileStream OpenSaveFile()
        {
            string path = Environment.CurrentDirectory;
            path = Path.Combine(path, saveFileName);
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 2048);
        }

        private FileStream CreateSaveFile()
        {
            string path = Environment.CurrentDirectory;
            path = Path.Combine(path, saveFileName);
            return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 2048);
        }

        private void f_load()
        {
            byte[] buffer;
            try
            {
                string path = Environment.CurrentDirectory;
                path = Path.Combine(path, saveFileName);

                using (FileStream loadFile = OpenSaveFile())
                {
                    //读取判断头
                    buffer = new byte[SaveHand.Length];
                    int r = loadFile.ReadBlock(buffer, 0, buffer.Length);

                    if(r != buffer.Length) return;
                    if (!buffer.EqualsBytes(SaveHand)) return;

                    f_loadStream(loadFile);
                }

                return;
            }
            catch (Exception)
            {

            }
           
         
        }

        private void f_save()
        {
            
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>(3);
                dict.Add(saveKeyKey, KeyText);
                dict.Add(saveOriginFile, OriginFilePath);
                dict.Add(saveToFile, TargetFilePath);

                using (FileStream savefile = CreateSaveFile())
                {
                    //存头
                    savefile.Write(SaveHand, 0, SaveHand.Length);
                    //存数据
                    p_parser.ConverToStream(dict, savefile);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region 前台控件获取
        /// <summary>
        /// 测试按钮
        /// </summary>
        //private Button TestButton => null;

        #region 文本框
        /// <summary>
        /// 文本框-密钥
        /// </summary>
        private TextBox KeyTextBox => col_textBox_inKey;
        /// <summary>
        /// 文本框-原文件路径
        /// </summary>
        private TextBox TextBoxOriginalFile => col_textBox_OriginalFile;
        /// <summary>
        /// 文本框-输出文件路径
        /// </summary>
        private TextBox TextBoxOutPutFile => col_textBox_OutputFile;
        #endregion
        #region 按钮
        /// <summary>
        /// 按钮-保存密钥
        /// </summary>
        private Button ButtonSaveKey => col_button_saveKey;
        /// <summary>
        /// 按钮-导入密钥
        /// </summary>
        private Button ButtonReadKey => col_button_importKey;
        /// <summary>
        /// 按钮-选择原文件路径
        /// </summary>
        private Button ButtonSclectFile => col_button_checkOriginalFile;
        /// <summary>
        /// 按钮-选择要保存到的目标文件
        /// </summary>
        private Button ButtonSaveFile => col_button_checkOutputFile;
        /// <summary>
        /// 按钮-开始加密
        /// </summary>
        private Button ButtonStart => col_button_Start;
        /// <summary>
        /// -按钮快速加密
        /// </summary>
        private Button ButtonQukeStart => col_button_qukeStart;

        /// <summary>
        /// 保存并退出按钮
        /// </summary>
        //private Button ButtonSaveExit => null;
        #endregion
        #region 对话框
        /// <summary>
        /// 对话框-选择文件目录
        /// </summary>
        private FolderBrowserDialog FloderDialog => /*col_folderBrowserDialog*/ null;
        /// <summary>
        /// 对话框-用于打开文件
        /// </summary>
        private OpenFileDialog ReadFileDialog => col_openFileDialog;
        /// <summary>
        /// 对话框-保存文件
        /// </summary>
        private SaveFileDialog SavFileDialog => col_saveFileDialog;
        /// <summary>
        /// 快速加密且覆盖文件
        /// </summary>
        private OpenFileDialog QukeEncFileDialog => col_saveFileDialogQukeEnc;
        #endregion
        #endregion

        #region 后台参数

        #region 静态字段
        private static MainForm sp_mainForm;
        #endregion

        #region 变量

        /// <summary>
        /// 异或加密
        /// </summary>
        private XOREncryption p_xorEncry;
        /// <summary>
        /// 加密流
        /// </summary>
        private XORStreamEncry p_xorStream;
        private byte[] p_buffer;

        private StreamParserDefault p_parser;

        private byte[] p_saveHand;
        #endregion

        #endregion

        #region 参数获取
        /// <summary>
        /// 获取主窗口实例
        /// </summary>
        public static MainForm MainWindow => sp_mainForm;
        /// <summary>
        /// 异或加密
        /// </summary>
        private XOREncryption XOREncry
        {
            get
            {
                return p_xorEncry;
            }
            set
            {
                p_xorEncry = value;
            }
        }
        /// <summary>
        /// 加密流
        /// </summary>
        private XORStreamEncry XorStream
        {
            get
            {
                return p_xorStream;
            }
            set => p_xorStream = value;
        }
        /// <summary>
        /// 流资源拷贝缓冲区
        /// </summary>
        private byte[] Buffer
        {
            get => p_buffer;
            set
            {
                p_buffer = value;
            }
        }
        /// <summary>
        /// 保存文件头
        /// </summary>
        private byte[] SaveHand
        {
            get
            {
                return p_saveHand;
            }
            set
            {
                p_saveHand = value;
            }
        }
        /// <summary>
        /// 密钥字符串转换方式
        /// </summary>
        private Encoding p_keyEncoding;

        private List<byte> p_byteListBuffer;
        #endregion

        #region 事件函数

        #region 按钮事件

        /// <summary>
        /// 按钮点击事件-保存密钥
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fe_ButtonClick_SaveKey(object sender, EventArgs e)
        {
            if(SaveFile(out string str))
            {
                f_saveKey(str);
            }
        }

        /// <summary>
        /// 按钮点击事件-读取密钥
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fe_ButtonClick_ReadKey(object sender, EventArgs e)
        {
            if(ReadFile(out string str))
            {
                f_readKey(str);
            }
        }

        /// <summary>
        /// 按钮点击事件-选择原文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fe_ButtonClick_SclectFile(object sender, EventArgs e)
        {
            string str = OriginFilePath;
            bool b1, b2;
            b1 = File.Exists(str);
            b2 = Directory.Exists(str);
            var read = ReadFileDialog;
            try
            {
                
                if (b1 || b2)
                {
                    
                    if (b1)
                        read.InitialDirectory = Path.GetFullPath(Path.GetDirectoryName(str));
                    else
                        read.InitialDirectory = Path.GetFullPath(str);

                    goto OverInitDire;
                }

                string ps = Path.GetDirectoryName(str);
                if (ps is null || ps.Length == 0) goto OverInitDire;

                read.InitialDirectory = Path.GetFullPath(Path.GetDirectoryName(ps));

            }
            catch (Exception ex)
            {
#if DEBUG
                ("错误:" + ex.Message).printl();
#endif
            }
            OverInitDire:
            string s2;

            if(ReadFile(out s2))
            {
                if(s2 != str) OriginFilePath = s2;
            }
        }
        /// <summary>
        /// 按钮点击事件-选择加密到的目标路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fe_ButtonClick_SaveFile(object sender, EventArgs e)
        {
            string str = TargetFilePath;
            bool b1, b2;
            b1 = File.Exists(str);
            b2 = Directory.Exists(str);
            var read = SavFileDialog;

            try
            {
                if (b1 || b2)
                {

                    if (b1)
                        read.InitialDirectory = Path.GetFullPath(Path.GetDirectoryName(str));
                    else
                        read.InitialDirectory = Path.GetFullPath(str);

                    goto OverInitDire;
                }

                string ps = Path.GetDirectoryName(str);
                if (ps is null || ps.Length == 0) goto OverInitDire;

                read.InitialDirectory = ps;

            }
            catch (Exception ex)
            {
#if DEBUG
                ("错误:" + ex.Message).printl();
#endif
            }
            OverInitDire:
            string s2;
            if(SaveFile(out s2))
            {
                if(str != s2) TargetFilePath = s2;
            }
        }

        /// <summary>
        /// 按钮点击事件-开始运作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fe_ButtonClick_Start(object sender, EventArgs e)
        {
            StartEnc();
        }
        /// <summary>
        /// 按钮点击事件-快速运作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fe_ButtonClick_QukeStart(object sender, EventArgs e)
        {
            QukeStart();
        }
        /// <summary>
        /// 按钮点击事件-保存并退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fe_ButtonClick_SaveAndExit(object sender, EventArgs e)
        {
            f_save();
            Application.Exit();
        }
        #endregion

        #region 窗体事件

        #endregion

        #endregion

        #region 功能

        #region 封装     

        /// <summary>
        /// 密钥文本
        /// </summary>
        private string KeyText
        {
            get => KeyTextBox.Text;
            set
            {
                KeyTextBox.Text = value;
            }
        }

        /// <summary>
        /// 原文件路径文本
        /// </summary>
        private string OriginFilePath
        {
            get
            {
                return TextBoxOriginalFile.Text;
            }
            set
            {
                TextBoxOriginalFile.Text = value;
            }
        }
        /// <summary>
        /// 目标文件路径文本
        /// </summary>
        private string TargetFilePath
        {
            get
            {
                return TextBoxOutPutFile.Text;
            }
            set
            {
                TextBoxOutPutFile.Text = value;
            }
        }


        const string cp_errorText = "错误";

        private bool IsFullPath(string path, out string fullPath, out Exception ex)
        {
            try
            {
                fullPath = Path.GetFullPath(path);
                ex = null;
                return true;
            }
            catch (Exception exc)
            {
                ex = exc;
                fullPath = null;
                return false;
            }
            
        }

        /// <summary>
        /// 获取原文件和输出文件
        /// </summary>
        /// <param name="originFile">原文件流</param>
        /// <param name="targetFile">输出文件流</param>
        /// <returns>是否成功</returns>
        private bool tryGetPathTo(out FileStream originFile, out FileStream targetFile)
        {
            originFile = null;
            targetFile = null;
            string op = OriginFilePath;
            string tp = TargetFilePath;
            bool flag;
            DialogResult d;

            if(op.Length == 0)
            {
                MessageBox.Show("请选择输入路径", cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (tp.Length == 0)
            {
                MessageBox.Show("请选择输出路径", cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            flag = File.Exists(op);
            if (!flag)
            {
                //原文件不存在
                MessageBox.Show("指定的原文件不存在", cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            
            string pt;
            flag = IsFullPath(tp, out pt, out Exception exf);

            if (!flag)
            {
                MessageBox.Show(exf.Message, cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            tp = pt;
            flag = File.Exists(tp);
            if (flag)
            {
                //目标有文件
                d = MessageBox.Show("指定的输出路径已存在文件，是否覆盖？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(d != DialogResult.Yes) return false;
            }
            try
            {
                originFile = new FileStream(op, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 8192);
                targetFile = new FileStream(tp, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite, 8192);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "发生未知错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
           
            return true;
        }

        bool f_getBytesKey(string key, out byte[] buffer)
        {
            int index = 0;
            byte b;

            try
            {
                List<byte> list = p_byteListBuffer;
                list.Clear();
                char* cp = stackalloc char[3];
                cp[2] = '\0';

                while (index < key.Length)
                {
                    if(key[index] == ' ')
                    {
                        index++;
                        continue;
                    }

                    //获取字符
                    cp[0] = key[index];
                    cp[1] = key[index + 1];

                    b = Convert.ToByte(new string(cp), 16);
                    list.Add(b);
                    index += 2;
                }

                buffer = list.ToArray();
                return true;
            }
            catch (Exception)
            {
                buffer = null;
                return false;
            }

        }

        byte[] f_getKey(string key)
        {          
            bool flag;
            byte[] buffer;

            flag = f_getBytesKey(key, out buffer);
            if (flag) return buffer;

            var encoding = p_keyEncoding;
            buffer = encoding.GetBytes(key);
            return buffer;
        }

        private bool tryGetKey(out byte[] key)
        {
            string kt = KeyText;
            key = null;
            if (kt.Length == 0)
            {
                MessageBox.Show("未填写密钥", cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            try
            {
                key = f_getKey(kt);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生未知错误" + Environment.NewLine + ex.Message, cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private bool copyTo(Stream stream, Stream to, byte[] buffer)
        {
            try
            {
                stream.CopyToStream(to, buffer);
                return true;
            }
            catch (Exception ex)
            {
                string error = "发生未知错误" + Environment.NewLine + ex.Message;
                MessageBox.Show(error, cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// 使用对话框获取文件路径
        /// </summary>
        /// <param name="filePath">获取的路径</param>
        /// <returns>成功获取</returns>
        private bool ReadFile(out string filePath)
        {
            var readfile = ReadFileDialog;

            var d = readfile.ShowDialog();

            if(d != DialogResult.OK)
            {
                //取消选择
                filePath = null;
                return false;
            }

            filePath = readfile.FileName;
            return true;
        }
        /// <summary>
        /// 使用对话框保存文件到路径
        /// </summary>
        /// <param name="filePath">获取的保存路径</param>
        /// <returns>成功获取</returns>
        private bool SaveFile(out string filePath)
        {
            var save = SavFileDialog;

            var d = save.ShowDialog();
            if(d != DialogResult.OK)
            {
                filePath = null;
                return false;
            }

            filePath = save.FileName;
            return true;
        }

        /// <summary>
        /// 使用目录对话框获取目录
        /// </summary>
        /// <param name="path">获取的目录</param>
        /// <returns>是否获取</returns>
        //private bool OpenDirectory(out string path)
        //{
        //    var f = FloderDialog;
        //    var d = f.ShowDialog();

        //    if (d != DialogResult.OK)
        //    {
        //        path = null;
        //        return false;
        //    }

        //    path = f.SelectedPath;
        //    return true;
        //}

        /// <summary>
        /// 快速获取
        /// </summary>
        /// <param name="originFile"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        private bool getQukeFile(out FileStream originFile, out FileStream targetFile)
        {
            DialogResult d;
            originFile = null;
            targetFile = null;
            d =  QukeEncFileDialog.ShowDialog();
            if(d != DialogResult.OK)
            {

                return false;
            }
        
            string op = QukeEncFileDialog.FileName;
            string tp = op;
            bool flag;
            
            flag = File.Exists(op);

            if (!flag)
            {
                //原文件不存在
                MessageBox.Show("指定要加密的文件不存在", cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                originFile = new FileStream(op, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 8192);
                targetFile = new FileStream(tp, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite, 8192);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生未知错误" + Environment.NewLine + ex.Message, cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void f_saveKey(string file)
        {
            string key = KeyText;
            byte[] buffer = new byte[4];
            try
            {
                using (FileStream save = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read, 1024))
                {
                    save.Write(SaveHand, 0, 4);

                    key.Length.ToByteArray(buffer, 0);
                    save.Write(buffer, 0, 4);
                    buffer = key.ToByteArray(0);
                    save.Write(buffer, 0, buffer.Length);
                }

                MessageBox.Show("保存成功", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + Environment.NewLine + ex.Message, cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void f_readKey(string file)
        {
            string key = KeyText;
            byte[] buffer = new byte[4];
            int r;
            int length;
            string errorText = string.Empty;
            try
            {
                using (FileStream save = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 1024))
                {
                    r = save.ReadBlock(buffer, 0, 4);

                    if(r != 4 || (!buffer.EqualsBytes(SaveHand)))
                    {
                        save.Close();
                        errorText = "不是密钥文件";
                        goto Over;
                    }

                    r = save.ReadBlock(buffer, 0, 4);
                    if(r != 4)
                    {
                        save.Close();
                        goto Over;
                    }

                    length = buffer.ToStructure<int>(0);

                    if (length > 16384)
                    {
                        save.Close();
                        goto Over;
                    }

                    length *= 2;
                    buffer = new byte[length];
                    r = save.ReadBlock(buffer, 0, length);
                    if(r != length)
                    {
                        save.Close();
                        goto Over;
                    }
                    KeyText = buffer.ToStringBuffer(0);
                }

                MessageBox.Show("读取完毕", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取失败" + Environment.NewLine + ex.Message, cp_errorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Over:
            MessageBox.Show(errorText, "读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        /// <summary>
        /// 开始加密
        /// </summary>
        public void StartEnc()
        {
            
            FileStream ofile, tfile;
            bool flag;
            byte[] key;

            flag = tryGetKey(out key);
            if (!flag)
            {
                return;
            }

            flag = tryGetPathTo(out ofile, out tfile);
            if (!flag)
            {
                ofile?.Close();
                tfile?.Close();
                //失败
                return;
            }


            //初始化加密对象
            XOREncry.Key = key;
            XOREncry.Reset();
            XorStream.SetNewStream(ofile);

            flag = copyTo(XorStream, tfile, Buffer);
            if (flag)
            {
                MessageBox.Show("运行完毕", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ofile?.Close();
            tfile?.Close();
        }
        /// <summary>
        /// 快速加密
        /// </summary>
        public void QukeStart()
        {
            //获取文件
            FileStream ofile, tfile;
            bool flag;
            byte[] key;

            flag = tryGetKey(out key);
            if (!flag)
            {
                return;
            }

            flag = getQukeFile(out ofile, out tfile);
            if (!flag)
            {
                ofile?.Close();
                tfile?.Close();
                return;
            }


            //初始化加密对象
            XOREncry.Key = key;
            XOREncry.Reset();
            XorStream.SetNewStream(ofile);

            flag = copyTo(XorStream, tfile, Buffer);
            if (flag)
            {
                MessageBox.Show("运行完毕", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ofile?.Close();
            tfile?.Close();
        }

        #endregion

        #region 重写

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        //this.ControlBox = false;
        //        const int CP_NOCLOSE_BUTTON = 0x200;
        //        CreateParams myCp = base.CreateParams;
        //        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        //        return myCp;
        //    }
        //}

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            f_save();
            base.OnFormClosing(e);
        }

        #endregion

    }
}
