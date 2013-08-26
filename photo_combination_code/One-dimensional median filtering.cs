using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace photo_combination
{
    class One_dimensional_median_filtering
    {
        public static int[] median_filter(int[] fetupoint)
        {
            int count = 0;
            int j = 0;

            for (int i = 3; i < (fetupoint.Length - 3); i ++ ) 
            {
                count = 0;
                for (j = i - 3; j < i + 3; j++)
                {
                    if (Math.Abs(fetupoint[j] - fetupoint[i]) <= 3)
                    {
                        count = count + 1;
                    }
                }

                if (count <= 1)
                    fetupoint[i] = -1;
            }

            return fetupoint;
        }
    }
}
