using UnityEngine;
using System.Collections;
using Chess;
public class TestCell : MonoBehaviour, IFigure {
    public byte test_id ;
    public Vector2 test_position;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        test_id = id;
        test_position = new Vector2(position.x, position.y);
	}

    #region Члены IFigure

    public CellStatus side { get; set; }

    public PointV2 position {get;set;}
 

    public int id { get;set;}
    


    public System.Collections.Generic.List<PointV2> CellsForWalks
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
            throw new System.NotImplementedException();
        }
    }

    public System.Collections.Generic.List<PointV2> CellsForAttacks
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion
}
