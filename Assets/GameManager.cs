using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] private GameObject endScreen;
    
    [Header("images")]
    [SerializeField] private Image[] caseLigne0;
    [SerializeField] private Image[] caseLigne1;
    [SerializeField] private Image[] caseLigne2;
    [SerializeField] private Image[] caseLigne3;
    [SerializeField] private Image[] caseLigne4;
    [SerializeField] private Image[] caseLigne5;
    Image[][] tableau = new Image[6][];

    private Stack<Coords> undoCoords = new Stack<Coords>();
    private Stack<CellType> undoPlayer = new Stack<CellType>();
    private Stack<Coords> redoCoords = new Stack<Coords>();
    private Stack<CellType> redoPlayer = new Stack<CellType>();
    
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
            Coords toto = DropToken(Board, CellType.Player1, colonne);
            Board[toto.X, toto.Y] = CellType.Player1;
            DisplayBoard(Board);
            TestIfWon(Board, CellType.Player1, toto);
            
            undoCoords.Push((toto));
            undoPlayer.Push(CellType.Player1);
        }
        else if (!player1Turn && TestToken(CellType.Player2,(short)colonne))
        {
            Coords toto2 = DropToken(Board, CellType.Player2, (int)Eval_Liam_Taccon(Board, CellType.Player2));
            Board[toto2.X, toto2.Y] = CellType.Player2;
            DisplayBoard(Board);
            TestIfWon(Board, CellType.Player2, toto2);
            
            undoCoords.Push((toto2));
            undoPlayer.Push(CellType.Player2);
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
                    directionX = 1;
                    directionY = 1;
                    break;
                case(3):
                    Debug.Log("deuxieme diago");
                    directionX = 1;
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
                        
                        //Debug.Log("kjhkjhgkjhg  "+(coupJoue.X + i * directionX)+"  "+ (coupJoue.Y + i * directionY));
                        if (Board[coupJoue.X + i * directionX, coupJoue.Y + i * directionY] != joueur)
                        {
                            
                            Debug.Log("changement de coté");
                            autreCote = true;
                            i = 1;
                        }
                        else
                        {
                            suite++;
                            //Debug.Log("dirx= "+directionX + "diry= "+directionY);
                            Debug.Log("suite : "+ suite + "    Coords : " + +(coupJoue.X + i * directionX)+"/"+ (coupJoue.Y + i * directionY) + "   dirX : " + directionX +" dirY : " + directionY);
                        }
                    }
                    else
                    {
                        autreCote = true;
                        i = 1;
                        //break;
                    }
                }

                if (autreCote)
                {

                    //Debug.Log((coupJoue.X - i*directionX) + " " + (coupJoue.Y - i*directionY));
                    if (!(coupJoue.X - i * directionX < 0 || coupJoue.X - i * directionX > 5 || coupJoue.Y - i * directionY > 6 || coupJoue.Y - i * directionY < 0))
                    {
                        //Debug.Log("noinonoion  "+(coupJoue.X - i * directionX)+"  "+ (coupJoue.Y - i * directionY));
                        if (Board[coupJoue.X - i * directionX, coupJoue.Y - i * directionY] != joueur)
                        {
                            Debug.Log("pas gagné");
                            break;
                        }
                        else
                        {
                            suite++;
                            Debug.Log("suite : "+suite + "    Coords : " + +(coupJoue.X - i * directionX)+"/"+ (coupJoue.Y - i * directionY) + "   dirX : " + directionX +" dirY : " + directionY);
                        }
                    }
                    else
                    {
                        //break;
                    }
                }

                if (suite >= 4)
                {
                    endScreen.SetActive(true);
                    Debug.Log("gagnééééééééééé");
                    return true;
                }
            }
        }
        return false;
    }
    

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Undo()
    {
        if(undoCoords.Count == 0)return;
        
        Board[undoCoords.Peek().X, undoCoords.Peek().Y] = CellType.Empty;
        redoCoords.Push(undoCoords.Pop());
        redoPlayer.Push(undoPlayer.Pop());
        
        DisplayBoard(Board);
    }
    

    public void Redo()
    {
        if(redoCoords.Count == 0)return;
        
        Board[redoCoords.Peek().X, redoCoords.Peek().Y] = redoPlayer.Peek();
        undoCoords.Push(redoCoords.Pop());
        undoPlayer.Push(redoPlayer.Pop());
        
        DisplayBoard(Board);
    }
    
    
    float Eval_Liam_Taccon(CellType[,] Board, CellType joueur)
    {
        
        return Random.Range(0, 7);
    }
}    
