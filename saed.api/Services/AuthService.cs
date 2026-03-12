using System.Text;
using System.Text.Json;
using saed.api.DTOs.Auth;
using saed.api.Services.Universidad;

namespace saed.api.Services
{
    public class AuthService
    {
        private readonly UniversidadApiService _universidad;
        public AuthService(UniversidadApiService universidad)
        {
            _universidad = universidad;
        }

        public async Task<LoginResponseDto> Login(LoginUniversidadDto dto)
        {
            var token = await _universidad.Authenticate(dto.Username, dto.Password);

            var alumno = await _universidad.GetStudentData(token, dto.Username);

            if (alumno != null)
            {
                return new LoginResponseDto
                {
                    Matricula = alumno.Matricula,
                    Nombre = $"{alumno.Nombre} {alumno.ApellidoPaterno}",
                    Rol = "Alumno",
                    Token = token
                };
            }



            throw new Exception("Usuario no pertenece a la universidad");
        }
    }
}