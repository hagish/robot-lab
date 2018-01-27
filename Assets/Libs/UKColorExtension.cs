using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public static class UKColorExtension
{
    public static Color SetR(this Color c, float r)
    {
        c.r = r;
        return c;
    }

    public static Color SetG(this Color c, float g)
    {
        c.g = g;
        return c;
    }

    public static Color SetB(this Color c, float b)
    {
        c.b = b;
        return c;
    }

    public static Color SetA(this Color c, float a)
    {
        c.a = a;
        return c;
    }
}
