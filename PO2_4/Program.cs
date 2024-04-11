using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO2_4
{
    internal class Program
    {
        static void parse(SortedDictionary<string, string[]> map, StreamReader f)
        {
            string Category = "";
              while(!f.EndOfStream)
            {
                string line = f.ReadLine();
                if (line[0] != '-')
                {
                    throw new FormatException();
                }
                if (line[1] == '-')
                {
                    if (Category == "")
                    {
                        throw new FormatException();
                    }
                    if (map.ContainsKey(Category))
                    {
                        string ZahirKebab = line.Substring(2);
                        string[] prods;
                        map.TryGetValue(Category, out prods);
                        prods.Append(ZahirKebab);
                        map.Add(Category, prods);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                Category = line.Substring(1);
                string[] kok = { };
                map.Add(line, kok);
            }
        }

        static void PrintCategory(SortedDictionary<string, string[]> map, string category)
        {
            Console.WriteLine("*" + category + ":");
            if (!map.ContainsKey(category))
            {
                throw new ArgumentException("Category does not exist!");
            }
            string[] strings = map[category];
            if (strings.Length == 0)
            {
                Console.WriteLine("No items in this category");
            }
            for (int i = 0; i < map[category].Length; i++)
            {
                Console.WriteLine(map[category]);
            }
        }

        static void PrintCategories(SortedDictionary<string, string[]> Map)
        {
            foreach (string category in Map.Keys)
            {
                Console.WriteLine(category);
            }
        }

        static void Print(SortedDictionary<string, string[]> Map)
        {
            foreach (string category in Map.Keys)
            {
                PrintCategory(Map, category);
            }
        }

        static void AddCategory(SortedDictionary<string, string[]> Map, string category)
        {
            string[] m = { };
            Map.Add(category, m);
        }

        static void AddItem(SortedDictionary<string, string[]> Map, string category, string item)
        {
            string[] m = { };
            if (Map.ContainsKey(category))
            {
                m = Map[category];
            }
            m.Append(item);
            Map.Add(category, m);
        }

        static void RemoveCategory(SortedDictionary<string, string[]> Map, string category)
        {
            Map.Remove(category);
        }

        static void RemoveAllItems(SortedDictionary<string, string[]> Map)
        {
            foreach(string category in Map.Keys)
            {
                RemoveCategory(Map, category);
            }
        }

        static void RemoveItem(SortedDictionary<string, string[]> Map, string category, string item)
        {
            if (!Map.ContainsKey(category))
            {
                throw new ArgumentException();
            }
            Map[category] = Map[category].Where(N => N != item).ToArray();
        }

        static void SaveTofile(SortedDictionary<string, string[]> Map, string filename)
        {
            StreamWriter f = new StreamWriter(filename);
            foreach (string category in Map.Keys)
            {
                f.WriteLine("-" + category);
                foreach (string  item in Map[category])
                {
                    f.WriteLine("--" + item);
                }
            }
        }

        static void Main(string[] args)
        {
            StreamReader r;
            try
            {
                r = new StreamReader(args[0]);
            } catch (FileNotFoundException e) { 
                Console.WriteLine("File not found");
                return;
            }
            SortedDictionary<string, string[]> Map = new SortedDictionary<string, string[]>();
            try
            {
                parse(Map, r);
            } catch (FormatException e) {
                Console.WriteLine("File corrupted");
                return;
            }
            bool exit = false;
            while  (!exit)
            {
                Console.WriteLine("Pick what you wish to do:\n" +
                        "1. Add category/product\n" +
                        "2. Show all categories&products\n" +
                        "3. Show all products in category\n" +
                        "4. Remove all products from list\n" +
                        "5. Delete a product from specified category\n" +
                        "6. Remove all items from a category\n" +
                        "7. Save list to file\n" +
                        "8. Exit");
                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                } catch (FormatException e)
                {
                    Console.WriteLine("Incorrect input");
                    continue;
                }
                string Category, Product;
                switch (choice)
                {
                    case 1:
                        PrintCategories(Map);
                        Console.WriteLine("Enter category name: ");
                        Category = Console.ReadLine();

                        Console.WriteLine("Enter product name: ");
                        Product = Console.ReadLine();
                        AddItem(Map, Category, Product);
                        break;
                    case 2:
                        PrintCategories(Map);
                        break;
                    case 3:
                        PrintCategories(Map);
                        Console.WriteLine("Enter category name: ");
                        Category = Console.ReadLine();
                        PrintCategory(Map, Category);
                        break; 
                    case 4:
                        RemoveAllItems(Map);
                        break;
                    case 5:
                        PrintCategories(Map);
                        Console.WriteLine("Enter category name: ");
                        Category = Console.ReadLine();

                        Console.WriteLine("Enter product name: ");
                        Product = Console.ReadLine();
                        try
                        {
                            RemoveItem(Map, Category, Product);
                        } catch (ArgumentException e)
                        {
                            Console.WriteLine("Incorrect input data");
                        }
                        break;
                    case 6:
                        PrintCategories(Map);
                        Console.WriteLine("Enter category name: ");
                        Category = Console.ReadLine();
                        RemoveCategory(Map, Category);
                        break;
                    case 7:
                        SaveTofile(Map, args[1]);
                        break;
                    case 8:
                        SaveTofile(Map, args[1]);
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Incorrect input data");
                        break;
                }
            }
        }
    }
}
