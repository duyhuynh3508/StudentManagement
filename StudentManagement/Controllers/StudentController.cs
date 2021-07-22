using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using StudentManagement.Data;
using StudentManagement.Entities;
using StudentManagement.Models;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace StudentManagement.Controllers
{

    public class StudentController : Controller
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IMapper _mapper;
        private readonly StudentDBContext _db;
        private readonly IStudentService _stdService;
        public StudentController( IStudentService stdService,
           IMapper mapper, StudentDBContext db)
        {
            _mapper = mapper;
            _stdService = stdService;
            _db = db;
        }

        /// <summary>
        /// Return all student.
        /// </summary>
        //View Get all student
        [Route("GetAllStudent")]
        [HttpGet]
        public IActionResult GetAllStudent(int? page)
        {
            try
            {
                var pageNumber = page ?? 1;
                int pageSize = 4;
                var x = _stdService.GetAll();
                var onePageOfStudents = x.ToPagedList(pageNumber, pageSize);
                _logger.Info("Access View Get All Student Page: " + pageNumber);
                return View(onePageOfStudents);
            }
            catch(Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }            
        }

        /// <summary>
        /// Return form add new student.
        /// </summary>
        //View add new student
        [Route("AddStudent")]
        [HttpGet]
        public IActionResult AddStudent()
        {
            try 
            {
                var cls = _db.Class.ToList();
                cls.Insert(0, new Class { ClassID = 0, ClassName = "Select" });
                ViewBag.ListClass = cls;
                _logger.Trace("Access View Add Student");
                return View();
            }
            catch (Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }
            
        }

        /// <summary>
        /// Return form edit student.
        /// </summary>
        //View Edit Student
        [Route("EditStudent")]
        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            try
            {
                var cls = _db.Class.ToList();
                cls.Insert(0, new Class { ClassID = 0, ClassName = "Select" });
                ViewBag.ListClass = cls;
                var b = _stdService.GetById(id);
                if (b != null)
                    _logger.Info("Access Edit Student :" + b.StudentName);
                return View(b);
            }
            catch (Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }            
        }

        /// <summary>
        /// Return form delete student.
        /// </summary>
        //View Delete Student
        [Route("DeleteStudent")]
        [HttpGet]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var cls = _db.Class.ToList();
                cls.Insert(0, new Class { ClassID = 0, ClassName = "Select" });
                ViewBag.ListClass = cls;
                var b = _stdService.GetById(id);
                if (b != null)
                    _logger.Info("Access Delete Student :" + b.StudentName);
                return View(b);
            }
            catch(Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }
        }

        /// <summary>
        /// Create new student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST / AddStudent
        ///     {
        ///        "studentname":"std",
        ///        "address": "string",
        ///        "classid": 1,
        ///        "phonenumber":"012345678"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Create succesfully</response>
        /// <response code="400">If item is null</response>  
        //Method Add New Student
        [Route("AddStudent")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddStudent([FromForm]  StudentModel model)
        {
            var obj = _mapper.Map<Student>(model);
            try 
            {
                _stdService.Add(obj);
                _logger.Info("Add Student :" + obj.StudentName + "successfully");
                this.GetAllStudent(1);                
                return View("GetAllStudent");
            }
            catch(ApplicationException e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }
        }

        /// <summary>
        /// Edit student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST / EditStudent
        ///     {
        ///        "studentname":"std",
        ///        "address": "string",
        ///        "classid": 1,
        ///        "phonenumber":"012345678"
        ///     }
        ///     
        /// </remarks>
        /// <response code="302">Found student</response>
        /// <response code="200">Edit successfully</response>
        /// <response code="400">If item is null</response>  
        //Method Edit Student
        [Route("EditStudent")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditStudent([FromBody] StudentModel model, int id)
        {
            try
            {
                var obj = _mapper.Map<Student>(model);
                obj.StudentID = id;
                _stdService.Update(obj);
                _db.Update(_stdService.GetById(obj.StudentID));
                _db.SaveChanges();
                this.GetAllStudent(1);
                _logger.Info("Edit StudentID :" + obj.StudentID +""+ "successfully");
                return View("GetAllStudent");
            }
            catch(Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }
        }

        /// <summary>
        /// Delete student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST / DeleteStudent
        ///     {
        ///        "studentname":"std",
        ///        "address": "string",
        ///        "classid": 1,
        ///        "phonenumber":"012345678"
        ///     }
        ///     
        /// </remarks>
        /// <response code="302">Found student</response>
        /// <response code="200">Delete successfully</response>
        /// <response code="400">If item is null</response>  
        //Method Delete Student
        [Route("DeleteStudent")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteStudent([FromBody] StudentModel model, int id)
        {
            try
            { 
                var obj = _mapper.Map<Student>(model);
                obj.StudentID = id;
                _stdService.Delete(obj.StudentID);
                _db.SaveChanges();
                this.GetAllStudent(1);
                _logger.Info("Delete StudentID :" + obj.StudentID +""+ "successfully");
                return View("GetAllStudent");
            }
            catch(Exception e)
            {
                _logger.Error("Error with exception: " + e);
                return Content("Error with exception: " + e);
            }
        }
    }
}
