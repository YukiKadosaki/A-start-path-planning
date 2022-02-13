using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0:�E
//1:�O
//2:��
//3���


public class Node
{
    const int check_direction_num = 4;//���C��4�����ɔ�΂�
    const float ray_max_distance = 100;
    const int direction_num = 4;

    private int m_count;
    private Vector3 m_position;
    private Vector3 m_GoalPositon;
    private List<Node> m_NextNode;
    private float m_RayLength; //�ׂ̃m�[�h��T�����߂̃��C�̒���
    private int m_DirectionNum;//�ǂ̕�������������Ă�����

    private Vector3[] m_direction = new Vector3[check_direction_num];
    
    private float m_gcost;//�����_�ł̃R�X�g

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

        //�ǂ̕�������������Ă��������v�Z����
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
                    //4�����Ƀ��C���΂��A���̂����o����
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
            Vector3 displacement = m_direction[i] * m_RayLength;//�����̍��W�Ǝ��̍��W�Ƃ̕ψ�
            Vector3 rayOrigin = m_position + displacement;
            rayOrigin.y = 2.5f;

            ray = new Ray(rayOrigin, Vector3.down);//4�����̃��C

            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, ray_max_distance))//���C����Q���ɓ�����Ȃ��Ƃ�
            {
                //�m�[�h�����
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
