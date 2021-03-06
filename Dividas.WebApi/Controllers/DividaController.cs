using Microsoft.AspNetCore.Mvc;
using Dividas.Repositorio;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Dividas.Dominio;
using AutoMapper;
using Dividas.WebApi.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace Dividas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DividaController : ControllerBase
    {
        private readonly IDividasRepositorio _repo;
        private readonly IMapper _mapper;
        public DividaController(IDividasRepositorio repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Aqui vem todas Informações
                var dividas = await _repo.GetAllAsync();
                // Aqui ja separo os itens do meu desejo 
                var results = _mapper.Map<DividaDto[]>(dividas);
                
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no getAll {ex.Message}");
            }
        }
        [HttpPost("upload")]
        public IActionResult upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources","Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0) {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create)) {
                        file.CopyTo(stream);
                    }
                }
                
                return Ok();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no getAll {ex.Message}");
            }
        }
        [HttpGet("dividasPaga")]
        public async Task<IActionResult> GetAllDividasPaga()
        {
            try
            {
                // Aqui vem todas Informações
                var dividas = await _repo.GetAllDividasPagaAsync();
                // Aqui ja separo os itens do meu desejo 
                var results = _mapper.Map<DividaDto[]>(dividas);
                
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no getAll {ex.Message}");
            }
        }
        [HttpGet("dividasPendentes")]
        public async Task<IActionResult> GetAllDividasPendetes()
        {
            try
            {
                // Aqui vem todas Informações
                var dividas = await _repo.GetAllDividasPendentesAsync();
                // Aqui ja separo os itens do meu desejo 
                var results = _mapper.Map<DividaDto[]>(dividas);
                
                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no getAll {ex.Message}");
            }
        }
        // Buscar Dividas Pagas por Titulo
        [HttpGet("getDividasPagasTitulo/{titulo}")]
        public async Task<IActionResult> GetDividasPagasTitulo(string titulo)
        {
            try
            {
                var dividas = await _repo.GetDividasPagaTituloAsync(titulo);
                var results = _mapper.Map<DividaDto[]>(dividas);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no getAll");
            }
        }
        // Buscar Dividas Pagas por Valor

        [HttpGet("getDividasPagasValor/{valor}")]
        public async Task<IActionResult> GetDividasPagasValor(double valor)
        {
            try
            {
                var dividas = await _repo.GetDividasPagaValorAsync(valor);
                var results = _mapper.Map<DividaDto[]>(dividas);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no getAll");
            }
        }
        // Buscar Dividas Pendetes por Titulo
        [HttpGet("getDividasPendentesTitulo/{titulo}")]
        public async Task<IActionResult> GetDividasPendentesTitulo(string titulo)
        {
            try
            {
                var dividas = await _repo.GetDividasPendentesTituloAsync(titulo);
                var results = _mapper.Map<DividaDto[]>(dividas);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no getAll");
            }
        }
        // Buscar Dividas Pendentes por Valor
        [HttpGet("getDividasPendentesValor/{valor}")]
        public async Task<IActionResult> GetDividasPendentesValor(double valor)
        {
            try
            {
                var dividas = await _repo.GetDividasPendentesValorAsync(valor);
                var results = _mapper.Map<DividaDto[]>(dividas);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no getAll");
            }
        }
        
        [HttpGet("getAllTitulo/{titulo}")]
        public async Task<IActionResult> Get(string titulo)
        {
            try
            {
                var dividas = await _repo.GetAllDividasAsyncByTitulo(titulo);
                var results = _mapper.Map<DividaDto[]>(dividas);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no getAll");
            }
        }

        [HttpGet("getAllValor/{valor}")]
        public async Task<IActionResult> Get(double valor)
        {
            try
            {
                var dividas = await _repo.GetAllDividasAsyncByValor(valor);
                var results = _mapper.Map<DividaDto[]>(dividas);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no getAll");
            }
        }
                
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _repo.GetAllDividasAsyncById(id);
                var results = _mapper.Map<DividaDto>(result);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no GetId");
            }
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Post(DividaDto model)
        {
            try
            {
                var divida = _mapper.Map<Divida>(model);
                _repo.Add(divida);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/divida/{model.Id}", _mapper.Map<DividaDto>(divida));
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro no GetId {ex.Message}");
            }
            return BadRequest();
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int id, DividaDto model)
        {
            try
            {
                var divida = await _repo.GetAllDividasAsyncById(id);
                if (divida == null) return NotFound();

                _mapper.Map(model, divida);

                _repo.Update(divida);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/divida/{model.Id}", _mapper.Map<DividaDto>(divida));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no GetId");
            }
            return BadRequest();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var divida = await _repo.GetAllDividasAsyncById(id);
                if (divida == null) return NotFound();

                _repo.Delete(divida);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro no GetId");
            }
            return BadRequest();
        }
    }
}