using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace DiscordMinesweeperGenerator {

    public class Program {

        private static Dictionary<int, string> tileConv = new Dictionary<int, string>() {
            { 0, "||:zero:|| " },  { 1, "||:one:|| " },   { 2, "||:two:|| " },
            { 3, "||:three:|| " }, { 4, "||:four:|| " },  { 5, "||:five:|| " },
            { 6, "||:six:|| " },   { 7, "||:seven:|| " }, { 8, "||:eight:|| " },
            { 9, "||:bomb:|| " },
        };

        public static void Main(string[] args) {
            Console.WriteLine("1. Create Puzzle");
            Console.WriteLine("2. Generate Puzzle");
            int action = GetInt("What would you like to do? ", 1, 2);
            List<string> puzzle = action == 1 ? CreatePuzzle() : GeneratePuzzle();
            Console.Clear();
            for (int i = 0; i < puzzle.Count; i++) { 
                Console.WriteLine(puzzle[i]);
            }
        }

        private static List<string> CreatePuzzle() {
            Console.Clear();
            int width = GetInt("How wide is your field? ");
            int height = GetInt("How tall is your field? ");
            string line = "";
            List<string> puzzle = new List<string>();
            for (int y = 0; y < height; y++) {
                int[] tiles = GetDigits("List bomb adjacent counts (9 for a bomb): ", width);
                for (int x = 0; x < width; x++) {
                    line += tileConv[tiles[x]];
                }
                puzzle.Add(line);
                line = "";
            }
            return puzzle;
        }

        private static List<string> GeneratePuzzle() {
            Console.Clear();
            Random rng = new Random();
            int width = GetInt("How wide is your field? ");
            int height = GetInt("How tall is your field? ");
            List<Point> bombPos = new List<Point>();
            int[,] tiles = new int[width, height];
            int bcount = GetInt("How many bombs? ");
            for (int i = 0; i < bcount; i++) {
                Point pos = new Point(rng.Next(0, width), rng.Next(0, height));
                if (bombPos.Any(p => p.X == pos.X && p.Y == pos.Y)) {
                    i--;
                    continue;
                }
                bombPos.Add(pos);
                tiles[pos.X, pos.Y] = 9;
            }
            string line = "";
            List<string> puzzle = new List<string>();
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) { 
                    if (tiles[x, y] != 9) {
                        tiles[x, y] = GetBombCount(new Point(x, y), tiles);
                    }
                    line += tileConv[tiles[x, y]];
                }
                puzzle.Add(line);
                line = "";
            }
            return puzzle;
        }

        private static int GetBombCount(Point pos, int[,] puzzle) {
            int count = 0;
            for (int x = pos.X - 1; x < pos.X + 2; x++) {
                for (int y = pos.Y - 1; y < pos.Y + 2; y++) {
                    if ((x == pos.X && y == pos.Y) == false) {
                        if (x >= 0 && x < puzzle.GetLength(0)) {
                            if (y >= 0 && y < puzzle.GetLength(1)) { 
                                count += puzzle[x, y] == 9 ? 1 : 0;
                            }
                        }
                    }
                }
            }
            return count;
        }

        private static int GetInt(string prompt) { 
            int ans = 0;
            bool parsed = false;
            while (parsed == false) { 
                Console.Write(prompt);
                parsed = int.TryParse(Console.ReadLine(), out ans);
                if (parsed == false) {
                    Console.WriteLine("ERROR: Could not parse int.");
                }
            }
            return ans;
        }

        private static int GetInt(string prompt, int min, int max) {
            int ans = 0;
            if (max < min) {
                ans = min;
                min = max;
                max = ans;
            }
            bool parsed = false;
            while (parsed == false) { 
                ans = GetInt(prompt);
                parsed = (ans >= min && ans <= max);
                if (parsed == false) {
                    Console.WriteLine("ERROR: Int must be between " + min + " and " + max + ".");
                }
            }
            return ans;
        }

        private static int[] GetDigits(string prompt, int count) {
            int[] ans = new int[count];
            bool parsed = false;
            Console.Write(prompt);
            while (parsed == false) {
                string line = Console.ReadLine();
                parsed = line != null && line.Length == count;
                if (parsed) {
                    for (int i = 0; i < count; i++) {
                        ans[i] = line[i] - '0';
                        parsed = parsed == true ? ans[i] >= 0 && ans[i] <= 9 : parsed;
                    }
                }
                if (parsed == false) {
                    Console.WriteLine("ERROR: Expected input is " + count + " digits.");
                }
            }
            return ans;
        }
    }
}