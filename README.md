# PREREQUISITES
_outdated_
- SDL Trados 2009 SP1 (or latest, hopefully)
- SDL SDK

# BUILD
Using Visual studio:
1. Open SDL Trados 2009 plugin.sln solution file with Microsoft Visual Studio 2013 or newer.
2. Select Build\Build Solution.

Or using msbuild directly: 
```
msbuild LetsMT.MTProvider.csproj /p:Configuration=v2015_Release
```

# INSTALLATION
Copy to built package file to apropriate directory acording to SDL Trados version

# Tilde Machine Translation Provider for SDL Trados Studio
## A custom build process
The project contains build configurations for the following plugin versions:
- TildeMT;
- MTPro;
- EU Presidency.

And it supports the following SDL Trados Studio Versions:
- 2009/2010;
- 2014;
- 2015/2017.

This is achieved by a customised build process that uses the same source to build the
solution but uses pre-build steps to ensure that:
1. the correct SDL SDK dlls are used (to suit the Trados version being built);
2. the correct resources are used (to suit the plugin version being built);
3. the correct assembly information and information in the `pluginpackage.manifest.xml`
   file is used for each of the plugin versions.

The dlls are handled by the [ReferencesCopy](Custom_targets/ReferencesCopy.targets)
custom build target which locates the correct SDL SDK directory and copies the dlls
found there to the [SDL_references](SDL_references) folder.

For each of the plugin versions a seperate `AssemblyInfo` file is maintained containing the
plugin-specific assembly information. The build process also re-defines the `AssemblyName`
attribute for the EU Presidency plugin version.

The resources are handled by maintaining a seperate `PluginResources.<version sufix>.resx`
file for each of the plugin versions. The appropriate resource file is then swapped in place
of the main `PluginResources.resx` file before the build. Resources common to all plugin
versions are placed in `Properties/Resources.resx`. See the `BeforeBuild` target in
[LetsMT.MTProvider.csproj](LetsMT.MTProvider.csproj) for more details.

The same is done for the `pluginresourcse.manifest.xml` file.

**_Note_** that the `PluginResources.resx` and `pluginpackage.manifest.xml` files
shouldn't be directly edited and they are ignored by git.