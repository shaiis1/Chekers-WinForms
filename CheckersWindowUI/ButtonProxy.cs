using System.Windows.Forms;

namespace CheckersWindowUI
{
    public class ButtonProxy : Button
    {
        private int m_Row;
        private int m_Col;

        public ButtonProxy(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public int ButtonColumn
        {
            get { return m_Col; }
            set { m_Col = value; }
        }

        public int ButtonRow
        {
            get { return m_Row; }
            set { m_Row = value; }
        }
    }
}
