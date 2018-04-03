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

namespace VentaServicios.Controllers.API
{
    [RoutePrefix("api/Supervisor")]
    public class SupervisorsController : ApiController
    {
        private ModelVentas db = new ModelVentas();

        // GET: api/Supervisors
        //public IQueryable<Supervisor> GetSupervisor()
        //{
        //    return db.Supervisor;
        //}
        #region listarVendedores

        [HttpPost]
        [Route("ListarVendedores")]
        public async Task<List<SupervisorRequest>> ListarVendedores(SupervisorRequest supervisorRequest)
        {
            

            var super = db.Supervisor.Where(x => x.IdUsuario == supervisorRequest.IdUsuario).FirstOrDefault();
            var lista = await db.Vendedor.Where(m => m.IdSupervisor == super.IdSupervisor && m.AspNetUsers.Estado ==1 && m.AspNetUsers.IdEmpresa==supervisorRequest.IdEmpresa ).Select(x => new SupervisorRequest
            {
                IdVendedor = x.IdVendedor,
                IdUsuario = x.AspNetUsers.Id,
                Identificacion = x.AspNetUsers.Identificacion,
                Nombres = x.AspNetUsers.Nombres,
                Apellidos = x.AspNetUsers.Apellidos,
                Direccion = x.AspNetUsers.Direccion,
                Telefono = x.AspNetUsers.Telefono,
                Correo = x.AspNetUsers.Email,
                IdEmpresa = x.AspNetUsers.IdEmpresa,
                NombresApellido = x.AspNetUsers.Nombres +" "+x.AspNetUsers.Apellidos
            }).ToListAsync();


            return lista;
        }
        #endregion




        #region listarSupervisor

        [HttpPost]
        [Route("ListarSupervisores")]
        public async Task<List<SupervisorRequest>> ListarSupervisores(EmpresaActual empresaActual)
        {
            var lista = await db.Supervisor.Where(x=> x.AspNetUsers.IdEmpresa==empresaActual.IdEmpresa && x.AspNetUsers.Estado == 1).Select(x => new SupervisorRequest
            {
                IdSupervisor = x.IdSupervisor,
                Apellidos = x.AspNetUsers.Apellidos,
                Direccion = x.AspNetUsers.Direccion,
                Identificacion = x.AspNetUsers.Identificacion,
                IdGerente = x.IdGerente,
                IdUsuario = x.AspNetUsers.Id,
                Nombres = x.AspNetUsers.Nombres,
                Telefono = x.AspNetUsers.Telefono,
            }).ToListAsync();

            return lista;
        }
        #endregion

        #region InsertarSupervisor
        [HttpPost]
        [Route("InsertarSupervisor")]
        public async Task<Response> InsertarSupervisor(SupervisorRequest supervisorRequest)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //var user = new  AspNetUsers();
                    //user.IdEmpresa = supervisorRequest.IdEmpresa;
                    //user.Identificacion = supervisorRequest.Identificacion;
                    //user.Apellidos = supervisorRequest.Apellidos;
                    //user.Nombres = supervisorRequest.Nombres;
                    //user.Direccion = supervisorRequest.Direccion;
                    //user.Telefono = supervisorRequest.Telefono;
                    //user.Email = supervisorRequest.Correo;
                    //user.UserName = supervisorRequest.Correo;
                    //user.PasswordHash = "123";
                    //db.AspNetUsers.Add(user);
                    //await db.SaveChangesAsync();

                    var gerente = db.Gerente.Where(x => x.AspNetUsers.IdEmpresa == supervisorRequest.IdEmpresa);
                    if (gerente !=null)
                    {
                        var super = new Supervisor();
                        super.IdGerente = gerente.FirstOrDefault().IdGerente;
                        super.IdUsuario = supervisorRequest.IdUsuario;
                        db.Supervisor.Add(super);

                    }
                    await db.SaveChangesAsync();
                    transaction.Commit();

                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
                    };

                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.Error,
                    };
                }
            }
        }
        #endregion



        // [HttpGet("{id}")]
        //[ResponseType(typeof(Supervisor))]
        [HttpPost]
        [Route("obtenerSupervisor")]
        public async Task<Response> obtenerSupervisor( SupervisorRequest id)
        {
            var supervisor = new SupervisorRequest();
            var vendedor = new VendedorRequest();
            var listaVendedores = new List<VendedorRequest>();

            int idEmpresa = Convert.ToInt32(id.IdEmpresa);
            supervisor.IdEmpresa = idEmpresa;
            vendedor.idEmpresa = idEmpresa;

            VendedoresController ctl = new VendedoresController();
            try
            {
                

                supervisor = await db.Supervisor.Where(m => m.IdSupervisor == id.IdSupervisor).Select(x => new SupervisorRequest
                {
                    IdUsuario = x.AspNetUsers.Id,
                    IdSupervisor = x.IdSupervisor,
                    Identificacion = x.AspNetUsers.Identificacion,
                    Nombres = x.AspNetUsers.Nombres,
                    Apellidos = x.AspNetUsers.Apellidos,
                    Direccion = x.AspNetUsers.Direccion,
                    Telefono = x.AspNetUsers.Telefono,
                    Correo = x.AspNetUsers.Email,
                    IdEmpresa = x.AspNetUsers.IdEmpresa,
                    IdGerente = x.IdGerente
                   }).SingleOrDefaultAsync();
                
                supervisor.ListaVendedores = await ctl.ListarVendedores(vendedor);
                supervisor.ListaVendedoresAsignados = supervisor.ListaVendedores.Where(x => x.IdSupervisor == id.IdSupervisor).ToList();
                supervisor.ListaVendedoresSinAsignar = supervisor.ListaVendedores.Where(x => x.IdSupervisor != id.IdSupervisor).ToList();


                if (supervisor == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };

                }
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = supervisor,

                };
            }

            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }

        }
        [HttpPost]
        [Route("Quitarvendedor")]
        public async Task<Response> Quitarvendedor(SupervisorRequest supervisorRequestd)
        {
                try
                {
                    var vendedor =  await db.Vendedor.Where(x => x.IdVendedor == supervisorRequestd.IdVendedor).FirstOrDefaultAsync();
                    if (vendedor != null)
                    {
                    vendedor.IdSupervisor = null;
                        db.Entry(vendedor).State= EntityState.Modified;
                        await db.SaveChangesAsync();

                    }

                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
                    };

                }

                catch (Exception ex)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.Error,
                    };
                }
        }
        [HttpPost]
        [Route("Asignarvendedor")]
        public async Task<Response> Asignarvendedor(SupervisorRequest supervisorRequestd)
        {
            try
            {
                var vendedor = await db.Vendedor.Where(x => x.IdVendedor == supervisorRequestd.IdVendedor).FirstOrDefaultAsync();
                if (vendedor != null)
                {
                    vendedor.IdSupervisor = supervisorRequestd.IdSupervisor;
                    db.Entry(vendedor).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                }

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio
                };

            }

            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        
    }
}