//**************************************************
// Copyright©2019 何冠峰
// Licensed under the MIT license
//**************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class ExportConfig
{
	public static ExportConfig Instance = new ExportConfig();

	public class ExportWrapper
	{
		/// <summary>
		/// 导出器类型
		/// </summary>
		public Type ExporterType;

		/// <summary>
		/// 导出路径
		/// </summary>
		public string ExportPath;
	}

	public string ExcelPath;

	// 导出配置
	public ExportWrapper ClientExportInfo;
	public ExportWrapper ClientExportConfigInfo;

	private ExportConfig()
	{
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public void Init()
	{
		string appPath = Directory.GetCurrentDirectory();

		ExcelPath = Path.Combine(appPath, "../Excel/");

		ClientExportInfo = new ExportWrapper();
		ClientExportInfo.ExporterType = typeof(CsExporter);
		ClientExportInfo.ExportPath = Path.Combine(appPath, "../Client/Assets/GameScript/Runtime/Config/AutoGenerateConfig");
		if (Directory.Exists(ClientExportInfo.ExportPath))
			Directory.Delete(ClientExportInfo.ExportPath, true);
		Directory.CreateDirectory(ClientExportInfo.ExportPath);

		ClientExportConfigInfo = new ExportWrapper();
		ClientExportConfigInfo.ExporterType = typeof(BytesExporter);
		ClientExportConfigInfo.ExportPath = Path.Combine(appPath, "../Client/Assets/GameRes/Config");
		if (Directory.Exists(ClientExportConfigInfo.ExportPath))
			Directory.Delete(ClientExportConfigInfo.ExportPath, true);
		Directory.CreateDirectory(ClientExportConfigInfo.ExportPath);
	}
}