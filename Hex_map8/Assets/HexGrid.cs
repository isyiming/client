using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HexGrid : MonoBehaviour
{

    //显示坐标使用的text
    public Text cellLabelPrefab;

    public HexCell cellPrefab;

    //声明六边形数组
    HexCell[] cells;



    //鼠标着色
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public Texture2D noiseSource;

    //定义六边形的公共宽度 高度 和单位预制件
    public int chunkCountX = 4, chunkCountZ = 3;

    public static int cellCountX, cellCountZ;

    public HexGridChunk chunkPrefab;

    HexGridChunk[] chunks;


    void Awake()
    {
        HexMetrics.noiseSource = noiseSource;

        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

        CreateChunks();

        CreateCells();

    }

    void Start()
    {
        //对单元格进行三角划分
        //hexMesh.Triangulate(cells);
    }



    void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    void CreateCells()
    {
        cells = new HexCell[cellCountZ * cellCountX];

        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void OnEnable()
    {
        HexMetrics.noiseSource = noiseSource;
    }

    void CreateCell(int x, int z, int i)
    {

        Vector3 position;

        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        //单元格的颜色,随机颜色
        float red = Random.Range(0.0f, 1.0f);
        float green = Random.Range(0.0f, 1.0f);
        float blue = Random.Range(0.0f, 1.0f);
        //cell.color = defaultColor;
        cell.color = new Color(red, green, blue);

        //记录邻近单元格
        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }


        //在text上显示坐标
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();


        //创建的最后添加显示坐标的ui
        cell.uiRect = label.rectTransform;

        //单元格的高度,随机高度
        //cell.Elevation = 2;
        cell.Elevation = (int)(Random.Range(0.0f, 1.0f) * 5);

        //单元格水位
        cell.WaterLevel = 2;

        AddCellToChunk(x, z, cell);

    }

    void AddCellToChunk(int x, int z, HexCell cell)
    {
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
    }



    //鼠标触摸网格方法
    public void TouchCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        Debug.Log("touched at " + coordinates.ToString());

        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = touchedColor;
    }

    //获取指定位置的单元格
    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        return cells[index];
    }


}
