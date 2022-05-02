using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace HR_SalesbyMatch
{
    class Result
    {

        /*
        * Complete the 'sockMerchant' function below.
        *
        * The function is expected to return an INTEGER.
        * The function accepts following parameters:
        *  1. INTEGER n
        *  2. INTEGER_ARRAY ar
        */

        public static int sockMerchant(int n, List<int> ar)
        {
            ar.Sort();
            int sumPair = 0;
            int matchPair = 0;
            int currentSock = ar[0];

            for (int a = 0; a < n; a += matchPair)
            {
                if (currentSock > 0 && currentSock < 101)
                {
                    currentSock = ar[a];
                    matchPair = 0;
                    for (int x = 0; x < n; x++)
                    {
                        if (currentSock == ar[x])
                        {
                            matchPair++;
                        }
                    }
                    sumPair += (matchPair / 2);
                }

            }
            return sumPair;
        }

    }
    class Solution
    {
        static void Main(string[] args)
        {
            TextWriter textWriter = new StreamWriter(("c:\\Users\\Tariq Saddler\\RevatureFiles\\TariqSaddlerRevature1_\\cSharpHRChallenges\\HR_SalesbyMatch\\OUTPUT.txt"), true);

            int n = Convert.ToInt32(Console.ReadLine().Trim());
            if (n > 0 && n < 101)
            {
                List<int> ar = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(arTemp => Convert.ToInt32(arTemp)).ToList();
                int result = Result.sockMerchant(n, ar);

                textWriter.WriteLine(result);

                textWriter.Flush();
                textWriter.Close();
            }
        }
    }
}