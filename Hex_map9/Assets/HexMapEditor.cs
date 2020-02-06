using UnityEngine;

public class HexMapEditor : MonoBehaviour
{

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;

	bool isDrag;
	HexDirection dragDirection;
	HexCell previousCell;


	int activeElevation = 3;//海拔
	int activeWaterLevel = 4;//水位
	//单元格上的特征物体密度，由于哈希生成，带有一定随机性
	int activeUrbanLevel=2, activeFarmLevel=2, activePlantLevel=2;


	bool applyElevation = true;
	bool applyWaterLevel = true;
	bool applyUrbanLevel = true, applyFarmLevel = false, applyPlantLevel = false;

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
				print("监测到鼠标拖动");
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

	void EditCell(HexCell cell)
	{
		print("编辑");

        //单元格颜色
		cell.Color = activeColor;

        //单元格高度
        //cell.Elevation = 2;

        //单元格水平面
        //if (applyWaterLevel)
        //{
        //cell.WaterLevel = activeWaterLevel;
        //}


        //是否在单元格添加特征
        //if (applyUrbanLevel)
        //{
        cell.UrbanLevel = activeUrbanLevel;
        //}
        //if (applyFarmLevel)
        //{
        cell.FarmLevel = activeFarmLevel;
        //}
        //if (applyPlantLevel)
        //{
        cell.PlantLevel = activePlantLevel;
        //}



        riverMode = OptionalToggle.Yes;
		roadMode = OptionalToggle.No;

		//编辑河流,如果先前有河流，移除原本河流
		if (riverMode == OptionalToggle.No)
		{
			cell.RemoveRiver();
		}
		//编辑道路,如果先前有，移除原本
		if (roadMode == OptionalToggle.No)
		{
			cell.RemoveRoads();
		}
		if (isDrag)
		{
			print("存在阻力");

			HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
			if (otherCell)
			{
				if (riverMode == OptionalToggle.Yes)
				{
					otherCell.SetOutgoingRiver(dragDirection);
				}
				if (roadMode == OptionalToggle.Yes)
				{
					otherCell.AddRoad(dragDirection);
				}
			}
		}
		//else if (isDrag && riverMode == OptionalToggle.Yes)
		//{
		//	HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
		//	if (otherCell)
		//	{
		//		otherCell.SetOutgoingRiver(dragDirection);
		//	}
		//}

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

	OptionalToggle riverMode, roadMode;

	public void SetRiverMode(int mode)
	{
		riverMode = (OptionalToggle)mode;
	}

	public void SetRoadMode(int mode)
	{
		roadMode = (OptionalToggle)mode;
	}

	public void SetApplyWaterLevel(bool toggle)
	{
		applyWaterLevel = toggle;
	}

	public void SetWaterLevel(float level)
	{
		activeWaterLevel = (int)level;
	}

	public void SetApplyUrbanLevel(bool toggle)
	{
		applyUrbanLevel = toggle;
	}

	public void SetUrbanLevel(float level)
	{
		activeUrbanLevel = (int)level;
	}

	public void SetApplyFarmLevel(bool toggle)
	{
		applyFarmLevel = toggle;
	}

	public void SetFarmLevel(float level)
	{
		activeFarmLevel = (int)level;
	}

	public void SetApplyPlantLevel(bool toggle)
	{
		applyPlantLevel = toggle;
	}

	public void SetPlantLevel(float level)
	{
		activePlantLevel = (int)level;
	}
}