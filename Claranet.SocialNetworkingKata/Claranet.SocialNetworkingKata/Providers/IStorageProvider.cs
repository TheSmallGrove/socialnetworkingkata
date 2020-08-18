using Claranet.SocialNetworkingKata.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Providers
{
    interface IStorageProvider
    {
        Task AddMessageForUser(string author, string message, DateTime time);
        Task<IEnumerable<Post>> GetMessagesByUser(string user);
        Task AddFollowerToUser(string user, string userToFollow);
        Task<IEnumerable<Post>> GetWallByUser(string user);
    }
}
