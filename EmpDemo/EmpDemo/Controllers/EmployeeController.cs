using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using EmpDemo.Models;
using Microsoft.AspNetCore.Hosting;

namespace EmpDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = "Select * from Employee";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("EmployeeDB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"Insert into dbo.Employee values
                (
                '" + employee.EmployeeName + @"'
                , '"+employee.DepartmentName+ @"'
                , '"+employee.DateOfJoining+ @"'
                , '"+employee.PhotoFileName+ @"'
                )";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("EmployeeDB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                update dbo.Employee set 
                EmployeeName = '" + employee.EmployeeName + @"'
                ,DepartmentName = '" + employee.DepartmentName + @"'
                ,DateOfJoining = '" + employee.DateOfJoining + @"'
                ,PhotoFileName = '" + employee.PhotoFileName + @"'
                where EmployeeId = " + employee.EmployeeId + @" 
                ";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("EmployeeDB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                    Delete from Employee 
                    where EmployeeID = " + id + @"
                    ";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("EmployeeDB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Delete Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);

                }

                return new JsonResult(filename);
            }
            catch
            {
                return new JsonResult("empty.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        [HttpGet]

        public JsonResult GetAllDepartmentNames()
        {
            string query = "Select DepartmentName from Department";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("EmployeeDB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
}
}
