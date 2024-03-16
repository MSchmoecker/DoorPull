# DoorPull

## About

Pull doors open with Ctrl + Use key.

![showcase](https://raw.githubusercontent.com/MSchmoecker/DoorPull/master/Docs/PullShowcase.mp4)

## Manual  Installation

This mod requires [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim)\
Extract the content of `DoorPull` into the `BepInEx/plugins` folder.

## Development

BepInEx must be setup at manual or with R2modman/Thunderstore Mod Manager.
Create a file called Environment.props inside the project root. Copy the example and change the Valheim install path to your location
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="Current" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    <PropertyGroup>
        <!-- Needs to be your path to the base Valheim folder -->
        <VALHEIM_INSTALL Condition="!Exists('$(VALHEIM_INSTALL)')">C:/Programme/Steam/steamapps/common/Valheim</VALHEIM_INSTALL>
        <VALHEIM_INSTALL Condition="!Exists('$(VALHEIM_INSTALL)')">E:/Programme/Steam/steamapps/common/Valheim</VALHEIM_INSTALL>
 
        <!-- Optional, needs to be your path to a r2modmanPlus profile folder -->
        <R2MODMAN_INSTALL>$(AppData)/r2modmanPlus-local/Valheim/profiles/Develop</R2MODMAN_INSTALL>
    </PropertyGroup>
</Project>
```
