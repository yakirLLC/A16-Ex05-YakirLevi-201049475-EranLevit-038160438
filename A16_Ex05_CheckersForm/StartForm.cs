using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using A16_Ex05_CheckersLogic;

namespace A16_Ex05_CheckersForm
{
    public class StartForm : Form
    {
        private int m_BoardSize = 6;
        private Player m_Player1, m_Player2;
        private Label m_LabelBoardSize = new Label();
        private Label m_LabelPlayers = new Label();
        private Label m_LabelPlayer1 = new Label();
        private Label m_LabelPlayer2 = new Label();
        private RadioButton m_RadioButton6x6 = new RadioButton();
        private RadioButton m_RadioButton8x8 = new RadioButton();
        private RadioButton m_RadioButton10x10 = new RadioButton();
        private TextBox m_TextBoxPlayer1 = new TextBox();
        private TextBox m_TextBoxPlayer2 = new TextBox();
        private CheckBox m_CheckBoxPlayer2 = new CheckBox();
        private Button m_ButtonDone = new Button();

        public StartForm()
        {
            this.Text = "Game Settings";
            this.Size = new Size(270, 210);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public Player Player1
        {
            get { return m_Player1; }
        }

        public Player Player2
        {
            get { return m_Player2; }
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        public bool IsPlayer2Exist
        {
            get { return m_CheckBoxPlayer2.Checked; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitControls();
        }

        private void InitControls()
        {
            m_LabelBoardSize.Text = "Board Size:";
            m_LabelBoardSize.Top = 8;
            m_LabelBoardSize.Left = 8;

            m_RadioButton6x6.Text = "6 x 6";
            m_RadioButton6x6.Checked = true;
            m_RadioButton6x6.Top = m_LabelBoardSize.Bottom - 4;
            m_RadioButton6x6.Left = m_LabelBoardSize.Left + 8;
            m_RadioButton6x6.AutoSize = true;

            m_RadioButton8x8.Text = "8 x 8";
            m_RadioButton8x8.Top = m_LabelBoardSize.Bottom - 4;
            m_RadioButton8x8.Left = m_RadioButton6x6.Right - 40;
            m_RadioButton8x8.AutoSize = true;

            m_RadioButton10x10.Text = "10 x 10";
            m_RadioButton10x10.Top = m_LabelBoardSize.Bottom - 4;
            m_RadioButton10x10.Left = m_RadioButton8x8.Right - 40;
            m_RadioButton10x10.AutoSize = true;

            m_LabelPlayers.Text = "Players:";
            m_LabelPlayers.Top = m_RadioButton6x6.Bottom;
            m_LabelPlayers.Left = 8;

            m_LabelPlayer1.Text = "Player 1:";
            m_LabelPlayer1.AutoSize = true;
            m_LabelPlayer1.Top = m_LabelPlayers.Bottom + 4;
            m_LabelPlayer1.Left = m_LabelPlayers.Left + 8;

            m_TextBoxPlayer1.Top = (m_LabelPlayer1.Top + (m_LabelPlayer1.Height / 2)) - (m_LabelPlayer1.Height / 2);

            m_CheckBoxPlayer2.Left = m_LabelPlayer1.Left;
            m_CheckBoxPlayer2.AutoSize = true;

            m_LabelPlayer2.Text = "Player 2:";
            m_LabelPlayer2.AutoSize = true;
            m_LabelPlayer2.Top = m_LabelPlayer1.Bottom + 10;
            m_LabelPlayer2.Left = m_CheckBoxPlayer2.Left + 16;

            m_TextBoxPlayer2.Top = (m_LabelPlayer2.Top + (m_LabelPlayer2.Height / 2)) - (m_LabelPlayer2.Height / 2);
            m_TextBoxPlayer2.Left = m_LabelPlayer2.Right - 30;

            m_TextBoxPlayer2.Text = "[Computer]";
            m_TextBoxPlayer2.Enabled = false;
            m_TextBoxPlayer1.Left = m_TextBoxPlayer2.Left;
            m_CheckBoxPlayer2.Top = (m_LabelPlayer2.Top + (m_LabelPlayer2.Height / 2)) - (m_LabelPlayer2.Height / 2) + 2;

            m_ButtonDone.Text = "Done";
            m_ButtonDone.Left = this.ClientSize.Width - m_ButtonDone.Width - 8;
            m_ButtonDone.Top = this.ClientSize.Height - m_ButtonDone.Height - 8;

            this.Controls.AddRange(new Control[] 
                                                   { 
                                                   m_LabelBoardSize, 
                                                   m_RadioButton6x6, 
                                                   m_RadioButton8x8, 
                                                   m_RadioButton10x10, 
                                                   m_LabelPlayers, 
                                                   m_LabelPlayer1, 
                                                   m_TextBoxPlayer1, 
                                                   m_CheckBoxPlayer2, 
                                                   m_LabelPlayer2, 
                                                   m_TextBoxPlayer2, 
                                                   m_ButtonDone
                                                   });
            m_ButtonDone.Click += new EventHandler(m_ButtonDone_Click);
            m_CheckBoxPlayer2.Click += new EventHandler(m_CheckBoxPlayer2_Click);
            m_RadioButton6x6.Click += new EventHandler(m_RadioButton_Click);
            m_RadioButton8x8.Click += new EventHandler(m_RadioButton_Click);
            m_RadioButton10x10.Click += new EventHandler(m_RadioButton_Click);
        }

        private void m_RadioButton_Click(object sender, EventArgs e)
        {
            if (m_RadioButton6x6.Checked)
            {
                m_BoardSize = 6;
            }
            else if (m_RadioButton8x8.Checked)
            {
                m_BoardSize = 8;
            }
            else
            {
                m_BoardSize = 10;
            }
        }

        private bool IsValidName(string i_Name)
        {
            return !(i_Name.Length > 20 || i_Name.Contains(" "));
        }

        private void m_CheckBoxPlayer2_Click(object sender, EventArgs e)
        {
            if (m_CheckBoxPlayer2.Checked)
            {
                m_TextBoxPlayer2.Text = string.Empty;
            }
            else
            {
                m_TextBoxPlayer2.Text = "[Computer]";
            }

            m_TextBoxPlayer2.Enabled = m_CheckBoxPlayer2.Checked;
        }

        private void m_ButtonDone_Click(object sender, EventArgs e)
        {
            bool isBadInput = false;
            MessageBoxButtons inputError = MessageBoxButtons.OK;

            if (m_TextBoxPlayer1.Text.Length != 0)
            {
                if (IsValidName(m_TextBoxPlayer1.Text))
                {
                    m_Player1 = new Player(m_TextBoxPlayer1.Text);
                }
                else
                {
                    isBadInput = true;
                    MessageBox.Show("Player Name can't be over 20 chars and can't contain spaces", "Player Name Bad Format", inputError);
                }

                if (m_CheckBoxPlayer2.Checked && m_TextBoxPlayer2.Text.Length != 0)
                {
                    if (IsValidName(m_TextBoxPlayer2.Text))
                    {
                        m_Player2 = new Player(m_TextBoxPlayer2.Text);
                    }
                    else
                    {
                        isBadInput = true;
                        MessageBox.Show("Player Name can't be over 20 chars and can't contain spaces", "Player Name Bad Format", inputError);
                    }
                }
                else if (!m_CheckBoxPlayer2.Checked)
                {
                    m_Player2 = new Player();
                }
                else
                {
                    isBadInput = true;
                    MessageBox.Show("Enter Player Name!", "Player Name Empty", inputError);
                }
            }
            else
            {
                isBadInput = true;
                MessageBox.Show("Enter Player Name!", "Player Name Empty", inputError);
            }

            if (!isBadInput)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
