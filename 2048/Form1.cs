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
        Label gameOverLab = new Label();
        Random r = new Random();
        int[,] mem = new int[4, 4];
        int score = 0;
        bool game_over = false;
        
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


            gameOverLab.Text = "GAME\nOVER";
            gameOverLab.Location = new Point(0, 40);
            gameOverLab.Size = new Size(320, 320);
            gameOverLab.TextAlign = ContentAlignment.MiddleCenter;
            gameOverLab.Font = new Font("Arial", 60);
            gameOverLab.BackColor = Color.Transparent;
            gameOverLab.Visible = false;
            this.Controls.Add(gameOverLab);

            NewGame(true);
        }

        void NewGame(bool ai)
        {
            for (int k = 0; k < 16; k++) mem[k / 4, k % 4] = 0;
            
            mem[r.Next(4), r.Next(4)] = 2;
            int i = r.Next(4);
            int j = r.Next(4);
            while(mem[i, j] != 0)
            {
                i = r.Next(4);
                j = r.Next(4);
            }
            mem[i, j] = (r.Next(10) < 9) ?  2 : 4;
            score = 0;

            if (ai)
            {
                foreach (Label p in polje)
                    p.Visible = true;

                UpdateScreen();
            }

            gameOverLab.Visible = false;
            game_over = false;
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

        bool move(int x1, int y1, int x2, int y2)
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
                    b = true;
                }
                else if(i<4 && j<4)
                {
                    mem[x2 + j * x, y2 + j * y] = mem[x2 + i * x, y2 + i * y];
                    j++;
                    i++;
                    if (j != i) b = true;
                }
            }
            for (; j < 4; j++) mem[x2 + j * x, y2 + j * y] = 0;
            return b;
        }

        void moveUp()
        {
            bool b = false;
            for (int i = 0; i < 4; i++)
                if (move(i, 3, i, 0))
                    b = true;
            if (b)
            {
                randomAdd();
                UpdateScreen();
            }
            else gameOver('v');
        }

        void moveDown()
        {
            bool b = false;
            for (int i = 0; i < 4; i++)
                if (move(i, 0, i, 3))
                    b = true;
            if (b)
            {
                randomAdd();
                UpdateScreen();
            }
            else gameOver('v');
        }

        void moveLeft()
        {
            bool b = false;
            for (int i = 0; i < 4; i++)
                if (move(3, i, 0, i))
                    b = true;
            if (b)
            {
                randomAdd();
                UpdateScreen();
            }
            else gameOver('h');
        }

        void moveRight()
        {
            bool b = false;
            for (int i = 0; i < 4; i++)
                if (move(0, i, 3, i))
                    b = true;
            if (b)
            {
                randomAdd();
                UpdateScreen();
            }
            else gameOver('h');
        }

        void randomAdd()
        {
            bool b = true;
            while (b)
            {
                int i = r.Next(4);
                int j = r.Next(4);
                if (mem[i, j] == 0)
                {
                    mem[i, j] = 2;
                    while (r.Next(4) == i && r.Next(4) == j)
                        mem[i, j] += mem[i, j];
                    b = false;  
                }
            }
        }

        void sequence(int[] seq)
        {
            foreach(int i in seq)
            {
                if (!game_over)
                {
                    if (i == 0) moveUp();
                    else if (i == 1) moveDown();
                    else if (i == 2) moveLeft();
                    else if (i == 3) moveRight();
                }
            }
        }

        void AI()
        {
            int n = 120;
            int ngen = 1000;
            Label progress = new Label();
            int[][] seq = new int[n][];
            int[] score = new int[n];
            int[] order = new int[n];
            string[] lines = new string[ngen];
            int lastscore = 1;
            int blscore = 2;
            int[] bestscore = new int[ngen];

            scoreLab.Visible = false;
            gameOverLab.Visible = false;
            for (int i = 0; i < 16; i++) polje[i / 4, i % 4].Visible = false;

            progress.Text = "START";
            progress.Location = new Point(0, 0);
            progress.Size = new Size(100, 40);
            progress.TextAlign = ContentAlignment.MiddleCenter;
            progress.Font = new Font("Arial", 18);
            this.Controls.Add(progress);

            for (int i = 0; i < n; i++) order[i] = i; //init order array which is used for sorting seq by score
            for (int i = 0; i < n; i++) //init seq
            {
                seq[i] = new int[r.Next(1, 100)];
                for (int j = 0; j < seq[i].Length; j++)
                    seq[i][j] = r.Next(4);
            }
            for (int generation = 0; generation < ngen; generation++)
            {
                for (int i = (generation == 0) ? 0 : 20; i < n; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        lastscore = 1;
                        blscore = 2;
                        while (!game_over && lastscore != this.score && blscore != lastscore) //checks if game is over and if it got stuck
                        {
                            blscore = lastscore;
                            lastscore = this.score;
                            sequence(seq[i]);
                        }
                        score[i] += this.score;
                        NewGame(false);
                    }
                    score[i] /= 10;
                    progress.Text = (generation * 100 + i).ToString();
                    scoreVal.Text = score[i].ToString();
                    gameOverLab.Visible = false;
                    this.Refresh();
                }
                Quicksort(ref score, ref order, 0, n - 1); //sorts score an saves order

                bestscore[generation] = score[0];

                reorder(ref seq, order);

                for (int i = 16; i < 20; i++) //init seq
                {
                    seq[i] = new int[r.Next(1, 100)];
                    for (int j = 0; j < seq[i].Length; j++)
                        seq[i][j] = r.Next(4);
                }

                mutation(ref seq);
            }

            for (int k = 0; k < ngen; k++) lines[k] = bestscore[k].ToString();
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\GitHub\2048\bestScores.txt"))
                foreach (string line in lines)
                    file.WriteLine(line);

            for (int i = 0; i < n; i++) //init seq
            {
                for (int j = 0; j < seq[i].Length; j++)
                    lines[i] += seq[i][j].ToString() + ", ";
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\GitHub\2048\seq.txt"))
                foreach (string line in lines)
                    file.WriteLine(line);

            scoreLab.Visible = true;
        }

        void reorder(ref int[][] A, int[] order)
        {
            int[][] tempA = new int[A.Length][]; //sorts seq by order
            tempA = A;
            for (int i = 0; i < A.Length; i++)
                A[i] = tempA[order[i]];
        }

        void gameOver(char orientation)
        {
            game_over = true;

            for (int i = 0; i < 16; i++)
                if (mem[i / 4, i % 4] == 0)
                {
                    game_over = false;
                    break;
                }

            if (game_over)
            {
                if (orientation == 'v')
                {
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 3; j++)
                            if (mem[j, i] == mem[j+1, i])
                            {
                                game_over = false;
                                break;
                            }
                }
                else if (orientation == 'h')
                {

                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 3; j++)
                            if (mem[i, j] == mem[i, j+1])
                            {
                                game_over = false;
                                break;
                            }
                }
            }
            if (game_over)
            {
                foreach(Label p in polje)
                {
                    p.Visible = false;
                }
                gameOverLab.Visible = true;
            }
        }

        void mutation(ref int[][] s)
        {
            for (int i = 20; i < 120; i+=5)
            {
                int length = r.Next(s[i].Length - 1);
                deletion(ref s[i], r.Next(s[i].Length - length), length);

                length = r.Next(s[i + 1].Length - 1);
                duplication(ref s[i + 1], r.Next(s[i + 1].Length - length), length);

                length = r.Next(s[i + 2].Length - 1);
                inversion(ref s[i + 2], r.Next(s[i + 2].Length - length), length);

                int rseq = r.Next(20);
                length = r.Next(s[rseq].Length - 1);
                insertion(ref s[i + 3], ref s[rseq], r.Next(s[i + 3].Length), r.Next(s[rseq].Length - length), length);

                length = r.Next(s[i + 4].Length - 1);
                rseq = r.Next(20);
                int length2 = r.Next(s[rseq].Length - 1);
                transposition(ref s[i + 4], ref s[rseq], r.Next(s[i + 4].Length - length), r.Next(s[rseq].Length - length2), length2, length);
            }
        }

        void deletion(ref int[] sequence, int position, int length)
        {
            int[] newSequence = new int [sequence.Length - length];

            for (int i = 0; i < newSequence.Length; i++)
                newSequence[i] = (i < position) ? sequence[i] : sequence[i + length];

            sequence = new int[newSequence.Length];
            sequence = newSequence;
        }

        void duplication(ref int[] sequence, int position, int length)
        {
            int[] newSequence = new int[sequence.Length + length];

            for (int i = 0; i < newSequence.Length; i++)
                newSequence[i] = (i < position + length) ? sequence[i] : sequence[i - length];

            sequence = new int[newSequence.Length];
            sequence = newSequence;
        }

        void inversion(ref int[] sequence, int position, int length)
        {
            int[] newSequence = new int[sequence.Length];

            for (int i = 0; i < newSequence.Length; i++)
                newSequence[i] = (i < position || i>=position + length) ? sequence[i] : sequence[position + length - (i - position + 1)];

            sequence = new int[newSequence.Length];
            sequence = newSequence;
        }
        
        void insertion(ref int[] sequence1, ref int[] sequence2, int position1, int position2, int length)
        {
            int[] newSequence1 = new int[sequence1.Length + length];
            int[] newSequence2 = new int[sequence2.Length - length];

            for (int i = 0; i < newSequence1.Length; i++)
                if (i < position1) newSequence1[i] = sequence1[i];
                else if (i >= position1 && i < position1 + length) newSequence1[i] = sequence2[position2 + i - position1];
                else newSequence1[i] = sequence1[i - length];

            for (int i = 0; i < newSequence2.Length; i++)
                newSequence2[i] = (i < position2) ? sequence2[i] : sequence2[i + length];

            sequence1 = new int[newSequence1.Length];
            sequence1 = newSequence1;

            sequence2 = new int[newSequence2.Length];
            sequence2 = newSequence2;
        }

        void transposition(ref int[] sequence1, ref int[] sequence2, int position1, int position2, int length1, int length2)
        {
            int[] newSequence1 = new int[sequence1.Length + length1 - length2];
            int[] newSequence2 = new int[sequence2.Length + length2 - length1];

            for (int i = 0; i < newSequence1.Length; i++)
                if (i < position1) newSequence1[i] = sequence1[i];
                else if (i >= position1 && i < position1 + length1) newSequence1[i] = sequence2[position2 + i - position1];
                else newSequence1[i] = sequence1[i - length1 + length2];

            for (int i = 0; i < newSequence2.Length; i++)
                if (i < position2) newSequence2[i] = sequence2[i];
                else if (i >= position2 && i < position2 + length2) newSequence2[i] = sequence1[position1 + i - position2];
                else newSequence2[i] = sequence2[i - length2 + length1];

            sequence1 = new int[newSequence1.Length];
            sequence1 = newSequence1;

            sequence2 = new int[newSequence2.Length];
            sequence2 = newSequence2;
        }

        void Quicksort(ref int[] A, ref int[] o, int start, int end)
        {
            if(start < end)
            {
                int partiotionIndex = start;
                int pivot = A[end];

                for (int i = start; i < end; i++)
                    if (A[i] >= pivot)
                    {
                        int a = A[i];
                        A[i] = A[partiotionIndex];
                        A[partiotionIndex] = a;
                        a = o[i];
                        o[i] = o[partiotionIndex];
                        o[partiotionIndex] = a;

                        partiotionIndex++;
                    }
                int b = A[end];
                A[end] = A[partiotionIndex];
                A[partiotionIndex] = b;
                b = o[end];
                o[end] = o[partiotionIndex];
                o[partiotionIndex] = b;

                Quicksort(ref A, ref o, start, partiotionIndex - 1);
                Quicksort(ref A, ref o, partiotionIndex + 1, end);
            }
        }

        private void Forma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R) NewGame(true);
            if (e.KeyCode == Keys.A) AI();
            if (!game_over)
            { 
                if (e.KeyCode == Keys.Up) moveUp();
                else if (e.KeyCode == Keys.Left) moveLeft();
                else if (e.KeyCode == Keys.Right) moveRight();
                else if (e.KeyCode == Keys.Down) moveDown();
            }
        }
    }
}
