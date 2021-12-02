using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using MotionFramework.IO;

[ExportAttribute("导出Xml文件")]
public class XmlExporter : BaseExporter
{
	public XmlExporter(SheetData sheetData)
		: base(sheetData)
	{
	}

	public override void ExportFile(string path, string createLogo)
	{
		StringBuilder sSb = new StringBuilder();
		sSb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
		sSb.Append("\r\n");
		sSb.Append("<root>");
		sSb.Append("\r\n");

		for (int i = 0; i < _sheet.Tables.Count; i++)
		{
			var table = _sheet.Tables[i];

			sSb.Append("     <data");
			for (int j = 0; j < _sheet.Heads.Count; j++)
			{
				var head = _sheet.Heads[j];
				if (!head.Name.Contains("#"))
				{
					var value = table.GetCellValue(head.CellNum);
					if (value == null)
						value = SettingConfig.Instance.AutoCompleteCellContent;

                    if (head.Logo.Contains(createLogo))
					    sSb.Append(" " + head.Name + "=\"" + value + "\"");
				}
			}
			sSb.Append(" />");
			sSb.Append("\r\n");
		}

		sSb.Append("</root>");

		// 创建文件
		string filePath = StringHelper.MakeSaveFullPath(path, $"{_sheet.FileName}.xml");
		using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
		{
			using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8")))
			{
				textWriter.Write(sSb.ToString());
			}
		}
	}
}