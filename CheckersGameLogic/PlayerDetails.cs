using System.Collections.Generic;
using CheckersGameLogic.CheckersEnumUtils;

namespace CheckersGameLogic
{
    public class PlayerDetails
    {
        private readonly string r_PlayerName;
        private readonly eSymboleType r_PlayerSymbole;
        private readonly eSymboleType r_KingSymbole;
        private readonly eDirectionType r_Direction;
        private readonly List<CheckersCell<eSymboleType>> r_ListOfGamePawns;
        private int m_GameScore;
        private int m_AllGamesScore;

        public PlayerDetails(eSymboleType i_Symbole, eSymboleType i_KingSymbole, string i_PlayerName)
        {
            r_PlayerName = i_PlayerName;
            r_PlayerSymbole = i_Symbole;
            r_KingSymbole = i_KingSymbole;
            initializeScores();
            r_Direction = getPlayerDirection();
            r_ListOfGamePawns = new List<CheckersCell<eSymboleType>>();
        }

        private void initializeScores()
        {
            m_GameScore = 0;
            m_AllGamesScore = 0;
        }

        private eDirectionType getPlayerDirection()
        {
            eDirectionType playerDirection;

            if (r_PlayerSymbole.Equals(eSymboleType.SecondPlayerSymbole))
            {
                playerDirection = eDirectionType.SecondPlayerDirection;
            }
            else
            {
                playerDirection = eDirectionType.FirstPlayerDirection;
            }

            return playerDirection;
        }

        public void UpdateGamePawns(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol, eSymboleType i_Symbole)
        {
            DeleteGamePawn(i_FromRow, i_FromCol);
            r_ListOfGamePawns.Add(new CheckersCell<eSymboleType>(i_ToRow, i_ToCol, i_Symbole));
        }

        public void DeleteGamePawn(int i_Row, int i_Col)
        {
            foreach (CheckersCell<eSymboleType> cellToDelete in r_ListOfGamePawns)
            {
                if (cellToDelete.Row == i_Row && cellToDelete.Col == i_Col)
                {
                    r_ListOfGamePawns.Remove(cellToDelete);
                    break;
                }
            }
        }

        public void DeleteAllGamePawns()
        {
            r_ListOfGamePawns.Clear();
        }

        public void UpdateScore()
        {
            int score = 0;

            foreach (CheckersCell<eSymboleType> pawn in r_ListOfGamePawns)
            {
                if (pawn.Value.Equals(r_KingSymbole))
                {
                    score += (int)ePawnScore.KingScore;
                }
                else
                {
                    score += (int)ePawnScore.RegularScore;
                }
            }

            m_GameScore = score;
        }

        public bool IsItAPlayerSymbole(eSymboleType i_Symbole)
        {
            return i_Symbole.Equals(r_PlayerSymbole) || i_Symbole.Equals(r_KingSymbole);
        }

        public string PlayerName
        {
            get { return r_PlayerName; }
        }

        public eSymboleType PlayerSymbole
        {
            get { return r_PlayerSymbole; }
        }

        public eSymboleType KingSyymbole
        {
            get { return r_KingSymbole; }
        }

        public List<CheckersCell<eSymboleType>> GamePawns
        {
            get { return r_ListOfGamePawns; }
        }

        public eDirectionType Direction
        {
            get { return r_Direction; }
        }

        public int GameScore
        {
            get { return m_GameScore; }
            set { m_GameScore = value; }
        }

        public int AllGamesScore
        {
            get { return m_AllGamesScore; }
            set { m_AllGamesScore = value; }
        }
    }
}
