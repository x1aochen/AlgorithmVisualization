using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GenerateScene : MonoBehaviour
{
    
    private GameObject singleCube;
    private GameObject mutilCube;
    private GameObject line;
    private Transform histogram; //Cube���ɵ�
    private Transform treeDiagram; //Tree���ɵ�
    //���ڵ����¼��
    public float nodeDistanceY;
    private Vector3 offset;
    private Vector3 scale;
    //���߶�
    private int treeHeight;
    //���鳤��
    private int num;
    public int Num
    {
        set => num = value;
    }


    private void Awake()
    {
        singleCube = Resources.Load<GameObject>("SingleNumberCube");
        mutilCube = Resources.Load<GameObject>("MutilNumberCube");
        line = Resources.Load<GameObject>("LineRender");
        histogram = GameObject.Find("Histogram").transform;
        treeDiagram = GameObject.Find("TreeDiagram").transform;
        Init();
    }
    private void Init()
    {
        //����cube����Y��
        scale = new Vector3(1, 0, 1);
        //cubeƫ����
        SortHelper.instance.Offset = offset = Vector3.right + Vector3.up / 2 + Vector3.right * 0.3f;
        //�������ڵ�Y���
        SortHelper.instance.NodeDistanceY = nodeDistanceY = -4;
        SortHelper.instance.TreeStartPos = treeDiagram;
    }

    public void HideTree()
    {
        treeDiagram.gameObject.SetActive(false);
    }

    public void ShowTree()
    {
        //˵����δ��ʼ����
        if (treeDiagram.childCount <= 0)
        {
            GenerateTreeDisgram();
        }

        treeDiagram.gameObject.SetActive(true);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void GenerateTreeDisgram()
    {
        if (num <= 0)
            return;
        treeHeight = 0;
        GetTreeHeight(0);
        //������ͷ�ڵ�
        Transform head = GameObject.Instantiate<GameObject>(mutilCube, treeDiagram).transform;
        head.position = treeDiagram.position;
        SetNumber(SortHelper.instance.sortDic[SortHelper.instance.array[0]], head);
        SortHelper.instance.OriginColor = head.GetComponent<Renderer>().material.color;
        SortHelper.instance.nodeArray[0] = head;
        GenerateTree(0);
        SortHelper.instance.TreeHeight = treeHeight;
        SortHelper.instance.ResetNodePos(0, treeHeight, head.position);
        GenerateLine();
        ShowTree();
    }

    /// <summary>
    /// �ݹ�������
    /// </summary>
    /// <param name="i">��������</param>
    /// <param name="floor">��ǰ����</param>
    /// <param name="parent">���ڵ�</param>
    private void GenerateTree(int i)
    {
        int k = 2 * i + 1;
        //��������
        if (k < num)
        {
            Transform left = GameObjectPool.instance.CreateObject("nutilCube", mutilCube);
            left.SetParent(treeDiagram);
            SetNumber(SortHelper.instance.sortDic[SortHelper.instance.array[k]], left);
            //node����nodeArray����
            SortHelper.instance.nodeArray[k] = left;
            GenerateTree(k);
            left.gameObject.SetActive(true);
        }
        //�����Һ���
        if (k + 1 < num)
        {
            Transform right = GameObjectPool.instance.CreateObject("nutilCube", mutilCube);
            right.SetParent(treeDiagram);
            SetNumber(SortHelper.instance.sortDic[SortHelper.instance.array[k + 1]], right);
            SortHelper.instance.nodeArray[k + 1] = right;
            GenerateTree(k + 1);
            right.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// �õ����������(���ڵ㲻��)
    /// </summary>
    /// <param name="i"></param>
    private void GetTreeHeight(int i)
    {
        if (2 * i + 1 < num)
        {
            treeHeight++;
            GetTreeHeight(2 * i + 1);
        }
    }

    /// <summary>
    /// ���ú��룬�����������Ƕ����չʾ����
    /// </summary>
    /// <param name="i"></param>
    /// <param name="node"></param>
    private void SetNumber(int i, Transform node)
    {
        TMP_Text[] numbers = node.GetComponentsInChildren<TMP_Text>();
        foreach (var num in numbers)
        {
            num.text = i.ToString();
        }
    }

    /// <summary>
    /// ������״ͼ
    /// </summary>
    public void GenerateHistogram()
    {
        for (int i = 0; i < num; i++)
        {
            SortHelper.instance.AddItem(i + 1, GenerateCube(i));
        }
        GenerateTreeDisgram();
    }

    /// <summary>
    /// ����Cube
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private Transform GenerateCube(int i)
    {
        Transform obj = GameObjectPool.instance.CreateObject("singleCube", singleCube);
        obj.SetParent(histogram);
        Transform cube = obj.transform.Find("Cube");

        cube.localScale = Vector3.up * (i + 1) + scale;
        obj.localPosition = offset * i;
        cube.GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Transform canvase = obj.Find("Canvas");
        canvase.localPosition = new Vector3(cube.localPosition.x, -i / 2.0f, -0.51f);
        canvase.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();

        return obj;
    }
    /// <summary>
    /// ������
    /// </summary>
    private void GenerateLine()
    {
        for (int i = num - 1; i > 0; i--)
        {
            Transform lineObj = GameObjectPool.instance.CreateObject("line", line);
            lineObj.SetParent(treeDiagram);
            Vector3[] pos = new Vector3[2]
            {
                SortHelper.instance.nodeArray[i].position,
                SortHelper.instance.nodeArray[(i - 1) / 2].position
            };
            lineObj.GetComponent<LineRenderer>().SetPositions(pos);
        }
    }

    public void HideAll()
    {
        if (histogram.childCount <= 0)
            return;
        Transform[] array = SortHelper.instance.array;
        for (int i = 0; i < num; i++)
        {
            array[i].gameObject.SetActive(false);
        }
        if (treeDiagram.childCount > 0 && SortHelper.instance.nodeArray[0] != null)
        {
            HideAllNode(SortHelper.instance.nodeArray);

        }
    }

    public void HideAllNode(Transform[] nodeArray)
    {
        for (int i = 0;i < num; i++)
        {
            nodeArray[i].gameObject.SetActive(false);
        }
        LineRenderer[] lines = treeDiagram.GetComponentsInChildren<LineRenderer>();
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].gameObject.SetActive(false);
        }
    }

}
