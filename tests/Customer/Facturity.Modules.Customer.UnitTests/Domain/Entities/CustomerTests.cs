using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public abstract class CustomerTests
{
    public class CrearClienteTests
    {
        [Theory]
        [ClassData(typeof(CrearClienteTestsObjetos))]
        public void CrearCliente_DatosValidos_ClienteCreadoCorrectamente(CrearClienteTestsHelper.DatosCrearClienteRecord datosCrearClienteRecord)
        {
            string alias = "MARCOS";
            var id = Guid.NewGuid();
            var tenantId = TenantId.Crear(Guid.NewGuid()).Value;
            
            var result = CrearClienteTestsHelper.CrearCliente(datosCrearClienteRecord, id, tenantId, alias);

            Assert.True(result.IsSuccess);
            
            Assert.Equal(id, result.Value.Id);
            Assert.Equal(tenantId, result.Value.TenantId);
            Assert.Equal(alias, result.Value.Alias);
            Assert.Equal(datosCrearClienteRecord.TaxId, result.Value.TaxId);
            Assert.Equal(datosCrearClienteRecord.Domicilio, result.Value.Domicilio);
            Assert.Equal(datosCrearClienteRecord.RazonSocial, result.Value.RazonSocial);
            Assert.Equal(datosCrearClienteRecord.Nombre, result.Value.Nombre);
            Assert.Equal(datosCrearClienteRecord.PrimerApellido, result.Value.PrimerApellido);
            Assert.Equal(datosCrearClienteRecord.SegundoApellido, result.Value.SegundoApellido);
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        [InlineData("Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. Razón social que excede el máximo de longitud. ")]
        public void CrearCliente_NifEspañol_EsPersonaJuridica_SoloValidaRazonSocial(string? razonSocialNuloVacio)
        {
            var datosCliente = CrearClienteTestsObjetos.ClienteNifPersonaJuridica1 with
            {
                RazonSocial = razonSocialNuloVacio
            };

            var result =  CrearClienteTestsHelper.CrearCliente(datosCliente);
            
            Assert.True(result.IsFailure);

            result.Errors.Should().Contain(string.IsNullOrWhiteSpace(razonSocialNuloVacio)
                ? CustomerErrors.LegalNameEmpty
                : CustomerErrors.LegalNameTooLong);

            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.SecondSurnameTooLong);
        }
        
        [Theory]
        [InlineData("", "")]
        [InlineData("     ", "       ")]
        [InlineData(null, null)]
        [InlineData("Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. Nombre que excede el límite de caracteres. ",
            "Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. Primer apellido que excede el límite de caracteres. ")]
        public void CrearCliente_NifEspañol_NoEsPersonaJuridica_SoloValidaNombreYApellido(string? nombre, string? primerApellido)
        {
            var datosCliente = CrearClienteTestsObjetos.ClienteNifNoPersonaJuridica1 with { Nombre = nombre, PrimerApellido = primerApellido };

            var result = CrearClienteTestsHelper.CrearCliente(datosCliente);
            
            Assert.True(result.IsFailure);

            result.Errors.Should().Contain(string.IsNullOrWhiteSpace(nombre)
                ? CustomerErrors.NameEmpty
                : CustomerErrors.NameTooLong);
            
            result.Errors.Should().Contain(string.IsNullOrWhiteSpace(nombre)
                ? CustomerErrors.FirstSurnameEmpty
                : CustomerErrors.FirstSurnameTooLong);

            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void CrearCliente_NingunoInformado_GeneraErrorNiRazonSocialNiNombre(string? vacio)
        {
            var datosCliente =  CrearClienteTestsObjetos.ClienteIdOtroCertificadoResidencia1 with
            { 
                RazonSocial = vacio, Nombre = vacio, PrimerApellido = vacio 
            };
            var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain(CustomerErrors.InvalidTaxDesignationType);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
        }

        [Fact]
        public void CrearCliente_SoloRazonSocialInformada_ValidaSoloRazonSocial()
        {
            var datosCliente = CrearClienteTestsObjetos.ClienteNifPersonaJuridica1 with 
            { 
                RazonSocial = Utilities.GetRandomLength(Parameters.Customer.LegalNameMaxLength + 1), 
                Nombre = null, 
                PrimerApellido = null 
            };
            var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

            result.Errors.Should().Contain(CustomerErrors.LegalNameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
        }

        [Fact]
        public void CrearCliente_SoloNombreInformado_ValidaSoloNombreYApellido()
        {
            var datosCliente = CrearClienteTestsObjetos.ClienteNifNoPersonaJuridica1 with 
            { 
                RazonSocial = null,
                Nombre = Utilities.GetRandomLength(Parameters.Customer.NameMaxLength + 1),
                PrimerApellido = Utilities.GetRandomLength(Parameters.Customer.SurnameMaxLength + 1)
            };
            var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

            result.Errors.Should().Contain(CustomerErrors.NameTooLong);
            result.Errors.Should().Contain(CustomerErrors.FirstSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
        }
        
        [Fact]
        public void CrearCliente_ConSegundoApellidoInformadoSuperaLongitudMaxima_Fallo()
        {
            var datosCliente = CrearClienteTestsObjetos.ClienteNifNoPersonaJuridica1 with 
            { 
                RazonSocial = null,
                Nombre = "Marcos",
                PrimerApellido = "García",
                SegundoApellido = Utilities.GetRandomLength(Parameters.Customer.SurnameMaxLength + 1)
            };
            var result = CrearClienteTestsHelper.CrearCliente(datosCliente);

            result.Errors.Should().Contain(CustomerErrors.SecondSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameTooLong);
            result.Errors.Should().NotContain(CustomerErrors.NameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.FirstSurnameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameEmpty);
            result.Errors.Should().NotContain(CustomerErrors.LegalNameTooLong);
        }
    }

    // public class ActualizarClienteTests
    // {
    //     [Theory]
    //     [ClassData(typeof(ActualizarClienteTestsObjetos))]
    //     public void ActualizarCliente_DatosValidos_ClienteActualizadoCorrectamente(
    //         ActualizarClienteTestsHelper.DatosRecord clienteOriginalDto,
    //         ActualizarClienteTestsHelper.DatosRecord clienteActualizarDto
    //     )
    //     {
    //         // Arrange
    //         var id = Guid.NewGuid();
    //         var tenantId = TenantId.Crear(999).Value;
    //
    //         var cliente = Cliente.Crear(
    //             id,
    //             tenantId,
    //             clienteOriginalDto.Alias,
    //             clienteOriginalDto.IdentificacionFiscal,
    //             clienteOriginalDto.RazonSocial,
    //             clienteOriginalDto.Nombre,
    //             clienteOriginalDto.PrimerApellido,
    //             clienteOriginalDto.SegundoApellido,
    //             clienteOriginalDto.Domicilio
    //         ).Value;
    //
    //         var nuevoTipoDenominacionFiscal = Cliente.ObtenerTipoDenominacionFiscal(
    //             clienteActualizarDto.IdentificacionFiscal.EsPersonaJuridica,
    //             clienteActualizarDto.RazonSocial,
    //             clienteActualizarDto.Nombre
    //         );
    //
    //         // Act
    //         var result = cliente.Actualizar(
    //             clienteActualizarDto.IdentificacionFiscal,
    //             clienteActualizarDto.RazonSocial,
    //             clienteActualizarDto.Nombre,
    //             clienteActualizarDto.PrimerApellido,
    //             clienteActualizarDto.SegundoApellido,
    //             clienteActualizarDto.Domicilio
    //         );
    //
    //         // Assert
    //         Assert.True(result.IsSuccess);
    //         Assert.Equal(clienteOriginalDto.Alias, cliente.Alias);
    //         Assert.Equal(id, cliente.Id);
    //         Assert.Equal(tenantId, cliente.TenantId);
    //         Assert.Equal(clienteActualizarDto.IdentificacionFiscal, cliente.IdentificacionFiscal);
    //         Assert.Equal(clienteActualizarDto.Domicilio, cliente.Domicilio);
    //
    //         switch (nuevoTipoDenominacionFiscal)
    //         {
    //             case TipoDenominacionFiscal.RazonSocial:
    //                 Assert.Equal(clienteActualizarDto.RazonSocial, cliente.RazonSocial);
    //                 Assert.Null(cliente.Nombre);
    //                 Assert.Null(cliente.PrimerApellido);
    //                 Assert.Null(cliente.SegundoApellido);
    //                 break;
    //             case TipoDenominacionFiscal.NombreApellidos:
    //                 Assert.Null(cliente.RazonSocial);
    //                 Assert.Equal(clienteActualizarDto.Nombre, cliente.Nombre);
    //                 Assert.Equal(clienteActualizarDto.PrimerApellido, cliente.PrimerApellido);
    //                 Assert.Equal(clienteActualizarDto.SegundoApellido, cliente.SegundoApellido);
    //                 break;
    //         }
    //     }
    //
    //
    //     // TODO: Pendiente de añadir test unitario de fallo esperado de actualización.
    //     
    //     //     [Theory]
    //     //     [ClassData(typeof(ActualizarClienteTestsObjetos))]
    //     //     public void ActualizarCliente_DatosValidos_Fallo(
    //     //         ActualizarClienteTestsHelper.DatosRecord clienteOriginalDto, 
    //     //         ActualizarClienteTestsHelper.DatosRecord clienteActualizarDto
    //     //         )
    //     //     {
    //     //         // Arrange
    //     //         var id = Guid.NewGuid();
    //     //         var tenantId = TenantId.Crear(999).Value;
    //     //
    //     //         var cliente = Cliente.Crear(
    //     //             id,
    //     //             tenantId,
    //     //             clienteOriginalDto.Alias,
    //     //             clienteOriginalDto.IdentificacionFiscal,
    //     //             clienteOriginalDto.RazonSocial,
    //     //             clienteOriginalDto.Nombre,
    //     //             clienteOriginalDto.PrimerApellido,
    //     //             clienteOriginalDto.SegundoApellido,
    //     //             clienteOriginalDto.Domicilio
    //     //         ).Value;
    //     //         
    //     //         var nuevoTipoDenominacionFiscal = Cliente.ObtenerTipoDenominacionFiscal(
    //     //             clienteActualizarDto.IdentificacionFiscal.EsPersonaJuridica,
    //     //             clienteActualizarDto.RazonSocial,
    //     //             clienteActualizarDto.Nombre
    //     //         );
    //     //         
    //     //         // Act
    //     //         var result = cliente.Actualizar(
    //     //             clienteActualizarDto.IdentificacionFiscal,
    //     //             clienteActualizarDto.RazonSocial,
    //     //             clienteActualizarDto.Nombre,
    //     //             clienteActualizarDto.PrimerApellido,
    //     //             clienteActualizarDto.SegundoApellido,
    //     //             clienteActualizarDto.Domicilio
    //     //         );
    //     //
    //     //         // Assert
    //     //         Assert.True(result.IsSuccess);
    //     //         Assert.Equal(clienteOriginalDto.Alias, cliente.Alias);
    //     //         Assert.Equal(id, cliente.Id);
    //     //         Assert.Equal(tenantId, cliente.TenantId);
    //     //         Assert.Equal(clienteActualizarDto.IdentificacionFiscal, cliente.IdentificacionFiscal);
    //     //         Assert.Equal(clienteActualizarDto.Domicilio, cliente.Domicilio);
    //     //
    //     //         switch (nuevoTipoDenominacionFiscal)
    //     //         {
    //     //             case TipoDenominacionFiscal.RazonSocial:
    //     //                 Assert.Equal(clienteActualizarDto.RazonSocial, cliente.RazonSocial);
    //     //                 Assert.Null(cliente.Nombre);
    //     //                 Assert.Null(cliente.PrimerApellido);
    //     //                 Assert.Null(cliente.SegundoApellido);
    //     //                 break;
    //     //             case TipoDenominacionFiscal.NombreApellidos:
    //     //                 Assert.Null(cliente.RazonSocial);
    //     //                 Assert.Equal(clienteActualizarDto.Nombre, cliente.Nombre);
    //     //                 Assert.Equal(clienteActualizarDto.PrimerApellido, cliente.PrimerApellido);
    //     //                 Assert.Equal(clienteActualizarDto.SegundoApellido, cliente.SegundoApellido);
    //     //                 break;
    //     //         }
    //     //     }
    //     // }
    // }

    public class ReconstruirClienteTests
{
    [Theory]
    [ClassData(typeof(ReconstruirClienteTestsObjetos))]
    public void ReconstruirCliente_DatosValidos_ClienteReconstruidoCorrectamente(
        ReconstruirClienteTestsHelper.DatosRecord datosCliente)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(Guid.NewGuid()).Value;
        const string alias = "MARCOS";
        
        // Posterior a todas las fechas del conjunto
        var fakeCurrentTime = new DateTimeOffset(2026, 11, 30, 08, 00 ,00, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Customer.Domain.Customer.Reconstruir(
            id, tenantId, alias, datosCliente.TaxId, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, 
            datosCliente.Domicilio, datosCliente.ActivatedAtUtc, datosCliente.DeactivatedAtUtc, 
            datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsSuccess);
    }
    
    [Theory]
    [ClassData(typeof(ReconstruirClienteTestsObjetos))]
    public void ReconstruirCliente_FechasDistintasAtUtcInvalido_Fallo(
        ReconstruirClienteTestsHelper.DatosRecord datosCliente)
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(Guid.NewGuid()).Value;
        const string alias = "MARCOS";
        
        // Anterior a todas las fechas del conjunto
        var fakeCurrentTime = new DateTimeOffset(2026, 06, 12, 08, 43, 11, TimeSpan.Zero); 
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Customer.Domain.Customer.Reconstruir(id, tenantId, alias, datosCliente.TaxId, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, 
            datosCliente.Domicilio, datosCliente.ActivatedAtUtc, datosCliente.DeactivatedAtUtc, 
            datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsFailure);
        result.Errors.Should().Contain(CustomerErrors.InvalidFutureDate);
    }
    
    [Fact]
    public void ReconstruirCliente_FechasActivacionDesactivacionSimultaneas_Fallo()
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.Crear(Guid.NewGuid()).Value;
        const string alias = "MARCOS";
        
        var activatedAtUtc = new DateTimeOffset(2026, 05, 11, 05, 12, 23, TimeSpan.Zero);
        var deactivatedAtUtc = new DateTimeOffset(2026, 07, 12, 23, 43, 11, TimeSpan.Zero);

        var datosCliente = ReconstruirClienteTestsHelper.DatosClienteObjeto with
        {
            ActivatedAtUtc = activatedAtUtc,
            DeactivatedAtUtc = deactivatedAtUtc
        };
        
        // Anterior a todas las fechas del conjunto
        var fakeCurrentTime = new DateTimeOffset(2026, 06, 12, 08, 43, 11, TimeSpan.Zero); 
        var fakeTimeProvider = new FakeTimeProvider(fakeCurrentTime);
        
        var result = Customer.Domain.Customer.Reconstruir(id, tenantId, alias, datosCliente.TaxId, datosCliente.RazonSocial, 
            datosCliente.Nombre, datosCliente.PrimerApellido, datosCliente.SegundoApellido, 
            datosCliente.Domicilio, datosCliente.ActivatedAtUtc, datosCliente.DeactivatedAtUtc, 
            datosCliente.DeletedAtUtc, fakeTimeProvider);
        
        Assert.True(result.IsFailure);
        result.Errors.Should().Contain(CustomerErrors.SimultaneousActivationDeactivationDate);
    }
}
    
}