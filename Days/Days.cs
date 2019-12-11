using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public static partial class Days {
  private const string InputBasePath = @"Days/Input/";

  private static string OutputResult (string part1 = "", string part2 = "") {
    return $"{Environment.NewLine}- Part 1: {part1}{Environment.NewLine}- Part 2: {part2}";
  }

  #region Day1

  public static string Day1 () {
    var input = File.ReadAllLines (Path.Combine (InputBasePath, "Day1.txt"));
    var test = new [] { "100756" };

    decimal p1 = 0;

    foreach (var mass in input) {
      var fuel = Math.Floor ((decimal) (int.Parse (mass) / 3)) - 2;

      p1 += fuel;
    }

    decimal p2 = 0;

    foreach (var mass in input) {
      var fuel = Math.Floor ((decimal) (int.Parse (mass) / 3)) - 2;

      p2 += fuel;

      while (fuel > 0) {
        fuel = Math.Floor ((decimal) (fuel / 3)) - 2;

        if (fuel > 0)
          p2 += fuel;
      }
    }

    return OutputResult (p1.ToString (), p2.ToString ());
  }

  #endregion

  #region Day2

  public static string Day2 () {
    var input = Initialize (12, 2);

    ProcessInput (input);

    var p1 = input[0];

    var noun = 0;
    var verb = 0;

    var p2 = input[0];

    var target = 19690720;

    while (true) {
      noun++;

      input = Initialize (noun, verb);

      ProcessInput (input);

      p2 = input[0];

      var diff = target - p2;

      if (diff < 120) //The verb basically just adds its value to the end result. It has a max of 120 though. If we find a value that's within 120 of the target, the verb is the difference.
      {
        verb = diff;
        break;
      }
    }

    return OutputResult (p1.ToString (), (100 * noun + verb).ToString ());
  }

  private static int[] Initialize (int noun, int verb) {
    var input = File.ReadAllText (Path.Combine (InputBasePath, "Day2.txt")).Split (',').Select (z => int.Parse (z)).ToArray ();

    //Set initial values
    input[1] = noun;
    input[2] = verb;

    return input;
  }

  private static int[] ProcessInput (int[] input) {
    for (var position = 0; position < input.Length;) {
      switch (input[position]) {
        case 1:
          {
            Process (input, position, false);

            position += 4;
            //Add position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 2:
          {
            Process (input, position, true);
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
            throw new Exception ($"At position {position}: Opcode {input[position]}");
          }
      }
    }

    return input;
  }

  private static void Process (int[] input, int position, bool multiply) {
    int output;

    if (multiply) {
      output = input[input[position + 1]] * input[input[position + 2]];
    } else {
      output = input[input[position + 1]] + input[input[position + 2]];
    }

    var target = input[position + 3];

    input[target] = output;
  }

  #endregion

  public static string Day3 () {
    var places = new List<Tuple<int, int>> { new Tuple<int, int> (0, 0) };
    var p1 = int.MaxValue;

    var lines = new [] { "R8,U5,L5,D3", "U7,R6,D4,L4" };

    //var lines = File.ReadAllLines(Path.Combine(InputBasePath, "Day3.txt"));

    foreach (var inputs in lines) {
      var x = 0;
      var y = 0;

      foreach (var input in inputs.Split (',')) {
        Process (ref x, ref y, input, places, out var intersection);

        if (intersection != null) {
          System.Console.WriteLine ($"Intersection at {intersection}!, {input}");

          p1 = Math.Min (Math.Abs (intersection.Item1) + Math.Abs (intersection.Item2), p1);

          System.Console.WriteLine ($"P1 is now {p1}");
        }

        // System.Console.WriteLine($"Current position = x: {x}, y: {y}");
      }

      // System.Console.WriteLine($"Final position = x: {x}, y: {y}");
    }

    Visualize (places);

    return OutputResult (p1.ToString ());
  }

  private static void Visualize (List<Tuple<int, int>> places) {
    var xcoords = places.Select (x => x.Item1).Distinct ();
    var ycoords = places.Select (x => x.Item2).Distinct ();

    var dimension = new Tuple<int, int> (xcoords.Max () - xcoords.Min (), ycoords.Max () - xcoords.Min ());

    var grid = new char[dimension.Item1][];

    for (var x = 0; x < grid.Length; x++) {
      grid[x] = new char[dimension.Item2];

      for (var y = 0; y < grid[x].Length; y++) {
        if (places.Contains (new Tuple<int, int> (x, y))) {
          if (grid[x][y] == 'o') {
            grid[x][y] = 'X';
          } else if (x == 0 && y == 0) {
            grid[x][y] = 'S';
          } else {
            grid[x][y] = 'o';
          }
        }
      }
    }

    for (var x = grid.Length - 1; x > -1; x--) {
      for (var y = 0; y < grid[x].Length; y++) {
        Console.Write (grid[x][y]);
      }
      Console.WriteLine ();
    }
  }

  private static void Process (ref int x, ref int y, string input, List<Tuple<int, int>> beenPlaces, out Tuple<int, int> intersection) {
    var distance = int.Parse (input.Substring (1));

    intersection = null;

    for (var step = distance; step > 0; step--) {
      switch (input[0]) {
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

      var current = new Tuple<int, int> (x, y);

      if (beenPlaces.Contains (current)) {
        intersection = current;
      } else {
        beenPlaces.Add (current);
      }
    }
  }
}