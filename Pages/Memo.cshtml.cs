using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Nihonn.Model;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Nihonn.Pages
{
    public class MemoModel : PageModel
    {
        public bool ShowAns { get; set; }
        public int CheckAns { get; set; }

        public WordInfo? Word;

        public IActionResult OnGet(string button_page, string katakana)
        {
            ShowAns = false;
            CheckAns = 0;

            Word = WordInfo.GetWord(HttpContext.Session);
            if (Word == null) { return RedirectToPage("MemoReview"); }

            if (button_page != null)
            {
                switch (button_page)
                {
                    case "next":
                        HttpContext.Session.SetString("Word", "");
                        Word = WordInfo.GetWord(HttpContext.Session);
                        if (Word == null) { return RedirectToPage("MemoReview"); }
                        return Page();
                    case "check":
                        if (katakana == Word.Katakana || katakana == "OK")
                        {
                            Word.CorrectlyMemo = true;
                            CheckAns = 1;
                        }
                        else
                        {
                            Word.ErrorInMemo = true;
                            CheckAns = 2;
                        }
                        HttpContext.Session.SetString(Word.SessionName, Word.ToJson());
                        return Page();
                    case "unknow":
                        Word.ErrorInMemo = true;
                        HttpContext.Session.SetString(Word.SessionName, Word.ToJson());
                        ShowAns = true;
                        return Page();
                }
            }

            return Page();
        }
    }
}
