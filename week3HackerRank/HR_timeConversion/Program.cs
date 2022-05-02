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

class Result
{

    /*
     * Complete the 'timeConversion' function below.
     *
     * The function is expected to return a STRING.
     * The function accepts STRING s as parameter.
     */

    public static string timeConversion(string s)
    {
        int Hour = Int32.Parse(s.Substring(0, 2));
        int imilitaryHour = Hour + 12;
        string stmilitaryHour = imilitaryHour.ToString();
        string militaryTime = s.Substring(2, 6);

        if (s.Substring(8) == "AM" && s.Substring(0, 2) == "12")
        {
            militaryTime = militaryTime.Insert(0, "00");
        }

        else if (s.Substring(8) == "PM" && s.Substring(0, 2) != "12")
        {
            if (s.Substring(0, 1) == "0")
            {
                Hour = Int32.Parse(s.Substring(1, 1));
                imilitaryHour = Hour + 12;
                stmilitaryHour = imilitaryHour.ToString();
            }
            if (Hour < 12)
            {
                militaryTime = militaryTime.Insert(0, stmilitaryHour);
            }
        }

        else
        {
            militaryTime = militaryTime.Insert(0, s.Substring(0, 2));
        }

        return militaryTime;

    }

}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        string s = Console.ReadLine();

        string result = Result.timeConversion(s);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}