using UnityEngine;
using System.Collections;
using Chess;
public class UGameBoard : MonoBehaviour {


    public GameObject cellObj;
    GameObject[,] CellsObjList = new GameObject[8, 8];
	// Use this for initialization
	void Start () 
    {
        PointV2 p = new PointV2(1,1);
        GameObject tmp;
        bool istest = true;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                tmp  = GameObject.Instantiate(cellObj,new Vector2((float)p.x ,(float)p.y),Quaternion.identity) as GameObject;
                tmp.GetComponent<TestCell>().position = p;
                CellsObjList[i, j] = tmp;
                if (istest){
                    if (j % 2 != 0)
                    { 
                        CellsObjList[i, j].renderer.material.color = Color.white;
                    }

                    else
                    {
                        CellsObjList[i, j].renderer.material.color = Color.black;
                    }
                }
                else if (!istest)
                {
                    if (j % 2 != 0) CellsObjList[i, j].renderer.material.color = Color.black;

                    else
                    {
                        Debug.Log("false, i % 2 == 0");
                        CellsObjList[i, j].renderer.material.color = Color.white;
                    }
                }
               

                
                p += PointV2.right;
            }
            istest = !istest;
            p = new PointV2(1, p.y+1);
        }

        // test(new Queen(CellStatus.Player1, new PointV2(1, 1)));

         Figure f1 = new Queen(CellStatus.Player1, new PointV2(1, 1));

         Figure f2 = new Queen(CellStatus.Player2, new PointV2(4, 4));


         GameBoard.Instance.Figures.Add(f1); GameBoard.Instance.Figures.Add(f2);
         f2.DeathEvent += f2_DeathEvent;
         test(f1);
         f1.Walk(new PointV2(4,4));

        /// test new alhoritm diagonales =============
         
        
	}

    void f2_DeathEvent(Figure obj)
    {
        Debug.LogError("DEATH");
    }
    void test(Figure f)
    {
        if (f == null)
        {
            return;
        }
        f.FocusFigureEvent += pawn_FocusFigureEvent;
        f.Focus();


    }

   

    void pawn_FocusFigureEvent(IFigure focusFigure)
    {
        try
        { 
            foreach (var itemCellsForWalks in focusFigure.CellsForWalks)
            {

                foreach (var item_Cells_Obj_List in CellsObjList)
                {
                    if (itemCellsForWalks.Equals(item_Cells_Obj_List.GetComponent<TestCell>().position))
                    {
                        item_Cells_Obj_List.renderer.material.color = Color.blue;
                    }

                }
            }
        }
        catch
        {
            Debug.LogError("ноги отсюда ростут: в цыклах");  
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
