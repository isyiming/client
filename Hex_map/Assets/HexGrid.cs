using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HexGrid : MonoBehaviour
{
    //定义六边形的公共宽度 高度 和单位预制件
    public int width = 6;
    public int height = 6;

    //显示坐标使用的text
    public Text cellLabelPrefab;

    //画布
    Canvas gridCanvas;

    public HexCell cellPrefab;

    //声明六边形数组
    HexCell[] cells;

    //六边形网格
    HexMesh hexMesh;


    //鼠标着色
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    void Awake()
    {
        //画布
        gridCanvas = GetComponentInChildren<Canvas>();

        //六边形网格
        hexMesh = GetComponentInChildren<HexMesh>();

        //画布
        gridCanvas = GetComponentInChildren<Canvas>();


    //创建六边形，铺设地图
    cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;


        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;


        //在text上显示坐标
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

    }

    void Start()
    {
        //对单元格进行三角划分
        hexMesh.Triangulate(cells);
    }



    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            TouchCell(hit.point);
        }
    }

    //void TouchCell(Vector3 position)
    //{
    //    position = transform.InverseTransformPoint(position);
    //    Debug.Log("touched at " + position);
    //}

    //鼠标触摸网格方法
    public void TouchCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        Debug.Log("touched at " + coordinates.ToString());

        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = touchedColor;
        hexMesh.Triangulate(cells);
    }
}
