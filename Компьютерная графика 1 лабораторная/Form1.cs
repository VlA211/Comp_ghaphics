using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram_Graphic_Lab
{
    public partial class Form1 : Form
    {
        bool grid = true;
        int C;
        int wibthBorter;
        Pen pen;
        Graphics canvas;
        //List<int> points = new List<int>() { 10, 0, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 };// { 1, 30, 57567, 47645746};
        //List<int> points = new List<int>() { 1, 30, 57567, 47645746};
        //List<int> points = new List<int>() { 100, 100, 100 };
        List<int> points = new List<int>();
        int stepWibth;
        double stepPoint;
        const string Path = "C:\\Users\\sffsd\\Desktop\\Diagramm.txt";
        int sizePen = 5;
        int stepHeight;
        Color color = Color.Black;
        int Step;
        public Form1()
        {
            InitializeComponent();
        }

        private int RoundTo1e(int x)
        {
            for(int i = 1, j = 0; j < 1000; j++, i *= 10)
            {
                if(x < i)
                {
                    return i;
                }
            }
            return -1;
        }

        int ssssss = 0;
        int wwwwwww = 0;
        
        private void writFile(string Path)
        {
            using (StreamReader reader = new StreamReader(Path))
            {
                string line;
                points.Clear();

                while ((line = reader.ReadLine()) != null)
                {
                    if (Int32.Parse(line) < 0)
                    {
                        var result = MessageBox.Show("Пропустить эти данные?", "Нормально отображаются только положительные целочисленные данные", MessageBoxButtons.YesNo);
                        if (DialogResult.Yes == result)
                        {
                            continue;
                        }
                    }
                    points.Add(Int32.Parse(line));
                    //points.Add(line);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if(points.Count == 0)
            {
                writFile(Path);
            }
            Step = points.Count();


            Bitmap Img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            canvas = Graphics.FromImage(Img);
            pictureBox1.Image = Img;
            pictureBox1.BackColor = Color.White;

            wibthBorter = 30;
            stepPoint = Math.Max((points.Max<int>() / 10),1);
            if(stepPoint * 10 > 99) {
                wibthBorter = (1+stepPoint.ToString().Length) * 12;
            }
            sizePen = 1;
            pen = new Pen(color, sizePen);

            stepWibth = Math.Max(((int)(pictureBox1.Size.Width) - 2 * wibthBorter) / Step, 1);
            stepHeight = (pictureBox1.Height - wibthBorter * 2) / 10;

            int rate = sizePen / 2;
            canvas.DrawLine(pen, pictureBox1.Width - wibthBorter, pictureBox1.Height - wibthBorter - rate, wibthBorter, pictureBox1.Height - wibthBorter - rate);// горизонталь
            canvas.DrawLine(pen, wibthBorter - rate, wibthBorter, wibthBorter - rate, pictureBox1.Height - wibthBorter);//вертикаль

            for (int i = wibthBorter, count = 0; i <= pictureBox1.Size.Width - wibthBorter; i += stepWibth, count++)//горизонталь
            {
                if (grid)
                {
                    canvas.DrawLine(new Pen(Color.Gray, 1), i - rate, pictureBox1.Height - wibthBorter, i - rate, wibthBorter);// штрихи
                }
                canvas.DrawLine(pen, i - rate, pictureBox1.Height - wibthBorter - 3, i - rate, pictureBox1.Height - wibthBorter + 3);// штрихи

                Label ss = new Label();

                ss.Location = new Point(i - rate - 12, pictureBox1.Height - wibthBorter + 6); //значение у штрихов
                ss.Font = new Font(ss.Font.FontFamily, (10));//высота шрифта
                ss.Size = new Size(68, Math.Max(10, 2 * (int)ss.Font.Size));//длина поля для письма
                ss.Text = (count).ToString();//размета

                ss.BackColor = Color.White;
                Controls.Add(ss);
                ss.BringToFront();

                ssssss = i - rate - wibthBorter;

            }
 
            for (int i = pictureBox1.Size.Height - wibthBorter, count = 0; i > wibthBorter; i -= stepHeight, count++)//вертикаль
            {
                if (grid)
                {
                    canvas.DrawLine(new Pen(Color.Gray, 1), wibthBorter, i - rate, pictureBox1.Width - wibthBorter, i - rate);// штрихи
                }
                canvas.DrawLine(pen, wibthBorter - 3, i - rate,wibthBorter + 3, i - rate);

                Label ss = new Label();
                int b = RoundTo1e((int)stepPoint);//разметка
                string d = (stepPoint * 10).ToString();

                ss.Location = new Point(0, i - rate-10);
                ss.Font = new Font(ss.Font.FontFamily, 8);
                ss.Size = new Size(wibthBorter - 3, Math.Max(10, 2* (int)ss.Font.Size));
                ss.Text = (count * b).ToString();

                ss.BackColor = Color.White;
                Controls.Add(ss);
                ss.BringToFront();
                wwwwwww = i - rate;

            }

            MyPaint();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void MyPaint()
        {
            double c = (double)(pictureBox1.Size.Height - wwwwwww - wibthBorter) / (10 * RoundTo1e((int)stepPoint));//19.5//29//

            double w = (double)(ssssss) / (double)points.Count;
            List<Point> pp= new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                Point f = new Point((int)(wibthBorter + i * w), (int)(pictureBox1.Height - wibthBorter - points[i] * c));
                pp.Add(f);
                canvas.DrawEllipse(new Pen(color, sizePen * 2), f.X, f.Y, 2, 2);
            }
            canvas.DrawLines(pen, pp.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string line;
                        points.Clear();
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (Int32.Parse(line) < 0)
                            {
                                var result = MessageBox.Show("Пропустить эти данные?", "Нормально отображаются только положительные целочисленные данные", MessageBoxButtons.YesNo);
                                if (DialogResult.Yes == result)
                                {
                                    continue;
                                }
                            }
                            points.Add(Int32.Parse(line));
                            //points.Add(line);
                        }
                    }
                    Form1_Load(sender, e);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            grid = !grid;
            Form1_Load(sender, e);
        }
    }
}
