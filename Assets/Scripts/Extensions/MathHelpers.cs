using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelpers
{
    public static double GetBellCurvePoint(double Percentage, double Midpoint)
    {
        return (Percentage - ((1 - Percentage) * Percentage)) * (1 / Midpoint);
    }
}