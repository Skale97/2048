using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace _2048
{
    public partial class Forma : System.Windows.Forms.Form
    {
        Label[,] polje = new Label[4, 4];
        int[,] mem = new int[4, 4];

        public Forma()
        {
            InitializeComponent();
            for (int i = 0; i < 16; i++) mem[i / 4, i % 4] = 0;
        }

        private void Forma_Load(object sender, EventArgs e)
        {
            int veličinaPolja = 80;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    polje[i, j] = new Label();
                    polje[i, j].Text = "";
                    polje[i, j].Location = new Point(i * veličinaPolja, j * veličinaPolja);
                    polje[i, j].Size = new Size(veličinaPolja, veličinaPolja);
                    polje[i, j].BorderStyle = BorderStyle.FixedSingle;
                    polje[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    polje[i, j].Font = new Font("Arial", 24);

                    this.Controls.Add(polje[i, j]);
                }
            NewGame();
        }

        void NewGame()
        {
            for (int k = 0; k < 16; k++) mem[k / 4, k % 4] = 0;

            Random r = new Random();
            mem[r.Next(0, 4), r.Next(0, 4)] += 2;
            int i = r.Next(0, 4);
            int j = r.Next(0, 4);
            do
            {
                i = r.Next(0, 4);
                j = r.Next(0, 4);
                mem[i, j] += 2;
            }
            while (mem[i, j] >= 4);
            UpdateScreen();
        }

        void UpdateScreen()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (mem[i, j] != 0) polje[i, j].Text = mem[i, j].ToString();
                    else polje[i, j].Text = "";
                }
        }

        void move(int x1, int y1, int x2, int y2)
        {
            int i = 0;
            int j = 0;
            int x = (x1 - x2) / 3;
            int y = (y1 - y2) / 3;
            while (i < 4 && j < 4)
            {
                while (i < 4 && mem[x2 + i * x, y2 + i * y] == 0)
                    i++;
                if (i < 4 && j < 4 && j>0 && mem[x2 + (j - 1) * x, y2 + (j - 1) * y] == mem[x2 + i * x, y2 + i * y] )
                {
                    mem[x2 + (j - 1) * x, y2 + (j - 1) * y] += mem[x2 + i * x, y2 + i * y];
                    i++;
                }
                else if(i<4 && j<4)
                {
                    mem[x2 + j * x, y2 + j * y] = mem[x2 + i * x, y2 + i * y];
                    j++;
                    i++;
                }
            }
            for (; j < 4; j++) mem[x2 + j * x, y2 + j * y] = 0;
        }

        void moveUp()
        {
            for (int i = 0; i < 4; i++)
                move(i, 3, i, 0);
            randomAdd();
            UpdateScreen();
        }

        void moveDown()
        {
            for (int i = 0; i < 4; i++)
                move(i, 0, i, 3);
            randomAdd();
            UpdateScreen();
        }

        void moveLeft()
        {
            for (int i = 0; i < 4; i++)
                move(3, i, 0, i);
            randomAdd();
            UpdateScreen();
        }

        void moveRight()
        {
            for (int i = 0; i < 4; i++)
                move(0, i, 3, i);
            randomAdd();
            UpdateScreen();
        }

        void randomAdd()
        {
            Random r = new Random();
            bool b = true;
            while (b)
            {
                int i = r.Next(0, 4);
                int j = r.Next(0, 4);
                if (mem[i, j] == 0)
                {
                    mem[i, j] = 2;
                    b = false;  
                }
            }
        }

        private void Forma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R) NewGame();
            else if (e.KeyCode == Keys.Up) moveUp();
            else if (e.KeyCode == Keys.Left) moveLeft();
            else if (e.KeyCode == Keys.Right) moveRight();
            else if (e.KeyCode == Keys.Down) moveDown();
        }
    }
}
