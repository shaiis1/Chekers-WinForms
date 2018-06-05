using CheckersGameLogic.CheckersEnumUtils;

namespace CheckersGameLogic
{
    public class LegalMoves
    {
        private readonly int r_FromSpotRow;
        private readonly int r_FromSpotColumn;
        private readonly int r_ToSpotRow;
        private readonly int r_ToSpotColumn;
        private readonly eMoveType r_KindOfMove;

        public LegalMoves(int i_FromSpotRow, int i_FromSpotColumn, int i_ToSpotRow, int i_ToSpotColumn, eMoveType i_KindOfMove)
        {
            r_FromSpotRow = i_FromSpotRow;
            r_FromSpotColumn = i_FromSpotColumn;
            r_ToSpotRow = i_ToSpotRow;
            r_ToSpotColumn = i_ToSpotColumn;
            r_KindOfMove = i_KindOfMove;
        }

        public bool IsItTheSameMove(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            return
                r_FromSpotRow == i_FromRow &&
                r_FromSpotColumn == i_FromCol &&
                r_ToSpotRow == i_ToRow &&
                r_ToSpotColumn == i_ToCol;
        }

        public int FromRow
        {
            get { return r_FromSpotRow; }
        }

        public int FromColumn
        {
            get { return r_FromSpotColumn; }
        }

        public int ToRow
        {
            get { return r_ToSpotRow; }
        }

        public int ToColumn
        {
            get { return r_ToSpotColumn; }
        }

        public eMoveType Move
        {
            get { return r_KindOfMove; }
        }
    }
}