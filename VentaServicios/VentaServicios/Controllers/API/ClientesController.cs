
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
        public async Task<List<Cliente>> GetClientForPosition(NearClientRequest posicion)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var clientes = await db.Cliente.Where(x => x.IdVendedor==posicion.myId).ToListAsync();
            List<Cliente> Clientes = new List<Cliente>();

            foreach (var cliente in clientes)
            {
                var cposition = new Position
                {
                    latitude = cliente.Latitud,
                    longitude = cliente.Longitud
                };
                if (GeoUtils.EstaCercaDeMi(posicion.Position, cposition, posicion.radio))
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
                var lista = await db.Cliente.Where(x => x.IdVendedor == vendedor.IdVendedor && x.Estado==1).Select(x => new ClienteRequest
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
        [Route("VerEstadisticosCliente")]
        public async Task<EstadisticosClienteRequest> VerEstadisticosVendedor(ClienteRequest clienteRequest)
        {

            var estadisticoVendedorRequest = new EstadisticosClienteRequest();

            var listaVisitas = new List<Visita>();

            try
            {

                // Lógica para estadísticos pasteles (tipo de compromiso)
                var listaCompromiso = await db.Compromiso
                    .Join(db.TipoCompromiso, com => com.IdTipoCompromiso, tc => tc.IdTipoCompromiso, (com, tc) => new { tcom = com, ttc = tc })
                    .Join(db.Visita, conjunto1 => conjunto1.tcom.idVisita, visita => visita.idVisita, (conjunto1, visita) => new { Aconjunto1 = conjunto1, Avis = visita })
                    .Join(db.Cliente, conjunto2 => conjunto2.Avis.idCliente, ven => ven.idCliente, (conjunto2, ven) => new { AConjunto2 = conjunto2, Aven = ven })

                .Where(y => y.Aven.idCliente == clienteRequest.IdCliente)
                .Select(x => new TipoCompromisoRequest
                {
                    IdTipoCompromiso = x.AConjunto2.Aconjunto1.ttc.IdTipoCompromiso,
                    Descripcion = x.AConjunto2.Aconjunto1.ttc.Descripcion

                }

                ).GroupBy(z => z.Descripcion).ToListAsync();


                var listaTipoCompromisos = new List<TipoCompromisoRequest>();

                for (int i = 0; i < listaCompromiso.Count; i++)
                {
                    var num = listaCompromiso.ElementAt(i).Count();

                    listaTipoCompromisos.Add(
                        new TipoCompromisoRequest
                        {
                            Descripcion = listaCompromiso.ElementAt(i).ElementAt(0).Descripcion,
                            CantidadCompromiso = num
                        }
                    );

                }


                // Lógica para compromisos cumplidos - incumplidos

                var cumplidos = await db.Compromiso
                    .Join(db.Visita, com => com.idVisita, v => v.idVisita, (com, v) => new { tcom = com, tv = v })
                    .Join(db.Cliente, conjunto => conjunto.tv.idCliente, ven => ven.idCliente, (conjunto, ven) => new { varConjunto = conjunto, tven = ven })
                    .Where(y => y.tven.idCliente == clienteRequest.IdCliente && !String.IsNullOrEmpty(y.varConjunto.tcom.Solucion))
                    .ToListAsync();

                var incumplidos = await db.Compromiso
                    .Join(db.Visita, com => com.idVisita, v => v.idVisita, (com, v) => new { tcom = com, tv = v })
                    .Join(db.Cliente, conjunto => conjunto.tv.idCliente, ven => ven.idCliente, (conjunto, ven) => new { varConjunto = conjunto, tven = ven })
                    .Where(y => y.tven.idCliente == clienteRequest.IdCliente && String.IsNullOrEmpty(y.varConjunto.tcom.Solucion))
                    .ToListAsync();


                estadisticoVendedorRequest.IdCliente = clienteRequest.IdCliente;
                estadisticoVendedorRequest.ListaTipoCompromiso = listaTipoCompromisos;
                estadisticoVendedorRequest.CompromisosCumplidos = cumplidos.Count();
                estadisticoVendedorRequest.CompromisosIncumplidos = incumplidos.Count();

                return estadisticoVendedorRequest;
            }
            catch (Exception ex)
            {
                return estadisticoVendedorRequest;
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
                                             Where(x => x.TipoCliente.IdEmpresa == clienteRequest.IdEmpresa
                                                     && x.Identificacion == clienteRequest.Identificacion)
                                             .FirstOrDefaultAsync();


                if (cliente == null)
                {
                    return new Response { IsSuccess = false };
                };

                if (cliente.idCliente==clienteRequest.IdCliente)
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
                Estado=clienteRequest.Estado,
            };
            try
            {
                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();

                var clienteRespuesta = new ClienteRequest { IdCliente = cliente.idCliente };
                return new Response {IsSuccess=true, Resultado=clienteRespuesta};

            }
            catch (Exception ex )
            {
                return new Response {IsSuccess=false};

            }
            

        }


        [HttpPost]
        [Route("EditarFotoCliente")]
        public async Task<Response> EditarFotoCliente(ClienteRequest clienteRequest)
        {
            var clienteEditar = await db.Cliente.Where(x => x.idCliente == clienteRequest.IdCliente).FirstOrDefaultAsync();
            clienteEditar.Foto = clienteRequest.Foto;
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
        [Route("EditarCliente")]
        public async Task<Response> EditarCliente(ClienteRequest clienteRequest)
        {
            var clienteEditar =await db.Cliente.Where(x => x.idCliente == clienteRequest.IdCliente).FirstOrDefaultAsync();
            clienteEditar.Apellido = clienteRequest.Apellido;
            clienteEditar.Direccion = clienteRequest.Direccion;
            clienteEditar.Email = clienteRequest.Email;
            clienteEditar.Identificacion = clienteRequest.Identificacion;
            clienteEditar.idTipoCliente = clienteRequest.IdTipoCliente;
            clienteEditar.IdVendedor = clienteRequest.IdVendedor;
            clienteEditar.Latitud = clienteRequest.Latitud;
            clienteEditar.Longitud = clienteRequest.Longitud;
            clienteEditar.Nombre = clienteRequest.Nombre;
            clienteEditar.Telefono = clienteRequest.Telefono;
            clienteEditar.TelefonoMovil = clienteRequest.TelefonoMovil;
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

        /// <summary>
        /// Recuperamos todos los datos necesarios del cliente para la aplicación 
        /// </summary>
        /// <param name="clienteRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DatosCliente")]
        public async Task<Response> DatosCliente(ClienteRequest clienteRequest)
        {
            try
            {
                List<CompromisoRequest> compromisos= new List<CompromisoRequest>();

                db.Configuration.ProxyCreationEnabled = false;

                var cliente = await db.Cliente.Where(x => x.idCliente == clienteRequest.IdCliente).FirstOrDefaultAsync();
                var compromisosaux = await db.Compromiso.Where(x => x.Visita.idCliente == clienteRequest.IdCliente).ToListAsync();
                if (compromisosaux!=null)
                {
                    foreach (var item in compromisosaux)
                    {
                        bool sol = true;
                        if (item.Solucion==null|| item.Solucion=="")
                        {
                            sol = false;
                        }
                        compromisos.Add(
                            new CompromisoRequest
                            {
                                idVisita=item.idVisita,
                                IdCompromiso=item.IdCompromiso,
                                IdTipoCompromiso=item.IdTipoCompromiso,
                                Solucion=item.Solucion,
                                isSolucion=sol,
                                isEnable=!sol,
                                Descripcion=item.Descripcion,
                            }
                            );
                    }
                }



                DatosClienteRequest dcr = new DatosClienteRequest
                {
                    cliente = cliente,
                    compromisos = compromisos
                };
                return new Response { IsSuccess = true, Resultado = dcr };
            }
            catch (Exception ex)
            {
                return new Response { IsSuccess = false, Message= ex.Message};
            }
        }
    }
}