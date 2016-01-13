﻿using System;
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
        //private Button m_SourceButton = new Button();
        private CheckersButton m_SourceButton;
        //private Button[,] m_Buttons;
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
                this.Size = new Size(527, 580);
            }
            else if (m_GameProperties.BoardSize == 8)
            {
                this.Size = new Size(685, 745);
            }
            else
            {
                this.Size = new Size(847, 900);
            }

            InitControls();
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    InitControls();
        //}

        private void InitControls()
        {
            int j, buttonTop = 50, buttonLeft = 20, currentButtonLeft = buttonLeft;

            m_LabelPlayer1.Text = m_GameProperties.Player1.Name + ":";
            m_LabelPlayer1.AutoSize = true;
            m_LabelPlayer1.Top = 8;
            m_LabelPlayer1.Left = this.ClientSize.Width / 4 - this.ClientSize.Width / 8;
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
            m_LabelPlayer2.Left = this.ClientSize.Width / 2 + this.ClientSize.Width / 4 - this.ClientSize.Width / 8;
            m_LabelPlayer2.ForeColor = Color.Blue;
            m_LabelPlayer2.Font = new Font(m_LabelPlayer2.Font.Name, 12, FontStyle.Bold); 

            m_LabelPlayer2Score.Text = m_GameProperties.Player2.Score.ToString();
            m_LabelPlayer2Score.AutoSize = true;
            m_LabelPlayer2Score.Top = 8;
            m_LabelPlayer2Score.Left = m_LabelPlayer2.Right;
            m_LabelPlayer2Score.ForeColor = Color.Blue;
            m_LabelPlayer2Score.Font = new Font(m_LabelPlayer2Score.Font.Name, 12, FontStyle.Bold); 

            //m_Buttons = new Button[m_GameProperties.BoardSize, m_GameProperties.BoardSize];
            m_Buttons = new CheckersButton[m_GameProperties.BoardSize, m_GameProperties.BoardSize];
            m_Board = new Board(m_GameProperties.BoardSize);

            for (int i = 0; i < m_GameProperties.BoardSize; i++)
            {
                for (j = 0; j < m_GameProperties.BoardSize; j++)
                {
                    //m_Buttons[i, j] = new Button();
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

            this.Controls.AddRange(new Control[] { m_LabelPlayer1, m_LabelPlayer2, m_LabelPlayer1Score, m_LabelPlayer2Score });
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
            return (m_Gameplay.AnyAdditionalMove(m_Board, i_Button.i, i_Button.j, Math.Abs(i_Button.i - m_SourceButton.i)));
        }

        private void buttonFirstClick_Click(object sender, EventArgs e)
        {
            //Button button = sender as Button;
            CheckersButton button = sender as CheckersButton;

            if (!m_Clicked)
            {
                if (IsValidClick(button))
                {
                    m_SourceButton = button;
                    MarkPressedButton(button);
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
                    }
                }
            }
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
            //Button button = sender as Button;
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
