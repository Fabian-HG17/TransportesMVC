using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using TransportesMVC.Models;
using TransportesMVC.Models.ViewModels;
using static TransportesMVC.Models.Utilidades;

namespace TransportesMVC.Controllers.Camiones
{
    public class CamionesController : Controller
    {
        // GET: Camiones
        public ActionResult Index()
        {
            //Crear una lista de Camiones en función de mi VM
            List<CamionesLista> listaCamiones = new List<CamionesLista>();
            //Llenar mi lista de camiones desde el contexto que traje de mi BD
            //Utilizando EF y LinQ
            using (TransportesEntities db = new TransportesEntities())
            {
                listaCamiones = (from c in db.Camiones
                                 select new CamionesLista
                                 {
                                     id_Camion = c.id_Camion,
                                     matricula = c.matricula,
                                     tipo_Camion = c.tipo_Camion,
                                     marca = c.marca,
                                     modelo = c.modelo,
                                     capacidad = c.capacidad,
                                     kilometraje = c.kilometraje,
                                     url_Foto = c.url_Foto,
                                     disponibilidad = c.disponibilidad,
                                     Chofer_ID = c.Chofer_ID

                                 }).ToList();

            }
            ViewBag.Title = "Listar Camiones";
            ViewData["Title"] = "Listar Camiones";
            return View(listaCamiones);
        }

        public ActionResult CamionesVista()
        {
            //Crear una lista de Camiones en función de mi VM
            List<Camiones_ChoferesLista> list = new List<Camiones_ChoferesLista>();
            //Llenar mi lista de camiones desde el contexto que traje de mi BD
            //Utilizando EF y LinQ
            using (TransportesEntities db = new TransportesEntities())
            {
                list = (from c in db.Camiones_Choferes
                        select new Camiones_ChoferesLista
                        {
                            id_Camion = c.id_Camion,
                            matricula = c.matricula,
                            tipo_Camion = c.tipo_Camion,
                            marca = c.marca,
                            modelo = c.modelo,
                            capacidad = c.capacidad,
                            kilometraje = c.kilometraje,
                            url_Foto = c.url_Foto,
                            disponibilidad = c.disponibilidad,
                            Chofer_ID = c.Chofer_ID,
                            Nombre_chofer = c.Nombre_chofer
                        }).ToList();

            }
            ViewBag.Title = "Listar Camiones";
            ViewData["Title"] = "Listar Camiones";
            return View(list);
        }

