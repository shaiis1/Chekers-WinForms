using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CheckersGameLogic;
using CheckersGameLogic.CheckersEnumUtils;

namespace CheckersWindowUI
{
    public partial class SettingsForm : Form
    {
        private readonly Label r_LabelBoardSize;
        private readonly Label r_LabelPlayers;
        private readonly Label r_LabelFirstPlayer;
        private readonly Label r_LabelSecondPlayer;
        private readonly TextBox r_TextBoxFirstPlayer;
        private readonly TextBox r_TextBoxSecondPlayer;
        private readonly RadioButton r_RadioButtonSixSize;
        private readonly RadioButton r_radioButtonEightSize;
        private readonly RadioButton r_RadioButtonTenSize;
        private readonly CheckBox r_CheckBoxSecondPlayer;
        private readonly Button r_ButtonDone;
        private PlayerDetails m_FirstPlayer;
        private PlayerDetails m_SecondPlayer;
        private eGameStyle m_GameStyle = eGameStyle.VsComputer;
        private CheckersForm m_FormCheckers;
        private int m_BoardSize;

        public SettingsForm()
        {
            this.Text = "Game Settings";
            r_LabelBoardSize = new Label();
            r_LabelPlayers = new Label();
            r_LabelFirstPlayer = new Label();
            r_LabelSecondPlayer = new Label();
            r_TextBoxFirstPlayer = new TextBox();
            r_TextBoxSecondPlayer = new TextBox();
            r_RadioButtonSixSize = new RadioButton();
            r_radioButtonEightSize = new RadioButton();
            r_RadioButtonTenSize = new RadioButton();
            r_CheckBoxSecondPlayer = new CheckBox();
            r_ButtonDone = new Button();
            initializeControls();
        }

        private void initializeControls()
        {
            int textBoxTop, secondTextBoxTop;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ClientSize = new Size(300, 350);

            r_LabelBoardSize.Location = new Point(10, 10);
            r_LabelBoardSize.Text = "Board Size:";

            r_RadioButtonSixSize.Location = new Point(30, r_LabelBoardSize.Bottom + 2);
            r_RadioButtonSixSize.Text = "6X6";

            r_radioButtonEightSize.Location = new Point(r_RadioButtonSixSize.Right + 2, r_LabelBoardSize.Bottom + 2);
            r_radioButtonEightSize.Text = "8X8";

            r_RadioButtonTenSize.Location = new Point(r_radioButtonEightSize.Right + 2, r_LabelBoardSize.Bottom + 2);
            r_RadioButtonTenSize.Text = "10X10";

            r_LabelPlayers.Location = new Point(r_LabelBoardSize.Left, r_RadioButtonSixSize.Bottom + 10);
            r_LabelPlayers.Text = "Players:";

            r_LabelFirstPlayer.Location = new Point(r_RadioButtonSixSize.Left, r_LabelPlayers.Bottom + 2);
            r_LabelFirstPlayer.Text = "First Player:";

            textBoxTop = r_LabelFirstPlayer.Top + (r_LabelFirstPlayer.Height / 2);
            textBoxTop -= r_TextBoxFirstPlayer.Height / 2;
            
            r_TextBoxFirstPlayer.Location = new Point(r_LabelFirstPlayer.Right + 22, textBoxTop);

            r_CheckBoxSecondPlayer.Location = new Point(r_LabelFirstPlayer.Left, r_LabelFirstPlayer.Bottom + 2);
            r_CheckBoxSecondPlayer.Checked = false;

            r_LabelSecondPlayer.Location = new Point(r_CheckBoxSecondPlayer.Right - 90, r_LabelFirstPlayer.Bottom + 6);
            r_LabelSecondPlayer.Text = "Second Player:";

            secondTextBoxTop = r_LabelSecondPlayer.Top + (r_LabelSecondPlayer.Height / 2);
            secondTextBoxTop -= r_TextBoxSecondPlayer.Height / 2;

            r_TextBoxSecondPlayer.Location = new Point(r_LabelSecondPlayer.Right + 8, secondTextBoxTop);
            r_TextBoxSecondPlayer.Enabled = false;
            r_TextBoxSecondPlayer.Text = "[Computer]";

            r_ButtonDone.Location = new Point(
                this.ClientSize.Width - r_ButtonDone.Width - 8,
                this.ClientSize.Height - r_ButtonDone.Height - 8);
            r_ButtonDone.Text = "Done";
            
            this.Controls.AddRange(new Control[]
            {
                    r_LabelBoardSize,
                    r_LabelPlayers,
                    r_LabelFirstPlayer,
                    r_LabelSecondPlayer,
                    r_TextBoxFirstPlayer,
                    r_TextBoxSecondPlayer,
                    r_RadioButtonSixSize,
                    r_radioButtonEightSize,
                    r_RadioButtonTenSize,
                    r_CheckBoxSecondPlayer,
                    r_ButtonDone
            });
            initializeEvents();
        }

