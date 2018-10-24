using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace ADS3
{
    public partial class Form1 : Form
    {
        int ex = 0;
        int job = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox2.ReadOnly = true;
            textBox1.ReadOnly = false;
            label2.Visible = true;
            label3.Visible = true;
            for (int i = 0; i < job; i++)
            {
                for (int j = 0; j < job; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = null;
                }
            }
            dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
            dataGridView1.Visible = false;
            label4.Visible = false;
            button5.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Regex r = new Regex(@"[\d]+");
            if (r.IsMatch(textBox1.Text) == true)
            {
                if (Convert.ToInt32(textBox1.Text) > 0)
                {
                    button3.Visible = true;
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ex = Convert.ToInt32(textBox1.Text);
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Regex r = new Regex(@"[\d]+");
            if (r.IsMatch(textBox2.Text) == true)
            {
                if (Convert.ToInt32(textBox2.Text) > 0)
                {
                    button4.Visible = true;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            job = Convert.ToInt32(textBox2.Text);
            textBox2.ReadOnly = true;
            label4.Text = "Fill the matrix with lengths";
            label4.Visible = true;
            dataGridView1.Visible = true;
            for(int i = 1; i <= job; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), i.ToString());
            }
            for(int i = 1; i <= job; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i - 1].HeaderCell.Value = i;
            }
            button5.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox2.ReadOnly = true;
            textBox1.ReadOnly = false;
            label2.Visible = true;
            label3.Visible = true;
            for (int i = 0; i < job; i++)
            {
                for (int j = 0; j < job; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = null;
                }
            }
            button3.Visible = false;
            button4.Visible = false;
            dataGridView1.Visible = false;
            label4.Visible = false;
            button5.Visible = false;
            for(int i = 0; i < job; i++)
            {
                for(int j = 0; j < job; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = null;
                }
            }
            label2.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label3.Visible = false;
            StreamReader Reader = new StreamReader("input.txt");
            int countEmp = Convert.ToInt32(Reader.ReadLine());
            string[] Emp = new string[countEmp];
            List<Class1> main = new List<Class1>();// вершини с их глубиной, названиями других вершин которые в нее входят и выходят
            {
                string[] all = new string[2];
                while (true)
                {
                    try
                    {
                        all = Reader.ReadLine().Split(' ');
                    }
                    catch { break; }
                    bool ok1 = false, ok2 = false;
                    for (int i = 0; i < main.Count; i++)// добавляет в список вершину и записывает в нее воходящие и выходящие
                    {
                        if (main[i].th == all[0])
                        {
                            main[i].outp.Add(all[1]);
                            if (ok2 == true) break;
                            ok1 = true;
                        }
                        else
                            if (main[i].th == all[1])
                        {
                            main[i].into.Add(all[0]);
                            if (ok1 == true) break;
                            ok2 = true;
                        }
                    }
                    if (ok1 == false) // если нет вершины добалвяет
                    {
                        main.Add(new Class1() { th = all[0] });
                        main[main.Count - 1].outp.Add(all[1]);
                    }
                    if (ok2 == false)
                    {
                        main.Add(new Class1() { th = all[1] });
                        main[main.Count - 1].into.Add(all[0]);
                    }
                }
            }
            Reader.Close();
            {
                List<int> treeWO = new List<int>();
                for (int i = 0; i < main.Count; i++)
                    if (main[i].outp.Count == 0) { treeWO.Add(i); main[i].deep = 0; }
                foreach (int i in treeWO) //считает глубину
                {
                    foreach (string s in main[i].into)
                        Fill(ref main, s, 1);
                }
            }
            main.Sort(); //сортирует по глубине
            int c = main[0].deep, stop = 0;
            List<string> Done = new List<string>();
            List<int> shouldBeD = new List<int>();
            while (Done.Count < main.Count)
            {
                bool[] ok = new bool[countEmp];
                int t = 0;
                List<string> doneInThisStep = new List<string>();
                if (shouldBeD.Count != 0) //если есть те которые надо сделать, то делает 
                {
                    for (int i = 0; i < shouldBeD.Count; i++)
                    {
                        if (t >= countEmp) break;
                        if (main[shouldBeD[i]].Can(Done) == false) { ok[t] = true; Emp[t++] += main[shouldBeD[i]].th; doneInThisStep.Add(main[shouldBeD[i]].th); shouldBeD.RemoveAt(i); }
                    }
                }
                if (t < countEmp) //заняты ли все рабочие 
                    for (int j = t; j < countEmp && stop < main.Count; j++)
                    {
                        while (stop < main.Count && main[stop].Can(Done)) shouldBeD.Add(stop++);
                        if (stop != main.Count)
                        {
                            ok[j] = true;
                            Emp[j] += main[stop].th;
                            doneInThisStep.Add(main[stop++].th);
                        }
                    }
                foreach (string s in doneInThisStep)
                {
                    Done.Add(s);
                }
                for (int i = 0; i < countEmp; i++)
                {
                    if (ok[i] == false) Emp[i] += " ";
                }
            }
            FileStream f = new FileStream("temp2.txt", FileMode.OpenOrCreate);
            StreamWriter w = new StreamWriter(f);
            foreach (string s in Emp)
            {
                if (s != null)
                {
                    for (int i = 0; i < s.Length - 1; i++)
                        w.Write("{0,3}", s[i]);
                    w.WriteLine("{0,3}", s[s.Length - 1]);
                }
            }
            w.Close();
            f.Close();
            StreamReader r = new StreamReader("temp2.txt");
            MessageBox.Show(r.ReadToEnd());
            r.Close();
            File.Delete("temp2.txt");
            File.Delete("temp.txt");
        }
        static void Fill(ref List<Class1> main, string name, int c)
        {
            for (int i = 0; i < main.Count; i++)
            {
                if (main[i].th == name) if (main[i].deep < c)
                    {
                        main[i].deep = c++;
                        foreach (string s in main[i].into)
                            Fill(ref main, s, c);
                    }
            }
        }
        class Class1 : IComparable
        {
            public List<string> into = new List<string>();
            public List<string> outp = new List<string>();
            public string th;
            public int deep;

            public int CompareTo(object obj)
            {
                Class1 temp = (Class1)obj;
                return deep.CompareTo(temp.deep) * -1;
            }
            public bool Can(List<string> Done)
            {
                int g = 0;
                for (int i = 0; i < Done.Count; i++)
                {
                    if (into.Contains(Done[i])) g++;
                    if (g == into.Count) return false;
                }
                if (g == into.Count) return false;
                return true;
            }
        }
        char[] names = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private void button5_Click(object sender, EventArgs e)
        {
            FileStream f1 = new FileStream("temp.txt", FileMode.OpenOrCreate);
            StreamWriter w1 = new StreamWriter(f1);
            w1.WriteLine(ex);
            for(int i = 0; i < job; i++)
            {
                for(int j = 0; j < job; j++)
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value) == 1)
                    {
                        w1.WriteLine(names[i] + " " + names[j]);
                    }
                }
            }
            w1.Close();
            f1.Close();
            StreamReader Reader = new StreamReader("temp.txt");
            int countEmp = Convert.ToInt32(Reader.ReadLine());
            string[] Emp = new string[countEmp];
            List<Class1> main = new List<Class1>();// вершини с их глубиной, названиями других вершин которые в нее входят и выходят
            {
                string[] all = new string[2];
                while (true)
                {
                    try
                    {
                        all = Reader.ReadLine().Split(' ');
                    }
                    catch { break; }
                    bool ok1 = false, ok2 = false;
                    for (int i = 0; i < main.Count; i++)// добавляет в список вершину и записывает в нее воходящие и выходящие
                    {
                        if (main[i].th == all[0])
                        {
                            main[i].outp.Add(all[1]);
                            if (ok2 == true) break;
                            ok1 = true;
                        }
                        else
                            if (main[i].th == all[1])
                        {
                            main[i].into.Add(all[0]);
                            if (ok1 == true) break;
                            ok2 = true;
                        }
                    }
                    if (ok1 == false) // если нет вершины добалвяет
                    {
                        main.Add(new Class1() { th = all[0] });
                        main[main.Count - 1].outp.Add(all[1]);
                    }
                    if (ok2 == false)
                    {
                        main.Add(new Class1() { th = all[1] });
                        main[main.Count - 1].into.Add(all[0]);
                    }
                }
            }
            Reader.Close();
            {
                List<int> treeWO = new List<int>();
                for (int i = 0; i < main.Count; i++)
                    if (main[i].outp.Count == 0) { treeWO.Add(i); main[i].deep = 0; }
                foreach (int i in treeWO) //считает глубину
                {
                    foreach (string s in main[i].into)
                        Fill(ref main, s, 1);
                }
            }
            main.Sort(); //сортирует по глубине
            int c = main[0].deep, stop = 0;
            List<string> Done = new List<string>();
            List<int> shouldBeD = new List<int>();
            while (Done.Count < main.Count)
            {
                bool[] ok = new bool[countEmp];
                int t = 0;
                List<string> doneInThisStep = new List<string>();
                if (shouldBeD.Count != 0) //если есть те которые надо сделать, то делает 
                {
                    for (int i = 0; i < shouldBeD.Count; i++)
                    {
                        if (t >= countEmp) break;
                        if (main[shouldBeD[i]].Can(Done) == false) { ok[t] = true; Emp[t++] += main[shouldBeD[i]].th; doneInThisStep.Add(main[shouldBeD[i]].th); shouldBeD.RemoveAt(i); }
                    }
                }
                if (t < countEmp) //заняты ли все рабочие 
                    for (int j = t; j < countEmp && stop < main.Count; j++)
                    {
                        while (stop < main.Count && main[stop].Can(Done)) shouldBeD.Add(stop++);
                        if (stop != main.Count)
                        {
                            ok[j] = true;
                            Emp[j] += main[stop].th;
                            doneInThisStep.Add(main[stop++].th);
                        }
                    }
                foreach (string s in doneInThisStep)
                {
                    Done.Add(s);
                }
                for (int i = 0; i < countEmp; i++)
                {
                    if (ok[i] == false) Emp[i] += " ";
                }
            }
            FileStream f = new FileStream("temp2.txt", FileMode.OpenOrCreate);
            StreamWriter w = new StreamWriter(f);
            foreach (string s in Emp)
            {
                if (s != null)
                {
                    for (int i = 0; i < s.Length - 1; i++)
                        w.Write("{0,3}", s[i]);
                    w.WriteLine("{0,3}", s[s.Length - 1]);
                }
            }
            w.Close();
            f.Close();
            StreamReader r = new StreamReader("temp2.txt");
            MessageBox.Show(r.ReadToEnd());
            r.Close();
            File.Delete("temp2.txt");
            File.Delete("temp.txt");
        }
    }
    }

