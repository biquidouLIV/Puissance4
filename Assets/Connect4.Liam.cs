using UnityEngine;

public partial class Connect4 : MonoBehaviour
{
    
 float Eval_Liam_Taccon(CellType[,] Board, CellType joueur, int colonne,int profondeur)
 {
     return EvalIATurn(Board, colonne, profondeur);
 }

     private float EvalPlayerTurn(CellType[,] Board, int colonne, int profondeur)
     {
         float bestScore = 0;
         if (profondeur == 0)
         {
             CellType[,] newBoard = (CellType[,])Board.Clone();
             for (int i = 0; i < Board.GetLength(1); i++)
             {
                 float score = Eval(newBoard, CellType.Player1, i);
                 if (score > bestScore)
                 {
                     bestScore = score;
                 }
             }
             return bestScore;
         }
         
         for (int i = 0; i < Board.GetLength(1); i++)
         {
             CellType[,] newBoard = (CellType[,])Board.Clone();
             Coords co = DropToken(newBoard, i);
             newBoard[co.X, co.Y] = CellType.Player1;

             EvalIATurn(newBoard, i, profondeur - 1);
         }

         return bestScore;
     }
        
        
        
     private float EvalIATurn(CellType[,] Board, int colonne, int profondeur)
     {
         float bestScore = 0;
         if (profondeur == 0)
         {
             
             CellType[,] newBoard = (CellType[,])Board.Clone();
             for (int i = 0; i < Board.GetLength(1); i++)
             {
                 float score = Eval(newBoard, CellType.Player2, i);
                 if (score > bestScore)
                 {
                     bestScore = score;
                 }
             }
             return bestScore;
         }
         
         for (int i = 0; i < Board.GetLength(1); i++)
         {
             CellType[,] newBoard = (CellType[,])Board.Clone();
             Coords co = DropToken(newBoard, i);
             newBoard[co.X, co.Y] = CellType.Player2;

             EvalPlayerTurn(newBoard, i, profondeur - 1);
         }

         return bestScore;
     }


    private float Eval(CellType[,] Board, CellType joueur, int colonne)
    {
        float score = 0.0f;
        Coords test = DropToken(Board,colonne);
        
        
        //si gagne
        if (TestIfWon(Board, joueur, test))
        {
            score += 100000;
        }

        //si joueur gagne
        if (TestIfWon(Board, CellType.Player1, test))
        {
            score += 10000;
        }

        //si trop haut
        if (!TestToken(joueur, (short)colonne))
        {
            score += -10000000;
        }



        score += CompteSuite(test, joueur);
        score += CompteSuite(test, CellType.Player1);
        
        
        //Debug.Log("score colonne "+colonne+" : " + score);
        return score;
    }
 
 

    private int CompteSuite(Coords test, CellType joueur)
    {
        int score = 0;
        int suite = 0;
        bool autreCote = false;
        int directionX = 0;
        int directionY = 0;

        for (int j = 0; j < 4; j++)
        {
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
                            suite += 2;
                            score += suite;
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
                            suite += 2;
                            score += suite;
                        }
                    }
                }
            }
        }

        return score;
    }
}

