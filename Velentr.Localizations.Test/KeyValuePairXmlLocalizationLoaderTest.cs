using System.Collections.Generic;
using NUnit.Framework;
using Velentr.Localizations.LocalizationLoaders;

namespace Velentr.Localizations.Test
{
    public class KeyValuePairXmlLocalizationLoaderTest
    {

        public string XmlDocument = @"
<Localization>
  <Text Name=""name"">George Washington</Text>
  <Text Name=""age"">89</Text>
  <Text Name=""height_in_inches"">5.75</Text>
  <Text Name=""addresses.home.street"">400 Mockingbird Lane</Text>
  <Text Name=""addresses.home.city"">Louaryland</Text>
  <Text Name=""addresses.home.state"">Hawidaho</Text>
  <Text Name=""addresses.home.zip"">99970</Text>
  <Text Name=""addresses2.home.street"">400 Mockingbird Lane</Text>
  <Text Name=""addresses2.home.city"">Louaryland</Text>
  <Text Name=""addresses2.home.state"">Hawidaho</Text>
  <Text Name=""addresses2.home.zip"">99970</Text>
</Localization>
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
        public void Setup() { }

        [Test]
        public void Test1()
        {
            var loader = new KeyValuePairXmlLocalizationLoader();
            var results = loader.LoadLocalizationFromFile(XmlDocument);

            Assert.AreEqual(results, YamlOutput);
        }

    }
}