using CheckersGameLogic.CheckersEnumUtils;

namespace CheckersGameLogic
{
    public class GameBoard
    {
        private readonly int r_BoardSize;
        private readonly CheckersCell<eSymboleType>[,] r_Board;
        private readonly PlayerDetails r_FirstPlayer;
        private readonly PlayerDetails r_SecondPlayer;

        public GameBoard(int i_BoardSize, PlayerDetails i_FirstPlayer, PlayerDetails i_SecondPlayer)
        {
            r_FirstPlayer = i_FirstPlayer;
            r_SecondPlayer = i_SecondPlayer;
            r_BoardSize = i_BoardSize;
            r_Board = new CheckersCell<eSymboleType>[r_BoardSize, r_BoardSize];
            initializeBoardValues();
        }

        private void initializeBoardValues()
        {
            int middleSpaces;

            middleSpaces = (r_BoardSize / 2) - 1;
            fillBothPlayerRows(middleSpaces);

            for (int i = middleSpaces; i < middleSpaces + 2; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    r_Board[i, j] = new CheckersCell<eSymboleType>(i, j);
                    SetCellValue(i, j, eSymboleType.SpaceSymbole);
                }
            }
        }

        private void fillBothPlayerRows(int i_EndRows)
        {
            int columnToAddSpaceFirstPlayer,
                columnToAddSpaceSecondPlayer, 
                columnToAddFirstPlayerSymbole,
                columnToAddSecondPlayerSymbole;

            for (int i = 0; i < i_EndRows; i++)
            {
                for (int j = 0; j < r_BoardSize; j = j + 2)
                {
                    generateColsSymbols(
                        i,
                        j,
                        out columnToAddSpaceFirstPlayer, 
                        out columnToAddSpaceSecondPlayer,
                        out columnToAddFirstPlayerSymbole,
                        out columnToAddSecondPlayerSymbole);

                    fillRowValues(
                        i, 
                        columnToAddSpaceSecondPlayer,
                        columnToAddSecondPlayerSymbole,
                        r_SecondPlayer.PlayerSymbole);
                    fillRowValues(
                        r_BoardSize - i - 1,
                        columnToAddSpaceFirstPlayer,
                        columnToAddFirstPlayerSymbole,
                        r_FirstPlayer.PlayerSymbole);
                }
            }
        }

        private void generateColsSymbols(
            int i_Row,
            int i_Col,
            out int i_SpaceColsFirstPlayer, 
            out int i_SpaceColsSecondPlayer,
            out int i_SymboleColsFirstPlayer,
            out int i_SymboleColsSecondPlayer)
        {
            i_SpaceColsFirstPlayer = r_BoardSize - i_Col - 1;
            i_SpaceColsSecondPlayer = i_Col;
            i_SymboleColsFirstPlayer = r_BoardSize - i_Col + (int)r_FirstPlayer.Direction - 1;
            i_SymboleColsSecondPlayer = i_Col + (int)r_SecondPlayer.Direction;

            if (i_Row % 2 != 0)
            {
                i_SpaceColsFirstPlayer = i_SymboleColsFirstPlayer;
                i_SymboleColsFirstPlayer = r_BoardSize - i_Col - 1;
                i_SpaceColsSecondPlayer = i_SymboleColsSecondPlayer;
                i_SymboleColsSecondPlayer = i_Col;
            }
        }

        private void fillRowValues(int i_Row, int i_ColToSpaces, int i_ColToPlayerSymbole, eSymboleType i_PlayerSymbole)
        {
            r_Board[i_Row, i_ColToSpaces] = new CheckersCell<eSymboleType>(i_Row, i_ColToSpaces);
            SetCellValue(i_Row, i_ColToSpaces, eSymboleType.SpaceSymbole);
            r_Board[i_Row, i_ColToPlayerSymbole] =
                new CheckersCell<eSymboleType>(i_Row, i_ColToPlayerSymbole, i_PlayerSymbole);
            addCellToPlayerList(r_Board[i_Row, i_ColToPlayerSymbole]);
        }

        public void SetCellValue(int i_Row, int i_Col, eSymboleType i_Value)
        {
            bool isAvailable = false;

            if (i_Value.Equals(eSymboleType.SpaceSymbole))
            {
                isAvailable = true;
            }
            else if(i_Row == 0 && i_Value.Equals(eSymboleType.FirstPlayerSymbole))
            {
                i_Value = eSymboleType.FirstPlayerKing;
            }
            else if (i_Row == r_BoardSize - 1 && i_Value.Equals(eSymboleType.SecondPlayerSymbole))
            {
                i_Value = eSymboleType.SecondPlayerKing;
            }

            r_Board[i_Row, i_Col].Value = i_Value;
            r_Board[i_Row, i_Col].Available = isAvailable;
        }

        private void addCellToPlayerList(CheckersCell<eSymboleType> i_Cell)
        {
            if (i_Cell.Value.Equals(r_FirstPlayer.PlayerSymbole))
            {
                r_FirstPlayer.GamePawns.Add(i_Cell);
            }
            else
            {
                r_SecondPlayer.GamePawns.Add(i_Cell);
            }
        }

        public bool IsCellExistAndAvailable(int i_Row, int i_Column)
        {
            return IsCellExist(i_Row, i_Column) && r_Board[i_Row, i_Column].Available;
        }

        public bool IsCellExist(int i_Row, int i_Column)
        {
            return i_Row >= 0 && i_Row < r_BoardSize && i_Column >= 0 && i_Column < r_BoardSize;
        }

        public CheckersCell<eSymboleType>[,] Board
        {
            get { return r_Board; }
        }

        public int BoardSize
        {
            get { return r_BoardSize; }
        }

        public PlayerDetails FirstPlayer
        {
            get { return r_FirstPlayer; }
        }

        public PlayerDetails SecondPlayer
        {
            get { return r_SecondPlayer; }
        }
    }
}
