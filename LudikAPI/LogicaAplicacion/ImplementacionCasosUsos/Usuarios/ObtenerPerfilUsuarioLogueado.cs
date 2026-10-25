using InterfacesRepositorio;
using LogicaAplicacion.DTOs.PreguntasDeSeguridadDTOs;
using LogicaAplicacion.DTOs.UsuarioDTOs;
using LogicaAplicacion.InterfacesCasosUsos.Usuario;
using LogicaNegocio.Entidades;
using LogicaNegocio.Resultados;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

public class ObtenerPerfilUsuarioLogueado : IObtenerPerfilUsuarioLogueado
{
    private readonly IRepositorioUsuarios _repoUsuarios;
    private readonly UserManager<Usuario> _userManager;
    private readonly IRepositorioEstudiantes _repoEstudiantes;
    private readonly IRepositorioProfesores _repoProfesores;

    public ObtenerPerfilUsuarioLogueado(
        IRepositorioUsuarios repoUsuarios,
        UserManager<Usuario> userManager,
        IRepositorioEstudiantes repoEstudiantes,
        IRepositorioProfesores repoProfesores)
    {
        _repoUsuarios = repoUsuarios;
        _userManager = userManager;
        _repoEstudiantes = repoEstudiantes;
        _repoProfesores = repoProfesores;
    }

    public async Task<Resultado<object>> EjecutarAsync(string usuarioId)
    {
        var rUsuario = await _repoUsuarios.GetByStringIdAsync(usuarioId);
        if (rUsuario.EsFallo)
            return Resultado<object>.Falla(rUsuario.Errores);
        var usuario = rUsuario.Valor;

        var roles = await _userManager.GetRolesAsync(usuario);

        var baseDto = new UsuarioBaseDto
        {
            Id = usuario.Id,
            UserName = usuario.UserName,
            Roles = roles,
            Nombre = usuario.NombreCompleto?.Nombre,
            Apellido = usuario.NombreCompleto?.Apellido
        };

        if (roles.Contains("Estudiante"))
        {
            var rEst = await _repoEstudiantes.GetByStringIdAsync(usuario.Id);
            if (rEst.EsFallo)
                return Resultado<object>.Falla(rEst.Errores);

            var respuestas = rEst.Valor.PreguntasSeguridad
                .Select(pr => new RespuestaDto
                {
                    PreguntaRespuestaSeguridadId = pr.Id,
                    PreguntaDeSeguridadId = pr.PreguntaDeSeguridadId,
                    Respuesta = pr.Respuesta
                })
                .ToList();

            var dto = new EstudiantePerfilDto
            {
                Id = baseDto.Id,
                UserName = baseDto.UserName,
                RespuestasSeguridad = respuestas,
                Nombre = baseDto.Nombre,
                Apellido = baseDto.Apellido
            };
            return Resultado<object>.Exitoso(dto);
        }

        if (roles.Contains("Profesor"))
        {
            var rProf = await _repoProfesores.GetByStringIdAsync(usuario.Id);
            if (rProf.EsFallo)
                return Resultado<object>.Falla(rProf.Errores);

            var dto = new ProfesorPerfilDto
            {
                Id = baseDto.Id,
                UserName = baseDto.UserName,
                Correo = rProf.Valor.Email,
                Nombre = baseDto.Nombre,
                Apellido = baseDto.Apellido
            };
            return Resultado<object>.Exitoso(dto);
        }

        return Resultado<object>.Falla(
            new Error("Usuario.RolInvalido",
                      "El usuario no tiene rol de Estudiante ni Profesor"));
    }
}
