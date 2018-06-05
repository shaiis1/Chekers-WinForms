using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersGameLogic;
using CheckersGameLogic.CheckersEnumUtils;

namespace CheckersWindowUI
{
    public partial class CheckersForm : Form
    {
        private readonly ButtonProxy[,] r_GameBoard;
        private readonly int r_BoardSize;
        private readonly eGameStyle r_GameStyle;
        private readonly PlayerDetails r_FirstPlayer;
        private readonly PlayerDetails r_SecondPlayer;
        private ButtonProxy m_SelectedButton;
        private ButtonProxy m_MoveToButton;
        private bool m_FirstPlayerMove = true;
        private bool m_IsItTheSamePlayerMove = false;
        private CheckersGameLogic.CheckersLogic m_Game;
        private ComputerMovesGenerator m_ComputerMovesGenerator;
        private Label m_FirstPlayerScoreLabel;
        private Label m_SecondPlayerScoreLabel;

        public CheckersForm(
            int i_BoardSize,
            PlayerDetails i_FirtsPlayer, 
            PlayerDetails i_SecondPlayer,
            eGameStyle i_GameStyle)
        {
            r_GameStyle = i_GameStyle;
            r_BoardSize = i_BoardSize;
            r_FirstPlayer = i_FirtsPlayer;
            r_SecondPlayer = i_SecondPlayer;
            r_GameBoard = new ButtonProxy[r_BoardSize, r_BoardSize];
            m_Game = new CheckersGameLogic.CheckersLogic(i_FirtsPlayer, i_SecondPlayer, i_BoardSize, i_GameStyle);
            initializeComputerMoveGeneratorIfNeeded();
            initializeControls();
        }

        private void initializeControls()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Checkers";
            this.Size = new Size((r_BoardSize * 55) - (r_BoardSize / 2), (r_BoardSize * 55) - (r_BoardSize * 5));
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            m_FirstPlayerScoreLabel = new Label();
            m_SecondPlayerScoreLabel = new Label();
            m_FirstPlayerScoreLabel.Text = getNamesAndScores(r_FirstPlayer);
            m_FirstPlayerScoreLabel.Size = new Size(150, 20);
            m_FirstPlayerScoreLabel.TextAlign = ContentAlignment.MiddleCenter;
            m_FirstPlayerScoreLabel.Font = new Font("Arial", 11, FontStyle.Bold);
            m_FirstPlayerScoreLabel.Location = new Point((ClientSize.Width / 2) - m_FirstPlayerScoreLabel.Width, 20);
            m_FirstPlayerScoreLabel.BackColor = Color.Aqua;
            m_SecondPlayerScoreLabel.Text = getNamesAndScores(r_SecondPlayer);
            m_SecondPlayerScoreLabel.Size = new Size(150, 20);
            m_SecondPlayerScoreLabel.Font = new Font("Arial", 11, FontStyle.Bold);
            m_SecondPlayerScoreLabel.Location = new Point(m_FirstPlayerScoreLabel.Right + 2, 20);
            this.Controls.AddRange(new Control[] { m_FirstPlayerScoreLabel, m_SecondPlayerScoreLabel });
            setBoard();
        }

        private string getNamesAndScores(PlayerDetails i_Player)
        {
            return string.Format("{0} : {1}", i_Player.PlayerName, i_Player.AllGamesScore);
        }

        private void initializeComputerMoveGeneratorIfNeeded()
        {
            if (r_GameStyle.Equals(eGameStyle.VsComputer))
            {
                m_ComputerMovesGenerator = new ComputerMovesGenerator(r_BoardSize);
            }
        }

