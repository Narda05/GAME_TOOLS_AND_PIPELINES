using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponLib
{
    public class Weapon
    {
        public enum WeaponType
        {
            Sword,
            Polearm,
            Claymore,
            Catalyst,
            Bow,
            None
        }
        // Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string Image { get; set; }
        public string SecondaryStat { get; set; }
        public string Passive { get; set; }

        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left is null) return -1;
            if (right is null) return 1;
            return string.Compare(left.Name, right.Name, StringComparison.Ordinal);
        }

        // TODO: add sort for each property:
        // CompareByType
        public static int CompareByType(Weapon left, Weapon right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left is null) return -1;
            if (right is null) return 1;
            return Comparer<WeaponType>.Default.Compare(left.Type, right.Type);
        }

        // CompareByRarity
        public static int CompareByRarity(Weapon left, Weapon right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left is null) return -1;
            if (right is null) return 1;
            return Comparer<int>.Default.Compare(left.Rarity, right.Rarity);
        }

        // CompareByBaseAttack
        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left is null) return -1;
            if (right is null) return 1;
            return Comparer<int>.Default.Compare(left.BaseAttack, right.BaseAttack);
        }


        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            // TODO: construct a comma seperated value string
            // Name,Type,Rarity,BaseAttack
            // Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive

            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public static bool TryParseType(string rawData, out Weapon weapon)
        {
            string[] values = rawData.Split(',');
            weapon = new Weapon();
            weapon.Name = values[0];
            weapon.SecondaryStat = values[5];
            weapon.Passive = values[6];

            if (Enum.TryParse(values[1], out WeaponType type))
            {
                weapon.Type = type;
            }
            else
            {
                Console.WriteLine($"Type {values[1]} is invalid format");
                weapon = null;
                return false;
            }
            weapon.Image = values[2];

            if (int.TryParse(values[3], out int rarity))
            {
                weapon.Rarity = rarity;
            }
            else
            {
                Console.WriteLine($"Rarity {values[3]} is invalid format");
                weapon = null;
                return false;
            }
            if (int.TryParse(values[4], out int baseAttack))
            {
                weapon.BaseAttack = baseAttack;
            }
            else
            {
                Console.WriteLine($"BaseAttack Attack {values[0]} is invalid format");
                weapon = null;
                return false;
            }
            return true;
        }
    }
}
