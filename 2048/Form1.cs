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
            while (mem[i, j] == 4);
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

        private void Forma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R) NewGame();
        }
    }
}
