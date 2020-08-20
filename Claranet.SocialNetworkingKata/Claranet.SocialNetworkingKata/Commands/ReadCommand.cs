﻿using Claranet.SocialNetworkingKata.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Commands
{
    class ReadCommand : ISocialCommand
    {
        private string User { get; }
        private IStorageProvider Storage { get; }
        private ITimeProvider Time { get; }
        private IInteractionProvider Interaction { get; }

        public ReadCommand(IStorageProvider storage, IInteractionProvider interaction, ITimeProvider time, IDictionary<string, string> arguments)
        {
            this.Interaction = interaction;
            this.Time = time;
            this.Storage = storage;

            string user;
            if (!arguments.TryGetValue("user", out user))
                throw new ArgumentException(nameof(user));
            this.User = user;
        }

        public async Task Execute()
        {
            try
            {
                var messages = await this.Storage.GetMessagesByUser(this.User);

                foreach (var m in messages)
                    this.Interaction.Write($"{m.Message} ({this.Time.ToSocialTime(m.Time)})");
            }
            catch (Exception ex)
            {
                this.Interaction.Error($"error reading: {ex.Message}");
            }
        }
    }
}
