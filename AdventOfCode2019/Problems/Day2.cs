﻿using System;
using static System.Convert;

namespace AdventOfCode2019.Problems
{
    public class Day2 : Problem<int>
    {
        public override int RunPart1() => RunPart(Part1GeneralRunner);
        public override int RunPart2() => RunPart(Part2GeneralRunner);

        private int Part1GeneralRunner(int[] numbersOriginal)
        {
            return General(numbersOriginal, 12, 2, Part1PostRunner);
        }
        private int Part2GeneralRunner(int[] numbersOriginal)
        {
            for (int noun = 0; noun < 100; noun++)
                for (int verb = 0; verb < 100; verb++)
                {
                    int result = General(numbersOriginal, noun, verb, Part2PostRunner);
                    if (result != -1)
                        return result;
                }
            return -1;
        }

        private int Part1PostRunner(int noun, int verb, int output)
        {
            return output;
        }
        private int Part2PostRunner(int noun, int verb, int output)
        {
            if (output == 19690720)
                return noun * 100 + verb;
            return -1;
        }

        private int General(int[] numbersOriginal, int noun, int verb, PostRunner postRunner)
        {
            var numbers = new int[numbersOriginal.Length];
            numbersOriginal.CopyTo(numbers, 0);
            numbers[1] = noun;
            numbers[2] = verb;
            for (int i = 0; i < numbers.Length; i += 4)
            {
                if (numbers[i] == 99)
                    break;
                numbers[numbers[i + 3]] = GetResult();

                int GetResult()
                {
                    return (numbers[i]) switch
                    {
                        1 => numbers[numbers[i + 1]] + numbers[numbers[i + 2]],
                        2 => numbers[numbers[i + 1]] * numbers[numbers[i + 2]],
                        _ => throw new Exception(),
                    };
                }
            }
            return postRunner(noun, verb, numbers[0]);
        }
        public int RunPart(GeneralRunner runner)
        {
            var code = FileContents.Split(',');
            var numbersOriginal = new int[code.Length];
            for (int i = 0; i < code.Length; i++)
                numbersOriginal[i] = ToInt32(code[i]);

            return runner(numbersOriginal);
        }

        public delegate int GeneralRunner(int[] numbersOriginal);
        public delegate int PostRunner(int noun, int verb, int output);
    }
}
