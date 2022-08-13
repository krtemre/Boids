using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoidHelper
{
    const int numOfDirections = 300;
    public static readonly Vector3[] directions;

    static BoidHelper()
    {
        directions = new Vector3[BoidHelper.numOfDirections];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for(int i = 0; i < numOfDirections; i++)
        {
            float t = (float)i / numOfDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float xDir = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float yDir = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float zDir = Mathf.Cos(inclination);

            directions[i] = new Vector3(xDir, yDir, zDir);
        }
    }
}
