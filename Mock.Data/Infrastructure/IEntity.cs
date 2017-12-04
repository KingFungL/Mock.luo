using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Code;

namespace Mock.Data
{
    public class IEntity<TEntity>
    {
        public virtual void Create()
        {
            var entity = this as ICreationAudited;
            var LoginInfo = OperatorProvider.Provider.CurrentUser;
            if (LoginInfo != null)
            {
                entity.CreatorUserId = LoginInfo.UserId;
            }
            entity.DeleteMark = false;
            entity.CreatorTime = DateTime.Now;
        }
        public virtual void Modify(int? keyValue)
        {
            var entity = this as IModificationAudited;
            entity.Id = keyValue;
            var LoginInfo = OperatorProvider.Provider.CurrentUser;
            if (LoginInfo != null)
            {
                entity.LastModifyUserId = LoginInfo.UserId;
            }
            entity.LastModifyTime = DateTime.Now;

        }
        public virtual void Remove()
        {
            var entity = this as IDeleteAudited;
            var LoginInfo = OperatorProvider.Provider.CurrentUser;
            if (LoginInfo != null)
            {
                entity.DeleteUserId = LoginInfo.UserId;
            }
            entity.DeleteTime = DateTime.Now;
            entity.DeleteMark = true;
        }
    }
}
