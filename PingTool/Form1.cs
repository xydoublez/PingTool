using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PingTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(ping);
            t.IsBackground = true;
            t.Start();
        }
        private void ping()
        {
            this.listBox1.BeginInvoke(new Action(() =>
            {
                //this.listBox1.BeginUpdate();
                this.listBox1.Items.Clear();
                //远程服务器IP
                string ipStr = this.textBox1.Text.ToString().Trim();
                //构造Ping实例
                Ping pingSender = new Ping();
                //Ping 选项设置
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                //测试数据
                string data = "test data abcabc";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                //设置超时时间
                int timeout = 6000;
                //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
                while (true)
                {
                    PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        listBox1.Items.Add(string.Format("{0}  来自 {1} 的回复: 字节={2} 时间={3}ms TTL={4}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            reply.Address.ToString(),
                            reply.Buffer.Length,
                            reply.RoundtripTime,
                            reply.Options.Ttl)
                            );

                    }
                    else
                    {
                        listBox1.Items.Add(string.Format("{0} 失败 状态：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), reply.Status.ToString()));
                    }
                    listBox1.Refresh();
                    Thread.Sleep(1000);
                }
            }));

        }
    }
}
