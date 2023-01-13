using Connect4.Classes;


namespace Connect4
{
    public partial class Connect4 : Form
    {

        Button[] buttons;
        PictureBox[,] tablePictures;
        GameLogic logic;
        int player = 0;
        ContextConnect4 context;


        public Connect4()
        {
            InitializeComponent();



        }


        private void btn_MouseEnter(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b == null)
                return;
            if (player == 1)
                b.ImageIndex = 0;
            else
                b.ImageIndex = 1;
        }
        private void btn_MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if(button == null)
                return ;
            button.ImageIndex = -1;
        }
        private void NewGame(object sender, EventArgs e)
        {
            
            Button btn = sender as Button;
            logic = new GameLogic();
            if (btn == null)
                return;
            if (btn.Name.Contains("Red"))
            {
                player = 1;
            }
            else
            {
                player = 2;
            }
            buttons = new Button[7];
            if (tablePictures == null)
                tablePictures = new PictureBox[6, 7];
            for(int i=0;i<7; i++)
            {
                Button b = new Button()
                {
                    Text = "",
                    Tag = i,
                    ImageList = imageList1,
                    BackColor = this.BackColor,
                    FlatStyle = FlatStyle.Flat,
                    Location = new System.Drawing.Point(0, 0),
                    Size = new System.Drawing.Size(70, 70),
                    Margin = new Padding(0),
                    Name = "buttonPlay" + i.ToString(),
                    ImageAlign = ContentAlignment.MiddleCenter,
                    TabIndex = 0
                    
                };
                b.Click += b_Click;
                b.MouseEnter += btn_MouseEnter;
                b.MouseLeave += btn_MouseLeave;
                tlpButtons.Controls.Add(b);
                buttons[i] = b;
            }
            //koristim ovo da se slike prikazu normlano, ali ne radi
            SuspendLayout();
            for(int i=0;i<6;i++)
            {
                for(int j=0;j<7;j++)
                {
                    if (tablePictures[i, j] != null)
                    {
                        tablePictures[i, j].Image.Dispose();
                        tablePictures[i, j].Image = global::Connect4.Properties.Resources.tableBackground;
                        tlpBoard.Controls.Add(tablePictures[i, j], j, i);
                    }
                    else
                    {
                        PictureBox p = new PictureBox()
                        {
                            Image = global::Connect4.Properties.Resources.tableBackground,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Location = new System.Drawing.Point(0, 0),
                            Size = new System.Drawing.Size(70, 70),
                            Margin = new System.Windows.Forms.Padding(0, 0, 0, 0)
                        };
                        tablePictures[i, j] = p;
                        tlpBoard.Controls.Add(p, j, i);
                    }
                    
                }
            }
            ResumeLayout(false);
            btnRed.Visible = false;
            btnYellow.Visible = false;
            tlpBoard.Visible = true;
            tlpButtons.Visible = true;

            context = new ContextConnect4(new Board(), 1);
            if(player != 1)
            {
                try
                {
                    this.Enabled = false;
                    if (context.CurrentState.nbMoves() == Board.WIDTH * Board.HEIGHT)
                    {
                        MessageBox.Show("Nereseno");
                        return;
                    }
                    Move p = logic.ReturnMove(context, 8);
                    DoMove(p.y);
                }
                finally
                {
                    this.Enabled = true;
                }
            }
        }

        void b_Click(object sender, EventArgs e)
        {
            Button b =sender as Button;
            if (b == null)
                return;
            int x = (int)b.Tag;

            if (context.CurrentState.canPlay(x))
            {
                DoMove(x);
                if (buttons == null)
                    return;
                if (context.CurrentPlayer != player)
                {
                    try
                    {
                        this.Enabled = false;
                        if (context.CurrentState.nbMoves() == Board.WIDTH * Board.HEIGHT)
                        {
                            MessageBox.Show("Nereseno");
                            return;
                        }
                        Move p = logic.ReturnMove(context, 8);
                        DoMove(p.y);
                    }
                    finally
                    {
                        this.Enabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Nije moguce odigrati ovaj potez");
            }
            
            

        }

        private void DoMove(int y)
        {
            string info;
            int player = 1 + (Convert.ToInt32(context.CurrentState.nbMoves()) % 2);
            if (context.CurrentState.isWinningMove(y))
            {
                context.CurrentState.play(y,context.CurrentPlayer);
                RefreshTable(y,player);
                context.Next();
                if (player == 1)
                {
                    MessageBox.Show("Pobedio Crveni");
                }
                else
                    MessageBox.Show("Pobedio Zuti");
                tlpBoard.Visible = false;
                tlpButtons.Visible = false;
                tlpButtons.Controls.Clear();
                tlpBoard.Controls.Clear();
                btnRed.Visible = true;
                btnYellow.Visible = true;
                buttons = null;
                return;
            }
            context.CurrentState.play(y, context.CurrentPlayer);
            RefreshTable(y,player);
            context.Next();
        }
        private void RefreshTable(int y, int player)
        {
            int x = 6-context.CurrentState.Top(y);
            if(player != 0)
            {
                tablePictures[x,y].Image.Dispose();
                if(player == 1)
                    tablePictures[x, y].Image = global::Connect4.Properties.Resources.red_circle_inTable;
                else
                    tablePictures[x, y].Image = global::Connect4.Properties.Resources.yellow_circle_inTable;
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            tlpBoard.Visible= false;
            tlpButtons.Visible= false;
            tlpButtons.Controls.Clear();
            tlpBoard.Controls.Clear();
            btnRed.Visible = true;
            btnYellow.Visible = true;
            buttons = null;

        }
    }
}