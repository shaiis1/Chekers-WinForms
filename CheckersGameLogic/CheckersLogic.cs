using System.Collections.Generic;
using CheckersGameLogic.CheckersEnumUtils;

namespace CheckersGameLogic
{
    public class CheckersLogic
    {
        private readonly eGameStyle r_GameStyle;
        private readonly GameBoard r_Board;
        private readonly List<LegalMoves> r_LegalMoves;
        private readonly List<LegalMoves> r_LegalJumpMoves;
        private PlayerDetails m_CurrentPlayer;
        private PlayerDetails m_LastPlayer;
        private bool m_GameOver = false;
        private bool m_Tie = false;

        public CheckersLogic(PlayerDetails i_FirstPlayer, PlayerDetails i_SecondPlayer, int i_BoardSize, eGameStyle i_GameStyle)
        {
            m_CurrentPlayer = i_FirstPlayer;
            m_LastPlayer = i_SecondPlayer;
            r_GameStyle = i_GameStyle;
            r_Board = new GameBoard(i_BoardSize, m_CurrentPlayer, m_LastPlayer);
            r_LegalMoves = new List<LegalMoves>();
            r_LegalJumpMoves = new List<LegalMoves>();
        }

        public bool PlayOneGame(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol, out bool o_IsItTheSamePlayerMove)
        {
            bool isValidInput = false;

            o_IsItTheSamePlayerMove = false;
            isValidInput = doMove(i_FromRow, i_FromCol, i_ToRow, i_ToCol, out o_IsItTheSamePlayerMove);             
            if (isValidInput && !o_IsItTheSamePlayerMove)
            {
                SwapPlayersTurns();
            }

            return isValidInput;
        }

        public bool IsAnyMovesLeft()
        {
            bool movesExist;

            r_LegalJumpMoves.Clear();
            r_LegalMoves.Clear();
            generateMovesToAllPawns();
            movesExist = checkIfAnyMovesExistAndUpdateThem();
            if (!movesExist)
            {
                if (m_CurrentPlayer.GamePawns.Count == 0)
                {
                    finishGame();
                }
                else
                {
                    m_Tie = true;
                }
            }
            else
            {
                if (m_Tie)
                {
                    finishGame();
                    m_Tie = false;
                }
            }

            return movesExist;
        }

        private void generateMovesToAllPawns()
        {
            foreach (CheckersCell<eSymboleType> gamePawn in m_CurrentPlayer.GamePawns)
            {
                if (gamePawn.Value.Equals(eSymboleType.FirstPlayerKing) || gamePawn.Value.Equals(eSymboleType.SecondPlayerKing))
                {
                    generateMovesToSpecificPawn(gamePawn.Row, gamePawn.Col, (int)m_LastPlayer.Direction);
                }

                generateMovesToSpecificPawn(gamePawn.Row, gamePawn.Col, (int)m_CurrentPlayer.Direction);
            }
        }

        private void generateMovesToSpecificPawn(int i_FromRow, int i_FromCol, int i_DirectionOfPlayer)
        {
            checkIfMoveAvailableAndAddToListOfLegalMoves(i_FromRow, i_FromCol, i_DirectionOfPlayer, eColumnMoveType.Left);
            checkIfMoveAvailableAndAddToListOfLegalMoves(i_FromRow, i_FromCol, i_DirectionOfPlayer, eColumnMoveType.Right);
            generateJumpMoves(i_FromRow, i_FromCol, i_DirectionOfPlayer);
        }

        private void checkIfMoveAvailableAndAddToListOfLegalMoves(
            int i_FromRow,
            int i_FromCol,
            int i_DirectionOfPlayer,
            eColumnMoveType i_ColumnMove)
        {
            if (r_LegalJumpMoves.Count == 0)
            {
                if (r_Board.IsCellExistAndAvailable(i_FromRow + i_DirectionOfPlayer, i_FromCol + (int)i_ColumnMove))
                {
                    updateListOfLegalMoves(
                        i_FromRow,
                        i_FromCol,
                        i_FromRow + i_DirectionOfPlayer,
                        i_FromCol + (int)i_ColumnMove,
                        eMoveType.Regular);
                }
            }
        }

