using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum CellType
    {
        Empty,
        Player1,
        Player2
    }


    
    private CellType[,] Board = new CellType[6, 7];
    private string ez;
    private bool player1Turn = true;
    
    [Header("images")]
    [SerializeField] private Image[] caseLigne0;
    [SerializeField] private Image[] caseLigne1;
    [SerializeField] private Image[] caseLigne2;
    [SerializeField] private Image[] caseLigne3;
    [SerializeField] private Image[] caseLigne4;
    [SerializeField] private Image[] caseLigne5;
    Image[][] tableau = new Image[6][];
    
    
    private void Start()
    {
        tableau[0] = caseLigne0;
        tableau[1] = caseLigne1;
        tableau[2] = caseLigne2;
        tableau[3] = caseLigne3;
        tableau[4] = caseLigne4;
        tableau[5] = caseLigne5;
        DisplayBoard(Board);
        
    }

    public void Button(int colonne)
    {
        
        
        if (player1Turn && TestToken(CellType.Player1,(short)colonne))
        {
            Board[DropToken(Board, CellType.Player1, colonne).X, DropToken(Board, CellType.Player1, colonne).Y] = CellType.Player1;
            DisplayBoard(Board);
            TestIfWon(Board, CellType.Player1, DropToken(Board, CellType.Player1, colonne));
        }
        else if (!player1Turn && TestToken(CellType.Player2,(short)colonne))
        {
            Board[DropToken(Board, CellType.Player2, colonne).X, DropToken(Board, CellType.Player2, colonne).Y] = CellType.Player2;
            DisplayBoard(Board);
            TestIfWon(Board, CellType.Player2, DropToken(Board, CellType.Player2, colonne));
        }
        else
        {
            return;
        }
        
        TestIfFinished(Board);
        player1Turn = !player1Turn;

    }
    
    public struct Coords
    {
        public int X;
        public int Y;

        public Coords(int posX, int posY)
        {
            X = posX;
            Y = posY;
        }
    }

    void ClearBoard(CellType[,] Board)
    {
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                Board[i,j] = CellType.Empty;
            }
        } 
    }
    
    void DisplayBoard(CellType[,] Board)
    {
        ez = "";
        for (int i = Board.GetLength(0)-1; i >= 0 ; i--)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                switch (Board[i,j])
                {
                    case(CellType.Empty):
                        ez += "[   ]";
                        tableau[i][j].GetComponent<Image>().color = Color.white;
                        break;
                    case(CellType.Player1):
                        ez += "[X]";
                        tableau[i][j].GetComponent<Image>().color = Color.red;
                        break;
                    case(CellType.Player2):
                        ez += "[O]";
                        tableau[i][j].GetComponent<Image>().color = Color.yellow;
                        break;
                }
            }
            ez += "\n";
        } 
        //.Log(ez);
    }




    bool TestToken(CellType joueur, short colonne)
    {
        if (Board[Board.GetLength(0)-1, colonne] == CellType.Empty)
        {
            return true;
        }

        return false;
    }
    
    private Coords DropToken(CellType[,] Board, CellType joueur, int colonne)
    {
        for (int i = 0; i < Board.GetLength(1) - 1; i++)
        {
            if (Board[i, colonne] == CellType.Empty)
            {
                return new Coords(i,colonne);
            }
        }
        return new Coords(-1, -1);
    }

    bool TestIfFinished(CellType[,] Board)
    {
        for (int i = 0; i < Board.GetLength(1)-1; i++)
        {
            if (Board[Board.GetLength(0) - 1, i] != CellType.Empty)
            { 
                return false;
            }
        }
        
        return true;
    }

    
    
    
    bool TestIfWon(CellType[,] Board, CellType joueur, Coords coupJoue)
    {
        int suite = 1;
        bool autreCote = false;

        int directionX = 0;
        int directionY = 0;

        Debug.Log("coupJoue  "+coupJoue.X+"/"+coupJoue.Y);


        for (int j = 0; j < 4; j++)
        {
            switch (j)
            {
                case(0): //vertical
                    Debug.Log("vertical");
                    directionX = -1;
                    directionY = 0;
                    break;
                case(1): //horizontal
                    Debug.Log("horizontal");
                    directionX = 0;
                    directionY = 1;
                    break;
                case(2): // diago 
                    Debug.Log("premiere diago");
                    directionX = -1;
                    directionY = 1;
                    break;
                case(3):
                    Debug.Log("deuxieme diago");
                    directionX = -1;
                    directionY = -1;
                    break;
                
            }

            suite = 1;
            autreCote = false;
            
            for (int i = 1; i < 4; i++)
            {
                if (!autreCote)
                {
                    
                    //Debug.Log((coupJoue.X + i * directionX) + " " + (coupJoue.Y + i * directionY));
                    if (!(coupJoue.X + i * directionX < 0 || coupJoue.X + i * directionX > 5 || coupJoue.Y + i * directionY > 6 || coupJoue.Y + i * directionY < 0))
                    {
                        
                        Debug.Log("kjhkjhgkjhg  "+(coupJoue.X + i * directionX)+"  "+ (coupJoue.Y + i * directionY));
                        if (Board[coupJoue.X + i * directionX, coupJoue.Y + i * directionY] != joueur)
                        {
                            Debug.Log("changement de coté");
                            autreCote = true;
                            i = 1;
                        }
                        else
                        {
                            suite++;
                            //Debug.Log(suite);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                if (autreCote)
                {

                    Debug.Log((coupJoue.X - i*directionX) + " " + (coupJoue.Y - i*directionY));
                    if (!(coupJoue.X - i * directionX < 0 || coupJoue.X - i * directionX > 5 || coupJoue.Y - i * directionY > 6 || coupJoue.Y - i * directionY < 0))
                    {
                        Debug.Log("noinonoion  "+(coupJoue.X - i * directionX)+"  "+ (coupJoue.Y - i * directionY));
                        if (Board[coupJoue.X - i * directionX, coupJoue.Y - i * directionY] != joueur)
                        {
                            //Debug.Log("pas gagné");
                        }
                        else
                        {
                            suite++;
                            //Debug.Log(suite);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                if (suite >= 4)
                {
                    Debug.Log("gagnééééééééééé");
                    return true;
                }
            }
        }
        return false;
    }


}    
