using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace projectLoXo.Pages
{
    public class SuatrithucModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public KnowledgeManagementModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private string LoxoFactsFilePath => Path.Combine(_environment.ContentRootPath, "Knowledge/Facts.txt");
        private string LoxoRulesFilePath => Path.Combine(_environment.ContentRootPath, "Knowledge/Rules.txt");
        private string LoxoFormulasFilePath => Path.Combine(_environment.ContentRootPath, "Knowledge/Formula.txt");

        [BindProperty]
        public string FactsContent { get; set; }

        [BindProperty]
        public string RulesContent { get; set; }

        [BindProperty]
        public string FormulasContent { get; set; }
        public void OnGet()
        {
            LoadKnowledgeBaseContent();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            string factsFilePath, rulesFilePath, formulasFilePath;

            
            
            factsFilePath = LoxoFactsFilePath;
            rulesFilePath = LoxoRulesFilePath;
            formulasFilePath = LoxoFormulasFilePath;
            

            // Save the content to the respective files
            await System.IO.File.WriteAllTextAsync(factsFilePath, FactsContent);
            await System.IO.File.WriteAllTextAsync(rulesFilePath, RulesContent);
            await System.IO.File.WriteAllTextAsync(formulasFilePath, FormulasContent);

            TempData["Message"] = "N?i dung ?ã ???c l?u thành công!";

            // Reload the content to reflect the changes
            LoadKnowledgeBaseContent();

            return Page();
        }

        private void LoadKnowledgeBaseContent()
        {
            string factsFilePath, rulesFilePath, formulasFilePath;

            
            
            factsFilePath = LoxoFactsFilePath;
            rulesFilePath = LoxoRulesFilePath;
            formulasFilePath = LoxoFormulasFilePath;
            

            FactsContent = System.IO.File.ReadAllText(factsFilePath);
            RulesContent = System.IO.File.ReadAllText(rulesFilePath);
            FormulasContent = System.IO.File.ReadAllText(formulasFilePath);
        }
    }
}
