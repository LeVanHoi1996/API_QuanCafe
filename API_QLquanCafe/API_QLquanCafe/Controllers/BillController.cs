using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace API_QLquanCafe.Controllers
{
    public class BillController : ApiController
    {
        //public IEnumerable<Bill> Get()
        //{
        //    using (QuanLyQuanCafeEntities dbcontext = new QuanLyQuanCafeEntities())
        //    {
        //        dbcontext.Configuration.ProxyCreationEnabled = false;
        //        return dbcontext.Bills.ToList();
        //    }
        //}
        public Bill Get(int id)
        {
            using (QuanLyQuanCafeEntities dbContext = new QuanLyQuanCafeEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                return dbContext.Bills.FirstOrDefault(e => e.id == id);
            }
        }
        
        public ProductApiCollection Get()
        {
            QuanLyQuanCafeEntities dbContext = new QuanLyQuanCafeEntities();
            var result = new ProductApiCollection();
            var dbProducts = dbContext.Bills;
            var apiModels = dbProducts.Select(x => new ProductApi { id = x.id }).ToArray();
            result.Products = apiModels;
            var status = dbContext.Bills.Any() ? 1 : 0;
            result.Status = Convert.ToByte(status);
            return result;
        }
        public HttpResponseMessage Post([FromBody] Bill employee)
        {
            using (QuanLyQuanCafeEntities dbContext = new QuanLyQuanCafeEntities())
            {
                try
                {
                    dbContext.Bills.Add(employee);
                    dbContext.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        employee.id.ToString());
                    return message;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
               
            }
        }
        public void Put(int id, [FromBody] Bill employee)
        {
            using (QuanLyQuanCafeEntities dbContext = new QuanLyQuanCafeEntities())
            {
                var entity = dbContext.Bills.FirstOrDefault(e => e.id == id);
                entity.idTable = employee.idTable;
                entity.DateCheckIn = employee.DateCheckIn;
                entity.totalPrice = employee.totalPrice;
                entity.discount = employee.discount;
                entity.status = employee.status;
                dbContext.SaveChanges();
            }
        }
    }
    public class ProductApiCollection
    {
        public ProductApi[] Products { get; set; }
        public byte Status { get; set; }
    }

    public class ProductApi
    {
        public int id { get; set; }
    }
}
