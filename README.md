# Velentr.Localizations
A simple and easy-to-use localization helper library.

# Installation
A nuget package is available: [Velentr.Localizations](https://www.nuget.org/packages/Velentr.Localizations/)

# Basic Usage
```
LocalizationSystem localizationSystem = new LocalizationSystem("en");

localizationSystem.AddLanguage("es", "en");
localizationSystem.AddLocalization("key", "hello world!", "en");
localizationSystem.AddLocalization("key", "hola mundo!", "es");

var localizedTextEs = localizationSystem.GetLocalization("key", "es");
var localizedTextEn = localizationSystem.GetLocalization("key", "en");
Console.WriteLine(localizedTextEn);
Console.WriteLine(localizedTextEs);
// "hello world!"
// "hola mundo!"
```

# Machine Translations
Machine translations can be enabled when adding a language by setting the `enableMachineTranslation` parameter when adding a new language. When this parameter is set, we'll reach out to the configured [Apertium](https://apertium.org/) client (which can be configured using the `localizationSystem.ConfigureApertiumClient()` method) to generate machine translations for any text that isn't already cached for that language. The language we'll count as translating from will be the `defaultLocale` for the language and the language we'll translate to will be the `locale` language.

### Example:
```
var localizationSystem = new LocalizationSystem("eng");
localizationSystem.AddLanguage("spa", "eng", enableMachineTranslation: true);
localizationSystem.AddLocalization("key", "hello world!", "eng");

var localizedTextEng = localizationSystem.GetLocalization("key", "eng");
var localizedTextSpa = localizationSystem.GetLocalization("key", "spa");
Console.WriteLine(localizedTextEng);
Console.WriteLine(localizedTextSpa);
// "hello world!"
// "hola mundo!"
```

### Notes
- Locales must be specified in the three-letter codes that Apertium uses. To see all valid pairs on the configured Apertium client you can use the `localizationSystem.Apertium.GetValidPairs()` method.
- Please make sure to follow any/all licenses that are applicable for your projects as per the licenses of [Apertium](https://github.com/apertium) if utilizing this functionality!
- Other methods of the Apertium client can be used as per the [client's documentation](https://github.com/vonderborch/Apertium.Net), but referenced via `localizationSystem.Apertium`.
- Support for other translation methods will come in a future update.

# Extensibility
Velentr.Localizations comes with two built-in localization loaders, the `YamlLocalizationLoader` and the `KeyValuePairXmlLocalizationLoader`. Other loaders can be used instead by creating a new loader inheriting from the `LocalizationLoader` abstract class, then when initializing the LocalizationSystem just pass in the new loader.

# Loading Localization Files
Although you can load localizations elsewhere and add in the individual strings by using the `localizationSystem.AddLocalization()` method, the preferred method of loading a localization file is to use either the `LoadLocalization` or `AddLocalizations` methods:
- `LoadLocalization`: Takes in a filepath, file contents, or a file stream, then will read the contents and add all of the localizations from the file to the requested locale using the configured LocalizationLoader.
- `AddLocalizations`: Takes in a dictionary of key/value pairs and adds all of the localizations to the requested locale. This approach is useful if you want to handle loading the localizations using a different system that isn't compatible with the built-in loader system (i.e. using a database).

# Future Plans
See list of issues under the Milestones: https://github.com/vonderborch/Velentr.Localizations/milestones
