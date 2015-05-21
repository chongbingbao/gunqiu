﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OctreeTest
{
public struct Int32Quad
{
    public int A;
    public int B;
    public int C;
    public int D;
    public Int32Quad(int a, int b, int c, int d)
    {
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;
    }
    public override string ToString()
    {
        return string.Format("{0}x+{1}y+{2}z={3}", A, B, C, D);
    }
}//代表一个平面方程对象
public class OctreeTable
{
    public static byte[] ConfigToNormalTypeId = new byte[256]
    {
        13,0,1,2,3,13,4,13,5,6,13,13,7,13,13,8,3,9,13,13,13,13,13,13,13,13,13,0,13,13,13,13,5,13,10,13,13,13
        ,13,1,13,13,13,13,13,13,13,13,7,13,13,11,13,13,13,13,13,13,13,13,13,13,13,2,0,13,13,13,9,13,13,13,13
        ,13,13,13,13,13,3,13,13,13,13,13,13,13,13,13,13,13,
        13,13,13,13,13,13,6,13,13,13,13,13,12,13,13,13,13,13,13,13,13,4,13,13,5,13,13,13,13,10,13,13,13,13
        ,13,13,13,1,1,13,13,13,13,13,13,13,10,13,13,13,13,5,13,
        13,4,13,13,13,13,13,13,13,13,12,13,13,13,13,13,6,13,13,13,13,13,13,
        13,13,13,13,13,13,13,13,13,13,13,3,13,13,13,13,13,13,13,13,13,9,13,13,13,0,2,13,13,13,
        13,13,13,13,13,13,13,13,11,13,13,7,13,13,13,13,13,13,13,13,1,13,13,13,13,10,13,5,13,13,
        13,13,0,13,13,13,13,13,13,13,13,13,9,3,8,13,13,7,13,13,6,5,13,4,13,3,2,1,0,13
    };//体元配置对应的法向量索引
    public static Int16Triple[] NormalTypeIdToNormal = new Int16Triple[13]
    {
        new Int16Triple(1,-1,-1),
        new Int16Triple(1,-1,1),
        new Int16Triple(1,-1,0),
        new Int16Triple(1,1,1),
        new Int16Triple(1,0,1),
        new Int16Triple(1,1,-1),
        new Int16Triple(1,0,-1),
        new Int16Triple(1,1,0),
        new Int16Triple(1,0,0),
        new Int16Triple(0,1,1),
        new Int16Triple(0,1,-1),
        new Int16Triple(0,1,0),
        new Int16Triple(0,0,1)
    };//枚举的法向量集合

    public static byte[] ConfigToEqType = new byte[256]
    {
        55,0,1,2,3,55,4,55,5,6,55,55,7,55,
        55,8,9,10,55,55,55,55,55,55,55,
        55,55,11,55,55,55,55,12,55,13,55,
        55,55,55,14,55,55,55,55,55,55,55
        ,55,15,55,55,16,55,55,55,55,55,55
        ,55,55,55,55,55,17,18,55,55,55,19,
        55,55,55,55,55,55,55,55,55,20,55,
        55,55,55,55,55,55,55,55,55,55,55,
        55,55,55,55,0,21,55,55,55,55,55,22,
        55,55,55,55,55,55,55,55,23,55,55,24,55,
        55,55,55,25,55,55,55,0,55,0,0,26,27,55,55,
        55,55,55,55,55,28,55,55,55,55,29,55,55,
        30,55,55,55,55,55,55,55,55,31,55,55,55,
        55,55,32,55,55,55,55,55,55,55,55,55,55,
        55,55,55,55,55,0,55,33,55,55,55,55,
        55,0,55,55,55,34,55,0,0,35,36,55,55,55,
        55,55,55,55,55,55,55,55,37,55,55,38,55,
        55,55,55,55,55,55,0,39,55,55,0,55,40,0,41,55,
        55,55,55,42,55,55,0,55,55,55,0,55,0,43,44,45,55
        ,55,46,55,0,47,48,55,49,0,50,51,52,53,55
    };//体元配置对应的方程索引，55表示非共面配置
    public static Int32Quad[] EqTypeToEqQuad = new Int32Quad[54]
    {
        new Int32Quad(1,-1,-1,-1),
        new Int32Quad(1,-1,1,0),
        new Int32Quad(1,-1,0,0),
        new Int32Quad(1,1,1,1),
        new Int32Quad(1,0,1,1),
        new Int32Quad(1,1,-1,0),
        new Int32Quad(1,0,-1,0),
        new Int32Quad(1,1,0,1),
        new Int32Quad(1,0,0,1),
        new Int32Quad(1,1,1,2),
        new Int32Quad(0,1,1,1),
        new Int32Quad(1,-1,-1,0),
        new Int32Quad(1,1,-1,1),
        new Int32Quad(0,1,-1,0),
        new Int32Quad(1,-1,1,1),
        new Int32Quad(1,1,0,1),
        new Int32Quad(0,1,0,0),
        new Int32Quad(1,-1,0,1),
        new Int32Quad(1,-1,-1,0),
        new Int32Quad(0,1,1,1),
        new Int32Quad(1,1,1,2),
        new Int32Quad(1,0,-1,0),
        new Int32Quad(0,0,1,1),
        new Int32Quad(1,0,1,2),
        new Int32Quad(1,1,-1,0),
        new Int32Quad(0,1,-1,-1),
        new Int32Quad(1,-1,1,2),
        new Int32Quad(1,-1,1,1),
        new Int32Quad(0,1,-1,0),
        new Int32Quad(1,1,-1,1),
        new Int32Quad(1,0,1,1),
        new Int32Quad(0,0,1,0),
        new Int32Quad(1,0,-1,1),
        new Int32Quad(1,1,1,1),
        new Int32Quad(0,1,1,0),
        new Int32Quad(1,-1,-1,1),
        new Int32Quad(1,-1,0,0),
        new Int32Quad(0,1,0,1),
        new Int32Quad(1,1,0,2),
        new Int32Quad(1,-1,1,0),
        new Int32Quad(0,1,-1,1),
        new Int32Quad(1,1,-1,2),
        new Int32Quad(1,-1,-1,-1),
        new Int32Quad(0,1,1,2),
        new Int32Quad(1,1,1,3),
        new Int32Quad(1,0,0,0),
        new Int32Quad(1,1,0,0),
        new Int32Quad(1,0,-1,-1),
        new Int32Quad(1,1,-1,-1),
        new Int32Quad(1,0,1,0),
        new Int32Quad(1,1,1,0),
        new Int32Quad(1,-1,0,-1),
        new Int32Quad(1,-1,1,-1),
        new Int32Quad(1,-1,-1,-2),
    };//体元配置对应的三角片方程集合
    public const byte NormalNotSimple = 13;

       
}
}
