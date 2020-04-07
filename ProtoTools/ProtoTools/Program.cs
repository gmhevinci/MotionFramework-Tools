using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ProtoTools
{
    internal class OpcodeInfo
    {
        public string Name;
        public int Opcode;
    }

    public static class Program
    {
        private const string protoPath = ".";
        private const string clientMessagePath = "../Assets/Works/_Hotfix_/Network/AutoGenerateMessage/";
        //private const string serverMessagePath = "../Server/Message/";

        private static readonly char[] splitChars = { ' ', '\t' };
        private static readonly List<OpcodeInfo> msgOpcode = new List<OpcodeInfo>();

        public static void Main()
        {
            string protoc = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                protoc = "protoc.exe";
            }
            else
            {
                protoc = "protoc";
            }

			//if (!Directory.Exists(serverMessagePath))
			//{
			//	Directory.CreateDirectory(serverMessagePath);
			//}

			if (!Directory.Exists(clientMessagePath))
			{
				Directory.CreateDirectory(clientMessagePath);
			}

            //ProcessHelper.Run(protoc, "--java_out=\"" + serverMessagePath + "\" --proto_path=\"./\" ProtoMessageDefine.proto", waitExit: true);

			ProcessHelper.Run(protoc, "--csharp_out=\"" + clientMessagePath + "\" --proto_path=\"./\" ProtoMessageDefine.proto", waitExit: true);

            //Proto2CS("Hotfix", "ProtoMessageDefine.proto", serverMessagePath, "ProtoMessageID", 10000, false);

			Proto2CS("Hotfix", "ProtoMessageDefine.proto", clientMessagePath, "ProtoMessageID", 10000);

			Console.WriteLine("proto2cs succeed!");
        }

        

        public static void Proto2CS(string ns, string protoName, string outputPath, string opcodeClassName, int startOpcode, bool isClient = true)
        {
            msgOpcode.Clear();
            string proto = Path.Combine(protoPath, protoName);

            string s = File.ReadAllText(proto);

            StringBuilder sb = new StringBuilder();
			if (isClient)
			{
				sb.Append("using MotionFramework.Network;\n");
			}
			else
			{
				sb.Append("using ETModel;\n");
			}

            sb.Append($"namespace {ns}\n");
            sb.Append("{\n");

            bool isMsgStart = false;

            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline == "")
                {
                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    sb.Append($"{newline}\n");
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    isMsgStart = true;
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }
                    else
                    {
                        parentClass = "";
                    }

                    msgOpcode.Add(new OpcodeInfo() { Name = msgName, Opcode = ++startOpcode });

                    sb.Append($"\t[NetworkMessage({opcodeClassName}.{msgName})]\n");
                    sb.Append($"\tpublic partial class {msgName} ");
                    if (parentClass != "")
                    {
                        sb.Append($": {parentClass} ");
                    }

                    sb.Append("{}\n\n");
                }

                if (isMsgStart && newline == "}")
                {
                    isMsgStart = false;
                }
            }

            sb.Append("}\n");

            GenerateOpcode(ns, opcodeClassName, outputPath, sb);
        }

        private static void GenerateOpcode(string ns, string outputFileName, string outputPath, StringBuilder sb)
        {
            sb.AppendLine($"namespace {ns}");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic static partial class {outputFileName}");
            sb.AppendLine("\t{");
            foreach (OpcodeInfo info in msgOpcode)
            {
                sb.AppendLine($"\t\t public const ushort {info.Name} = {info.Opcode};");
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            string csPath = Path.Combine(outputPath, outputFileName + ".cs");
            File.WriteAllText(csPath, sb.ToString());
        }
    }
}
