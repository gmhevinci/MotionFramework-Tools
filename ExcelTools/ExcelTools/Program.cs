using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTools
{
	class Program
	{
		// 1代表平台ios, 2代表平台andorid
		private static int pType = 1;

		static void Main(string[] args)
		{
			pType = 1;
			if (args.Length > 0 && (args[0] == "android" || args[0] == "Android"))
				pType = 2;

			// 初始化配置;
			ExportConfig.Instance.Init();
			SettingConfig.Instance.Init();

			ExportExcel();

			Console.WriteLine("配置导出完毕！");
			//string str = Console.ReadLine();
		}

		static void ExportExcel()
		{
			string autoFile = Path.Combine(ExportConfig.Instance.ExcelPath, LanguageMgr.StrAutoGenerateLanguageExcelName + ".xlsx");
			if (File.Exists(autoFile))
			{
				File.Delete(autoFile);
			}

			string[] files = Directory.GetFiles(ExportConfig.Instance.ExcelPath, "*.*", SearchOption.AllDirectories);

			List<string> fileList = new List<string>();
			for (int i = 0; i < files.Length; i++)
			{
				if (pType == 1 && (files[i].Contains("/Android") || files[i].Contains("\\Android")))
				{
					continue;
				}

				if (pType == 2 && (files[i].Contains("/IOS") || files[i].Contains("\\IOS")))
				{
					continue;
				}

				if (files[i].EndsWith(".xlsx") || files[i].EndsWith(".xls"))
				{
					fileList.Add(files[i]);
				}
			}

			// 导出Excel文件列表
			ExportCenter.Export(fileList);
		}
	}
}
