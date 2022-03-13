﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using WanderLost.Shared;

namespace WanderLost.Client
{
    public class MerchantHubClient : IMerchantHubClient, IAsyncDisposable
    {
        public HubConnection HubConnection {get; init; }

        public MerchantHubClient(NavigationManager navigationManager)
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri($"/{nameof(IMerchantHubClient)}"))
                .WithAutomaticReconnect()
                .Build();
        }

        public async ValueTask DisposeAsync()
        {
            if(HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }
        }

        public async Task SubscribeToServer(string server)
        {
            await HubConnection.SendAsync(nameof(SubscribeToServer), server);
        }

        public async Task UnsubscribeFromServer(string server)
        {
            await HubConnection.SendAsync(nameof(UnsubscribeFromServer), server);
        }

        public async Task UpdateMerchant(string server, ActiveMerchant merchant)
        {
            await HubConnection.SendAsync(nameof(UpdateMerchant), server, merchant);
        }

        public void OnUpdateMerchant(Action<string, ActiveMerchant> action)
        {
            HubConnection.On(nameof(UpdateMerchant), action);
        }
    }
}
