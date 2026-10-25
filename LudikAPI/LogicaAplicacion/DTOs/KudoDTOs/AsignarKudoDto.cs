namespace LogicaAplicacion.DTOs.KudoDTOs;

public class AsignarKudoDto
{
    public int IdPerfilEstudianteRecibe { get; set; }
    public int IdPerfilEstudianteEmisor { get; set; }

    public TipoKudoDto Kudo { get; set; }
}