using UnityEngine;

public partial class Connect4 : MonoBehaviour
{
    
     float Eval_Liam_Taccon(CellType[,] Board, CellType joueur, int colonne,int profondeur)
     {
         CellType[,] newBoard = (CellType[,])Board.Clone();
         Coords co = DropToken(newBoard, colonne);
         newBoard[co.X, co.Y] = CellType.Player2;
         
         float score = EvalPlayerTurn(newBoard, profondeur, co);
         Debug.Log("colonne " + (colonne + 1) + " : " + score);
         return score;
     }
     
     

     private float EvalPlayerTurn(CellType[,] Board, int profondeur, Coords lastCoords)
     {
         float bestScore = float.MaxValue;
         if (profondeur == 0)
         {
             return Eval(Board, CellType.Player2, lastCoords);
         }
         
          for (int i = 0; i < Board.GetLength(1); i++)
          {
              if (Board[Board.GetLength(0) - 1, i] != CellType.Empty)
              {
                  continue;
              }
              
              CellType[,] newBoard = (CellType[,])Board.Clone();
              Coords co = DropToken(newBoard, i);
              
              if (co.X == -1)
              {
                  continue;
              }
              
              newBoard[co.X, co.Y] = CellType.Player1;

              float score = EvalIATurn(newBoard, profondeur-1, co);

              if (score < bestScore)
              {
                  bestScore = score;
              }
          }
         

              return bestScore;
          
     }
        
        
        
     private float EvalIATurn(CellType[,] Board, int profondeur, Coords lastCoords)
     {
         float bestScore = float.MinValue;
         if (profondeur == 0)
         {
             return Eval(Board, CellType.Player2, lastCoords);
         }
         
         for (int i = 0; i < Board.GetLength(1); i++)
         {
             if (Board[Board.GetLength(0) - 1, i] != CellType.Empty)
             {
                 continue;
             }
             
             CellType[,] newBoard = (CellType[,])Board.Clone();
             Coords co = DropToken(newBoard, i);
             
             if (co.X == -1)
             {
                 continue;
             }
             
             newBoard[co.X, co.Y] = CellType.Player2;

             float score = EvalPlayerTurn(newBoard, profondeur-1, co);

             if (score > bestScore)
             {
                 bestScore = score;
             }
         }


             return bestScore;

         
     }


    private float Eval(CellType[,] Board, CellType joueur, Coords co)
    {
        
        
        if (TestIfWon(Board, CellType.Player2, co))
        {
            return 100000;
        }
        
        if (TestIfWon(Board, CellType.Player1, co))
        {
            return 10000;
        }


        float score = CompteSuite(Board, co, CellType.Player2) + CompteSuite(Board, co, CellType.Player1) * 1.5f;
        return score;
    }
 
 

    private int CompteSuite(CellType[,] Board,Coords test, CellType joueur)
    {
        int score = 0;
        bool autreCote = false;
        int directionX = 0;
        int directionY = 0;

        for (int j = 0; j < 4; j++)
        {
            int suite = 0;
            switch (j)
            {
                case (0):
                    directionX = -1;
                    directionY = 0;
                    break;
                case (1):
                    directionX = 0;
                    directionY = 1;
                    break;
                case (2):
                    directionX = 1;
                    directionY = 1;
                    break;
                case (3):   
                    directionX = 1;
                    directionY = -1;
                    break;

            }
            autreCote = false;
            for (int i = 1; i < 4; i++)
            {
                if (!autreCote)
                {
                    if (!(test.X + i * directionX < 0 || test.X + i * directionX > 5 ||
                          test.Y + i * directionY > 6 || test.Y + i * directionY < 0))
                    {
                        if (Board[test.X + i * directionX, test.Y + i * directionY] != joueur)
                        {
                            autreCote = true;
                            i = 1;
                        }
                        else
                        {
                            suite ++;
                        }
                    }
                    else
                    {
                        autreCote = true;
                        i = 1;
                    }
                }

                if (autreCote)
                {
                    if (!(test.X - i * directionX < 0 || test.X - i * directionX > 5 ||
                          test.Y - i * directionY > 6 || test.Y - i * directionY < 0))
                    {
                        if (Board[test.X - i * directionX, test.Y - i * directionY] != joueur)
                        {
                            break;
                        }
                        else
                        {
                            suite ++;
                        }
                    }
                }
            }
            if (suite == 1) score += 20;
            if (suite == 2) score += 200;
            if (suite == 3) score += 500;
        }

        return score;
    }
}

