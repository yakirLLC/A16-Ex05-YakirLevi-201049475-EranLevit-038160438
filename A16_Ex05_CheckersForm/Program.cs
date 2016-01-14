using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace A16_Ex05_CheckersForm
{
    public class Program
    {
        public static void Main()
        {
            StartForm startForm = new StartForm();
            BoardForm boadForm;

            startForm.ShowDialog();
            if (startForm.DialogResult == DialogResult.OK)
            {
                do
                {
                    boadForm = new BoardForm(startForm);
                    boadForm.ShowDialog();
                }
                while(boadForm.DialogResult == DialogResult.Yes);
            }
        }
    }
}
