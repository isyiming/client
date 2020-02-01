using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexCell : MonoBehaviour
{
    //单元格坐标
    public HexCoordinates coordinates;

    //单元格颜色
    public Color color;

    //单元格临近格
    [SerializeField]
    HexCell[] neighbors;

    //单元格的坐标显示ui
    public RectTransform uiRect;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //用于检索邻近单元格
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y +=
                (HexMetrics.SampleNoise(position).y * 2f - 1f) *
                HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;
        }
    }

    //单元格高度
    int elevation;


    //获取特定方向的单元格边缘类型
    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(
            elevation, neighbors[(int)direction].elevation
        );
    }

    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(
            elevation, otherCell.elevation
        );
    }

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

}
