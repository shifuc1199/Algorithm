using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public float x;
    public float y;
    
    public float G;
    public float H;

    public float F { get { return G + H; } }

    public bool isCanWalk=true;

    public Node(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public void CalculateG(Node n)
    {
        var dx = Mathf.Abs(x - n.x);
        var dy = Mathf.Abs(y - n.y);
        if (dx > dy)
        {
            G = 14 * dy + 10 * (dx - dy)+n.G;
        }
        else
        {
           G = 14 * dx + 10 * (dy - dx) + n.G;
        }

    }
    public void CalculateH(Node n)
    {
        var dx = Mathf.Abs(x - n.x);
        var dy = Mathf.Abs(y - n.y);
        if (dx > dy)
        {
            H= 14 * dy + 10 * (dx - dy);
        }
        else
        {
            H= 14 * dx + 10 * (dy - dx);
        }
    }
    public void Print()
    {
        Debug.Log("CanWalk: "+isCanWalk+": "+x + "," + y + " G: " + G + " H: " + H+" F: "+F);
    }
    
}
public class Astar : MonoBehaviour
{
    public int Width;
    public int Height;
    public Node[,] Girds;
    public List<Node> OpenList = new List<Node>();
    public List<Node> CloseList = new List<Node>();
    public Vector2 EndPos;
    public Vector2 StartPos;
    public GameObject prefab;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
              if(Physics.CheckBox(new Vector2((-4.5f + i), (-4.5f + j)), new Vector2(0.25f, 0.25f), Quaternion.identity, LayerMask.GetMask("wall")))
                {
                    Gizmos.color = Color.red;
                }
              else
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawCube(new Vector2((-4.5f + i), (-4.5f + j)),new Vector3(0.5f,0.5f,0.5f));
            }
        }

    }
    private void Start()
    {
        Node start_node = new Node(StartPos.x, StartPos.y);
        Node end_node = new Node(EndPos.x, EndPos.y);
         Girds = new Node[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
     
                Girds[i, j] = new Node((-4.5f+i),(-4.5f+j));
                Girds[i, j].isCanWalk = ! Physics.CheckBox(new Vector2((-4.5f + i), (-4.5f + j)),new Vector2(0.25f,0.25f),Quaternion.identity,LayerMask.GetMask("wall"));
                Girds[i, j].CalculateH(end_node);
            }
        }

        OpenList.Add(start_node);
        FindAround((int)(OpenList[0].x + 4.5f), (int)(OpenList[0].y + 4.5f));
        CloseList.Add(OpenList[0]);
        OpenList.RemoveAt(0);
        foreach (var item in OpenList)
        {
            item.Print();
        }

        while (OpenList.Find(a => { return (a.x == EndPos.x && a.y == EndPos.y); }) == null && OpenList.Count > 0)
        {
            Node parent_node = FindMinFNodeInOpenList();
            FindAround((int)(parent_node.x + 4.5f), (int)(parent_node.y + 4.5f));
            CloseList.Add(parent_node);
            OpenList.Remove(parent_node);
        }

        foreach (var item in CloseList)
        {
            Instantiate(prefab, new Vector2(item.x, item.y), Quaternion.identity);
        }
    }
    public void FindAround(int x, int y)
    {
    
        FindOneGrid(x + 1, y+1,x,y);
        FindOneGrid(x + 1, y,x,y);
        FindOneGrid(x + 1, y-1,x,y);

        FindOneGrid(x - 1, y + 1,x,y);
        FindOneGrid(x - 1, y, x,y);
        FindOneGrid(x - 1, y - 1, x,y);

        FindOneGrid(x , y + 1, x, y);
        FindOneGrid(x , y - 1, x, y);
     
    }
    public Node FindMinFNodeInOpenList()
    {
        Node n=null;
        float F = float.MaxValue;
        foreach (var item in OpenList)
        {
            if(item.F<F)
            {
                F = item.F;
                n = item;
            }
        }
        return n;
    }
    public void FindOneGrid(int x, int y,int startx,int starty)
    {
 

        if (x < 0 || x >= Width)
            return;
        if (y < 0 || y >= Width)
            return;

        if (!CloseList.Contains(Girds[x, y]))
        {
            if (!OpenList.Contains(Girds[x, y]))
            {
                if(Girds[x, y].isCanWalk)
                OpenList.Add(Girds[x, y]);
               
            }
            else
            {
                Girds[x, y].CalculateG(Girds[startx,starty]);

                //更新G
            }
        }
    }
}
