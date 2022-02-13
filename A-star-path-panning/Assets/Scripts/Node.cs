using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0:右
//1:前
//2:左
//3後ろ


public class Node
{
    const int check_direction_num = 4;//レイを4方向に飛ばす
    const float ray_max_distance = 100;
    const int direction_num = 4;

    private int m_count;
    private Vector3 m_position;
    private Vector3 m_GoalPositon;
    private List<Node> m_NextNode;
    private float m_RayLength; //隣のノードを探すためのレイの長さ
    private int m_DirectionNum;//どの方向から向かってきたか

    private Vector3[] m_direction = new Vector3[check_direction_num];
    
    private float m_gcost;//現時点でのコスト

    public Vector3 Position
    {
        get => m_position;
    }

    public float Gcost
    {
        get => m_gcost;
    }


    public Node(Vector3 position, Vector3 goalPostion, int count, float gcost, float length = 0.5f)
    {
        m_gcost = gcost;
        NodeCounter.coutPluse();

        m_RayLength = length;
        m_position = position;
        m_count = count;

        m_direction[0] = Vector3.right;
        m_direction[1] = Vector3.forward;
        m_direction[2] = Vector3.left;
        m_direction[3] = Vector3.back;

        //どの方向から向かってきたかを計算する
        /*m_DirectionNum = dnum < 4 ? (dnum + 2) % 4 : dnum;

        Debug.Log("aiu");
        Ray ray1 = new Ray(m_position + Vector3.up, Vector3.down);
        if (!Physics.Raycast(ray1, 10))
        {
            GameObject obj = (GameObject)Resources.Load("Sphere");
            Instantiate(obj, m_position, Quaternion.identity);
        }


        if (m_count < 8)
        {
            Vector3[] direction = new Vector3[direction_num];


            direction[0] = Vector3.right;
            direction[1] = Vector3.forward;
            direction[2] = Vector3.left;
            direction[3] = Vector3.back;


            Ray ray = new Ray();
            for (int i = 0; i < direction_num; i++)
            {
                if (i != m_DirectionNum)
                {
                    ray = new Ray(m_position, direction[i]);
                    //4方向にレイを飛ばし、物体を検出する
                    if (!Physics.SphereCast(ray, 0.1f, m_RayLength))
                    {
                        Node n = new Node(m_position + direction[i] * m_RayLength, m_count + 1, m_RayLength, i);
                        //m_NextNode.Add(n);
                    }
                }
            }
        }
        */


    }

    public List<Node> GetNextNode()
    {
        List<Node> n = new List<Node>();


        Debug.DrawRay(m_position, Vector3.down, Color.red, 10f, false);

        Ray ray;
        for(int i = 0; i < check_direction_num; i++)
        {
            Vector3 displacement = m_direction[i] * m_RayLength;//自分の座標と次の座標との変位
            Vector3 rayOrigin = m_position + displacement;
            rayOrigin.y = 2.5f;

            ray = new Ray(rayOrigin, Vector3.down);//4方向のレイ

            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, ray_max_distance))//レイが障害物に当たらないとき
            {
                //ノードを作る
                n.Add(new Node(m_position + displacement, m_GoalPositon, m_count, m_gcost + m_RayLength));
                //Debug.Log(m_position + displacement);
                //Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f, false);
            }
            else
            {
                //Debug.DrawRay(ray.origin, ray.direction, Color.blue, 50, false);
                //Debug.Log("Name:" + hit.collider.name);
            }
            

        }

        return n;
    }

    public float GetFcost()
    {
        return m_gcost + GetHcost();
    }

    public float GetHcost()
    {
        Vector3 a = m_position;
        a.y = 0;
        Vector3 b = m_GoalPositon;
        b.y = 0;
        return Vector3.Distance(a, b);
    }
}
