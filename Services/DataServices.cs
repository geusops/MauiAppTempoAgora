using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppTempoAgora.Services
{
    public class DataServices
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "7c2e049f008f2294ef91d1ebff7b3443";
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    //instanciando um objeto datetime
                    DateTime time = new();

                    //criando 2 objetos datetime populando com os valores sunrise e sunset que vem do retorno da call 
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    //populando o objeto t
                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    };//fecha objeto tempo
                }//fecha if se o status da query for sucesso
            }// fecha laço using
            return t;
        }
    }
}