        private void initializeEvents()
        {
            r_ButtonDone.Click += buttonDone_Pressed;
            r_CheckBoxSecondPlayer.Click += checkBoxSecondPlayer_Pressed;
            r_RadioButtonSixSize.Click += radioButtonSixSize_Pressed;
            r_radioButtonEightSize.Click += radioButtonEightSize_Pressed;
            r_RadioButtonTenSize.Click += radioButtonTenSize_Pressed;
        }

        private void initializeFirstPlayer()
        {
            m_FirstPlayer = new PlayerDetails(
                eSymboleType.FirstPlayerSymbole, 
                eSymboleType.FirstPlayerKing,
                r_TextBoxFirstPlayer.Text);
        }

        private void buttonDone_Pressed(object sender, EventArgs e)
        {
            if (!r_radioButtonEightSize.Checked &&  !r_RadioButtonSixSize.Checked && !r_RadioButtonTenSize.Checked)
            {
                startAgainAndShowRelevantMessage(eSettingsErrors.BoardSizeError);
            }
            else if (r_TextBoxFirstPlayer.Text.Equals(string.Empty) || r_TextBoxSecondPlayer.Text.Equals(string.Empty))
            {
                startAgainAndShowRelevantMessage(eSettingsErrors.NamesError);                
            }
            else
            {
                initializeFirstPlayer();
                initializeSecondPlayer();
                this.Hide();
                m_FormCheckers = new CheckersForm(m_BoardSize, m_FirstPlayer, m_SecondPlayer, m_GameStyle);
                m_FormCheckers.ShowDialog();
            }
        }

        private void startAgainAndShowRelevantMessage(eSettingsErrors i_Error)
        {
            SettingsForm startSettingsAgain;

            this.Hide();
            startSettingsAgain = new SettingsForm();
            MessageBox.Show(getErrorMessage(i_Error));
            startSettingsAgain.ShowDialog();
        }

        private string getErrorMessage(eSettingsErrors i_Error)
        {
            string errorMessage = string.Empty;

            switch(i_Error)
            {
                case eSettingsErrors.BoardSizeError:
                    errorMessage = string.Format("You must choose board size.{0}Please try again!", Environment.NewLine);
                    break;
                case eSettingsErrors.NamesError:
                    errorMessage = string.Format("You must enter names.{0}Please try again!", Environment.NewLine);
                    break;
            }

            return errorMessage;
        }

        private void initializeSecondPlayer()
        {
            m_SecondPlayer = new PlayerDetails(
                eSymboleType.SecondPlayerSymbole,
                eSymboleType.SecondPlayerKing,
                r_TextBoxSecondPlayer.Text);
        }

        private void checkBoxSecondPlayer_Pressed(object sender, EventArgs e)
        {
            r_CheckBoxSecondPlayer.Checked = true;
            r_TextBoxSecondPlayer.Enabled = true;
            m_GameStyle = eGameStyle.VsPlayer;
            r_TextBoxSecondPlayer.Text = string.Empty;
            r_CheckBoxSecondPlayer.Click -= checkBoxSecondPlayer_Pressed;
            r_CheckBoxSecondPlayer.Click += checkBoxSecondPlayer_PressedAgain;          
        }

        private void checkBoxSecondPlayer_PressedAgain(object sender, EventArgs e)
        {
            r_CheckBoxSecondPlayer.Checked = false;
            r_TextBoxSecondPlayer.Enabled = false;
            m_GameStyle = eGameStyle.VsComputer;
            r_TextBoxSecondPlayer.Text = "[Computer]";
            r_CheckBoxSecondPlayer.Click -= checkBoxSecondPlayer_PressedAgain;
            r_CheckBoxSecondPlayer.Click += checkBoxSecondPlayer_Pressed;
        }

        private void radioButtonSixSize_Pressed(object sender, EventArgs e)
        {
            m_BoardSize = 6;
        }

        private void radioButtonEightSize_Pressed(object sender, EventArgs e)
        {
            m_BoardSize = 8;
        }

        private void radioButtonTenSize_Pressed(object sender, EventArgs e)
        {
            m_BoardSize = 10;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
        }
    }
}
