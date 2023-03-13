using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ModuloContabilidadApi.ApplicationContexts;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace ModuloContabilidadApi.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration       _configuration;
    private readonly IMapper              _mapper;
    private          string               _secretKey;

    public AuthRepository(ApplicationDbContext dbContext, IMapper mapper,
                          IConfiguration configuration)
    {
        _dbContext     = dbContext;
        _mapper        = mapper;
        _configuration = configuration;
        _secretKey     = _configuration.GetValue<string>("ApiSettings:Secret");
    }

    public bool IsUnique(string nombre)
    {
        var usuario =
            _dbContext.Usuarios.FirstOrDefault(x => x.Nombre == nombre);
        if (usuario is null)
        {
            return true;
        }

        return false;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var usuario = _dbContext.Usuarios.SingleOrDefault(
            u => u.Nombre == loginRequestDto.Nombre && u.Password ==
                loginRequestDto.Password
        );
        if (usuario is null)
        {
            return null;
        }

        var tokenHandler    = new JwtSecurityTokenHandler();
        var key             = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role,usuario.Tipo),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDto loginResponseDto = new()
        {
            Usuario = _mapper.Map<UsuarioDto>(usuario),
            Token   = new JwtSecurityTokenHandler().WriteToken(token)
        };
        return loginResponseDto;
    }

    public async Task<UsuarioDto> Registrar(RegistroRequestDto
                                                requestDto)
    {
        Usuario usuario = new()
        {
            Nombre   = requestDto.Nombre,
            Password = requestDto.Password
        };
        await _dbContext.Usuarios.AddAsync(usuario);
        await _dbContext.SaveChangesAsync();
        usuario.Password = string.Empty;
        return _mapper.Map<UsuarioDto>(usuario);
    }
}