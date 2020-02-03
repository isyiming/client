using UnityEngine;

public class HexMapEditor : MonoBehaviour
{

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;



	void Awake()
	{
		SelectColor(0);
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
            //鼠标点击时，增加的单元格高度
            activeElevation ++;

            HandleInput();
		}
	}

    //鼠标点击单元格更改单元格颜色
	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)){
			EditCell(hexGrid.GetCell(hit.point));
		}
	}

	public void SelectColor(int index)
	{
		activeColor = colors[index];
	}

	int activeElevation;

	void EditCell(HexCell cell)
	{
		cell.color = activeColor;
		cell.Elevation = activeElevation;
		//hexGrid.Refresh();
	}

    //设定单元格高度范围
	public void SetElevation(float elevation)
	{
		activeElevation = (int)elevation;
	}


}