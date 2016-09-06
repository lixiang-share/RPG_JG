using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeTui_demo_02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WebBrowser webBrower = new WebBrowser();
           // webBrower.ScriptErrorsSuppressed = true;
           webBrower.DocumentCompleted +=
            new WebBrowserDocumentCompletedEventHandler(PrintDocument);
           webBrower.Navigate("https://dev.getui.com/dos4.0/index.html#login");
        }


        private void PrintDocument(object sender,WebBrowserDocumentCompletedEventArgs e)
        {
           
            WebBrowser webBrower = ((WebBrowser)sender);
            HtmlDocument hd = webBrower.Document;
            string value = hd.GetElementById("password").InnerText;
            Logger.Log(value);
            HtmlElement he = hd.GetElementById("btn");
            Logger.Log(webBrower.DocumentText);
        }
    }
}
