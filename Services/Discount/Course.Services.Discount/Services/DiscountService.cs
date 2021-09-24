using Course.Services.Discount.Models;
using Course.Shared.Dtos;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;

            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> DeleteAsync(int id)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });

            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<List<DiscountItem>>> GetAllAsync()
        {
            var discounts = await _dbConnection.QueryAsync<DiscountItem>("Select * from discount");

            return Response<List<DiscountItem>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<DiscountItem>> GetCodeByUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<DiscountItem>("select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = code });

            var hasDiscount = discounts.FirstOrDefault();

            if (hasDiscount == null)
            {
                return Response<DiscountItem>.Fail("Discount not found", 404);
            }

            return Response<DiscountItem>.Success(hasDiscount, 200);
        }

        public async Task<Response<DiscountItem>> GetByIdAsync(int id)
        {
            var discount = (await _dbConnection.QueryAsync<DiscountItem>("select * from discount where id=@Id", new { Id = id })).SingleOrDefault();

            if (discount == null)
            {
                return Response<DiscountItem>.Fail("Discount not found", 404);
            }

            return Response<DiscountItem>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> SaveAsync(DiscountItem discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount);

            if (saveStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("an error occurred while adding", 500);
        }

        public async Task<Response<NoContent>> UpdateAsync(DiscountItem discount)
        {
            var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id", new { Id = discount.Id, UserId = discount.UserId, Code = discount.Code, Rate = discount.Rate });

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Discount not found", 404);
        }

    
    }
}
