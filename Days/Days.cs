using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

public static partial class Days
{
  private const string InputBasePath = @"Days/Input/";

  private static string OutputResult(string part1, string part2)
  {
    return $"{Environment.NewLine}- Part 1: {part1}{Environment.NewLine}- Part 2: {part2}";
  }

  #region Day1

  public static string Day1()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day1.txt"));
    var test = new[] { "100756" };

    decimal p1 = 0;

    foreach (var mass in input)
    {
      var fuel = Math.Floor((decimal)(int.Parse(mass) / 3)) - 2;

      p1 += fuel;
    }

    decimal p2 = 0;

    foreach (var mass in input)
    {
      var fuel = Math.Floor((decimal)(int.Parse(mass) / 3)) - 2;

      p2 += fuel;

      while (fuel > 0)
      {
        fuel = Math.Floor((decimal)(fuel / 3)) - 2;

        if (fuel > 0)
          p2 += fuel;
      }
    }

    return OutputResult(p1.ToString(), p2.ToString());
  }

  #endregion

  #region Day2

  public static string Day2()
  {
    var testInput = new[] { 1,1,1,4,99,5,6,0,99};

    for (var position = 0; position < testInput.Length;)
    {
      switch (testInput[position])
      {
        case 1:
          {
            var output = testInput[testInput[position + 1]] + testInput[testInput[position + 2]];
            var target = testInput[position + 3];
            position += 4;
            // System.Console.WriteLine($"Saving {output} at {target}.");
            testInput[target] = output;
            //Add position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 2:
          {
            var output = testInput[testInput[position + 1]] * testInput[testInput[position + 2]];
            var target = testInput[position + 3];
            position += 4;
            // System.Console.WriteLine($"Saving {output} at {target}.");

            testInput[target] = output;
            //Multiply position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 99:
          {
            position = testInput.Length;
          }break;
      }
      
      // System.Console.WriteLine($"Position is {position}.");
      // if(position < testInput.Length)
      // {
      //   System.Console.WriteLine($"Opcode: {testInput[position]}");
      // }
      
    }

    System.Console.WriteLine(string.Join(",", testInput));

    return OutputResult("", "");
  }

  #endregion

}