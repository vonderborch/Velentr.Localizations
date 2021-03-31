using System.Collections.Generic;
using NUnit.Framework;
using Velentr.Localizations.LocalizationLoaders;

namespace Velentr.Localizations.Test
{
    public class TestYamlLocalizationLoaderTest
    {
        public string YamlDocument = @"
name: George Washington
age: 89
height_in_inches: 5.75
addresses:
  home:
    street: 400 Mockingbird Lane
    city: Louaryland
    state: Hawidaho
    zip: 99970
addresses2:
  home:
    street: 400 Mockingbird Lane
    city: Louaryland
    state: Hawidaho
    zip: 99970
";

        public Dictionary<string, string> YamlOutput = new Dictionary<string, string>()
        {
            {"name", "George Washington"},
            {"age", "89"},
            {"height_in_inches", "5.75"},
            {"addresses.home.street", "400 Mockingbird Lane"},
            {"addresses.home.city", "Louaryland"},
            {"addresses.home.state", "Hawidaho"},
            {"addresses.home.zip", "99970"},
            {"addresses2.home.street", "400 Mockingbird Lane"},
            {"addresses2.home.city", "Louaryland"},
            {"addresses2.home.state", "Hawidaho"},
            {"addresses2.home.zip", "99970"},
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var loader = new YamlLocalizationLoader();
            var results = loader.LoadLocalizationFromFile(YamlDocument);

            Assert.AreEqual(results, YamlOutput);
        }
    }
}