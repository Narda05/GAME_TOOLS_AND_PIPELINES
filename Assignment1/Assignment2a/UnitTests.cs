using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assignment2a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection WeaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            WeaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            // Expected Value: 48
            // TODO: call WeaponCollection.GetHighestBaseAttack() and confirm that it matches the expected value using asserts.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Load should succeed for existing data file.");
            Assert.That(WeaponCollection.GetHighestBaseAttack(), Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            // Expected Value: 23
            // TODO: call WeaponCollection.GetLowestBaseAttack() and confirm that it matches the expected value using asserts.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Load should succeed for existing data file.");
            Assert.That(WeaponCollection.GetLowestBaseAttack(), Is.EqualTo(23));
        }

        //[TestCase(WeaponType.Sword, 21)]
        //public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(WeaponType type, int expectedValue)
        //{
        //    // TODO: call WeaponCollection.GetAllWeaponsOfType(type) and confirm that the weapons list returns Count matches the expected value using asserts.
        //}

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfRarity(stars) and confirm that the weapons list returns Count matches the expected value using asserts.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Load should succeed for existing data file.");
            var list = WeaponCollection.GetAllWeaponsOfRarity(stars);
            Assert.That(list.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            // TODO: load returns true, expect WeaponCollection with count of 95 .
            Assert.That(WeaponCollection.Load(inputPath), Is.True);
            Assert.That(WeaponCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            // TODO: load returns false, expect an empty WeaponCollection
            var badPath = CombineToAppPath("file_that_does_not_exist.csv");
            Assert.That(WeaponCollection.Load(badPath), Is.False);
            Assert.That(WeaponCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            // TODO: save returns true, load returns true, and WeaponCollection is not empty.
            Assert.That(WeaponCollection.Load(inputPath), Is.True);
            Assert.That(WeaponCollection.Count, Is.GreaterThan(0));

            Assert.That(WeaponCollection.Save(outputPath), Is.True);

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(outputPath), Is.True);
            Assert.That(reloaded.Count, Is.GreaterThan(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            WeaponCollection.Clear();
            Assert.That(WeaponCollection.Save(outputPath), Is.True);

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(outputPath), Is.True);
            Assert.That(reloaded.Count, Is.EqualTo(0));
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            // TODO: create a Weapon with the stats above set properly
            // TODO: uncomment this once you added the Type1 and Type2
            var expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = Weapon.WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented.
            //Assert.That(Weapon.TryParse(line, out actual));
            Assert.That(Weapon.TryParseType(line, out actual), Is.True);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Type, Is.EqualTo(expected.Type));
            Assert.That(actual.Image, Is.EqualTo(expected.Image));
            Assert.That(actual.Rarity, Is.EqualTo(expected.Rarity));
            Assert.That(actual.BaseAttack, Is.EqualTo(expected.BaseAttack));

            // TODO: check for the rest of the properties, Image,Rarity,SecondaryStat,Passive
            Assert.That(actual.SecondaryStat, Is.EqualTo(expected.SecondaryStat));
            Assert.That(actual.Passive, Is.EqualTo(expected.Passive));
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null.
            string invalid = "1,Bulbasaur,A,B,C,65,65";
            Weapon parsed = null;
            Assert.That(Weapon.TryParseType(invalid, out parsed), Is.False);
            Assert.That(parsed, Is.Null);
        }
    }
}
