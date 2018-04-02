
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VentaServicios.ModeloDato;
using VentaServicios.ObjectRequest;
using VentaServicios.Utils;
using VentaServicios.Utils.GeoUtils;

namespace VentaServicios.Controllers.API
{


    [RoutePrefix("api/Clientes")]
    public class ClientesController : ApiController
    {
        private readonly ModelVentas db = new ModelVentas();

        
        [HttpPost]
        [Route("ObtenerTipoClientePorEmpresa")]
        public async Task<List<TipoClienteRequest>> ObtenerTipoClientePorEmpresa(EmpresaActual empresaActual)
        {
            try
            {
                var lista = await db.TipoCliente.Where(x=>x.IdEmpresa==empresaActual.IdEmpresa).Select(x => new TipoClienteRequest
                {
                    idTipoCliente=x.idTipoCliente,
                    Tipo=x.Tipo,

                }).ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                return new List<TipoClienteRequest>();
            }
        }



        // GET: api/Clientes
        [HttpPost]
        [Route("ListarClientes")]
        public async Task<List<ClienteRequest>> ListarClientes(EmpresaActual empresaActual)
        {
            try
            {
                if (empresaActual.IdEstado == EstadoCliente.Todos)
                {
                    var listaTotalClientes = await db.Cliente.Where(x => x.TipoCliente.Empresa.IdEmpresa == empresaActual.IdEmpresa).Select(x => new ClienteRequest
                    {
                        Apellido = x.Apellido,
                        ApellidosVendedor = x.Vendedor.AspNetUsers.Apellidos,
                        Email = x.Email,
                        Firma = x.Firma,
                        Foto = x.Foto,
                        IdCliente = x.idCliente,
                        IdTipoCliente = x.idTipoCliente,
                        IdVendedor = x.IdVendedor,
                        Latitud = x.Latitud,
                        Longitud = x.Longitud,
                        Nombre = x.Nombre,
                        NombresVendedor = x.Vendedor.AspNetUsers.Nombres,
                        Telefono = x.Telefono,
                        TipoCliente = x.TipoCliente.Tipo,
                        Identificacion = x.Identificacion,
                        Direccion = x.Direccion,
                        IdEmpresa = x.TipoCliente.IdEmpresa,
                        TelefonoMovil = x.TelefonoMovil,
                        RazonSocial = x.RazonSocial,
                        Estado = x.Estado,

                    }).ToListAsync();
                    return listaTotalClientes;
                }

                var lista = await db.Cliente.Where(x => x.TipoCliente.Empresa.IdEmpresa == empresaActual.IdEmpresa && x.Estado == empresaActual.IdEstado).Select(x => new ClienteRequest
                {
                    Apellido = x.Apellido,
                    ApellidosVendedor = x.Vendedor.AspNetUsers.Apellidos,
                    Email = x.Email,
                    Firma = x.Firma,
                    Foto = x.Foto,
                    IdCliente = x.idCliente,
                    IdTipoCliente = x.idTipoCliente,
                    IdVendedor = x.IdVendedor,
                    Latitud = x.Latitud,
                    Longitud = x.Longitud,
                    Nombre = x.Nombre,
                    NombresVendedor = x.Vendedor.AspNetUsers.Nombres,
                    Telefono = x.Telefono,
                    TipoCliente = x.TipoCliente.Tipo,
                    Identificacion = x.Identificacion,
                    Direccion = x.Direccion,
                    IdEmpresa = x.TipoCliente.IdEmpresa,
                    TelefonoMovil = x.TelefonoMovil,
                    RazonSocial = x.RazonSocial,
                    Estado = x.Estado,

                }).ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                return new List<ClienteRequest>();
            }
        }

        [ResponseType(typeof(Cliente))]
        [HttpPost]
        [Route("GetNearClients")]
        public async Task<List<Cliente>> GetClientForPosition(Position posicion)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var clientes = await db.Cliente.ToListAsync();
            List<Cliente> Clientes = new List<Cliente>();

