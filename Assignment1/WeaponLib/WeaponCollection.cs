using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeaponLib;

namespace WeaponLib
{
    internal class WeaponCollection : List<Weapon>, IPeristence, IXmlSerializable, IJsonSerializable, ICsvSerializable
    {
        public bool Load(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Load failed: filename is null or empty.");
                return false;
            }

            var ext = Path.GetExtension(filename).ToLowerInvariant();
            return ext switch
            {
                ".xml" => LoadXML(filename),
                ".json" => LoadJSON(filename),
                ".csv" => LoadCSV(filename),
                "" => LoadCSV(filename), // default to CSV if no extension provided
                _ => UnsupportedLoadExtension(filename, ext)
            };
        }

        public bool Save(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Save failed: filename is null or empty.");
                return false;
            }

            var ext = Path.GetExtension(filename).ToLowerInvariant();
            return ext switch
            {
                ".xml" => SaveAsXML(filename),
                ".json" => SaveAsJSON(filename),
                ".csv" => SaveAsCSV(filename),
                "" => SaveAsCSV(filename), // default to CSV if no extension provided
                _ => UnsupportedSaveExtension(filename, ext)
            };
        }

        // Implementation of IXmlSerializable, IJsonSerializable, and ICsvSerializable

        // CSV 
        public bool LoadCSV(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"File not found: {filename}");
                    return false;
                }

                Clear();

                foreach (var line in File.ReadLines(filename))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Use the existing parser on Weapon to obtain an instance (no `new Weapon()` here)
                    if (Weapon.TryParseType(line, out var weapon))
                    {
                        Add(weapon);
                    }
                    else
                    {
                        // parsing already logs details; skip malformed line
                        continue;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadCSV failed: {ex.Message}");
                return false;
            }
        }

        public bool SaveAsCSV(string filename)
        {
            try
            {
                File.WriteAllLines(filename, this.Select(w => w.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveAsCSV failed: {ex.Message}");
                return false;
            }
        }

        //XML
        public bool LoadXML(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"File not found: {filename}");
                    return false;
                }

                var serializer = new XmlSerializer(typeof(List<Weapon>), new XmlRootAttribute("Weapons"));

                using var stream = File.OpenRead(filename);
                if (serializer.Deserialize(stream) is List<Weapon> list)
                {
                    Clear();
                    AddRange(list);
                    return true;
                }

                Console.WriteLine("LoadXML failed: deserialized data was not a list of Weapon.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadXML failed: {ex.Message}");
                return false;
            }
        }

        public bool SaveAsXML(string filename)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Weapon>), new XmlRootAttribute("Weapons"));
                using var stream = File.Create(filename);
                // Serialize a simple List<Weapon> to keep XML shape straightforward
                serializer.Serialize(stream, this.ToList());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveAsXML failed: {ex.Message}");
                return false;
            }
        }


        // JSON
        private static JsonSerializerOptions JsonOptions()
                   => new JsonSerializerOptions
                   {
                       WriteIndented = true,
                       Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                   };

        public bool LoadJSON(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"File not found: {filename}");
                    return false;
                }

                var json = File.ReadAllText(filename);
                var list = JsonSerializer.Deserialize<List<Weapon>>(json, JsonOptions());
                if (list is null)
                {
                    Console.WriteLine("LoadJSON failed: deserialized to null.");
                    return false;
                }

                Clear();
                AddRange(list);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadJSON failed: {ex.Message}");
                return false;
            }
        }

        public bool SaveAsJSON(string filename)
        {
            try
            {
                var json = JsonSerializer.Serialize(this.ToList(), JsonOptions());
                File.WriteAllText(filename, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveAsJSON failed: {ex.Message}");
                return false;
            }
        }

        public bool SaveXML(string path) => SaveAsXML(path);
        public bool SaveJSON(string path) => SaveAsJSON(path);
        public bool SaveCSV(string path) => SaveAsCSV(path);

        // Helpers 
        private bool UnsupportedLoadExtension(string filename, string ext)
        {
            Console.WriteLine($"Load failed: unsupported file extension '{ext}' for file '{filename}'.");
            return false;
        }

        private bool UnsupportedSaveExtension(string filename, string ext)
        {
            Console.WriteLine($"Save failed: unsupported file extension '{ext}' for file '{filename}'.");
            return false;
        }
        public int GetHighestBaseAttack()
        {
            return this.Count == 0 ? 0 : this.Max(w => w.BaseAttack);
        }

        public int GetLowestBaseAttack()
        {
            return this.Count == 0 ? 0 : this.Min(w => w.BaseAttack);
        }

        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType type)
        {
            return this.Where(w => w.Type == type).ToList();
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            return this.Where(w => w.Rarity == stars).ToList();
        }

        public void SortBy(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                Sort(Weapon.CompareByName);
                return;
            }

            switch (columnName.Trim().ToLowerInvariant())
            {
                case "name":
                    Sort(Weapon.CompareByName);
                    break;
                case "type":
                    Sort(Weapon.CompareByType);
                    break;
                case "rarity":
                    Sort(Weapon.CompareByRarity);
                    break;
                case "baseattack":
                case "base attack":
                case "attack":
                    Sort(Weapon.CompareByBaseAttack);
                    break;
                default:
                    // unknown column -> default to name
                    Sort(Weapon.CompareByName);
                    break;
            }
        }
    }
}
