using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nihonn.Model;
using SharpCompress.Common;
using System.Collections;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using ThirdParty.Json.LitJson;
using MongoDB.Bson.Serialization.Serializers;

namespace Nihonn.Pages
{
    public class WordListModel : PageModel
    {
        public List<WordInfo> Words;

        public IActionResult OnGet(string button_page)
        {
            Words = WordInfo.JsonToList(HttpContext.Session);

            if (button_page != null)
            {
                switch (button_page)
                {
                    case "back":
                        return RedirectToPage("Index");
                    case "memo":
                        return RedirectToPage("Memo");
                }
            }
            return Page();
        }
    }
}
