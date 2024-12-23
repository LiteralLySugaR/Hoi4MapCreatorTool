using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace HoI4MapCreatorTool
{
    class Program
    {
        public static string Version = "1.6.9";
        public static List<string> Entries = new List<string>
        {
            "add_core_of",
            "victory_points",
            "owner",
            "set_demilitarized_zone",
            "add_claim_by"
        };
        public class StateInfo
        {
            public string File;
            public string[] Provinces;
        }
        public static List<StateInfo> StatesInfo = new List<StateInfo>();
        public static int menuType = 0;
        public static List<Color> ProvinceColours = new List<Color>();

        //public class StateData
        //{
        //    public string Name;
        //    public int ID;
        //    public Color Colour;
        //}
        //public static List<Color> StateColours = new List<Color>();
        //public static List<StateData> StatesDef = new List<StateData>();
        //
        //static void StateDefinition()
        //{
        //    if (StateColours.Count == 0)
        //    {
        //        StateColours.Add(GetColorFromStringRGB("0 0 0"));
        //        StateColours.Add(GetColorFromStringRGB("16 16 176"));
        //    }
        //    if (menuType != 7)
        //    {
        //        Console.WriteLine($"===== STATE DEFINITION | STATE MANIPULATION TOOL =====");
        //        menuType = 7;
        //    }
        //    Console.Write("StateDefinition: ");
        //    string[] args = Console.ReadLine().Split(' ');
        //
        //    if (args[0].Equals("help"))
        //    {
        //        Console.WriteLine("Available commands:\n" +
        //                " - help\n" +
        //                "    > Will display this list.\n\n" +
        //                " - create\n" +
        //                "    > create state definition based on history\\states.\n\n" +
        //                " - generateRGB\n" +
        //                "    > generate an RGB colour that is not used in [StateDefinitionFile].\n\n" +
        //                " - read [StateDefinitionFile]\n" +
        //                "    > Based on [StateDefinitionFile] will update history\\states\\ files by creating new states if doesnt exist here but does in Definition file and updating other states Existing states will be untouched if they exist in the Definition and no provinces were changed.\n\n" +
        //                " - end\n" +
        //                "    > Close the app.\n\n" +
        //                " - clear\n" +
        //                "    > Clear the mess you and this app have done.\n\n" +
        //                " - return\n" +
        //                "    > Return to previous page.\n");
        //    }
        //    if (args[0].Equals("create"))
        //    {
        //        Console.WriteLine($"[StateDefinition] Commencing Operation, this may take up to 10 minutes . . .");
        //        Thread.Sleep(3000);
        //        CreateStateDefinition();
        //    }
        //    if (args[0].Equals("clear"))
        //    {
        //        Console.Clear();
        //        menuType = 0;
        //    }
        //    if (args[0].Equals("return"))
        //    {
        //        Console.Clear();
        //        Main();
        //    }
        //    if (args[0].Equals("end"))
        //    {
        //        Environment.Exit(0);
        //    }
        //    Thread.Sleep(1000);
        //    Console.WriteLine(" ");
        //    StateDefinition();
        //}
        //public class PixelData
        //{
        //    public int X;
        //    public int Y;
        //    public Color Colour;
        //}
        //public static List<PixelData> Pixels = new List<PixelData>();
        //static void WritePixelsToList(Bitmap bitmap)
        //{
        //    int x = 0;
        //    int y = 0;
        //    Pixels.Clear();
        //    while (x < bitmap.Width - 1 || y < bitmap.Height - 1)
        //    {
        //        Pixels.Add(new PixelData() { X = x, Y = y, Colour = bitmap.GetPixel(x, y) });
        //
        //        if (x < bitmap.Width - 1) { x++; }
        //        else if (x == bitmap.Width - 1 && y < bitmap.Height - 1) { x = 0; y++; }
        //    }
        //
        //    //Last pixel
        //    Pixels.Add(new PixelData() { X = bitmap.Width - 1, Y = bitmap.Height - 1, Colour = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1) });
        //}
        //[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        //internal static extern int GdipBitmapGetPixel(HandleRef bitmap, int x, int y, out int argb);
        //static void CreateStateDefinition()
        //{
        //    List<Color> ReplaceColours = new List<Color>();
        //    List<Color> ReplaceColoursWater = new List<Color>();
        //
        //    Console.WriteLine($"[CreateStateDefinition] Reading map\\provinces.bmp . . .");
        //    Bitmap BMP1 = new Bitmap(@"map\provinces.bmp");
        //    Graphics BMP = Graphics.FromImage(BMP1);
        //    WritePixelsToList(BMP1);
        //
        //    Console.WriteLine($"[CreateStateDefinition] Reading states . . .");
        //    string[] Files = Directory.GetFiles("history\\states\\");
        //    foreach (string file in Files)
        //    {
        //        Console.WriteLine($"[CreateStateDefinition] Reading {file}");
        //        string[] lines = File.ReadAllLines(file);
        //        foreach (string line in lines)
        //        {
        //            if (line.Contains("provinces"))
        //            {
        //                string str1 = lines[lines.ToList().IndexOf(line) + 1];
        //                str1 = str1.Replace("	", string.Empty);
        //                string[] provs = str1.Split(' ');
        //                //Console.WriteLine($"[CreateStateDefinition] {file.Split('\\').Last()} : \n\"{str1}\"");
        //                foreach (string str in File.ReadAllLines(@"map\definition.csv"))
        //                {
        //                    if (str.Contains("sea") && !str.StartsWith("0"))
        //                    {
        //                        string[] str2 = str.Split(';');
        //                        ReplaceColoursWater.Add(GetColorFromStringRGB($"{str2[1]} {str2[2]} {str2[3]}"));
        //                        Console.WriteLine($"[CreateStateDefinition] Found sea province {str2[0]}");
        //                    }
        //                    else if (str.Contains("land") && !str.StartsWith("0"))
        //                    {
        //                        foreach (string prov in provs)
        //                        {
        //                            if (str.StartsWith(prov))
        //                            {
        //                                string[] str2 = str.Split(';');
        //                                ReplaceColours.Add(GetColorFromStringRGB($"{str2[1]} {str2[2]} {str2[3]}"));
        //                                Console.WriteLine($"[CreateStateDefinition] Found state province ({str2[0]}) in {file.Split('\\').Last()}");
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                Color StateColour = GetColorFromStringRGB(GenerateRGBState());
        //                Console.WriteLine($"[CreateStateDefinition] State #{file.Split('\\').Last().Split('-')[0]} assigned colour {GetStringRGBFromColor(StateColour)}");
        //
        //                int x = 0;
        //                int y = 0;
        //                while (x < BMP.Width && y < BMP.Height)
        //                {
        //                    if (ReplaceColours.Contains(BMP.GetPixel(x, y)))
        //                    {
        //                        BMP.SetPixel(x, y, StateColour);
        //                    }
        //                    else if (ReplaceColoursWater.Contains(BMP.GetPixel(x, y)))
        //                    {
        //                        BMP.SetPixel(x, y, GetColorFromStringRGB("16 16 176"));
        //                    }
        //                    else
        //                    {
        //                        BMP.SetPixel(x, y, GetColorFromStringRGB("0 0 0"));
        //                    }
        //                
        //                    if (x >= (BMP.Width - 1)) { x = 0; y++; }
        //                    else { x++; }
        //                }
        //                StatesDef.Add(new StateData { ID = Convert.ToInt32(file.Split('\\').Last().Split('-')[0]), Colour = StateColour, Name = file.Split('-')[1].Split('.')[0] });
        //            }
        //        }
        //    }
        //    foreach (PixelData PD in Pixels.ToArray())
        //    {
        //        if (ReplaceColoursWater.Contains(PD.Colour))
        //        {
        //            PD.Colour = GetColorFromStringRGB("16 16 176");
        //        }
        //        else
        //        {
        //            PD.Colour = GetColorFromStringRGB("0 0 0");
        //        }
        //    }
        //    foreach (PixelData PD in Pixels.ToArray()) { BMP.SetPixel(PD.X, PD.Y, PD.Colour); }
        //
        //    BMP.Save("StateDefinitionTG.png");
        //    List<string> Alllines = new List<string>();
        //    foreach (StateData SD in StatesDef)
        //    {
        //        Alllines.Add($"{SD.ID};{SD.Colour.R};{SD.Colour.G};{SD.Colour.B};{SD.Name}");
        //    }
        //    if (!File.Exists("StateDefinition.txt")) { File.Create("StateDefinition.txt").Close(); }
        //    File.WriteAllLines("StateDefinition.txt", Alllines);
        //}
        //public static string GenerateRGBState()
        //{
        //    string toReturn;
        //    Random R1 = new Random();
        //    byte R = (byte)R1.Next(0, 255);
        //    byte G = (byte)R1.Next(0, 255);
        //    byte B = (byte)R1.Next(0, 255);
        //    if (StateColours.Contains(GetColorFromStringRGB($"{R} {G} {B}")))
        //    {
        //        toReturn = GenerateRGBState();
        //    }
        //    else
        //    {
        //        toReturn = $"{R} {G} {B}";
        //    }
        //    return toReturn;
        //}
        static void ResourcesEntry(string state)
        {
            if (menuType != 4)
            {
                Console.WriteLine($"===== RESOURCES ENTRY OF {state} =====");
                menuType = 4;
            }
            Console.Write("ResourcesEntry: ");
            string[] args = Console.ReadLine().Split(' ');

            if (args[0].Equals("help"))
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("Available commands:\n" +
                        " - help <resources>\n" +
                        "    > Will display this list.\n\n" +
                        " - edit [entry] [value]\n" +
                        "    > Edit existing entry.\n\n" +
                        " - end\n" +
                        "    > Close the app.\n\n" +
                        " - clear\n" +
                        "    > Clear the mess you and this app have done.\n\n" +
                        " - create\n" +
                        "    > Create a resource entry.\n\n" +
                        " - check\n" +
                        "    > Check for existing resource entry.\n\n" +
                        " - add [entry] [value]\n" +
                        "    > Add an entry.\n\n" +
                        " - remove [entry]\n" +
                        "    > Remove an entry.\n\n" +
                        " - return\n" +
                        "    > Return to previous page.\n\n" +
                        " - returnmain\n" +
                        "    > Return to main page.\n");
                }
                else
                {
                    if (args[1].Equals("resources"))
                    {
                        Console.WriteLine("All Resources: \n0 - aluminium\n1 - chromium\n2 - oil\n3 - rubber\n4 - tungsten\n5 - steel\n");
                    }
                }
            }
            if (args[0].Equals("edit"))
            {
                if (args.Length >= 3)
                {
                    if (Resources.Contains(args[1]))
                    {
                        ResourcesEditEntry(state, args[1], args[2]);
                    }
                    else
                    {
                        string entry = ConvertToStringResources(Convert.ToInt32(args[1]));
                        ResourcesEditEntry(state, entry, args[2]);
                    }
                }
                else
                {
                    Console.WriteLine($"[ResourcesEntry] Invalid amount of arguments! {args.Length}, while required 3.");
                }
            }
            if (args[0].Equals("add"))
            {
                if (args.Length >= 3)
                {
                    if (Resources.Contains(args[1]))
                    {
                        ResourcesAddEntry(state, args[1], args[2]);
                    }
                    else
                    {
                        string entry = ConvertToStringResources(Convert.ToInt32(args[1]));
                        ResourcesAddEntry(state, entry, args[2]);
                    }
                }
                else
                {
                    Console.WriteLine($"[ResourcesEntry] Invalid amount of arguments! {args.Length}, while required 3.");
                }
            }
            if (args[0].Equals("remove"))
            {
                if (args.Length >= 2)
                {
                    if (Resources.Contains(args[1]))
                    {
                        ResourcesRemoveEntry(state, args[1]);
                    }
                    else
                    {
                        string entry = ConvertToStringResources(Convert.ToInt32(args[1]));
                        ResourcesRemoveEntry(state, entry);
                    }
                }
                else
                {
                    Console.WriteLine($"[ResourcesEntry] Invalid amount of arguments! {args.Length}, while required 2.");
                }
            }
            if (args[0].Equals("create"))
            {
                CreateResourceEntry(state);
            }
            if (args[0].Equals("check"))
            {
                Console.WriteLine($"[CheckResourcesEntry] Do {state} contains resource entry: {CheckResourcesEntry(state)}");
            }
            if (args[0].Equals("clear"))
            {
                Console.Clear();
                menuType = 0;
            }
            if (args[0].Equals("return"))
            {
                Console.Clear();
                StateHistoryEntry(state);
            }
            if (args[0].Equals("returnmain"))
            {
                Console.Clear();
                Main();
            }
            if (args[0].Equals("end"))
            {
                Environment.Exit(0);
            }
            Thread.Sleep(1000);
            Console.WriteLine(" ");
            ResourcesEntry(state);
        }
        static void ResourcesEditEntry(string state, string entry, string value)
        {
            string path = @"history\states\" + state;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                if (str.Contains(entry))
                {
                    string str2 = str.Split('=')[1];
                    string str3 = str.Replace(str2, $" {value}");
                    string[] str4 = File.ReadAllLines(path);
                    str4[index] = str3;
                    File.WriteAllLines(path, str4);
                    Console.WriteLine($"[ResourcesEditEntry] Changed entry {entry} to {value} in {state}");
                    break;
                }
            }
        }
        static void ResourcesAddEntry(string state, string entry, string value)
        {
            string path = @"history\states\" + state;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                if (str.Contains("resources"))
                {
                    string str2 = str;
                    string str3 = $"{str2}\n		{entry} = {value}";
                    string[] str4 = File.ReadAllLines(path);
                    str4[index] = str3;
                    File.WriteAllLines(path, str4);
                    Console.WriteLine($"[StateAddEntry] Created new {entry} entry in {state} with value {value}");
                    break;
                }
            }
        }
        static void ResourcesRemoveEntry(string state, string entry)
        {
            string path = @"history\states\" + state;
            //int amount = 0;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                if (str.Contains(entry) && str.Contains("="))
                {
                    string[] str1 = File.ReadAllLines(path);
                    str1[index] = null;
                    string[] str2 = str1.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    File.WriteAllLines(path, str2);
                    //amount++;
                }
            }
            Console.WriteLine($"[StateAddEntry] Removed {entry} entry in {state}");
        }
        static void CreateResourceEntry(string state)
        {
            string path = @"history\states\" + state;
            Console.WriteLine($"[CreateResourceEntry] Creating resources entry for {state}");
            int a = 0;
            foreach (string str in File.ReadAllLines(path))
            {
                a++;
                if (str.Contains("resources"))
                {
                    Console.WriteLine($"[CreateResourceEntry] Abandoning, Found resources entry in {state}");
                    break;
                }
                if (!str.Contains("resources") && a == File.ReadAllLines(path).Length)
                {
                    foreach (string str1 in File.ReadAllLines(path))
                    {
                        int index = File.ReadAllLines(path).ToList().IndexOf(str1);
                        if (str1.Contains("name"))
                        {
                            string str2 = str1;
                            string entry = "\n	resources = {\n	}";
                            string str3 = str2 + entry;
                            string[] str4 = File.ReadAllLines(path);
                            str4[index] = str3;
                            File.WriteAllLines(path, str4);
                            Console.WriteLine($"[CreateResourceEntry] Created history entry in {state}");
                            break;
                        }
                    }
                }
            }
        }
        static bool CheckResourcesEntry(string state)
        {
            string path = @"history\states\" + state;
            bool doContains = false;
            foreach (string str in File.ReadAllLines(path))
            {
                if (str.Contains("resources"))
                {
                    doContains = true;
                    break;
                }
            }
            return doContains;
        }
        static void StateHistoryEntry(string state)
        {
            if (menuType != 2)
            {
                Console.WriteLine($"===== HISTORY ENTRY OF {state} =====");
                menuType = 2;
            }
            Console.Write("HistoryEntry: ");
            string[] args = Console.ReadLine().Split(' ');

            if (args[0].Equals("help"))
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("Available commands:\n" +
                        " - help <entries>\n" +
                        "    > Will display this list.\n\n" +
                        " - resourcesentry\n" +
                        "    > Go to resource entry page.\n\n" +
                        " - end\n" +
                        "    > Will close this app.\n\n" +
                        " - clear\n" +
                        "    > Will clear the mess you and this app have done.\n\n" +
                        " - create\n" +
                        "    > Create an entry.\n\n" +
                        " - check\n" +
                        "    > Check for existing entry.\n\n" +
                        " - setowner [countryTAG]\n" +
                        "    > Set state's owner.\n\n" +
                        " - add [entry] [value]\n" +
                        "    > Add an entry.\n\n" +
                        " - remove [entry] <EntryValue>\n" +
                        "    > Remove an entry. Leave EntryValue empty to remove all entries of defined type.\n\n" +
                        " - return\n" +
                        "    > Return to previous page.\n");
                }
                else
                {
                    if (args[1].Equals("entries"))
                    {
                        Console.WriteLine("All entries: \n - victory_points [province] [value]\n - owner [countryTAG]\n - add_core_of [countryTAG]\n - add_claim_by [countryTAG]\n - set_demilitarized_zone [yes/no]");
                    }
                }
            }
            if (args[0].Equals("resourcesentry"))
            {
                Console.Clear();
                ResourcesEntry(state);
            }
            if (args[0].Equals("remove"))
            {
                if (CheckHistoryEntry(state))
                {
                    if (Entries.Contains(args[1]))
                    {
                        if (args.Length == 2)
                        {
                            StateRemoveEntry(state, args[1]);
                        }
                        if (args.Length > 2)
                        {
                            StateRemoveEntry(state, args[1], args[2]);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[StateHistoryEntry] Unable to find entry {args[1]}");
                    }
                }
                else
                {
                    Console.WriteLine($"[StateHistoryEntry] Unable to find history entry!");
                }
            }
            if (args[0].Equals("add"))
            {
                if (CheckHistoryEntry(state))
                {
                    if (Entries.Contains(args[1]) && args.Length > 2)
                    {
                        if (args[1] != "victory_points")
                        {
                            StateAddEntry(state, args[1], args[2]);
                        }
                        else
                        {
                            if (args.Length >= 4)
                            {
                                string[] value = new string[2];
                                value[0] = args[2];
                                value[1] = args[3];
                                StateAddEntry(state, args[1], value);
                            }
                            else
                            {
                                Console.WriteLine($"[StateHistoryEntry] Usage: add victory_points [province] [value]");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[StateHistoryEntry] Entry AND Value required!");
                    }
                }
                else
                {
                    Console.WriteLine($"[StateHistoryEntry] Unable to setowner without history entry!");
                }
            }
            if (args[0].Equals("setowner"))
            {
                if (CheckHistoryEntry(state))
                {
                    if (args[1] != null)
                    {
                        StateSetOwner(state, args[1]);
                    }
                    else
                    {
                        Console.WriteLine($"[StateHistoryEntry] Unable to setowner without country TAG!");
                    }
                }
                else
                {
                    Console.WriteLine($"[StateHistoryEntry] Unable to setowner without history entry!");
                }
            }
            if (args[0].Equals("create"))
            {
                CreateHistoryEntry(state);
            }
            if (args[0].Equals("check"))
            {
                Console.WriteLine($"[CheckHistoryEntry] Do {state} contains history entry: {CheckHistoryEntry(state)}");
            }
            if (args[0].Equals("clear"))
            {
                Console.Clear();
                menuType = 0;
            }
            if (args[0].Equals("return"))
            {
                Console.Clear();
                Main();
            }
            if (args[0].Equals("end"))
            {
                Environment.Exit(0);
            }
            Thread.Sleep(1000);
            Console.WriteLine(" ");
            StateHistoryEntry(state);
        }
        static void StateAddEntry(string state, string entry, string value)
        {
            string path = @"history\states\" + state;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                if (str.Contains("history"))
                {
                    string str2 = str;
                    string str3 = $"{str2}\n		{entry} = {value}";
                    string[] str4 = File.ReadAllLines(path);
                    str4[index] = str3;
                    File.WriteAllLines(path, str4);
                    Console.WriteLine($"[StateAddEntry] Created new {entry} entry in {state} with value {value}");
                    break;
                }
            }
        }
        static void StateAddEntry(string state, string entry, string[] value)
        {
            string path = @"history\states\" + state;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                if (str.Contains("history"))
                {
                    string str2 = str;
                    string vp = "\n		victory_points = {\n			" + value[0] + " " + value[1] + "\n		}";
                    string str3 = $"{str2}{vp}";
                    string[] str4 = File.ReadAllLines(path);
                    str4[index] = str3;
                    File.WriteAllLines(path, str4);
                    Console.WriteLine($"[StateAddEntry] Created new {entry} entry in {state} with values {value[0]} {value[1]}");
                    break;
                }
            }
        }
        static void StateRemoveEntry(string state, string entry, string value)
        {
            string path = @"history\states\" + state;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                if (str.Contains(entry) && str.Contains(value) && entry != "victory_points")
                {
                    string[] str1 = File.ReadAllLines(path);
                    str1[index] = null;
                    string[] str2 = str1.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    File.WriteAllLines(path, str2);
                    Console.WriteLine($"[StateAddEntry] Removed {entry} entry in {state} with value {value}");
                }
                if (str.Contains(entry) && str.Contains(value) && entry == "victory_points")
                {
                    string[] str4 = File.ReadAllLines(path);
                    str4[index] = "";
                    if (!str.Contains("}"))
                    {
                        str4[index+1] = "";
                    }
                    str4 = str4.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    File.WriteAllLines(path, str4);
                    Console.WriteLine($"[StateAddEntry] Removed {entry} entry in {state} with value {value}");
                }
                if (str.Contains(entry) && !str.Contains(value) && entry == "victory_points")
                {
                    if (File.ReadAllLines(path)[index + 1].Contains(value))
                    {
                        string[] str4 = File.ReadAllLines(path);
                        str4[index] = "";
                        str4[index + 1] = "";
                        str4[index + 2] = "";
                        str4 = str4.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                        File.WriteAllLines(path, str4);
                        Console.WriteLine($"[StateAddEntry] Removed {entry} entry in {state} with value {value}");
                    }
                    else
                    {
                        Console.WriteLine($"[StateAddEntry] Ignoring {entry} entry in {state} at {index}");
                    }
                }
            }
        }
        static void StateRemoveEntry(string state, string entry)
        {
            string path = @"history\states\" + state;
            int amount = 0;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                if (str.Contains(entry) && str.Contains("=") && entry != "victory_points")
                {
                    string[] str1 = File.ReadAllLines(path);
                    str1[index] = null;
                    string[] str2 = str1.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    File.WriteAllLines(path, str2);
                    amount++;
                }
                if (str.Contains(entry) && (entry == "victory_points"))
                {
                    Console.WriteLine($"[StateAddEntry] Abandoning, cannot remove {entry} without additional value!");
                    break;
                }
            }
            Console.WriteLine($"[StateAddEntry] Removed {amount} of {entry} entries in {state}");
        }
        static void StateSetOwner(string state, string TAG)
        {
            string path = @"history\states\" + state;
            int a = 0;
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);
                a++;
                if (str.Contains("owner"))
                {
                    string str2 = str.Split('=')[1];
                    string str3 = str.Replace(str2, $" {TAG}");
                    string[] str4 = File.ReadAllLines(path);
                    str4[index] = str3;
                    File.WriteAllLines(path, str4);
                    Console.WriteLine($"[StateSetOwner] Owner changed from {str2} to {TAG} in {state}");
                    break;
                }
                if (!str.Contains("owner") && a == File.ReadAllLines(path).Length)
                {
                    foreach (string str1 in File.ReadAllLines(path))
                    {
                        index = File.ReadAllLines(path).ToList().IndexOf(str1);
                        if (str1.Contains("history"))
                        {
                            string str2 = str1;
                            string newEntry = $"\n		owner = {TAG}";
                            string str3 = str2 + newEntry;
                            string[] str4 = File.ReadAllLines(path);
                            str4[index] = str3;
                            File.WriteAllLines(path, str4);
                            Console.WriteLine($"[StateSetOwner] Created new entry for owner with value {TAG} in {state} history entry.");
                            break;
                        }
                    }
                }
            }
        }
        static void CreateHistoryEntry(string state)
        {
            string path = @"history\states\" + state;
            Console.WriteLine($"[CreateHistoryEntry] Creating history entry for {state}");
            int a = 0;
            foreach (string str in File.ReadAllLines(path))
            {
                a++;
                if (str.Contains("history"))
                {
                    Console.WriteLine($"[CreateHistoryEntry] Abandoning, Found history entry in {state}");
                    break;
                }
                if (!str.Contains("history") && a == File.ReadAllLines(path).Length)
                {
                    foreach (string str1 in File.ReadAllLines(path))
                    {
                        int index = File.ReadAllLines(path).ToList().IndexOf(str1);
                        if (str1.Contains("name"))
                        {
                            string str2 = str1;
                            string entry = "\n	history = {\n	}";
                            string str3 = str2 + entry;
                            string[] str4 = File.ReadAllLines(path);
                            str4[index] = str3;
                            File.WriteAllLines(path, str4);
                            Console.WriteLine($"[CreateHistoryEntry] Created history entry in {state}");
                            break;
                        }
                    }
                }
            }
        }
        static bool CheckHistoryEntry(string state)
        {
            string path = @"history\states\" + state;
            bool doContains = false;
            foreach (string str in File.ReadAllLines(path))
            {
                if (str.Contains("history"))
                {
                    doContains = true;
                    break;
                }
            }
            return doContains;
        }
        static void UseArray(List<string> StatesIDS)
        {
            if (menuType != 3)
            {
                Console.WriteLine($"===== HISTORY ENTRY OF MULTIPLE STATES =====");
                menuType = 3;
            }
            Console.Write("ArrayHistoryEntry: ");
            string[] args = Console.ReadLine().Split(' ');

            if (args[0].Equals("help"))
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("Available commands:\n" +
                        " - help <entries>\n" +
                        "    > Will display this list.\n\n" +
                        " - resourcesentry\n" +
                        "    > Go to resource entry page.\n\n" +
                        " - showarray\n" +
                        "    > Show selected states.\n\n" +
                        " - clear\n" +
                        "    > Clear the mess you and this app have done.\n\n" +
                        " - create\n" +
                        "    > Create an entry.\n\n" +
                        " - check\n" +
                        "    > Check for existing entry.\n\n" +
                        " - end\n" +
                        "    > Will close this app.\n\n" +
                        " - remove [stateID]\n" +
                        "    > Remove a state from the array.\n\n" +
                        " - setowner [countryTAG]\n" +
                        "    > Set state's owner.\n\n" +
                        " - add [entry] [value]\n" +
                        "    > Add an entry.\n\n" +
                        " - remove [entry] <EntryValue>\n" +
                        "    > Remove an entry. Leave EntryValue empty to remove all entries of defined type.\n\n" +
                        " - return\n" +
                        "    > Return to previous page.\n");
                }
                else
                {
                    if (args[1].Equals("entries"))
                    {
                        Console.WriteLine("All entries: \n - owner [countryTAG]\n - add_core_of [countryTAG]\n - add_claim_by [countryTAG]");
                    }
                }
            }
            if (args[0].Equals("showarray"))
            {
                string showArray = string.Join("\n ", StatesIDS);
                Console.WriteLine($"All States in the array: \n {showArray}");
            }
            if (args[0].Equals("remove"))
            {
                List<string> ExceptionStates = new List<string>();
                foreach (string state in StatesIDS)
                {
                    if (CheckHistoryEntry(state))
                    {
                        if (Entries.Contains(args[1]) && args.Length > 2)
                        {
                            if (args[2] != "victory_points")
                            {
                                StateRemoveEntry(state, args[1], args[2]);
                            }
                            else
                            {
                                Console.WriteLine($"[StateHistoryEntry] Victory Points for multiple state is not allowed!");
                                break;
                            }
                        }
                        else if (Entries.Contains(args[1]) && args.Length == 2)
                        {
                            StateRemoveEntry(state, args[1]);
                        }
                    }
                    else
                    {
                        ExceptionStates.Add(state);
                    }
                }
                if (ExceptionStates.Count > 0)
                {
                    string str = string.Join("\n ", ExceptionStates);
                    Console.WriteLine($"[StateHistoryEntry] Exception States: \n {str}");
                }
            }
            if (args[0].Equals("add"))
            {
                List<string> ExceptionStates = new List<string>();
                foreach (string state in StatesIDS)
                {
                    if (CheckHistoryEntry(state))
                    {
                        if (Entries.Contains(args[1]) && args.Length > 2)
                        {
                            if (args[1] != "victory_points")
                            {
                                StateAddEntry(state, args[1], args[2]);
                            }
                            else
                            {
                                Console.WriteLine($"[StateHistoryEntry] Victory Points for multiple state is not allowed!");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[StateHistoryEntry] Entry AND Value required!");
                            break;
                        }
                    }
                    else
                    {
                        ExceptionStates.Add(state);
                    }
                }
                if (ExceptionStates.Count > 0)
                {
                    string str = string.Join("\n ", ExceptionStates);
                    Console.WriteLine($"[StateHistoryEntry] Exception States: \n {str}");
                }
            }
            if (args[0].Equals("setowner"))
            {
                List<string> ExceptionStates = new List<string>();
                foreach (string state in StatesIDS)
                {
                    if (CheckHistoryEntry(state))
                    {
                        if (args.Length > 1)
                        {
                            StateSetOwner(state, args[1]);
                        }
                        else
                        {
                            Console.WriteLine($"[UseArray] Unable to setowner without country TAG!");
                            break;
                        }
                    }
                    else
                    {
                        ExceptionStates.Add(state);
                    }
                }
                if (ExceptionStates.Count > 0)
                {
                    string str = string.Join("\n ", ExceptionStates);
                    Console.WriteLine($"[StateHistoryEntry] Exception States: \n {str}");
                }
            }
            if (args[0].Equals("create"))
            {
                foreach (string state in StatesIDS)
                {
                    CreateHistoryEntry(state);
                }
            }
            if (args[0].Equals("check"))
            {
                foreach (string state in StatesIDS)
                {
                    Console.WriteLine($"[CheckHistoryEntry] Do {state} contains history entry: {CheckHistoryEntry(state)}");
                }
            }
            if (args[0].Equals("clear"))
            {
                Console.Clear();
                menuType = 0;
            }
            if (args[0].Equals("return"))
            {
                Console.Clear();
                Main();
            }
            if (args[0].Equals("end"))
            {
                Environment.Exit(0);
            }
            Thread.Sleep(1000);
            Console.WriteLine(" ");
            UseArray(StatesIDS);
        }
        static void ProvinceDefinition()
        {
            if (menuType != 5)
            {
                Console.WriteLine($"===== PROVINCE DEFINITION | PROVINCE MANIPULATION TOOL =====");
                menuType = 5;
            }
            Console.Write("Province: ");
            string[] args = Console.ReadLine().Split(' ');
            
            if (args[0].Equals("help"))
            {
                if (args.Length == 1)
                {
                    Console.WriteLine("Available commands:\n" +
                    " - help <createLandType/clt>\n" +
                    "    > Will display this list.\n\n" +
                    " - clear\n" +
                    "    > Will clear the mess you and this app have done.\n\n" +
                    " - end\n" +
                    "    > Will close this app.\n\n" +
                    " - return\n" +
                    "    > Return to previous page.\n\n" +
                    " - colorsyntax\n" +
                    "    > Show list of colours to use when making terrain input.\n\n" +
                    " - tohex [R-G-B]\n" +
                    "    > Convert RGB colour to HEX.\n\n" +
                    " - set [coastal/terrainType/provinceType/continent] [value] [definitionFile] [province(s)]\n" +
                    "    > Changes specified data of the province(s).\n\n" +
                    " - generateRGB\n" +
                    "    > Generate an RGB colour that is not used in definition.csv.\n\n" +
                    " - create [DefinitionFile] [R-G-B/HEX] [ProvinceType] [isCoastal] <continentType>\n" +
                    "    > Create a province.\n\n" +
                    " - createLandType/clt [TerrainInput] [outputFileName] <MinX-MinY> <MaxX-MaxY>\n" +
                    "    > Using terrain input sets provinces type for entire map. Min/Max(X/Y) sets the check area in pixels for the tool. Example: map/TerrainInput2.bmp newDefinition 338-565 2724-1587\n");
                }
                else
                {
                    if (args[1].Equals("createLandType") || args[1].Equals("clt"))
                    {
                        Console.WriteLine("This command requires province map (map/provinces.bmp), a Terrain Input File path and output file name. Optional you can set starting pixel to check from and last pixel position to check (\"-\" BETWEEN X AND Y IS REQUIRED), otherwise it will take longer to check since it will check EACH pixel. The Output file will ALWAYS be created as a .txt file.");
                    }
                }
            }
            if (args[0].Equals("set"))
            {
                string[] data = new string[4] { "coastal", "terrainType", "provinceType", "continent" };

                if (data.Contains(args[1]) && args.Length >= 5)
                {
                    string dataType = args[1];

                    string dataValue = args[2];

                    string dataDefinition = args[3];

                    string[] provinces = args;

                    provinces.ToList().RemoveRange(0, 4);

                    if (File.Exists(dataDefinition))
                    {
                        SetProvinceDefinition(dataType, dataValue, dataDefinition, provinces);
                    }
                    else
                    {
                        Console.WriteLine($"Definition file not found. Aborting.");
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid second argument or missing argument!");
                }
            }
            if (args[0].Equals("tohex") && args.Length >= 2)
            {
                string hex = StringRGBtoHex(args[1].Split('-'));
                Console.WriteLine($"[ProvinceDefinition] Hex: {hex}");
            }
            if (args[0].Equals("create"))
            {
                string[] ColourRGB = new string[3];
                if (args.Length > 4)
                {
                    if (args[2].Contains("-"))
                    {
                        ColourRGB = args[2].Split('-');
                    }
                    else
                    {
                        ColourRGB = HexToStringRGB(args[2]);
                    }
                }
                if (args.Length == 5)
                {
                    CreateProvinceDefEntry(args[1], ColourRGB, "land", args[3], args[4], "1");
                }
                else if (args.Length > 5)
                {
                    CreateProvinceDefEntry(args[1], ColourRGB, "land", args[3], args[4], args[5]);
                }
                else
                {
                    Console.WriteLine($"[ProvinceDefinition] Command has {args.Length} arguments while 5 required!");
                }
            }
            if (args[0].Equals("colorsyntax"))
            {
                Console.WriteLine("Color Syntax Table (for Terrain Input image)\n" +
                "________________________________________\n" +
                "   TYPE   |   HEX  |     RGB     |\n" +
                "________________________________________\n" +
                "  WATER   | 0000FF |   0 0 255   |\n" +
                "\n" +
                "  PLAINS  | FF8142 |  255 129 66 |\n" +
                "\n" +
                "  HILLS   | F8FF99 | 248 255 153 |\n" +
                "\n" +
                " MOUNTAIN | 9DC0D0 | 157 192 208 |\n" +
                "\n" +
                "  URBAN   | 787878 | 120 120 120 |\n" +
                "\n" +
                "  MARSH   | 4C6023 |  76 96 35   |\n" +
                "\n" +
                "  DESERT  | FF7F00 |  255 127 0  |\n" +
                "\n" +
                "  FOREST  | 59C755 |  89 199 85  |\n" +
                "\n" +
                "  JUNGLE  | 7FBF00 |  127 191 0  |\n" +
                "________________________________________");
            }
            if (args[0].Equals("createLandType") || args[0].Equals("clt"))
            {
                //[ID];[R];[G];[B]
                //Split(';')[0] - ID
                //338 565 > 2724 1587 | (+1)

                //1. Read each pixel from map/provinces.bmp             X
                //2. Return Pixel XY and RGB                            X
                //3. Get TerrainType by Pixel Color at XY               X
                //3.1 Ignore color [0 0 255]                            X
                //4. Find ID by color using [R];[G];[B]                 X
                //5. Replace terrainEntry with GetTerrainType result    X
                if (args.Length == 3)
                {
                    Console.WriteLine($"[ProvinceDefinition] It will take some time, please wait.");
                    List<PixelInfo> pixelInfos = ReadPixelFromProvinces(args[1]);

                    if (File.Exists($"{args[2]}.txt")) { File.Delete($"{args[2]}.txt"); }
                    File.Create($"{args[2]}.txt").Close();
                    Console.WriteLine($"[ReplaceProvinceTypeEntry] Created file {args[2]}.txt");

                    ReplaceProvinceTypeEntry(pixelInfos, args[2]);
                }
                else if (args.Length == 4)
                {
                    Console.WriteLine($"[ProvinceDefinition] It will take some time, please wait.");
                    List<PixelInfo> pixelInfos = ReadPixelFromProvinces(args[1], args[3]);

                    if (File.Exists($"{args[2]}.txt")) { File.Delete($"{args[2]}.txt"); }
                    File.Create($"{args[2]}.txt").Close();
                    Console.WriteLine($"[ReplaceProvinceTypeEntry] Created file {args[2]}.txt");

                    ReplaceProvinceTypeEntry(pixelInfos, args[2]);

                }
                else if (args.Length >= 5)
                {
                    Console.WriteLine($"[ProvinceDefinition] It will take some time, please wait.");
                    List<PixelInfo> pixelInfos = ReadPixelFromProvinces(args[1], args[3], args[4]);

                    if (File.Exists($"{args[2]}.txt")) { File.Delete($"{args[2]}.txt"); }
                    File.Create($"{args[2]}.txt").Close();
                    Console.WriteLine($"[ReplaceProvinceTypeEntry] Created file {args[2]}.txt");

                    ReplaceProvinceTypeEntry(pixelInfos, args[2]);
                }
                else
                {
                    Console.WriteLine($"[ProvinceDefinition] Syntax Error! 3 arguments are required.");
                }
            }
            if (args[0].Equals("generateRGB"))
            {
                Console.WriteLine($"[GenerateRGB] {GenerateRGB()}");
            }
            if (args[0].Equals("end"))
            {
                Environment.Exit(0);
            }
            if (args[0].Equals("clear"))
            {
                Console.Clear();
                menuType = 0;
            }
            if (args[0].Equals("return"))
            {
                Console.Clear();
                Main();
            }
            Thread.Sleep(1000);
            Console.WriteLine(" ");
            ProvinceDefinition();
        }
        public static void SetProvinceDefinition(string dataType, string dataValue, string definitionFile, string[] provinces)
        {
            //id;R;G;B;terrainType;coastal;provinceType;continent
            //terrainTypes: land, sea, lake
            //provinceTypes: unknown, plains, hills, mountain, jungle, forest, desert, marsh, urban, lakes, ocean

            int changeIndex = 0;

            if (dataType == "coastal")
            {changeIndex = 5; }
            else if (dataType == "terrainType")
            { changeIndex = 4; }
            else if (dataType == "provinceType")
            { changeIndex = 6; }
            else if (dataType == "continent")
            { changeIndex = 7; }

            string[] lines = File.ReadAllLines(definitionFile);

            int hits = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string provID = lines[i].Split(';')[0];

                if (provinces.Contains(provID))
                {
                    string[] defLine = lines[i].Split(';');

                    defLine[changeIndex] = dataValue;

                    if (dataType == "terrainType" && dataValue == "land")
                    {
                        defLine[6] = "plains";
                    }
                    else if (dataType == "terrainType" && dataValue == "sea")
                    {
                        defLine[6] = "ocean";
                    }
                    else if (dataType == "terrainType" && dataValue == "lake")
                    {
                        defLine[6] = "lakes";
                    }

                    lines[i] = string.Join(";", defLine);

                    Console.WriteLine($"[SetProvinceDefinition] Hit at line {i}, province {provID}");

                    hits++;
                }

                if (hits == provinces.Length)
                {
                    Console.WriteLine($"[SetProvinceDefinition] Writing changes. . .");

                    File.WriteAllLines(definitionFile, lines);

                    break;
                }
            }
        }
        public static string[] HexToStringRGB(string Hex)
        {
            string h1 = $"{Hex.ToCharArray()[0]}{Hex.ToCharArray()[1]}";
            string h2 = $"{Hex.ToCharArray()[2]}{Hex.ToCharArray()[3]}";
            string h3 = $"{Hex.ToCharArray()[4]}{Hex.ToCharArray()[5]}";
            h1 = Convert.ToInt32(h1, 16).ToString();
            h2 = Convert.ToInt32(h2, 16).ToString();
            h3 = Convert.ToInt32(h3, 16).ToString();
            string[] RGB = new string[] { h1, h2, h3 };
            return RGB;
        }
        public static string StringRGBtoHex(string[] RGB)
        {
            string Hex = $"{DecimalToHexadecimal(Convert.ToInt32(RGB[0]))}{DecimalToHexadecimal(Convert.ToInt32(RGB[1]))}{DecimalToHexadecimal(Convert.ToInt32(RGB[2]))}";
            return Hex;

        }
        private static string DecimalToHexadecimal(int integer)
        {
            if (integer <= 9 && integer > 0) { return $"0{integer}"; }
            else if (integer <= 0) { return "00"; }
            int hex = integer;
            string hexStr = string.Empty;

            while (integer > 0)
            {
                hex = integer % 16;

                if (hex < 10)
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 48).ToString());
                else
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 55).ToString());

                integer /= 16;
            }

            return hexStr;
        }
        public static void CreateProvinceDefEntry(string DefinitionPath, string[] RGB, string TerrainType, string ProvinceType, string isCoastal, string continent)
        {
            string[] AllLines = File.ReadAllLines(DefinitionPath);
            AllLines = AllLines.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            string lastLine = AllLines[AllLines.Length - 1];
            string ID = (Convert.ToInt32(lastLine.Split(';')[0]) + 1).ToString();
            string newLine = $"{ID};{RGB[0]};{RGB[1]};{RGB[2]};{TerrainType};{isCoastal};{ProvinceType};{continent}";

            AllLines[AllLines.Length - 1] = $"{AllLines[AllLines.Length - 1]}\n{newLine}";
            File.WriteAllLines(DefinitionPath, AllLines);
            Console.WriteLine($"[CreateProvinceDefEntry] Created new entry {newLine} in {DefinitionPath}");
        }
        public static List<PixelInfo> ReadPixelFromProvinces(string PathTerrainInput)
        {
            Bitmap TerrainInput = new Bitmap(PathTerrainInput);
            Bitmap provinceMap = new Bitmap(@"map\provinces.bmp");

            Color[] IgnoredColors = new Color[5];
            IgnoredColors[0] = Color.FromArgb(0, 0, 255);
            IgnoredColors[1] = Color.FromArgb(40, 83, 176);
            IgnoredColors[2] = Color.FromArgb(75, 162, 198);
            IgnoredColors[3] = Color.FromArgb(56, 118, 217);
            IgnoredColors[4] = Color.FromArgb(58, 91, 255);

            List<Color> ColorsToIngore = new List<Color>();
            foreach (Color IC in IgnoredColors)
            {
                ColorsToIngore.Add(IC);
            }

            List<PixelInfo> ProvPixelInfo = new List<PixelInfo>();
            int x = 0;
            int y = 0;
            while (x < provinceMap.Width && y < provinceMap.Height)
            {
                if (!ColorsToIngore.Contains(provinceMap.GetPixel(x, y)))
                {
                    PixelInfo pixel = new PixelInfo()
                    {
                        X = x,
                        Y = y,
                        RGB = $"{GetStringRGBFromColor(provinceMap.GetPixel(x, y))}",
                        Type = ConvertColorToType(TerrainInput.GetPixel(x, y)),
                        ID = FindProvinceIdByColor(provinceMap.GetPixel(x, y))
                    };
                    ProvPixelInfo.Add(pixel);
                    ColorsToIngore.Add(provinceMap.GetPixel(x, y));
                }

                if (x >= (provinceMap.Width - 1)) { x = 0; y++; }
                else { x++; }
            }
            return ProvPixelInfo;
        }
        public static List<PixelInfo> ReadPixelFromProvinces(string PathTerrainInput, string MinXY, string MaxXY)
        {
            Bitmap TerrainInput = new Bitmap(PathTerrainInput);
            Bitmap provinceMap = new Bitmap(@"map\provinces.bmp");

            int MinX = Convert.ToInt32(MinXY.Split('-')[0]);
            int MinY = Convert.ToInt32(MinXY.Split('-')[1]);
            int MaxX = Convert.ToInt32(MaxXY.Split('-')[0]);
            int MaxY = Convert.ToInt32(MaxXY.Split('-')[1]);

            Color[] IgnoredColors = new Color[5];
            IgnoredColors[0] = Color.FromArgb(0, 0, 255);
            IgnoredColors[1] = Color.FromArgb(40, 83, 176);
            IgnoredColors[2] = Color.FromArgb(75, 162, 198);
            IgnoredColors[3] = Color.FromArgb(56, 118, 217);
            IgnoredColors[4] = Color.FromArgb(58, 91, 255);

            List<Color> ColorsToIngore = new List<Color>();
            foreach (Color IC in IgnoredColors)
            {
                ColorsToIngore.Add(IC);
            }

            List<PixelInfo> ProvPixelInfo = new List<PixelInfo>();
            int x = MinX;
            int y = MinY;
            while (x >= MinX && x < (MaxX + 1) && y >= MinY && y < (MaxY + 1))
            {
                if (!ColorsToIngore.Contains(provinceMap.GetPixel(x, y)))
                {
                    PixelInfo pixel = new PixelInfo()
                    {
                        X = x,
                        Y = y,
                        RGB = $"{GetStringRGBFromColor(provinceMap.GetPixel(x, y))}",
                        Type = ConvertColorToType(TerrainInput.GetPixel(x, y)),
                        ID = FindProvinceIdByColor(provinceMap.GetPixel(x, y))
                    };
                    ProvPixelInfo.Add(pixel);
                    ColorsToIngore.Add(provinceMap.GetPixel(x, y));
                }

                if (x >= MaxX) { x = MinX; y++; }
                else { x++; }
            }
            return ProvPixelInfo;
        }
        public static List<PixelInfo> ReadPixelFromProvinces(string PathTerrainInput, string MinXY)
        {
            Bitmap TerrainInput = new Bitmap(PathTerrainInput);
            Bitmap provinceMap = new Bitmap(@"map\provinces.bmp");

            int MinX = Convert.ToInt32(MinXY.Split('-')[0]);
            int MinY = Convert.ToInt32(MinXY.Split('-')[1]);

            Color[] IgnoredColors = new Color[5];
            IgnoredColors[0] = Color.FromArgb(0, 0, 255);
            IgnoredColors[1] = Color.FromArgb(40, 83, 176);
            IgnoredColors[2] = Color.FromArgb(75, 162, 198);
            IgnoredColors[3] = Color.FromArgb(56, 118, 217);
            IgnoredColors[4] = Color.FromArgb(58, 91, 255);

            List<Color> ColorsToIngore = new List<Color>();
            foreach (Color IC in IgnoredColors)
            {
                ColorsToIngore.Add(IC);
            }

            List<PixelInfo> ProvPixelInfo = new List<PixelInfo>();
            int x = MinX;
            int y = MinY;
            while (x >= MinX && x <= provinceMap.Width && y >= MinY && y <= provinceMap.Height)
            {
                if (!ColorsToIngore.Contains(provinceMap.GetPixel(x, y)))
                {
                    PixelInfo pixel = new PixelInfo()
                    {
                        X = x,
                        Y = y,
                        RGB = $"{GetStringRGBFromColor(provinceMap.GetPixel(x, y))}",
                        Type = ConvertColorToType(TerrainInput.GetPixel(x, y)),
                        ID = FindProvinceIdByColor(provinceMap.GetPixel(x, y))
                    };
                    ProvPixelInfo.Add(pixel);
                    ColorsToIngore.Add(provinceMap.GetPixel(x, y));
                }

                if (x >= (provinceMap.Width - 1)) { x = MinX; y++; }
                else { x++; }
            }
            return ProvPixelInfo;
        }
        public static void ReplaceProvinceTypeEntry(List<PixelInfo> pixelInfos, string NewFile)
        {
            string path = $"{NewFile}.txt";
            string Defpath = @"map\definition.csv";
            int index = 0;
            int amount = 0;

            string[] lines = File.ReadAllLines(Defpath);
            File.WriteAllLines(path, lines);

            string[] newLines = File.ReadAllLines(path);

            foreach (PixelInfo Pi in pixelInfos)
            {
                foreach (string str in newLines)
                {
                    index = newLines.ToList().IndexOf(str);
                    if (str.StartsWith($"{Pi.ID};") && !str.Contains("sea") && !str.Contains(Pi.Type))
                    {
                        string[] str2 = str.Split(';');
                        str2[6] = Pi.Type;
                        string str3 = string.Join(";", str2);
                        string[] str4 = File.ReadAllLines(path);
                        str4[index] = str3;

                        File.WriteAllLines(path, str4);
                        amount++;
                    }
                }
            }
            Console.WriteLine($"[ReplaceProvinceTypeEntry] Operation Successful! Replaced {amount} of entries.");
        }
        public static int FindProvinceIdByColor(Color colour)
        {
            string path = @"map\definition.csv";
            string strColor = GetStringRGBFromColor(colour);
            strColor = string.Join(";", strColor.Split(' '));
            string toReturn = "";

            foreach (string str in File.ReadAllLines(path))
            {
                if (str.Contains(strColor))
                {
                    toReturn = str.Split(';')[0];
                    break;
                }
            }
            return Convert.ToInt32(toReturn);
        }
        public static Color GetColorFromStringRGB(string color)
        {
            string[] RGB = color.Split(' ');
            return Color.FromArgb(Convert.ToInt32(RGB[0]), Convert.ToInt32(RGB[1]), Convert.ToInt32(RGB[2]));
        }
        public static string GetStringRGBFromColor(Color color)
        {
            return $"{color.R} {color.G} {color.B}";
        }
        public static string ConvertColorToType(Color colour)
        {
            string toReturn = "plains";
            ColorTypes colorTypes = new ColorTypes();
            if (colour.Equals(colorTypes.HillssColor))
            {
                toReturn = "hills";
            }
            else if (colour.Equals(colorTypes.MountainColor))
            {
                toReturn = "mountain";
            }
            else if (colour.Equals(colorTypes.UrbanColor))
            {
                toReturn = "urban";
            }
            else if (colour.Equals(colorTypes.MarshColor))
            {
                toReturn = "marsh";
            }
            else if (colour.Equals(colorTypes.DesertColor))
            {
                toReturn = "desert";
            }
            else if (colour.Equals(colorTypes.ForestColor))
            {
                toReturn = "forest";
            }
            else if (colour.Equals(colorTypes.JungleColor))
            {
                toReturn = "jungle";
            }

            return toReturn;
        }
        public class ColorTypes
        {
            //Water #0000FF
            public Color PlainsColor = Color.FromArgb(255, 129, 66); // #FF8142
            public Color HillssColor = Color.FromArgb(248, 255, 153); // #F8FF99
            public Color MountainColor = Color.FromArgb(157, 192, 208); // #9DC0D0
            public Color UrbanColor = Color.FromArgb(120, 120, 120); // #787878
            public Color MarshColor = Color.FromArgb(76, 96, 35); // #4C6023
            public Color DesertColor = Color.FromArgb(255, 127, 0); // #FF7F00
            public Color ForestColor = Color.FromArgb(89, 199, 85); // #59C755
            public Color JungleColor = Color.FromArgb(127, 191, 0); // #7FBF00
        }
        public class PixelInfo
        {
            public int X;
            public int Y;
            public string RGB;
            public string Type;
            public int ID;
        }
        public static string GenerateRGB()
        {
            string toReturn;
            Random R1 = new Random();
            byte R = (byte)R1.Next(0, 255);
            byte G = (byte)R1.Next(0, 255);
            byte B = (byte)R1.Next(0, 255);
            if (ProvinceColours.Contains(GetColorFromStringRGB($"{R} {G} {B}")))
            {
                toReturn = GenerateRGB();
            }
            else
            {
                toReturn = $"{R} {G} {B}";
            }
            return toReturn;
        }
        public static void UpdateProvinceColours()
        {
            string definition = @"map\definition.csv";
            string[] lines = File.ReadAllLines(definition);
            foreach (string str in lines)
            {
                if (str.Contains(";") && !str.Contains("#"))
                {
                    string[] localStr = str.Split(';');
                    Color localRGB = GetColorFromStringRGB($"{localStr[1]} {localStr[2]} {localStr[3]}");
                    if (!ProvinceColours.Contains(localRGB))
                    {
                        ProvinceColours.Add(localRGB);
                    }
                }
            }
        }
        public static void UpdateStatesProvinces()
        {
            string[] Files = Directory.GetFiles("history\\states\\");
            foreach (string file in Files)
            {
                string[] lines = File.ReadAllLines(file);
                foreach (string str in lines)
                {
                    if (str.Contains("provinces"))
                    {
                        string str1 = lines[lines.ToList().IndexOf(str) + 1];

                        str1 = str1.Replace("	", string.Empty);

                        string[] provs = str1.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                        StatesInfo.Add(new StateInfo { File = file, Provinces = provs });
                        break;
                    }
                }
            }
        }
        static void StrategicRegionMain()
        {
            if (menuType != 6)
            {
                Console.WriteLine($"===== STRATEGIC REGIONS MANIPULATION TOOL =====");
                menuType = 6;
            }
            Console.Write("Action: ");
            string[] args = Console.ReadLine().Split(' ');

            if (args[0].Equals("help"))
            {
                Console.WriteLine("Available commands:\n" +
                            " - help\n" +
                            "    > Will display this list.\n\n" +
                            " - transfer [Provinces] [StratRegionID]\n" +
                            "    > Transfer all written provinces to the strategic region. If no other regions contains written provinces it will simply add them, otherwise remove from other regions.\n" +
                            "    > At least two arguments required.\n\n" +
                            " - transfer state [StatesID] [StratRegionID]\n" +
                            "    > Transfer all states provinces to the strategic region. If no other regions contains written provinces it will simply add them, otherwise remove from other regions.\n\n" +
                            " - clear\n" +
                            "    > Clear the mess this app and you wrote here.\n\n" +
                            " - end\n" +
                            "    > Close the app.\n");
            }
            if (args[0].Equals("transfer"))
            {
                if (args.Length >= 3 && !args.Contains("state"))
                {
                    //map\\strategicregions\\
                    List<string> provs = args.ToList();
                    string targetStratRegion = args.Last();
                    provs.Remove(args[0]);
                    provs.Remove(args.Last());
                    provs.TrimExcess();

                    ChangeStratRegion(provs, targetStratRegion);
                }
                else if (args.Length >= 4 && args.Contains("state"))
                {
                    //map\\strategicregions\\
                    List<string> states = args.ToList();
                    string targetStratRegion = args.Last();
                    states.Remove(args[0]);
                    states.Remove(args.Last());
                    states.TrimExcess();

                    List<string> Provs = new List<string>();
                    foreach (StateInfo Si in StatesInfo)
                    {
                        foreach (string state in states)
                        {
                            if (Si.File.Split('\\').Last().Split('-')[0].Equals(state))
                            {
                                Provs.AddRange(Si.Provinces);
                                break;
                            }
                        }
                    }
                    ChangeStratRegion(Provs, targetStratRegion);
                }
            }
            if (args[0].Equals("clear"))
            {
                Console.Clear();
                menuType = 0;
            }
            if (args[0].Equals("return"))
            {
                Console.Clear();
                Main();
            }
            if (args[0].Equals("end"))
            {
                Environment.Exit(0);
            }
            Thread.Sleep(1000);
            Console.WriteLine(" ");
            StrategicRegionMain();
        }
        public static void ChangeStratRegion(List<string> Provinces, string StrategicRegionID)
        {
            string[] Files = Directory.GetFiles("map\\strategicregions\\");
            List<string> provsTransfer = new List<string>();
            string TargetFile = "";
            foreach (string file in Files)
            {
                //Console.WriteLine($"[ChangeStratRegion] Working with {file} . . .");
                if (file.Split('\\').Last().Split('-')[0].Equals(StrategicRegionID))
                {
                    Console.WriteLine($"[ChangeStratRegion] Target file found.");
                    TargetFile = file;
                }
                else
                {
                    string[] lines = File.ReadAllLines(file);
                    foreach (string line in lines.ToArray())
                    {
                        if (line.Contains("provinces"))
                        {
                            string str1 = lines[lines.ToList().IndexOf(line) + 1];
                            str1 = str1.Replace("	", string.Empty);
                            string[] str2 = str1.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                            foreach (string str in str2)
                            {
                                if (Provinces.Contains(str))
                                {
                                    Console.WriteLine($"[ChangeStratRegion] Spotted written province ({str}) in {file.Split('\\').Last()}, writting changes. . .");
                                    provsTransfer.Add(str);
                                    string[] allLines = File.ReadAllLines(file);
                                    allLines[allLines.ToList().IndexOf(line) + 1] = allLines[allLines.ToList().IndexOf(line) + 1].Replace($" {str}", string.Empty);
                                    File.WriteAllLines(file, allLines);
                                }
                            }
                        }
                    }
                }
            }
            foreach (string line in File.ReadAllLines(TargetFile).ToArray())
            {
                if (line.Contains("provinces"))
                {
                    Console.WriteLine($"[ChangeStratRegion] Writing Changes in {TargetFile} . . .");
                    string[] allLines = File.ReadAllLines(TargetFile);
                    string targetLine = allLines[allLines.ToList().IndexOf(line) + 1];

                    if (targetLine.EndsWith(" "))
                    {
                        allLines[allLines.ToList().IndexOf(line) + 1] += $"{string.Join(" ", Provinces)}";
                    }
                    else { allLines[allLines.ToList().IndexOf(line) + 1] += $" {string.Join(" ", Provinces)}"; }

                    File.WriteAllLines(TargetFile, allLines);
                    break;
                }
            }
        }
        static void Main()
        {
            while (true)
            {
                try
                {
                    if (ProvinceColours.Count < 1)
                    {
                        UpdateProvinceColours();
                        UpdateStatesProvinces();
                    }
                    if (menuType != 1)
                    {
                        Console.WriteLine($"===== MAIN | STATES MANIPULATION TOOL (v{Version} by Arxy) =====");
                        menuType = 1;
                    }
                    Console.Write("State: ");
                    string[] args = Console.ReadLine().Split(' ');

                    //if (args[0].Equals("statedefinition") || args[0].Equals("statedef"))
                    //{
                    //    Console.Clear();
                    //    StateDefinition();
                    //}
                    if (args[0].Equals("provincedefinition") || args[0].Equals("provdef"))
                    {
                        Console.Clear();
                        ProvinceDefinition();
                    }
                    if (args[0].Equals("strategicregion") || args[0].Equals("sr") || args[0].Equals("stratregion"))
                    {
                        Console.Clear();
                        StrategicRegionMain();
                    }
                    if (args[0].Equals("usearray"))
                    {
                        List<string> States = args.ToList();
                        States.Remove(args[0]);
                        States.TrimExcess();
                        List<string> StatesIDS = new List<string>();
                        foreach (string state in States)
                        {
                            StatesIDS.Add(GetStateByID(Convert.ToInt32(state)));
                        }
                        Console.Clear();
                        UseArray(StatesIDS);
                    }
                    if (args[0].Equals("resourcesentry") || args[0].Equals("resent"))
                    {
                        if (args.Length >= 2)
                        {
                            if (GetStateByID(Convert.ToInt32(args[1])) != null)
                            {
                                string state = GetStateByID(Convert.ToInt32(args[1]));
                                Console.Clear();
                                ResourcesEntry(state);
                            }
                        }
                    }
                    if (args[0].Equals("historyentry") || args[0].Equals("hisent"))
                    {
                        if (args.Length >= 2)
                        {
                            if (GetStateByID(Convert.ToInt32(args[1])) != null)
                            {
                                string state = GetStateByID(Convert.ToInt32(args[1]));
                                Console.Clear();
                                StateHistoryEntry(state);
                            }
                        }
                    }
                    if (args[0].Equals("about"))
                    {
                        Console.WriteLine($"Hearts of Iron 4: Map Modding Tool\nVersion: {Version}\nBy July");
                    }
                    if (args[0].Equals("help"))
                    {
                        if (args.Length == 1)
                        {
                            Console.WriteLine("Available commands:\n" +
                            " - help <categories>\n" +
                            "    > Will display this list. If second argument specified (categories) it will show all vanilla categories names and ids.\n\n" +
                            " - about\n" +
                            "    > About this app.\n\n" +
                            " - resourcesentry/resent [stateID]\n" +
                            "    > Go in state's resource entry editor tab.\n\n" +
                            " - category [stateID] [value]\n" +
                            "    > Set state's category. Support both name and id argument as value.\n\n" +
                            " - historyentry/hisent [stateID]\n" +
                            "    > Go in state's history entry editor tab.\n\n" +
                            " - provincedefinition/provdef\n" +
                            "    > Go in province definition editor tab.\n\n" +
                            " - manpower [stateID] [value]\n" +
                            "    > Set/change state's manpower.\n\n" +
                            " - manpower percent [stateIDtoTakeFrom] [stateID] [valuePercent]\n" +
                            "    > Set/change state's manpower using percentage from first state to second. NOTE: first state has to have at least some manpower in order to work.\n\n" +
                            " - transfer [stateID] [provinces]\n" +
                            "    > Transfer all written provinces to the state. Support array from 1 to 2.147.483.647 (basically infinite) of provinces.\n\n" +
                            " - usearray [statesIDS]\n" +
                            "    > Will open alternate to this menu that allows to edit multiple states at once.\n\n" +
                            " - clear\n" +
                            "    > Clear the mess this app and you wrote here.\n\n" +
                            " - end\n" +
                            "    > Close the app.\n\n" +
                            " - create [Name] [provinces]\n" +
                            "    > Create a state. [Name] will be used in localisation that should be located at localisation\\english\\state_names_l_english.yml. " +
                            "Basically, if your states loc is situated lets say in my_parents_love_me_l_english.yml then it wont work.\n\n" +
                            " - strategicregion\\sr\\stratregion\n" +
                            "    > Go in Strategic Region editor tab.\n\n" +
                            " - statedefinition/statedef (removed)\n" +
                            "    > Go in state definition editor tab.\n\n" +
                            " - givelist , removearray [FileToRemoveFrom]* [FileRemover]* , giveliststate [Exclude/Include/Single]* [RemoveFromDef]\n" +
                            "    > Arxy's 'Dev Commands', mainly serve no use but debugging provinces.\n\n" +
                            " - merge [targetStateID(s)/all] [StateToMergeIn]\n" +
                            "    > Merge selected states. Left into right. Typing \"all\" as first argument wont require second since all states will be merged into the state with the smallest id. " +
                            "[StateToMergeIn] should always be last in the list, the most right.\n");
                        }
                        else
                        {
                            if (args[1].Equals("categories"))
                            {
                                string[] allcat = new string[12];
                                allcat[0] = $"0  - {CategoriesString[0]}";
                                allcat[1] = $"1  - {CategoriesString[1]}";
                                allcat[2] = $"2  - {CategoriesString[2]}";
                                allcat[3] = $"3  - {CategoriesString[3]}";
                                allcat[4] = $"4  - {CategoriesString[4]}";
                                allcat[5] = $"5  - {CategoriesString[5]}";
                                allcat[6] = $"6  - {CategoriesString[6]}";
                                allcat[7] = $"7  - {CategoriesString[7]}";
                                allcat[8] = $"8  - {CategoriesString[8]}";
                                allcat[9] = $"9  - {CategoriesString[9]}";
                                allcat[10] = $"10 - {CategoriesString[10]}";
                                allcat[11] = $"11 - {CategoriesString[11]}";
                                string helpcat = string.Join("\n", allcat);
                                Console.WriteLine($"All state categories: \n{helpcat}");
                            }
                        }
                    }
                    if (args[0].Equals("givelist"))
                    {
                        string[] lines = File.ReadAllLines(@"map\definition.csv");
                        string toReturn = "";
                        string type = args[1];
                        foreach (string line in lines)
                        {
                            if (line.Contains($";{type};"))
                            {
                                string provID = line.Split(';')[0];
                                toReturn += $"{provID} ";
                            }
                        }
                        Console.WriteLine($"[MAIN] returned: {toReturn}");
                    }
                    if (args[0].Equals("removearray"))
                    {
                        string RemoveFrom = args[1];
                        string Remover = args[2];
                        string toReturn = "";
                        List<string> RemoveA = new List<string>();
                        RemoveA.AddRange(File.ReadAllLines(RemoveFrom)[0].Split(' '));
                        List<string> RemoveB = new List<string>();
                        RemoveB.AddRange(File.ReadAllLines(Remover)[0].Split(' '));

                        foreach (string str in RemoveA.ToArray())
                        {
                            if (RemoveB.Contains(str))
                            {
                                RemoveA.Remove(str);
                            }
                        }
                        toReturn = string.Join(" ", RemoveA);

                        Console.WriteLine($"[MAIN] returned: {toReturn}");
                    }
                    if (args[0].Equals("giveliststate"))
                    {
                        string[] files = Directory.GetFiles(@"history\states\");
                        string toReturn = "";
                        string State = args[1];
                        string Type = args[2];
                        string ProvAction = args[3];
                        foreach (string file in files)
                        {
                            if (Type == "Exclude" && !file.Split('\\').Last().Split('-')[0].Equals(State))
                            {
                                foreach (string line in File.ReadAllLines(file))
                                {
                                    if (line.Contains("provinces"))
                                    {
                                        string str = File.ReadAllLines(file)[File.ReadAllLines(file).ToList().IndexOf(line) + 1];
                                        str = str.Trim('	'); //codding space
                                        if (str.EndsWith(" "))
                                        {
                                            toReturn += $"{str}";
                                        }
                                        else
                                        {
                                            toReturn += $"{str} ";
                                        }
                                    }
                                }
                            }
                            else if (Type == "Include")
                            {
                                foreach (string line in File.ReadAllLines(file))
                                {
                                    if (line.Contains("provinces"))
                                    {
                                        string str = File.ReadAllLines(file)[File.ReadAllLines(file).ToList().IndexOf(line) + 1];
                                        str = str.Trim('	'); //codding space
                                        if (str.EndsWith(" "))
                                        {
                                            toReturn += $"{str}";
                                        }
                                        else
                                        {
                                            toReturn += $"{str} ";
                                        }
                                    }
                                }
                            }
                            else if (Type == "Single" && file.Split('\\').Last().Split('-')[0].Equals(State))
                            {
                                foreach (string line in File.ReadAllLines(file))
                                {
                                    if (line.Contains("provinces"))
                                    {
                                        string str = File.ReadAllLines(file)[File.ReadAllLines(file).ToList().IndexOf(line) + 1];
                                        str = str.Trim('	'); //codding space
                                        if (str.EndsWith(" "))
                                        {
                                            toReturn += $"{str}";
                                        }
                                        else
                                        {
                                            toReturn += $"{str} ";
                                        }
                                    }
                                }
                            }
                        }
                        List<string> ProvsA = new List<string>();
                        ProvsA.AddRange(toReturn.Split(' '));
                        List<string> ProvsB = new List<string>();
                        foreach (string line in File.ReadAllLines(@"map\definition.csv"))
                        {
                            if (!line.Contains("unknown"))
                            {
                                ProvsB.Add(line.Split(';')[0]);
                            }
                        }
                        if (ProvAction == "RemoveFromDef")
                        {
                            foreach (string str in ProvsB.ToArray())
                            {
                                if (ProvsA.Contains(str))
                                {
                                    ProvsB.Remove(str);
                                }
                            }
                            toReturn = string.Join(" ", ProvsB);
                        }

                        Console.WriteLine($"[MAIN] returned: {toReturn}");
                    }
                    if (args[0].Equals("category"))
                    {
                        string state = GetStateByID(Convert.ToInt32(args[1]));
                        string Category;
                        string param = "state_category";
                        if (CategoriesString.Contains(args[2]))
                        {
                            Category = args[2];
                        }
                        else
                        {
                            Category = ConvertToStringCategory(Convert.ToInt32(args[2]));
                        }
                        RedactStateParameter(state, Category, param);
                        Console.WriteLine($"[Main] Changed Value Successfully!");
                    }
                    if (args[0].Equals("manpower"))
                    {
                        if (args.Length == 3)
                        {
                            string state = GetStateByID(Convert.ToInt32(args[1]));
                            int value = Convert.ToInt32(args[2]);

                            RedactStateParameter(state, value, args[0]);
                            Console.WriteLine($"[Main] Changed Value Successfully!");
                        }
                        else if (args.Length > 3 && args[1].Equals("percent"))
                        {
                            string FromStateFile = GetStateByID(Convert.ToInt32(args[2]));
                            string ToStateFile = GetStateByID(Convert.ToInt32(args[3]));

                            float percent = (float)Convert.ToDouble(args[4]);
                            string numFromFile = null;
                            string numToFile = null;
                            string[] FromLines = File.ReadAllLines(@"history\states\" + FromStateFile);
                            string[] ToLines = File.ReadAllLines(@"history\states\" + ToStateFile);

                            foreach (string line in FromLines)
                            {
                                if (line.Contains(args[0]))
                                {
                                    numFromFile = line.Split('=')[1];
                                    break;
                                }
                            }
                            foreach (string line in ToLines)
                            {
                                if (line.Contains(args[0]))
                                {
                                    numToFile = line.Split('=')[1];
                                    break;
                                }
                            }

                            int value = (int)((Convert.ToInt32(numFromFile) / 100) * (100 - percent));
                            RedactStateParameter(FromStateFile, value, args[0]);

                            value = (int)(Convert.ToInt32(numToFile) + ((Convert.ToInt32(numFromFile) / 100) * percent));
                            RedactStateParameter(ToStateFile, value, args[0]);
                        }
                    }
                    if (args[0].Equals("transfer"))
                    {
                        string cmd = args[0];
                        List<string> provinces = args.ToList();
                        provinces.Remove(cmd);
                        provinces.RemoveAt(0);
                        provinces.TrimExcess();

                        List<string> StatesFromPaths = GetProvincesinStates(provinces.ToArray(), @"history\states");
                        string StateTo = GetStateByID(Convert.ToInt32(args[1]));
                        if (!StatesFromPaths.Contains(StateTo))
                        {
                            RedactState(StatesFromPaths.ToArray(), StateTo, provinces.ToArray());
                            Console.WriteLine($"[Main] Transfer Successful!");
                        }
                        else
                        {
                            Console.WriteLine("ERROR: one of the provinces is already in the state!");
                        }
                    }
                    if (args[0].Equals("end"))
                    {
                        Environment.Exit(0);
                    }
                    if (args[0].Equals("clear"))
                    {
                        Console.Clear();
                        menuType = 0;
                    }
                    if (args[0].Equals("create"))
                    {
                        string name = args[1];
                        List<string> provinces = args.ToList();
                        provinces.Remove(args[0]);
                        provinces.Remove(name);

                        int stateInt = GetFreeState();
                        List<string> StatesPaths = GetProvincesinStates(provinces.ToArray(), @"history\states");
                        CreateState(name, stateInt, provinces.ToArray(), StatesPaths.ToArray());
                        Console.WriteLine($"[Main] Successfully created state!");
                        Thread.Sleep(4000);
                    }
                    if (args[0].Equals("merge") && args.Length >= 2)
                    {
                        string[] states = args;
                        states.ToList().Remove(args[0]);
                        string toMerge = states.Last();
                        states.ToList().Remove(toMerge);
                        states.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                        if (!args[1].Equals("all") && states.Length >= 2) { TransferStatesProvs(states, toMerge, false); }
                        else if (args[1].Equals("all")) { TransferStatesProvs(states, "1", true); }
                    }
                    Thread.Sleep(1000);
                    Console.WriteLine(" ");
                    Main();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[Main-Error] {e.Message}\n[SOURCE] {e.Source}\n\n{e}\n");
                }
            }
        }
        static void TransferStatesProvs(string[] AllStates, string ToMergeState, bool AllInOne)
        {
            List<string> Provinces = new List<string>();
            string[] files = Directory.GetFiles(@"history\states\");
            string MergeFile = "";
            foreach (string file in files.ToArray())
            {
                Console.WriteLine($"[TransferStatesProvs] Working with {file} . . .");
                string MfileName = file.Split('\\').Last();
                string MstateID = MfileName.Split('-')[0];
                if (ToMergeState == MstateID) 
                {
                    MergeFile = file;
                    foreach (StateInfo Si in StatesInfo)
                    {
                        if (Si.File.Equals(file))
                        {
                            Console.WriteLine($"[TransferStatesProvs] Found Merge file.");
                            Provinces.AddRange(Si.Provinces);
                            break;
                        }
                    }
                }

                if (AllInOne && ToMergeState != file.Split('\\').Last().Split('-')[0])
                {
                    string fileName = file.Split('\\').Last();
                    if (!fileName.StartsWith("1-"))
                    {
                        foreach (StateInfo Si in StatesInfo)
                        {
                            if (Si.File.Equals(file))
                            {
                                Console.WriteLine($"[TransferStatesProvs] Found selected state (ID:{file.Split('\\').Last().Split('-')[0]})");
                                Provinces.AddRange(Si.Provinces);
                                File.Delete(file);
                                break;
                            }
                        }
                    }
                }
                else if (!AllInOne && ToMergeState != file.Split('\\').Last().Split('-')[0])
                {
                    string fileName = file.Split('\\').Last();
                    string stateID = fileName.Split('-')[0];
                    if (AllStates.ToList().Contains(stateID))
                    {
                        foreach (StateInfo Si in StatesInfo)
                        {
                            if (Si.File.Equals(file))
                            {
                                Console.WriteLine($"[TransferStatesProvs] Found selected state (ID:{stateID})");
                                Provinces.AddRange(Si.Provinces);
                                File.Delete(file);
                                break;
                            }
                        }
                    }
                }

            }
            //string line = $"	provinces = {{\n		{string.Join(" ", Provinces)}\n	}}";
            if (!string.IsNullOrEmpty(MergeFile))
            {
                string[] lines = File.ReadAllLines(MergeFile);
                foreach (string str in lines.ToArray())
                {
                    if (str.Contains("provinces"))
                    {
                        Console.WriteLine($"[TransferStatesProvs] Writing lines in {MergeFile.Split('\\').Last()}");
                        lines[lines.ToList().IndexOf(str) + 1] = $"		{string.Join(" ", Provinces)}";
                        File.WriteAllLines(MergeFile, lines);
                        break;
                    }
                }
            }
        }
        static string[] CreateScript(int ID, string[] provinces)
        {
            string JoinProv = string.Join(" ", provinces);
            string[] Lines = new string[10];
            Lines[0] = "state = {";
            Lines[1] = "	id = " + ID;
            Lines[2] = "	name = STATE_" + ID;
            Lines[3] = "	provinces = {";
            Lines[4] = "		" + JoinProv + " ";
            Lines[5] = "	}";
            Lines[6] = "	manpower = 0";
            Lines[7] = "	buildings_max_level_factor = 1.000";
            Lines[8] = "	local_supplies = 0.000";
            Lines[9] = "}";
            return Lines;
        }
        static void CreateLocalisationEntry(string StateName, string LocName)
        {
            string locPath = @"localisation\english\state_names_l_english.yml";

            //StreamWriter sw = new StreamWriter(locPath, true);
            string[] Lines = File.ReadAllLines(locPath).Where(s => !string.IsNullOrEmpty(s)).ToArray();

            string toAdd = "";
            toAdd += $"{StateName}:0 \"{LocName}\"";

            if (!string.IsNullOrEmpty(Lines[Lines.ToList().IndexOf(Lines.Last())]))
            {
                Lines[Lines.ToList().IndexOf(Lines.Last())] += $"\n{toAdd}";
            }
            else
            {
                Lines[Lines.ToList().IndexOf(Lines.Last())] += $" {toAdd}\n";
            }

            StreamReader sr = new StreamReader(locPath, true);
            System.Text.Encoding encoding = sr.CurrentEncoding;
            sr.Close();

            File.WriteAllLines(locPath, Lines, encoding);

            //sw.WriteLine($"\n {StateName}:0 \"{LocName}\"");
            //sw.Flush();
            //sw.Close();
            //File.WriteAllLines(locPath, lines);

            Console.WriteLine($"[CreateLocalisationEntry] Created new Entry for {StateName} \"{LocName}\"");
        }
        static string GetStateByID(int StateID)
        {
            string[] files = Directory.GetFiles(@"history\states");
            string str1 = null;
            foreach (string str in files)
            {
                if (str.Contains($"{StateID}-") || str.Contains($"{StateID} -") || str.Contains($"{StateID} - "))
                {
                    string[] str2 = str.Split('\\');
                    string[] str3 = str2[str2.Length - 1].Split('-');
                    if (str3[0].Equals($"{StateID}") || str3[0].Equals($"{StateID} "))
                    {
                        Console.WriteLine($"[GetStateByID] Found State {str2[str2.Length - 1]} by ID {StateID}");
                        str1 = str2[str2.Length - 1];
                        break;
                    }
                }
            }
            return str1;
        }
        static void RedactStateParameter(string state, float percent, string param)
        {
            string path = @"history\states\" + state;

            Console.WriteLine("[RedactStateParameter] Changing Entries");
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);

                if (str.Contains(param) && str.Contains("="))
                {
                    string[] str2 = str.Split('=');
                    string str3 = str2[str2.Length - 1];
                    str3.Trim();
                    int value = (int)((Convert.ToInt32(str3) / 100) * percent);
                    string str4 = str.Replace(str3, " " + value.ToString());
                    string[] str5 = File.ReadAllLines(path);
                    str5[index] = str4;
                    File.WriteAllLines(path, str5);
                    Console.WriteLine($"[RedactStateParameter] Found an replaced {param} value from {str3} to {value}");
                }
            }
        }
        static void RedactStateParameter(string state, int value, string param)
        {
            bool valueChanged = false;
            string path = @"history\states\" + state;
            int a = File.ReadAllLines(path).Length;
            int b = 0;

            Console.WriteLine("[RedactStateParameter] Changing Entries");
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);

                b++;
                if (str.Contains(param) && str.Contains("="))
                {
                    string[] str2 = str.Split('=');
                    string str3 = str2[str2.Length - 1];
                    str3.Trim();
                    string str4 = str.Replace(str3, " " + value.ToString());
                    string[] str5 = File.ReadAllLines(path);
                    str5[index] = str4;
                    File.WriteAllLines(path, str5);
                    valueChanged = true;
                    Console.WriteLine($"[RedactStateParameter] Found an replaced {param} value from {str3} to {value}");
                }
                if (!str.Contains(param) && b == a && !valueChanged)
                {
                    Console.WriteLine($"[RedactStateParameter] Couldn't find {param} parameter.");
                    foreach (string str1 in File.ReadAllLines(path))
                    {
                        index = File.ReadAllLines(path).ToList().IndexOf(str1);
                        if (str1.Contains("name"))
                        {
                            Console.WriteLine($"[RedactStateParameter] Creating new {param} parameter.");
                            string str2 = File.ReadAllLines(path)[index];
                            string str3 = $"\n	{param} = {value}";
                            string str4 = str2 + str3;
                            string[] str5 = File.ReadAllLines(path);
                            str5[index] = str4;
                            File.WriteAllLines(path, str5);
                            valueChanged = true;
                        }
                    }
                }
            }
        }
        static void RedactStateParameter(string state, string value, string param)
        {
            bool valueChanged = false;
            string path = @"history\states\" + state;
            int a = File.ReadAllLines(path).Length;
            int b = 0;

            Console.WriteLine("[RedactStateParameter] Changing Entries");
            foreach (string str in File.ReadAllLines(path))
            {
                int index = File.ReadAllLines(path).ToList().IndexOf(str);

                b++;
                if (str.Contains(param) && str.Contains("="))
                {
                    string[] str2 = str.Split('=');
                    string str3 = str2[str2.Length - 1];
                    str3.Trim();
                    string str4 = str.Replace(str3, " " + value.ToString());
                    string[] str5 = File.ReadAllLines(path);
                    str5[index] = str4;
                    File.WriteAllLines(path, str5);
                    valueChanged = true;
                    Console.WriteLine($"[RedactStateParameter] Found an replaced {param} value from {str3} to {value}");
                }
                else if (!str.Contains(param) && b == a && !valueChanged)
                {
                    Console.WriteLine($"[RedactStateParameter] Couldn't find {param} parameter.");
                    foreach (string str1 in File.ReadAllLines(path))
                    {
                        index = File.ReadAllLines(path).ToList().IndexOf(str1);
                        if (str1.Contains("name"))
                        {
                            Console.WriteLine($"[RedactStateParameter] Creating new {param} parameter.");
                            string str2 = File.ReadAllLines(path)[index];
                            string str3 = $"\n	{param} = {value}";
                            string str4 = str2 + str3;
                            string[] str5 = File.ReadAllLines(path);
                            str5[index] = str4;
                            File.WriteAllLines(path, str5);
                            valueChanged = true;
                        }
                    }
                }
            }
        }
        static void RedactState(string[] StatesFrom, string StateTo, string[] provinces)
        {
            Console.WriteLine("[RedactState] Changing Entries");
            foreach (string province in provinces)
            {
                //Console.WriteLine($"[RedactState] Took Province {province}");
                foreach (string file in StatesFrom)
                {
                    Console.WriteLine($"[RedactState] Checking Province {province} in {file}");
                    foreach (string str in File.ReadAllLines(file))
                    {
                        int index = File.ReadAllLines(file).ToList().IndexOf(str);
                        string lastIndex = str.Split(' ')[str.Split(' ').Length - 1];

                        if (str.Contains($"	{province} "))
                        {
                            if (File.ReadAllLines(file)[index - 1].Contains("provinces") || File.ReadAllLines(file)[index].Contains("provinces"))
                            {
                                string[] a = file.Split('\\');
                                string FN = a[a.Length - 1];

                                string str2 = str.Replace($"	{province} ", "	");
                                string[] str3 = File.ReadAllLines(file);
                                str3[index] = str2;
                                File.WriteAllLines(file, str3);
                                Console.WriteLine($"[CreateState] Cleared {province} entry at {FN}");
                            }
                        }
                        else if (str.Contains($" {province} "))
                        {
                            if (File.ReadAllLines(file)[index - 1].Contains("provinces") || File.ReadAllLines(file)[index].Contains("provinces"))
                            {
                                string[] a = file.Split('\\');
                                string FN = a[a.Length - 1];

                                string str2 = str.Replace($" {province} ", " ");
                                string[] str3 = File.ReadAllLines(file);
                                str3[index] = str2;
                                File.WriteAllLines(file, str3);
                                Console.WriteLine($"[CreateState] Cleared {province} entry at {FN}");
                            }
                        }
                        else if (str.Contains($" {province}") && lastIndex.Equals($"{province}"))
                        {
                            if (File.ReadAllLines(file)[index - 1].Contains("provinces") || File.ReadAllLines(file)[index].Contains("provinces"))
                            {
                                string[] a = file.Split('\\');
                                string FN = a[a.Length - 1];

                                string str2 = str.Replace($" {province}", " ");
                                string[] str3 = File.ReadAllLines(file);
                                str3[index] = str2;
                                File.WriteAllLines(file, str3);
                                Console.WriteLine($"[CreateState] Cleared {province} entry at {FN}");
                            }
                        }
                    }
                }
            }
            string path = @"history\states\" + StateTo;
            int indexA = 0;
            foreach (string str in File.ReadAllLines(path))
            {
                if (str.Contains("provinces"))
                {
                    indexA = File.ReadAllLines(path).ToList().IndexOf(str) + 1;
                }
            }
            string provincesLine = File.ReadAllLines(path)[indexA];
            string newProvinces = string.Join(" ", provinces);
            string final = "";
            if (provincesLine.ToCharArray()[provincesLine.ToCharArray().Length - 1].Equals(' '))
            {
                final = $"{provincesLine}{newProvinces} ";
            }
            else if (!provincesLine.ToCharArray()[provincesLine.ToCharArray().Length - 1].Equals(' '))
            {
                final = $"{provincesLine} {newProvinces} ";
            }
            string[] allLines = File.ReadAllLines(path);
            allLines[indexA] = final;

            Console.WriteLine($"[RedactState] Wrote new provinces {newProvinces} in {StateTo}");
            File.WriteAllLines(path, allLines);
        }
        static void CreateState(string stateName, int stateID, string[] provinces, string[] ProvinceStates)
        {
            string FileName = $"{stateID}-{stateName}.txt";
            Console.WriteLine("[CreateState] Clearing Entries");
            foreach (string province in provinces)
            {
                //Console.WriteLine($"[CreateState] Took Province {province}");
                foreach (string file in ProvinceStates)
                {
                    Console.WriteLine($"[CreateState] Checking Province {province} in {file}");
                    foreach (string str in File.ReadAllLines(file))
                    {
                        int index = File.ReadAllLines(file).ToList().IndexOf(str);
                        string lastIndex = str.Split(' ')[str.Split(' ').Length - 1];

                        if (str.Contains($"	{province} "))
                        {
                            if (File.ReadAllLines(file)[index - 1].Contains("provinces") || File.ReadAllLines(file)[index].Contains("provinces"))
                            {
                                string[] a = file.Split('\\');
                                string FN = a[a.Length - 1];

                                string str2 = str.Replace($"	{province} ", "	");
                                string[] str3 = File.ReadAllLines(file);
                                str3[index] = str2;
                                File.WriteAllLines(file, str3);
                                Console.WriteLine($"[CreateState] Cleared {province} entry at {FN}");
                            }
                        }
                        if (str.Contains($" {province} "))
                        {
                            if (File.ReadAllLines(file)[index - 1].Contains("provinces") || File.ReadAllLines(file)[index].Contains("provinces"))
                            {
                                string[] a = file.Split('\\');
                                string FN = a[a.Length - 1];

                                string str2 = str.Replace($" {province} ", " ");
                                string[] str3 = File.ReadAllLines(file);
                                str3[index] = str2;
                                File.WriteAllLines(file, str3);
                                Console.WriteLine($"[CreateState] Cleared {province} entry at {FN}");
                            }
                        }
                        if (str.Contains($" {province}") && lastIndex.Equals($"{province}"))
                        {
                            if (File.ReadAllLines(file)[index - 1].Contains("provinces") || File.ReadAllLines(file)[index].Contains("provinces"))
                            {
                                string[] a = file.Split('\\');
                                string FN = a[a.Length - 1];

                                string str2 = str.Replace($" {province}", " ");
                                string[] str3 = File.ReadAllLines(file);
                                str3[index] = str2;
                                File.WriteAllLines(file, str3);
                                Console.WriteLine($"[CreateState] Cleared {province} entry at {FN}");
                            }
                        }
                    }
                }
            }
            string path = @"history\states\" + FileName;

            File.Create(path).Close();
            Console.WriteLine($"[CreateState] Created new State as {FileName}");
            File.WriteAllLines(path, CreateScript(stateID, provinces));

            Console.WriteLine($"[CreateState] Wrote initial script in {FileName}");
            CreateLocalisationEntry($" STATE_{stateID}", stateName);
        }
        static int GetFreeState()
        {
            int FreeCount = 0;
            string path = @"history\states\";
            FreeCount = Directory.EnumerateFiles(path).Count() + 1;
            Console.WriteLine($"[GetFreeState] Defined Free State Number: {FreeCount}");
            return FreeCount;
        }
        static List<string> GetProvincesinStates(string[] provinces, string Path)
        {
            List<string> ReturnStates = new List<string>();
            string[] Files = Directory.GetFiles(Path);
            foreach (string province in provinces)
            {
                foreach (string filePath in Files)
                {
                    foreach (string str in File.ReadAllLines(filePath))
                    {
                        //Console.WriteLine($"[GetProvincesInStates] Reading {str}");
                        int index = File.ReadAllLines(filePath).ToList().IndexOf(str);
                        string lastIndex = str.Split(' ')[str.Split(' ').Length - 1];

                        if (str.Contains($"	{province} ") || str.Contains($" {province} ") && !lastIndex.Equals($"{province}"))
                        {
                            if (File.ReadAllLines(filePath)[index - 1].Contains("provinces") || File.ReadAllLines(filePath)[index].Contains("provinces"))
                            {
                                string[] fL = filePath.Split('\\');
                                string FileLog = fL[fL.Length - 1];
                                Console.WriteLine($"[GetProvincesInStates] Logged province {province} in {FileLog}");
                                if (!ReturnStates.Contains(filePath))
                                {
                                    ReturnStates.Add(filePath);
                                }
                            }
                            //break;
                        }
                        else if ((!str.Contains($" {province}") || !str.Contains($" {province} ")) && lastIndex.Equals($"{province}"))
                        {
                            if (File.ReadAllLines(filePath)[index - 1].Contains("provinces") || File.ReadAllLines(filePath)[index].Contains("provinces"))
                            {
                                string[] fL = filePath.Split('\\');
                                string FileLog = fL[fL.Length - 1];
                                Console.WriteLine($"[GetProvincesInStates] Logged province {province} in {FileLog}");
                                Console.WriteLine($"[GetProvincesInStates] Current index {index} line {str}");
                                Console.WriteLine($"[GetProvincesInStates] Previous index {(index - 1)} line {File.ReadAllLines(filePath)[index - 1]}");
                                if (!ReturnStates.Contains(filePath))
                                {
                                    ReturnStates.Add(filePath);
                                }
                            }
                            //break;
                        }
                    }
                }
            }
            return ReturnStates;
        }
        public static string ConvertToStringCategory(int category)
        {
            string toReturn;
            if (category > -1 && category < CategoriesString.Count + 1)
            {
                toReturn = CategoriesString[category];
            }
            else
            {
                toReturn = "wasteland";
                Console.WriteLine($"[ConvertToStringCategory] State Category returned \"null\", proceeding with value of \"wasteland\".");
            }
            return toReturn;
        }
        public static string ConvertToStringResources(int category)
        {
            string toReturn;
            if (category > -1 && category < Resources.Count + 1)
            {
                toReturn = Resources[category];
            }
            else
            {
                toReturn = "aluminium";
                Console.WriteLine($"[ConvertToStringResources] Resource returned \"null\", proceeding with value of \"aluminium\".");
            }
            return toReturn;
        }
        public static List<string> CategoriesString = new List<string>
        {
            "wasteland",
            "enclave",
            "tiny_island",
            "pastoral",
            "small_island",
            "rural",
            "town",
            "large_town",
            "city",
            "large_city",
            "metropolis",
            "megalopolis"
        };
        public static List<string> Resources = new List<string>
        {
            "aluminium",
            "chromium",
            "oil",
            "rubber",
            "tungsten",
            "steel"
        };
    }
}
