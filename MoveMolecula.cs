using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BrownianMotion
{
    public partial class movingForm : Form
    {
        int count;
        const int molWidth = 20, 
                  molHeight = 20;
        Molecula[] arrMoleculas;
        List<Thread> threads;

        public movingForm()
        {
            InitializeComponent();
        }

        public movingForm(int count)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            this.count = count;

            Random rnd = new Random();
            arrMoleculas = new Molecula[count];

            for (int i = 0; i < arrMoleculas.Length; i++)
            {
                arrMoleculas[i] = new Molecula(
                    new PictureBox(),
                    rnd.Next(1, Size.Width - 2 * molWidth),
                    rnd.Next(1, Size.Height - 5 * molHeight),
                    0,
                    trackBar1.Value
                );
                arrMoleculas[i].square.Width = molWidth;
                arrMoleculas[i].square.Height = molHeight;
                arrMoleculas[i].square.BackColor = Color.Black;
                arrMoleculas[i].square.Top = (int)arrMoleculas[i].y;
                arrMoleculas[i].square.Left = (int)arrMoleculas[i].x;
                this.Controls.Add(arrMoleculas[i].square);
            }

            InitializeThreads();
        }
        /// <summary>
        ///     Инициализация потоков
        /// </summary> 
        private void InitializeThreads()
        {
            threads = new List<Thread>();
            foreach (Molecula mol in arrMoleculas)
            {
                Thread thr = new Thread(MoveMolecula);
                threads.Add(thr);
                thr.IsBackground = true;
                thr.Start(mol);
            }
            Thread thrField= new Thread(RefreshField);
            thrField.IsBackground = true;
            thrField.Priority = ThreadPriority.AboveNormal;
            threads.Add(thrField);
            thrField.Start();
        }

        /// <summary>
        ///     Перерисовка формы
        /// </summary>
        private void RefreshField()
        {
            while (true)
            {
                Thread.Sleep(10);
                this.BeginInvoke((MethodInvoker)(() => { this.Refresh(); }));
            }
        }

        /// <summary>
        ///     Получение новых координат и угла направления
        /// </summary>
        /// <param name="mol"></param>
        private void MoveMolecula(object mol)
        {
            while (true)
            {
                GetCoords((Molecula)mol);
            }
        }

        /// <summary>
        ///     Метод, возвращающий новые координаты и угол
        /// </summary>
        /// <param name="mol"></param>
        private void GetCoords(Molecula mol)
        {
            Thread.Sleep(mol.speed);
            mol.x = mol.square.Left;
            mol.y = mol.square.Top;
            mol.x += mol.speed * Math.Sin(mol.angle * Math.PI / 180);
            mol.y += mol.speed * Math.Cos(mol.angle * Math.PI / 180);

            if (mol.y < 0)
            {
                mol.y = 0;
                mol.angle = 180 - mol.angle;
            }

            if (mol.x < 0)
            {
                mol.x = 0;
                mol.angle *= -1;
            }

            if (mol.x > Size.Width - 2 * molWidth)
            {
                mol.x = Size.Width - 2 * molWidth;
                mol.angle *= -1;
            }

            if (mol.y > Size.Height - 5 * molHeight)
            {
                mol.y = Size.Height - 5 * molHeight;
                mol.angle = 180 - mol.angle;
            }
        }

        /// <summary>
        ///     Отрисовка молекул
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Draw_Molecula(object sender, EventArgs e)
        {
            foreach (Molecula mol in arrMoleculas)
            {
                mol.square.Top = (int)mol.y;
                mol.square.Left = (int)mol.x;
                mol.speed = trackBar1.Value;
            }
        }

        /// <summary>
        ///     Смена направления и скорости его смены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Angle(object sender, EventArgs e)
        {
            Random arg = new Random();
            foreach(Molecula mol in arrMoleculas)
            {
                mol.angle = arg.Next(0, 360);
            }
            timer.Interval = Math.Abs(trackBar1.Value - trackBar1.Maximum) * 10 + 200;
        }

        /// <summary>
        ///     Завершение потоков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            foreach (Thread thr in threads)
            {
                thr.Abort();
            }
        }
    }
}
