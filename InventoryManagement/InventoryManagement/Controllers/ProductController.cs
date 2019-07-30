using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
     [RoutePrefix("api")]
    public class ProductController : ApiController
    {
        //authetication
        [HttpGet]
        [Route("AuthenticateUser")]
        public string AuthenticateUser(string Username,string Password)
        {
            string role = null;
            var cs = ConfigurationManager.ConnectionStrings["dbcs"].ToString();
            using(var con = new SqlConnection(cs))
            {
                string procedure = "GetRole";
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(procedure,con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username",Username);
                cmd.Parameters.AddWithValue("@password", Password);
                con.Open();


                reader =cmd.ExecuteReader();
                if (reader.Read())
                {
                    role = reader.GetString(0);
                }
            }
            return role;
        }


        //return products list
        [HttpGet]
        [Route("ShowProducts")]
        public HttpResponseMessage ShowProducts()
        {
            var cs = ConfigurationManager.ConnectionStrings["dbcs"].ToString();
            var products = new List<productDetails>();
            using (var con = new SqlConnection(cs))
            {
                string procedure = "ListOfProducts";
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(procedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new productDetails
                    {
                        ProductName = reader["Pname"].ToString(),
                        Quantity = Convert.ToInt16(reader["Quantity"])
                    });
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, products);
        }

        //adding product
        [HttpGet]
        [Route("AddProduct")]
        public HttpResponseMessage AddProduct(int ProductId,int DeptIt,string ProductName,int Quantity,int Price)
        {
            var cs = ConfigurationManager.ConnectionStrings["dbcs"].ToString();
            var products = new List<productDetails>();
            using (var con = new SqlConnection(cs))
            {
                string procedure = "AddProduct";
                
                SqlCommand cmd = new SqlCommand(procedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pid", ProductId);
                cmd.Parameters.AddWithValue("@did", DeptIt);
                cmd.Parameters.AddWithValue("@pname", ProductName);
                cmd.Parameters.AddWithValue("@quantity", Quantity);
                cmd.Parameters.AddWithValue("@price", Price);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Request.CreateResponse(HttpStatusCode.OK,"Ok");
        }


        [HttpGet]
        [Route("DeptDetails")]
        public HttpResponseMessage DeptDetails()
        {
            var deptDetails=new List<Department>();
            var cs = ConfigurationManager.ConnectionStrings["dbcs"].ToString();
            var products = new List<productDetails>();
            using (var con = new SqlConnection(cs))
            {
                string procedure = "GetDeptDetails";
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(procedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    deptDetails.Add(new Department
                    {
                        Dname = reader["Dname"].ToString(),
                        Did = Convert.ToInt16(reader["Did"])
                    });
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, deptDetails);
        }
    }
}
