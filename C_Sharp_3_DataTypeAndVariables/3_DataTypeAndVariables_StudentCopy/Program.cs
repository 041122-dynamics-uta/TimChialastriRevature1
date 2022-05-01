using System;

namespace _3_DataTypeAndVariablesChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            byte b = 255;
            sbyte sb = -128;
            int num = 1;
            uint ui = 0;
            short s = -32768;
            ushort us = 0;
            long l = -9223372036854775808;
            ulong ul = 0;
            float f = (float)(Math.Pow(3.4,38));
            double d = Math.Pow(-1.79769313486232,308);
            char ch = 'c';
            bool isValid = false;
            object j = null;
            string name = "Hello World";
            decimal dec = (decimal)(7.9 * Math.Pow(10,28));

            Console.WriteLine(PrintValues(num));
            Console.WriteLine(PrintValues(b));
            Console.WriteLine(PrintValues(sb));
            Console.WriteLine(PrintValues(ui));
            Console.WriteLine(PrintValues(s));
            Console.WriteLine(PrintValues(us));
            Console.WriteLine(PrintValues(l));
            Console.WriteLine(PrintValues(ul));
            Console.WriteLine(PrintValues(ch));
            Console.WriteLine(PrintValues(d));
            Console.WriteLine(PrintValues(f));
            Console.WriteLine(PrintValues(isValid));
            Console.WriteLine(PrintValues(j));
            Console.WriteLine(PrintValues(name));
            Console.WriteLine(PrintValues(dec));
            
        

        }

        /// <summary>
        /// This method has an 'object' type parameter. 
        /// 1. Create a switch statement that evaluates for the data type of the parameter
        /// 2. You will need to get the data type of the parameter in the 'case' part of the switch statement
        /// 3. For each data type, return a string of exactly format, "Data type => <type>" 
        /// 4. For example, an 'int' data type will return ,"Data type => int",
        /// 5. A 'ulong' data type will return "Data type => ulong",
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string PrintValues(object obj)
        {
          Console.WriteLine(obj.GetType() + "This is the string");
          //  throw new NotImplementedException($"PrintValues() has not been implemented");
          // string s = "";
          switch (obj) {
              case string s:
                 s = "Data type => String";
                 return s;
              case string b:
                 b = "Data type => String";
                 return b;
              case int num:
                 num = "Data type => int";
                 return num;
              case long ul:
                 ul = "Data type => ulong";
                 return ul;
              case "byte":
                 s = "Data type => byte";
                 return s;
              case sbyte sb":
                 sb = "Data type => sbyte";
                 return sb;
              case uint ui:
                 ui = "Data type => uint";
                 return ui;
              case short s:
                 s = "Data type => short";
                 return s;
              case ushort us:
                 us = "Data type => ushort";
                 return us;
              case long l:
                 l = "Data type => long";
                 return l;
              case float f":
                 f = "Data type => float";
                 return f;
              case double d:
                 d = "Data type => double";
                 return d;
              case char ch:
                 ch = "Data type => char";
                 return ch;
              case bool isValid:
                 isValid = "Data type => bool";
                 return isValid;
              case Object j:
                 j = "Data type => object";
                 return j;
              case decimal dec:
                 dec = "Data type => decimal";
                 return dec; 
              default:
                 break; 
          }
            Console.WriteLine(obj.ToString());
          return "";
        }

        /// <summary>
        /// THis method has a string parameter.
        /// 1. Use the .TryParse() method of the Int32 class (Int32.TryParse()) to convert the string parameter to an integer. 
        /// 2. You'll need to investigate how .TryParse() and 'out' parameters work to implement the body of StringToInt().
        /// 3. If the string cannot be converted to a integer, return 'null'. 
        /// 4. Investigate how to use '?' to make non-nullable types nullable.
        /// </summary>
        /// <param name="numString"></param>
        /// <returns></returns>
        public static int? StringToInt(string numString)
        {
            int i;
            bool mybool = Int32.TryParse(numString, out i);
            
            if (mybool)
            {
            return i;
            }
            else {
               return null;
            }
        }
    }// end of class
}// End of Namespace
