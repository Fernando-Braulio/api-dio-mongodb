using api_dio_mongo.Models;
using api_dio_mongo.Data.Collections;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace api_dio_mongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public async Task<ActionResult> SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            await _infectadosCollection.InsertOneAsync(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [HttpDelete]
        public ActionResult DeletarInfectados([FromQuery] string id)
        {
            MongoDB.Bson.ObjectId objId = new MongoDB.Bson.ObjectId(id);
            var deleteFilter = Builders<Infectado>.Filter.Eq("_id", objId);

            if (deleteFilter == null)
                return NotFound();
                
            _infectadosCollection.DeleteOne(deleteFilter);
            return Ok();
        }

    }
}