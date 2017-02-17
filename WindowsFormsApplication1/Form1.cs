﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Graphics cizim;
        Gemi karsiGemi;
        Gemi bizimGemi;
        Graphics g;
        bool veriOnayla = false;
        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            cizim = CreateGraphics();

        }
        static public void yaz()
        {

        }
        static int katSayi = 10;
        static List<Gemi> gemiler = new List<Gemi>();

        private void Form1_DoubleClick(object sender, EventArgs e)
        {

            Point merkez = new Point(Control.MousePosition.X - this.Location.X, Control.MousePosition.Y - this.Location.Y);
            //      Console.WriteLine((Control.MousePosition.X - this.Location.X) + ";" + (Control.MousePosition.Y - this.Location.Y )+ "");
            Pen pen = new Pen(Color.Black, 3);

            cizim.DrawLine(pen, merkez.X, merkez.Y - 30, merkez.X + 10, merkez.Y - 10);
            cizim.DrawLine(pen, merkez.X + 10, merkez.Y - 30, merkez.X, merkez.Y - 10);
            Form2 form2 = new Form2();
            form2.Show();

        }
        public static void setVeriler(int guvenli_alan, int hiz, int yon, Point merkez)
        {
            gemiler.Add(new Gemi(guvenli_alan * katSayi, hiz, yon, merkez));
        }
      

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            veriOnayla = true;

            bizimGemi = gemiler.ElementAt(0);
            karsiGemi = gemiler.ElementAt(1);
            /*tcpaHesapla(karsiGemi);
            dcpaHesapla(karsiGemi);*/


            for (int i = 0; i < gemiler.Count; i++)
            {
                gemiCiz(gemiler.ElementAt(i));
                if (i > 0)//tcp,dcp Hesaplancak
                {
                    gemiler.ElementAt(i).tcpa = Gemi.tcpaHesapla(gemiler.ElementAt(0),gemiler.ElementAt(i));
                    gemiler.ElementAt(i).dcpa = Gemi.dcpaHesapla(gemiler.ElementAt(0),gemiler.ElementAt(i));
                }
            }

            durumKontrolu(gemiler.ElementAt(0), gemiler.ElementAt(1));

           /* gemiKonumlandir(bizimGemi);
            gemiKonumlandir(karsiGemi);*/           
        }

    
        
        public void gemiKonumlandir(Gemi _gemi)
        {

            int w = this.Width;
            int h = this.Height;
            Graphics gg = this.CreateGraphics();
            Pen pen = new Pen(Color.Blue, 2);

            _gemi.merkez.X = w / 2;
            _gemi.merkez.Y = h / 2;
            gg.DrawArc(pen, _gemi.merkez.X, _gemi.merkez.Y, 10, 10, 0, 360);
        }

        public String durumKontrolu(Gemi _bizimGemi, Gemi _karsiGemi)
        {
            String s = "";
            double rota = _karsiGemi.yon - _bizimGemi.yon;
            if ((rota >= 355 && rota <= 360) || (rota >= 0 && rota < 5))
            {
                s = "Head-on";
            }
            else if (rota >= 247.5 && rota < 355)
            {
                s = "Crossing Stand-on";
            }
            else if (rota >= 112.5 && rota < 247.5)
            {
                s = "Overtaking";
            }
            else if (rota >= 5 && rota < 112.5)
            {
                s = "Crossing Gige-way";
            }
            return s;
        }
        public Gemi gemiHareketEttir(Gemi gemi)
        {
            gemi.merkez.X += Convert.ToInt32(gemi.hiz
                   * Math.Cos((gemi.yon + 90) * Math.PI / 180));
            gemi.merkez.Y += Convert.ToInt32(gemi.hiz
                * -Math.Sin((gemi.yon + 90) * Math.PI / 180));
            return gemi;
        }

        private bool carpistiMi(Gemi _bizimGemi, Gemi _karsiGemi)
        {
           
            if (Math.Pow((_karsiGemi.merkez.X - _bizimGemi.merkez.X), 2) +
                Math.Pow((_karsiGemi.merkez.Y - _bizimGemi.merkez.Y), 2) <= Math.Pow(_bizimGemi.guvenli_alan, 2))
            {
                MessageBox.Show("Çarpıştı");
                return true;
            }
            return false;
        }

        Point cizimKonumu=new Point();
        private void gemiCiz(Gemi gemi)
        {
            cizimKonumu.X = this.Width / 2;
            cizimKonumu.Y = this.Height / 2;
                

            int r = 500;
            int x = gemi.merkez.X + Convert.ToInt32(r * Math.Cos((gemi.yon + 90) * Math.PI / 180));
            int y = gemi.merkez.Y + Convert.ToInt32(r * -Math.Sin((gemi.yon + 90) * Math.PI / 180));
            Console.WriteLine(Math.Sin(gemi.yon * Math.PI / 180) + "");
            Point hedef = new Point(x, y);

            Pen cevre = new Pen(Color.Red, 1);
            Pen yol = new Pen(Color.Blue, 1);

            Point rotaKonum = new Point();
            rotaKonum.X = gemi.merkez.X + cizimKonumu.X;
            rotaKonum.Y = gemi.merkez.Y + cizimKonumu.Y;

            Point rotaHedef=new Point();
            rotaHedef.X = hedef.X + cizimKonumu.X;
            rotaHedef.Y = hedef.Y + cizimKonumu.Y;

            g.DrawEllipse(cevre,cizimKonumu.X+gemi.merkez.X - gemi.guvenli_alan / 2,
                cizimKonumu.Y+gemi.merkez.Y - gemi.guvenli_alan / 2, gemi.guvenli_alan, gemi.guvenli_alan);
            g.DrawLine(yol, rotaKonum, rotaHedef);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            karsiGemi = gemiHareketEttir(karsiGemi);
            bizimGemi = gemiHareketEttir(bizimGemi);

            g.Clear(this.BackColor);
            gemiCiz(bizimGemi);
            gemiCiz(karsiGemi);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (veriOnayla)
            {
                timer1.Interval = 100;
                timer1.Enabled = !timer1.Enabled;

                if (timer1.Enabled == true)
                    button3.Text = "Durdur";
                else
                    button3.Text = "Devam";
                for (int i = 0; i < gemiler.Count; i++)
                {
                    gemiCiz(gemiler.ElementAt(i));
                }
            }
            else
            {
                MessageBox.Show("Oncelikle Verileri Onaylayin");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
         
        }

    }
}
