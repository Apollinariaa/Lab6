using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_6_cs
{
    [Serializable]
    public class TimeItem
    {
        public int n { get; set; }
        public int repeat { get; set; }
        public double sh_lb { get; set; }
        public double ss { get; set; }
        public double k { get { return sh_lb / ss; } }

        public TimeItem(int n, int repeat, double sh_lb, double ss)
        {
            this.n = n;
            this.repeat = repeat;
            this.sh_lb = sh_lb;
            this.ss = ss;
        }


        public override string ToString()
        {
            return n + "\t "
                + repeat + "\t"
                + sh_lb + "\t"
                + ss + "\t"
                + k + "\t";
        }
    }
}