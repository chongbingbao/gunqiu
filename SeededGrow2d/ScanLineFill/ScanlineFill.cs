using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SeededGrow2d
{
    class ScanlineFill2d
    {
        protected int count = 0;
        protected Container<Int16Double> container;//这个容器可以是Queue和Stack中任意一种，这里抽象成一个Container
        protected BitMap2d bmp;
        public FlagMap2d flagsMap;
        protected virtual void ExcuteScanlineFill(BitMap2d data, Int16Double seed)
        {
            this.bmp = data;
            data.ResetVisitCount();
            flagsMap = new FlagMap2d(data.width, data.height);
            flagsMap.SetFlagOn(seed.X, seed.Y, true);
            container.Push(seed);
            Process(seed);
            while (!container.Empty())
            {
                Int16Double p = container.Pop();
                int xleft = FindXLeft(p);
                int xright = FindXRight(p);
                if (p.Y - 1 >= 0)
                    CheckRange(xleft, xright, p.Y - 1);
                if (p.Y + 1 < data.height)
                    CheckRange(xleft, xright, p.Y + 1);
            }
        }//该函数为扫描线法主体
        protected void CheckRange(int xleft, int xright, int y)
        {
            for (int i = xleft; i <= xright; )
            {
                if ((!flagsMap.GetFlagOn(i, y)) && IncludePredicate(i, y))
                {
                    int rb = i + 1;
                    while (rb <= xright && (!flagsMap.GetFlagOn(rb, y)) && IncludePredicate(rb, y))
                    {
                        rb++;
                    }
                    rb--;
                    Int16Double t = new Int16Double(rb, y);
                    flagsMap.SetFlagOn(rb, y, true);
                    container.Push(t);
                    Process(t);
                    i = rb + 1;
                }
                else
                {
                    i++;
                }
            }
        }//CheckRange操作
        protected int FindXLeft(Int16Double p)
        {
            int xleft = p.X - 1;
            while (true)
            {
                if (xleft == -1 || flagsMap.GetFlagOn(xleft, p.Y))
                {
                    break;
                }
                else
                {
                    byte value = bmp.GetPixel(xleft, p.Y);
                    if (IncludePredicate(xleft, p.Y))
                    {
                        Int16Double t = new Int16Double(xleft, p.Y);
                        flagsMap.SetFlagOn(xleft, p.Y, true);
                        Process(t);
                        xleft--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return xleft + 1;
        }//FindXLeft操作
        protected int FindXRight(Int16Double p)
        {
            int xright = p.X + 1;
            while (true)
            {
                if (xright == bmp.width || flagsMap.GetFlagOn(xright, p.Y))
                {
                    break;
                }
                else
                {
                    byte value = bmp.GetPixel(xright, p.Y);
                    if (IncludePredicate(xright, p.Y))
                    {
                        Int16Double t = new Int16Double(xright, p.Y);
                        flagsMap.SetFlagOn(xright, p.Y, true);
                        Process(t);
                        xright++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return xright - 1;
        }//FindXRight操作
        protected bool IncludePredicate(int x, int y)
        {
            return bmp.GetPixel(x, y) == BitMap2d.WHITE;
        }
        protected void Process(Int16Double p)
        {
            count++;
        }
    }
    class ScanlineFill2d_T : ScanlineFill2d
    {
        public Stopwatch watch = new Stopwatch();
        public ResultReport report;
        public  void ExcuteScanlineFill_Queue(BitMap2d data, Int16Double seed)
        {
            watch.Start();
            container = new Container_Queue<Int16Double>();
            base.ExcuteScanlineFill(data, seed);
            watch.Stop();
            report.time = watch.ElapsedMilliseconds;
            report.result_point_count = count;
            report.bmp_get_count = data.action_get_count;
            report.bmp_set_count = data.action_set_count;
            report.container_pop_count = container.action_pop_count;
            report.container_push_count = container.action_push_count;
            report.container_max_size = container.max_contain_count;
            report.flag_get_count = flagsMap.action_get_count;
            report.flag_set_count = flagsMap.action_set_count;
            return;
        }
        public  void ExcuteScanlineFill_Stack(BitMap2d data, Int16Double seed)
        {
            watch.Start();
            container = new Container_Stack<Int16Double>();
            base.ExcuteScanlineFill(data, seed);
            watch.Stop();
            report.time = watch.ElapsedMilliseconds;
            report.result_point_count = count;
            report.bmp_get_count = data.action_get_count;
            report.bmp_set_count = data.action_set_count;
            report.container_pop_count = container.action_pop_count;
            report.container_push_count = container.action_push_count;
            report.container_max_size = container.max_contain_count;
            report.flag_get_count = flagsMap.action_get_count;
            report.flag_set_count = flagsMap.action_set_count;
            return;
        }
    }
}
