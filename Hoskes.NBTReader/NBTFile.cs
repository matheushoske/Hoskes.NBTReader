using NBTExplorer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Hoskes.NBTReader
{
    public class NBTFile
    {
        private readonly NbtFileDataNode _fileDataNode;
        public NbtFileDataNode Root { get => _fileDataNode; }
        public DataNodeCollection Nodes { get => _fileDataNode.Nodes; }
        
        public static NBTFile StartLoad(string path) 
        {
            var file = LoadFileDataNode(path);
            file.Load();
            return new NBTFile(file);
        }
        public static NBTFile LoadFullFile(string path)
        {
            var file = LoadFileDataNode(path);
            file.LoadAllChilds();
            return new NBTFile(file);
        }
        private static NbtFileDataNode LoadFileDataNode(string path) 
        {
            var file = NbtFileDataNode.TryCreateFrom(path);
            if (file is null)
                throw new FileLoadException("The selected NBT file was not loaded correctly.");
            return file;
        }
        public static T Deserialize<T>(string filePath)
        {
            var nbtFile = NBTFile.StartLoad(filePath);
            return (T)LoadType(nbtFile.Root,typeof(T));
        }
        private static object LoadType(DataNode node, Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            var instance = Activator.CreateInstance(type);
            foreach (var prop in type.GetProperties())
            {
                LoadProperty(prop, instance, node);
            }
            return instance;

        }
        private static void LoadProperty(PropertyInfo prop, object instance, DataNode node) 
        {
            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            string tagName;
            var nameAttribute = prop.GetCustomAttribute<NBTTagNameAttribute>();
            if (nameAttribute == null) tagName = prop.Name;
            else tagName = nameAttribute.Name;
            if (!typeof(IEnumerable<object>).IsAssignableFrom(propType))
                LoadNonEnumerableProperty(prop, instance, node, tagName, propType);
            else
                LoadEnumerableProperty(prop, instance, node, tagName, propType);
        }
        public static void LoadNonEnumerableProperty(PropertyInfo prop, object instance, DataNode node, string tagName, Type underLyingType) 
        {
            if (underLyingType.IsValueType || underLyingType == typeof(string) || underLyingType == typeof(DateTime))
            {
                var value = node.GetChildValue(tagName, underLyingType);
                prop.SetValue(instance, value);
            }
            else
            {
                var childTag = node.LoadChild(tagName);
                if (childTag == null) return;
                childTag.Load();
                prop.SetValue(instance, LoadType(childTag, underLyingType));
            }
        }
        public static void LoadEnumerableProperty(PropertyInfo prop, object instance, DataNode node, string tagName, Type underLyingType)
        {
            throw new NotImplementedException();
        }
        private NBTFile(NbtFileDataNode fileDataNode)
        {
            this._fileDataNode = fileDataNode;
        }

    }
}
