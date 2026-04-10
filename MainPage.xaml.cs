using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using Microsoft.UI.Xaml.Controls;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataServices.GetPrevisao(txt_cidade.Text);
                    

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n" +
                                         $"Descrição: {t.description} \n" +
                                         $"Velocidade: {t.speed} \n" +
                                         $"Visibilidade: {t.visibility} \n";

                        lbl_res.Text = dados_previsao;

                        string mapa = $"https://embed.windy.com/embed.html?" + 
                            $"type=map&location=coordinates&metricRain=mm&metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface" + 
                            $"&lat={t.lat.ToString().Replace(",", ".")}&lon={t.lon.ToString().Replace(",", ".")}";

                        wv_mapa.Source = mapa;

                    }
                    else
                    {

                        lbl_res.Text = "Sem dados de Previsão";
                    }

                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            } 
            catch (Exception ex) {
                await DisplayAlert("Ops", ex.Message, "OK");
            }

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try {
                //instanciando uma classe de geolocalizacao
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium,TimeSpan.FromSeconds(10));

                //instanciando uma classe do tipo local
                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                        $"Longitude: {local.Longitude} \n";

                    lbl_coords.Text = local_disp;

                    //chamando o metodo que criamos usando a lat e long que pegamos do dispositivo
                    GetCidade(local.Latitude, local.Longitude);
                }
                else
                {
                    lbl_coords.Text = "Nenhuma Localização";
                }

            } 
            catch (FeatureNotSupportedException fnsEx) {
                await DisplayAlert("Erro: Dispositivo não Supporta", fnsEx.Message,"OK");
                
            } 
            catch (FeatureNotEnabledException fnhEX)
            {
                await DisplayAlert("Erro: Localização desabilitada", fnhEX.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissaão da Localização", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void GetCidade(double lat, double lon)
        {
            try
            {
                //criando uma tarefa do tipo placemark e populando com o resultado do geocoding que, por sua vez recebeu a latitude de longitude
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                //criando um objeto place e populando com o primeiro resultado do array places coletado acima
                Placemark? place = places.FirstOrDefault();
                await DisplayAlert("Aqui foi", place.ToString(), "OK");

                if (place != null)
                {
                    await DisplayAlert("Aqui foi tambem", place.ToString(), "OK");
                    txt_cidade.Text = place.Locality;
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Erro: Obtenção do nome da cidade", ex.Message, "OK");
            }
            
        }
    }
}
