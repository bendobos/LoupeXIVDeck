namespace Loupedeck.LoupeXIVDeckPlugin
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using StructureMap;

    using static Loupedeck.LoupeXIVDeckPlugin.FFXIVGameTypes;

    public class FFXIVApi : IFFXIVApi
    {
        private readonly HttpClient client = new HttpClient();
        private String baseUrl;

        public FFXIVApi(IContainer container)
        {
            var pluginLink = container.GetInstance<IFFXIVPluginLink>();

            pluginLink.GetIsApplicationReadySubject().Subscribe(ready =>
            {
                if (ready)
                {
                    this.baseUrl = pluginLink.GetBaseUrl();

                    this.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        pluginLink.GetApiKey()
                    );
                }
            });

        }

        async public Task<Byte[]> GetIcon(Int32 iconId, Boolean hq = false)
        {
            var response = await this.client.GetAsync($"{this.baseUrl}/icon/{iconId}{(hq ? "?hq" : "")}");
            var result = await response.Content.ReadAsByteArrayAsync();

            return result;
        }

        async public Task<Boolean> RunTextCommand(String command)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new FFXIVTextCommand(command)));
            var response = await this.client.PostAsync($"{this.baseUrl}/command", content);

            return response.IsSuccessStatusCode;
        }

        async public Task<HttpResponseMessage> GetHotbarSlot(Int32 hotbarId, Int32 slotId)
        {
            var response = await this.client.GetAsync($"{this.baseUrl}/hotbar/{hotbarId}/{slotId}");

            return response;
        }

        async public Task<Boolean> TriggerHotbarSlot(Int32 hotbarId, Int32 slotId)
        {
            var response = await this.client.PostAsync($"{this.baseUrl}/hotbar/{hotbarId}/{slotId}/execute", new StringContent(""));

            return response.IsSuccessStatusCode;
        }

        async public Task<FFXIVAction> GetAction(String type, Int32 id)
        {
            var response = await this.client.GetAsync($"{this.baseUrl}/action/{type}/{id}");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<FFXIVAction>(result);
        }

        async public Task<Boolean> ExecuteAction(String type, Int32 id)
        {
            var response = await this.client.PostAsync($"{this.baseUrl}/action/{type}/{id}/execute", new StringContent(""));

            return response.IsSuccessStatusCode;
        }

        async public Task<FFXIVClass> GetClass(Int32 classId)
        {
            var response = await this.client.GetAsync($"{this.baseUrl}/classes/{classId}");
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<FFXIVClass>(result);
        }

        async public Task<Boolean> TriggerClass(Int32 classId)
        {
            var response = await this.client.PostAsync($"{this.baseUrl}/classes/{classId}/execute", new StringContent(""));

            return response.IsSuccessStatusCode;
        }
    }
}
