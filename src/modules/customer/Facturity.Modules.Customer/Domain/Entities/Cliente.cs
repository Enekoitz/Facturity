using System.Runtime.CompilerServices;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed class Cliente : AggregateRoot<Guid>
{
    public string Alias { get; private set; }
    public IIdentificacionFiscal IdentificacionFiscal { get; private set; }
    public string? RazonSocial { get; private set; }
    public string? Nombre { get; private set; }
    public string? PrimerApellido { get; private set; }
    public string? SegundoApellido { get; private set; }
    public Domicilio? Domicilio { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }
    
    private Cliente() { }

    private Cliente(
        Guid id,
        TenantId tenantId,
        string alias,
        IIdentificacionFiscal identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio) : base(id, tenantId)
    {
        Alias = alias;
        IdentificacionFiscal = identificacionFiscal;
        RazonSocial = razonSocial;
        Nombre = nombre;
        PrimerApellido = primerApellido;
        SegundoApellido = segundoApellido;
        Domicilio = domicilio;
    }

    public static Result<Cliente> Crear(
        Guid id,
        TenantId tenantId,
        string alias,
        IIdentificacionFiscal identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio) 
    {

        var errores = new List<Error>();

        if (id == Guid.Empty)
            errores.Add(ClienteErrors.IdInvalido);

        if (tenantId.Value <= 0)
            errores.Add(TenantErrors.IdInvalido);

        ValidarAlias(alias, errores);

        ValidarNombresApellidosRazonSocial(identificacionFiscal.EsPersonaJuridica, razonSocial, nombre, primerApellido, segundoApellido, errores);

        if (errores.Count > 0)
            return Result.Fail<Cliente>(errores);

        return Result.Ok(new Cliente(id,
                                    tenantId,
                                    alias,
                                    identificacionFiscal,
                                    razonSocial,
                                    nombre,
                                    primerApellido,
                                    segundoApellido,
                                    domicilio));
    }
    
    public static Result<Cliente> Reconstruir(
        Guid id,
        TenantId tenantId,
        string alias,
        IIdentificacionFiscal identificacionFiscal,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        Domicilio? domicilio,
        DateTime? deletedAtUtc) 
    {
        var clienteResult = Crear(id, tenantId, alias, identificacionFiscal, razonSocial, nombre, primerApellido,segundoApellido, domicilio);

        if (deletedAtUtc is not null && deletedAtUtc > DateTime.UtcNow)
            return Result<Cliente>.Fail(ClienteErrors.FechaInvalida);
        
        if (clienteResult.IsFailure)
            return clienteResult;
        
        var cliente = clienteResult.Value;
        
        cliente.DeletedAtUtc = deletedAtUtc;
            
        return Result.Ok(cliente);
    }

    private static void ValidarAlias(
        string alias,
        List<Error> errores)
    {
        if (Validate.EsVacio(alias))
            errores.Add(ClienteErrors.AliasInvalido);

        if (Validate.SuperaLongitudMaxima(alias, ClienteParametros.AliasMaxLength))
            errores.Add(ClienteErrors.AliasDemasiadoLargo);
    }

    private static void ValidarNombresApellidosRazonSocial(
        bool? esPersonaJuridica,
        string? razonSocial,
        string? nombre,
        string? primerApellido,
        string? segundoApellido,
        List<Error> errores)
    {
        switch (esPersonaJuridica)
        {
            case false:
                ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                break;
            case true:
                ValidarRazonSocial(razonSocial, errores);
                break;
            default:
            {
                if (!string.IsNullOrWhiteSpace(razonSocial))
                {
                    ValidarRazonSocial(razonSocial, errores);
                }
                else if(!string.IsNullOrWhiteSpace(nombre))
                {
                    ValidarNombreApellido(nombre, primerApellido, segundoApellido, errores);
                }
                else
                {
                    errores.Add(ClienteErrors.NiRazonSocialNiNombreApellido);
                }

                break;
            }
        }   
    }

    private static void ValidarNombreApellido(
        string? nombre, 
        string? primerApellido, 
        string? segundoApellido, 
        List<Error> errores)
    {
        if(Validate.EsVacio(nombre))
            errores.Add(ClienteErrors.NombreVacio);

        if (Validate.SuperaLongitudMaxima(nombre, ClienteParametros.NombreMaxLength))
            errores.Add(ClienteErrors.NombreDemasiadoLargo);

        if (Validate.EsVacio(primerApellido))
            errores.Add(ClienteErrors.PrimerApellidoVacio);

        if (Validate.SuperaLongitudMaxima(primerApellido, ClienteParametros.ApellidoMaxLength))
            errores.Add(ClienteErrors.PrimerApellidoDemasiadoLargo);

        if (Validate.SuperaLongitudMaxima(segundoApellido, ClienteParametros.ApellidoMaxLength))
            errores.Add(ClienteErrors.SegundoApellidoDemasiadoLargo);
    }

    private static void ValidarRazonSocial(
        string? razonSocial,
        List<Error> errores)
    {
        if (Validate.EsVacio(razonSocial))
            errores.Add(ClienteErrors.RazonSocialVacio);

        if (Validate.SuperaLongitudMaxima(razonSocial, ClienteParametros.RazonSocialMaxLength))
            errores.Add(ClienteErrors.RazonSocialDemasiadoLargo);
    }
}