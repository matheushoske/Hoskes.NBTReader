# NBTReader

Hoskes.NBTReader is an open-source NBT reader library for all common sources of NBT data. It's mainly intended for reading [Minecraft](http://www.minecraft.net) game data.
With this library you can easy read NBT files deserializing it into .NET objects.

This code uses an amazing NBTModel created by jaquadro for [NBTExplorer](https://github.com/jaquadro/NBTExplorer)
## Supported Formats

NBTReader supports reading and writing the following formats:

* Standard NBT files (e.g. level.dat)
* Schematic files
* Uncompressed NBT files (e.g. idcounts.dat)
* Minecraft region files (*.mcr)
* Minecraft anvil files (*.mca)
* Cubic Chunks region files (r2*.mcr, r2*.mca)

### .NET

Hoskes.NBTReader library is using .NET Standard 2.1

### Using the Hoskes.NBTReader to deserialize an NBT File

```csharp
	LevelClass level = NBTFile.Deserialize<LevelClass>("mypath\\level.dat");
```
Example class:
```csharp
	class LevelClass
	{
		[NBTTagName("Data")]
		public DataClass Data { get; set; }
	}
	class DataClass
	{
		[NBTTagName("Version")]
		public VersionClass Version { get; set; }

		[NBTTagName("Difficulty")]
		public int Difficulty { get; set; }

		[NBTTagName("hardcore")]
		public bool Hardcore { get; set; }

		[NBTTagName("WasModded")]
		public bool? WasModded { get; set; }

		[NBTTagName("GameType")]
		public bool GameType { get; set; }
	}
	class VersionClass
	{
		[NBTTagName("Name")]
		public string Value { get; set; }
	}
```