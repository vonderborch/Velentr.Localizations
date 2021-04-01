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

var localizedText = localizationSystem.GetLocalization("key", "en");
Console.WriteLine(localizedText);
```

# Extensibility
Velentr.Localizations comes with two built-in localization loaders, the `YamlLocalizationLoader` and the `KeyValuePairXmlLocalizationLoader`. Other loaders can be used instead by creating a new loader inheriting from the `LocalizationLoader` abstract class, then when initializing the LocalizationSystem just pass in the new loader.

# Loading Localization Files
Although you can load localizations elsewhere and add in the individual strings by using the `localizationSystem.AddLocalization()` method, the preferred method of loading a localization file is to use either the `LoadLocalization` or `AddLocalizations` methods:
- `LoadLocalization`: Takes in a filepath, file contents, or a file stream, then will read the contents and add all of the localizations from the file to the requested locale using the configured LocalizationLoader.
- `AddLocalizations`: Takes in a dictionary of key/value pairs and adds all of the localizations to the requested locale. This approach is useful if you want to handle loading the localizations using a different system that isn't compatible with the built-in loader system (i.e. using a database).

# Future Plans
See list of issues under the Milestones: https://github.com/vonderborch/Velentr.Localizations/milestones
