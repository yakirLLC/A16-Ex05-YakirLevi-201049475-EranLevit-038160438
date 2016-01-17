using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using A16_Ex05_CheckersLogic;

namespace A16_Ex05_CheckersForm
{
    public class BoardForm : Form
    {
        private bool m_Clicked = false;
        private Label m_LabelPlayer1 = new Label();
        private Label m_LabelPlayer2 = new Label();
        private Label m_LabelPlayer1Score = new Label();
        private Label m_LabelPlayer2Score = new Label();
        private CheckersButton m_SourceButton;
        private CheckersButton[,] m_Buttons;
        private Board m_Board;
        private Gameplay m_Gameplay = new Gameplay();
        private StartForm m_GameProperties;

        public BoardForm(StartForm i_GameProperties)
        {
            m_GameProperties = i_GameProperties;
            this.Text = "Damka";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            if (m_GameProperties.BoardSize == 6)
            {
                this.Size = new Size(528, 580);
            }
            else if (m_GameProperties.BoardSize == 8)
            {
                this.Size = new Size(686, 746);
            }
            else
            {
                this.Size = new Size(848, 900);
            }

            InitControls();
        }

        private void InitControls()
        {
            int j, buttonTop = 50, buttonLeft = 20, currentButtonLeft = buttonLeft;

            m_LabelPlayer1.Text = m_GameProperties.Player1.Name + ":";
            m_LabelPlayer1.AutoSize = true;
            m_LabelPlayer1.Top = 8;
            m_LabelPlayer1.Left = (this.ClientSize.Width / 4) - (m_LabelPlayer1.Width / 2);
            m_LabelPlayer1.ForeColor = Color.Green;
            m_LabelPlayer1.Font = new Font(m_LabelPlayer1.Font.Name, 12, FontStyle.Bold);

            m_LabelPlayer1Score.Text = m_GameProperties.Player1.Score.ToString();
            m_LabelPlayer1Score.AutoSize = true;
            m_LabelPlayer1Score.Top = 8;
            m_LabelPlayer1Score.Left = m_LabelPlayer1.Right;
            m_LabelPlayer1Score.ForeColor = Color.Blue;
            m_LabelPlayer1Score.Font = new Font(m_LabelPlayer1Score.Font.Name, 12, FontStyle.Bold);

            m_LabelPlayer2.Text = m_GameProperties.Player2.Name + ":";
            m_LabelPlayer2.AutoSize = true;
            m_LabelPlayer2.Top = 8;
            m_LabelPlayer2.Left = (this.ClientSize.Width / 2) + (this.ClientSize.Width / 4) - (m_LabelPlayer2.Width / 2);
            m_LabelPlayer2.ForeColor = Color.Blue;
            m_LabelPlayer2.Font = new Font(m_LabelPlayer2.Font.Name, 12, FontStyle.Bold);

            m_LabelPlayer2Score.Text = m_GameProperties.Player2.Score.ToString();
            m_LabelPlayer2Score.AutoSize = true;
            m_LabelPlayer2Score.Top = 8;
            m_LabelPlayer2Score.Left = m_LabelPlayer2.Right + 5;
            m_LabelPlayer2Score.ForeColor = Color.Blue;
            m_LabelPlayer2Score.Font = new Font(m_LabelPlayer2Score.Font.Name, 12, FontStyle.Bold); 

            m_Buttons = new CheckersButton[m_GameProperties.BoardSize, m_GameProperties.BoardSize];
            m_Board = new Board(m_GameProperties.BoardSize);

            for (int i = 0; i < m_GameProperties.BoardSize; i++)
            {
                for (j = 0; j < m_GameProperties.BoardSize; j++)
                {
                    m_Buttons[i, j] = new CheckersButton(m_GameProperties.BoardSize);
                    m_Buttons[i, j].Size = new Size(80, 80);
                    m_Buttons[i, j].Text = m_Board.GetBoard[i, j];
                    m_Buttons[i, j].Font = new Font(m_Buttons[i, j].Font.Name, 20, FontStyle.Bold);
                    m_Buttons[i, j].AutoSize = true;
                    m_Buttons[i, j].Top = buttonTop;
                    m_Buttons[i, j].Left = currentButtonLeft;
                    if (Board.IsValidSpot(i, j))
                    {
                        m_Buttons[i, j].BackColor = Color.White;
                        m_Buttons[i, j].i = i;
                        m_Buttons[i, j].j = j;
                        m_Buttons[i, j].Click += new EventHandler(buttonFirstClick_Click);
                    }
                    else
                    {
                        m_Buttons[i, j].Enabled = false;
                        m_Buttons[i, j].BackColor = Color.Gray;
                    }

                    currentButtonLeft = m_Buttons[i, j].Right;
                    this.Controls.Add(m_Buttons[i, j]);
                }

                currentButtonLeft = buttonLeft;
                buttonTop = m_Buttons[i, j - 1].Bottom;
            }

            this.Controls.AddRange(new Control[] { m_LabelPlayer1, m_LabelPlayer1Score, m_LabelPlayer2, m_LabelPlayer2Score });
        }

        private bool IsValidClick(CheckersButton i_Button)
        {
            return (m_Gameplay.Player1Turn && m_Gameplay.IsPlayerTroop(1, i_Button.Text)) || (!m_Gameplay.Player1Turn && m_Gameplay.IsPlayerTroop(2, i_Button.Text));
        }

