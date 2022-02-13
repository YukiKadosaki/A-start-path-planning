using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainAnalyst : MonoBehaviour
{
    private Node m_RootNode;
    public Vector3 pos;
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 a = Vector3.up;
        Vector3 b = Vector3.up;
        Vector3 c = Vector3.down;


        //AStar(transform.position, obj.transform.position);
        StartCoroutine(A_Star(transform.position, obj.transform.position));
        
    }


    private void CreateNode(Vector3 position)
    {
        m_RootNode = new Node(position, obj.transform.position,0, 0.5f);
    }

    public void AStar(Vector3 startPos, Vector3 goalPos)
    {
        int count = 0;
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        open.Add(new Node(startPos, goalPos,0, 0));


        
        while(count < 100)
        {
            //openリストの先頭を取り出す
            //int index = Random.Range(0, open.Count - 1);
            Node s = open[0];
            open.RemoveAt(0);
            Debug.Log("Fcost : " + s.GetFcost());
            

            //Debug.Log("Open : " + open.Count);

            //sの隣のノードを取り出して
            foreach (Node n in s.GetNextNode())
            {
                if (!InList(open, n) && !InList(closed, n))
                {
                    open.Add(n);
                }
            }
            count++;
            closed.Add(s);

            //Debug.Log(NodeCounter.nodeNum);
        }
        

        List<Vector3> ret = new List<Vector3>();
        ret.Add(Vector3.zero);

        //return ret;
    }

    private IEnumerator A_Star(Vector3 startPos, Vector3 goalPos)
    {
        int count = 0;
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        open.Add(new Node(startPos, obj.transform.position, 0, 0));



        while (count < 1000)
        {
            //openリストの先頭を取り出す
            int index = GetSmallestCostNode(open);
            Node s = open[index];
            open.RemoveAt(index);

            if (IsGoal(s))
            {
                Debug.LogError("Goal");
                break;
            }

            //Debug.LogError("Gcost : " + s.Gcost + " Hcost : " + s.GetHcost() + " Fcost : " + s.GetFcost());


            //Debug.Log("Open : " + open.Count);

            //sの隣のノードを取り出して
            foreach (Node n in s.GetNextNode())
            {
                //すでに探索済みだったらopenには入れない
                if (!InList(closed, n))
                {
                    //可能であればOpenリストにnを入れる。
                    CheckOpenList(open, n);
                }


            }
            count++;
            closed.Add(s);

            //Debug.Log(NodeCounter.nodeNum);
            yield return new WaitForSeconds(0.01f);
        }


        List<Vector3> ret = new List<Vector3>();
        ret.Add(Vector3.zero);

        

        //return ret;
    }

    //fコストが最小のノードの添え字を返す
    private int GetSmallestCostNode(List<Node> node)
    {
        int index = 0;
        float temp = 10000;
        for(int i = 0;i < node.Count; i++)
        {
            float fcost = node[i].GetFcost();
            if(fcost < temp)
            {
                index = i;
                temp = fcost;
            }
        }

        return index;
    }

    //同じだったらコストの小さい方にする
    private void CheckOpenList(List<Node> nodes, Node node)
    {
        //すでにリストの中にあるかどうか
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].Position == node.Position && nodes[i].Gcost > node.Gcost)
            {
                nodes[i] = node;
                return;
            }
        }
        //Openリスト中になければ入れる。
        nodes.Add(node);
    }

    private bool InList(List<Node> nodes, Node node)
    {
        //すでにリストの中にあるかどうか
        for(int i = 0;i < nodes.Count; i++)
        {
            if(nodes[i].Position == node.Position)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsGoal(Node n)
    {
        if(Vector3.Distance(n.Position, obj.transform.localPosition) < 1)
        {
            return true;
        }
        return false;
    }
}
