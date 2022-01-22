using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;
using MediaToolkit;
using System.IO;
using System.Net;

namespace youtubedownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool Durum = true;

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //await
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Lütfen kaydetmek istediğiniz dosyayı seçin" })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {


                    GetTitle();
                    label4.Text = "İndirme işlemi başladı. Lütfen Bekleyin.";
                    label4.ForeColor = Color.Red;

                    var youtube = YouTube.Default;
                    var video = await youtube.GetVideoAsync(textBox1.Text);
                    File.WriteAllBytes(fbd.SelectedPath + @"\" + video.FullName, await video.GetBytesAsync());


                    var imputFile = new MediaToolkit.Model.MediaFile { Filename = fbd.SelectedPath + @"\" + video.FullName };
                    var outputFile = new MediaToolkit.Model.MediaFile { Filename = $"{fbd.SelectedPath + @"\" + video.FullName}.mp3" };

                    using (var enging = new Engine())
                    {

                        enging.GetMetadata(imputFile);
                        enging.Convert(imputFile, outputFile);
                    }
                    if (Durum == true)
                    {
                        File.Delete(fbd.SelectedPath + @"\" + video.FullName);
                    }
                    else
                    {
                        File.Delete($"{fbd.SelectedPath + @"\" + video.FullName}.mp3");
                    }



                    label4.Text = "İndirme işlemi tamamlandı!";
                    label4.ForeColor = Color.Green;




                }
                else
                {
                    MessageBox.Show("Lütfen bir dosya yolu seçin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void GetTitle()
        {
            WebRequest istek = HttpWebRequest.Create(textBox1.Text);
            WebResponse yanıt;
            yanıt = istek.GetResponse();
            StreamReader bilgiler = new StreamReader(yanıt.GetResponseStream());
            string gelen = bilgiler.ReadToEnd();
            int baslangic = gelen.IndexOf("<title>") + 7;
            int bitis = gelen.Substring(baslangic).IndexOf("</title>");
            string gelenbilgiler = gelen.Substring(baslangic, bitis);
            label5.Text = (gelenbilgiler);
        }

       
       

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult Cikis;
            Cikis = MessageBox.Show("Program Kapatılacak Emin siniz?", "Kapatma Uyarısı!", MessageBoxButtons.YesNo);
            if (Cikis == DialogResult.Yes)
            {
                Application.Exit();
            }
            if (Cikis == DialogResult.No)
            {
                Application.Run();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Durum = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Durum = false;
        }
    }
}

