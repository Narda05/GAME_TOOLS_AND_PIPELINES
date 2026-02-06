using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2a
{
    class WeaponCollection : List<Weapon>, IPeristence
    {
        public bool Load(string filename)
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
            catch(Exception ex)
            {
                Console.WriteLine($"Load failed: {ex.Message}");
                return false;
            }
        }

        public bool Save(string filename)
        {
            try
            {
                File.WriteAllLines(filename, this.Select(w => w.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save failed: {ex.Message}");
                return false;
            }
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
