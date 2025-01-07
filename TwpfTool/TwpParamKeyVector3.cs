﻿using System;
using System.IO;
using System.Xml.Serialization;

namespace TwpfTool
{
    [XmlType]
    public class TwpParamKeyVector3 : TwpParamKey
    {
        [XmlAttribute]
        public float x, y, z;

        public new void Read(BinaryReader reader)
        {
            base.Read(reader);
            x = reader.ReadSingle();
            y = reader.ReadSingle();
            z = reader.ReadSingle();
            if (Program.IsVerbose)
                Console.WriteLine($"Time: {base.time}, Value: ({x}, {y}, {z})");
        }
        public new void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }
    }
}
