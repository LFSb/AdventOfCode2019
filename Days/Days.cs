using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public static partial class Days
{
  private const string InputBasePath = @"Days/Input/";

  private static string OutputResult(string part1 = "", string part2 = "")
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

  #region Day3

  //Take two lines of input. Each line details a wire and the direction it is heading. The directions are delimited by a ','. The direction itself contains the direction, and the amount of steps in that direction you should take. So R8 means go 8 steps to the right. U20 means 20 steps up.
  //Basically, we are walking a grid, with an X axis and a Y axis. Going Right means adding one to the X axis. Going up means adding one to the Y axis. Left and Down mean detracting from the X and Y axis respectively.
  //We should check where the two wires (two lines) are intersecting. We should then print out the Manhattan distance (which is the combined absolute values of the X and Y axis) of the colission that is closest to the beginning.
  //In other words, all the collisions will have a manhattan distance, and we need the lowest one. Print that out.
  //Game rules: A wire cannot intersect itself. Also, technically both wires cross at 0,0 as they both start from there, but that one doesn't count.
  public static string Day3()
  {
    var places = new Dictionary<Tuple<int, int>, int> { { new Tuple<int, int>(0, 0), 1 } };
    var target = new Dictionary<Tuple<int, int>, int>();

    //var lines = new[] { "R8,U5,L5,D3", "U7,R6,D4,L4" };

    //var lines = new[] { "R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83" };

    var lines = new[] { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" };

    //var lines = File.ReadAllLines(Path.Combine(InputBasePath, "Day3.txt"));

    foreach (var inputs in lines)
    {
      var x = 0;
      var y = 0;

      foreach (var input in inputs.Split(','))
      {
        Process(ref x, ref y, input, places, target);
      }

      target = places;
    }

    //Visualize(places);

    var colissions = target.Where(p => p.Value > 1).Select(s => Math.Abs(s.Key.Item1) + Math.Abs(s.Key.Item2)).OrderBy(v => v);

    var p1 = colissions.First();

    return OutputResult(p1.ToString());
  }

  private static void Process(ref int x, ref int y, string input, Dictionary<Tuple<int, int>, int> beenPlaces, Dictionary<Tuple<int, int>, int> target)
  {
    var distance = int.Parse(input.Substring(1));

    for (var step = distance; step > 0; step--)
    {
      switch (input[0])
      {
        case 'U':
          {
            y++;
          }
          break;
        case 'R':
          {
            x++;
          }
          break;
        case 'D':
          {
            y--;
          }
          break;
        case 'L':
          {
            x--;
          }
          break;
      }

      var current = new Tuple<int, int>(x, y);

      if (target.ContainsKey(current))
      {
        target[current]++;
        System.Console.WriteLine($"{input} led to a collision at {current}! Distance: {Math.Abs(current.Item1) + Math.Abs(current.Item2)}");
      }
      else if (beenPlaces.ContainsKey(current))
      {
        System.Console.WriteLine($"{input} led to a collision at {current}! However, it is intersecting with itself.");
      }
      else
      {
        beenPlaces.Add(current, 1);
      }
    }
  }


  private static void Visualize(Dictionary<Tuple<int, int>, int> places)
  {
    var xcoords = places.Select(x => x.Key.Item1).Distinct();
    var ycoords = places.Select(x => x.Key.Item2).Distinct();

    var dimension = new Tuple<int, int>((xcoords.Max() - xcoords.Min()) + 1, (ycoords.Max() - xcoords.Min()) + 1);

    var grid = new char[dimension.Item1][];

    for (var x = 0; x < grid.Length; x++)
    {
      grid[x] = new char[dimension.Item2];

      for (var y = 0; y < grid[x].Length; y++)
      {
        var current = new Tuple<int, int>(x, y);

        if (places.ContainsKey(current))
        {
          if (places[current] > 1)
          {
            grid[x][y] = 'X';
          }
          else if (x == 0 && y == 0)
          {
            grid[x][y] = '0';
          }
          else
          {
            grid[x][y] = 'o';
          }
        }
        else
        {
          grid[x][y] = '.';
        }
      }
    }

    for (var x = grid.Length - 1; x > -1; x--)
    {
      for (var y = 0; y < grid[x].Length; y++)
      {
        Console.Write(grid[x][y]);
      }
      Console.WriteLine();
    }
  }

  #endregion

  #region Day4
  public static string Day4()
  {
    var candidates = new List<int>();

    int lower = 138307;
    int upper = 654504;

    for (var i = lower; i <= upper; i++)
    {
      if (TestP1(i))
      {
        candidates.Add(i);
        //System.Console.WriteLine($"Found one! {i} added to the list. We're at {candidates.Distinct().Count()} now.");
      }
    }

    var p1 = candidates.Distinct().Count().ToString();

    candidates = new List<int>();

    for (var i = lower; i <= upper; i++)
    {
      if (TestP2(i))
      {
        candidates.Add(i);
        System.Console.WriteLine($"Found one! {i} added to the list. We're at {candidates.Distinct().Count()} now.");
      }
    }

    var p2 = candidates.Distinct().Count().ToString();

    return OutputResult(p1, p2);
  }

  private static bool TestP1(int i)
  {
    var increasing = true;
    var doubles = false;

    var text = i.ToString();

    for (var idx = 0; idx < text.Length; idx++)
    {
      var current = (int)text[idx];

      if (idx + 1 == text.Length)
      {
        break;
      }

      var next = (int)text[idx + 1];

      if (next < current)
        increasing = false;

      if (next == current)
      {
        doubles = true;
      }
    }

    return increasing && doubles;
  }

  private static bool TestP2(int i)
  {
    var increasing = true;
    var doubles = false;

    var text = i.ToString();

    for (var idx = 0; idx < text.Length; idx++)
    {
      var current = (int)text[idx];

      if (idx + 1 == text.Length)
      {
        break;
      }

      var next = (int)text[idx + 1];

      var nextNext = idx + 2 < text.Length ? (int)text[idx + 2] : -1;
      var previous = idx - 1 >= 0 ? (int)text[idx -1] : -1;

      if (next < current)
        increasing = false;

      if (next == current) //We got a match, now we need to make sure that it isn't part of a larger group.
      {
        if (nextNext != current && previous != current)
        {
          doubles = true;
        }
      }
    }

    return increasing && doubles;
  }

  #endregion


}