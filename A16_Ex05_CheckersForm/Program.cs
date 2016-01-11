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
            StartForm sf = new StartForm();
            BoardForm bf;

            sf.ShowDialog();
            if (sf.DialogResult == DialogResult.OK)
            {
                bf = new BoardForm(sf);
                bf.ShowDialog();
            }
        }
    }
}
