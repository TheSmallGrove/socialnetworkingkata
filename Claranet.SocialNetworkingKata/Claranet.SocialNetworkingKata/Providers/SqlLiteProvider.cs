using Claranet.SocialNetworkingKata.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Providers
{
    class SqlLiteProvider : IStorageProvider
    {
        public Task InitializeIfRequired()
        {
            throw new NotImplementedException();
        }

        public Task AddFollowerToUser(string user, string userToFollow)
        {
            throw new NotImplementedException();
        }

        public Task AddMessageForUser(string author, string message, DateTime time)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> GetMessagesByUser(string user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> GetWallByUser(string user)
        {
            throw new NotImplementedException();
        }
    }
}
