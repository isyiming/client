using UnityEngine;

public class HexMapEditor : MonoBehaviour
{

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;

	void Awake()
	{
		SelectColor(0);
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
            //鼠标点击时，增加的单元格高度
            activeElevation =3;

            HandleInput();
		}
		else
		{
			previousCell = null;
		}
	}

    //鼠标点击单元格更改单元格颜色
	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

        //监测到鼠标点击时，
		if (Physics.Raycast(inputRay, out hit))
		{
            //获取鼠标点击的单元格
			HexCell currentCell = hexGrid.GetCell(hit.point);
			//如果上一个点击的单元格存在并且当前单元格和上一个不是同一个
			if (previousCell && previousCell != currentCell)
			{
				ValidateDrag(currentCell);
			}
			else
			{
				isDrag = false;
			}
			EditCell(currentCell);
			previousCell = currentCell;
		}
		else
		{
			previousCell = null;
		}
	}

	void ValidateDrag(HexCell currentCell)
	{
		for (
			dragDirection = HexDirection.NE;
			dragDirection <= HexDirection.NW;
			dragDirection++
		)
		{
			if (previousCell.GetNeighbor(dragDirection) == currentCell)
			{
				isDrag = true;
				return;
			}
		}
		isDrag = false;
	}

	public void SelectColor(int index)
	{
		activeColor = colors[index];
	}

	int activeElevation;

	void EditCell(HexCell cell)
	{
		cell.Color = activeColor;

		cell.Elevation = activeElevation;

		//编辑河流,如果先前有河流，移除原本河流
		if (riverMode == OptionalToggle.No)
		{
			print("无法编辑河流");
			cell.RemoveRiver();
		}
		else if (isDrag && riverMode == OptionalToggle.Yes)
		{
			print("编辑河流");
			HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
			if (otherCell)
			{
				otherCell.SetOutgoingRiver(dragDirection);
			}
		}

	}

	//设定单元格高度范围
	public void SetElevation(float elevation)
	{
		activeElevation = (int)elevation;
	}

	enum OptionalToggle
	{
		Yes,Ignore, No
	}

	OptionalToggle riverMode;

	public void SetRiverMode(int mode)
	{
		riverMode = (OptionalToggle)mode;
	}


}