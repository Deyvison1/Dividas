using Dividas.Dominio.Identity;

namespace Dividas.WebApi.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NomeCompleto { get; set; }
        public string Role { get; set; }
        
    }
}