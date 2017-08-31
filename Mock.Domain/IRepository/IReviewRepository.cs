using Mock.Data;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Domain
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        /// <summary>
        /// 得到最新的count条回复信息
        /// </summary>
        /// <returns></returns>
        dynamic GetRecentReview(int count);
    }
}
