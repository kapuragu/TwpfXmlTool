using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace TwpfTool
{
    internal class Program
    {
        private const string twpfExt = ".twpf";
        private const string xmlExt = ".xml";
        private const string stringIdDictionaryName = "twpf_stringId_dictionary.txt";
        public static bool IsVerbose;
        private static void Main(string[] args)
        {
            Dictionary<ulong,string> dict = new Dictionary<ulong,string>();
            foreach (string key in File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + '\\' + stringIdDictionaryName)) 
            {
                dict[TwpParamKeyStringId.StrCode(key)] = key;
            }
            foreach (var arg in args)
            {
                if (arg.ToLower() == "-verbose")
                {
                    IsVerbose = true;
                    break;
                }
            }

            foreach (string arg in args) 
            {
                if (File.Exists(arg)) 
                {
                    if (Path.GetExtension(arg)==twpfExt)
                    {
                        TwpFile twpf = new TwpFile();
                        using (BinaryReader reader = new BinaryReader(new FileStream(arg, FileMode.Open)))
                        {
                            if(IsVerbose)
                                Console.WriteLine($"Reading {Path.GetFileName(arg)}...");
                            twpf.Read(reader, dict);
                        }

                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TwpFile));
                        using (FileStream xmlStream = new FileStream(Path.GetFileNameWithoutExtension(arg) + twpfExt + xmlExt, FileMode.Create))
                        {
                            xmlSerializer.Serialize(xmlStream, twpf);
                        }
                    }
                    else if (Path.GetExtension(arg)==xmlExt)
                    {
                        TwpFile file = new TwpFile();

                        using (FileStream xmlStream = new FileStream(arg, FileMode.Open))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TwpFile));
                            file = (TwpFile)xmlSerializer.Deserialize(xmlStream);
                        }

                        using (BinaryWriter writer = new BinaryWriter(new FileStream(Path.GetFileNameWithoutExtension(arg), FileMode.Create)))
                        {
                            if (IsVerbose)
                                Console.WriteLine($"Writing {Path.GetFileName(arg)}...");
                            file.Write(writer);
                        }
                    }
                }
            }

            if (IsVerbose)
                Console.Read();
        }
    }
}
