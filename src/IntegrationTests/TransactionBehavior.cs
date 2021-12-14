//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using StackExchange.Redis;
//using Xunit;

//namespace IntegrationTests
//{
//    public class TransactionBehavior
//    {
//        [Fact]
//        public async Task ShouldApplyTransaction()
//        {
//            //Arrange   
//            var redis = TestTools.CreateRedisService();
//            var key = redis.Db().String("foo");
            
//            RedisValue resultKeyVal = default;

//            try
//            {
//                await using var t = redis.Db().BeginTransaction();
//                {
//                    t.Enqueue(db => db.String("foo").SetAsync("foo_val"));
//                }

//                //Act
//                resultKeyVal = await key.GetAsync();
//            }
//            catch (Exception)
//            {
//                await key.DeleteAsync();
//            }

//            //Assert
//                Assert.Equal("foo_val", resultKeyVal);
//        }

//        [Fact]
//        public async Task ShouldNotApplyTransactionWhenNotCommitted()
//        {
//            //Arrange   
//            var redis = TestTools.CreateRedisService();
            
//            var key = redis.Db().String("foo");
//            await key.SetAsync("foo_val");
            
//            RedisValue resultKeyVal = default;

//            try
//            {
//                await using var t = redis.Db().BeginTransaction();
//                {
//                    t.Enqueue(db => db.String("foo").SetAsync("bar_val"));

//                    //Act
//                    resultKeyVal = await key.GetAsync();
//                }
//            }
//            catch (Exception)
//            {
//                await key.DeleteAsync();
//            }

//            //Assert
//            Assert.Equal("foo_val", resultKeyVal);
//        }
//    }
//}
