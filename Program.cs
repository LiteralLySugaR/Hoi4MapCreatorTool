using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace HoI4MapCreatorTool
{
    class Program
    {
        public static string Version = "1.6.1";
        public static List<string> Entries = new List<string>
        {
            "add_core_of",
            "victory_points",
            "owner",
            "set_demilitarized_zone",
            "add_claim_by"
        };
        public static int menuType = 0;
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
                    Console.WriteLine("Available commands:\n - help <resources>\n - edit [entry] [value]\n - end\n - clear\n - create\n - check\n - add [entry] [value]\n - remove [entry]\n - return\n - returnmain\n");
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
                    Console.WriteLine("Available commands:\n - help <entries>\n - resourcesentry\n - end\n - clear\n - create\n - check\n - setowner [countryTAG]\n - add [entry] [value]\n - remove [entry] <EntryValue> (leave EntryValue empty to remove all entries of defined type)\n - return\n");
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
                    Console.WriteLine("Available commands:\n - help <entries>\n - resourcesentry\n - showarray\n - clear\n - create\n - check\n - end\n - remove [stateID]\n - setowner [countryTAG]\n - add [entry] [value]\n - remove [entry] <EntryValue> (leave EntryValue empty to remove all entries of defined type)\n - return\n");
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
                    " - clear\n" +
                    " - end\n" +
                    " - return\n" +
                    " - createLandType/clt [TerrainInput] [outputFileName] <MinX-MinY> <MaxX-MaxY> (example: map/TerrainInput2.bmp newDefinition 338-565 2724-1587)");
                }
                else
                {
                    if (args[1].Equals("createLandType") || args[1].Equals("clt"))
                    {
                        Console.WriteLine("This command requires province map (map/provinces.bmp), a Terrain Input File path and output file name. Optional you can set starting pixel to check from and last pixel position to check (\"-\" BETWEEN X AND Y IS REQUIRED), otherwise it will take longer to check since it will check EACH pixel. The Output file will ALWAYS be created as a .txt file.");
                    }
                }
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
        static List<PixelInfo> ReadPixelFromProvinces(string PathTerrainInput)
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
        static List<PixelInfo> ReadPixelFromProvinces(string PathTerrainInput, string MinXY, string MaxXY)
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
        static List<PixelInfo> ReadPixelFromProvinces(string PathTerrainInput, string MinXY)
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
            public Color PlainsColor = Color.FromArgb(255, 129, 66);
            public Color HillssColor = Color.FromArgb(248, 255, 153);
            public Color MountainColor = Color.FromArgb(157, 192, 208);
            public Color UrbanColor = Color.FromArgb(120, 120, 120);
            public Color MarshColor = Color.FromArgb(76, 96, 35);
            public Color DesertColor = Color.FromArgb(255, 127, 0);
            public Color ForestColor = Color.FromArgb(89, 199, 85);
            public Color JungleColor = Color.FromArgb(127, 191, 0);
        }
        public class PixelInfo
        {
            public int X;
            public int Y;
            public string RGB;
            public string Type;
            public int ID;
        }
        static void Main()
        {
            while (true)
            {
                try
                {
                    if (menuType != 1)
                    {
                        Console.WriteLine($"===== MAIN | STATES MANIPULATION TOOL (v{Version} by July) =====");
                        menuType = 1;
                    }
                    Console.Write("State: ");
                    string[] args = Console.ReadLine().Split(' ');

                    if (args[0].Equals("provincedefinition") || args[0].Equals("provdef"))
                    {
                        Console.Clear();
                        ProvinceDefinition();
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
                        Console.WriteLine($"Hearts of Iron 4: State Tool\nVersion: {Version}\nBy July");
                    }
                    if (args[0].Equals("help"))
                    {
                        if (args.Length == 1)
                        {
                            Console.WriteLine("Available commands:\n" +
                            " - help <categories>\n" +
                            " - about\n" +
                            " - resourcesentry/resent [stateID]\n" +
                            " - category [stateID] [value]\n" +
                            " - historyentry/hisent [stateID]\n" +
                            " - provincedefinition/provdef\n" +
                            " - manpower [stateID] [value]\n" +
                            " - transfer [stateID] [provinces]\n" +
                            " - usearray [statesIDS]\n" +
                            " - clear\n" +
                            " - end\n" +
                            " - create [Name] [provinces]");
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
                        string state = GetStateByID(Convert.ToInt32(args[1]));
                        int value = Convert.ToInt32(args[2]);

                        RedactStateParameter(state, value, args[0]);
                        Console.WriteLine($"[Main] Changed Value Successfully!");
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
                    Thread.Sleep(1000);
                    Console.WriteLine(" ");
                    Main();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[Main-Error] {e}");
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

            StreamWriter sw = new StreamWriter(locPath, true);
            sw.WriteLine($"{StateName}:0 \"{LocName}\"");
            sw.Flush();
            sw.Close();
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
                if (str.Contains(param))
                {
                    if (str.Contains(" = "))
                    {
                        string[] str2 = str.Split(' ');
                        string str3 = str2[str2.Length - 1];
                        string str4 = str.Replace(str3, value.ToString());
                        string[] str5 = File.ReadAllLines(path);
                        str5[index] = str4;
                        File.WriteAllLines(path, str5);
                        valueChanged = true;
                        Console.WriteLine($"[RedactStateParameter] Found an replaced {param} value from {str3} to {value}");
                    }
                    else
                    {
                        string[] str2 = str.Split('=');
                        string str3 = str2[str2.Length - 1];
                        string str4 = str.Replace(str3, value.ToString());
                        string[] str5 = File.ReadAllLines(path);
                        str5[index] = str4;
                        File.WriteAllLines(path, str5);
                        valueChanged = true;
                        Console.WriteLine($"[RedactStateParameter] Found an replaced {param} value from {str3} to {value}");
                    }
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
                if (str.Contains(param))
                {
                    if (str.Contains(" = "))
                    {
                        string[] str2 = str.Split(' ');
                        string str3 = str2[str2.Length - 1];
                        string str4 = str.Replace(str3, value.ToString());
                        string[] str5 = File.ReadAllLines(path);
                        str5[index] = str4;
                        File.WriteAllLines(path, str5);
                        valueChanged = true;
                        Console.WriteLine($"[RedactStateParameter] Found an replaced {param} value from {str3} to {value}");
                    }
                    else
                    {
                        string[] str2 = str.Split('=');
                        string str3 = str2[str2.Length - 1];
                        string str4 = str.Replace(str3, value.ToString());
                        string[] str5 = File.ReadAllLines(path);
                        str5[index] = str4;
                        File.WriteAllLines(path, str5);
                        valueChanged = true;
                        Console.WriteLine($"[RedactStateParameter] Found an replaced {param} value from {str3} to {value}");
                    }
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
                Console.WriteLine($"[ConvertToStringCategory] State Category Returned \"null\", proceeding with value of \"wasteland\".");
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
                Console.WriteLine($"[ConvertToStringResources] State Category Returned \"null\", proceeding with value of \"aluminium\".");
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
            "megapolis"
        };
        public static List<string> Resources = new List<string>
        {
            "aluminium",
            "chromium",
            "oil",
            "rubber",
            "tungsten",
            "steel",
        };
    }
}
