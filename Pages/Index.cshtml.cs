using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Nihonn.Model;
using System;
using System.Reflection;
using System.Text;

namespace Nihonn.Pages
{
	public class IndexModel : PageModel
	{
		public int MemoCount = 0;

		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public IActionResult OnGet(string button_page)
		{
			HttpContext.Session.Clear();
			var newWords = WordInfo.GetNewWordsFromDB();
			var memoWords = WordInfo.GetTodayMemoWordsFromDB();
			MemoCount = memoWords.Count;

			if (button_page != null)
			{
				switch (button_page)
				{
					case "new":
						WordInfo.ListToJson(newWords, HttpContext.Session);
						HttpContext.Session.SetString("Word", "");
                        return RedirectToPage("WordList", new { action = "new"});
					case "re":
                        WordInfo.ListToJson(memoWords, HttpContext.Session);
                        HttpContext.Session.SetString("Word", "");
                        return RedirectToPage("WordList", new { action = "memo" });
					case "all":
						WordInfo.ListToJson(WordInfo.GetAllWordsFromDB(), HttpContext.Session);
						HttpContext.Session.SetString("Word", "");
						return RedirectToPage("WordList", new { action = "memo" });
				}
			}
			return Page();
		}
	}
}