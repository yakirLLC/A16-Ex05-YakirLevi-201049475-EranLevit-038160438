using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace A16_Ex05_CheckersForm
{
    public class CheckersButton : Button
    {
        private readonly int r_BoardSize;
        private int m_i;
        private int m_j;

        public CheckersButton(int i_BoardSize)
        {
            r_BoardSize = i_BoardSize;
        }

        public int i
        {
            get { return m_i; }
            set 
            {
                if (value >= 0 && value < r_BoardSize)
                {
                    m_i = value;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public int j
        {
            get { return m_j; }
            set 
            {
                if (value >= 0 && value < r_BoardSize)
                {
                    m_j = value;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }
}
