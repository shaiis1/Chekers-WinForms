using System;

namespace CheckersGameLogic
{
    public class ComputerMovesGenerator
    {
        private readonly Random r_ComputerRand;
        private readonly int r_RandomMaxValue;
        private int m_FromRow;
        private int m_FromCol;
        private int m_ToRow;
        private int m_ToCol;

        public ComputerMovesGenerator(int i_RandomMaxValue)
        {
            r_RandomMaxValue = i_RandomMaxValue;
            r_ComputerRand = new Random();
        }

        public void GenerateMoves()
        {
            m_FromRow = r_ComputerRand.Next(r_RandomMaxValue);
            m_FromCol = r_ComputerRand.Next(r_RandomMaxValue);
            m_ToRow = r_ComputerRand.Next(r_RandomMaxValue);
            m_ToCol = r_ComputerRand.Next(r_RandomMaxValue);
        }

        public int FromRow
        {
            get { return m_FromRow; }
        }

        public int FromCol
        {
            get { return m_FromCol; }
        }

        public int ToRow
        {
            get { return m_ToRow; }
        }

        public int ToCol
        {
            get { return m_ToCol; }
        }
    }
}
