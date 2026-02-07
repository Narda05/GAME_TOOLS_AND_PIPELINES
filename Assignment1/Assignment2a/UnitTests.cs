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
        private string jsonPath;
        private string xmlPath;
        private string emptyJsonPath;
        private string emptyCsvPath;
        private string emptyXmlPath;


        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";
        const string JSON_FILE = "weapons.json";
        const string XML_FILE = "weapons.xml";
        const string EMPTY_JSON_FILE = "empty.json";
        const string EMPTY_CSV_FILE = "empty.csv";
        const string EMPTY_XML_FILE = "empty.xml";

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
            jsonPath = CombineToAppPath(JSON_FILE);
            xmlPath = CombineToAppPath(XML_FILE);
            emptyJsonPath = CombineToAppPath(EMPTY_JSON_FILE);
            emptyCsvPath = CombineToAppPath(EMPTY_CSV_FILE);
            emptyXmlPath = CombineToAppPath(EMPTY_XML_FILE);
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
            if (File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
            }
            if (File.Exists(xmlPath))
            {
                File.Delete(xmlPath);
            }
            if (File.Exists(emptyJsonPath))
            {
                File.Delete(emptyJsonPath);
            }
            if (File.Exists(emptyCsvPath))
            {
                File.Delete(emptyCsvPath);
            }
            if (File.Exists(emptyXmlPath))
            {
                File.Delete(emptyXmlPath);
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

        // ------------------ JSON serialization unit tests ------------------
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            // Load CSV, Save(...) to JSON, then Load(...) the JSON output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.Save(jsonPath), Is.True, "Saving to JSON via Save should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(jsonPath), Is.True, "Loading saved JSON via Load should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            // Load CSV, SaveAsJSON(...) to weapons.json, then Load(...) the JSON output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.SaveAsJSON(jsonPath), Is.True, "Saving to JSON via SaveAsJSON should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(jsonPath), Is.True, "Loading saved JSON via Load should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            // Load CSV, SaveAsJSON(...) to weapons.json, then LoadJSON(...) the JSON output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.SaveAsJSON(jsonPath), Is.True, "Saving to JSON via SaveAsJSON should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.LoadJSON(jsonPath), Is.True, "Loading saved JSON via LoadJSON should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            // Load CSV, Save(...) to weapons.json, then LoadJSON(...) the JSON output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.Save(jsonPath), Is.True, "Saving to JSON via Save should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.LoadJSON(jsonPath), Is.True, "Loading saved JSON via LoadJSON should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }

        // ------------------ CSV parsing unit tests ------------------

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            // Load CSV, Save(...) to CSV, then Load(...) the CSV output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.Save(outputPath), Is.True, "Saving to CSV via Save should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(outputPath), Is.True, "Loading saved CSV via Load should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            // Load CSV, SaveAsCSV(...) to output.csv, then LoadCSV(...) the CSV output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.SaveAsCSV(outputPath), Is.True, "Saving to CSV via SaveAsCSV should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.LoadCSV(outputPath), Is.True, "Loading saved CSV via LoadCSV should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }


        // ------------------ XML serialization unit tests ------------------

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            // Load CSV, Save(...) to XML, then Load(...) the XML output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.Save(xmlPath), Is.True, "Saving to XML via Save should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(xmlPath), Is.True, "Loading saved XML via Load should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            // Load CSV, SaveAsXML(...) to weapons.xml, then LoadXML(...) the XML output and validate 95 entries.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.SaveAsXML(xmlPath), Is.True, "Saving to XML via SaveAsXML should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.LoadXML(xmlPath), Is.True, "Loading saved XML via LoadXML should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(95), "Reloaded collection should contain 95 entries.");
        }

        // ------------------ Save empty as JSON unit test ------------------

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
            // Create empty WeaponCollection, SaveAsJSON(...) to empty.json, then Load(...) the JSON output and validate 0 entries.
            WeaponCollection.Clear();
            Assert.That(WeaponCollection.SaveAsJSON(emptyJsonPath), Is.True, "Saving empty collection to JSON should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(emptyJsonPath), Is.True, "Loading saved empty JSON via Load should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(0), "Reloaded collection should be empty.");
        }

        // ------------------ Save empty as CSV unit test ------------------

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            // Create empty WeaponCollection, SaveAsCSV(...) to empty.csv, then Load(...) the CSV output and validate 0 entries.
            WeaponCollection.Clear();
            Assert.That(WeaponCollection.SaveAsCSV(emptyCsvPath), Is.True, "Saving empty collection to CSV should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(emptyCsvPath), Is.True, "Loading saved empty CSV via Load should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(0), "Reloaded collection should be empty.");
        }

        // ------------------ Save empty as XML unit test ------------------

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            // Create empty WeaponCollection, SaveAsXML(...) to empty.xml, then Load(...) the XML output and validate 0 entries.
            WeaponCollection.Clear();
            Assert.That(WeaponCollection.SaveAsXML(emptyXmlPath), Is.True, "Saving empty collection to XML should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.Load(emptyXmlPath), Is.True, "Loading saved empty XML via Load should succeed.");
            Assert.That(reloaded.Count, Is.EqualTo(0), "Reloaded collection should be empty.");
        }

        // ------------------ Invalid format tests ------------------

        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            // Save JSON, then try to load it with LoadXML => should fail and produce empty collection.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.SaveAsJSON(jsonPath), Is.True, "Saving collection as JSON should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.LoadXML(jsonPath), Is.False, "Loading a JSON file with LoadXML should fail.");
            Assert.That(reloaded.Count, Is.EqualTo(0), "Reloaded collection should be empty after failed LoadXML.");
        }

        [Test]
        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            // Save XML, then try to load it with LoadJSON => should fail and produce empty collection.
            Assert.That(WeaponCollection.Load(inputPath), Is.True, "Initial CSV load should succeed.");
            Assert.That(WeaponCollection.SaveAsXML(xmlPath), Is.True, "Saving collection as XML should succeed.");

            var reloaded = new WeaponCollection();
            Assert.That(reloaded.LoadJSON(xmlPath), Is.False, "Loading an XML file with LoadJSON should fail.");
            Assert.That(reloaded.Count, Is.EqualTo(0), "Reloaded collection should be empty after failed LoadJSON.");
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            // Attempt to LoadXML on a CSV file => should fail and produce empty collection.
            Assert.That(WeaponCollection.LoadXML(inputPath), Is.False, "LoadXML on a CSV file should fail.");
            Assert.That(WeaponCollection.Count, Is.EqualTo(0), "Collection should be empty after failed LoadXML.");
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            // Attempt to LoadJSON on a CSV file => should fail and produce empty collection.
            Assert.That(WeaponCollection.LoadJSON(inputPath), Is.False, "LoadJSON on a CSV file should fail.");
            Assert.That(WeaponCollection.Count, Is.EqualTo(0), "Collection should be empty after failed LoadJSON.");
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
