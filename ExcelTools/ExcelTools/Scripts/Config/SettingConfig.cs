//**************************************************
// Copyright©2019 何冠峰
// Licensed under the MIT license
//**************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class SettingConfig
{
	public static readonly SettingConfig Instance = new SettingConfig();

	/// <summary>
	/// 是否开启数值单元格自动补全
	/// </summary>
	public bool EnableAutoCompleteCell = true;

	/// <summary>
	/// 数值单元格自动补全的内容
	/// </summary>
	public string AutoCompleteCellContent = "0";


	private SettingConfig()
	{
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public void Init()
	{
	}
}