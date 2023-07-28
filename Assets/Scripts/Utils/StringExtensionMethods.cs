using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class StringExtensionMethods
{
  public static int GetStableHashCode(this string str)
  {
    int num1 = 5381;
    int num2 = num1;
    for (int index = 0; index < str.Length && str[index] != char.MinValue; index += 2)
    {
      num1 = (num1 << 5) + num1 ^ (int) str[index];
      if (index != str.Length - 1 && str[index + 1] != char.MinValue)
        num2 = (num2 << 5) + num2 ^ (int) str[index + 1];
      else
        break;
    }
    return num1 + num2 * 1566083941;
  }

  public static int[] AllIndicesOf(this string thisString, string substring)
  {
    List<int> intList = new List<int>();
    if (string.IsNullOrEmpty(substring))
      return intList.ToArray();
    for (int index1 = 0; index1 < thisString.Length - substring.Length + 1; ++index1)
    {
      bool flag = true;
      for (int index2 = 0; index2 < substring.Length; ++index2)
      {
        if ((int) thisString[index1 + index2] != (int) substring[index2])
        {
          flag = false;
          break;
        }
      }
      if (flag)
        intList.Add(index1);
    }
    return intList.ToArray();
  }

  public static int[] AllIndicesOf(this string thisString, char target)
  {
    List<int> intList = new List<int>();
    for (int index = 0; index < thisString.Length; ++index)
    {
      if ((int) thisString[index] == (int) target)
        intList.Add(index);
    }
    return intList.ToArray();
  }

  public static string RemoveRichTextTags(this string text) => Regex.Replace(text, "<[\\/a-zA-Z0-9= \"\\\"''#;:()$_-]*?>", string.Empty);
}
