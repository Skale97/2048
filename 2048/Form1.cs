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
        Label scoreLab = new Label();
        Label scoreVal = new Label();
        Random r = new Random();
        int[,] mem = new int[4, 4];
        int score = 0;

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
                    polje[i, j].Location = new Point(i * veličinaPolja, j * veličinaPolja + 40);
                    polje[i, j].Size = new Size(veličinaPolja, veličinaPolja);
                    polje[i, j].BorderStyle = BorderStyle.FixedSingle;
                    polje[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    polje[i, j].Font = new Font("Arial", 24);

                    this.Controls.Add(polje[i, j]);
                }
            scoreLab.Text = "SCORE";
            scoreLab.Location = new Point(0, 0);
            scoreLab.Size = new Size(100, 40);
            scoreLab.TextAlign = ContentAlignment.MiddleCenter;
            scoreLab.Font = new Font("Arial", 18);
            this.Controls.Add(scoreLab);

            scoreVal.Text = "0";
            scoreVal.Location = new Point(100, 0);
            scoreVal.Size = new Size(300, 40);
            scoreVal.TextAlign = ContentAlignment.MiddleCenter;
            scoreVal.Font = new Font("Arial", 24);
            this.Controls.Add(scoreVal);

            NewGame();
        }

        void NewGame()
        {
            for (int k = 0; k < 16; k++) mem[k / 4, k % 4] = 0;
            
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
            score = 0;
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
            scoreVal.Text = score.ToString();
        }

        void move(int x1, int y1, int x2, int y2)
        {
            bool b = false;
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
                    score += mem[x2 + (j - 1) * x, y2 + (j - 1) * y];
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
            bool b = true;
            while (b)
            {
                int i = r.Next(0, 4);
                int j = r.Next(0, 4);
                if (mem[i, j] == 0)
                {
                    mem[i, j] = 2;
                    while (r.Next(0, 4) == i && r.Next(0, 4) == j)
                        mem[i, j] += mem[i, j];
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
