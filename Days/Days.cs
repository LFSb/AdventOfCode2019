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
    var input = File.ReadAllText(Path.Combine(InputBasePath, "Day2.txt")).Split(',').Select(z => int.Parse(z)).ToArray();

    //Set initial values
    input[1] = 12;
    input[2] = 2;

    for (var position = 0; position < input.Length;)
    {
      System.Console.WriteLine(string.Join(",", input));

      switch (input[position])
      {
        case 1:
          {
            var output = input[input[position + 1]] + input[input[position + 2]];
            var target = input[position + 3];
            position += 4;
            
            // System.Console.WriteLine($"Saving {output} at {target}.");
            
            input[target] = output;
            //Add position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 2:
          {
            var output = input[input[position + 1]] * input[input[position + 2]];
            var target = input[position + 3];
            position += 4;
            
            // System.Console.WriteLine($"Saving {output} at {target}.");

            input[target] = output;
            //Multiply position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 99:
          {
            position = input.Length;
          }break;
        default:
        {
          throw new Exception($"At position {position}: Opcode {input[position]}");
        }
      }
      
      // System.Console.WriteLine($"Position is {position}.");
      // if(position < input.Length)
      // {
      //   System.Console.WriteLine($"Opcode: {input[position]}");
      // }      
    }   

    return OutputResult(input[0].ToString(), "");
  }

  #endregion

}