namespace saed.api.Model
{
    public class Instrumento
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public TipoInstrumento TipoInstrumento { get; set; } = TipoInstrumento.Curricular;

        public bool EsPlantilla { get; set; } = true;

        public bool Activo { get; set; } = true;

        public int? ProcesoId { get; set; }

        public Proceso? Proceso { get; set; }

        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
    }
}
