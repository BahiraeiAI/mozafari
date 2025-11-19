using System;
using Microsoft.AspNetCore.Mvc;
using RoshedTehran.Data;
using RoshedTehran.DTOs;
using RoshedTehran.Models;
using RoshedTehran.Services;

namespace RoshedTehran.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EntityController:ControllerBase
	{
        private readonly ApplicationDbContext _DbContext;
        private readonly EmbederService _Embeder;
        private readonly QdrantService _QdrantService;
		public EntityController(ApplicationDbContext dbContext,EmbederService embeder,QdrantService qdrantService)
		{
            _DbContext = dbContext;
            _Embeder = embeder;
            _QdrantService = qdrantService;
		}

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddEntity([FromBody] EntityDto dto)
        {
            Entity entity = new Entity()
            {
                Id = new Guid(),
                Domain = dto.Domain,
                URI = dto.URI,
                Instagram = dto.Instagram,
                GoogleMapIdentifierURI = dto.GoogleMapIdentifierURI,
                TitleTag = dto.TitleTag,
                MetaDescription = dto.MetaDescription,
                DOM = dto.DOM,
                PhoneNumber = dto.PhoneNumber,
                Location = dto.Location,
                Email = dto.Email,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
            };


            await _DbContext.Entities.AddAsync(entity);
            _DbContext.SaveChanges();


            float[] Embedding = await _Embeder.GetEmbeddingAsyncEntity(dto.URI, dto.TitleTag, dto.MetaDescription); ;
            await _QdrantService.SaveEntityAsync(entity.Id,entity.TitleTag ?? "Unkown",Embedding);

            return Ok();
        }
    }
}

