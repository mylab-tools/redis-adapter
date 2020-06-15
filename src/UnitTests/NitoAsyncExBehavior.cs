//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Nito.AsyncEx;
//using Xunit;

//namespace UnitTests
//{
//    public class NitoAsyncExBehavior
//    {
//        [Fact]
//        public async Task ShouldNotifyAboutLockFail()
//        {
//            //Arrange
//            var l = new AsyncLock();

//            //Act

//            using (await l.LockAsync(CancellationToken.None))
//            {
//                using (await l.LockAsync(new CancellationToken(true)))
//                {

//                }
//            }

//            //Assert

//        }
//    }
//}
