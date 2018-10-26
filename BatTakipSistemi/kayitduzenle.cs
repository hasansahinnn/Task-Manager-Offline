using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace BatTakipSistemi
{
    public partial class kayitduzenle : Form
    {

        Form1 frm1 = new Form1();
        public string VeriTabani2;
        void veritabanigoster()     //    ----------------- Kayit Göster ----------------- 
        {
            if (VeriTabani2 != null)
            {
                OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + VeriTabani2 + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
                baglanti.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [Sayfa1$]", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt.DefaultView;
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Veri Tabanı Seçin!");
            }
        }
        
    


        public kayitduzenle()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            veritabanigoster();
        }

        private void kayitduzenle_Load(object sender, EventArgs e)
        {
            VeriTabani2 = label2.Text;
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            excell exc = new excell(VeriTabani2,1);
            int sil = Convert.ToInt32(textBox1.Text);
            exc.DeleteWorksheet(sil+1);
            exc.kapat();
            veritabanigoster();
        }

        
    }
}
