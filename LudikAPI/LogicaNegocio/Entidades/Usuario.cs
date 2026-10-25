using System;
using System.ComponentModel.DataAnnotations;
using LogicaNegocio.InterfacesEntidades;
using LogicaNegocio.Resultados;
using LogicaNegocio.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace LogicaNegocio.Entidades
{
    public  class Usuario : IdentityUser, IValidable
	{

        public string? ImagenPerfil { get; set; }

        [Required]
        public NombreCompleto NombreCompleto { get; set; }

        public Resultado esValido()
        {
            throw new NotImplementedException();
        }
    }

}

