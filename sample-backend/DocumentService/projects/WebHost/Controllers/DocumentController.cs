using Microsoft.AspNetCore.Mvc;

namespace WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(ILogger<DocumentController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public IEnumerable<Document> Get()
        {
            Document document1 = new();
            document1.DocumentName = "Vorsorgeausweis";
            document1.DocumentTemplate = "VorsrogeausweisTemplate";
            document1.Print = true;
            yield return document1;

            Document document2 = new();
            document2.DocumentName = "BestaetigungNEW";
            document2.DocumentTemplate = "BestaetigungTemplate";
            document2.Print = true;
            yield return document2;

            Document document3 = new();
            document3.DocumentName = "Mahnung";
            document3.DocumentTemplate = "MahnungTemplate";
            document3.Print = true;
            yield return document3;

            Document document4 = new();
            document4.DocumentName = "Mahnung2";
            document4.DocumentTemplate = "MahnungTemplate2";
            document4.Print = true;
            yield return document4;
        }
    }
}