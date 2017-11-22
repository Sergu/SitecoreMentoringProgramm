using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutOfWebrotApp.Models.Components.Navigation;

namespace OutOfWebrotApp.Services.Interfaces.Language
{
	public interface ILanguageService
	{
		Languages GetAllLanguages();
	}
}
