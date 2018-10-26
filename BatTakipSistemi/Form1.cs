using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualBasic.Devices;
namespace BatTakipSistemi
{

    public partial class Form1 : Form
    {
        private static DateTime lastTime;
        private static TimeSpan lastTotalProcessorTime;
        private static DateTime curTime;
        private static TimeSpan curTotalProcessorTime;
        public string DosyaYolu;
        public string VeriTabani;
        public string[,] Dizi = { { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "0", "0", "0", "0", "0", "0", "0" }, { "explorer", "0", "0", "0", "0", "0", "0" } };
        public int[] Dizi2 = {10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000 };
        public long totalGBRam = Convert.ToInt32((new ComputerInfo().TotalPhysicalMemory / (Math.Pow(1024, 3))) + 0.5);
        public int p;
        public Form1()
        {
            InitializeComponent();
        }



        //-------------------------------------------- KAYIT İŞLEMLERİ ----------------------------------------------

        void kayitekle()     
        {
            try
            {
                if (VeriTabani != null)
                {
                    if (textBox1.Text != "")
                    {
                        OleDbCommand komut = new OleDbCommand();
                        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + VeriTabani + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
                        baglanti.Open();
                        komut.Connection = baglanti;
                        string sql = "Insert into [Sayfa1$] (ProgramYolu,ProgramAdi,Ram) values('" + DosyaYolu + "','" + textBox1.Text + "','" + textBox2.Text + "')";
                        komut.CommandText = sql;
                        komut.ExecuteNonQuery(); Application.Restart(); 
                        if (p == 1)
                        {
                            MessageBox.Show("Kayıt İşlemlerinden Sonra Programı Yeniden Başlatın"); p--;
                        }
                        

                        baglanti.Close();
                       
                    }
                    else
                    {
                        MessageBox.Show("Program Adı Girin!");
                    }
                }
                else
                {
                    MessageBox.Show("Veri Tabanı Seçin!");
                }
            }
            catch
            {

            }

        }
  

        private void button1_Click(object sender, EventArgs e)
        {
            kayitekle();
        }


        private void button2_Click(object sender, EventArgs e)  
        {
            OpenFileDialog dosya = new OpenFileDialog();

            dosya.Filter = "Bat Dosyası | *.exe; | Bat Dosyası | *.bat; | Tüm Dosyalar | *.* ";
            dosya.Title = "Bimsa Console İzleme";
            dosya.ShowDialog();
            DosyaYolu = dosya.FileName;

        }

        //-------------------------------------------- FORMLOAD ----------------------------------------------

        private void Form1_Load(object sender, EventArgs e)
        {int maxram= Convert.ToInt16(totalGBRam * 1000);  textBox2.Text = 1000.ToString();
            progressBar2.Maximum = maxram;
            progressBar4.Maximum = maxram;
            progressBar6.Maximum = maxram;
            progressBar8.Maximum = maxram;
            progressBar10.Maximum = maxram;
            progressBar12.Maximum = maxram;
            progressBar14.Maximum = maxram;
            progressBar16.Maximum = maxram;
            progressBar18.Maximum = maxram;
            progressBar20.Maximum = maxram;
            progressBar22.Maximum = maxram;
            progressBar24.Maximum = maxram;
            VeriTabani = "C:\\Uygulama Takip/takipprogrami.xlsx"; p = 1;
            OpenFile();
            timer1.Stop();

        }

        private void button4_Click(object sender, EventArgs e)  //    ----------------- Veritabanı Seç ----------------- 
        {
            OpenFileDialog dosya2 = new OpenFileDialog();
            dosya2.Filter = "Excell Dosyası | *.xlsx; | Tüm Dosyalar | *.* ";
            dosya2.Title = "Bimsa Console İzleme       - Excell Dosyası Seçin!";
            dosya2.ShowDialog();
            VeriTabani = dosya2.FileName;
            kayitduzenle kayit2 = new kayitduzenle();
            OpenFile();
        }

