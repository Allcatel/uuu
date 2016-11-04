using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class Factor
        {
            int k;
            int n;
            double rangs;

            List<List<int>> list = new List<List<int>>();
            List<List<double>> rangbiglist = new List<List<double>>();
            List<numkol> ranglist = new List<numkol>();
            List<int> sortlist = new List<int>();

            List<double> rangsr = new List<double>();
            List<double> rangsum = new List<double>();

            struct numkol
            {
                public int num;
                public double ran;

                  public numkol (int nu, double ra)
                {
                    num = nu;
                    ran = ra;
                }
            }

            public List<List<int>> GetList ()
            {
                return list;
            }

            public List<List<double>> GetRangList()
            {
                return rangbiglist;
            }
            public void TableBuild( DataGridView table, int width)
            {
                DataGridViewTextBoxColumn col0;
               // n = num; k = kol;
                for (int i = 0; i < k; i++)
                {
                    col0 = new DataGridViewTextBoxColumn();
                    col0.Width = width;
                    col0.HeaderText = Convert.ToString(i+1);
                    table.Columns.Add(col0);
                }

                table.Rows.Add(n);
                for (int i = 0; i < n; i ++)
                {
                        table.Rows[i].HeaderCell.Value = Convert.ToString(i + 1);
                        table.Rows[i].Height = 20; 
                }
                
            }

            public void TableFill(DataGridView table)
            {
                for (int i = 0; i<n; i++)
                {
                    for (int j = 0; j < k; j++)
                        table.Rows[i].Cells[j].Value = list[i][j];
                }
            }

            public void TableRangFill(DataGridView table)
            {
                table.Rows.Add(2);
                table.Rows[n].HeaderCell.Value = "Rj";
                table.Rows[n + 1].HeaderCell.Value = "rj";
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < k; j++)
                        table.Rows[i].Cells[j].Value = Math.Round(rangbiglist[i][j], 3);
                }
                double temp;
                for (int i=0; i<k; i++)
                {
                    temp = new double();
                    temp = 0;
                    for (int j=0; j<n; j ++)
                    {
                        temp += rangbiglist[j][i];
                    }
                    rangsum.Add(temp);      
                    rangsr.Add(temp / n);
                    table.Rows[n].Cells[i].Value = Math.Round(temp, 3);
                    table.Rows[n+1].Cells[i].Value = Math.Round(temp/n, 3);
                    rangs += temp;
                }

            }

            public void FileRead(string filepath)
            {
                bool reading = true;
                string[] temp = new string[255];
                List<int> templist = new List<int>();
                try
                {
                    temp = File.ReadAllLines(filepath);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("File not found");
                }

                int key = 0;
                bool enterance = false;
                foreach (string x in temp)
                {             
                    templist = new List<int>();
                    foreach (string z in x.Split(' '))
                    {
                        if (key == 2)
                        { templist.Add(Convert.ToInt32(z)); enterance = true; }
                        else if (key == 1) { k = Convert.ToInt32(z); key++; }
                        else { n = Convert.ToInt32(z); key++; }
                    }
                    if (enterance == true)
                    {
                        list.Add(templist);
                        sortlist.AddRange(templist);
                    }
                }
                sortlist.Sort();
            }

            public void Rang ()
            {
                int kol = 0;
                double place = 0;  
                int num = sortlist[0];
                for (int i =0; i<sortlist.Count; i++)
                {
                    if (num == sortlist[i]) { kol++; place += i + 1; }
                    else { ranglist.Add(new numkol(num, place/kol)); num = sortlist[i]; kol = 1; place = i+1; }
                }
                ranglist.Add(new numkol(num, place/kol));
                Ranglistfill();
            }

            private void Ranglistfill()
            {
                int temp;
                int z;
                List<double> tlist;
                for (int i = 0; i < n; i++)
                {
                    tlist  = new List<double>();
                    for (int j = 0; j < k; j++)
                    {
                        temp = list[i][j];
                        z = 0;
                       while (ranglist[z].num != temp)
                          z++;
                        tlist.Add(ranglist[z].ran);
                    }
                    rangbiglist.Add(tlist);
                }
            }

            
        }

        Factor fact = new Factor();
        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
            Factor fact = new Factor();
            fact.FileRead(textBox3.Text);
            fact.TableBuild(table, 40);
            button2.Enabled = true; button2.Visible = true;
            button1.Enabled = false;
            fact.TableFill(table);
            table2.Visible = true;
            fact.TableBuild(table2, 40);
            fact.Rang();
            fact.TableRangFill(table2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