        private void MarkPressedButton(CheckersButton i_Button)
        {
            m_Clicked = true;
            i_Button.BackColor = Color.SkyBlue;
            i_Button.Click -= new EventHandler(buttonFirstClick_Click);
            i_Button.Click += new EventHandler(buttonSecondClick_Click);
        }

        private void ShowCurrentPlayerTurn()
        {
            if (m_Gameplay.Player1Turn)
            {
                m_LabelPlayer1.ForeColor = Color.Green;
                m_LabelPlayer2.ForeColor = Color.Blue;
            }
            else
            {
                m_LabelPlayer2.ForeColor = Color.Green;
                m_LabelPlayer1.ForeColor = Color.Blue;
            }
        }

        private void ButtonMoveClick(CheckersButton i_Button)
        {
            m_Gameplay.Move(ref m_Board, m_SourceButton.i, m_SourceButton.j, i_Button.i, i_Button.j);
            UnmarkButton(m_SourceButton);
            UpdateButtonMatrixStatus();
        }

        private bool CheckAdditionalMoves(CheckersButton i_Button)
        {
            return m_Gameplay.AnyAdditionalMove(m_Board, i_Button.i, i_Button.j, Math.Abs(i_Button.i - m_SourceButton.i));
        }

        private bool IsComputerTurn()
        {
            return !m_Gameplay.Player1Turn && m_GameProperties.Player2.IsComputer;
        }

        private void MakeComputerMove()
        {
            Move move;
            int iSource;
            int jSource;
            int iDestination;
            int jDestination;

            do
            {
                move = m_Gameplay.GetComputerMove(m_Board);
                iSource = move.IPreviousMove;
                jSource = move.JPreviousMove;
                iDestination = move.INextMove;
                jDestination = move.JNextMove;
                m_SourceButton = m_Buttons[iSource, jSource];
                m_Gameplay.Move(ref m_Board, iSource, jSource, iDestination, jDestination);
                UpdateButtonMatrixStatus();
            }
            while (CheckAdditionalMoves(m_Buttons[iDestination, jDestination]));
        }

        private void buttonFirstClick_Click(object sender, EventArgs e)
        {
            CheckersButton button = sender as CheckersButton;
            bool gameOver;

            if (!m_Clicked)
            {
                if (IsValidClick(button))
                {
                    m_SourceButton = button;
                    MarkPressedButton(button);
                }
                else
                {
                    if (button.Text.Equals(" "))
                    {
                        MessageBox.Show("Empty Space!");
                    }
                    else
                    {
                        MessageBox.Show("Opponent's Troop!");
                    }
                }
            }
            else
            {
                if (m_Gameplay.IsValidMove(m_Board, m_SourceButton.i, m_SourceButton.j, button.i, button.j))
                {
                    ButtonMoveClick(button);
                    if (!CheckAdditionalMoves(button))
                    {
                        m_Gameplay.SwitchTurn();
                        ShowCurrentPlayerTurn();
                        gameOver = CheckIfGameOver();
                        if (IsComputerTurn() && !gameOver)
                        {
                            MakeComputerMove();
                            m_Gameplay.SwitchTurn();
                            ShowCurrentPlayerTurn();
                            gameOver = CheckIfGameOver();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Move!");
                }
            }
        }

        private bool CheckIfGameOver()
        {
            string winningTroop;
            bool gameOver = true;

            if (m_Gameplay.IsGameOver(m_Board, false, out winningTroop))
            {
                if (winningTroop.Equals("X"))
                {
                    m_GameProperties.Player1.Score = m_Gameplay.CalculateScore(m_Board, winningTroop, false);
                    DialogResult = MessageBox.Show(string.Format("{0} Won!\nAnother Round?", m_GameProperties.Player1.Name), "Damka", MessageBoxButtons.YesNo);
                }
                else if (winningTroop.Equals("O"))
                {
                    m_GameProperties.Player2.Score = m_Gameplay.CalculateScore(m_Board, winningTroop, false);
                    DialogResult = MessageBox.Show(string.Format("{0} Won!\nAnother Round?", m_GameProperties.Player2.Name), "Damka", MessageBoxButtons.YesNo);
                }

                this.Close();
            }
            else if (m_Gameplay.IsDraw(m_Board))
            {
                DialogResult = MessageBox.Show(string.Format("Tie!\nAnotherRound?", m_GameProperties.Player2.Name), "Damka", MessageBoxButtons.YesNo);
            }
            else
            {
                gameOver = false;
            }

            return gameOver;
        }

        private void UpdateButtonMatrixStatus()
        {
            for (int i = 0; i < m_GameProperties.BoardSize; i++)
            {
                for (int j = 0; j < m_GameProperties.BoardSize; j++)
                {
                    m_Buttons[i, j].Text = m_Board.GetBoard[i, j];
                }
            }
        }

        private void UnmarkButton(CheckersButton i_Button)
        {
            m_Clicked = false;
            i_Button.BackColor = Color.White;
            i_Button.Click += new EventHandler(buttonFirstClick_Click);
            i_Button.Click -= new EventHandler(buttonSecondClick_Click);
        }

        private void buttonSecondClick_Click(object sender, EventArgs e)
        {
            CheckersButton button = sender as CheckersButton;

            if (m_Clicked)
            {
                if (IsValidClick(button))
                {
                    m_SourceButton = null;
                    UnmarkButton(button);
                }
            }
        }
    }
}
