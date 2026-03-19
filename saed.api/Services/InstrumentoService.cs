using saed.api.Data.Repositories;
using saed.api.DTOs.Instrumentos;
using saed.api.Model;

namespace saed.api.Services
{
    // Orquesta el flujo del modulo y hace el mapeo explicito entre DTOs y entidades.
    public class InstrumentoService
    {
        private readonly InstrumentoRepository _repository;

        public InstrumentoService(InstrumentoRepository repository)
        {
            _repository = repository;
        }

        #region Operaciones publicas
        public async Task<List<InstrumentoCreateDto>> GetAllAsync()
        {
            var instrumentos = await _repository.GetAllAsync();
            return instrumentos.Select(MapToDto).ToList();
        }

        public async Task<InstrumentoCreateDto?> GetByIdAsync(int id)
        {
            var instrumento = await _repository.GetByIdFullAsync(id);
            return instrumento == null ? null : MapToDto(instrumento);
        }

        public async Task<InstrumentoCreateDto> CreateAsync(InstrumentoCreateDto dto)
        {
            var entidad = MapToEntity(dto);
            var creado = await _repository.CreateAsync(entidad);
            return MapToDto(creado);
        }

        public async Task<InstrumentoCreateDto?> ClonePlantillaAsync(int id)
        {
            var clonado = await _repository.ClonarPlantillaAsync(id);
            return clonado == null ? null : MapToDto(clonado);
        }

        public async Task<InstrumentoCreateDto?> UpdateAsync(int id, InstrumentoCreateDto dto)
        {
            var entidad = MapToEntity(dto);
            entidad.Id = id;

            var actualizado = await _repository.UpdateAsync(entidad);
            return actualizado == null ? null : MapToDto(actualizado);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
        #endregion

        #region Entidad a DTO
        // Convierte el grafo persistido a un contrato listo para la API.
        private static InstrumentoCreateDto MapToDto(Instrumento instrumento)
        {
            return new InstrumentoCreateDto
            {
                Id = instrumento.Id,
                Nombre = instrumento.Nombre,
                TipoInstrumento = instrumento.TipoInstrumento,
                EsPlantilla = instrumento.EsPlantilla,
                Activo = instrumento.Activo,
                ProcesoId = instrumento.ProcesoId,
                Categorias = instrumento.Categorias
                    .Select(MapCategoriaToDto)
                    .ToList()
            };
        }

        private static CategoriaDto MapCategoriaToDto(Categoria categoria)
        {
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Orden = categoria.Orden,
                Preguntas = categoria.Preguntas
                    .Select(MapPreguntaToDto)
                    .ToList()
            };
        }

        private static PreguntaDto MapPreguntaToDto(Pregunta pregunta)
        {
            return new PreguntaDto
            {
                Id = pregunta.Id,
                Texto = pregunta.Texto,
                Tipo = pregunta.Tipo,
                Orden = pregunta.Orden,
                Opciones = pregunta.Opciones
                    .Select(MapOpcionToDto)
                    .ToList()
            };
        }

        private static OpcionDto MapOpcionToDto(OpcionRespuesta opcion)
        {
            return new OpcionDto
            {
                Id = opcion.Id,
                Texto = opcion.Texto,
                Valor = opcion.Valor
            };
        }
        #endregion

        #region DTO a entidad
        // Reconstruye el grafo de dominio desde el payload enviado por la API.
        private static Instrumento MapToEntity(InstrumentoCreateDto dto)
        {
            return new Instrumento
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                TipoInstrumento = dto.TipoInstrumento,
                EsPlantilla = dto.EsPlantilla,
                Activo = dto.Activo,
                ProcesoId = dto.ProcesoId,
                Categorias = dto.Categorias
                    .Select(MapCategoriaToEntity)
                    .ToList()
            };
        }

        private static Categoria MapCategoriaToEntity(CategoriaDto dto)
        {
            return new Categoria
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Orden = dto.Orden,
                Preguntas = dto.Preguntas
                    .Select(MapPreguntaToEntity)
                    .ToList()
            };
        }

        private static Pregunta MapPreguntaToEntity(PreguntaDto dto)
        {
            return new Pregunta
            {
                Id = dto.Id,
                Texto = dto.Texto,
                Tipo = dto.Tipo,
                Orden = dto.Orden,
                Opciones = dto.Opciones
                    .Select(MapOpcionToEntity)
                    .ToList()
            };
        }

        private static OpcionRespuesta MapOpcionToEntity(OpcionDto dto)
        {
            return new OpcionRespuesta
            {
                Id = dto.Id,
                PreguntaId = 0,
                Valor = dto.Valor,
                Texto = dto.Texto
            };
        }
        #endregion
    }
}