        private void button3_Click(object sender, EventArgs e)  //    ----------------- Kayit Düzenle -----------------
        {
            if (VeriTabani != null)
            {
                kayitduzenle kayit2 = new kayitduzenle();
                kayit2.label2.Text = VeriTabani;
                kayit2.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veri Tabanı Seçin!");
            }
        }


        //-------------------------------------------- LİSTELEME ----------------------------------------------

        private void ProgramlariListele()
        {
           

            for (int t = 1; t < 14; t++)
            {
                foreach (Process p in Process.GetProcesses("."))
                {
                    if (p.ProcessName =="vlc")
                    {
                       
                        Dizi[t, 4] = "Başlangıç Zamanı:"+p.StartTime.ToString();

                        /* ram */
                        Dizi[t, 2] = ((Convert.ToInt32(Math.Round(Convert.ToDecimal(p.PrivateMemorySize64 / 1024))) / 2000) ).ToString();
                        // ON/OFF
                        Dizi[t, 3] = (p.Responding.ToString());
                        /* CPu */
                        int i = 0;
                        while (i < 2)
                        {
                            try
                            {
                                Process[] pp = Process.GetProcessesByName(Dizi[t, 0]);


                                Process t1 = pp[0];
                                if (lastTime == null || lastTime == new DateTime())
                                {
                                    lastTime = DateTime.Now;
                                    lastTotalProcessorTime = t1.TotalProcessorTime;
                                }
                                else
                                {
                                    curTime = DateTime.Now;
                                    curTotalProcessorTime = t1.TotalProcessorTime;

                                    double CPUUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);
                                    Dizi[t, 1] = Convert.ToInt32((CPUUsage * 100)).ToString();

                                    lastTime = curTime;
                                    lastTotalProcessorTime = curTotalProcessorTime;
                                }


                                Thread.Sleep(200); 
                            }
                            catch
                            {
                                
                            }
                            i++;
                        }
                       


                        Thread.Sleep(250);

                        //_----------------------------------------------------------------------------YAZDIRMA İŞLEMİ------------------------------------------------------------------------------
                        
                        label8.Text = "%"+Dizi[1, 1];
                        if (Convert.ToInt16(Dizi[1, 2]) > Dizi2[1])
                            label9.ForeColor = System.Drawing.Color.Red;
                        else
                            label9.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[2, 2]) > Dizi2[2])
                            label12.ForeColor = System.Drawing.Color.Red;
                        else
                            label12.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[3, 2]) > Dizi2[3])
                            label20.ForeColor = System.Drawing.Color.Red;
                        else
                            label20.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[4, 2]) > Dizi2[4])
                            label29.ForeColor = System.Drawing.Color.Red;
                        else
                            label29.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[5, 2]) > Dizi2[5])
                            label61.ForeColor = System.Drawing.Color.Red;
                        else
                            label61.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[6, 2]) > Dizi2[6])
                            label53.ForeColor = System.Drawing.Color.Red;
                        else
                            label53.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[7, 2]) > Dizi2[7])
                            label45.ForeColor = System.Drawing.Color.Red;
                        else
                            label45.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[8, 2]) > Dizi2[8])
                            label37.ForeColor = System.Drawing.Color.Red;
                        else
                            label37.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[9, 2]) > Dizi2[9])
                            label109.ForeColor = System.Drawing.Color.Red;
                        else
                            label109.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[10, 2]) > Dizi2[10])
                            label101.ForeColor = System.Drawing.Color.Red;
                        else
                            label101.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[11, 2]) > Dizi2[11])
                            label77.ForeColor = System.Drawing.Color.Red;
                        else
                            label77.ForeColor = System.Drawing.Color.Green;
                        if (Convert.ToInt16(Dizi[12, 2]) > Dizi2[12])
                            label69.ForeColor = System.Drawing.Color.Red;
                        else
                            label69.ForeColor = System.Drawing.Color.Green;
                        label3.Text = Dizi[1, 0];
                        label9.Text = Dizi[1, 2] + "MB";
                        label10.Text = Dizi[1, 3];
                        label74.Text = Dizi[1, 4];
                        label18.Text = Dizi[2, 0];
                        label13.Text = "%" + Dizi[2, 1];
                        label12.Text = Dizi[2, 2] + "MB";
                        label11.Text = Dizi[2, 3];
                        label82.Text = Dizi[2, 4];
                        label26.Text = Dizi[3, 0];
                        label21.Text = "%" + Dizi[3, 1];
                        label20.Text = Dizi[3, 2] + "MB";
                        label19.Text = Dizi[3, 3];
                        label86.Text = Dizi[3, 4];
                        label35.Text = Dizi[4, 0];
                        label30.Text = "%" + Dizi[4, 1];
                        label29.Text = Dizi[4, 2] + "MB";
                        label28.Text = Dizi[4, 3];
                        label90.Text = Dizi[4, 4];
                        label67.Text = Dizi[5, 0];
                        label62.Text = "%" + Dizi[5, 1];
                        label61.Text = Dizi[5, 2] + "MB";
                        label60.Text = Dizi[5, 3];
                        label87.Text = Dizi[5, 4];
                        label59.Text = Dizi[6, 0];
                        label54.Text = "%" + Dizi[6, 1];
                        label53.Text = Dizi[6, 2] + "MB";
                        label52.Text = Dizi[6, 3];
                        label88.Text = Dizi[6, 4];
                        label51.Text = Dizi[7, 0];
                        label46.Text = "%" + Dizi[7, 1];
                        label45.Text = Dizi[7, 2] + "MB";
                        label44.Text = Dizi[7, 3];
                        label89.Text = Dizi[7, 4];
                        label43.Text = Dizi[8, 0];
                        label38.Text = "%" + Dizi[8, 1];
                        label37.Text = Dizi[8, 2] + "MB";
                        label36.Text = Dizi[8, 3];
                        label91.Text = Dizi[8, 4];
                        label115.Text = Dizi[9, 0];
                        label110.Text = "%" + Dizi[9, 1];
                        label109.Text = Dizi[9, 2] + "MB";
                        label108.Text = Dizi[9, 3];
                        label95.Text = Dizi[9, 4];
                        label107.Text = Dizi[10, 0];
                        label102.Text = "%" + Dizi[10, 1];
                        label101.Text = Dizi[10, 2] + "MB";
                        label100.Text = Dizi[10, 3];
                        label94.Text = Dizi[10, 4];
                        label83.Text = Dizi[11, 0];
                        label78.Text = "%" + Dizi[11, 1];
                        label77.Text = Dizi[11, 2] + "MB";
                        label76.Text = Dizi[11, 3];
                        label93.Text = Dizi[11, 4];
                        label75.Text = Dizi[12, 0];
                        label70.Text = "%" + Dizi[12, 1];
                        label69.Text = Dizi[12, 2]+"MB";
                        label68.Text = Dizi[12, 3];
                        label92.Text = Dizi[12, 4];
                //---------------------------------------Progress BAR ----------------------------------------
                        progressBar1.Value = Convert.ToInt16(Dizi[1,1]);
                        progressBar2.Value = Convert.ToInt16(Dizi[1,2]);
                        progressBar3.Value = Convert.ToInt16(Dizi[2, 1]);
                        progressBar4.Value = Convert.ToInt16(Dizi[2, 2]);
                        progressBar5.Value = Convert.ToInt16(Dizi[3, 1]);
                        progressBar6.Value = Convert.ToInt16(Dizi[3, 2]);
                        progressBar7.Value = Convert.ToInt16(Dizi[4, 1]);
                        progressBar8.Value = Convert.ToInt16(Dizi[4, 2]);
                        progressBar9.Value = Convert.ToInt16(Dizi[5, 1]);
                        progressBar10.Value = Convert.ToInt16(Dizi[5, 2]);
                        progressBar11.Value = Convert.ToInt16(Dizi[6, 1]);
                        progressBar12.Value = Convert.ToInt16(Dizi[6, 2]);
                        progressBar13.Value = Convert.ToInt16(Dizi[7, 1]);
                        progressBar14.Value = Convert.ToInt16(Dizi[7, 2]);
                        progressBar15.Value = Convert.ToInt16(Dizi[8, 1]);
                        progressBar16.Value = Convert.ToInt16(Dizi[8, 2]);
                        progressBar17.Value = Convert.ToInt16(Dizi[9, 1]);
                        progressBar18.Value = Convert.ToInt16(Dizi[9, 2]);
                        progressBar19.Value = Convert.ToInt16(Dizi[10, 1]);
                        progressBar20.Value = Convert.ToInt16(Dizi[10, 2]);
                        progressBar21.Value = Convert.ToInt16(Dizi[11, 1]);
                        progressBar22.Value = Convert.ToInt16(Dizi[11, 2]);
                        progressBar23.Value = Convert.ToInt16(Dizi[12, 1]);
                        progressBar24.Value = Convert.ToInt16(Dizi[12, 2]);

                        //------------------------------Panel RENKLENDİRME------------------------------
                        if (Dizi[1,3]=="FALSE")
                            panel1.BackColor = Color.Crimson;                    
                        else
                            panel1.BackColor = Color.White;

                        if (Dizi[2, 3] == "FALSE")
                            panel3.BackColor = Color.Crimson;
                        else
                            panel3.BackColor = Color.White;

                        if (Dizi[3, 3] == "FALSE")
                            panel4.BackColor = Color.Crimson;
                        else
                            panel4.BackColor = Color.White;

                        if (Dizi[4, 3] == "FALSE")
                            panel5.BackColor = Color.Crimson;
                        else
                            panel5.BackColor = Color.White;

                        if (Dizi[5, 3] == "FALSE")
                            panel9.BackColor = Color.Crimson;
                        else
                            panel9.BackColor = Color.White;

                        if (Dizi[6, 3] == "FALSE")
                            panel8.BackColor = Color.Crimson;
                        else
                            panel8.BackColor = Color.White;

                        if (Dizi[7, 3] == "FALSE")
                            panel7.BackColor = Color.Crimson;
                        else
                            panel7.BackColor = Color.White;

                        if (Dizi[8, 3] == "FALSE")
                            panel6.BackColor = Color.Crimson;
                        else
                            panel6.BackColor = Color.White;

                        if (Dizi[9, 3] == "FALSE")
                            panel15.BackColor = Color.Crimson;
                        else
                            panel15.BackColor = Color.White;

                        if (Dizi[10, 3] == "FALSE")
                            panel14.BackColor = Color.Crimson;
                        else
                            panel14.BackColor = Color.White;

                        if (Dizi[11, 3] == "FALSE")
                            panel11.BackColor = Color.Crimson;
                        else
                            panel11.BackColor = Color.White;

                        if (Dizi[12, 3] == "FALSE")
                            panel10.BackColor = Color.Crimson;
                        else
                            panel10.BackColor = Color.White;
                        //_----------------------------------------------------------------------------------------------------------------------------------------------------------
                        break;
                    }

                    else
                    {
                        if(Dizi[t,3]=="True")
                        {
                            Dizi[t, 4] = "Kapanma Zamanı=" + DateTime.Now;
                        }
                        Dizi[t, 3] = "FALSE";
                        Dizi[t, 2] = "0";
                        Dizi[t, 1] = "0";
                        
                    }

                }
            }
           

        }




        private void timer1_Tick(object sender, EventArgs e)
        {
             ProgramlariListele();
        }


        //-------------------------------------------- VERİLERİ ÇEK ----------------------------------------------

        public void OpenFile()       
        {
            excell excel = new excell(@"" + VeriTabani + "", 1);
            Dizi[1, 0] = excel.ReadCell(1, 1);
            Dizi[2, 0] = excel.ReadCell(2, 1);
            Dizi[3, 0] = excel.ReadCell(3, 1);
            Dizi[4, 0] = excel.ReadCell(4, 1);
            Dizi[5, 0] = excel.ReadCell(5, 1);
            Dizi[6, 0] = excel.ReadCell(6, 1);
            Dizi[7, 0] = excel.ReadCell(7, 1);
            Dizi[8, 0] = excel.ReadCell(8, 1);
            Dizi[9, 0] = excel.ReadCell(9, 1);
            Dizi[10, 0] = excel.ReadCell(10, 1);
            Dizi[11, 0] = excel.ReadCell(11, 1);
            Dizi[12, 0] = excel.ReadCell(12, 1);
            Dizi[1, 5] = excel.ReadCell(1, 0);
            Dizi[2, 5] = excel.ReadCell(2, 0);
            Dizi[3, 5] = excel.ReadCell(3, 0);
            Dizi[4, 5] = excel.ReadCell(4, 0);
            Dizi[5, 5] = excel.ReadCell(5, 0);
            Dizi[6, 5] = excel.ReadCell(6, 0);
            Dizi[7, 5] = excel.ReadCell(7, 0);
            Dizi[8, 5] = excel.ReadCell(8, 0);
            Dizi[9, 5] = excel.ReadCell(9, 0);
            Dizi[10, 5] = excel.ReadCell(10, 0);
            Dizi[11, 5] = excel.ReadCell(11, 0);
            Dizi[12, 5] = excel.ReadCell(12, 0);
            Dizi2[1] = Convert.ToInt16(excel.ReadCell(1, 2));
            Dizi2[2] = Convert.ToInt16(excel.ReadCell(2, 2));
            Dizi2[3] = Convert.ToInt16(excel.ReadCell(3, 2));
            Dizi2[4] = Convert.ToInt16(excel.ReadCell(4, 2));
            Dizi2[5] = Convert.ToInt16(excel.ReadCell(5, 2));
            Dizi2[6] = Convert.ToInt16(excel.ReadCell(6, 2));
            Dizi2[7] = Convert.ToInt16(excel.ReadCell(7, 2));
            Dizi2[8] = Convert.ToInt16(excel.ReadCell(8, 2));
            Dizi2[9] = Convert.ToInt16(excel.ReadCell(9, 2));
            Dizi2[10] = Convert.ToInt16(excel.ReadCell(10, 2));
            Dizi2[11] = Convert.ToInt16(excel.ReadCell(11, 2));
            Dizi2[12] = Convert.ToInt16(excel.ReadCell(12, 2));
            excel.kapat();


        }
        //-------------------------------------------- PROGRAMI BAŞLAT -----------------------------------------

        private void button5_Click(object sender, EventArgs e)  
        {
            if (VeriTabani != null)
            {
                //ProgramlariListele();
                timer1.Enabled = true;
                timer1.Start();
            }

            else
                MessageBox.Show("Veri Tabanı Seçin");
        }

        //--------------------------------------------UYGULAMA ÇALIŞTIRMA -------------------------------------------
        private void button7_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (Dizi[1, 5] != "0")
                System.Diagnostics.Process.Start(@"" + Dizi[1, 5] + "");
            else
                MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[2, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[2, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[3, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[3, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[4, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[4, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[5, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[5, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[6, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[6, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[7, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[7, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[8, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[8, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[9, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[9, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[10, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[10, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[11, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[11, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[12, 5] != "0")
                    System.Diagnostics.Process.Start(@"" + Dizi[12, 5] + "");
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        //--------------------------------------------UYGULAMA KAPATMA --------------------------------------------

        private void button18_Click(object sender, EventArgs e)
        {
            try{
                if (Dizi[1, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[1, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch{
                MessageBox.Show("Tekrar Deneyin");
            }
           
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[2, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[2, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[3, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[3, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[4, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[4, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[5, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[5, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[6, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[6, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[7, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[7, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[8, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[8, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[9, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[9, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[10, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[10, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[11, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[11, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                if (Dizi[12, 0] != "0")
                {
                    foreach (var process in Process.GetProcessesByName(@"" + Dizi[12, 0] + ""))
                    {
                        process.Kill();
                        MessageBox.Show("Ugulama Kapandı!");
                    }
                }
                else
                    MessageBox.Show("Ugulama Bulunamadı!");
            }
            catch
            {
                MessageBox.Show("Tekrar Deneyin");
            }
        }

        
    }
}
