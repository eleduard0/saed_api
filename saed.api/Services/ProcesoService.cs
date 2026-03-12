using saed.api.Data.Repositories;
using saed.api.Model;

namespace saed.api.Services
{
    public class ProcesoService
    {
        private readonly ProcesoRepository _repository;

        public ProcesoService(ProcesoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Proceso> CrearProceso(int anio, string periodo, DateTime inicio, DateTime fin)
        {
            var activo = await _repository.GetActivo();

            if (activo != null)
                throw new Exception("Ya existe un proceso activo");

            var proceso = new Proceso
            {
                Anio = anio,
                Periodo = periodo,
                FechaInicio = inicio,
                FechaFin = fin,
                Activo = true
            };

            await _repository.Add(proceso);

            return proceso;
        }

        public async Task<List<Proceso>> ObtenerProcesos()
        {
            return await _repository.GetAll();
        }

        public async Task<Proceso?> ObtenerActivo()
        {
            return await _repository.GetActivo();
        }
        public async Task<Proceso> CerrarProceso(int id)
        {
            var proceso = await _repository.GetById(id);

            if (proceso == null)
                throw new Exception("Proceso no encontrado");

            if (!proceso.Activo)
                throw new Exception("El proceso ya está cerrado");

            proceso.Activo = false;

            await _repository.Update(proceso);

            return proceso;
        }
        public async Task<Proceso?> ObtenerPorId(int id)
        {
            var proceso = await _repository.GetById(id);
            if (proceso == null)
                throw new Exception("Proceso no encontrado");

            return proceso;
        }
    }
}