        private void setBoard()
        {
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    r_GameBoard[i, j] = new ButtonProxy(i, j);
                    r_GameBoard[i, j].BackColor = (i + j) % 2 == 0 ? Color.Gray : Color.White;
                    r_GameBoard[i, j].Location = new Point(
                        ((i + (r_BoardSize / 2)) * 30) - (this.Width / 10),
                        m_FirstPlayerScoreLabel.Bottom + (j * 30));
                    r_GameBoard[i, j].Size = new Size(30, 30);
                    if (r_GameBoard[i, j].BackColor == Color.Gray)
                    {
                        r_GameBoard[i, j].Enabled = false;
                    }

                    r_GameBoard[i, j].Click += pawn_Pressed;
                    this.Controls.Add(r_GameBoard[i, j]);
                }
            }

            setPawns();
        }

        private void setPawns()
        {
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    r_GameBoard[j, i].Text = string.Format("{0}", (char)m_Game.Board[i, j].Value);
                    r_GameBoard[j, i].Font = new Font("Arial", 12, FontStyle.Bold);
                }
            }
        }

        private void pawn_Pressed(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            m_SelectedButton = (ButtonProxy)sender;

            foreach (Button btn in r_GameBoard)
            {
                if (!btn.BackColor.Equals(Color.Gray))
                {
                    btn.BackColor = Color.White;
                }
            }

            currentButton.BackColor = Color.Aqua;
            foreach (Button button in r_GameBoard)
            {
                button.Click -= pawn_Pressed;
                if (!button.Equals(currentButton))
                {
                    button.Click += pawn_PressedAfterAnotherPawnPressed;
                }
                else
                {                  
                    button.Click += pawn_PressedAgain;
                }
            }
        }

        private void pawn_PressedAfterAnotherPawnPressed(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            m_MoveToButton = (ButtonProxy)sender;
            foreach (Button btn in r_GameBoard)
            {
                if (!btn.BackColor.Equals(Color.Gray))
                {
                    btn.BackColor = Color.White;
                }
            }

            currentButton.BackColor = Color.White;
            foreach (Button button in r_GameBoard)
            {
                button.Click += pawn_Pressed;
                if(button.Equals((Button)m_SelectedButton))
                {
                    button.Click -= pawn_PressedAgain;
                }
                else
                {
                    button.Click -= pawn_PressedAfterAnotherPawnPressed;
                }              
            }

            playerMove();
        }

        private void pawn_PressedAgain(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

            currentButton.BackColor = Color.White;
            foreach (Button button in r_GameBoard)
            {
                button.Click += pawn_Pressed;
                if(currentButton.Equals(button))
                {
                    button.Click -= pawn_PressedAgain;
                }
                else
                {
                    button.Click -= pawn_PressedAfterAnotherPawnPressed;
                }              
            }
        }

        private void playerMove()
        {
            int fromRow, fromCol, toRow, toCol;
            bool isItValidInput = true;

            if (m_Game.IsAnyMovesLeft())
            {
                fromRow = m_SelectedButton.ButtonColumn;
                fromCol = m_SelectedButton.ButtonRow;
                toRow = m_MoveToButton.ButtonColumn;
                toCol = m_MoveToButton.ButtonRow;
                isItValidInput = m_Game.PlayOneGame(
                    fromRow,
                    fromCol,
                    toRow,
                    toCol,
                    out m_IsItTheSamePlayerMove);
                setPawns();
            }

            if(!checkGameStatus())
            {
                handleScreen(isItValidInput);
            }
        }

        private bool checkGameStatus()
        {
            bool gameOver = false;

            if (m_Game.IsGameOver)
            {
                doWhenGameOver();
            }
            else
            {
                if(!m_Game.IsAnyMovesLeft())
                {
                    gameOver = true;
                    doWhenGameOver();
                }
            }

            return gameOver;
        }

        private void ComputerMove()
        {
            int fromRow, fromCol, toRow, toCol;
            bool isItValidInput;

            if (m_Game.IsAnyMovesLeft())
            {
                do
                {
                    m_ComputerMovesGenerator.GenerateMoves();
                    fromRow = m_ComputerMovesGenerator.FromCol;
                    fromCol = m_ComputerMovesGenerator.FromRow;
                    toRow = m_ComputerMovesGenerator.ToCol;
                    toCol = m_ComputerMovesGenerator.ToRow;
                    isItValidInput = m_Game.PlayOneGame(fromRow, fromCol, toRow, toCol, out m_IsItTheSamePlayerMove);
                }
                while (!isItValidInput || m_IsItTheSamePlayerMove);

                setPawns();
            }

            checkGameStatus();  
        }

        private void handleScreen(bool i_IsValidInput)
        {
            if (i_IsValidInput)
            {
                if (m_IsItTheSamePlayerMove)
                {
                    MessageBox.Show(string.Format("{0}{1}", Environment.NewLine, "Now jump again!"));
                }
                else
                {
                    m_FirstPlayerMove = !m_FirstPlayerMove;
                    if (r_GameStyle.Equals(eGameStyle.VsComputer))
                    {
                        ComputerMove();
                        m_FirstPlayerMove = !m_FirstPlayerMove;
                    }
                    else
                    {
                        decideWhicePlayerMove();
                    }
                }
            }
            else
            {
                if(m_IsItTheSamePlayerMove)
                {
                    MessageBox.Show("You have to eat!");
                }
                else
                {
                    MessageBox.Show(string.Format("{0}{1}", Environment.NewLine, "Please enter valid input!"));
                }               
            }
        }

        private void decideWhicePlayerMove()
        {
            if (m_FirstPlayerMove)
            {
                m_FirstPlayerScoreLabel.BackColor = Color.Aqua;
                m_SecondPlayerScoreLabel.BackColor = Color.White;
            }
            else
            {
                m_FirstPlayerScoreLabel.BackColor = Color.White;
                m_SecondPlayerScoreLabel.BackColor = Color.Aqua;
            }
        }

        private void doWhenGameOver()
        {
            printAndUpdateScores();
        }

        private void printAndUpdateScores()
        {
            PlayerDetails winner, loser;
            string gameOverMessage;

            if (m_Game.GetWinner(out winner, out loser))
            {
                gameOverMessage = getScoreMessage(winner, loser, winner.GameScore, loser.GameScore);
                m_Game.UpdateScoresForNewGame(winner, loser);
            }
            else
            {
                gameOverMessage = string.Format("Its a tie!");
            }

            DialogResult result = MessageBox.Show(gameOverMessage, "Another Round?", MessageBoxButtons.YesNo);
            if (result.Equals(DialogResult.Yes))
            {
                m_FirstPlayerScoreLabel.Text = getNamesAndScores(r_FirstPlayer);
                m_SecondPlayerScoreLabel.Text = getNamesAndScores(r_SecondPlayer);
                m_Game = new CheckersGameLogic.CheckersLogic(r_FirstPlayer, r_SecondPlayer, r_BoardSize, r_GameStyle);
                m_FirstPlayerMove = true;
                setPawns();
                decideWhicePlayerMove();
                m_FirstPlayerMove = !m_FirstPlayerMove;
            }
            else
            {
                MessageBox.Show("Thanks for playing!");
                this.Close();
            }
        }

        private string getScoreMessage(
            PlayerDetails i_Winner,
            PlayerDetails i_Loser,
            int i_WinnerScore,
            int i_LoserScore)
        {
            return string.Format(
                    "Winner is : {0} ({1}), Score : {2}.{3}Loser is : {4} ({5}), Score : {6}.",
                    i_Winner.PlayerName,
                    (char)i_Winner.PlayerSymbole,
                    i_WinnerScore,
                    Environment.NewLine,
                    i_Loser.PlayerName,
                    (char)i_Loser.PlayerSymbole,
                    i_LoserScore);
        }

        private void CheckersForm_Load(object sender, EventArgs e)
        {
        }
    }
}
