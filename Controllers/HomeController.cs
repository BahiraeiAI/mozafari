using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoshedTehran.Data;
using RoshedTehran.DTOs;
using RoshedTehran.Models;
using RoshedTehran.Services;
using RoshedTehran.ViewModels;

namespace RoshedTehran.Controllers;

[Route("")]
[Route("[controller]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _DbContext;
    private readonly EmbederService _Embeder;
    private readonly QdrantService _QdrantService;

    public HomeController(ILogger<HomeController> logger
        ,ApplicationDbContext dbContext
        ,EmbederService embeder
        ,QdrantService qdrantService)
    {
        _logger = logger;
        _DbContext = dbContext;
        _Embeder = embeder;
        _QdrantService = qdrantService;
    }

    [Route("")]
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Index(string? SearchQuery)
    {
        if(SearchQuery is null)
        {

            IndexViewModel IndexVM = new IndexViewModel
            {
                SearchQuery = null,
                Results = null
            };
        
            return View(IndexVM);
        }
        
        var Query = new QueryEntity
        {
            Id = new Guid(),
            Query = SearchQuery,
            DateTime = DateTime.Now.ToUniversalTime()
        };

        await _DbContext.Queries.AddAsync(Query);


        float[] Vector = await _Embeder.GetEmbeddingAsyncQuery(SearchQuery);
        var results = await _QdrantService.SearchAsync(Vector);

        List<IdsScores> IDScores = results.Select(item => new IdsScores { Id = Guid.Parse(item.Id.Uuid), Similarity_Score = item.Score }).ToList();
        List<Guid> ids = results.Select(r => Guid.Parse(r.Id.Uuid)).ToList();


        IEnumerable<ResultObject> Records = await _DbContext.Entities
            .Where(n => ids.Contains(n.Id)).Select(i => new ResultObject { Id = i.Id, Domain = i.Domain, URI = i.URI, Instagram = i.Instagram , GoogleMapIdentifierURI = i.GoogleMapIdentifierURI , TitleTag = i.TitleTag, MetaDescription = i.MetaDescription,DOM =i.DOM,PhoneNumber = i.PhoneNumber, Location = i.Location, Email = i.Email,Latitude = i.Latitude, Longitude = i.Longitude, RegistrationDate = i.RegistrationDate })
            //.OrderBy(item => item.CreatedAt)
            //.Reverse()
            .ToListAsync();

        foreach (ResultObject entity in Records)
        {
            foreach (IdsScores ids1 in IDScores)
            {
                if (entity.Id == ids1.Id)
                {
                    entity.SimilarityScore = ids1.Similarity_Score;
                }
            }
        }

        IndexViewModel Indexvm = new IndexViewModel
        {
            SearchQuery = null,
            Results = Records
        };
        return View(Records);
    }
    

    [Route("[action]")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