        private void generateJumpMoves(int i_FromRow, int i_FromCol, int i_DirectionOfPlayer)
        {
            checkIfJumpAvailableAndAddToListOfLegalMoves(i_FromRow, i_FromCol, i_DirectionOfPlayer, eColumnMoveType.Left);
            checkIfJumpAvailableAndAddToListOfLegalMoves(i_FromRow, i_FromCol, i_DirectionOfPlayer, eColumnMoveType.Right);
        }

        private void checkIfJumpAvailableAndAddToListOfLegalMoves(
            int i_FromRow,
            int i_FromCol,
            int i_DirectionOfPlayer,
            eColumnMoveType i_ColumnMove)
        {
            if(r_Board.IsCellExist(i_FromRow + i_DirectionOfPlayer, i_FromCol + (int)i_ColumnMove) &&
               m_LastPlayer.IsItAPlayerSymbole(r_Board.Board[i_FromRow + i_DirectionOfPlayer, i_FromCol + (int)i_ColumnMove].Value) &&
               r_Board.IsCellExistAndAvailable(i_FromRow + (2 * i_DirectionOfPlayer), i_FromCol + (2 * (int)i_ColumnMove)))
            {
                updateListOfLegalMoves(
                    i_FromRow,
                    i_FromCol,
                    i_FromRow + (2 * i_DirectionOfPlayer),
                    i_FromCol + (2 * (int)i_ColumnMove),
                    eMoveType.Jump);
            }
        }

        private void updateListOfLegalMoves(
            int i_FromSpotRow,
            int i_FromSpotCol,
            int i_ToSpotRow,
            int i_ToSpotCol,
            eMoveType i_KindOfMove)
        {
            List<LegalMoves> legalMoves;

            if (i_KindOfMove == eMoveType.Regular)
            {
                legalMoves = r_LegalMoves;
            }
            else
            {
                legalMoves = r_LegalJumpMoves;
            }

            legalMoves.Add(new LegalMoves(i_FromSpotRow, i_FromSpotCol, i_ToSpotRow, i_ToSpotCol, i_KindOfMove));
        }

        private bool checkIfAnyMovesExistAndUpdateThem()
        {
            bool isExist = false;

            if (r_LegalJumpMoves.Count != 0)
            {
                isExist = true;
                r_LegalMoves.Clear();
            }

            return isExist || r_LegalMoves.Count > 0;
        }

        private void finishGame()
        {
            updateScore();
            m_GameOver = true;
        }

        private void updateScore()
        {
            m_CurrentPlayer.UpdateScore();
            m_LastPlayer.UpdateScore();
        }

        private bool doMove(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol, out bool o_IsItTheSamePlayerMove)
        {
            bool isValidInput = true;
            eMoveType kindOfMove;
            List<LegalMoves> legalMoves;

            if (r_LegalJumpMoves.Count != 0)
            {
                kindOfMove = eMoveType.Jump;
                legalMoves = r_LegalJumpMoves;
                o_IsItTheSamePlayerMove = true;
            }
            else
            {
                kindOfMove = eMoveType.Regular;
                legalMoves = r_LegalMoves;
                o_IsItTheSamePlayerMove = false;
            }

            if(checkIfMoveIsInLegalMoves(i_FromRow, i_FromCol, i_ToRow, i_ToCol, legalMoves))
            {
                updateMove(i_FromRow, i_FromCol, i_ToRow, i_ToCol, kindOfMove);

                if (!(kindOfMove.Equals(eMoveType.Jump) && checkIfCanJumpAgain(i_ToRow, i_ToCol)))
                {
                    o_IsItTheSamePlayerMove = false;
                }
            }
            else
            {
                isValidInput = false;
            }

            return isValidInput;
        }

