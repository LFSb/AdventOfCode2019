using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    ProcessOpCode(input);

    var p1 = input[0];

    var noun = 0;
    var verb = 0;

    var p2 = input[0];

    var target = 19690720;

    while (true)
    {
      noun++;

      input = Initialize(noun, verb);

      ProcessOpCode(input);

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

  private static int[] ProcessOpCode(int[] input)
  {
    for (var position = 0; position < input.Length;)
    {
      switch (input[position])
      {
        case 1:
          {
            ProcessDay2(input, position, false);

            position += 4;
            //Add position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 2:
          {
            ProcessDay2(input, position, true);
            position += 4;
            //Multiply position+1 and position+2 together, store the output at the position specified in position+3
          }
          break;
        case 3:
        {
          ProcessDay5(input, position);
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

  private static void ProcessDay5(int[] input, int position)
  {
    System.Console.WriteLine("Please enter Input: ");
    var inputValue = int.Parse(Console.ReadLine());
  }

  private static void ProcessDay2(int[] input, int position, bool multiply)
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

  private class Point 
  {
    public Point(int x, int y, int steps)
    {
      X = x;
      Y = y;
      Steps = steps;
    }

    public int X { get; private set; }

    public int Y { get; private set; }

    public int Steps { get; private set; }    
  }

  private class PointComparer : IEqualityComparer<Point>
  {
    public bool Equals(Point x, Point y)
    {
      return x.X == y.X && x.Y == y.Y;
    }

    public int GetHashCode(Point point)
    {
      return (point.X + point.Y).GetHashCode();
    }
  }
  

  public static string Day3()
  {
    //var lines = new[] { "R8,U5,L5,D3", "U7,R6,D4,L4" };

    //var lines = new[] { "R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83" };

    //var lines = new[] { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" };

    var lines = File.ReadAllLines(Path.Combine(InputBasePath, "Day3.txt"));

    var pointsLists = new List<List<Point>>();

    foreach (var inputs in lines)
    {
      var x = 0;
      var y = 0;
      var steps = 0;

      var traveledPoints = new List<Point>();

      foreach (var input in inputs.Split(','))
      {
        traveledPoints.AddRange(Process(ref steps, ref x, ref y, input));
      }

      pointsLists.Add(traveledPoints);
    }

    var firstLine = pointsLists.First();
    var secondLine = pointsLists.Last();
    var comparer = new PointComparer();
    
    var intersected = secondLine.Intersect(firstLine, comparer);

    var colissions =  intersected.Select(s => Math.Abs(s.X) + Math.Abs(s.Y)).OrderBy(v => v);

    var p1 = colissions.First();

    var p2 = int.MaxValue;

    foreach(var intersection in intersected)
    {
      var firstLineSteps = firstLine.First(x => comparer.Equals(x, intersection)).Steps;
      var secondLineSteps = secondLine.First(x => comparer.Equals(x, intersection)).Steps;

      p2 = Math.Min(firstLineSteps + secondLineSteps, p2);
    }

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private static List<Point> Process(ref int steps, ref int x, ref int y, string input)
  {
    var traveledPoints = new List<Point>();

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

      traveledPoints.Add(new Point(x, y, ++steps));
    }

    return traveledPoints;
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
        //System.Console.WriteLine($"Found one! {i} added to the list. We're at {candidates.Distinct().Count()} now.");
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

  #region Day5
  public static string Day5()
  {
    return OutputResult();
  }

  private static void ProcessDay5(int[] input, int position, bool immediate)
  {
    throw new NotImplementedException();
  }

  #endregion

  #region Day6
  public static string Day6()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day6.txt"));
    //var input = new []{ "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN" }; 

    var orbits = new Dictionary<string, List<string>>();

    foreach(var line in input)
    {
      Process(line, orbits);
    }

    var p1 = Count(orbits);

    //For p2 we need to do something a little different, We ("YOU") need to calculate how to get to Santa ("SAN").
    //We should probably do this by first determining the first common orbit. We start counting from the objects that YOU and SAN actually orbit.
    //In the above example, SAN orbits I, and I orbits D. YOU orbits K, which orbits J, which orbits E, which orbits D. The total amount of orbital hops is K -> J -> E -> D -> I = 4 hops.

    var youOrbit = orbits.First(z => z.Value.Contains("YOU")).Key;
    var sanOrbit = orbits.First(z => z.Value.Contains("SAN")).Key;

    var p2 = FindCommonOrbits(youOrbit, sanOrbit, orbits).OrderBy(x => x.Item2).First().Item2;

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private static List<Tuple<string, int>> FindCommonOrbits(string youOrbit, string sanOrbit, Dictionary<string, List<string>> orbits)
  {
    var youOrbits = ReturnOrbits(youOrbit, orbits, 0);
    var sanOrbits = ReturnOrbits(sanOrbit, orbits, 0);

    var weightedOrbits = new List<Tuple<string, int>>();

    //Here is where it gets challenging. How are we going to calculate which of the common orbits requires the least amount of steps? Brute force it?
    //Let's cross that bridge when we get to it.
    var commonOrbits = youOrbits.Select(o => o.Item1).Intersect(sanOrbits.Select(o => o.Item1)).ToList();

    //Now we should weigh these orbits by the amount of "hops" it took from both orbits
    foreach(var commonOrbit in commonOrbits)
    {
      weightedOrbits.Add(new Tuple<string, int>(commonOrbit, youOrbits.First(x => x.Item1 == commonOrbit).Item2 + sanOrbits.First(x => x.Item1 == commonOrbit).Item2));
    }

    return weightedOrbits;
  }

  private static List<Tuple<string, int>> ReturnOrbits(string youOrbit, Dictionary<string, List<string>> orbits, int hops)
  {
    hops++;

    var orbiting = orbits.Where(x => x.Value.Contains(youOrbit)).Select(z => new Tuple<string, int>(z.Key, hops)).ToList();

    var indirectOrbits = new List<Tuple<string, int>>();

    foreach(var orbit in orbiting)
    {
      indirectOrbits.AddRange(ReturnOrbits(orbit.Item1, orbits, hops));
    }

    orbiting.AddRange(indirectOrbits);

    return orbiting;
  }

  private static int Count(Dictionary<string, List<string>> orbits)
  {
    var objects = orbits.SelectMany(z => z.Value).Distinct();

    var checksum = 0;

    foreach(var obj in objects)
    {
      checksum += CalculateOrbits(obj, orbits);
    }

    return checksum;
  }

  private static int CalculateOrbits(string obj, Dictionary<string, List<string>> orbits)
  {
    var inOrbit = orbits.Where(z => z.Value.Contains(obj)).Select(z => z.Key).Distinct();

    var indirectOrbits = 0;

    foreach(var orbiting in inOrbit)
    {
      indirectOrbits += CalculateOrbits(orbiting, orbits);
    }   

    return inOrbit.Count() + indirectOrbits;
  }

  private static void Process(string line, Dictionary<string, List<string>> orbits)
  {
    var split = line.Split(')');

    if(orbits.ContainsKey(split[0]))
    {
      orbits[split[0]].Add(split[1]);
    }
    else
    {
      orbits.Add(split[0], new List<string>{ split[1] });
    }
  }
  #endregion

  #region Day 7

  public static string Day7()
  {

    return OutputResult();
  }

  #endregion

  #region Day 8

  public static string Day8()
  {
    var x = 25;
    var y = 6;

    var input = File.ReadAllText(Path.Combine(InputBasePath, "Day8.txt"));

    //var input = "123456789012";

    var layers = new List<char[][]>();

    for(var layer = 0; layer < input.Length; layer += (x * y))
    {   
      var l = new char[y][];

      for(var i = 0; i < y; i++)
      {
        l[i] = new char[x];

        for(var j = 0; j < x; j++)
        {
          var val = input[j + (i * x) + layer];

          l[i][j] = val;
        }
      }     

      layers.Add(l);
    }     

    var targetLayer = layers.OrderBy(layer => layer.SelectMany(arr => arr.Select(z => z)).Count(z => z == '0')).First().SelectMany(arr => arr.Select(z => z));

    var p1 = targetLayer.Count(z => z == '1') * targetLayer.Count(z => z == '2');

    return OutputResult(p1.ToString());
  }

  #endregion
}