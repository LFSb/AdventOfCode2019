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

  private static string OutputResult(string part1 = string.Empty, string part2 = string.Empty)
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
    var input = Initialize(12, 2);

    ProcessInput(input);

    var p1 = input[0];

    var noun = 0;
    var verb = 0;

    var p2 = input[0];

    var target = 19690720;

    while (true)
    {
      noun++;

      input = Initialize(noun, verb);

      ProcessInput(input);

      p2 = input[0];

      var diff = target - p2;

      if (diff < 120) //The verb basically just adds its value to the end result. It has a max of 120 though. If we find a value that's within 120 of the target, the verb is the difference.
      {
        verb = diff;
        break;
      }
    }

    return OutputResult(p1.ToString(), (100 * noun + verb).ToString());
  }

  private static int[] Initialize(int noun, int verb)
  {
    var input = File.ReadAllText(Path.Combine(InputBasePath, "Day2.txt")).Split(',').Select(z => int.Parse(z)).ToArray();

    //Set initial values
    input[1] = noun;
    input[2] = verb;

    return input;
  }

  private static int[] ProcessInput(int[] input)
  {
    for (var position = 0; position < input.Length;)
    {
      switch (input[position])
      {
        case 1:
          {
            Process(input, position, false);

            position += 4;
            //Add position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 2:
          {
            Process(input, position, true);
            position += 4;
            //Multiply position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 99:
          {
            position = input.Length;
          }
          break;
        default:
          {
            throw new Exception($"At position {position}: Opcode {input[position]}");
          }
      }
    }

    return input;
  }

  private static void Process(int[] input, int position, bool multiply)
  {
    int output;

    if (multiply)
    {
      output = input[input[position + 1]] * input[input[position + 2]];
    }
    else
    {
      output = input[input[position + 1]] + input[input[position + 2]];
    }

    var target = input[position + 3];

    input[target] = output;
  }

  #endregion

  public static string Day3()
  {
    return OutputResult();
  }
}