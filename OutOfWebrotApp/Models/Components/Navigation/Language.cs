﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutOfWebrotApp.Models.Components.Navigation
{
	public class Language
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public bool IsCurrentLanguage { get; set; }
	}
}