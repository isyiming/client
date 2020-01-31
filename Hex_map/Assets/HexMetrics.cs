using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    //定义六边形的外接圆和内切圆半径
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;

    //定义六边形六个角相对于中心的坐标
    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };
}