namespace CheckersGameLogic
{
    public class CheckersCell<T> where T : struct
    {
        private T m_Value;
        private bool m_Available;
        private int m_Row;
        private int m_Col;

        public CheckersCell(int i_Row, int i_Col)
        {
            m_Value = default(T);
            m_Available = true;
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public CheckersCell(int i_Row, int i_Col, T i_Value)
        {
            m_Value = i_Value;
            m_Available = false;
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public T Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public bool Available
        {
            get { return m_Available; }
            set { m_Available = value; }
        }

        public int Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public int Col
        {
            get { return m_Col; }
            set { m_Col = value; }
        }
    }
}
