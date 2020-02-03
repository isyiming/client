using UnityEngine;
using UnityEngine.UI;


public class HexGridChunk : MonoBehaviour
{

    HexCell[] cells;

    HexMesh hexMesh;
    Canvas gridCanvas;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
    }

    void Start()
    {
        //检查当前chunk下有多少个子物体，如果超过4个，说明没有cell，那么不进行三角剖分
        Transform[] father = GetComponentsInChildren<Transform>();
        //foreach (var child in father)
        //{
        //    Debug.Log(father.Length);
        //}
        if (father.Length > 4)
        {
            hexMesh.Triangulate(cells);
        }
        hexMesh.Triangulate(cells);
    }

    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }

    //为了减少三角剖分次数，设立标志变量enabled，只在cell改变的时候进行剖分
    public void Refresh()
    {
        //hexMesh.Triangulate(cells);
        enabled = true;
    }

    void LateUpdate()
    {
        //检查当前chunk下有多少个子物体，如果超过4个，说明没有cell，那么不进行三角剖分
        Transform[] father = GetComponentsInChildren<Transform>();
        if (father.Length>4)
        {
            hexMesh.Triangulate(cells);
        }
        enabled = false;
    }
}