        public ActionResult ViewLinQ()
        {
            List<Camiones_ChoferesLista> list = new List<Camiones_ChoferesLista>();
            using (TransportesEntities db = new TransportesEntities())
            {
                list = (from c in db.Camiones
                        join ch in db.Choferes on c.Chofer_ID equals ch.idchofer
                        select new Camiones_ChoferesLista
                        {
                            id_Camion = c.id_Camion,
                            matricula = c.matricula,
                            tipo_Camion = c.tipo_Camion,
                            marca = c.marca,
                            modelo = c.modelo,
                            capacidad = c.capacidad,
                            kilometraje = c.kilometraje,
                            url_Foto = c.url_Foto,
                            disponibilidad = c.disponibilidad,
                            Chofer_ID = c.Chofer_ID,
                            Nombre_chofer = ch.nombre + " " + ch.apellido_p + " " + ch.apellido_m
                        }).ToList();

            }
            return View(list);
        }
        public ActionResult Nuevo_Camion()
        {
            CargarDDL();
            return View();
        }
        [HttpPost]
        public ActionResult Nuevo_Camion(CamionDTO model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new TransportesMVC.Models.Camiones();
                        camion.id_Camion = model.id_Camion;
                        camion.matricula = model.matricula;
                        camion.tipo_Camion = model.tipo_Camion;
                        camion.marca = model.marca;
                        camion.modelo = model.modelo;
                        camion.capacidad = model.capacidad;
                        camion.kilometraje = model.kilometraje;
                        camion.disponibilidad = model.disponibilidad;
                        camion.Chofer_ID = model.Chofer_ID;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Imagenes/Camiones/");
                            if (!Directory.Exists(pathdir))
                            {
                                Directory.CreateDirectory(pathdir);
                            }
                            imagen.SaveAs(pathdir + filename);
                            camion.url_Foto = "/Imagenes/Camiones/" + filename;
                        }
                        //Agregamos nuestro camión al contexto
                        context.Camiones.Add(camion);
                        //Guardamos camnios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Camiones/ViewLinQ");
                    }
                }
                else
                {
                    Alert("Datos no válidos", NotificationType.error);
                    CargarDDL();
                    return View(model);
                }


            }
            catch (DbEntityValidationException ex)
            {
                //varaiable para respuesta
                string resp = "";
                //Recorro todos los posibles errores de validacion de la entidad referencial de lña BD
                foreach (var error in ex.EntityValidationErrors)
                {
                    //Recorro los detalles de cada uno de esos posibles errores
                    foreach (var validationerror in error.ValidationErrors)
                    {
                        resp = "Error en la entidad: " + error.Entry.Entity.GetType().Name;
                        resp += "  Propiedad: " + validationerror.PropertyName;
                        resp += " Error: " + validationerror.ErrorMessage;
                    }
                }
                CargarDDL();
                return View(model);
            }
        }
        public ActionResult Editar_Camion(int id)
        {
            CargarDDL();

            TransportesMVC.Models.Camiones camion = new TransportesMVC.Models.Camiones();
            if (id > 0)
            {
                using (TransportesEntities context = new TransportesEntities())
                {
                    camion = context.Camiones.Where(x => x.id_Camion == id).FirstOrDefault();
                }
                ViewBag.Title = "Editar camión no: " + camion.id_Camion;
                if (camion != null)
                {
                    return View(camion);
                }
                else
                {
                    return Redirect("~/Index");
                }

            }
            else
            {
                return Redirect("~/Index");
            }

        }
        [HttpPost]
        public ActionResult Editar_Camion(TransportesMVC.Models.Camiones model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransportesEntities context = new TransportesEntities())
                    {
                        var camion = new TransportesMVC.Models.Camiones();
                        camion.id_Camion = model.id_Camion;
                        camion.matricula = model.matricula;
                        camion.tipo_Camion = model.tipo_Camion;
                        camion.marca = model.marca;
                        camion.modelo = model.modelo;
                        camion.capacidad = model.capacidad;
                        camion.kilometraje = model.kilometraje;
                        camion.disponibilidad = model.disponibilidad;
                        camion.Chofer_ID = model.Chofer_ID;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Imagenes/Camiones/");

                            if (model.url_Foto.Contains(filename))
                            {
                                //Es el mismo, no actualizamos
                                camion.url_Foto = model.url_Foto;
                            }
                            else
                            {
                                //Es nuevo entonces actualizamos
                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }
                                //Borrar Imagen anterior 
                                //Validamos si existe
                                try
                                {
                                    string pathdir_old = Server.MapPath("~" + model.url_Foto);
                                    if (System.IO.File.Exists(pathdir_old))
                                    {
                                        //Si existe la borro
                                        System.IO.File.Delete(pathdir_old);
                                    }
                                }
                                catch (Exception e)
                                {

                                    Alert("Error: " + e.Message, NotificationType.error);
                                }
                                imagen.SaveAs(pathdir + filename);
                                camion.url_Foto = "/Imagenes/Camiones/" + filename;

                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }
                                imagen.SaveAs(pathdir + filename);
                                camion.url_Foto = "/Imagenes/Camiones/" + filename;
                            }

                        }
                        else
                        {
                            camion.url_Foto = model.url_Foto;
                        }
                        //Agregamos nuestro camión al contexto
                        context.Entry(camion).State = System.Data.Entity.EntityState.Modified;
                        //Guardamos camnios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Camiones/ViewLinQ");
                    }
                }
                else
                {
                    Alert("Datos no válidos", NotificationType.error);
                    CargarDDL();
                    return View(model);
                }


            }
            catch (DbEntityValidationException ex)
            {
                //varaiable para respuesta
                string resp = "";
                //Recorro todos los posibles errores de validacion de la entidad referencial de lña BD
                foreach (var error in ex.EntityValidationErrors)
                {
                    //Recorro los detalles de cada uno de esos posibles errores
                    foreach (var validationerror in error.ValidationErrors)
                    {
                        resp = "Error en la entidad: " + error.Entry.Entity.GetType().Name;
                        resp += "  Propiedad: " + validationerror.PropertyName;
                        resp += " Error: " + validationerror.ErrorMessage;
                    }
                }
                CargarDDL();
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Eliminar_Camion(int id)
        {
            try
            {
                using(TransportesEntities context = new TransportesEntities())
                {
                    TransportesMVC.Models.Camiones _camion = context.Camiones.Where(
                        f => f.id_Camion == id).FirstOrDefault();
                    if( _camion != null )
                    {
                        context.Camiones.Remove(_camion);
                        context.SaveChanges();
                        Alert("Registro Eliminado con éxito", NotificationType.success);
                        return Redirect("~/Camiones/ViewLinQ");
                    }
                    else
                    {
                        Alert("No existe el recurso solicitado", NotificationType.info);
                        return Redirect("~/Camiones/ViewLinQ");
                    }
                }
            }
            catch (Exception e)
            {
                Alert("Error: " + e.Message, NotificationType.error);
                return Redirect("~/Camiones/ViewLinQ");
            }
        }
        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>Swal.fire('" +
                notificationType.ToString().ToUpper() + "', '" + message + "','" +
                notificationType + "')" + "</script>";
            TempData["notification"] = msg;
        }

        public void CargarDDL()
        {
            //ViewBag
            List<ChoferDDL> listChoferes = new List<ChoferDDL>();
            listChoferes.Insert(0, new ChoferDDL { id_Chofer = 0, Nombre = "Seleccione un Chofer" });
            using (TransportesEntities db = new TransportesEntities())
            {
                foreach (var ch in db.Choferes)
                {
                    ChoferDDL _aux = new ChoferDDL();
                    _aux.id_Chofer = ch.idchofer;
                    _aux.Nombre = ch.nombre + " " + ch.apellido_p + " " + ch.apellido_m;
                    listChoferes.Add(_aux);

                }
            }
            ViewBag.ListaChoferes = listChoferes;
        }
    }
}