            foreach (var cliente in clientes)
            {
                var cposition = new Position
                {
                    latitude = cliente.Latitud,
                    longitude = cliente.Longitud
                };
                if (GeoUtils.EstaCercaDeMi(posicion, cposition, 0.1))
                {
                    Clientes.Add(cliente);
                }
            }
            return Clientes;
        }
        [Route("ListarClientesPorVendedor")]
        public async Task<List<ClienteRequest>> ListarClientesPorVendedor(VendedorRequest vendedor)
        {
            try
            {
                var lista = await db.Cliente.Where(x => x.IdVendedor == vendedor.IdVendedor).Select(x => new ClienteRequest
                {
                    Apellido = x.Apellido,
                    ApellidosVendedor = x.Vendedor.AspNetUsers.Apellidos,
                    Email = x.Email,
                    Firma = x.Firma,
                    Foto = x.Foto,
                    IdCliente = x.idCliente,
                    IdTipoCliente = x.idTipoCliente,
                    IdVendedor = x.IdVendedor,
                    Latitud = x.Latitud,
                    Longitud = x.Longitud,
                    Nombre = x.Nombre,
                    NombresVendedor = x.Vendedor.AspNetUsers.Nombres,
                    Telefono = x.Telefono,
                    TipoCliente = x.TipoCliente.Tipo,
                    Identificacion = x.Identificacion,
                    Direccion = x.Direccion,
                    IdEmpresa = x.TipoCliente.IdEmpresa,
                    TelefonoMovil = x.TelefonoMovil,
                    RazonSocial=x.RazonSocial,
                }).ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                return new List<ClienteRequest>();
            }
        }
        [HttpPost]
        [Route("ExisteClientePorEmpresa")]
        public async Task<Response> ExisteClientePorEmpresa(ClienteRequest clienteRequest)
        {
            try
            {
                var cliente = await db.Cliente.
                                             Where(x => x.Vendedor.AspNetUsers.IdEmpresa==clienteRequest.IdEmpresa 
                                                     && x.Identificacion==clienteRequest.Identificacion)
                                             .FirstOrDefaultAsync();

                if (cliente==null)
                {
                    return new Response { IsSuccess = false };
                }
                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }
        [HttpPost]
        [Route("ExisteClienteEditarPorEmpresa")]
        public async Task<Response> ExisteClienteEditarPorEmpresa(ClienteRequest clienteRequest)
        {
            try
            {
                var cliente = await db.Cliente.
                                             Where(x => x.Vendedor.AspNetUsers.IdEmpresa == clienteRequest.IdEmpresa
                                                     && x.Identificacion == clienteRequest.Identificacion)
                                             .FirstOrDefaultAsync();


                if (cliente == null)
                {
                    return new Response { IsSuccess = false };
                };

                if (cliente.idCliente==cliente.idCliente)
                {
                    return new Response { IsSuccess = false };
                }

                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }
        // POST: api/Clientes
        [HttpPost]
        [Route("InsertarCliente")]
        public async Task<Response> InsertarCliente(ClienteRequest clienteRequest)
        {
            var cliente = new Cliente
            {
                Apellido=clienteRequest.Apellido,
                Email=clienteRequest.Email,
                Foto=clienteRequest.Foto,
                RazonSocial=clienteRequest.RazonSocial,
                Identificacion=clienteRequest.Identificacion,
                idTipoCliente=clienteRequest.IdTipoCliente,
                IdVendedor=clienteRequest.IdVendedor,
                Latitud=clienteRequest.Latitud,
                Longitud=clienteRequest.Longitud,
                Nombre=clienteRequest.Nombre,
                Telefono=clienteRequest.Telefono,
                TelefonoMovil=clienteRequest.TelefonoMovil,
                Direccion=clienteRequest.Direccion,
                Firma=clienteRequest.Firma,
                Estado=0,
            };
            try
            {
                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();
                return new Response {IsSuccess=true, };
            }
            catch (Exception ex)
            {
                return new Response {IsSuccess=false};
            }            
        }      
        [HttpPost]
        [Route("EditarCliente")]
        public async Task<Response> EditarCliente(ClienteRequest clienteRequest)
        {
            var clienteEditar =await db.Cliente.Where(x => x.idCliente == clienteRequest.IdCliente).FirstOrDefaultAsync();
            clienteEditar.Apellido = clienteRequest.Apellido;
            clienteEditar.Email = clienteRequest.Email;
            clienteEditar.Foto = clienteRequest.Foto;
            clienteEditar.Identificacion = clienteRequest.Identificacion;
            clienteEditar.idTipoCliente = clienteRequest.IdTipoCliente;
            clienteEditar.IdVendedor = clienteRequest.IdVendedor;
            clienteEditar.Latitud = clienteRequest.Latitud;
            clienteEditar.Longitud = clienteRequest.Longitud;
            clienteEditar.Nombre = clienteRequest.Nombre;
            clienteEditar.Telefono = clienteRequest.Telefono;
            clienteEditar.TelefonoMovil = clienteRequest.TelefonoMovil;
            clienteEditar.Direccion = clienteRequest.Direccion;
            clienteEditar.RazonSocial = clienteRequest.RazonSocial;                      
            try
            {
                db.Entry(clienteEditar).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true, };
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        [HttpPost]
        [Route("ObtenerCliente")]
        public async Task<Response> ObtenerCliente(ClienteRequest clienteRequest)
        {
            try
            {
               var cliente=await db.Cliente.Where(x=>x.idCliente==clienteRequest.IdCliente).
                                       Select(x=> new ClienteRequest
                                       {
                                           Apellido = x.Apellido,
                                           ApellidosVendedor = x.Vendedor.AspNetUsers.Apellidos,
                                           Email = x.Email,
                                           Firma = x.Firma,
                                           Foto = x.Foto,
                                           IdCliente = x.idCliente,
                                           IdTipoCliente = x.idTipoCliente,
                                           IdVendedor = x.IdVendedor,
                                           Latitud = x.Latitud,
                                           Longitud = x.Longitud,
                                           Nombre = x.Nombre,
                                           NombresVendedor = x.Vendedor.AspNetUsers.Nombres,
                                           Telefono = x.Telefono,
                                           TipoCliente = x.TipoCliente.Tipo,
                                           Identificacion = x.Identificacion,
                                           Direccion = x.Direccion,
                                           IdEmpresa=x.TipoCliente.IdEmpresa,
                                           TelefonoMovil=x.TelefonoMovil,
                                           RazonSocial=x.RazonSocial,
                                       } ).FirstOrDefaultAsync();
                return new Response { IsSuccess = true, Resultado=cliente};
            }
            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }
        }

        [HttpPost]
        [Route("DesactivarCliente")]
        public async Task<Response> DesactivarCliente(ClienteRequest clienteRequest)
        {
            try
            {
                var cliente = await db.Cliente.Where(x => x.idCliente == clienteRequest.IdCliente).FirstOrDefaultAsync();
                cliente.Estado = 0;
                db.Entry(cliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true};
            }

            catch (Exception)
            {
                return new Response { IsSuccess = false };
            }


        }

        [HttpPost]
        [Route("ActivarCliente")]
        public async Task<Response> ActivarCliente(ClienteRequest clienteRequest)
        {
            try
            {
                var cliente = await db.Cliente.Where(x => x.idCliente == clienteRequest.IdCliente).FirstOrDefaultAsync();
                cliente.Estado = 1;
                db.Entry(cliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return new Response { IsSuccess = true};
            }

            catch (Exception)
            {
                return new Response { IsSuccess = false };

            }


        }

    }
}