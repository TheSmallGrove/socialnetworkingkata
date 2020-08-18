using Claranet.SocialNetworkingKata.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Providers
{
    class InMemoryProvider : IStorageProvider
    {
        private IList<Post> Posts = new List<Post>();
        private IList<FollowedUser> FollowedUsers = new List<FollowedUser>();

        public Task AddFollowerToUser(string user, string userToFollow)
        {
            if (!this.FollowedUsers.Any(_ => _.Followed == userToFollow && _.User == user))
            {
                var follower = new FollowedUser
                {
                    User = user,
                    Followed = userToFollow
                };

                this.FollowedUsers.Add(follower);
            }
            return Task.CompletedTask;
        }

        public Task AddMessageForUser(string author, string message, DateTime time)
        {
            var post = new Post
            {
                Author = author,
                Time = time,
                Message = message
            };

            this.Posts.Add(post);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Post>> GetMessagesByUser(string user)
        {
            return Task.FromResult(from post in this.Posts
                                   where post.Author == user
                                   select post);
        }

        public async Task<IEnumerable<Post>> GetWallByUser(string user)
        {
            return await Task.FromResult((from post in this.Posts
                                          join follower in this.FollowedUsers on post.Author equals follower.Followed
                                          where follower.User == user
                                          select post)
                                          .Union(await this.GetMessagesByUser(user)));
        }

        public Task InitializeIfRequired()
        {
            return Task.CompletedTask;
        }
    }
}
