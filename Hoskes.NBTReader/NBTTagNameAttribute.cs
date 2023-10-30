using System;
using System.Collections.Generic;
using System.Text;

namespace Hoskes.NBTReader
{
    public class NBTTagNameAttribute : Attribute
    {
        public string Name { get; }
        public NBTTagNameAttribute(string name) 
        {
            Name = name;
        }

        
    }
}
