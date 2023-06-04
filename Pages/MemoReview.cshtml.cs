using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using Nihonn.Model;

namespace Nihonn.Pages
{
    public class MemoReviewModel : PageModel
    {
        public List<WordInfo> Words;

        public IActionResult OnGet(string button_page)
        {
            Words = WordInfo.JsonToList(HttpContext.Session);

            if (button_page != null)
            {
                switch (button_page)
                {
                    case "update":
                        Words.ForEach(t => t.Update());
                        return RedirectToPage("Index");
                    case "back":
                        return RedirectToPage("Index");
                }
            }
            return Page();
        }
    }
}
