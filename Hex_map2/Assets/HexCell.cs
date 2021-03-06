﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;

    public Color color;

    [SerializeField]
    HexCell[] neighbors;


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


}