        private bool checkIfMoveIsInLegalMoves(
            int i_FromSpotRow,
            int i_FromSpotColumn,
            int i_ToSpotRow,
            int i_ToSpotColumn,
            List<LegalMoves> i_LegalMoves)
        {
            bool isItInLegalMoves = false;

            foreach (LegalMoves move in i_LegalMoves)
            {
                if (move.IsItTheSameMove(i_FromSpotRow, i_FromSpotColumn, i_ToSpotRow, i_ToSpotColumn))
                {
                    isItInLegalMoves = true;
                    break;
                }
            }

            return isItInLegalMoves;
        }

        private void updateMove(int i_FromSpotRow, int i_FromSpotColumn, int i_ToSpotRow, int i_ToSpotColumn, eMoveType i_KindOfMove)
        {
            int movementRow, movmentCol;

            if (i_KindOfMove.Equals(eMoveType.Jump))
            {
                movementRow = i_FromSpotRow + getDirectionType(i_FromSpotRow, i_ToSpotRow);
                movmentCol = i_FromSpotColumn + getDirectionType(i_FromSpotColumn, i_ToSpotColumn);
                r_Board.SetCellValue(movementRow, movmentCol, eSymboleType.SpaceSymbole);
                m_LastPlayer.DeleteGamePawn(movementRow, movmentCol);
            }

            r_Board.SetCellValue(i_ToSpotRow, i_ToSpotColumn, r_Board.Board[i_FromSpotRow, i_FromSpotColumn].Value);
            m_CurrentPlayer.UpdateGamePawns(
                i_FromSpotRow,
                i_FromSpotColumn,
                i_ToSpotRow,
                i_ToSpotColumn,
                r_Board.Board[i_ToSpotRow, i_ToSpotColumn].Value);
            r_Board.SetCellValue(i_FromSpotRow, i_FromSpotColumn, eSymboleType.SpaceSymbole);
        }

        private int getDirectionType(int i_FirstIndex, int i_SecondIndex)
        {
            return i_FirstIndex < i_SecondIndex ?
                (int)eDirectionType.SecondPlayerDirection :
                (int)eDirectionType.FirstPlayerDirection;
        }

        private bool checkIfCanJumpAgain(int i_Row, int i_Col)
        {
            r_LegalJumpMoves.Clear();

            if (r_Board.Board[i_Row, i_Col].Value.Equals(m_CurrentPlayer.KingSyymbole))
            {
                generateJumpMoves(i_Row, i_Col, (int)m_LastPlayer.Direction);
            }

            generateJumpMoves(i_Row, i_Col, (int)m_CurrentPlayer.Direction);

            return r_LegalJumpMoves.Count > 0;
        }

        public void SwapPlayersTurns()
        {
            PlayerDetails tempForSwap = m_LastPlayer;

            m_LastPlayer = m_CurrentPlayer;
            m_CurrentPlayer = tempForSwap;
        }

        public bool GetWinner(out PlayerDetails o_Winner, out PlayerDetails o_Loser)
        {
            o_Winner = m_CurrentPlayer;
            o_Loser = m_LastPlayer;
            if (!m_Tie)
            {
                if (o_Winner.GameScore < o_Loser.GameScore)
                {
                    o_Winner = m_LastPlayer;
                    o_Loser = m_CurrentPlayer;
                }
            }

            return !m_Tie;
        }

        public void HandleQuit()
        {
            m_CurrentPlayer.DeleteAllGamePawns();
            finishGame();
        }

        public void UpdateScoresForNewGame(PlayerDetails i_Winner, PlayerDetails i_Loser)
        {
            i_Winner.AllGamesScore += i_Winner.GameScore - i_Loser.GameScore;
            i_Winner.GameScore = 0;
            i_Loser.GameScore = 0;
            i_Winner.DeleteAllGamePawns();
            i_Loser.DeleteAllGamePawns();
        }

        public CheckersCell<eSymboleType>[,] Board
        {
            get { return r_Board.Board; }
        }

        public GameBoard Game
        {
            get { return r_Board; }
        }

        public bool IsGameOver
        {
            get { return m_GameOver; }
        }

        public PlayerDetails CurrentPlayer
        {
            get { return m_CurrentPlayer; }
        }

        public PlayerDetails LastPlayer
        {
            get { return m_LastPlayer; }
        }

        public bool TieFlag
        {
            get { return m_Tie; }
        }
    }
}