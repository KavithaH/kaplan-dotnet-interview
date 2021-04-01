using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using WebApiTest.Data;
using WebApiTest.Models;

namespace WebApiTest.Services
{
    public class OrderItemsService : IOrderItemsService
    {
        public OrderItemsService( TestDbContext dbContext )
        {
            testDbContext = dbContext;
        }

        public OrderItemsModel Get( int orderID )
        {
            IEnumerable<OrderItem> orderItems = testDbContext.OrderItems.Where( oi => oi.OrderID == orderID );
            if (orderItems.Any())
            {
                return new OrderItemsModel
                {
                    OrderID = orderID,
                    Items = orderItems.Select(oi => new OrderItemModel
                    {
                        LineNumber = oi.LineNumber,
                        ProductID = oi.ProductID,
                        StudentPersonID = oi.StudentPersonID,
                        Price = oi.Price
                    })
                };
            }
            return null;
        }

        public async Task<short> AddAsync( int orderID, OrderItemModel item )
        {
            if ( item.LineNumber != 0 )
            {
                throw new ValidationException( "LineNumber is generated and cannot be specified" );
            }

            var lineNumber = (short)( testDbContext.OrderItems.Where( oi => oi.OrderID == orderID ).Max( oi => oi.LineNumber ) + 1 );

            await testDbContext.OrderItems.AddAsync( new OrderItem
                                                         {
                                                             OrderID = orderID,
                                                             LineNumber = lineNumber,
                                                             Price = item.Price,
                                                             ProductID = item.ProductID,
                                                             StudentPersonID = item.StudentPersonID
                                                         } );

            await testDbContext.SaveChangesAsync();

            return lineNumber;
        }

        public async Task<string> DeleteAsync(int orderID, OrderDelModel item)
        {
            var deleteOrder = testDbContext.OrderItems.Where(oi => oi.OrderID == orderID && oi.LineNumber == item.LineNumber).FirstOrDefault();
            if (deleteOrder != null)
            {
                testDbContext.OrderItems.Remove(deleteOrder);
                await testDbContext.SaveChangesAsync();
                return "Order Deleted Successfully";
            }
            return null;
        }

        private readonly TestDbContext testDbContext;
    }
}
